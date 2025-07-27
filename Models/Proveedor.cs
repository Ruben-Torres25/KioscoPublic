using System.ComponentModel.DataAnnotations;

namespace KioscoApp.Models
{
    public class Proveedor
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Cuit { get; set; }

        public string Contacto { get; set; }
    }
}
