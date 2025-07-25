using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using KioscoApp.Data;
using KioscoApp.Helpers;
using KioscoApp.Models;

namespace KioscoApp.ViewModels
{
    public class ProductoViewModel : INotifyPropertyChanged
    {
        private readonly KioscoDbContext _context;

        public ObservableCollection<Producto> Productos { get; set; }

        private Producto _productoActual;
        public Producto ProductoActual
        {
            get => _productoActual;
            set
            {
                _productoActual = value;
                OnPropertyChanged(nameof(ProductoActual));
            }
        }

        public string TerminoBusqueda { get; set; }

        // Commands
        public ICommand GuardarCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand BuscarCommand { get; }
        public ICommand LimpiarBusquedaCommand { get; }

        public ProductoViewModel()
        {
            _context = App.AppHost.Services.GetService(typeof(KioscoDbContext)) as KioscoDbContext;

            Productos = new ObservableCollection<Producto>(_context.Productos.ToList());
            ProductoActual = new Producto();

            GuardarCommand = new RelayCommand(GuardarProducto);
            EditarCommand = new RelayCommand(EditarProducto);
            EliminarCommand = new RelayCommand(EliminarProducto);
            BuscarCommand = new RelayCommand(BuscarProducto);
            LimpiarBusquedaCommand = new RelayCommand(LimpiarBusqueda);
        }

        private void GuardarProducto(object obj)
        {
            if (string.IsNullOrWhiteSpace(ProductoActual.Nombre))
            {
                MessageBox.Show("Completa el nombre del producto.");
                return;
            }

            if (ProductoActual.Id == 0)
                _context.Productos.Add(ProductoActual);
            else
                _context.Productos.Update(ProductoActual);

            _context.SaveChanges();
            CargarProductos();
            ProductoActual = new Producto();
        }

        private void EditarProducto(object obj)
        {
            if (obj is Producto producto)
                ProductoActual = producto;
        }

        private void EliminarProducto(object obj)
        {
            if (obj is Producto producto)
            {
                var confirm = MessageBox.Show($"¿Eliminar {producto.Nombre}?", "Confirmar", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    _context.Productos.Remove(producto);
                    _context.SaveChanges();
                    CargarProductos();
                }
            }
        }

        private void BuscarProducto(object obj)
        {
            var term = TerminoBusqueda?.ToLower() ?? "";
            var resultados = _context.Productos
                .Where(p => p.Nombre.ToLower().Contains(term) || p.CodigoBarras.ToLower().Contains(term))
                .ToList();
            Productos = new ObservableCollection<Producto>(resultados);
            OnPropertyChanged(nameof(Productos));
        }

        private void LimpiarBusqueda(object obj)
        {
            TerminoBusqueda = "";
            OnPropertyChanged(nameof(TerminoBusqueda));
            CargarProductos();
        }

        private void CargarProductos()
        {
            Productos = new ObservableCollection<Producto>(_context.Productos.ToList());
            OnPropertyChanged(nameof(Productos));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
