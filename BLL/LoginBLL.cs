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
    Se encarga de manejar la lógica de negocio del Login, como validar los datos de entrada y manejar las excepciones. 
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class LoginBLL
    {

        // Instancia de la clase UsuarioDAL para manejar la lógica de acceso a datos relacionada con los usuarios.
        private readonly LoginDAL _loginDal = new LoginDAL();

        public Usuario Login(string dni, string password)
        {
            // Validaciones básicas de superficie
            if (string.IsNullOrEmpty(dni)) throw new Exception("El DNI es obligatorio.");
            if (string.IsNullOrEmpty(password)) throw new Exception("La contraseña es obligatoria.");

            // Llamamos a la DAL
            Usuario user = _loginDal.Login(dni, password);

            // Si la DAL devuelve null, es que los datos están mal
            if (user == null) throw new Exception("DNI o Contraseña incorrectos.");

            return user;
        }
    }
}
