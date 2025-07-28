using Sofware_CMJC_Version_1._0.VistaModelo;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sofware_CMJC_Version_1._0.Vistas
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            var viewModel = new LoginVM();
            DataContext = viewModel;

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginVM viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }
        private void Recuperar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginVM viewModel)
            {
                string loginName = Microsoft.VisualBasic.Interaction.InputBox("Por favor, ingrese su Usuario:", "Recuperar contraseña");
                viewModel.RecuperarContrasena(loginName);
            }
        }
    }
}
