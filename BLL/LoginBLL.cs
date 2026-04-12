using DAL;
using ENTITY;
using System;
using System.Security.Cryptography;
using System.Text;


namespace BLL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de negocio del Login. 
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class LoginBLL
    {

        /* Instancia de la clase LoginDAL para manejar la lógica de acceso a datos relacionada con el login. */
        private readonly LoginDAL _loginDal = new LoginDAL();

        // -----------------------------------------------------------------------------------------------------------------------
        // LOGIN - Valida datos y contraseña hasheada.
        // -----------------------------------------------------------------------------------------------------------------------
        public Usuario Login(string dni, string password)
        {
            /* Validaciones básicas. DNI y Contraseña obligatorios. */
            if (string.IsNullOrEmpty(dni)) throw new Exception("El DNI es obligatorio.");
            if (string.IsNullOrEmpty(password)) throw new Exception("La contraseña es obligatoria.");

            /* Se hashea la contraseña. */
            string passwordHash = Utilidades.EncriptarPassword(password);

            /* Se llama a la DAL para Login. */
            Usuario user = _loginDal.Login(dni, passwordHash);

            /* Si la DAL devuelve null, los datos están mal */
            if (user == null)
            {
                throw new Exception("DNI o Contraseña incorrectos.");
            }

            /* Si todo sale bien, devuelve el usuario. */
            return user;
        }
    }
}
