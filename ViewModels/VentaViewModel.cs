using KioscoApp.Data;
using KioscoApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using KioscoApp.Helpers;

namespace KioscoApp.ViewModels
{
    public class ItemVenta
    {
        public Producto Producto { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal Subtotal => Producto.PrecioVenta * Cantidad;
    }

    public class VentaViewModel : INotifyPropertyChanged
    {
        private readonly KioscoDbContext _context;
        private Producto? _productoSeleccionado;

        public ObservableCollection<ItemVenta> Carrito { get; set; } = new();
        public string CodigoBusqueda { get; set; } = string.Empty;
        public int CantidadSeleccionada { get; set; } = 1;

        public ICommand BuscarProductoCommand { get; }
        public ICommand AgregarAlCarritoCommand { get; }

        public VentaViewModel()
        {
            _context = new KioscoDbContext(
                App.AppHost.Services.GetService(typeof(DbContextOptions<KioscoDbContext>)) as DbContextOptions<KioscoDbContext>
            );

            BuscarProductoCommand = new RelayCommand(_ => BuscarProducto());
            AgregarAlCarritoCommand = new RelayCommand(_ => AgregarAlCarrito());
        }

        public Producto? ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                _productoSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalVenta => Carrito.Sum(x => x.Subtotal);

        private void BuscarProducto()
        {
            if (string.IsNullOrWhiteSpace(CodigoBusqueda))
                return;

            ProductoSeleccionado = _context.Productos
                .FirstOrDefault(p =>
                    p.CodigoBarras == CodigoBusqueda ||
                    p.Nombre.ToLower().Contains(CodigoBusqueda.ToLower()));
        }

        private void AgregarAlCarrito()
        {
            if (ProductoSeleccionado == null || CantidadSeleccionada < 1)
                return;

            var existente = Carrito.FirstOrDefault(x => x.Producto.Id == ProductoSeleccionado.Id);
            if (existente != null)
            {
                existente.Cantidad += CantidadSeleccionada;
            }
            else
            {
                Carrito.Add(new ItemVenta
                {
                    Producto = ProductoSeleccionado,
                    Cantidad = CantidadSeleccionada
                });
            }

            OnPropertyChanged(nameof(Carrito));
            OnPropertyChanged(nameof(TotalVenta));

            CantidadSeleccionada = 1;
            CodigoBusqueda = string.Empty;
            ProductoSeleccionado = null;

            OnPropertyChanged(nameof(CodigoBusqueda));
            OnPropertyChanged(nameof(CantidadSeleccionada));
            OnPropertyChanged(nameof(ProductoSeleccionado));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
