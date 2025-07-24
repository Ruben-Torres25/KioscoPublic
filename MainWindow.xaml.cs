using System.Windows;
using KioscoApp.Data;
using KioscoApp.Models;
using System.Linq;

namespace KioscoApp
{
    public partial class MainWindow : Window
    {
        private readonly KioscoDbContext _context;

        public MainWindow()
        {
            InitializeComponent();

            // Obtiene el contexto del contenedor Host
            _context = ((App)Application.Current).AppHost.Services.GetService(typeof(KioscoDbContext)) as KioscoDbContext;

            CargarProductos();
        }

        private void CargarProductos()
        {
            var productos = _context.Productos.ToList();
            ProductosDataGrid.ItemsSource = productos;
        }

        private void GuardarProducto_Click(object sender, RoutedEventArgs e)
        {
            var producto = new Producto
            {
                CodigoBarras = CodigoBarrasTextBox.Text,
                Nombre = NombreTextBox.Text,
                Descripcion = DescripcionTextBox.Text,
                PrecioCompra = decimal.Parse(PrecioCompraTextBox.Text),
                PrecioVenta = decimal.Parse(PrecioVentaTextBox.Text),
                Stock = int.Parse(StockTextBox.Text),
                Categoria = CategoriaTextBox.Text
            };

            _context.Productos.Add(producto);
            _context.SaveChanges();

            MessageBox.Show("Producto guardado!");

            CargarProductos();

            // Limpia campos
            CodigoBarrasTextBox.Clear();
            NombreTextBox.Clear();
            DescripcionTextBox.Clear();
            PrecioCompraTextBox.Clear();
            PrecioVentaTextBox.Clear();
            StockTextBox.Clear();
            CategoriaTextBox.Clear();
        }
    }
}
