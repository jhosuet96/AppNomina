using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppNominas.Model
{
    public class Nomina
    {
        [Key]
        public int Id { get; set; }


        public decimal SueldoBruto { get; set; }

        public decimal RetencionAFP { get; set; }

        public decimal RetencionARS { get; set; }
        public decimal RetencionISR { get; set; }
        
        public decimal SueldoImponible { get; set; }
        
        [ForeignKey(nameof(AppNominas.Model.RetencionISR))]
        public int RetencionIsrId { get; set; }
        //|\\
        public decimal TotalRetencion { get; set; }

        public decimal SueldoNeto { get; set; }
        public bool Activo { get; set; }
        //|\\
        [ForeignKey(nameof(Empleado))]
        public int EmpleadoID { get; set; }

        public DateTime FechaCreacion { get; set; }
        

        public DateTime FechaNominaGenerada { get; set; }
        //|\\
        public virtual Empleado Empleados { get; set; }
        public virtual RetencionISR RetencionIsr { get; set; }
        
    }
}
