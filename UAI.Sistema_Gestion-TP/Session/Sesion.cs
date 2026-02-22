using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAI.Sistema_Gestion_TP.UserSesion
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de mantener la sesión del usuario que ha iniciado sesión en el sistema. Guarda una instancia y un método para cerrar sesión.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public static class Sesion
    {
        // Esta variable guarda la instancia del usuario que ha iniciado sesión en el sistema. Es de tipo Usuario, que es la clase base para los diferentes tipos de usuarios (Administrador, Profesor, Alumno).
        public static Usuario UsuarioLogueado { get; set; }

        // Metodo para cerrar sesión. 
        public static void Logout()
        {
            UsuarioLogueado = null;
        }

        // Metodo para saber si hay algun usuario logueado. 
        public static bool IsLogged()
        {
            return UsuarioLogueado != null;
        }
    }
}
