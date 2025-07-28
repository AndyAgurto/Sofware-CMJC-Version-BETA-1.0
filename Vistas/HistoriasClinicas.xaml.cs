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
    /// Lógica de interacción para HistoriasClinicas.xaml
    /// </summary>
    public partial class HistoriasClinicas : UserControl
    {
        public string UserFullName;
        private HistoriaClinicaVM viewModel;
        public HistoriasClinicas(string userFullName)
        {
            InitializeComponent();
            UserFullName = userFullName;
            viewModel = new HistoriaClinicaVM(UserFullName);
            DataContext = viewModel;
        }


    }
}
