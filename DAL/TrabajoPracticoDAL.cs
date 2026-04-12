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
    Se encarga de manejar la lógica de acceso de datos relacionada con los Trabajos Prácticos y sus Entregas automáticas.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class TrabajoPracticoDAL
    {
        private readonly AccesoDatos access = new AccesoDatos();
        private readonly AlumnoMateriaDAL _alumnoMateriaDAL = new AlumnoMateriaDAL();

        /* Toma los datos de la Trabajo Practico. */
        private const string COLUMNAS = @"
            TP.ID_TP, TP.ID_MATERIA, TP.TITULO, TP.DESCRIPCION, TP.FECHA_LIMITE, TP.ACTIVO,
            M.NOMBRE AS NOMBRE_MATERIA, M.ACTIVO AS ACTIVO_MATERIA";

        /* Se encarga de hacer JOIN de las tablas Entrega, Usuario, TrabajoPractico y Materia */
        private const string JOINS = @"
             FROM TrabajoPractico TP
             INNER JOIN Materia M ON TP.ID_MATERIA = M.ID_MATERIA";

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR TPs - Filtro opcional por materia y/o profesor
        // -----------------------------------------------------------------------------------------------------------------------
        public List<TrabajoPractico> ListarTPs(int idMateria = 0, int idProfesor = 0)
        {
            /* Se instancia una lista de Trabajos Practicos y de Parametros (si se filtra por algun criterio) */
            List<TrabajoPractico> list = new List<TrabajoPractico>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            /* Consulta SQL básica para mostrar los trabajos prácticos. */
            string sql = $"SELECT {COLUMNAS} {JOINS} WHERE 1=1";

            /* Filtros opcionales. Se agregan a la consulta en caso de que se haga una búsqueda más específica. (Por Materia y/o Profesor) */
            if (idMateria > 0)
            {
                sql += " AND TP.ID_MATERIA = @idMateria";
                parameters.Add(new SqlParameter("@idMateria", SqlDbType.Int) { Value = idMateria });
            }

            if (idProfesor > 0)
            {
                sql += " AND TP.ID_MATERIA IN (SELECT ID_MATERIA FROM Profesor_Materia WHERE ID_PROFESOR = @idProfesor)";
                parameters.Add(new SqlParameter("@idProfesor", SqlDbType.Int) { Value = idProfesor });
            }

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters.ToArray());

            /* Por cada fila, se mapea con la entidad Trabajo Practico. */
            foreach (DataRow row in table.Rows)
                list.Add(MapearTP(row));

            /* Se devuelve la lista de TPs. */
            return list;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // OBTENER POR ID - Devuelve un TP específico o null si no existe
        // -----------------------------------------------------------------------------------------------------------------------
        public TrabajoPractico ObtenerPorId(int id)
        {
            /* Consulta SQL para buscar un TP por su ID. */
            string sql = $"SELECT {COLUMNAS} {JOINS} WHERE TP.ID_TP = @id";

            /* Se instancia una lista de parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters);

            /* Si no hubo resultados se devuelve NULL. */
            if (table.Rows.Count == 0)
                return null;

            /* Caso contrario se devuelve el TP mapeado con la entidad TrabajoPractico. */
            return MapearTP(table.Rows[0]);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CREAR TP + GENERAR ENTREGAS - Transacción: INSERT en TrabajoPractico + INSERT en Entrega por cada alumno del profesor
        // -----------------------------------------------------------------------------------------------------------------------
        public int CrearTPConEntregas(TrabajoPractico tp, int idProfesor)
        {

            /* Se instancia un ID con un valor inicial de 0 */
            int idGenerado = 0;

            /* Obtenemos los alumnos por profesor en esa materia ANTES de abrir la transacción. */
            List<AlumnoMateria> alumnos = _alumnoMateriaDAL.ListarAlumnosPorProfesorEnMateria(idProfesor, tp.IdMateria);

            /* Realizamos la Transacción */
            access.ExecuteTransaction((conn, tran) =>
            {
                /* PASO 1: Sentencia INSERT en TrabajoPractico. */
                string sqlTP = @"INSERT INTO TrabajoPractico (ID_MATERIA, TITULO, DESCRIPCION, FECHA_LIMITE, ACTIVO)
                                 VALUES (@idMateria, @titulo, @desc, @fechaLimite, 1);
                                 SELECT SCOPE_IDENTITY();";

                /* Se crea el comando y se le pasa la sentencia, la conexión y la transacción. */
                SqlCommand cmdTP = new SqlCommand(sqlTP, conn, tran);

                /* Se le agregan los parámetros (la información del Trabajo Practico) */
                cmdTP.Parameters.Add(new SqlParameter("@idMateria", SqlDbType.Int) { Value = tp.IdMateria });
                cmdTP.Parameters.Add(new SqlParameter("@titulo", SqlDbType.VarChar) { Value = tp.Titulo });
                cmdTP.Parameters.Add(new SqlParameter("@desc", SqlDbType.VarChar) { Value = tp.Descripcion });
                cmdTP.Parameters.Add(new SqlParameter("@fechaLimite", SqlDbType.DateTime) { Value = tp.FechaLimite });

                /* Se ejecuta la sentencia y genera un ID porque la sentencia tiene SCOPE_IDENTITY() */
                object res = cmdTP.ExecuteScalar();

                /* El ID generado ahora es el numero de ID de TP resultante de la ejecución del INSERT */
                idGenerado = Convert.ToInt32(res);

                /* PASO 2: Sentencia INSERT en Entrega por cada alumno del profesor de esa materia */
                string sqlEntrega = @"INSERT INTO Entrega (ID_ALUMNO, ID_TP, ID_PROFESOR, ESTADO_ENTREGA, ESTADO_CORRECCION)
                                      VALUES (@idAlumno, @idTP, @idProfesor, 'No Entregado', 'PENDIENTE')";

                /* Por cada Alumno inscripto de esa materia del profesor se hace lo siguiente. */
                foreach (AlumnoMateria inscripcion in alumnos)
                {
                    /* Se crea el comando y se le pasa la sentencia, la conexión y la transacción. */
                    SqlCommand cmdEntrega = new SqlCommand(sqlEntrega, conn, tran);

                    /* Se le agregan los parámetros (la información para la entrega )*/
                    cmdEntrega.Parameters.Add(new SqlParameter("@idAlumno", SqlDbType.Int) { Value = inscripcion.IdAlumno });
                    cmdEntrega.Parameters.Add(new SqlParameter("@idTP", SqlDbType.Int) { Value = idGenerado });
                    cmdEntrega.Parameters.Add(new SqlParameter("@idProfesor", SqlDbType.Int) { Value = idProfesor });
                    cmdEntrega.ExecuteNonQuery();
                }
            });

            /* Si todo salió bien, se retorna el ID del TP generado*/
            return idGenerado;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // MODIFICAR TP - Solo permite modificar Titulo, Descripcion y FechaLimite. Devuelve true si se modificó.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool ModificarTP(TrabajoPractico tp)
        {
            /* Sentencia SQL para modificar un TP. */
            string sql = @"UPDATE TrabajoPractico 
                           SET TITULO = @titulo, DESCRIPCION = @desc, FECHA_LIMITE = @fechaLimite
                           WHERE ID_TP = @id";

            /* Se instancia una lista de parámetros necesarios para la modficiación de un TP en específico. */
            SqlParameter[] parameters = {
                new SqlParameter("@titulo",      SqlDbType.VarChar)  { Value = tp.Titulo },
                new SqlParameter("@desc",        SqlDbType.VarChar)  { Value = tp.Descripcion },
                new SqlParameter("@fechaLimite", SqlDbType.DateTime) { Value = tp.FechaLimite },
                new SqlParameter("@id",          SqlDbType.Int)      { Value = tp.Id }
            };

            /* Se ejecuta la sentencia. El resultado (números de filas afectadas) se guarda en una variable. */
            int rowsAffected = access.Write(sql, parameters);

            /* Según el resultado, se devuelve un valor booleano. */
            return rowsAffected > 0;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CAMBIAR ESTADO - Soft Delete / Reactivación. Devuelve true si se modificó al menos una fila.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool CambiarEstadoTP(int id, bool estado)
        {
            /* Sentencia SQL para dar de baja o reactivar un TP. */
            string sql = "UPDATE TrabajoPractico SET ACTIVO = @est WHERE ID_TP = @id";

            /* Se instancia una lista de parametros necesarios para realizar el cambio de estado al TP en específico. */
            SqlParameter[] parameters = {
                new SqlParameter("@est", SqlDbType.Bit) { Value = estado },
                new SqlParameter("@id",  SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la sentencia. El resultado (números de filas afectadas) se guarda en una variable. */
            int rowsAffected = access.Write(sql, parameters);

            return rowsAffected > 0;
        }

        /* ------------------------------------------ [MÉTODOS AUXILIARES] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // MAPEARTP - Mapea todas las filas de resultados de la consulta con la entidad TrabajoPractico
        // -----------------------------------------------------------------------------------------------------------------------
        private TrabajoPractico MapearTP(DataRow row)
        {
            return new TrabajoPractico
            {
                Id = Convert.ToInt32(row["ID_TP"]),
                IdMateria = Convert.ToInt32(row["ID_MATERIA"]),
                Titulo = row["TITULO"].ToString(),
                Descripcion = row["DESCRIPCION"].ToString(),
                FechaLimite = Convert.ToDateTime(row["FECHA_LIMITE"]),
                Activo = Convert.ToBoolean(row["ACTIVO"]),
                Materia = new Materia
                {
                    Id = Convert.ToInt32(row["ID_MATERIA"]),
                    Nombre = row["NOMBRE_MATERIA"].ToString(),
                    Activo = Convert.ToBoolean(row["ACTIVO_MATERIA"])
                }
            };
        }
    }
}
