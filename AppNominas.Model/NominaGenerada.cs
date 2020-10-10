using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AppNominas.Model
{
    public class NominaGenerada
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Nomina))]
        public int NominasId { get; set; }
        public DateTime FechaNominaGenerada { get; set; }
        public DateTime FechaProximoCorte { get; set; }
        public int DiasLaborados { get; set; }
        public decimal SueldoGenerado { get; set; }
        public virtual Nomina Nominas { get; set; }
    }
}
