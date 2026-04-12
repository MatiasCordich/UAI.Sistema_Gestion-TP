using DAL.Conexion;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de acceso de datos relacionada con las Entregas.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class EntregaDAL
    {
        private readonly AccesoDatos access = new AccesoDatos();

        /* Columnas explícitas reutilizables en todos los SELECT */

        /* Toma los datos de la Entrega, Alum o, Profesor, Trabajo Practico y Materia*/
        private const string COLUMNAS = @"
            E.ID_ENTREGA, E.ID_ALUMNO, E.ID_TP, E.ID_PROFESOR,
            E.CONTENIDO_LINK, E.ESTADO_ENTREGA, E.ESTADO_CORRECCION,
            E.NOTA, E.DEVOLUCION, E.FECHA_ENTREGA, E.FECHA_CORRECCION,
            A.DNI AS DNI_ALUMNO, A.NOMBRE AS NOMBRE_ALUMNO, A.APELLIDO AS APELLIDO_ALUMNO,
            A.EMAIL AS EMAIL_ALUMNO, A.ROL AS ROL_ALUMNO, A.ACTIVO AS ACTIVO_ALUMNO, A.LEGAJO,
            P.DNI AS DNI_PROFESOR, P.NOMBRE AS NOMBRE_PROFESOR, P.APELLIDO AS APELLIDO_PROFESOR,
            P.EMAIL AS EMAIL_PROFESOR, P.ROL AS ROL_PROFESOR, P.ACTIVO AS ACTIVO_PROFESOR, P.NRO_EMPLEADO,
            TP.TITULO, TP.DESCRIPCION, TP.FECHA_LIMITE, TP.ACTIVO AS ACTIVO_TP,
            M.ID_MATERIA, M.NOMBRE AS NOMBRE_MATERIA";

        /* Se encarga de hacer JOIN de las tablas Entrega, Usuario, TrabajoPractico y Materia */
        private const string JOINS = @"
            FROM Entrega E
            INNER JOIN Usuario A ON E.ID_ALUMNO   = A.ID_USUARIO
            INNER JOIN Usuario P ON E.ID_PROFESOR = P.ID_USUARIO
            INNER JOIN TrabajoPractico TP ON E.ID_TP = TP.ID_TP
            INNER JOIN Materia M ON TP.ID_MATERIA = M.ID_MATERIA";

        /* ------------------------------------------ [MÉTODOS BASICOS] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR ENTREGAS POR TP - Devuelve todas las entregas de un TP específico
        // -----------------------------------------------------------------------------------------------------------------------
        public List<Entrega> ListarEntregasPorTP(int id)
        {
            /* Se instancia una lista de Entregas. */
            List<Entrega> list = new List<Entrega>();

            /* Consulta SQL para seleccionar las entregasr por Trabajo Práctico. */
            string sql = $"SELECT {COLUMNAS} {JOINS} WHERE E.ID_TP = @idTP";

            /* Se instancia la lista de parámetros necesarios para la consulta. */
            SqlParameter[] parameters = {
                new SqlParameter("@idTP", SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la sentencia y el resultado se guarda en una tabla. */
            DataTable table = access.Read(sql, parameters);

            /* Por cada fila de la tabla de resultados, se mapea con el objeto Entrega. */
            foreach (DataRow row in table.Rows)
                list.Add(MapearEntrega(row));

            /* Devolver la lista mapeada. */
            return list;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR ENTREGAS POR ALUMNO - Devuelve todas las entregas de un alumno específico
        // -----------------------------------------------------------------------------------------------------------------------
        public List<Entrega> ListarEntregasPorAlumno(int idAlumno)
        {
            /* Se instancia una lista de Entregas. */
            List<Entrega> list = new List<Entrega>();

            /* Consulta SQL para seleccionar las entregas por Alumno . */
            string sql = $"SELECT {COLUMNAS} {JOINS} WHERE E.ID_ALUMNO = @idAlumno";

            /* Se instancia la lista de parámetros necesarios para la consulta. */
            SqlParameter[] parameters = {
                new SqlParameter("@idAlumno", SqlDbType.Int) { Value = idAlumno }
            };

            /* Se ejecuta la sentencia y el resultado se guarda en una tabla. */
            DataTable table = access.Read(sql, parameters);

            /* Por cada fila de la tabla de resultados, se mapea con el objeto Entrega. */
            foreach (DataRow row in table.Rows)
                list.Add(MapearEntrega(row));

            /* Devolver la lista mapeada. */
            return list;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // OBTENER POR ID - Devuelve una entrega específica o null si no existe
        // -----------------------------------------------------------------------------------------------------------------------
        public Entrega ObtenerPorId(int id)
        {
            /* Consulta para seleccionar una Entrega en específico. */
            string sql = $"SELECT {COLUMNAS} {JOINS} WHERE E.ID_ENTREGA = @id";

            /* Se instancia una lista de parámetros para la consulta. */
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la consulta y el resultado se guarda en una tabla (objeto DataTable)*/
            DataTable table = access.Read(sql, parameters);

            /* Si no hay filas, es porque no hubo resultados. Entonces se devuelve NULL. */
            if (table.Rows.Count == 0)
                return null;

            /* Si hubo resultados, se devuelve el primer resultado ya mapeado con el objeto Entrega. */
            return MapearEntrega(table.Rows[0]);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // ENTREGAR TP - El alumno sube el link y se marca como Entregado. Devuelve true si se modificó.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool EntregarTP(int idEntrega, string contenidoLink)
        {
            /* Sentencia SQL para modificar la información de la Entrega. */
            string sql = @"UPDATE Entrega 
                           SET CONTENIDO_LINK  = @link,
                               ESTADO_ENTREGA  = 'Entregado',
                               FECHA_ENTREGA   = @fechaEntrega
                           WHERE ID_ENTREGA = @id";

            /* Se instancia una lista de parámetros necesarios para la modificación de datos y entrega en específico. */
            SqlParameter[] parameters = {
                new SqlParameter("@link", SqlDbType.VarChar) { Value = contenidoLink },
                new SqlParameter("@fechaEntrega", SqlDbType.DateTime) { Value = DateTime.Now },
                new SqlParameter("@id", SqlDbType.Int) { Value = idEntrega }
            };

            /* Se ejecuta la consulta, nos devuele el numero de filas afectadas. */
            int rowsAffected = access.Write(sql, parameters);

            /* Dependiendo del número de filas afectadas devolvemos un booleano. */
            return rowsAffected  > 0;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CORREGIR ENTREGA - El profesor carga la nota y devolución. Devuelve true si se modificó.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool CorregirEntrega(int idEntrega, int nota, string devolucion)
        {
            /* Sentencia SQL para modificar información de la Entrega. */
            string sql = @"UPDATE Entrega 
                           SET NOTA = @nota,
                               DEVOLUCION = @devolucion,
                               ESTADO_CORRECCION = 'CORREGIDO',
                               FECHA_CORRECCION = @fechaCorreccion
                           WHERE ID_ENTREGA = @id";

            /* Se instancia una lista de parámetros necesarios para la modificación de la Entrega y la Entrega en Específico*/
            SqlParameter[] parameters = {
                new SqlParameter("@nota",SqlDbType.Int) { Value = nota },
                new SqlParameter("@devolucion",SqlDbType.VarChar) { Value = devolucion },
                new SqlParameter("@fechaCorreccion",SqlDbType.DateTime) { Value = DateTime.Now },
                new SqlParameter("@id",SqlDbType.Int) { Value = idEntrega }
            };

            /* Se ejecuta la consulta, nos devuele el numero de filas afectadas. */
            int rowsAffected = access.Write(sql, parameters);

            /* Dependiendo del número de filas afectadas devolvemos un booleano. */
            return rowsAffected > 0;

        }

        /* ------------------------------------------ [MÉTODOS AUXILIARES] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // MAPEAR ENTREGA - Mapea cada fila del resultado de la consulta a la entidad Entrega.
        // -----------------------------------------------------------------------------------------------------------------------
        private Entrega MapearEntrega(DataRow row)
        {
            return new Entrega
            {
                Id = Convert.ToInt32(row["ID_ENTREGA"]),
                IdAlumno = Convert.ToInt32(row["ID_ALUMNO"]),
                IdTP = Convert.ToInt32(row["ID_TP"]),
                IdProfesor = Convert.ToInt32(row["ID_PROFESOR"]),

                ContenidoLink = row["CONTENIDO_LINK"] != DBNull.Value ? row["CONTENIDO_LINK"].ToString() : null,
                EstadoEntrega = row["ESTADO_ENTREGA"].ToString(),
                EstadoCorreccion = row["ESTADO_CORRECCION"].ToString(),

                Nota = row["NOTA"] != DBNull.Value ? (int?)Convert.ToInt32(row["NOTA"]) : null,
                Devolucion = row["DEVOLUCION"] != DBNull.Value ? row["DEVOLUCION"].ToString() : null,

                FechaEntrega = row["FECHA_ENTREGA"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["FECHA_ENTREGA"]) : null,
                FechaCorreccion = row["FECHA_CORRECCION"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["FECHA_CORRECCION"]) : null,

                Alumno = new Alumno
                {
                    Id = Convert.ToInt32(row["ID_ALUMNO"]),
                    DNI = row["DNI_ALUMNO"].ToString(),
                    Nombre = row["NOMBRE_ALUMNO"].ToString(),
                    Apellido = row["APELLIDO_ALUMNO"].ToString(),
                    Email = row["EMAIL_ALUMNO"].ToString(),
                    Rol = row["ROL_ALUMNO"].ToString(),
                    Activo = Convert.ToBoolean(row["ACTIVO_ALUMNO"]),
                    Legajo = row["LEGAJO"] != DBNull.Value ? row["LEGAJO"].ToString() : null
                },

                Profesor = new Profesor
                {
                    Id = Convert.ToInt32(row["ID_PROFESOR"]),
                    DNI = row["DNI_PROFESOR"].ToString(),
                    Nombre = row["NOMBRE_PROFESOR"].ToString(),
                    Apellido = row["APELLIDO_PROFESOR"].ToString(),
                    Email = row["EMAIL_PROFESOR"].ToString(),
                    Rol = row["ROL_PROFESOR"].ToString(),
                    Activo = Convert.ToBoolean(row["ACTIVO_PROFESOR"]),
                    NroEmpleado = row["NRO_EMPLEADO"] != DBNull.Value ? row["NRO_EMPLEADO"].ToString() : null
                },

                TrabajoPractico = new TrabajoPractico
                {
                    Id = Convert.ToInt32(row["ID_TP"]),
                    IdMateria = Convert.ToInt32(row["ID_MATERIA"]),
                    Titulo = row["TITULO"].ToString(),
                    Descripcion = row["DESCRIPCION"].ToString(),
                    FechaLimite = Convert.ToDateTime(row["FECHA_LIMITE"]),
                    Activo = Convert.ToBoolean(row["ACTIVO_TP"]),
                    Materia = new Materia
                    {
                        Id = Convert.ToInt32(row["ID_MATERIA"]),
                        Nombre = row["NOMBRE_MATERIA"].ToString(),
                        Activo = Convert.ToBoolean(row["ACTIVO_MATERIA"])
                    }
                }
            };
        }
    }
}
