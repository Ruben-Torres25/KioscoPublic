using System.ComponentModel.DataAnnotations;

namespace KioscoApp.Models
{
    public class Proveedor
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string CUIT { get; set; } = string.Empty;

        [Required]
        public string Contacto { get; set; } = string.Empty;
    }
}
