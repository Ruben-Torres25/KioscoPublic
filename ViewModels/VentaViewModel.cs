using System;
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
    public class VentaViewModel : INotifyPropertyChanged
    {
        private readonly KioscoDbContext _context;

        public ObservableCollection<Producto> Productos { get; set; }
        public ObservableCollection<Venta> VentasRealizadas { get; set; }

        private Producto _productoSeleccionado;
        public Producto ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                _productoSeleccionado = value;
                OnPropertyChanged(nameof(ProductoSeleccionado));
            }
        }

        public int CantidadVenta { get; set; }
        public string CodigoBusqueda { get; set; }
        public string MetodoPago { get; set; } = "Efectivo";

        public ICommand BuscarProductoCommand { get; }
        public ICommand RegistrarVentaCommand { get; }

        public VentaViewModel()
        {
            _context = App.AppHost.Services.GetService(typeof(KioscoDbContext)) as KioscoDbContext;

            Productos = new ObservableCollection<Producto>(_context.Productos.ToList());
            VentasRealizadas = new ObservableCollection<Venta>();

            BuscarProductoCommand = new RelayCommand(BuscarProducto);
            RegistrarVentaCommand = new RelayCommand(RegistrarVenta);
        }

        private void BuscarProducto(object obj)
        {
            if (string.IsNullOrWhiteSpace(CodigoBusqueda))
            {
                MessageBox.Show("Ingresa un código de barras.");
                return;
            }

            ProductoSeleccionado = _context.Productos.FirstOrDefault(p => p.CodigoBarras == CodigoBusqueda);

            if (ProductoSeleccionado == null)
            {
                MessageBox.Show("Producto no encontrado.");
            }

            OnPropertyChanged(nameof(ProductoSeleccionado));
        }

        private void RegistrarVenta(object obj)
        {
            if (ProductoSeleccionado == null || CantidadVenta <= 0)
            {
                MessageBox.Show("Selecciona un producto válido y una cantidad mayor a cero.");
                return;
            }

            if (ProductoSeleccionado.Stock < CantidadVenta)
            {
                MessageBox.Show("Stock insuficiente.");
                return;
            }

            // Registrar venta
            var venta = new Venta
            {
                ProductoId = ProductoSeleccionado.Id,
                Cantidad = CantidadVenta,
                PrecioUnitario = ProductoSeleccionado.PrecioVenta,
                MetodoPago = MetodoPago
            };

            _context.Ventas.Add(venta);

            // Descontar stock
            ProductoSeleccionado.Stock -= CantidadVenta;
            _context.Productos.Update(ProductoSeleccionado);

            // Registrar movimiento caja
            var movimiento = new MovimientoCaja
            {
                Descripcion = $"Venta de {ProductoSeleccionado.Nombre} x {CantidadVenta}",
                Monto = venta.Total
            };
            _context.MovimientosCaja.Add(movimiento);

            _context.SaveChanges();

            MessageBox.Show("Venta registrada correctamente!");

            // Refrescar listas
            VentasRealizadas.Add(venta);
            Productos = new ObservableCollection<Producto>(_context.Productos.ToList());

            // Limpiar
            CantidadVenta = 0;
            CodigoBusqueda = "";
            ProductoSeleccionado = null;

            OnPropertyChanged(nameof(VentasRealizadas));
            OnPropertyChanged(nameof(Productos));
            OnPropertyChanged(nameof(ProductoSeleccionado));
            OnPropertyChanged(nameof(CantidadVenta));
            OnPropertyChanged(nameof(CodigoBusqueda));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
