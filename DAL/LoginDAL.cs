using DAL.Conexion;
using ENTITY;
using System;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de acceso de datos relacionada al Login con respecto a los Usuarios. 
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class LoginDAL
    {
        /* Instancia de la clase AccesoDatos para manejar la conexión a la base de datos y ejecutar consultas SQL. */
        private readonly AccesoDatos access = new AccesoDatos();

        // -----------------------------------------------------------
        // MÉTODO LOGIN.
        // -----------------------------------------------------------
        public Usuario Login(string dni, string password)
        {
            /* Consulta SQL para buscar al usuario que intenta ingresar. */
            string sql = @"SELECT ID_USUARIO, DNI, NOMBRE, APELLIDO, EMAIL, ROL, ACTIVO, LEGAJO, NRO_EMPLEADO
                           FROM Usuario
                           WHERE DNI = @dni AND PASSWORD = @pass AND ACTIVO = 1";

            /* Se crea una lista de parámetros necesarios para realizar la búsqueda del Usaurio. */
            SqlParameter[] parameters = {
                new SqlParameter("@dni", SqlDbType.VarChar) { Value = dni},
                new SqlParameter("@pass", SqlDbType.VarChar) { Value = password }
            };

            /* Se ejecuta la consulta. El resutaldo se guarda en una tabla (DataTable). */
            DataTable table = access.Read(sql, parameters);

            /* Si la tabla no contiene files, se retorna NULL. */
            if (table.Rows.Count == 0)
            {
                return null; 
            }

            /* Se obtiene la primera fila del resultado que corresponde al usuario encontrado. */
            DataRow row = table.Rows[0];

            /* Se obtiene el Rol del usuario encontrado. */
            string rol = row["ROL"].ToString();

            /* Se crea la clase base de Usuario que se transformará en un tipo de Usuario según el Rol. */
            Usuario usuario;

            /* Segun el Rol, se instancia la clase correspondiente (Administrador, Profesor o Alumno). */
            switch (rol)
            {
                case "ADMIN":
                    usuario = new Administrador();
                    break;

                case "PROFESOR":
                    usuario = new Profesor
                    {
                        NroEmpleado = row["NRO_EMPLEADO"] != DBNull.Value ? row["NRO_EMPLEADO"].ToString() : null
                    };
                    break;

                case "ALUMNO":
                    usuario = new Alumno
                    {
                        Legajo = row["LEGAJO"] != DBNull.Value ? row["LEGAJO"].ToString() : null
                    };
                    break;

                default:
                    return null; // Rol desconocido
            }
        
            /* Se cargan los datos comunes. */
            usuario.Id = Convert.ToInt32(row["ID_USUARIO"]);
            usuario.DNI = row["DNI"].ToString();
            usuario.Nombre = row["NOMBRE"].ToString();
            usuario.Apellido = row["APELLIDO"].ToString();
            usuario.Email = row["EMAIL"].ToString();
            usuario.Rol = rol;

            /* Se devuelve el resultado. */
            return usuario;
  
        }
    }
}
