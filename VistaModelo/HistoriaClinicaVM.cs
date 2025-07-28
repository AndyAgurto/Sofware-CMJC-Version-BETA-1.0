using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.Vistas;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class HistoriaClinicaVM : BaseVM
    {
        private string userFullName;
        private VerConsultas verConsultasWindow;
        private RecetaEstandarListaGeneral verRecetasWindow;
        public string UserFullName
        {
            get { return userFullName; }
            set
            {
                userFullName = value;
                OnPropertyChanged(nameof(UserFullName));
            }
        }
        private ObservableCollection<HistoriaClinica> historiasClinicas;
        public ObservableCollection<HistoriaClinica> HistoriasClinicas
        {
            get { return historiasClinicas; }
            set
            {
                historiasClinicas = value;
                OnPropertyChanged(nameof(HistoriasClinicas));
            }
        }

        private string textoBusqueda;
        public string TextoBusqueda
        {
            get { return textoBusqueda; }
            set
            {
                textoBusqueda = value;
                OnPropertyChanged(nameof(TextoBusqueda));
            }
        }
        private HistoriaClinica historiaSeleccionada;
        public HistoriaClinica HistoriaSeleccionada
        {
            get { return historiaSeleccionada; }
            set
            {
                historiaSeleccionada=value;
                OnPropertyChanged(nameof(HistoriaSeleccionada));
            }
        }

        public ICommand BuscarCommand { get; private set; }
        public ICommand VerHistoriaCommand { get; private set; }
        public ICommand VerConsultasCommand { get; private set; }
        public ICommand VerRecetasCommand { get; private set; }
        public HistoriaClinicaVM(string userFullName)
        {
            UserFullName = userFullName;

            BuscarCommand = new CommandVM(Buscar);
            VerHistoriaCommand = new CommandVM(VerHistoriaClinica);
            VerConsultasCommand = new CommandVM(VerConsulta);
            VerRecetasCommand=new CommandVM(VerRecetas);

            HistoriasClinicas = new ObservableCollection<HistoriaClinica>();
            CargaHistoriasDesdeBaseDeDatos();

        }
        private void CargaHistoriasDesdeBaseDeDatos()
        {
            using (var dbContext = new CMJC_Context())
            {
                // Realiza la consulta a la base de datos para obtener las historias clínicas con sus pacientes
                var historiasDesdeDB = dbContext.HistoriasClinicas.Include(h => h.Paciente).ToList();

                // Limpia la colección existente (si la hay)
                HistoriasClinicas.Clear();

                // Agrega los pacientes obtenidos a la colección
                foreach (var historia in historiasDesdeDB)
                {
                    HistoriasClinicas.Add(historia);
                }
            }
        }
        private void Buscar(object parameter)
        {
           if(!string.IsNullOrEmpty(TextoBusqueda))
            {
                // Filtra la lista de historias clínicas según el TextoBusqueda
                var historiasFiltradas = HistoriasClinicas
                .Where(h => h.Codigo.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    h.Paciente.Nombres.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    h.Paciente.Apellidos.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                    h.Paciente.DIdentidad.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase))
                .ToList();

                // Actualizar la lista de historias con los resultados de la búsqueda
                HistoriasClinicas = new ObservableCollection<HistoriaClinica>(historiasFiltradas);

            }
            else
            {
                CargaHistoriasDesdeBaseDeDatos();

            }
        }

        public void VerHistoriaClinica(object parameter)
        {
            try
            {
                // Verificar si hay datos en la propiedad
                if (HistoriaSeleccionada != null)
                {
                    StringBuilder mensaje = new StringBuilder();
                    mensaje.AppendLine($"Historia Clínica N°: {HistoriaSeleccionada.Codigo}");
                    mensaje.AppendLine($"Nombre del Paciente: {HistoriaSeleccionada.Paciente.Nombres} {HistoriaSeleccionada.Paciente.Apellidos}");
                    mensaje.AppendLine($"Fecha de Nacimiento: {HistoriaSeleccionada.Paciente.FechaNacimiento}");
                    mensaje.AppendLine($"Edad: {HistoriaSeleccionada.Paciente.Edad}");
                    mensaje.AppendLine($"Ocupación: {HistoriaSeleccionada.Paciente.Ocupacion}");
                    mensaje.AppendLine($"Dirección: {HistoriaSeleccionada.Paciente.Direccion}");
                    mensaje.AppendLine($"Aseguradora: {HistoriaSeleccionada.Paciente.Aseguradora}");
                    mensaje.AppendLine($"Sexo: {HistoriaSeleccionada.Paciente.Genero}");
                    mensaje.AppendLine($"Documento de ID.: {HistoriaSeleccionada.Paciente.TipoIdentidad}");
                    mensaje.AppendLine($"N° ID: {HistoriaSeleccionada.Paciente.DIdentidad}");
                    mensaje.AppendLine($"Estado Civil: {HistoriaSeleccionada.Paciente.EstadoCivil}");
                    mensaje.AppendLine($"Medico: Dr. Julio César Agurto Urcia");
                    mensaje.AppendLine($"Especialidad: Traumatología");
                    mensaje.AppendLine();
                    mensaje.AppendLine("Antecedentes:");
                    mensaje.AppendLine($"Antecedentes Familiares: {HistoriaSeleccionada.AntecedentesFamiliares}");
                    mensaje.AppendLine($"Antecedentes Patológicos: {HistoriaSeleccionada.AntecedentesPatologicos}");
                    mensaje.AppendLine($"Antecedentes Quirúrgicos: {HistoriaSeleccionada.AntecedentesQuirurgicos}");
                    mensaje.AppendLine($"Alergias: {HistoriaSeleccionada.Alergias}");
                    mensaje.AppendLine();

                    // Mostrar el MessageBox con la información
                    MessageBox.Show(mensaje.ToString(), "Historia Clínica", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Si no hay datos, mostrar un mensaje indicando que no hay datos para mostrar
                    MessageBox.Show("Por favor selecciona una historia.", "Historia Clínica", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y mostrar un mensaje de error
                MessageBox.Show($"Error al ver Historia Clínica: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void VerConsulta(object parameter)
        {
            // Verifica si hay una Historia Seleccionada
            if (HistoriaSeleccionada != null)
            {
                // Obtener la ViewModel de VerConsultasVM desde la ventana VerConsultas
                var verConsultasViewModel = new VerConsultasVM(UserFullName, HistoriaSeleccionada);

                if (verConsultasViewModel != null)
                {
                    // Crear una nueva instancia de la ventana Ver Consultas
                    verConsultasWindow = new VerConsultas(verConsultasViewModel);
                }

                // Obtener la ventana Menu desde las ventanas abiertas
                var menuWindow = Application.Current.Windows.OfType<Menu>().FirstOrDefault();

                if (menuWindow != null)
                {
                    // Ocultar la ventana principal (Menu)
                    menuWindow.Hide();

                    // Mostrar la ventana EditPaciente
                    verConsultasWindow.ShowDialog();

                    // Cuando la ventana EditPaciente se cierre, mostrar nuevamente la ventana Menu
                    menuWindow.Show();
                }
            }
            else
            {
                MessageBox.Show("Seleciona una historia.", "Advertencia");
            }
        }

        public void VerRecetas(object parameter)
        {
            verRecetasWindow = new RecetaEstandarListaGeneral();
            // Obtener la ventana Menu desde las ventanas abiertas
            var menuWindow = Application.Current.Windows.OfType<Menu>().FirstOrDefault();

                if (menuWindow != null)
                {
                    // Ocultar la ventana principal (Menu)
                    menuWindow.Hide();

                    // Mostrar la ventana EditPaciente
                    verRecetasWindow.ShowDialog();

                    // Cuando la ventana EditPaciente se cierre, mostrar nuevamente la ventana Menu
                    menuWindow.Show();
                }
        }

    }
}
