using Sofware_CMJC_Version_1._0.Vistas;
using Sofware_CMJC_Version_1._0.VistaModelo;
using System.Windows;
using System;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using CMJC.Migrations;
using System.Linq;

namespace Sofware_CMJC_Version_1._0
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application //, IServiceProvider
    {
       
       protected override void OnStartup(StartupEventArgs e)
       {
            
           base.OnStartup(e);
           try
           {
               // Verifica si hay al menos un usuario en la base de datos
               if (HayUsuariosEnBaseDeDatos())
               {
                   // Si hay usuarios, muestra la ventana de inicio de sesión
                   MostrarVentanaLogin();
               }
               else
               {
                   // Si no hay usuarios, muestra la ventana de registro (SignUp)
                   MostrarVentanaSignUp();
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show($"Error crítico al iniciar la aplicación: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               Shutdown();
           }
        }
        private bool HayUsuariosEnBaseDeDatos()
        {
            using (var contexto = new CMJC_Context()) 
            {
                try
                {
                    return contexto.Usuarios.Any(); // Verifica si hay al menos un usuario en la base de datos
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al consultar la base de datos: {ex.Message}", "Error de base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        private void MostrarVentanaLogin()
        {
            var loginWindow = new Login();
            loginWindow.Show();
        }

        private void MostrarVentanaSignUp()
        {
            var signUpWindow = new SignUp();

            // Suscribir al evento Closed para volver al Login cuando se cierre SignUp
            signUpWindow.Closed += (sender, args) => MostrarVentanaLogin();

            signUpWindow.Show();
        }
    }
}
