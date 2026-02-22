using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de negocio del Usuario, como validar los datos de entrada y manejar las excepciones.  
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class UsuarioBLL
    {
        private readonly UsuarioDAL _usuarioDAL = new UsuarioDAL();


        // 1. LISTAR USUARIOS
        public List<Usuario> ListarUsuarios(string dni = "", string rol = "Todos", string estado = "Todos")
        {
            return _usuarioDAL.ListarUsuarios(dni, rol, estado);
        }

        // 2. GUARDAR (ALTA Y MODIFICACIÓN)
        public int GuardarUsuario(Usuario usuario) {

            /* Regla de negocio 1: Campos obligatorios */
            if (string.IsNullOrWhiteSpace(usuario.DNI)) throw new Exception("El DNI es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Nombre)) throw new Exception("El Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Apellido)) throw new Exception("El Apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Email)) throw new Exception("El Email es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Password)) throw new Exception("La contraseña es obligatoria.");
            if (string.IsNullOrWhiteSpace(usuario.Rol)) throw new Exception("El Rol es obligatorio.");

            /* Regla de negocio 2: DNI debe tener entre 6 y 8 caracteres */
            if (usuario.DNI.Length < 6 || usuario.DNI.Length > 8)
                throw new Exception("El DNI debe tener entre 6 y 8 caracteres. Intente otra vez.");

            /* Regla de negocio 3: Contraseña (longitud mínima y alfanumérica) */
            if (usuario.Password.Length < 6)
                throw new Exception("Error: La contraseña debe tener al menos 6 caracteres. Intente otra vez.");

            bool tieneLetras = usuario.Password.Any(char.IsLetter);
            bool tieneNumeros = usuario.Password.Any(char.IsNumber);

            if (!tieneLetras || !tieneNumeros)
                throw new Exception("La contraseña debe ser alfanumérica (contener letras y números). Intente otra vez.");

            /* Regla de negocio 4: Email y DNI únicos */
            /* Traemos todos los usuarios para comparar */
            List<Usuario> existentes = _usuarioDAL.ListarUsuarios("", "Todos", "Todos");

            /* Validamos si ya existe un usuario con ese DNI (excluyendo al usuario mismo si estamos editando) */
            if (existentes.Any(u => u.DNI == usuario.DNI && u.Id != usuario.Id))
                throw new Exception("Error: Ya existe un usuario registrado con ese DNI. Intente otra vez.");

            /* Validamos si ya está en uso el Email */
            if (existentes.Any(u => u.Email.ToLower() == usuario.Email.ToLower() && u.Id != usuario.Id))
                throw new Exception("Error: El Email ya está siendo utilizado por otro usuario. Intente otra vez.");

            /* Procedemos a hacer la ejecución dependiendo si es una Alta o una Modificación */
            int resultado;

            /* Si el ID de usuario es 0 entonces estamos ante una alta */
            if (usuario.Id == 0)
            {
                resultado = _usuarioDAL.InsertarUsuario(usuario);
            }
            else
            {
                resultado = _usuarioDAL.ModificarUsuario(usuario);
            }

            /* Si el resultado es 0 (0 filas afectadas) lanzar una excepción */
            if (resultado == 0)
            {
                throw new Exception("Error: No se pudo realizar la insercción o modificación de datos.");
            }

            /* Devolver el resultado */
            return resultado;
        }

        // 3. CAMBIAR ESTADO (BAJA O RECTIVACIÓN)
        public int CambiarEstado(int idUsuarioObjetivo, bool nuevoEstado, int idUsuarioLogueado)
        {
            // REGLA DE NEGOCIO 1: Evita darse de baja así mismo
            if (nuevoEstado == false && idUsuarioObjetivo == idUsuarioLogueado)
            {
                throw new Exception("Seguridad: No puedes darte de baja a ti mismo.");
            }

            /* Procedemos a hacer el Cambio de Estado */
            int resultado = _usuarioDAL.CambiarEstadoUsuario(idUsuarioObjetivo, nuevoEstado);

            /* Si el resultado es 0 (0 filas afectadas) lanzar una excepción */
            if (resultado == 0)
            {
                throw new Exception("Error: No se realizó el cambio de Estado.");
            }

            return resultado;
        }
    }
}
