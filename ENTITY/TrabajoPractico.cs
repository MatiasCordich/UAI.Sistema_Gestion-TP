using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /* Clase TrabajoPractico - Representa un trabajo práctico en el sistema */
    public class TrabajoPractico
    {
        public int Id { get; set; }
        public int IdMateria { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public bool Activo { get; set; }
    }
}
