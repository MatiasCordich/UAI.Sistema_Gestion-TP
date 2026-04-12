using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /* Clase Usuario - Es una clase base para los usuarios del sistema */
    public abstract class Usuario
    {
        // Columnas de la tabla
        public int Id { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; }

        // Propiedades Calculadas
        public string NombreCompleto => $"{Apellido}, {Nombre}";
       
    }
}
