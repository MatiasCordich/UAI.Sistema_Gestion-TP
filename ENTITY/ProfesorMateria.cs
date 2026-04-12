using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /* Clase ProfesorMateria - Representa las asignaciones de profesores a una materia en el sistema */
    public class ProfesorMateria
    {
        // Columnas de la tabla (IDs planos para DAL)
        public int IdProfesor { get; set; }
        public int IdMateria { get; set; }

        // Objetos completos
        public Profesor Profesor { get; set; }
        public Materia Materia { get; set; }

        // Propiedades calculadas
        public string ResumenAsignacion => $"{Profesor?.NombreCompleto ?? "Sin profesor"} - {Materia?.Nombre ?? "Sin materia"}";
    }
}
