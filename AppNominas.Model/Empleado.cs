using System.ComponentModel.DataAnnotations;

namespace AppNominas.Model
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Sexo { get; set; }
        public decimal SueldoBruto { get; set; }
        public bool Activo { get; set; }
    }
}
