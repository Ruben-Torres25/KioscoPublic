using System.Windows;
using KioscoApp.ViewModels;

namespace KioscoApp.Views
{
    public partial class CajaView : Window
    {
        public CajaView()
        {
            InitializeComponent();
            DataContext = new CajaViewModel();
        }
    }
}
