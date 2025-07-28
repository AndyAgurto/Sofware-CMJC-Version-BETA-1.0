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
    /// Lógica de interacción para VentanaInformacionReceta.xaml
    /// </summary>
    public partial class VentanaInformacionReceta : Window
    {
        // Propiedades públicas para almacenar la información ingresada
        public DateTime FechaCaducidad { get; private set; }
        public string NombreArchivo { get; private set; }
        public string TipoMIME { get; private set; }

        public VentanaInformacionReceta()
        {
            InitializeComponent();
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar y guardar la información ingresada
            if (DateTime.TryParse(txtFechaCaducidad.Text, out DateTime fechaCaducidad))
            {
                FechaCaducidad = fechaCaducidad;
                NombreArchivo = txtNombreArchivo.Text;
                TipoMIME = txtTipoMIME.Text;

                // Indicar que la ventana se cerró correctamente
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Formato de fecha no válido. Por favor, ingrese una fecha válida.", "Error");
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // Indicar que la ventana se cerró sin guardar
            DialogResult = false;
        }
    }
}
