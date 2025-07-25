using System.Windows;
using KioscoApp.Data;
using KioscoApp.Models;
using System.Linq;

namespace KioscoApp
{
    public partial class MainWindow : Window
    {
        private readonly KioscoDbContext _context;
        private Producto productoEnEdicion = null;

        public MainWindow()
        {
            InitializeComponent();

            // Obtiene el contexto del contenedor Host
            // Línea correcta
            _context = App.AppHost.Services.GetService(typeof(KioscoDbContext)) as KioscoDbContext;



            CargarProductos();
        }

        private void CargarProductos()
        {
            var productos = _context.Productos.ToList();
            ProductosDataGrid.ItemsSource = productos;
        }

        private void GuardarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (productoEnEdicion == null)
            {
                // Insertar nuevo
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
            }
            else
            {
                // Actualizar existente
                productoEnEdicion.CodigoBarras = CodigoBarrasTextBox.Text;
                productoEnEdicion.Nombre = NombreTextBox.Text;
                productoEnEdicion.Descripcion = DescripcionTextBox.Text;
                productoEnEdicion.PrecioCompra = decimal.Parse(PrecioCompraTextBox.Text);
                productoEnEdicion.PrecioVenta = decimal.Parse(PrecioVentaTextBox.Text);
                productoEnEdicion.Stock = int.Parse(StockTextBox.Text);
                productoEnEdicion.Categoria = CategoriaTextBox.Text;

                _context.Productos.Update(productoEnEdicion);
                productoEnEdicion = null;
            }

            _context.SaveChanges();
            MessageBox.Show("Producto guardado!");

            CargarProductos();
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            CodigoBarrasTextBox.Clear();
            NombreTextBox.Clear();
            DescripcionTextBox.Clear();
            PrecioCompraTextBox.Clear();
            PrecioVentaTextBox.Clear();
            StockTextBox.Clear();
            CategoriaTextBox.Clear();
        }

        private void EditarProducto_Click(object sender, RoutedEventArgs e)
        {
            var productoSeleccionado = (sender as FrameworkElement).DataContext as Producto;
            if (productoSeleccionado != null)
            {
                productoEnEdicion = productoSeleccionado;

                CodigoBarrasTextBox.Text = productoEnEdicion.CodigoBarras;
                NombreTextBox.Text = productoEnEdicion.Nombre;
                DescripcionTextBox.Text = productoEnEdicion.Descripcion;
                PrecioCompraTextBox.Text = productoEnEdicion.PrecioCompra.ToString();
                PrecioVentaTextBox.Text = productoEnEdicion.PrecioVenta.ToString();
                StockTextBox.Text = productoEnEdicion.Stock.ToString();
                CategoriaTextBox.Text = productoEnEdicion.Categoria;
            }
        }

        private void EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            var productoSeleccionado = (sender as FrameworkElement).DataContext as Producto;
            if (productoSeleccionado != null)
            {
                var resultado = MessageBox.Show($"¿Estás seguro de eliminar '{productoSeleccionado.Nombre}'?", "Confirmar", MessageBoxButton.YesNo);
                if (resultado == MessageBoxResult.Yes)
                {
                    _context.Productos.Remove(productoSeleccionado);
                    _context.SaveChanges();
                    CargarProductos();
                }
            }
        }

    }
}
