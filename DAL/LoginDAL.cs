using DAL.Conexion;
using ENTITY;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar las consultas SQL relacionada con los usuarios, como el login.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class LoginDAL
    {

        // Instancia de la clase AccesoDatos para manejar la conexión a la base de datos y ejecutar consultas SQL.
        private readonly AccesoDatos access = new AccesoDatos();

        // Método de Login
        public Usuario Login(string dni, string password)
        {
            // Consulta SQL buscando por DNI y Pass, solo usuarios activos
            string sql = "SELECT * FROM Usuario WHERE DNI = @dni AND PASSWORD = @pass AND ACTIVO = 1";

            // Creamos los parámetros para la consulta SQL.
            SqlParameter[] p = {
                new SqlParameter("@dni", dni),
                new SqlParameter("@pass", password)
            };

            // Ejecutamos la consulta SQL y obtenemos los resultados en un DataTable.
            DataTable dt = access.Read(sql, p);

            // Si la consulta devolvió al menos una fila, analizamos el resultado.
            if (dt.Rows.Count > 0)
            {
                // Obtenemos la primera fila del resultado, que corresponde al usuario encontrado.
                DataRow fila = dt.Rows[0];

                // Leemos el rol del usuario desde la fila de resultados.
                string rol = fila["ROL"].ToString();

                // Creamos una variable de tipo Usuario para almacenar la instancia del usuario que vamos a crear.
                Usuario usuario;

                // Segun el Rol, creamos una instancia de la clase correspondiente (Administrador, Profesor o Alumno).
                switch (rol)
                {
                    case "ADMIN":
                        usuario = new Administrador();
                        break;
                    case "PROFESOR":
                        usuario = new Profesor { NroEmpleado = fila["NRO_EMPLEADO"].ToString() };
                        break;
                    case "ALUMNO":
                        usuario = new Alumno { Legajo = fila["LEGAJO"].ToString() };
                        break;
                    default:
                        return null;
                }

                // Cargamos los datos comunes (de la clase padre Usuario)
                usuario.Id = (int)fila["ID_USUARIO"];
                usuario.DNI = fila["DNI"].ToString();
                usuario.Nombre = fila["NOMBRE"].ToString();
                usuario.Apellido = fila["APELLIDO"].ToString();
                usuario.Email = fila["EMAIL"].ToString();
                usuario.Rol = rol;

                return usuario;
            }
            return null; // Si no encontró nada
        }
    }
}
