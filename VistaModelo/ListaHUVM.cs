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
    public class ListaHUVM : BaseVM
    {
        private string busqueda;
        public string Busqueda
        {
            get { return busqueda; }
            set
            {
                busqueda = value;
                OnPropertyChanged(nameof(Busqueda));
                // Llamar al método de búsqueda cada vez que cambia el criterio
                BuscarUsuarios(null);
            }
        }
        public ObservableCollection<Usuario> TodosLosUsuarios { get; set; }
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
        public ICommand VolverAlMenuCommand { get; private set; }
        public ListaHUVM()
        {
            TodosLosUsuarios = new ObservableCollection<Usuario>();

            // Llena la lista con datos de la base de datos
            CargarUsuariosDesdeBaseDeDatos();
            VolverAlMenuCommand = new CommandVM(VolverAlMenu);
        }
        // Método de búsqueda
        public void BuscarUsuarios(object parameter)
        {
            string criterioBusqueda = Busqueda.ToLower();

            if (string.IsNullOrWhiteSpace(criterioBusqueda))
            {
                CargarUsuariosDesdeBaseDeDatos();
            }
            else
            {
                // Filtrar los usuarios según el criterio de búsqueda.
                var resultadosDeBusqueda = TodosLosUsuarios
                    .Where(p =>
                        p.LoginName.ToLower().Contains(criterioBusqueda) ||
                        p.Nombres.ToLower().Contains(criterioBusqueda) ||
                        p.Apellidos.ToLower().Contains(criterioBusqueda)
                    )
                    .ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Limpiar la colección actual y agregar los resultados de la búsqueda.
                    TodosLosUsuarios.Clear();
                    foreach (var usuario in resultadosDeBusqueda)
                    {
                        TodosLosUsuarios.Add(usuario);
                    }
                });
            }
        }
        private void CargarUsuariosDesdeBaseDeDatos()
        {
            using (var dbContext = new CMJC_Context())
            {
                // Realiza la consulta a la base de datos para obtener los usuarios
                var usuariosDesdeDB = dbContext.Usuarios
                    .Include(u => u.HistoriasUsuarios)  // Incluye las historias relacionadas
                    .ToList();

                // Limpia la colección existente (si la hay)
                TodosLosUsuarios.Clear();

                // Agrega los usuarios obtenidos a la colección
                foreach (var usuario in usuariosDesdeDB)
                {
                    TodosLosUsuarios.Add(usuario);
                }
            }
        }
        private void VolverAlMenu(object parameter)
        {
           
            var listaHUWindow = Application.Current.Windows.OfType<ListaHU>().FirstOrDefault();
             if (listaHUWindow != null)
              {
                    listaHUWindow.Close();
              }
        }
    }
}
