using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /* Clase Materia - Representa una materia en el sistema */
    public class Materia
    {
        // Columnas de la tabla
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        // Propiedades Calculadas
        public string Estado => Activo ? "Activa" : "Inactiva";
    }
}
