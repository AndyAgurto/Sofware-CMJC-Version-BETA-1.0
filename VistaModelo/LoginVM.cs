using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Vistas;


namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class LoginVM : BaseVM
    {
        private Menu menuWindow;
        public string UserFullName;
        private string user;
        private string password;
        private ICommand loginCommand;


        public string LoginName
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged(nameof(LoginName));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new CommandVM(Login, CanLogin);
                }
                return loginCommand;
            }
        }

        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(LoginName) && !string.IsNullOrWhiteSpace(Password);
        }

        private void Login(object parameter)
        {
            using (var dbContext = new CMJC_Context()) 
            {
                var user = dbContext.Usuarios.FirstOrDefault(u => u.LoginName == LoginName);

                if (user != null)
                {
                    if (user.Estado == "Bloqueado")
                    {
                        MessageBox.Show("Usuario bloqueado, contacte con el administrador.");
                        return;
                    }

                    if (VerifyPassword(Password, user.PasswordHash, user.PasswordSalt))
                    {
                        UserFullName = $"{user.Nombres} {user.Apellidos}";
                        MessageBox.Show("Inicio de sesión exitoso");

                        var menuVM = new MenuVM(LoginName);

                        // Pasa el UserFullName al constructor de Menu
                        menuWindow = new Menu(UserFullName);
                        menuWindow.DataContext = menuVM;

                        // Cierra la ventana actual (Login)
                        var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                        if (currentWindow != null)
                        {
                            currentWindow.Close();
                        }

                        // Muestra la ventana Menu
                        menuWindow.Show();

                    }
                    else
                    {
                        // Las credenciales son incorrectas, muestra un mensaje de error al usuario.
                        MessageBox.Show("Nombre de usuario o contraseña incorrectos");
                    }
                }
                else
                {
                    // El usuario no existe, muestra un mensaje de error al usuario.
                    MessageBox.Show("El usuario no existe");
                }
            }
        }

        public LoginVM()
        {

        }

        public void RecuperarContrasena(string loginName)
        {
            try
            {
                using (var dbContext = new CMJC_Context())
                {
                    var user = dbContext.Usuarios.FirstOrDefault(u => u.LoginName == loginName);
                    if (user != null)
                    {
                        string recipient = user.Correo;
                        string newPassword = GenerateRandomPassword();

                        var (passwordHash, passwordSalt) = HashPassword(newPassword);
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                        dbContext.SaveChanges();

                        SendEmail(recipient, newPassword);
                    }
                    else
                    {
                        MessageBox.Show("El Usuario ingresado no fue encontrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string GenerateRandomPassword()
        {
            // Longitud deseada de la contraseña
            int length = 8;

            // Caracteres que se pueden utilizar en la contraseña
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";

            // Generador de números aleatorios seguro
            Random random = new Random();

            // Generar la contraseña aleatoria
            string password = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }

        private void SendEmail(string recipient, string newPassword)
        {
            try
            {
                using (var dbContext = new CMJC_Context())
                {
                    // Buscar al usuario por su dirección de correo electrónico o nombre de usuario
                    var user = dbContext.Usuarios.FirstOrDefault(u => u.Correo == recipient || u.LoginName == recipient);

                    if (user != null)
                    {
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress("CMJC Soporte.", user.Correo)); // Utiliza el correo electrónico del usuario
                        message.To.Add(new MailboxAddress("Andy Agurto", user.Correo)); // El destinatario es el usuario que solicitó la recuperación

                        message.Subject = "Recuperación de contraseña";

                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.TextBody = $"Tu nueva contraseña es: {newPassword}"; // 

                        message.Body = bodyBuilder.ToMessageBody();

                        using (var client = new SmtpClient())
                        {
                            client.Connect("smtp.gmail.com", 587, false); // Para Gmail
                                                                          //client.Connect("smtp.live.com", 587, false); // Para Hotmail (Outlook)
                            client.Authenticate("druagurto@gmail.com", "ubtx vair tfwe gnna"); // Cambia a tus credenciales
                            client.Send(message);
                            client.Disconnect(true);
                        }

                        MessageBox.Show("Se ha enviado un correo electrónico con la nueva contraseña.");
                    }
                    else
                    {
                        MessageBox.Show("El usuario no fue encontrado en la base de datos.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo electrónico: " + ex.Message);
            }
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

        private bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA256(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false; // Las contraseñas no coinciden.
                    }
                }
                return true; // Las contraseñas coinciden.
            }
        }
    }
}
