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
using System.Windows.Shapes;

namespace Sofware_CMJC_Version_1._0.Vistas
{
    /// <summary>
    /// Lógica de interacción para TipoAtencionDialog.xaml
    /// </summary>
    public partial class TipoAtencionDialog : Window
    {
        public string TipoAtencionSeleccionado { get; private set; }
        public TipoAtencionDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TipoAtencionSeleccionado = "Ambulatorio";
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TipoAtencionSeleccionado = "Emergencia";
            Close();
        }
    }
}
