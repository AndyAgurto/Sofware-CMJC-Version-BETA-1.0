using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using System;
using System.Linq;
using System.Windows;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class MenuVM:BaseVM
    {
        private bool esAdministrador;
        private string loginName;
        private Visibility perfilVisible;

        public MenuVM(string loginName)
        {
            this.loginName = loginName;
            EsAdministrador = DetermineIfUserIsAdmin();
            
        }

        private bool DetermineIfUserIsAdmin()
        {
            using (var dbContext = new CMJC_Context())
            {
                var user = dbContext.Usuarios.FirstOrDefault(u => u.LoginName == loginName);

                if (user != null)
                {
                    // Verifica si el valor de TipoUser es "Administrador" para determinar si es un administrador.
                    return user.TipoUsuario.Contains("Administrador");
                }
                else
                {
                    return false; // Por defecto, no es administrador.
                }
            }
        }

        public bool EsAdministrador
        {
            get { return esAdministrador; }
            set
            {
                esAdministrador = value;
                OnPropertyChanged(nameof(EsAdministrador));
                // Actualiza la visibilidad del perfil basado en si es administrador o no
                if (EsAdministrador==true)
                {
                    PerfilVisible=Visibility.Visible;
                }
                else
                {
                    PerfilVisible = Visibility.Hidden;
                }
                
            }
        }

        public Visibility PerfilVisible
        {
            get { return perfilVisible; }
            set
            {
                perfilVisible = value;
                OnPropertyChanged(nameof(PerfilVisible));
            }
        }
    }

}
