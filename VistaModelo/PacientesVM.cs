using Microsoft.EntityFrameworkCore;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.Vistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class PacientesVM : BaseVM
    {
        private AddPaciente addPacienteWindow;
        private EditPaciente editPacienteWindow;

        private string busqueda;
        private Paciente pacienteSeleccionado;
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
        public ObservableCollection<Paciente> TodosLosPacientes { get; set; }

        public string Busqueda
        {
            get { return busqueda; }
            set
            {
                busqueda = value;
                OnPropertyChanged(nameof(Busqueda));
                // Llamar al método de búsqueda cada vez que cambia el criterio
                BuscarPacientes(null);
            }
        }
        public Paciente PacienteSeleccionado
        {
            get { return pacienteSeleccionado; }
            set
            {
                pacienteSeleccionado = value;
                OnPropertyChanged(nameof(PacienteSeleccionado));
            }
        }
        public ICommand AgregarPacienteCommand { get; private set; }

        public ICommand EditarPacienteCommand { get; private set; }
        public ICommand EliminarPacienteCommand { get; private set; }

        public PacientesVM(string userFullName)
        {
            UserFullName = userFullName;

            AgregarPacienteCommand = new CommandVM(AgregarPaciente);
            EditarPacienteCommand = new CommandVM(EditarPaciente, PuedeEditarPaciente);
            EliminarPacienteCommand = new CommandVM(EliminarPaciente, PuedeEliminarPaciente);

            TodosLosPacientes = new ObservableCollection<Paciente>();

            // Llena la lista con datos de la base de datos
            CargarPacientesDesdeBaseDeDatos();
        }
        private void CargarPacientesDesdeBaseDeDatos()
        {
            using (var dbContext = new CMJC_Context()) 
            {
                // Realiza la consulta a la base de datos para obtener los pacientes
                var pacientesDesdeDB = dbContext.Pacientes.ToList();

                // Limpia la colección existente (si la hay)
                TodosLosPacientes.Clear();

                // Agrega los pacientes obtenidos a la colección
                foreach (var paciente in pacientesDesdeDB)
                {
                    TodosLosPacientes.Add(paciente);
                }
            }
        }
        // Método de búsqueda
        public void BuscarPacientes(object parameter)
        {
            string criterioBusqueda = Busqueda.ToLower();

            if (string.IsNullOrWhiteSpace(criterioBusqueda))
            {
                CargarPacientesDesdeBaseDeDatos();
            }
            else
            {
                // Filtrar los pacientes según el criterio de búsqueda.
                var resultadosDeBusqueda = TodosLosPacientes
                    .Where(p =>
                        p.DIdentidad.ToLower().Contains(criterioBusqueda) ||
                        p.Nombres.ToLower().Contains(criterioBusqueda) ||
                        p.Apellidos.ToLower().Contains(criterioBusqueda)
                    )
                    .ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Limpiar la colección actual y agregar los resultados de la búsqueda.
                    TodosLosPacientes.Clear();
                    foreach (var paciente in resultadosDeBusqueda)
                    {
                        TodosLosPacientes.Add(paciente);
                    }
                });
            }
        }
        private void EliminarPaciente(object parameter)
        {
            if (PacienteSeleccionado != null)
            {
                try
                {
                    // Guarda el ID_Paciente antes de eliminar el paciente de la lista
                    int idPaciente = PacienteSeleccionado.ID_Paciente;
                    // Elimina el paciente seleccionado de la lista
                    TodosLosPacientes.Remove(PacienteSeleccionado);

                    // Elimina el paciente de la base de datos
                    using (var dbContext = new CMJC_Context())
                    {
                        var paciente = dbContext.Pacientes.FirstOrDefault(p => p.ID_Paciente == idPaciente);

                        if (paciente != null)
                        {
                            dbContext.Pacientes.Remove(paciente);
                            dbContext.SaveChanges();
                        }
                    }

                    MessageBox.Show("Paciente eliminado exitosamente.", "Mensaje");
                }
                catch (Exception ex)
                {
                    // Maneja cualquier excepción que pueda ocurrir durante la eliminación
                    MessageBox.Show($"Error al eliminar el paciente: {ex.Message}", "Error");
                }
            }
            else
            {
                // Muestra un mensaje indicando que no hay paciente seleccionado
                MessageBox.Show("No hay un paciente seleccionado para eliminar.", "Mensaje");
            }
        }
        public void AgregarPaciente(object parameter)
        {
            // Crear una nueva instancia de la ventana AddPaciente
            addPacienteWindow = new AddPaciente(UserFullName);

            // Obtener la ventana Menu desde las ventanas abiertas
            var menuWindow = Application.Current.Windows.OfType<Menu>().FirstOrDefault();

            if (menuWindow != null)
            {
                // Ocultar la ventana principal (Menu)
                menuWindow.Hide();

                // Mostrar la ventana AddPaciente
                addPacienteWindow.ShowDialog();

                // Cuando la ventana AddPaciente se cierre, mostrar nuevamente la ventana Menu
                menuWindow.Show();
            }

        }
        public void EditarPaciente(object parameter)
        {
            if (PacienteSeleccionado != null)
            {
                // Obtener la ViewModel de EditPacienteVM desde la ventana EditPaciente
                var editPacienteViewModel = new EditPacienteVM(PacienteSeleccionado);

                if (editPacienteViewModel != null)
                {
                    // Crear una nueva instancia de la ventana EditPaciente
                    editPacienteWindow = new EditPaciente(UserFullName, editPacienteViewModel);
                }

                // Obtener la ventana Menu desde las ventanas abiertas
                var menuWindow = Application.Current.Windows.OfType<Menu>().FirstOrDefault();

                if (menuWindow != null)
                {
                    // Ocultar la ventana principal (Menu)
                    menuWindow.Hide();

                    // Mostrar la ventana EditPaciente
                    editPacienteWindow.ShowDialog();

                    // Cuando la ventana EditPaciente se cierre, mostrar nuevamente la ventana Menu
                    menuWindow.Show();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un paciente para editar.", "Mensaje");
            }
        }

        private bool PuedeEliminarPaciente(object parameter)
        {
            // Verifica si hay un paciente seleccionado en la lista
            return PacienteSeleccionado != null;
        }
        private bool PuedeEditarPaciente(object parameter)
        {
            return PacienteSeleccionado != null;
        }

    }
}
