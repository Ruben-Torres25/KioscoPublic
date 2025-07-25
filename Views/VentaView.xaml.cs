using System.Windows;
using KioscoApp.ViewModels;

namespace KioscoApp.Views
{
    public partial class VentaView : Window
    {
        public VentaView()
        {
            InitializeComponent();
            DataContext = new VentaViewModel();
        }
    }
}
