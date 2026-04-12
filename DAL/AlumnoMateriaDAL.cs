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
    Se encarga de manejar la lógica de acceso de datos relacionada con la tabla intermedia Alumno_Materia (Inscripciones).
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class AlumnoMateriaDAL
    {
        /* Instancia de la clase AccesoDatos para manejar la conexión a la base de datos y ejecutar consultas SQL. */
        private readonly AccesoDatos access = new AccesoDatos();

        /* Columnas para mostrar la información completa de la inscripción. */
        private const string COLUMNAS = @"
           AM.ID_ALUMNO, AM.ID_MATERIA,
           M.NOMBRE AS NOMBRE_MATERIA,
           A.NOMBRE AS NOMBRE_ALUMNO, A.APELLIDO AS APELLIDO_ALUMNO,
           A.EMAIL AS EMAIL_ALUMNO, A.NRO_LEGAJO,
           P.NOMBRE AS NOMBRE_PROFESOR, P.APELLIDO AS APELLIDO_PROFESOR,
           P.EMAIL AS EMAIL_PROFESOR, P.NRO_EMPLEADO";

        /* Tablas que se unen para mostrar toda la información de la inscripción. */
        private const string JOINS = @"
            FROM Alumno_Materia AM
            INNER JOIN Materia  M ON AM.ID_MATERIA  = M.ID_MATERIA
            INNER JOIN Usuario  A ON AM.ID_ALUMNO   = A.ID_USUARIO
            INNER JOIN Usuario  P ON AM.ID_PROFESOR = P.ID_USUARIO";

        /* ------------------------------------------ [METODOS BASICOS] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR ALUMNOS POR MATERIA - Devuelve los alumnos inscriptos a una materia en particular.
        // -----------------------------------------------------------------------------------------------------------------------
        public List<AlumnoMateria> ListarAlumnosPorMateria(int id)
        {
            /* Se instancia una lista de Alumnos inscriptos por Materia */
            List<AlumnoMateria> list = new List<AlumnoMateria>();

            /* Consulta SQL para mostrar los alumnos inscriptos por Materia */
            string sql = $"SELECT {COLUMNAS} {JOINS} WHERE AM.ID_MATERIA = @idMateria";

            /* Se instancian los parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@idMateria", SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters);

            /* Por cada fila de la tabla, se mapea con la entidad AlumnoMateria */
            foreach (DataRow row in table.Rows)
                list.Add(MapearInscripcion(row));

            /* Devolvemos la lista de Alumnos inscriptos por Materia */
            return list;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR MATERIAS POR ALUMNO - Devuelve las materias en las que está inscripto un alumno en particular. 
        // -----------------------------------------------------------------------------------------------------------------------
        public List<AlumnoMateria> ListarMateriasPorAlumno(int id)
        {
            /* Se instancia una lista de AlumnoMateria */
            List<AlumnoMateria> list = new List<AlumnoMateria>();

            /* Consulta SQL para mostrar las materias que está inscripto un alumno en particular. */
            string sql = $"SELECT {COLUMNAS} {JOINS} WHERE AM.ID_ALUMNO = @idAlumno";

            /* Se instancian los parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@idAlumno", SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters);

            /* Por cada fila de la tabla, se mapea con la entidad AlumnoMateria */
            foreach (DataRow row in table.Rows)
                list.Add(MapearInscripcion(row));

            /* Devolvemos la lista de Materias por Alumno */
            return list;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR ALUMNOS POR PROFESOR EN MATERIA - Devuelve los alumnos de un profesor específico en una materia
        // -----------------------------------------------------------------------------------------------------------------------
        public List<AlumnoMateria> ListarAlumnosPorProfesorEnMateria(int idProfesor, int idMateria)
        {
            /* Se instancia una lista de AlumnoMateria */
            List<AlumnoMateria> list = new List<AlumnoMateria>();

            /* Consulta SQL para listar los alumnos de un profesor específico en una materia */
            string sql = $"SELECT {COLUMNAS} {JOINS} WHERE AM.ID_MATERIA = @idMateria AND AM.ID_PROFESOR = @idProfesor";

            /* Se crea una lista de parametros necesarios para la consulta. */
            SqlParameter[] parameters = {
                new SqlParameter("@idMateria",  SqlDbType.Int) { Value = idMateria },
                new SqlParameter("@idProfesor", SqlDbType.Int) { Value = idProfesor }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters);

            /* Por cada fila de la tabla, se mapea con la entidad AlumnoMateria. */
            foreach (DataRow row in table.Rows)
                list.Add(MapearInscripcion(row));

            /* Se devuelve la lista. */
            return list;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // INSCRIBIR ALUMNO EN MATERIA 
        // -----------------------------------------------------------------------------------------------------------------------
        public bool InscribirAlumno(int idAlumno, int idMateria, int idProfesor)
        {
            /* Sentencia SQL para inscribir un alumno a una materia */
            string sql = @"INSERT INTO Alumno_Materia (ID_ALUMNO, ID_MATERIA, ID_PROFESOR) 
                           VALUES (@idAlumno, @idMateria, @idProfesor)";

            /* Se instancia una lista de parámetros necesarios para realizar el INSERT. */
            SqlParameter[] parameters = {
                new SqlParameter("@idAlumno",  SqlDbType.Int) { Value = idAlumno },
                new SqlParameter("@idMateria", SqlDbType.Int) { Value = idMateria },
                new SqlParameter("@idProfesor",SqlDbType.Int) { Value = idProfesor }
            };

            /* Devolvemos el resultado booleano de la sentencia ejecutada */
            return access.WriteTablaIntermedia(sql, parameters);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // MODIFICAR PROFESOR DE UNA INSCRIPCION 
        // -----------------------------------------------------------------------------------------------------------------------
        public bool ModificarInscripcion(int idAlumno, int idMateria, int idProfesorNuevo)
        {
            /* Sentencia SQL para actualizar el profesor asignado a un alumno en una materia */
            string sql = @"UPDATE Alumno_Materia 
                           SET ID_PROFESOR = @idProfesorNuevo
                           WHERE ID_ALUMNO = @idAlumno AND ID_MATERIA = @idMateria";

            /* Se instancia una lista de parámetros (los datos necesarios para realizar el UPDATE) */
            SqlParameter[] parameters = {
                new SqlParameter("@idAlumno",       SqlDbType.Int) { Value = idAlumno },
                new SqlParameter("@idMateria",      SqlDbType.Int) { Value = idMateria },
                new SqlParameter("@idProfesorNuevo",SqlDbType.Int) { Value = idProfesorNuevo }
            };

            /* Se ejecuta la sentencia. El resultado (filas afectadas) se guarda en una variable. */
            int rowsAffected = access.Write(sql, parameters);

            /* Segun el resultado de la sentencia, devolvemos un booleano. */
            return rowsAffected > 0;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // DESINSCRIBIR ALUMNO DE MATERIA
        // -----------------------------------------------------------------------------------------------------------------------
        public bool DesinscribirAlumno(int idAlumno, int idMateria)
        {
            /* Sentencia SQL para desinscribir a un Alumno de una Materia */
            string sql = @"DELETE FROM Alumno_Materia 
                           WHERE ID_ALUMNO = @idAlumno AND ID_MATERIA = @idMateria";

            /* Se instancia una lista de parámetros necesarios para realizar el DELETE. */
            SqlParameter[] parameters = {
                new SqlParameter("@idAlumno",  SqlDbType.Int) { Value = idAlumno },
                new SqlParameter("@idMateria", SqlDbType.Int) { Value = idMateria }
            };

            /* Se ejecuta la sentencia. El resultado (número de filas afectadas) se guarda en una variable. */
            int rowsAffected = access.Write(sql, parameters);

            /* Segun el resultado de la sentencia, devolvemos un booleano. */
            return rowsAffected > 0;
        }

        /* ------------------------------------------ [METODOS AUXILIARES] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // MAPEAR ALUMNOMATERIA - Se encarga de mapear los resultados de las consultas a la entidad AlumnoMateria. 
        // -----------------------------------------------------------------------------------------------------------------------
        private AlumnoMateria MapearInscripcion(DataRow row)
        {
            return new AlumnoMateria
            {
                IdAlumno = Convert.ToInt32(row["ID_ALUMNO"]),
                IdMateria = Convert.ToInt32(row["ID_MATERIA"]),

                Materia = new Materia
                {
                    Id = Convert.ToInt32(row["ID_MATERIA"]),
                    Nombre = row["NOMBRE_MATERIA"].ToString(),
                },

                AlumnoInscripto = new Alumno
                {
                    Nombre = row["NOMBRE_ALUMNO"].ToString(),
                    Apellido = row["APELLIDO_ALUMNO"].ToString(),
                    Email = row["EMAIL_ALUMNO"].ToString(),
                    Legajo = row["NRO_LEGAJO"] != DBNull.Value ? row["NRO_LEGAJO"].ToString() : null
                },

                ProfesorAsignado = new Profesor
                {
                    Nombre = row["NOMBRE_PROFESOR"].ToString(),
                    Apellido = row["APELLIDO_PROFESOR"].ToString(),
                    Email = row["EMAIL_PROFESOR"].ToString(),
                    NroEmpleado = row["NRO_EMPLEADO"] != DBNull.Value ? row["NRO_EMPLEADO"].ToString() : null
                }
            };
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // EXISTE INSCRIPCION - Se encarga de verificar si el Alumno ya se encuentra inscripto a la Materia.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool ExisteInscripcion(int idAlumno, int idMateria)
        {
            /* Consulta SQL para verificar si el alumno ya está inscripto en la materia */
            string sql = @"SELECT COUNT(*) FROM Alumno_Materia 
                           WHERE ID_ALUMNO = @idAlumno AND ID_MATERIA = @idMateria";

            /* Se instancia una lista de parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@idAlumno",  SqlDbType.Int) { Value = idAlumno },
                new SqlParameter("@idMateria", SqlDbType.Int) { Value = idMateria }
            };

            /* Se ejecuta la consult. El valor se guarda en una tabla (DataTable) */
            DataTable table = access.Read(sql, parameters);

            /* Dependiendo el resultado de la sentencia, se devuelve el resultado booleano. */
            return Convert.ToInt32(table.Rows[0][0]) > 0;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CONTAR ALUMNOS POR PROFESOR EN MATERIA -  Devuelve la cantidad de alumnos asignados a un profesor en una materia.
        // -----------------------------------------------------------------------------------------------------------------------
        public int ContarAlumnosPorProfesorEnMateria(int idProfesor, int idMateria)
        {
            /* Consulta SQL para verificar la cantidad de Alumnos inscriptos a una materia por profesor */
            string sql = @"SELECT COUNT(*) FROM Alumno_Materia 
                           WHERE ID_MATERIA = @idMateria AND ID_PROFESOR = @idProfesor";

            /* Se instancia una lista de parámetros necesarios para realizar la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@idMateria",  SqlDbType.Int) { Value = idMateria },
                new SqlParameter("@idProfesor", SqlDbType.Int) { Value = idProfesor }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable) */
            DataTable table = access.Read(sql, parameters);

            /* Devuelvo la cantidad */
            return Convert.ToInt32(table.Rows[0][0]);
        }

 
    }
}
