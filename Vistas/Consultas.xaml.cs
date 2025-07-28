using Sofware_CMJC_Version_1._0.VistaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sofware_CMJC_Version_1._0.Vistas
{
    /// <summary>
    /// Lógica de interacción para Consultas.xaml
    /// </summary>
    public partial class Consultas : UserControl
    {
        private ConsultasVM viewModel;
        public string UserFullName;
        public Consultas(string userFullName)
        {
            InitializeComponent();
            UserFullName = userFullName;
            viewModel = new ConsultasVM(UserFullName);
            DataContext = viewModel;
        }

    }
}
