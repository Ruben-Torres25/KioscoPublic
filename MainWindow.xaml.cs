using System.Windows;
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
    }
}
