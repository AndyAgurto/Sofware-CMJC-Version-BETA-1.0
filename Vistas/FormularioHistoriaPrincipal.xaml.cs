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
    /// Lógica de interacción para FormularioHistoriaPrincipal.xaml
    /// </summary>
    public partial class FormularioHistoriaPrincipal : Window
    {
        public FormularioHistoriaPrincipal()
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
            this.Close();
        }

    }
}
