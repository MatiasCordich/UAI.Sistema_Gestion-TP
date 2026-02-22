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

        // Leer cadena de conexión
        private readonly string strConn = ConfigurationManager.ConnectionStrings["GestionConn"].ConnectionString;

        // Conexión a la base de datos
        private SqlConnection conn;

        // 1. Método para ejecutar consultas de lectura (SELECT)
        public DataTable Read(string consulta, SqlParameter[] parametros = null)
        {
            DataTable tabla = new DataTable();

            // El 'using' sirve para abrir y cerrar la conexión automáticamente, incluso si ocurre una excepción.
            using (conn = new SqlConnection(strConn))
            {
                // Se crea un comando (consulta) SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(consulta, conn)
                {
                    CommandType = CommandType.Text
                };

                // Si se proporcionan parámetros, se agregan a la consulta.
                if (parametros != null)
                {
                    command.Parameters.AddRange(parametros);
                }

                // El SqlDataAdapter se encarga de ejecutar la consulta y llenar el DataTable con los resultados.
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                try
                {
                    // Intenta llenar el DataTable con los resultados de la consulta.
                    adapter.Fill(tabla);
                }
                catch (SqlException ex)
                {
                    // Caso contrario mostar un mensaje de Error
                    throw new Exception("Error al leer datos de la base: " + ex.Message);
                }
            }
            return tabla;
        }

        // 2. Método para ejecutar comandos de escritura (INSERT, UPDATE, DELETE)
        // Retorna la cantidad de filas afectadas
        public int Write(string consulta, SqlParameter[] parameters)
        {

            // La cantidad de filas afectadas por la consulta
            int rowsAffected = 0;

            // El 'using' sirve para abrir y cerrar la conexión automáticamente, incluso si ocurre una excepción.
            using (conn = new SqlConnection(strConn))
            {

                // Se crea un comando (consulta) SQL con la consulta y la conexión
                SqlCommand command = new SqlCommand(consulta, conn)
                {
                    CommandType = CommandType.Text
                };

                // Si se proporcionan parámetros, se agregan a la consulta.
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                
                try
                {
                    // Intenta abrir la conexión y ejectuar la consulta. 
                    conn.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Caso contrario mostar un mensaje de Error
                    throw new Exception("Error al escribir en la base de datos: " + ex.Message);
                }
            }

            // Devuelve la cantidad de filas afectadas por la consulta (INSERT, UPDATE, DELETE)
            return rowsAffected;
        }
    }
}
