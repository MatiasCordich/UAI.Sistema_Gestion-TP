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

        // Columnas de la tabla
        public int Id{ get; set; }
        public int IdMateria { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
        public bool Activo { get; set; }

        // Objeto Completos
        public Materia Materia { get; set; }

        // Porpiedades calculadas
        public bool EstaVencido => FechaLimite < DateTime.Now;
        public bool EstaActivo => Activo && !EstaVencido;
        public string Estado => !Activo ? "Inactivo" : EstaVencido ? "Vencido" : "Vigente";
        public int DiasRestantes => EstaVencido ? 0 : (FechaLimite - DateTime.Now).Days;
    }
}
