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
    public class CajaViewModel : INotifyPropertyChanged
    {
        private readonly KioscoDbContext _context;

        public ObservableCollection<MovimientoCaja> MovimientosCaja { get; set; }

        private string _descripcion;
        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                OnPropertyChanged(nameof(Descripcion));
            }
        }

        private decimal _monto;
        public decimal Monto
        {
            get => _monto;
            set
            {
                _monto = value;
                OnPropertyChanged(nameof(Monto));
            }
        }

        public decimal TotalCaja => MovimientosCaja.Sum(m => m.Monto);

        public ICommand AgregarIngresoCommand { get; }
        public ICommand AgregarGastoCommand { get; }

        public CajaViewModel()
        {
            _context = App.AppHost.Services.GetService(typeof(KioscoDbContext)) as KioscoDbContext;

            MovimientosCaja = new ObservableCollection<MovimientoCaja>(_context.MovimientosCaja.OrderByDescending(m => m.Fecha).ToList());

            AgregarIngresoCommand = new RelayCommand(AgregarIngreso);
            AgregarGastoCommand = new RelayCommand(AgregarGasto);
        }

        private void AgregarIngreso(object obj)
        {
            if (Monto <= 0)
            {
                MessageBox.Show("El monto debe ser mayor a cero.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                MessageBox.Show("La descripción no puede estar vacía.");
                return;
            }

            var movimiento = new MovimientoCaja
            {
                Descripcion = Descripcion,
                Monto = Monto
            };

            _context.MovimientosCaja.Add(movimiento);
            _context.SaveChanges();

            MovimientosCaja.Insert(0, movimiento);

            MessageBox.Show("Ingreso registrado.");

            Descripcion = "";
            Monto = 0;

            OnPropertyChanged(nameof(TotalCaja));
            OnPropertyChanged(nameof(Descripcion));
            OnPropertyChanged(nameof(Monto));
        }

        private void AgregarGasto(object obj)
        {
            if (Monto <= 0)
            {
                MessageBox.Show("El monto debe ser mayor a cero.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                MessageBox.Show("La descripción no puede estar vacía.");
                return;
            }

            var movimiento = new MovimientoCaja
            {
                Descripcion = Descripcion,
                Monto = -Monto // gasto → monto negativo
            };

            _context.MovimientosCaja.Add(movimiento);
            _context.SaveChanges();

            MovimientosCaja.Insert(0, movimiento);

            MessageBox.Show("Gasto registrado.");

            Descripcion = "";
            Monto = 0;

            OnPropertyChanged(nameof(TotalCaja));
            OnPropertyChanged(nameof(Descripcion));
            OnPropertyChanged(nameof(Monto));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
