using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Clase utilitaria con métodos compartidos por las distintas clases de la BLL.
    Es estática porque no se puede instanciar (crear objetos a partir de la clase). Se accede a sus métodos directamente a través
    del nombre de la clase
    ------------------------------------------------------------------------------------------------------------------------------*/
    public static class Utilidades
    {

        // -----------------------------------------------------------------------------------------------------------------------
        // HASH SHA256 - Convierte una contraseña en texto plano a su representación SHA256 en hexadecimal (64 caracteres).
        // -----------------------------------------------------------------------------------------------------------------------
        public static string EncriptarPassword(string password)
        {
            /* Se encripta la contraseña usando el codigo de encriptación SHA256 */
            using (SHA256 sha256Hash = SHA256.Create())
            {
                /* Se convierte la contraseña ingresada en texto plano a bytes y calcular el hash. */
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                /* Se instancia el string builder, un string mutable de caracterees para el hash. */
                StringBuilder builder = new StringBuilder();

                /* Se recorre cada byte del hash y lo convierte en un hexadecimal de dos digitos. */
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));

                /* Devuelve el hash (los 64 caracteres hexadecimales) */
                return builder.ToString();
            }
        }

    }
}
