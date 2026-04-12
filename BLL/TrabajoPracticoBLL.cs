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
    Se encarga de manejar la lógica de negocio de los Trabajos Prácticos.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class TrabajoPracticoBLL
    {
        /* DALs de TrabajoPractico, Materia y Profesor. */
        private readonly TrabajoPracticoDAL _tpDAL = new TrabajoPracticoDAL();
        private readonly MateriaDAL _materiaDAL = new MateriaDAL();
        private readonly ProfesorMateriaDAL _profesorMateriaDAL = new ProfesorMateriaDAL();

        /* ------------------------------------------ [METODOS BASICOS] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR TPs - Filtro opcional por materia y/o profesor
        // -----------------------------------------------------------------------------------------------------------------------
        public List<TrabajoPractico> ListarTPs(int idMateria = 0, int idProfesor = 0)
        {
            return _tpDAL.ListarTPs(idMateria, idProfesor);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CREAR TP - Valida, y delega el INSERT + generación de entregas a la DAL via transacción.
        // Devuelve el ID del TP generado.
        // -----------------------------------------------------------------------------------------------------------------------
        public int CrearTP(TrabajoPractico tp, int idProfesor)
        {
            // Regla 1: Campos obligatorios
            if (string.IsNullOrWhiteSpace(tp.Titulo))
                throw new Exception("El título del TP es obligatorio.");
            if (string.IsNullOrWhiteSpace(tp.Descripcion))
                throw new Exception("La descripción del TP es obligatoria.");

            // Regla 2: Fecha límite debe ser futura
            if (tp.FechaLimite <= DateTime.Now)
                throw new Exception("La fecha límite debe ser una fecha futura.");

            // Regla 3: La materia debe existir y estar activa
            Materia materia = _materiaDAL.ObtenerPorId(tp.IdMateria);
            if (materia == null)
                throw new Exception("La materia no existe.");
            if (!materia.Activo)
                throw new Exception("No se puede crear un TP para una materia inactiva.");

            // Regla 4: El profesor debe estar asignado a la materia
            if (!_profesorMateriaDAL.ExisteAsignacion(idProfesor, tp.IdMateria))
                throw new Exception("El profesor no está asignado a esta materia.");

            // DAL se encarga del INSERT del TP + generación de entregas en una sola transacción
            return _tpDAL.CrearTPConEntregas(tp, idProfesor);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // MODIFICAR TP 
        // -----------------------------------------------------------------------------------------------------------------------
        public int ModificarTP(TrabajoPractico tp)
        {
            /* Regla 1: Campos obligatorios. */
            if (string.IsNullOrWhiteSpace(tp.Titulo))
                throw new Exception("El título del TP es obligatorio.");
            if (string.IsNullOrWhiteSpace(tp.Descripcion))
                throw new Exception("La descripción del TP es obligatoria.");

            /* Regla 2: Fecha límite debe ser futura. */
            if (tp.FechaLimite <= DateTime.Now)
                throw new Exception("La fecha límite debe ser una fecha futura.");

            /* Regla 3: El TP debe existir y estar activo. */
            TrabajoPractico tpExistente = _tpDAL.ObtenerPorId(tp.Id);
            if (tpExistente == null)
                throw new Exception("El TP no existe.");
            if (!tpExistente.Activo)
                throw new Exception("No se puede modificar un TP inactivo.");

            /* Regla 4: No se puede modificar un TP vencido. */
            if (tpExistente.EstaVencido)
                throw new Exception("No se puede modificar un TP vencido.");

            bool modificado = _tpDAL.ModificarTP(tp);

            /* Se evalua el resultado. */
            if (!modificado)
            {
                throw new Exception("No se pudo modificar el TP.");
            }

            /* Si todo salió bien, devolvemos el ID del TP modificado. */
            return tp.Id;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CAMBIAR ESTADO DEL TP
        // -----------------------------------------------------------------------------------------------------------------------
        public int CambiarEstado(int idTP, bool nuevoEstado)
        {
            /* Se valida la existencia del Trabajo Practico*/
            TrabajoPractico tp = _tpDAL.ObtenerPorId(idTP);

            if (tp == null)
                throw new Exception("El TP no existe.");

            /* Se valida el estado del TP. */
            if (tp.Activo == nuevoEstado)
                throw new Exception(nuevoEstado ? "El TP ya está activo." : "El TP ya está inactivo.");

            /* Se ejecuta el DAL para cambiar estado del TP. */
            bool resultado = _tpDAL.CambiarEstadoTP(idTP, nuevoEstado);

            /* Se evalua el resultado. */
            if (!resultado)
            {
                throw new Exception("No se pudo cambiar el estado del TP.");
            }

            /* Si todo salió bien, devolvemos el ID del TP que se le cambió el estado. */
            return idTP;
        }

    }
}
