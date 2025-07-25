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
        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            string termino = BuscarTextBox.Text.Trim().ToLower();

            var resultados = _context.Productos
                .Where(p => p.Nombre.ToLower().Contains(termino) || p.CodigoBarras.ToLower().Contains(termino))
                .ToList();

            ProductosDataGrid.ItemsSource = resultados;
        }

        private void LimpiarBusqueda_Click(object sender, RoutedEventArgs e)
        {
            BuscarTextBox.Clear();
            CargarProductos();
        }

        private void GuardarProducto_Click(object sender, RoutedEventArgs e)
        {
            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(CodigoBarrasTextBox.Text) ||
                string.IsNullOrWhiteSpace(NombreTextBox.Text) ||
                string.IsNullOrWhiteSpace(PrecioCompraTextBox.Text) ||
                string.IsNullOrWhiteSpace(PrecioVentaTextBox.Text) ||
                string.IsNullOrWhiteSpace(StockTextBox.Text))
            {
                MessageBox.Show("Por favor completa todos los campos obligatorios.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validar números válidos
            if (!decimal.TryParse(PrecioCompraTextBox.Text, out decimal precioCompra))
            {
                MessageBox.Show("Precio de Compra no es válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PrecioVentaTextBox.Text, out decimal precioVenta))
            {
                MessageBox.Show("Precio de Venta no es válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(StockTextBox.Text, out int stock))
            {
                MessageBox.Show("Stock no es válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (productoEnEdicion == null)
            {
                // Insertar nuevo
                var producto = new Producto
                {
                    CodigoBarras = CodigoBarrasTextBox.Text,
                    Nombre = NombreTextBox.Text,
                    Descripcion = DescripcionTextBox.Text,
                    PrecioCompra = precioCompra,
                    PrecioVenta = precioVenta,
                    Stock = stock,
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
                productoEnEdicion.PrecioCompra = precioCompra;
                productoEnEdicion.PrecioVenta = precioVenta;
                productoEnEdicion.Stock = stock;
                productoEnEdicion.Categoria = CategoriaTextBox.Text;

                _context.Productos.Update(productoEnEdicion);
                productoEnEdicion = null;
            }

            _context.SaveChanges();
            MessageBox.Show("Producto guardado correctamente!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

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
