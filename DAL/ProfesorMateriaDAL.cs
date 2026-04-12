using DAL.Conexion;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de acceso de datos relacionada con la tabla intermedia Profesor_Materia.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class ProfesorMateriaDAL
    {
        /* Instancia de la clase AccesoDatos para manejar la conexión a la base de datos y ejecutar consultas SQL. */
        private readonly AccesoDatos access = new AccesoDatos();

        /* Columnas para mostrar la información completa del profesor. */
        private const string COLUMNAS = @"
            U.ID_USUARIO, U.NRO_EMPLEADO, U.DNI,
            U.NOMBRE, U.APELLIDO, U.EMAIL, U.ACTIVO,
            M.ID_MATERIA, M.NOMBRE AS NOMBRE_MATERIA";

        /* Tablas que se unen para obtener los profesores asignados a una materia. */
        private const string JOINS = @"
            FROM Usuario U
            INNER JOIN Profesor_Materia PM ON U.ID_USUARIO = PM.ID_PROFESOR
            INNER JOIN Materia M ON PM.ID_MATERIA = M.ID_MATERIA";

        /* ------------------------------------------ [METODOS BASICOS] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR PROFESORES POR MATERIA - Devuelve los profesores asignados a una materia
        // -----------------------------------------------------------------------------------------------------------------------
        public List<ProfesorMateria> ListarProfesoresPorMateria(int idMateria)
        {
            /* Se instancia una lista de Profesores asignados a una materia */
            List<ProfesorMateria> list = new List<ProfesorMateria>();

            /* Consulta SQL para buscar a los profesores asignados a una materia. */
            string sql = $"SELECT {COLUMNAS} {JOINS} WHERE PM.ID_MATERIA = @idMateria";

            /* Se instancia una lista de parametros necesarios para realizar la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@idMateria", SqlDbType.Int) { Value = idMateria }
            };

            /* Se instancia una tabla para guardar los resultados de la búsqueda */
            DataTable table = access.Read(sql, parameters);

            /* Por cada fila de la tabla, se mapea con la entidad Profesor */
            foreach (DataRow row in table.Rows)
                list.Add(MapearAsignacion(row));

            /* Se devuelve la fila. */
            return list;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // ASIGNAR PROFESOR A MATERIA - Devuelve true si se insertó correctamente.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool AsignarProfesor(int idProfesor, int idMateria)
        {
            /* Sentencia SQL asignar un profesor a una materia */
            string sql = @"INSERT INTO Profesor_Materia (ID_PROFESOR, ID_MATERIA) 
                           VALUES (@idProfesor, @idMateria)";

            /* Se instancia una lista de parámetros necesarios para realizar la asignación. */
            SqlParameter[] parameters = {
                new SqlParameter("@idProfesor", SqlDbType.Int) { Value = idProfesor },
                new SqlParameter("@idMateria",  SqlDbType.Int) { Value = idMateria }
            };

            /* Retorna un booleano si la operación se realizó o no */
            /*  Nota: usa WriteTablaIntermedia porque opera sobre la tabla intermedia Profesor_Materia. */
            return access.WriteTablaIntermedia(sql, parameters);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // QUITAR PROFESOR DE UNA MATERIA - Devuelve true si se eliminó correctamente.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool QuitarProfesor(int idProfesor, int idMateria)
        {
            /* Sentencia SQL para quitar un profesor de una materia */
            string sql = @"DELETE FROM Profesor_Materia 
                           WHERE ID_PROFESOR = @idProfesor AND ID_MATERIA = @idMateria";

            /* Se instancia una lista de parámetros necesarios para realizar el DELETE de una asignación. */
            SqlParameter[] parameters = {
                new SqlParameter("@idProfesor", SqlDbType.Int) { Value = idProfesor },
                new SqlParameter("@idMateria",  SqlDbType.Int) { Value = idMateria }
            };

            /* Se ejecuta la sentencia. El valor (número de filas afectadas) se guarda en una variable. */
            /* Nota: usa Write (no WriteTablaIntermedia) porque el DELETE sí devuelve filas afectadas. */
            int rowsAffected = access.Write(sql, parameters);

            /* Según el valor, se devuelve un booleano. */
            return  rowsAffected > 0;
        }

        /* ------------------------------------------ [METODOS AUXILIARES] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // MAPEAR PROFESORMATERIA (ASIGNACIÓN) - Mapea las filas de los resultados de la consulta con la entidad ProfesorMateria
        // -----------------------------------------------------------------------------------------------------------------------
        private ProfesorMateria MapearAsignacion(DataRow row)
        {
            return new ProfesorMateria
            {
                IdProfesor = Convert.ToInt32(row["ID_USUARIO"]),
                IdMateria = Convert.ToInt32(row["ID_MATERIA"]),

                Profesor = new Profesor
                {
                    Id = Convert.ToInt32(row["ID_USUARIO"]),
                    DNI = row["DNI"].ToString(),
                    Nombre = row["NOMBRE"].ToString(),
                    Apellido = row["APELLIDO"].ToString(),
                    Email = row["EMAIL"].ToString(),
                    Activo = Convert.ToBoolean(row["ACTIVO"]),
                    NroEmpleado = row["NRO_EMPLEADO"] != DBNull.Value ? row["NRO_EMPLEADO"].ToString() : null,
                },

                Materia = new Materia
                {
                    Id = Convert.ToInt32(row["ID_MATERIA"]),
                    Nombre = row["NOMBRE_MATERIA"].ToString(),
                }
            };
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // EXISTE ASIGNACION - Verifica si el profesor ya está asignado a la materia.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool ExisteAsignacion(int idProfesor, int idMateria)
        {
            /* Consulta SQL para verificar si ya hay una asignación de un profesor a una materia */
            string sql = @"SELECT COUNT(*) FROM Profesor_Materia 
                           WHERE ID_PROFESOR = @idProfesor AND ID_MATERIA = @idMateria";

            /* Se instancia una lista de parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@idProfesor", SqlDbType.Int) { Value = idProfesor },
                new SqlParameter("@idMateria",  SqlDbType.Int) { Value = idMateria }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla. */
            DataTable table = access.Read(sql, parameters);

            /* Devuelve un booleano dependiendo de la cantidad de resultados de la búsqueda. */
            return Convert.ToInt32(table.Rows[0][0]) > 0;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CONTAR PROFESORES POR MATERIA - Verificar la cantidad de profesores asignados a una materia. 
        // -----------------------------------------------------------------------------------------------------------------------
        public int ContarProfesoresPorMateria(int idMateria)
        {
            /* Consulta SQL que devuelve la cantidad de profesores asignados a una materia  */
            string sql = "SELECT COUNT(*) FROM Profesor_Materia WHERE ID_MATERIA = @idMateria";

            /* Se instancia una lista de parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@idMateria", SqlDbType.Int) { Value = idMateria }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla. */
            DataTable table = access.Read(sql, parameters);

            /* Se devuelve la cantidad de resultados. */
            return Convert.ToInt32(table.Rows[0][0]);
        }
    }
}
