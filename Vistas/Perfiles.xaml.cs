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
    /// Lógica de interacción para Perfiles.xaml
    /// </summary>
    public partial class Perfiles : UserControl
    {
        private PerfilesVM viewModel;
        public Perfiles(string userFullName)
        {
            InitializeComponent();
            viewModel = new PerfilesVM(userFullName);
            DataContext = viewModel;
        }

    }
}
