namespace KioscoApp.Models
{
    public class DetalleVenta
    {
        public Producto Producto { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal Subtotal => Producto.PrecioVenta * Cantidad;
    }
}
