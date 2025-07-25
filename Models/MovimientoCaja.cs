namespace KioscoApp.Models
{
    public class MovimientoCaja
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Descripcion { get; set; }
        public decimal Monto { get; set; } // Positivo entrada, negativo salida
    }
}
