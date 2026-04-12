using DAL.Conexion;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de acceso de datos relacionada con los Usuarios. 
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class UsuarioDAL
    {
        /* Instancia de la clase AccesoDatos para manejar la conexión a la base de datos y ejecutar consultas SQL. */
        private readonly AccesoDatos access = new AccesoDatos();

        /* Toma los datos de la Usuario. */
        private const string COLUMNS= @"ID_USUARIO, DNI, NOMBRE, APELLIDO, EMAIL, ROL, ACTIVO, LEGAJO, NRO_EMPLEADO";

        /* ------------------------------------------ [MÉTODOS BASICOS] ------------------------------------------ */

        // -----------------------------------------------------------
        // LISTAR USUARIOS. CON FILTROS DE BÚSQUEDA (Dni, Rol, Estado)
        // -----------------------------------------------------------
        public List<Usuario> ListarUsuarios(string dni, string rol, string estado)
        {

            /* Se instancia la lista de Usuarios */
            List<Usuario> list = new List<Usuario>();

            /* Se instancia la lista de parámetros */
            List <SqlParameter> parameters = new List<SqlParameter>();

            /* Se instancia la consulta de SELECT básica */
            string sql = $"SELECT {COLUMNS} FROM Usuario WHERE 1=1";

            /* Si a la función se le pasó parámetros de búsqueda se hace el filtrado dinámico. */
            if (!string.IsNullOrEmpty(dni))
            {
                sql += " AND DNI LIKE @dni";
                parameters.Add(new SqlParameter("@dni", SqlDbType.VarChar) { Value = dni + "%" });
            }

            if (!string.IsNullOrWhiteSpace(rol) && rol != "Todos")
            {
                sql += " AND ROL = @rol";
                parameters.Add(new SqlParameter("@rol", SqlDbType.VarChar) { Value = rol });
            }

            if (estado == "Activos")
            {
                sql += " AND ACTIVO = 1";
            }
            else if (estado == "Inactivos")
            {
                sql += " AND ACTIVO = 0";
            }

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters.ToArray());

            /* Por cada fila de la tabla, se mapea con la entidad Usuario. */
            foreach (DataRow row in table.Rows)
            {
                list.Add(MapearUsuario(row));
            }

            /* Se devuelve la lista de usuarios. */
            return list;
        }

        // -----------------------------------------------------------
        // OBTENER USUARIO POR ID
        // -----------------------------------------------------------
        public Usuario ObtenerPorId(int id)
        {
            /* Se instancia la consulta SQL para seleccionar un usuario por su ID. */
            string sql = $"SELECT {COLUMNS} FROM Usuario WHERE ID_USUARIO = @id";

            /* Se instancia una lista de parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters);

            /* Si la tabla tuvo cero resultados, se retorna NULL. */
            if (table.Rows.Count == 0)
                return null;

            /* Devolver el Usuario encontrado ya mapeado. */
            return MapearUsuario(table.Rows[0]);
        }

        // -----------------------------------------------------------
        // INSERTAR NUEVO USUARIO
        // -----------------------------------------------------------
        public int InsertarUsuario(Usuario user)
        {
            /* Sentencia SQL para insertar un Usuario nuevo */
            string sql = @"INSERT INTO Usuario (DNI, NOMBRE, APELLIDO, EMAIL, PASSWORD, ROL, LEGAJO, NRO_EMPLEADO, ACTIVO) 
                         VALUES (@dni, @nom, @ape, @email, @pass, @rol, @leg, @nro, 1);
                         SELECT SCOPE_IDENTITY();";

            /* Se ejecuta la sentencia. Se guarda el ID generado en la variable. */
            int idGenerated = access.WriteIdentity(sql, ObtenerParametros(user));

            /* Se devuelve el ID generado */
            return idGenerated;
        }
        // -----------------------------------------------------------
        // MODIFICAR USUARIO EXISTENTE
        // -----------------------------------------------------------
        public bool ModificarUsuario(Usuario user)
        {
            /* Sentencia SQL para modificar un usuario */
            string sql = @"UPDATE Usuario 
                           SET DNI = @dni, NOMBRE = @nom, APELLIDO = @ape, EMAIL = @email, 
                               PASSWORD = @pass, ROL = @rol, LEGAJO = @leg, NRO_EMPLEADO = @nro 
                           WHERE ID_USUARIO = @id";

            /* Se instancia la lista de parámetros. Se llama a la función GetParameters() */
            List<SqlParameter> parameters = new List<SqlParameter>(ObtenerParametros(user));

            /* Se agrega a la lista los parámetros la ID de búsqueda. */
            parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = user.Id });

            /* Se ejecuta la sentencia. El resultado (número de filas afectadas) se guarda en una variable. */
            int rowsAffected = access.Write(sql, parameters.ToArray());

            /* Según el valor, retorna un booleano. */
            return rowsAffected > 0;
        }

        // -----------------------------------------------------------
        // CAMBIAR ESTADO DEL USUARIO (BAJA/REACTIVACIÓN)
        // -----------------------------------------------------------
        public bool CambiarEstadoUsuario(int id, bool estado)
        {
            /* Sentencia SQL para modificar el estado del Usuario */
            string sql = "UPDATE Usuario SET ACTIVO = @est WHERE ID_USUARIO = @id";

            /*  Se instancia una lista de parámetros y se agregan los parámetros necesarios para ejecutar la sentencia. */
            SqlParameter[] parameters = {
                new SqlParameter("@est", SqlDbType.Bit) { Value = estado },
                new SqlParameter("@id",  SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la sentencia. El resultado (número de filas afectadas) se guarda en una variable. */
            int rowsAffected = access.Write(sql, parameters.ToArray());

            /* Según el valor, retorna un booleano. */
            return rowsAffected > 0;
        }

        /* ------------------------------------------ [METODOS AUXILIARES] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // OBTENER PARÁMETROS - Realiza la lógica del número de Legajo o número de Empleado y lista todos los datos necesarios para 
        // hacer un INSERT o UPDATE. 
        // Devuelve la lista de parámetros. 
        // -------------------------------------------------------------------------------------------------------------------------
        private SqlParameter[] ObtenerParametros(Usuario user)
        {
            /* Si es Alumno -> NroLegajo. */
            object nroLegajo;
            if (user is Alumno)
                nroLegajo = "ALU" + user.DNI;
            else
                nroLegajo = DBNull.Value;

            /* Si es Profesor -> NroLegajo. */
            object nroEmpleado;
            if (user is Profesor)
                nroEmpleado = "EMP" + user.DNI;
            else
                nroEmpleado = DBNull.Value;

            /* Se crea una lista de parámetros para la creación o modificación de datos */
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@dni",  SqlDbType.VarChar) { Value = user.DNI      },
                new SqlParameter("@nom",  SqlDbType.VarChar) { Value = user.Nombre   },
                new SqlParameter("@ape",  SqlDbType.VarChar) { Value = user.Apellido },
                new SqlParameter("@email",SqlDbType.VarChar) { Value = user.Email    },
                new SqlParameter("@pass", SqlDbType.VarChar) { Value = user.Password },
                new SqlParameter("@rol",  SqlDbType.VarChar) { Value = user.Rol      },
                new SqlParameter("@leg",  SqlDbType.VarChar) { Value = nroLegajo },
                new SqlParameter("@nro",  SqlDbType.VarChar) { Value = nroEmpleado }
            };

            /* Se devuelve la lista de parámetros */
            return parameters.ToArray();

        }

        // -----------------------------------------------------------------------------------------------------------------------
        // MAPEAR USUARIO - Recibe las filas de la tabla (DataRow) y la transforma a la entidad Usuario.
        // Devuelve el Usuario ya mapeado. 
        // -------------------------------------------------------------------------------------------------------------------------
        private Usuario MapearUsuario(DataRow row)
        {

            /* Se obtiene el Rol del Usuario */
            string rol = row["ROL"].ToString();

            /* Se instancia un nuevo Usuario */
            Usuario user;

            /* Dependiendo del rol, se instancia la clase correspondiente */
            switch (rol)
            {
                case "ADMIN":
                    user = new Administrador();
                    break;
                case "PROFESOR":
                    user = new Profesor
                    {
                        NroEmpleado = row["NRO_EMPLEADO"] != DBNull.Value ? row["NRO_EMPLEADO"].ToString() : null
                    };
                    break;
                case "ALUMNO":
                    user = new Alumno
                    {
                        Legajo = row["LEGAJO"] != DBNull.Value ? row["LEGAJO"].ToString() : null
                    };
                    break;
                default:
                    throw new Exception($"Rol desconocido: {rol}");
            }

            /* Se le asignan los datos comunes a todos los usuarios */
            user.Id = Convert.ToInt32(row["ID_USUARIO"]);
            user.DNI = row["DNI"].ToString();
            user.Nombre = row["NOMBRE"].ToString();
            user.Apellido = row["APELLIDO"].ToString();
            user.Email = row["EMAIL"].ToString();
            user.Rol = rol; 
            user.Activo = Convert.ToBoolean(row["ACTIVO"]);

            /* Se deuvele el Usuario ya mapeado */
            return user;
        }


        // -----------------------------------------------------------------------------------------------------------------------
        // OBTENER CONTRASEÑA ACTUAL - Devuelve la contraseña hasheada actual de un usuario. Se usa al modificar
        // para no pisar la contraseña si el usuario no la cambió.
        // -----------------------------------------------------------------------------------------------------------------------
        public string ObtenerPasswordActual(int id)
        {
            /* Consulta SQL para obtener solo la contraseña del usuario */
            string sql = "SELECT PASSWORD FROM Usuario WHERE ID_USUARIO = @id";

            /* Se instancia una lista de parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.Int) { Value = id }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla. */
            DataTable table = access.Read(sql, parameters);

            /* Si no hay resultados, se devuelve null. */
            if (table.Rows.Count == 0)
                return null;

            /* Se devuelve la contraseña hasheada actual. */
            return table.Rows[0]["PASSWORD"].ToString();
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // EXISTE DNI - Verifica si ya existe un usuario con ese DNI (excluyendo al propio usuario al editar).
        // Devuelve un booleano. 
        // -----------------------------------------------------------------------------------------------------------------------
        public bool ExisteDNI(string dni, int idExcluir = 0)
        {
            /* Trae todos los usuarios para validar unicidad según DNI */
            string sql = "SELECT COUNT(*) FROM Usuario WHERE DNI = @dni AND ID_USUARIO != @id";

            /*  Se instancia una lista de parámetros y se agregan los parámetros necesarios para la búsqueda */
            SqlParameter[] parameters = {
                new SqlParameter("@dni", SqlDbType.VarChar) { Value = dni },
                new SqlParameter("@id",  SqlDbType.Int)     { Value = idExcluir }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters);

            /* Se devuelve un booleano dependiendo si la tabla tiene o no registros encontrados */
            return Convert.ToInt32(table.Rows[0][0]) > 0;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // EXISTE EMAIL - Verifica si ya existe un usuario con ese Email (excluyendo al propio usuario al editar).
        // Devuelve un booleano. 
        // -----------------------------------------------------------------------------------------------------------------------
        public bool ExisteEmail(string email, int idExcluir = 0)
        {
            /* Consulta SQL para buscar a todos los usuarios que tengan un EMAIL (excluyendo al propio usuario al editar) */
            string sql = "SELECT COUNT(*) FROM Usuario WHERE EMAIL = @email AND ID_USUARIO != @id";

            /*  Se instancia una lista de parámetros y se agregan los parámetros necesarios para la búsqueda. */
            SqlParameter[] parameters = {
                new SqlParameter("@email", SqlDbType.VarChar) { Value = email },
                new SqlParameter("@id",    SqlDbType.Int)     { Value = idExcluir }
            };

            /* Se ejecuta la consulta. El resultado se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters);

            /* Se devuelve un booleano dependiendo si la tabla tiene o no registros encontrados. */
            return Convert.ToInt32(table.Rows[0][0]) > 0;
        }

    }
}
