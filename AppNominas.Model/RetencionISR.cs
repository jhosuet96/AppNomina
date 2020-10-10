using System.ComponentModel.DataAnnotations;

namespace AppNominas.Model
{
    public class RetencionISR
    {
        [Key]
        public int Id { get; set; }
        public decimal LimiteInferior { get; set; }
        public decimal LimiteSuperior { get; set; }
        public decimal PorcentajeExcedente { get; set; }
        public decimal TasaFija { get; set; }
        
    }


}
