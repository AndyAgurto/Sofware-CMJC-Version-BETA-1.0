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
    public class EditarUsuarioVM : BaseVM
    {
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
        private ObservableCollection<string> tipoUsuarios;
        public ObservableCollection<string> TipoUsuarios
        {
            get => tipoUsuarios;
            set
            {
                tipoUsuarios = value;
                OnPropertyChanged(nameof(TipoUsuarios));
            }
        }
        private string loginName;
        public string LoginName
        {
            get => loginName;
            set
            {
                loginName = value;
                OnPropertyChanged(nameof(LoginName));
            }
        }
        private string nombres;
        public string Nombres
        {
            get => nombres;
            set
            {
                nombres = value;
                OnPropertyChanged(nameof(Nombres));
            }
        }
        private string apellidos;
        public string Apellidos
        {
            get => apellidos;
            set
            {
                apellidos = value;
                OnPropertyChanged(nameof(Apellidos));
            }
        }
        private string correo;
        public string Correo
        {
            get => correo;
            set
            {
                correo = value;
                OnPropertyChanged(nameof(Correo));
            }
        }
        private string tipoUsuario;
        public string TipoUsuario
        {
            get => tipoUsuario;
            set
            {
                tipoUsuario = value;
                OnPropertyChanged(nameof(TipoUsuario));
            }
        }
        private string nuevaContraseña;
        public string NuevaContraseña
        {
            get { return nuevaContraseña; }
            set
            {
                nuevaContraseña = value;
                OnPropertyChanged(nameof(NuevaContraseña));
            }
        }

        private string confirmarContraseña;
        public string ConfirmarContraseña
        {
            get { return confirmarContraseña; }
            set
            {
                confirmarContraseña = value;
                OnPropertyChanged(nameof(ConfirmarContraseña));
            }
        }
        public ICommand ActualizarCommand { get; private set; }
        public ICommand DesbloquearCommand { get; private set; }
        public EditarUsuarioVM(Usuario usuarioSeleccionado)
        {
            UsuarioSeleccionado = usuarioSeleccionado;

            CargarDatosDeUsuario(UsuarioSeleccionado);

           TipoUsuarios = new ObservableCollection<string>
            {
                "Usuario Estandar",
                "Administrador",
            };
            ActualizarCommand = new CommandVM(Actualizar);
            DesbloquearCommand = new CommandVM(Desbloquear);
        }
        private void CargarDatosDeUsuario(Usuario usuarioSeleccionado)
        {
            LoginName= usuarioSeleccionado.LoginName;
            Nombres = usuarioSeleccionado.Nombres;
            Apellidos = usuarioSeleccionado.Apellidos;
            TipoUsuario = usuarioSeleccionado.TipoUsuario;
            Correo = usuarioSeleccionado.Correo;

        }
        private void Actualizar(object parameter)
        {
            try
            {
                // Verificar si se ingresó una nueva contraseña
                if (!string.IsNullOrEmpty(NuevaContraseña) && !string.IsNullOrEmpty(ConfirmarContraseña))
                {
                    // Verificar si las contraseñas coinciden
                    if (NuevaContraseña == ConfirmarContraseña)
                    {
                        // Generar hash y salt para la nueva contraseña
                        using (var hmac = new System.Security.Cryptography.HMACSHA512())
                        {
                            UsuarioSeleccionado.PasswordSalt = hmac.Key;
                            UsuarioSeleccionado.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(NuevaContraseña));
                        }
                    }
                    else
                    {
                        // Mostrar mensaje de error si las contraseñas no coinciden
                        MessageBox.Show("Las contraseñas no coinciden. Inténtelo de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Aquí deberías implementar la lógica para actualizar el usuario en la base de datos
                using (var dbContext = new CMJC_Context())
                {
                    // Actualiza el usuario en la base de datos
                    dbContext.Entry(UsuarioSeleccionado).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }

                // Cierra la ventana después de actualizar los cambios
                if (parameter is Window window)
                {
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes mostrar un mensaje, registrar el error, etc.)
                MessageBox.Show($"Error al actualizar los cambios: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Desbloquear(object parameter)
        {
            try
            {       // Verificar si el usuario ya está desbloqueado
                    if (UsuarioSeleccionado.Estado == "Bloqueado")
                    {
                        // Actualizar el estado del usuario a "Desbloqueado"
                        UsuarioSeleccionado.Estado = "Desbloqueado";

                        // Aquí deberías implementar la lógica para actualizar el usuario en la base de datos
                        using (var dbContext = new CMJC_Context())
                        {
                            // Actualiza el usuario en la base de datos
                            dbContext.Entry(UsuarioSeleccionado).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        }

                        // Notificar al usuario que el usuario ha sido desbloqueado
                        MessageBox.Show("Usuario desbloqueado correctamente.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Este usuario no está bloqueado.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes mostrar un mensaje, registrar el error, etc.)
                MessageBox.Show($"Error al desbloquear el usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
