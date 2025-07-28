using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.VistaModelo;

namespace Sofware_CMJC_Version_1._0.Vistas
{
    /// <summary>
    /// Lógica de interacción para FormularioHistoriaSecundaria.xaml
    /// </summary>
    public partial class FormularioHistoriaSecundaria : Window
    {
        public FormularioHistoriaSecundaria()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private void PnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ReleaseCapture();
            WindowInteropHelper helper = new WindowInteropHelper(this);

            // Deshabilitar la maximización temporalmente
            this.ResizeMode = ResizeMode.NoResize;

            SendMessage(helper.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void PnlControlBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Restaurar la capacidad de cambiar el tamaño al soltar el ratón
            this.ResizeMode = ResizeMode.CanResize;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea regresar al menu? Los datos no guardados se perderán.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // Cierra la ventana
                this.Close();
            }
        }

        private void CantidadTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Limpiar paneles existentes
            DiagnosticoStackPanel.Children.Clear();
            // Obtener el texto ingresado
            string cantidadText = CantidadTextBox.Text;

            // Verificar si el texto es vacío
            if (string.IsNullOrWhiteSpace(cantidadText))
            {
            }
            else if (!int.TryParse(cantidadText, out int cantidadDiagnostico) || cantidadDiagnostico <= 0 || cantidadDiagnostico > 4)
            {
                // Mostrar mensaje de error
                MessageBox.Show("Por favor, ingrese un número válido entre 1 y 4 diagnósticos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Resto del código sigue igual
                char[] letras = { 'P', 'D', 'R' }; // Puedes personalizar las letras según tus necesidades

                for (int i = 0; i < cantidadDiagnostico; i++)
                {
                    // Crear un nuevo UniformGrid para cada conjunto de diagnóstico
                    UniformGrid diagnosticoGrid = new UniformGrid
                    {
                        Columns = 5, // 3 columnas para los TextBox y 3 para los CheckBox
                        Margin = new Thickness(10, 0, 0, 10),
                    };

                    // TextBox para CIE10
                    TextBox cie10TextBox = new TextBox
                    {
                        Foreground = Brushes.Black,
                        Width = 80,
                        FontSize = 16,
                        Height = 20,
                        Margin = new Thickness(-100, 0, 0, 10),
                        FontFamily = (FontFamily)FindResource("FranklinGothicFont")
                    };

                    // TextBox para Descripción
                    TextBox descripcionTextBox = new TextBox
                    {
                        Foreground = Brushes.Black,
                        Width = 350,
                        FontSize = 16,
                        Height = 20,
                        Margin = new Thickness(-55, 0, 0, 10),
                        FontFamily = (FontFamily)FindResource("FranklinGothicFont")
                    };

                    // Agregar controles al UniformGrid
                    diagnosticoGrid.Children.Add(cie10TextBox);
                    diagnosticoGrid.Children.Add(descripcionTextBox);

                    // Agregar CheckBox para cada tipo de diagnóstico (P, D, R)
                    foreach (char letra in letras)
                    {
                        CheckBox tipoDiagnosticoCheckBox = new CheckBox
                        {
                            Content = letra.ToString(),
                            FontSize = 16,
                            Width = 50,
                            FontFamily = (FontFamily)FindResource("FranklinGothicFont"),
                            Margin = new Thickness(50, 0, 0, 0),
                        };

                        diagnosticoGrid.Children.Add(tipoDiagnosticoCheckBox);
                    }
                    // Agregar el UniformGrid al StackPanel principal
                    DiagnosticoStackPanel.Children.Add(diagnosticoGrid);
                }
            }
        }

        private void GuardarConsulta_Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (FormularioHistoriaSecundariaVM)DataContext;
            viewModel.GuardarDiagnosticos(DiagnosticoStackPanel.Children.OfType<UniformGrid>().ToList());

            GuardarConsulta.Visibility = Visibility.Hidden;
        }
    }
}
