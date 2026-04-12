using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /*------------------------------------------------------------------------------------------------------------------------------
    Se encarga de manejar la lógica de negocio de las Materias, sus asignaciones a Profesores
    y las inscripciones de Alumnos
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class MateriaBLL
    {
        /* DAL de Usuarios, ProfesorMateria y AlumnoMateria. */
        private readonly MateriaDAL _materiaDAL = new MateriaDAL();
        private readonly ProfesorMateriaDAL _profesorMateriaDAL = new ProfesorMateriaDAL();
        private readonly AlumnoMateriaDAL _alumnoMateriaDAL = new AlumnoMateriaDAL();

        /* ------------------------------------------ [METODOS BASICOS] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR MATERIAS - Filtro opcional por estado
        // -----------------------------------------------------------------------------------------------------------------------
        public List<Materia> ListarMaterias(string estado = "Todas")
        {
            return _materiaDAL.ListarMaterias(estado);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CREAR MATERIA - Valida y delega el INSERT a la DAL. Devuelve el ID generado.
        // -----------------------------------------------------------------------------------------------------------------------
        public int CrearMateria(Materia materia)
        {
            /* Regla 1: Nombre obligatorio */
            if (string.IsNullOrWhiteSpace(materia.Nombre))
                throw new Exception("El nombre de la materia es obligatorio.");

            /* Regla 2: Nombre único */
            if (_materiaDAL.ExisteNombre(materia.Nombre))
                throw new Exception("Ya existe una materia con ese nombre.");

            return _materiaDAL.InsertarMateria(materia);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // MODIFICAR MATERIA 
        // -----------------------------------------------------------------------------------------------------------------------
        public int ModificarMateria(Materia materia)
        {
            /* Regla 1: Nombre obligatorio */
            if (string.IsNullOrWhiteSpace(materia.Nombre))
                throw new Exception("El nombre de la materia es obligatorio.");

            /* Regla 2: Nombre único (excluyendo la propia materia) */
            if (_materiaDAL.ExisteNombre(materia.Nombre, materia.Id))
                throw new Exception("Ya existe una materia con ese nombre.");

            /* Se ejecuta el DAL de Modificar Materia */
            bool modificado = _materiaDAL.ModificarMateria(materia);

            /* Se evalua el resultado de la modifación*/
            if (!modificado) 
                throw new Exception("No se pudo modificar la materia.");

            /* Si todo salió bien, devuelvo el ID de Materia que se modificó */
            return materia.Id;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CAMBIAR ESTADO - Soft Delete / Reactivación.
        // -----------------------------------------------------------------------------------------------------------------------
        public int CambiarEstado(int idMateria, bool nuevoEstado)
        {
            /* Regla 1: No se puede reactivar una materia que ya está activa ni desactivar una que ya está inactiva. */
            Materia materia = _materiaDAL.ObtenerPorId(idMateria);

            /* Se valida que la materia exista */
            if (materia == null)
                throw new Exception("La materia no existe.");

            /* Según el nuevo estado, se muestra un mensaje o otro */
            if (materia.Activo == nuevoEstado)
                throw new Exception(nuevoEstado ? "La materia ya está activa." : "La materia ya está inactiva.");

            /* Se ejecuta el DAL para hacer el Soft Delete */
            bool resultado = _materiaDAL.CambiarEstadoMateria(idMateria, nuevoEstado);

            /* Se evalua el resultado una vez ejecutado el Soft Delete */
            if (!resultado) 
                throw new Exception("No se pudo cambiar el estado de la materia.");

            /* Si todo salió bien, devuelvo el ID de la materia que cambió de Estado */
            return idMateria;
        }

        // ------------------------------------------------ [PROFESOR - MATERIA] -------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------------------
        // ASIGNAR PROFESOR A MATERIA
        // -----------------------------------------------------------------------------------------------------------------------
        public bool AsignarProfesor(int idProfesor, int idMateria)
        {
            /* Regla 1: La materia debe estar activa */
            Materia materia = _materiaDAL.ObtenerPorId(idMateria);
            if (materia == null)
                throw new Exception("La materia no existe.");

            if (!materia.Activo)
                throw new Exception("No se puede asignar un profesor a una materia inactiva.");

            /* Regla 2: No se debe duplicar la asignación */
            if (_profesorMateriaDAL.ExisteAsignacion(idProfesor, idMateria))
                throw new Exception("El profesor ya está asignado a esta materia.");

            /* Regla 3: Límite de 5 profesores por materia */
            if (_profesorMateriaDAL.ContarProfesoresPorMateria(idMateria) >= 5)
                throw new Exception("La materia ya alcanzó el límite de 5 profesores asignados.");

            /* Ejecuto el DAL para asignar un profesor a una Materia */
            bool resultado = _profesorMateriaDAL.AsignarProfesor(idProfesor, idMateria);

            /* Se evalua el resultado una vez ejecutado el DAL */
            if (!resultado) 
                throw new Exception("No se pudo asignar el profesor a la materia.");

            /* Si todo salío bien, devuelvo true */
            return true;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // QUITAR PROFESOR DE UNA MATERIA
        // -----------------------------------------------------------------------------------------------------------------------
        public bool QuitarProfesor(int idProfesor, int idMateria)
        {
            /* Regla 1 : La asignación debe existir */
            if (!_profesorMateriaDAL.ExisteAsignacion(idProfesor, idMateria))
                throw new Exception("El profesor no está asignado a esta materia.");

            /* Ejecuto el DAL para quitar el profesor de la materia */
            bool resultado = _profesorMateriaDAL.QuitarProfesor(idProfesor, idMateria);

            /* Se evalua el resultado una vez ejecutado el DAL */
            if (!resultado) throw new Exception("No se pudo quitar el profesor de la materia.");

            /* Si todo salió bien, devuelvo true */
            return resultado;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR PROFESORES POR MATERIA
        // -----------------------------------------------------------------------------------------------------------------------
        public List<ProfesorMateria> ListarProfesoresPorMateria(int idMateria)
        {
            /* Regla 1: La materia debe existir */
            Materia materia = _materiaDAL.ObtenerPorId(idMateria);

            if (materia == null)
                throw new Exception("La materia no existe.");

            /* Devuelvo la lista de Profesores por Materia */
            return _profesorMateriaDAL.ListarProfesoresPorMateria(idMateria);
        }

        // ------------------------------------------------- [ALUMNO - MATERIA] --------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR ALUMNOS POR MATERIA
        // -----------------------------------------------------------------------------------------------------------------------
        public List<AlumnoMateria> ListarAlumnosPorMateria(int idMateria)
        {
            /* Regla 1: La materia debe exister */
            Materia materia = _materiaDAL.ObtenerPorId(idMateria);

            if (materia == null)
                throw new Exception("La materia no existe.");

            /* Si existe, se ejecuta el DAL para listar Alumnos por Materia */
            return _alumnoMateriaDAL.ListarAlumnosPorMateria(idMateria);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR MATERIAS POR ALUMNO
        // -----------------------------------------------------------------------------------------------------------------------
        public List<AlumnoMateria> ListarMateriasPorAlumno(int idAlumno)
        {
            /* Se ejecuta el DAL para listar Materias por Alumno */
            return _alumnoMateriaDAL.ListarMateriasPorAlumno(idAlumno);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // INSCRIBIR ALUMNO EN MATERIA
        // -----------------------------------------------------------------------------------------------------------------------
        public bool InscribirAlumno(int idAlumno, int idMateria, int idProfesor)
        {
            /* Regla 1: La materia debe estar activa. */
            Materia materia = _materiaDAL.ObtenerPorId(idMateria);
            if (materia == null)
                throw new Exception("La materia no existe.");

            if (!materia.Activo)
                throw new Exception("No se puede inscribir un alumno en una materia inactiva.");

            /* Regla 2: El profesor debe estar asignado a la materia. */
            if (!_profesorMateriaDAL.ExisteAsignacion(idProfesor, idMateria))
                throw new Exception("El profesor no está asignado a esta materia.");

            /* Regla 3: El alumno no puede estar ya inscripto. */
            if (_alumnoMateriaDAL.ExisteInscripcion(idAlumno, idMateria))
                throw new Exception("El alumno ya está inscripto en esta materia.");

            // Regla 4: Límite de 30 alumnos por profesor en la materia
            if (_alumnoMateriaDAL.ContarAlumnosPorProfesorEnMateria(idProfesor, idMateria) >= 30)
                throw new Exception("Esta clase ya alcanzó los 30 alumnos.");

            /* Se ejecuta el DAL para inscribir el Alumno a la materia */
            bool resultado = _alumnoMateriaDAL.InscribirAlumno(idAlumno, idMateria, idProfesor);

            /* Se evalua el resultado de la ejución del DAL */
            if (!resultado) 
                throw new Exception("No se pudo inscribir al alumno en la materia.");

            /* Si todo salió bien, se devuelve el resultado (True) */
            return resultado;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // MODIFICAR PROFESOR DE UNA INSCRIPCION - Cambia el profesor asignado a un alumno en una materia.
        // -----------------------------------------------------------------------------------------------------------------------
        public bool ModificarInscripcion(int idAlumno, int idMateria, int idProfesorNuevo)
        {
            /* Regla 1: La inscripción debe existir */
            if (!_alumnoMateriaDAL.ExisteInscripcion(idAlumno, idMateria))
                throw new Exception("El alumno no está inscripto en esta materia.");

            /* Regla 2: El nuevo profesor debe estar asignado a la materia */
            if (!_profesorMateriaDAL.ExisteAsignacion(idProfesorNuevo, idMateria))
                throw new Exception("El profesor no está asignado a esta materia.");

            /* Regla 3: Límite de 30 alumnos por profesor en la materia */
            if (_alumnoMateriaDAL.ContarAlumnosPorProfesorEnMateria(idProfesorNuevo, idMateria) >= 30)
                throw new Exception("El profesor ya alcanzó el límite de 30 alumnos en esta materia.");

            /* Se ejecuta el DAL para modificar el profesor de la inscripción */
            bool resultado = _alumnoMateriaDAL.ModificarInscripcion(idAlumno, idMateria, idProfesorNuevo);

            /* Se evalúa el resultado */
            if (!resultado)
            {
                throw new Exception("No se pudo modificar la inscripción.");
            }

            /* Si todo sale bien, devuelvo el resultado (True). */
            return resultado;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // DESINSCRIBIR ALUMNO DE MATERIA
        // -----------------------------------------------------------------------------------------------------------------------
        public bool DesinscribirAlumno(int idAlumno, int idMateria)
        {
            // Regla: La inscripción debe existir
            if (!_alumnoMateriaDAL.ExisteInscripcion(idAlumno, idMateria))
                throw new Exception("El alumno no está inscripto en esta materia.");

            /* Se ejecuta el DAL para inscribir el Alumno a la materia */
            bool resultado = _alumnoMateriaDAL.DesinscribirAlumno(idAlumno, idMateria);

            /* Se evalua el resultado de la ejución del DAL */
            if (!resultado) throw new Exception("No se pudo desinscribir al alumno de la materia.");

            /* Si todo salió bien, se devuelve el resultado (True) */
            return resultado;
        }
    }
}

