using Microsoft.EntityFrameworkCore;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class HistoriaBasicaVM : BaseVM
    {
        //Cargar datos del Paciente
        private string userFullName;

        public string TipoAtencion;
        public string UserFullName
        {
            get { return userFullName; }
            set
            {
                userFullName = value;
                OnPropertyChanged(nameof(UserFullName));
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
                // Actualizar el nombre completo cuando se selecciona un nuevo paciente
                ActualizarNombreCompleto();
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
        // Método para actualizar el nombre completo
        private void ActualizarNombreCompleto()
        {
            if (PacienteSeleccionado != null)
            {
                NombreCompleto = $"{PacienteSeleccionado.Nombres} {PacienteSeleccionado.Apellidos}";
            }
            else
            {
                NombreCompleto = string.Empty;
            }
        }
        private string nombreCompleto;
        public string NombreCompleto
        {
            get { return nombreCompleto; }
            set
            {
                nombreCompleto = value;
                OnPropertyChanged(nameof(NombreCompleto));
            }
        }
        //Para pasar a la siguienteVM Consultas (clase)
        private bool ambulatorioSeleccionado;
        public bool AmbulatorioSeleccionado
        {
            get { return ambulatorioSeleccionado; }
            set
            {
                ambulatorioSeleccionado = value;
                OnPropertyChanged(nameof(AmbulatorioSeleccionado));

                if (value)
                {
                    EmergenciaSeleccionada = false;
                    TipoAtencion = "Ambulatorio";// pasar a la siguiente VM
                }
            }
        }

        private bool emergenciaSeleccionada;
        public bool EmergenciaSeleccionada
        {
            get { return emergenciaSeleccionada; }
            set
            {
                emergenciaSeleccionada = value;
                OnPropertyChanged(nameof(EmergenciaSeleccionada));

                if (value)
                {
                    AmbulatorioSeleccionado = false;
                    TipoAtencion = "Emergencia";// pasar a la siguiente VM
                }
            }
        }
 
        private int numeroConsultas;

        private string _fechaNacimientoConEdad;
        public string FechaNacimientoConEdad
        {
            get => _fechaNacimientoConEdad;
            set
            {
                if (_fechaNacimientoConEdad != value)
                {
                    _fechaNacimientoConEdad = value;
                    OnPropertyChanged(nameof(FechaNacimientoConEdad));
                }
            }
        }
        private void AsignarFechaNacimientoConEdad()
        {
            FechaNacimientoConEdad = $"{PacienteSeleccionado.FechaNacimiento:dd/MM/yyyy} / {PacienteSeleccionado.Edad} años";

        }
        //Para Historia Clinica
        private string familiares;
        public string Familiares
        {
            get { return familiares; }
            set
            {
                familiares = value;
                OnPropertyChanged(nameof(Familiares));
            }
        }
        private string patologicos;
        public string Patologicos
        {
            get { return patologicos; }
            set
            {
                patologicos = value;
                OnPropertyChanged(nameof(Patologicos));
            }
        }
        private string quirurgicos;
        public string Quirurgicos
        {
            get { return quirurgicos; }
            set
            {
                quirurgicos= value;
                OnPropertyChanged(nameof(Quirurgicos));
            }
        }
        private string alergias;
        public string Alergias
        {
            get { return alergias; }
            set
            {
                alergias = value;
                OnPropertyChanged(nameof(Alergias));
            }
        }
        private Visibility guardarHistoriaVisibility;
        private Visibility nuevaConsultaVisibility;

        public Visibility GuardarHistoriaVisibility
        {
            get { return guardarHistoriaVisibility; }
            set
            {
                guardarHistoriaVisibility = value;
                OnPropertyChanged(nameof(GuardarHistoriaVisibility));
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
        private string atencion;
        public string Atencion
        {
            get => atencion;
            set
            {
                atencion = value;
                OnPropertyChanged(nameof(Atencion));
            }
        }

        public ICommand GuardarHistoriaCommand { get; private set; }
        public ICommand NuevaConsultaCommand { get; private set; }
        public HistoriaBasicaVM(string userFullName, Paciente pacienteSeleccionado)
        {
            UserFullName = userFullName;
            PacienteSeleccionado = pacienteSeleccionado;
            AsignarFechaNacimientoConEdad();

            numeroConsultas = 1;
            Atencion = $"ACMJCN°{numeroConsultas.ToString("D2")}";

            GuardarHistoriaCommand = new CommandVM(GuardarHistoria);
            NuevaConsultaCommand = new CommandVM(NuevaConsulta);

            GuardarHistoriaVisibility = Visibility.Visible;
            NuevaConsultaVisibility = Visibility.Hidden;
        }

        private void GuardarHistoria(object parameter)
        {
            try
            {
                string antecedentesFamiliares = Familiares;
                string antecedentesPatologicos = Patologicos;
                string antecedentesQuirurgicos = Quirurgicos;
                string alergias = Alergias;
                int UsuarioID = ObtenerUsuarioIDPorNombreCompleto(UserFullName);

                // Generar el código de Historia Clínica
                string codigoHistoriaClinica = $"CMJC{PacienteSeleccionado.ID_Paciente}";

                // Crear una nueva instancia de HistoriaClinica
                HistoriaClinica nuevaHistoria = new HistoriaClinica
                {
                    Codigo = codigoHistoriaClinica,
                    AntecedentesFamiliares = antecedentesFamiliares,
                    AntecedentesPatologicos = antecedentesPatologicos,
                    AntecedentesQuirurgicos = antecedentesQuirurgicos,
                    Alergias = alergias,
                    ID_Paciente = PacienteSeleccionado.ID_Paciente,
                    ID_Usuario = UsuarioID
                };

                using (var context = new CMJC_Context())
                {
                    context.HistoriasClinicas.Add(nuevaHistoria);
                    context.SaveChanges();

                    if (PacienteSeleccionado != null)
                    {
                        // Recargar el paciente seleccionado desde la base de datos para obtener la relación actualizada
                        PacienteSeleccionado = context.Pacientes
                            .Include(p => p.HistoriaClinica)  
                            .Single(p => p.ID_Paciente == PacienteSeleccionado.ID_Paciente);

                        PacienteSeleccionado.AsignacionHistoria = "Asignado";
                        HistoriaClinica = nuevaHistoria;
                        // Obtener el usuario actual
                        var usuarioActual = context.Usuarios.FirstOrDefault(u => u.Nombres + " " + u.Apellidos == UserFullName);

                        if (usuarioActual != null)
                        {
                            // Incrementar NHistorias
                            usuarioActual.NHistoria++;

                            // Guardar los cambios en el usuario actual
                            context.Entry(usuarioActual).State = EntityState.Modified;
                        }

                        // Guardar los cambios en el paciente seleccionado
                        context.Entry(PacienteSeleccionado).State = EntityState.Modified;
                        context.SaveChanges(); // Guarda los cambios en la base de datos
                    }
                }

                GuardarHistoriaVisibility = Visibility.Hidden;
                NuevaConsultaVisibility = Visibility.Visible;

                MessageBox.Show("Historia Clínica guardada correctamente.", "Éxito");

                // Reiniciar campos o realizar otras acciones después de guardar
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al intentar guardar la historia clínica: {ex.Message}", "Error");
            }
        }
        private void NuevaConsulta(object parameter)
        {
            // Verifica si hay un Paciente Seleccionado
            if (PacienteSeleccionado != null && HistoriaClinica!=null)
            {
                // Oculta la ventana actual (HistoriaBasica)
                var historiaBasicaWindow = Application.Current.Windows.OfType<HistoriaBasica>().FirstOrDefault();
                if (historiaBasicaWindow != null)
                {
                    historiaBasicaWindow.Close();
                }

                using (var context = new CMJC_Context())
                {
                    // Cargar el paciente seleccionado desde la base de datos con la historia clínica
                    PacienteSeleccionado = context.Pacientes
                        .Include(p => p.HistoriaClinica)
                        .Single(p => p.ID_Paciente == PacienteSeleccionado.ID_Paciente);
                }

                // Abre la ventana FormularioHistoriaPrincipal llevando consigo el UserFullName
                var formularioHistoriaPrincipalVM = new FormularioHistoriaPrincipalVM(UserFullName, PacienteSeleccionado, HistoriaClinica,TipoAtencion, $"ACMJCN°{numeroConsultas.ToString("D2")}");
                var formularioHistoriaPrincipalWindow = new FormularioHistoriaPrincipal
                {
                    DataContext = formularioHistoriaPrincipalVM
                };

                // Muestra la ventana FormularioHistoriaPrincipal
                formularioHistoriaPrincipalWindow.ShowDialog();

            }
            else
            {
                MessageBox.Show("Error al abrir la ventana.", "Advertencia");
            }
        }
        public int ObtenerUsuarioIDPorNombreCompleto(string userFullName)
        {
            using (var dbContext = new CMJC_Context())
            {
                // Consulta en memoria utilizando AsEnumerable
                var usuario = dbContext.Usuarios.FirstOrDefault(u => u.Nombres + " " + u.Apellidos == UserFullName);

                if (usuario != null)
                {
                    return usuario.ID_Usuario;
                }

                throw new Exception($"No se encontró un usuario con el nombre completo: {userFullName}");
            }
        }
        private void LimpiarCampos()
        {
            Familiares = string.Empty;
            Patologicos = string.Empty;
            Quirurgicos = string.Empty;
            Alergias = string.Empty;
        }

    }
}
