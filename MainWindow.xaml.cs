using System.Windows;
using KioscoApp.ViewModels;
using KioscoApp.Views;



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
    }
    private void AbrirVentas_Click(object sender, RoutedEventArgs e)
        {
            var ventanaVenta = new VentaView();
            ventanaVenta.ShowDialog();
        }
    }
