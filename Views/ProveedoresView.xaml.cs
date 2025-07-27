using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KioscoApp.Data;
using KioscoApp.Models;

namespace KioscoApp.Views
{
    public partial class ProveedoresView : Window
    {
        private readonly KioscoDbContext _db;
        private Proveedor _proveedorSeleccionado;

        public ProveedoresView()
        {
            InitializeComponent();
            _db = new KioscoDbContext(App.AppHost.Services.GetService(typeof(KioscoDbContext)) as DbContextOptions<KioscoDbContext>);
            CargarProveedores();
        }

        private void CargarProveedores()
        {
            ProveedoresDataGrid.ItemsSource = _db.Proveedores.ToList();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            var proveedor = new Proveedor
            {
                Nombre = NombreTextBox.Text,
                CUIT = CuitTextBox.Text,
                Contacto = ContactoTextBox.Text
            };

            _db.Proveedores.Add(proveedor);
            _db.SaveChanges();
            CargarProveedores();
            LimpiarCampos();
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (_proveedorSeleccionado == null) return;

            _proveedorSeleccionado.Nombre = NombreTextBox.Text;
            _proveedorSeleccionado.CUIT = CuitTextBox.Text;
            _proveedorSeleccionado.Contacto = ContactoTextBox.Text;

            _db.SaveChanges();
            CargarProveedores();
            LimpiarCampos();
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (_proveedorSeleccionado == null) return;

            _db.Proveedores.Remove(_proveedorSeleccionado);
            _db.SaveChanges();
            CargarProveedores();
            LimpiarCampos();
        }

        private void ProveedoresDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _proveedorSeleccionado = ProveedoresDataGrid.SelectedItem as Proveedor;
            if (_proveedorSeleccionado != null)
            {
                NombreTextBox.Text = _proveedorSeleccionado.Nombre;
                CuitTextBox.Text = _proveedorSeleccionado.CUIT;
                ContactoTextBox.Text = _proveedorSeleccionado.Contacto;
            }
        }

        private void LimpiarCampos()
        {
            NombreTextBox.Text = "";
            CuitTextBox.Text = "";
            ContactoTextBox.Text = "";
            _proveedorSeleccionado = null;
        }
    }
}
