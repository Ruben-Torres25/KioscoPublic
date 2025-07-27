using KioscoApp.Data;
using KioscoApp.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;

namespace KioscoApp.Views
{
    public partial class ClientesView : Window
    {
        private readonly KioscoDbContext _context;

        public ClientesView()
        {
            InitializeComponent();

            // Inyección de dependencias correctamente
            _context = App.AppHost.Services.GetRequiredService<KioscoDbContext>();

            CargarClientes();
        }

        private void CargarClientes()
        {
            var clientes = _context.Clientes.ToList();
            ClientesDataGrid.ItemsSource = clientes;
        }

        private void GuardarCliente_Click(object sender, RoutedEventArgs e)
        {
            var cliente = new Cliente
            {
                Nombre = NombreTextBox.Text,
                CUIT = CuitTextBox.Text,
                Direccion = DireccionTextBox.Text,
                CondicionFiscal = CondicionFiscalTextBox.Text
            };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            MessageBox.Show("Cliente guardado correctamente.");
            CargarClientes();
        }

        private Cliente? clienteSeleccionado = null;

        private void ClientesDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            clienteSeleccionado = ClientesDataGrid.SelectedItem as Cliente;
            if (clienteSeleccionado != null)
            {
                NombreTextBox.Text = clienteSeleccionado.Nombre;
                CuitTextBox.Text = clienteSeleccionado.CUIT;
                DireccionTextBox.Text = clienteSeleccionado.Direccion;
                CondicionFiscalTextBox.Text = clienteSeleccionado.CondicionFiscal;
            }
        }

        private void EditarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (clienteSeleccionado == null)
            {
                MessageBox.Show("Seleccione un cliente de la lista para editar.");
                return;
            }

            clienteSeleccionado.Nombre = NombreTextBox.Text;
            clienteSeleccionado.CUIT = CuitTextBox.Text;
            clienteSeleccionado.Direccion = DireccionTextBox.Text;
            clienteSeleccionado.CondicionFiscal = CondicionFiscalTextBox.Text;

            _context.Clientes.Update(clienteSeleccionado);
            _context.SaveChanges();

            MessageBox.Show("Cliente actualizado correctamente.");
            CargarClientes();

            // Limpiar formulario
            NombreTextBox.Clear();
            CuitTextBox.Clear();
            DireccionTextBox.Clear();
            CondicionFiscalTextBox.Clear();
            clienteSeleccionado = null;
        }
        private void EliminarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (clienteSeleccionado == null)
            {
                MessageBox.Show("Seleccione un cliente de la lista para eliminar.");
                return;
            }

            var resultado = MessageBox.Show("¿Está seguro que desea eliminar este cliente?", "Confirmar eliminación", MessageBoxButton.YesNo);
            if (resultado == MessageBoxResult.Yes)
            {
                _context.Clientes.Remove(clienteSeleccionado);
                _context.SaveChanges();

                MessageBox.Show("Cliente eliminado correctamente.");
                CargarClientes();

                // Limpiar formulario
                NombreTextBox.Clear();
                CuitTextBox.Clear();
                DireccionTextBox.Clear();
                CondicionFiscalTextBox.Clear();
                clienteSeleccionado = null;
            }
        }


    }
}
