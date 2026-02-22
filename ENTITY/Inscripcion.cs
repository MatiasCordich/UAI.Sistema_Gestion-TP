using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /* Clase Inscripcion - Representa la inscripción de un alumno en una materia */
    public class Inscripcion
    {
        public int Alumno { get; set; }
        public Materia Materia { get; set; }
        public Profesor ProfesorAsignado { get; set; }
    }
}
