using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.Vistas;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class ConsultasVM : BaseVM
    {

        private string userFullName;
        private string busqueda;
        private ObservableCollection<Paciente> pacientes;

        public string Busqueda
        {
            get { return busqueda; }
            set
            {
                busqueda = value;
                OnPropertyChanged(nameof(Busqueda));
            }
        }
        public string UserFullName
        {
            get { return userFullName; }
            set
            {
                userFullName = value;
                OnPropertyChanged(nameof(UserFullName));
            }
        }
        private int numeroConsultas;

        public ObservableCollection<Paciente> Pacientes
        {
            get { return pacientes; }
            set
            {
                pacientes = value;
                OnPropertyChanged(nameof(Pacientes));
            }
        }

        private Visibility nuevaHistoriaVisibility;
        private Visibility nuevaConsultaVisibility;

        public Visibility NuevaHistoriaVisibility
        {
            get { return nuevaHistoriaVisibility; }
            set
            {
                nuevaHistoriaVisibility = value;
                OnPropertyChanged(nameof(NuevaHistoriaVisibility));
            }
        }

        public Visibility NuevaConsultaVisibility
        {
            get { return nuevaConsultaVisibility; }
            set
            {
                nuevaConsultaVisibility = value;
                OnPropertyChanged(nameof(NuevaConsultaVisibility));
            }
        }

        private Paciente pacienteSeleccionado;
        public Paciente PacienteSeleccionado
        {
            get { return pacienteSeleccionado; }
            set
            {
                pacienteSeleccionado = value;
                OnPropertyChanged(nameof(PacienteSeleccionado));

                // Actualizar la visibilidad de los botones basándose en la cantidad de consultas
                if (PacienteSeleccionado != null)
                {
                    // Si hay asignacion, ocultar el botón "Nueva Historia"
                   if(PacienteSeleccionado.AsignacionHistoria == "Sin Asignar")
                    {
                        NuevaHistoriaVisibility = Visibility.Visible;
                        NuevaConsultaVisibility = Visibility.Hidden;
                    }
                    else
                    {
                        NuevaHistoriaVisibility = Visibility.Hidden;
                        NuevaConsultaVisibility = Visibility.Visible;
                    }
                }
                else
                {
                    NuevaHistoriaVisibility = Visibility.Hidden;
                    NuevaConsultaVisibility = Visibility.Hidden;
                }
            }
        }
        private HistoriaClinica historiaClinica;
        public HistoriaClinica HistoriaClinica
        {
            get { return historiaClinica; }
            set
            {
                historiaClinica = value;
                OnPropertyChanged(nameof(HistoriaClinica));
            }
        }
        public ICommand BuscarCommand { get; private set; }
        public ICommand NuevaHistoriaCommand { get; private set; }
        public ICommand NuevaConsultaCommand { get; private set; }
        public ConsultasVM(string userFullName) 
        {
            UserFullName = userFullName;

            Pacientes = new ObservableCollection<Paciente>();

            CargarPacientesDesdeBaseDeDatos();

            BuscarCommand = new CommandVM(BuscarPaciente);
            NuevaHistoriaCommand = new CommandVM(NuevaHistoria);
            NuevaConsultaCommand=new CommandVM(NuevaConsulta);

            // Establecer la visibilidad inicial
            NuevaHistoriaVisibility = Visibility.Hidden;
            NuevaConsultaVisibility = Visibility.Hidden;

        }
        private void CargarPacientesDesdeBaseDeDatos()
        {
            using (var dbContext = new CMJC_Context())
            {
                // Realiza la consulta a la base de datos para obtener los pacientes
                var pacientesDesdeDB = dbContext.Pacientes.ToList();

                // Limpia la colección existente (si la hay)
                Pacientes.Clear();

                // Agrega los pacientes obtenidos a la colección
                foreach (var paciente in pacientesDesdeDB)
                {
                    Pacientes.Add(paciente);
                }
            }
        }
        private void BuscarPaciente(object parameter)
        {
            try
            {
                using (var dbContext = new CMJC_Context())
                {
                    // Realizar la búsqueda por DocIdentidad
                    var paciente = dbContext.Pacientes
                        .Where(hc => hc.DIdentidad.Contains(Busqueda))
                        .ToList();

                    // Actualizar la lista de Pacientes con los resultados de la búsqueda
                    Pacientes = new ObservableCollection<Paciente>(paciente);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la búsqueda
                MessageBox.Show($"Error al buscar el paciente: {ex.Message}", "Error");
            }
        }

        private void NuevaHistoria(object parameter)
        {
            try
            {
                // Verifica si hay un paciente seleccionado
                if (PacienteSeleccionado != null)
                {
                    // Oculta la ventana actual (Menu)
                    var menuWindow = Application.Current.Windows.OfType<Menu>().FirstOrDefault();
                    if (menuWindow != null)
                    {
                        menuWindow.Hide();
                    }

                    // Abre la ventana HistoriaBasica llevando consigo el UserFullName y el PacienteSeleccionado
                    var historiaBasicaVM = new HistoriaBasicaVM(UserFullName, PacienteSeleccionado);
                    var historiaBasicaWindow = new HistoriaBasica
                    {
                        DataContext = historiaBasicaVM
                    };

                    // Muestra la ventana HistoriaBasica
                    historiaBasicaWindow.ShowDialog();

                    // Cuando la ventana HistoriaBasica se cierre, mostrar nuevamente la ventana Menu
                    if (menuWindow != null)
                    {
                        menuWindow.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un Paciente antes de crear una nueva historia clínica.", "Advertencia");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ventana de nueva historia: {ex.Message}", "Error");
            }
        }
        private void NuevaConsulta(object parameter)
        {
            try
            {
                int idpaciente = pacienteSeleccionado.ID_Paciente;
                using (var context = new CMJC_Context())
                {
                    // Consulta para obtener la historia clínica del paciente seleccionado
                    var historiaClinica = context.HistoriasClinicas
                        .Where(hc => hc.ID_Paciente == idpaciente)
                        .FirstOrDefault();
                    HistoriaClinica = historiaClinica;

                    int idhistoria = HistoriaClinica.ID_HistoriaClinica;
                    var consultas = context.Consultas
                        .Where(c => c.ID_HistoriaClinica == idhistoria)
                        .Count();

                    numeroConsultas = consultas+1;
                }

                // Verifica si hay un Paciente Seleccionado
                if (PacienteSeleccionado != null && HistoriaClinica != null)
                    {
                    
                    // Muestra el cuadro de diálogo personalizado
                    var tipoAtencionDialog = new TipoAtencionDialog();
                    tipoAtencionDialog.ShowDialog();

                    // Obtiene el resultado del cuadro de diálogo
                    string tipoAtencion = tipoAtencionDialog.TipoAtencionSeleccionado;

                    // Si el usuario cancela la selección
                    if (string.IsNullOrEmpty(tipoAtencion))
                        return;

                    // Oculta la ventana actual (Menu)
                    var menuWindow = Application.Current.Windows.OfType<Menu>().FirstOrDefault();
                    if (menuWindow != null)
                    {
                        menuWindow.Hide();
                    }

                    // Abre la ventana FormularioHistoriaPrincipal llevando consigo el UserFullName
                    var formularioHistoriaPrincipalVM = new FormularioHistoriaPrincipalVM(UserFullName, PacienteSeleccionado, HistoriaClinica, tipoAtencion, $"ACMJCN°{numeroConsultas.ToString("D2")}");
                    var formularioHistoriaPrincipalWindow = new FormularioHistoriaPrincipal
                    {
                       DataContext = formularioHistoriaPrincipalVM
                    };

                    // Muestra la ventana FormularioHistoriaPrincipal
                    formularioHistoriaPrincipalWindow.ShowDialog();

                    // Cuando la ventana FormularioHistoriaPrincipal se cierre, mostrar nuevamente la ventana Menu
                    if (menuWindow != null)
                    {
                        menuWindow.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un Paciente antes de crear una nueva consulta.", "Advertencia");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ventana de nueva consulta: {ex.Message}", "Error");
            }
        }

    }
}
