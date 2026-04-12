using DAL.Conexion;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de acceso de datos relacionada con las Materias.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class MateriaDAL
    {
        /* Instancia de la clase AccesoDatos para manejar la conexión a la base de datos y ejecutar consultas SQL. */
        private readonly AccesoDatos access = new AccesoDatos();

        /* ------------------------------------------ [MÉTODOS BASICOS] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR MATERIAS - Filtro opcional por estado (Activas/Inactivas/Todas)
        // -----------------------------------------------------------------------------------------------------------------------
        public List<Materia> ListarMaterias(string estado = "Todas")
        {
            /* Se instancia una lista de Materias. */
            List<Materia> list = new List<Materia>();

            /* Consulta SQL para seleccionar todas las materias. */
            string sql = "SELECT ID_MATERIA, NOMBRE, ACTIVO FROM Materia WHERE 1=1";

            /* Si se agrega un valor al filtro de estado, se agrega a la consulta */
            if (estado == "Activas") 
                sql += " AND ACTIVO = 1";
            else if (estado == "Inactivas") 
                sql += " AND ACTIVO = 0";

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable) */
            DataTable table = access.Read(sql);

            /* Por cada fila de la tabla se mapea con la entidad Materia. */
            foreach (DataRow row in table.Rows)
                list.Add(MapearMateria(row));

            /* Se devuelve la lista de materias */
            return list;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // OBTENER POR ID - Devuelve una Materia específica o null si no existe
        // -----------------------------------------------------------------------------------------------------------------------
        public Materia ObtenerPorId(int id)
        {
            /* Consulta SQL para seleccionar una materia por su ID */
            string sql = "SELECT ID_MATERIA, NOMBRE, ACTIVO FROM Materia WHERE ID_MATERIA = @id";

            /* Se instancia una lista de parámetros, se agregan los parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable) */
            DataTable tabla = access.Read(sql, parameters);

            /* Si el resultado de la tabla da 0 registros, se devuelve NULL. */
            if (tabla.Rows.Count == 0)
                return null;

            /* Si hay resultado, se devuelve mapeado con la entidad Materia. */
            return MapearMateria(tabla.Rows[0]);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // INSERTAR MATERIA - Devuelve el ID generado.
        // -----------------------------------------------------------------------------------------------------------------------
        public int InsertarMateria(Materia materia)
        {
            /* Sentencia SQL para insertar una materia. */
            string sql = @"INSERT INTO Materia (NOMBRE, ACTIVO) 
                           VALUES (@nom, 1);
                           SELECT SCOPE_IDENTITY();";

            /* Se crea una lista de parámetros necesarios para realizar la insercción. */
            SqlParameter[] parametros = {
                new SqlParameter("@nom", SqlDbType.VarChar) { Value = materia.Nombre }
            };

            /* Se ejecuta la sentencia. El resultado será el nuevo ID de la materia creada. */
            int newId = access.WriteIdentity(sql, parametros);

            /* Se devuelve el ID de la materia creada. */
            return newId;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // MODIFICAR MATERIA - Devuelve true si se modificó al menos una fila.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool ModificarMateria(Materia materia)
        {
            /* Sentencia SQL para modificar una materia. */
            string sql = @"UPDATE Materia 
                           SET NOMBRE = @nom 
                           WHERE ID_MATERIA = @id";

            /* Se instancia una lista de parámetros necesarios para realizar la modificación de la materia en particular. */
            SqlParameter[] parametros = {
                new SqlParameter("@nom", SqlDbType.VarChar) { Value = materia.Nombre },
                new SqlParameter("@id",  SqlDbType.Int)     { Value = materia.Id }
            };

            /* Se ejecuta la sentencia. El valor de filas afectadas se guarda en una variable. */
            int rowsAffected = access.Write(sql, parametros);

            /* Se devuelve un booleano en base a la cantidad de filas afectadas. */
            return rowsAffected > 0;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CAMBIAR ESTADO - Soft Delete / Reactivación. Devuelve true si se modificó al menos una fila.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool CambiarEstadoMateria(int id, bool estado)
        {
            /* Consulta SQL para cambiar el estado de la Materia. */
            string sql = "UPDATE Materia SET ACTIVO = @est WHERE ID_MATERIA = @id";

            /* Se instancia una lista de parámetros para cambiar el estado de la materia en específico. */
            SqlParameter[] parametros = {
                new SqlParameter("@est", SqlDbType.Bit) { Value = estado },
                new SqlParameter("@id",  SqlDbType.Int) { Value = id }
            };

            /* Se devuelve un booleano en base a la cantidad de filas afectadas. */
            return access.Write(sql, parametros) > 0;
        }

        /* ------------------------------------------ [MÉTODOS AUXILIARES] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // MAPEAR MATERIA - Devuelve los resultados de la consulta mapeados con la entidad Materia.
        // -----------------------------------------------------------------------------------------------------------------------
        private Materia MapearMateria(DataRow row)
        {

            /* Devuelve la materia con sus datos ya mapeados para visualizar */
            return new Materia
            {
                Id = Convert.ToInt32(row["ID_MATERIA"]),
                Nombre = row["NOMBRE"].ToString(),
                Activo = Convert.ToBoolean(row["ACTIVO"])
            };
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // EXISTE NOMBRE - Devuelve true si ya existe una materia con ese nombre (excluyendo la propia al editar)
        // -----------------------------------------------------------------------------------------------------------------------
        public bool ExisteNombre(string nombre, int idExcluir = 0)
        {
            /* Consulta para traer todas las materias que tengan ese nombre (exluyendo la propia) */
            string sql = "SELECT COUNT(*) FROM Materia WHERE NOMBRE = @nom AND ID_MATERIA != @id";

            /* Se instancia una lista de parámetros necesarios para realizar la búsqueda. */
            SqlParameter[] parametros = {
                new SqlParameter("@nom", SqlDbType.VarChar) { Value = nombre },
                new SqlParameter("@id",  SqlDbType.Int)     { Value = idExcluir }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable) */
            DataTable tabla = access.Read(sql, parametros);

            /* Devuelve un booleano dependiendo si la tabla tiene o no registros en base al resultado de búsqueda. */
            return Convert.ToInt32(tabla.Rows[0][0]) > 0;
        }

        
       

    }
}
