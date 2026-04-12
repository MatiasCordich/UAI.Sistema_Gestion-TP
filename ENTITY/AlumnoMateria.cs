using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /* Clase AlumnoMateria - Representa la inscripción de un alumno en una materia */
    public class AlumnoMateria
    {
        // Columnas de la tabla 
        public int IdAlumno { get; set; }
        public int IdMateria { get; set; }
        public int IdProfesor { get; set; }

        // Objetos completo 
        public Materia Materia { get; set; }
        public Alumno AlumnoInscripto { get; set; }
        public Profesor ProfesorAsignado { get; set; }

        // Propiedades calculadas
        public string ResumenInscripcion => $"{Materia?.Nombre ?? "Sin materia"} - Prof. {ProfesorAsignado?.NombreCompleto ?? "Sin asignar"}";
    }
}
