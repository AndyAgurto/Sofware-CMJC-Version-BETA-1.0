using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class SignUpVM : BaseVM
    {
        private ObservableCollection<string> tipoUsuarios;
        private string tipoUsuario;
        private string loginName;
        private string password;
        private string nombres;
        private string apellidos;
        private string correo;

        public ObservableCollection<string> TipoUsuarios
        {
            get => tipoUsuarios;
            set
            {
                tipoUsuarios = value;
                OnPropertyChanged(nameof(TipoUsuarios));
            }
        }

        public string TipoUsuario
        {
            get => tipoUsuario;
            set
            {
                tipoUsuario = value;
                OnPropertyChanged(nameof(TipoUsuario));
            }
        }

        public string LoginName
        {
            get => loginName;
            set
            {
                loginName = value;
                OnPropertyChanged(nameof(LoginName));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Nombres
        {
            get => nombres;
            set
            {
                nombres = value;
                OnPropertyChanged(nameof(Nombres));
            }
        }

        public string Apellidos
        {
            get => apellidos;
            set
            {
                apellidos = value;
                OnPropertyChanged(nameof(Apellidos));
            }
        }

        public string Correo
        {
            get => correo;
            set
            {
                correo = value;
                OnPropertyChanged(nameof(Correo));
            }
        }

        public SignUpVM()
        {
            TipoUsuarios = new ObservableCollection<string>
            {
                "Usuario Estandar",
                "Administrador",
            };

            SignUpCommand = new CommandVM(SignUp, CanRegister);
        }

        public ICommand SignUpCommand { get; private set; }

        private bool CanRegister(object parameter)
        {
            return !string.IsNullOrWhiteSpace(LoginName) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(Nombres) &&
                   !string.IsNullOrWhiteSpace(Apellidos) &&
                   !string.IsNullOrWhiteSpace(TipoUsuario) &&
                   !string.IsNullOrWhiteSpace(Correo);
        }

        private void SignUp(object parameter)
        {
            if (!IsPasswordValid(Password))
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres, un dígito y un carácter especial.");
                return;
            }

            var (passwordHash, passwordSalt) = HashPassword(Password);

            using (var dbContext = new CMJC_Context())
            {
                if (dbContext.Usuarios.Any(u => u.LoginName == LoginName))
                {
                    MessageBox.Show("Ya existe un usuario con este nombre de usuario. Por favor, elige otro nombre.");
                    return;
                }

                var newUsuario = new Usuario
                {
                    LoginName = LoginName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Nombres = Nombres,
                    Apellidos = Apellidos,
                    TipoUsuario = TipoUsuario,
                    Correo = Correo,
                    Estado = "Activado",
                    NHistoria=0
                };

                dbContext.Usuarios.Add(newUsuario);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Registro exitoso");
        }

        private (byte[] PasswordHash, byte[] PasswordSalt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var salt = hmac.Key;
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (hash, salt);
            }
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length >= 6 && password.Any(char.IsDigit) && password.Any(IsSpecialCharacter);
        }

        private bool IsSpecialCharacter(char c)
        {
            string specialCharacters = "!@#$%^&*()_+";
            return specialCharacters.Contains(c);
        }
    }
}
