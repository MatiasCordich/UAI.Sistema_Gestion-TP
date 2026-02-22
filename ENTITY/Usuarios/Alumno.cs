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
        public string Legajo { get; set; }

        // Un alumno tiene una lista de materias inscriptas
        public List<Inscripcion> MateriasInscriptas { get; set; } = new List<Inscripcion>();
    }
}
