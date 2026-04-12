using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace DAL.Conexion
{

    /*------------------------------------------------------------------------------------------------------------------------------
    Esta clase se encarga de manejar la conexión a la base de datos y ejecutar las consultas SQL.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class AccesoDatos
    {
        
        /* Se encarga de leer la cadena de conexión del App. Config */
        private readonly string strConn = ConfigurationManager.ConnectionStrings["GestionConn"].ConnectionString;

        // -----------------------------------------------------------
        // 1. SELECT - Ejecutar consultas en la Base de Datos. 
        // Este método se encarga de ejecutar las consultas de lectura.
        // Devuelve un objeto de tipo DataTable (tabla). 
        // -----------------------------------------------------------
        public DataTable Read(string query, SqlParameter[] parameters = null)
        {

            /* Se instancia la tabla con la que se llenará los resultados de la consulta. */
            DataTable table = new DataTable();

            /* El uso del "using" sirve para abrir y cerrar la conexión automáticamente. */
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                /* Se crea una variable que va a contener la consulta SQL y la cadena de conexión a la DB. */
                SqlCommand command = new SqlCommand(query, conn)
                {
                    CommandType = CommandType.Text
                };

                /* En caso de que existan parámetros de consulta, se agregan a la variable comando. */
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                /* Se crea el "adapter", este objeto se encarga de ejecutar la consulta y llenar la tabla con los resultados. */
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                try
                {
                    /* Se abre la conexión. */
                    conn.Open();

                    /* Se intenta llenar la tabla co los resultados de la búsqueda. */
                    adapter.Fill(table);
                }
                catch (SqlException ex)
                {
                    /* Si hubo algun error, se lanza un mensaje de error. */
                    throw new Exception("Error al leer datos de la base: " + ex.Message);
                }
            }

            /* Retorna la tabla con los resultados. */
            return table;
        }

        // ---------------------------------------------------------------------------------------------------------
        // 2. INSERT - Ejecutar una insercción a la Base de Datos. 
        // Este método se encargará de insertar los registros a las tablas de la Base de Datos. 
        // Retorna el ID generado por la consulta. Esto se debe si la tabla tiene el campo ID marcado como IDENTITY. 
        // ---------------------------------------------------------------------------------------------------------
        public int WriteIdentity(string query, SqlParameter[] parameters)
        {
            /* Se instancia la variable idGenerado con una valor inicial de 0. */
            int newId = 0;

            /* El uso del "using" sirve para abrir y cerrar la conexión automáticamente. */
            using (SqlConnection conn = new SqlConnection(strConn))
            {

                /* Se crea variable que va a contener la sentencia SQL y la cadena de conexión a la DB. */
                SqlCommand command = new SqlCommand(query, conn);

                /* En caso de que existan parámetros de consulta, se agregan a la variable comando. */
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    /* Se abre la conexión. */
                    conn.Open();

                    /* Se ejecuta la sentencia para realizar un INSERT */
                    /* Las sentencias INSERT que tienen al final SELECT SCOPE_IDENTITY() indica que obtenga el ID generado del nuevo registro insertado. */
                    /* El resultado de la sentencia se guarda en una variable. */
                    object res = command.ExecuteScalar();

                    /* Si el resultado de la sentencia es válido (generó un ID) se convierte a entero y se le asigna al variable idGenerado. */
                    if (res != null && res != DBNull.Value)
                    {
                        newId = Convert.ToInt32(res);
                    }
                       
                }
                catch (SqlException ex)
                {
                    /* Caso contrario, se muestra un mensaje de error. */ 
                    throw new Exception("Error en inserción: " + ex.Message);
                }
            }

            /* Se devuelve el ID del nuevo registro insertado a la DB. */
            return newId;
        }

        // ---------------------------------------------------------------------------------
        // 3. UPDATE y DELETE - Ejecutar una modificación o eliminar un registro en la DB.
        // Este método se encargará de modificar registros o eliminarlos de la Base de Datos. 
        // Retorna la cantidad de filas que fueron afectadas por la sentencias. 
        // ----------------------------------------------------------------------------------
        public int Write(string query, SqlParameter[] parameters)
        {

            /* Se instancia la variable que va a contener el número de filas afectadas. Inicializa en cero. */
            int rowsAffected = 0;

            /* El uso del "using" sirve para abrir y cerrar la conexión automáticamente. */
            using (SqlConnection conn = new SqlConnection(strConn))
            {

                /* Se crea variable que va a contener la sentencia SQL y la cadena de conexión a la DB. */
                SqlCommand command = new SqlCommand(query, conn)
                {
                    CommandType = CommandType.Text
                };

                /* En caso de que existan parámetros de consulta, se agregan a la variable comando. */
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    /* Se abre la conexión. */
                    conn.Open();

                    /* Ejecuta la sentencia, el resultado será la cantidad de filas afectadas y se guarda en variable inicial. */
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    /* En caso de error, se devuelve un mensaje de Error. */
                    throw new Exception("Error en actualización/borrado: " + ex.Message);
                }
            }

            /* Se devuelve el número de filas afectadas por la consulta (UPDATE o DELETE) */
            return rowsAffected;
        }

        // ----------------------------------------------------------------------------------------------
        // 4. INSERT - Insertar registros en las tablas Intermedias (ProfesorMateria y AlumnoMateria).
        // Este método se encargará de insertar registros en las tablas intermedias de la Base de Datos. 
        // Retorna la cantidad de filas afectadas por la insercción.
        // -----------------------------------------------------------------------------------------------

        public bool WriteTablaIntermedia(string query, SqlParameter[] parameters)
        {
            /* Reutilizamos la el método Write(). */
            /* Se puede usar también para hacer un INSERT porque en este caso se necesita devolver el número de filas afectadas. */
            /* Siempre me va a devolver un entero porque las validaciones del resultado de ejecución ya está contempladas en el método Write(). */
            int rowsAffected = Write(query, parameters);

            /* Según el resultado de la cantidad de filas afectadas, se devuelve un valor booleano. */
            return rowsAffected > 0;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // 5. EJECUTAR TRANSACCION - Agrupa múltiples operaciones SQL en una sola transacción.
        //    Si alguna operación falla, se hace Rollback de todas. Si todas tienen éxito, se hace Commit.
        //    Se usa cuando dos o más operaciones deben ejecutarse juntas o no debe ejecutarse ninguna.
        //    
        //    Action<SqlConnection, SqlTransaction> es un Wrapper. ExecuteTransaction recibe una función que está escrita
        //    de esta manera. A su vez, Action<> recibe como parámetro la conexión a la DB y la transacción. 
        //    
        //    No devuelve nada. 
        // -----------------------------------------------------------------------------------------------------------------------
        public void ExecuteTransaction(Action<SqlConnection, SqlTransaction> function)
        {
            /* El uso del "using" sirve para abrir y cerrar la conexión automáticamente. */
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                /* Se abre la conexión. */
                conn.Open();

                /* Se inicializa la transacción, es decir, registrá los cambios pero no se confirman todavía. */
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    /* Se ejecuta el bloque SQL que se le pasa (los INSERTs, UPDATEs, etc.) */
                    /* Se le pasa la conexión y la transacción para que la sentencia participe. */
                    function(conn, transaction);

                    /* Si todo salio bien, se confirma la ejecución exitosa. */
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    /* Si algo salió mal, se revierten todos los cambios. */                   
                    transaction.Rollback();

                    /* Se muestra un mensaje de error. */
                    throw new Exception("Transacción revertida: " + ex.Message);
                }
            }
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // 6. EJECUTAR TRANSACCION CON RETORNO - Igual que ExecuteTransaction pero permite devolver un valor.
        //    Se usa cuando se necesita capturar un resultado generado dentro de la transacción (ej: SCOPE_IDENTITY).
        //
        //    T es un "tipo genérico" — Es un "comodín" que representa cualquier tipo de dato.
        //    En lugar de escribir un método para int, otro para string, otro para bool,
        //    se escribe uno solo con T y C# lo resuelve según cómo se use.
        //
        //    Func<SqlConnection, SqlTransaction, T> es otro Wrapper parecido a Action<>.
        //    En este caso ExecuteTransaction<T> recibe una función que toma la cadena de conexión,
        //    la transacción y devuelve un valor de tipo T (a diferencia del Action<> que no devuelve nada.)
        // -----------------------------------------------------------------------------------------------------------------------
        public T ExecuteTransaction<T>(Func<SqlConnection, SqlTransaction, T> function)
        {
            /* El uso del "using" sirve para abrir y cerrar la conexión automáticamente. */
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                /* Se abre la conexión. */
                conn.Open();

                /* Se inicializa la transacción, es decir, registrá los cambios pero no se confirman todavía. */
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    /* Se ejecuta el bloque SQL que se le pasa (los INSERTs, UPDATEs, etc.) y se guarda lo que devuelve. */
                    /* Se le pasa la conexión y la transacción para que la sentencia participe. */
                    T resultado = function(conn, transaction);

                    /* Si todo salio bien, se confirma la ejecución exitosa. */
                    transaction.Commit();

                    /* Se devuelve el valor resultante. */
                    return resultado;
                }
                catch (Exception ex)
                {
                    /* Si algo salió mal, se revierten todos los cambios. */
                    transaction.Rollback();

                    /* Se muestra un mensaje de error. */
                    throw new Exception("Transacción revertida: " + ex.Message);
                }
            }
        }
    }
}
