using DAL.Conexion;
using ENTITY;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de acceso de datos relacionada con los Usuarios. 
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class UsuarioDAL
    {
        // Instancia de la clase AccesoDatos para manejar la conexión a la base de datos y ejecutar consultas SQL.
        private readonly AccesoDatos access = new AccesoDatos();

        // -----------------------------------------------------------
        // LISTAR USUARIOS. CON FILTROS DE BÚSQUEDA (Dni, Rol, Estado)
        // -----------------------------------------------------------
        public List<Usuario> ListarUsuarios(string dni, string rol, string estado)
        {

            /* Instanciamos una lista de Usuarios */
            List<Usuario> listUsers = new List<Usuario>();

            /* Instanciamos una lista de parámetros */
            List <SqlParameter> parameters = new List<SqlParameter>();

            /* Instanciamos la consulta de SELECT básica */
            string sql = "SELECT * FROM Usuario WHERE 1=1";

            /* Dependiendo de si la función se le pasó parámetros de búsqueda se hace el filtrado dinámico */
            if (!string.IsNullOrEmpty(dni))
            {
                sql += " AND DNI LIKE @dni";
                parameters.Add(new SqlParameter("@dni", dni + "%"));
            }

            if (rol != "Todos")
            {
                sql += " AND ROL = @rol";
                parameters.Add(new SqlParameter("@rol", rol));
            }

            if (estado == "Activos") sql += " AND ACTIVO = 1";
            else if (estado == "Inactivos") sql += " AND ACTIVO = 0";

            /* Transformamos el resultado de la consulta en una tabla */
            DataTable tableUsers = access.Read(sql, parameters.ToArray());

            /* Por cada registro encontrado se agrega a la lista mapeado */
            foreach (DataRow row in tableUsers.Rows)
            {
                listUsers.Add(MapearUsuario(row));
            }

            return listUsers;
        }

        // -----------------------------------------------------------
        // INSERTAR NUEVO USUARIO
        // -----------------------------------------------------------
        public int InsertarUsuario(Usuario user)
        {
            /* Sentencia SQL para insertar un Usuario nuevo */
            string sql = @"INSERT INTO Usuario (DNI, NOMBRE, APELLIDO, EMAIL, PASSWORD, ROL, LEGAJO, NRO_EMPLEADO, ACTIVO) 
                         VALUES (@dni, @nom, @ape, @email, @pass, @rol, @leg, @nro, 1)";

            /* Ejecutamos la sentencia, nos va a devolver un número de filas afectadas */
            int rowsAffected = access.Write(sql, ObtenerParametros(user));

            /* Devolvemos el número de filas afectadas */
            return rowsAffected;
        }
        // -----------------------------------------------------------
        // MODIFICAR USUARIO EXISTENTE
        // -----------------------------------------------------------
        public int ModificarUsuario(Usuario user)
        {
            /* Sentencia SQL para modificar un Usuario existente */
            string sql = @"UPDATE Usuario SET DNI=@dni, NOMBRE=@nom, APELLIDO=@ape, EMAIL=@email, 
                         PASSWORD=@pass, ROL=@rol, LEGAJO=@leg, NRO_EMPLEADO=@nro 
                         WHERE ID_USUARIO = @id";

            /* Instanciamos una lista de parámetros */
            List<SqlParameter> parameters = new List<SqlParameter>(ObtenerParametros(user));

            /* Agregamos el único parámetro de búsqueda */
            parameters.Add(new SqlParameter("@id", user.Id));

            /* Ejecutamos la sentencia, nos va a devolver un número de filas afectadas */
            int rowsAffected = access.Write(sql, parameters.ToArray());

            /* Devolvemos el número de filas afectadas */
            return rowsAffected;
        }

        // -----------------------------------------------------------
        // CAMBIAR ESTADO DEL USUARIO (BAJA/REACTIVACIÓN)
        // -----------------------------------------------------------
        public int CambiarEstadoUsuario(int id, bool estado)
        {
            /* Sentencia SQL para modificar el estado del Usuario */
            string sql = "UPDATE Usuario SET ACTIVO = @est WHERE ID_USUARIO = @id";

            /* Instanciamos una lista de parámetros para nuestra sentencia */
            SqlParameter[] parameters = {
                new SqlParameter("@est", estado),
                new SqlParameter("@id", id)
            };

            /* Ejecutamos la sentencia, guardamos el numero de filas afectadas */
            int rowsAffected = access.Write(sql, parameters.ToArray());

            /* Devolvemos el numero de filas afectadas */
            return rowsAffected;
        }

        // ---------------------------------------------------------
        // MÉTODOS AUXILIARES
        // ---------------------------------------------------------
        private SqlParameter[] ObtenerParametros(Usuario user)
        {
            /* Dependiendo si el usuario es Alumno o Profesor se pobla un valor u otro siguiendo una nomenclatura */
            object legajoValue;
            if (user is Alumno)
                legajoValue = "ALU" + user.DNI;
            else
                legajoValue = DBNull.Value;

            object nroEmpleadoValue;
            if (user is Profesor)
                nroEmpleadoValue = "EMP" + user.DNI;
            else
                nroEmpleadoValue = DBNull.Value;

            /* Devolvemos la fila de parametros */
            return new SqlParameter[] {
                new SqlParameter("@dni", user.DNI),
                new SqlParameter("@nom", user.Nombre),
                new SqlParameter("@ape", user.Apellido),
                new SqlParameter("@email", user.Email),
                new SqlParameter("@pass", user.Password),
                new SqlParameter("@rol", user.Rol),
                new SqlParameter("@leg", legajoValue),
                new SqlParameter("@nro", nroEmpleadoValue)
            };
        }

        private Usuario MapearUsuario(DataRow row)
        {
            /* Obtenemos el Rol del Usuario */
            string rol = row["ROL"].ToString();

            /* Instanciamos un nuevo Usuario */
            Usuario user;

            /*Dependiendo del rol, instanciamos la clase correspondiente */
            if (rol == "ADMIN") user = new Administrador();
            else if (rol == "PROFESOR") user = new Profesor();
            else user = new Alumno();

            // Asignamos los datos comunes a todos los usuarios
            user.Id = Convert.ToInt32(row["ID_USUARIO"]);
            user.DNI = row["DNI"].ToString();
            user.Nombre = row["NOMBRE"].ToString();
            user.Apellido = row["APELLIDO"].ToString();
            user.Email = row["EMAIL"].ToString();
            user.Rol = rol;
            user.Activo = Convert.ToBoolean(row["ACTIVO"]);

            // Asignamos datos específicos si existen
            if (user is Alumno) ((Alumno)user).Legajo = row["LEGAJO"].ToString();
            if (user is Profesor) ((Profesor)user).NroEmpleado = row["NRO_EMPLEADO"].ToString();

            return user;
        }


    }
}
