using Microsoft.EntityFrameworkCore;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.Vistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class PerfilesVM : BaseVM
    {
        private SignUp signUpWindow;
        private EditarUsuario editUsuarioWindow;
        private ListaHU listaHUWindow;

        private string userFullName;
        public string UserFullName
        {
            get { return userFullName; }
            set
            {
                userFullName = value;
                OnPropertyChanged(nameof(UserFullName));
            }
        }
        private ObservableCollection<Usuario> usuarios;

        private ObservableCollection<Usuario> usuarioFiltrados;
        public ObservableCollection<Usuario> UsuarioFiltrados
        {
            get { return usuarioFiltrados; }
            set
            {
                usuarioFiltrados = value;
                OnPropertyChanged(nameof(UsuarioFiltrados));
            }
        }
        private string textBusqueda;
        public string TextBusqueda
        {
            get { return textBusqueda; }
            set
            {
                if (textBusqueda != value)
                {
                    textBusqueda = value;
                    OnPropertyChanged(nameof(TextBusqueda));
                    // Llamar a tu lógica de búsqueda aquí
                    FiltrarPerfiles();
                }
            }
        }
        private Usuario usuarioSeleccionado;
        public Usuario UsuarioSeleccionado
        {
            get { return usuarioSeleccionado; }
            set
            {
                usuarioSeleccionado = value;
                OnPropertyChanged(nameof(UsuarioSeleccionado));
            }
        }
        public ICommand NuevoUsuarioCommand { get; private set; }
        public ICommand EditarUsuarioCommand { get; private set; }
        public ICommand BloquearUsuarioCommand { get; private set; }
        public ICommand HistoriaUsuarioCommand { get; private set; }
        public PerfilesVM(string userFullName)
        {
            UserFullName = userFullName;
            CargarUsuariosDesdeBaseDeDatos();

            NuevoUsuarioCommand = new CommandVM(NuevoUsuario);
            EditarUsuarioCommand = new CommandVM(EditarUsuario);
            BloquearUsuarioCommand = new CommandVM(BloquearUsuario);
            HistoriaUsuarioCommand = new CommandVM(VerListaHU);
        }
        private void CargarUsuariosDesdeBaseDeDatos()
        {
            UsuarioFiltrados = new ObservableCollection<Usuario>();
            using (var dbContext = new CMJC_Context())
            {
                usuarios = new ObservableCollection<Usuario>(dbContext.Usuarios.ToList());
                UsuarioFiltrados = usuarios;
            }

        }
        //Metodod de Busqueda
        private void FiltrarPerfiles()
        {
            if (!string.IsNullOrWhiteSpace(TextBusqueda))
            {
                // Filtra los ususarios según el TextoBusqueda
                UsuarioFiltrados = new ObservableCollection<Usuario>(
                    usuarios
                        .Where(c =>
                            c.LoginName.Contains(TextBusqueda, StringComparison.OrdinalIgnoreCase) ||
                            c.Nombres.Contains(TextBusqueda, StringComparison.OrdinalIgnoreCase) ||
                            c.Apellidos.Contains(TextBusqueda, StringComparison.OrdinalIgnoreCase))
                        .ToList());
            }
            else
            {
                // Si el texto de búsqueda está vacío, muestra todos los Usuarios
                UsuarioFiltrados = usuarios;
            }
        }
        private void NuevoUsuario(object parameter)
        {
            signUpWindow = new SignUp();

            // Obtener la ventana Menu desde las ventanas abiertas
            var menuWindow = Application.Current.Windows.OfType<Menu>().FirstOrDefault();

            if (menuWindow != null)
            {
                // Ocultar la ventana principal (Menu)
                menuWindow.Hide();

                signUpWindow.ShowDialog();

                menuWindow.Show();
            }
        }
        private void EditarUsuario(object parameter)
        {
            if (UsuarioSeleccionado != null)
            {
                // Obtener la ViewModel de EditUsuarioVM desde la ventana EditUsuario
                var editUsuarioViewModel = new EditarUsuarioVM(UsuarioSeleccionado);
                editUsuarioWindow = new EditarUsuario(editUsuarioViewModel);

                // Obtener la ventana Menu desde las ventanas abiertas
                var menuWindow = Application.Current.Windows.OfType<Menu>().FirstOrDefault();

                if (menuWindow != null)
                {
                    // Ocultar la ventana principal (Menu)
                    menuWindow.Hide();

                    // Mostrar la ventana EditUsuario
                    editUsuarioWindow.ShowDialog();

                    // Cuando la ventana EditUsuario se cierre, mostrar nuevamente la ventana Menu
                    menuWindow.Show();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un Usuario para editar.", "Mensaje");
            }
        }
        private void BloquearUsuario(object parameter)
        {
            if (UsuarioSeleccionado != null)
            {
                // Verificar si el usuario ya está bloqueado
                if (UsuarioSeleccionado.Estado != "Bloqueado")
                {
                    // Actualizar el estado del usuario a "Bloqueado"
                    UsuarioSeleccionado.Estado = "Bloqueado";

                    try
                    {
                        using (var dbContext = new CMJC_Context())
                        {
                            // Actualiza el usuario en la base de datos
                            dbContext.Entry(UsuarioSeleccionado).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        }

                        // Notificar al usuario que el usuario ha sido bloqueado
                        MessageBox.Show("Usuario bloqueado correctamente.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores (puedes mostrar un mensaje, registrar el error, etc.)
                        MessageBox.Show($"Error al bloquear el usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Este usuario ya está bloqueado.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un Usuario...", "Mensaje");
            }
        }
        private void VerListaHU(object parameter) 
        {
            listaHUWindow = new ListaHU(userFullName);

            // Obtener la ventana Menu desde las ventanas abiertas
            var menuWindow = Application.Current.Windows.OfType<Menu>().FirstOrDefault();

            if (menuWindow != null)
            {
                // Ocultar la ventana principal (Menu)
                menuWindow.Hide();

                listaHUWindow.ShowDialog();

                menuWindow.Show();
            }
        }
    }
}
