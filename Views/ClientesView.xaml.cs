using System.Windows;
using KioscoApp.ViewModels;

namespace KioscoApp.Views
{
    public partial class ClientesView : Window
    {
        public ClientesView()
        {
            InitializeComponent();
            DataContext = new ClientesViewModel();
        }
    }
}
