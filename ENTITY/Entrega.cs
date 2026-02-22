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
    }
}
