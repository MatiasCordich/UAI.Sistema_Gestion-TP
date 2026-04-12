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
    Se encarga de manejar la lógica de negocio de las Entregas.
    ------------------------------------------------------------------------------------------------------------------------------*/
    public class EntregaBLL
    {
        private readonly EntregaDAL _entregaDAL = new EntregaDAL();

        /* ------------------------------------------ [METODOS BASICOS] ------------------------------------------ */

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR ENTREGAS POR TP
        // -----------------------------------------------------------------------------------------------------------------------
        public List<Entrega> ListarEntregasPorTP(int idTP)
        {
            return _entregaDAL.ListarEntregasPorTP(idTP);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // LISTAR ENTREGAS POR ALUMNO
        // -----------------------------------------------------------------------------------------------------------------------
        public List<Entrega> ListarEntregasPorAlumno(int idAlumno)
        {
            return _entregaDAL.ListarEntregasPorAlumno(idAlumno);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // ENTREGAR TP - El alumno sube el link de su entrega.
        // -----------------------------------------------------------------------------------------------------------------------
        public int EntregarTP(int idEntrega, int idAlumno, string contenidoLink)
        {
            Entrega entrega = _entregaDAL.ObtenerPorId(idEntrega);

            // Regla 1: La entrega debe existir
            if (entrega == null)
                throw new Exception("La entrega no existe.");

            // Regla 2: Solo el alumno dueño puede entregar
            if (entrega.IdAlumno != idAlumno)
                throw new Exception("No podés entregar un TP que no te pertenece.");

            // Regla 3: No se puede volver a entregar si ya fue entregado
            if (entrega.EstaEntregado)
                throw new Exception("El TP ya fue entregado.");

            // Regla 4: No se puede entregar si el TP está vencido
            if (entrega.EstaVencido)
                throw new Exception("No se puede entregar el TP porque la fecha límite ya pasó.");

            // Regla 5: El link es obligatorio
            if (string.IsNullOrWhiteSpace(contenidoLink))
                throw new Exception("El link de entrega es obligatorio.");

            bool resultado = _entregaDAL.EntregarTP(idEntrega, contenidoLink);
            if (!resultado) throw new Exception("No se pudo registrar la entrega.");

            return idEntrega;
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // CORREGIR ENTREGA - El profesor carga la nota y devolución.
        // -----------------------------------------------------------------------------------------------------------------------
        public int CorregirEntrega(int idEntrega, int idProfesor, int nota, string devolucion)
        {
            Entrega entrega = _entregaDAL.ObtenerPorId(idEntrega);

            // Regla 1: La entrega debe existir
            if (entrega == null)
                throw new Exception("La entrega no existe.");

            // Regla 2: Solo el profesor asignado puede corregir
            if (entrega.IdProfesor != idProfesor)
                throw new Exception("No podés corregir una entrega que no te pertenece.");

            // Regla 3: Solo se puede corregir si fue entregado
            if (!entrega.EstaEntregado)
                throw new Exception("No se puede corregir un TP que aún no fue entregado.");

            // Regla 4: No se puede corregir dos veces
            if (entrega.EstaCorregido)
                throw new Exception("La entrega ya fue corregida.");

            // Regla 5: La nota debe estar entre 0 y 10
            if (nota < 0 || nota > 10)
                throw new Exception("La nota debe estar entre 0 y 10.");

            // Regla 6: La devolución es obligatoria
            if (string.IsNullOrWhiteSpace(devolucion))
                throw new Exception("La devolución es obligatoria.");

            bool resultado = _entregaDAL.CorregirEntrega(idEntrega, nota, devolucion);
            if (!resultado) throw new Exception("No se pudo registrar la corrección.");

            return idEntrega;
        }
    }
}
