using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de negocio del Usuario.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class UsuarioBLL
    {
        /* DAL de Usuarios. */
        private readonly UsuarioDAL _usuarioDAL = new UsuarioDAL();

        /* ------------------------------------------ [METODOS BASICOS] ------------------------------------------ */

        // -----------------------------------------------------------
        // LISTAR USUARIOS - Filtros opcionales por DNI, Rol y Estado
        // -----------------------------------------------------------
        public List<Usuario> ListarUsuarios(string dni = "", string rol = "Todos", string estado = "Todos")
        {
            return _usuarioDAL.ListarUsuarios(dni, rol, estado);
        }

        // -----------------------------------------------------------
        // CREAR USUARIO
        // -----------------------------------------------------------
        public int CrearUsuario(Usuario user)
        {

            /* Regla 1: Campos obligatorios */
            ValidarCamposObligatorios(user);

            /* Regla 1B: Contraseña obligatoria al crear */
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("La contraseña es obligatoria.");

            /* Regla 2:  Email válido */
            if (!EsEmailValido(user.Email))
                throw new Exception("El formato del Email no es válido.");

            /* Regla 3. DNI, longitud entre 6 y 8 caracteres */
            if (user.DNI.Length < 6 || user.DNI.Length > 8)
                throw new Exception("El DNI debe tener entre 6 y 8 caracteres. Intente otra vez.");

            /* Regla 4. Formato de contraseña válido */
            ValidarPassword(user.Password);

            /* Regla 5. DNI único */
            if (_usuarioDAL.ExisteDNI(user.DNI))
            {
                throw new Exception("Ya existe un usuario registrado con ese DNI.");
            }

            /* Regla 6. Email único */
            if (_usuarioDAL.ExisteEmail(user.Email))
                throw new Exception("El Email ya está siendo utilizado por otro usuario.");

            /* Una vez que pasó todas las validaciones, se hashea la constraseña */
            user.Password = Utilidades.EncriptarPassword(user.Password);

            /* Se ejecuta la función de insertar un usuario a la DB */
            int newID = _usuarioDAL.InsertarUsuario(user);

            /* Se devuelve el ID generado para el usuario generado */
            return newID;
        }

        // -----------------------------------------------------------
        // MODIFICAR USUARIO 
        // -----------------------------------------------------------
        public int ModificarUsuario(Usuario user)
        {
            /* Regla 1: Campos obligatorios */
            ValidarCamposObligatorios(user);

            /* Regla 2: Email válido */
            if (!EsEmailValido(user.Email))
                throw new Exception("El formato del Email no es válido.");

            /* Regla 3: DNI entre 6 y 8 caracteres */
            if (user.DNI.Length < 6 || user.DNI.Length > 8)
                throw new Exception("El DNI debe tener entre 6 y 8 caracteres.");

            /* Regla 4: DNI único (excluyendo al propio usuario) */
            if (_usuarioDAL.ExisteDNI(user.DNI, user.Id))
                throw new Exception("Ya existe un usuario registrado con ese DNI.");

            /* Regla 5: Email único (excluyendo al propio usuario) */
            if (_usuarioDAL.ExisteEmail(user.Email, user.Id))
                throw new Exception("El Email ya está siendo utilizado por otro usuario.");

            /* Si hay una nueva contraseña, se valida y hashea.*/
            /* Si viene vacío, se mantiene la contraseña actual de la DB. */
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                ValidarPassword(user.Password);
                user.Password = Utilidades.EncriptarPassword(user.Password);
            }
            else
            {
                user.Password = _usuarioDAL.ObtenerPasswordActual(user.Id);
            }

            /* Se ejecuta la función de modificar un usuario de la DB */
            bool modificado = _usuarioDAL.ModificarUsuario(user);

            /* Si no se pudo modificar, lanzar un mensaje de error */
            if (!modificado)
                throw new Exception("No se pudo modificar el usuario.");

            /* Caso contrario devolver el ID del usuario modificado */
            return user.Id;
        }

        // -----------------------------------------------------------
        // CAMBIAR ESTADO DEL USUARIO (BAJA/REACTIVACIÓN)
        // -----------------------------------------------------------
        public int CambiarEstado(int idUsuarioObjetivo, bool nuevoEstado, int idUsuarioLogueado)
        {
            /* Regla 1: Evita darse de baja a sí mismo */
            if (nuevoEstado == false && idUsuarioObjetivo == idUsuarioLogueado)
                throw new Exception("Seguridad: No puedes darte de baja a vos mismo.");

            /* SOFT DELETE: Cambia el estado del Usuario */
            bool resultado = _usuarioDAL.CambiarEstadoUsuario(idUsuarioObjetivo, nuevoEstado);

            /* Si la modificación no se realizó, lanzamos una excepción */
            if (!resultado)
                throw new Exception("Error: No se pudo cambiar el estado del Usuario. Intente otra vez.");

            /* Si todo salió bien, devolvemos el ID del Usuario modificado */
            return idUsuarioObjetivo;
        }

        /* ------------------------------------------ [METODOS AUXILIARES] ------------------------------------------ */

        // -----------------------------------------------------------
        // VALIDAR CAMPOS OBLIGATORIOS
        // -----------------------------------------------------------
        private void ValidarCamposObligatorios(Usuario usuario)
        {
            /* Valida que los campos sean obligatorios */
            if (string.IsNullOrWhiteSpace(usuario.DNI)) throw new Exception("El DNI es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Nombre)) throw new Exception("El Nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Apellido)) throw new Exception("El Apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Email)) throw new Exception("El Email es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuario.Rol)) throw new Exception("El Rol es obligatorio.");
        }

        // -----------------------------------------------------------
        // VALIDAR FORMATO DE EMAIL
        // -----------------------------------------------------------
        private bool EsEmailValido(string email)
        {
            /* Devuelve un booleano si el valor ingresado es un Email o no */
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // -----------------------------------------------------------
        // VALIDAR FORMATO DE CONTRASEÑA
        // -----------------------------------------------------------
        private void ValidarPassword(string pass)
        {

            /* Se valida que la contraseña tenga mínimo 6 caracteres */
            if (pass.Length < 6)
                throw new Exception("La contraseña debe tener al menos 6 caracteres.");

            /* Se valida que la constraseña deba ser alfanumerica */
            if (!pass.Any(char.IsLetter) || !pass.Any(char.IsNumber))
                throw new Exception("La contraseña debe ser alfanumérica.");

        }


    }
}
