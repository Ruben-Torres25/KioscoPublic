using System.Windows;
using KioscoApp.Views;
using KioscoApp.ViewModels;

namespace KioscoApp
{
    public partial class MainWindow : Window
    {
        public ProductoViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new ProductoViewModel();
            DataContext = ViewModel;
        }

        private void AbrirVentas_Click(object sender, RoutedEventArgs e)
        {
            var ventanaVenta = new VentaView();
            ventanaVenta.ShowDialog();
        }

        private void AbrirCaja_Click(object sender, RoutedEventArgs e)
        {
            var ventanaCaja = new CajaView();
            ventanaCaja.ShowDialog();
        }

        private void AbrirClientes_Click(object sender, RoutedEventArgs e)
        {
            var ventanaClientes = new ClientesView();
            ventanaClientes.ShowDialog();
        }
    }
}
