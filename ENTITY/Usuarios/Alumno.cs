using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /* Clase Alumno - Representa a un alumno en el sistema */
    public class Alumno : Usuario
    {

        // Columna específica de la tabla
        public string Legajo { get; set; }
    }
}
