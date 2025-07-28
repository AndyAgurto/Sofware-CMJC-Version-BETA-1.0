using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sofware_CMJC_Version_1._0.VistaModelo;


namespace Sofware_CMJC_Version_1._0.Vistas
{
    /// <summary>
    /// Lógica de interacción para Pacientes.xaml
    /// </summary>
    public partial class Pacientes : UserControl
    {
        private PacientesVM viewModel;
        public string UserFullName;
        public Pacientes(string userFullName)
        {
            InitializeComponent();
            UserFullName = userFullName;
            viewModel = new PacientesVM(UserFullName);
            DataContext = viewModel;

        }

        private void txtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Lógica para realizar la búsqueda cuando el texto cambie.
            viewModel.BuscarPacientes(null);

        }
    }
}
