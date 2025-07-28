using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.VistaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sofware_CMJC_Version_1._0.Vistas
{
    /// <summary>
    /// Lógica de interacción para Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public string UserFullName;
        public Menu(string userFullName)
        {
            InitializeComponent();
            UserFullNameTextBlock.Text = userFullName;
            UserFullName=userFullName;

        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void PnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);

        }
        private void PnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void PacientesButton_Click(object sender, RoutedEventArgs e)
        {
            // Crea una instancia del UserControl
            Pacientes pacientes = new Pacientes(UserFullName);

            // Asigna el UserControl al ContentControl en la vista
            Pacientes.Content = pacientes;
            Consultas.Content = null;
            HistoriasClinicas.Content = null;
            Perfiles.Content = null;
        }
        private void ConsultasButton_Click(object sender, RoutedEventArgs e)
        {
            // Crea una instancia del UserControl
            Consultas consulta = new Consultas(UserFullName);

            // Asigna el UserControl al ContentControl en la vista
            Pacientes.Content = null;
            Consultas.Content = consulta;
            HistoriasClinicas.Content = null;
            Perfiles.Content = null;
        }
        private void HistoriaButton_Click(object sender, RoutedEventArgs e)
        {
            // Crea una instancia del UserControl
            HistoriasClinicas historiaClinicas = new HistoriasClinicas(UserFullName);

            // Asigna el UserControl al ContentControl en la vista
            Pacientes.Content = null;
            Consultas.Content = null;
            HistoriasClinicas.Content = historiaClinicas;
            Perfiles.Content = null;
        }

        private void PerfilButton_Click(object sender, RoutedEventArgs e)
        {
            // Crea una instancia del UserControl
            Perfiles perfiles = new Perfiles(UserFullName);

            // Asigna el UserControl al ContentControl en la vista
            Pacientes.Content = null;
            Consultas.Content = null;
            HistoriasClinicas.Content = null;
            Perfiles.Content = perfiles;
        }
        private void CerrarSesionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Cierra la ventana actual (ventana de menú)
                this.Hide();

                // Crea una nueva instancia de la ventana Login y muéstrala
                var loginWindow = new Login();
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}", "Error");
            }
        }
    }
}
