using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{

    /* Clase Profesor - Representa el profesor en el sistema */
    public class Profesor : Usuario
    {
        public string NroEmpleado { get; set; }

        // Lista de materias que el profesor puede enseñar
        public List<Materia> MateriasDictadas { get; set; } = new List<Materia>();
    }
}
