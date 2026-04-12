using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /* Clase Entrega - Representa la entrega de un trabajo práctico por parte de un alumno */
    public class Entrega
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public int IdTP { get; set; }
        public int IdProfesor { get; set; } // El profesor asignado que debe corregir
        public string ContenidoLink { get; set; }
        public string EstadoEntrega { get; set; }    // "No Entregado", "Entregado"
        public string EstadoCorreccion { get; set; } // "PENDIENTE", "CORREGIDO"
        public int? Nota { get; set; }
        public string Devolucion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaCorreccion { get; set; }

        // Objetos completos
        public Alumno Alumno { get; set; }
        public TrabajoPractico TrabajoPractico { get; set; }
        public Profesor Profesor { get; set; }

        // Propiedades calculadas 
        public bool EstaEntregado => EstadoEntrega == "Entregado";
        public bool EstaCorregido => EstadoCorreccion == "CORREGIDO";
        public bool EstaVencido => !EstaEntregado && TrabajoPractico?.FechaLimite < DateTime.Now;
        public bool Aprobo => Nota.HasValue && Nota >= 6;
        public string ResumenEstado => EstaCorregido ? $"Corregido - Nota: {Nota ?? 0}" : EstaEntregado ? "Entregado - Pendiente de corrección" : EstaVencido ? "Vencido sin entregar" : "Pendiente de entrega";
    }
}
