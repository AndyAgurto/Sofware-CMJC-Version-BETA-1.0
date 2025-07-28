using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.Vistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Xceed.Words.NET;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class VerConsultasVM : BaseVM
    {
        private HistoriaClinica historiaSeleccionada;
        private ObservableCollection<Consulta> consultasFiltradas;
        private string textBusqueda;
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
        public HistoriaClinica HistoriaSeleccionada
        {
            get { return historiaSeleccionada; }
            set
            {
                historiaSeleccionada = value;
                OnPropertyChanged(nameof(HistoriaSeleccionada));
            }
        }
        public ObservableCollection<Consulta> ConsultasFiltradas
        {
            get { return consultasFiltradas; }
            set
            {
                consultasFiltradas = value;
                OnPropertyChanged(nameof(ConsultasFiltradas));
            }
        }
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
                    FiltrarConsultas(TextBusqueda);
                }
            }
        }
        private Consulta _consultaSeleccionada;
        public Consulta ConsultaSeleccionada
        {
            get { return _consultaSeleccionada; }
            set
            {
                _consultaSeleccionada = value;
                OnPropertyChanged(nameof(ConsultaSeleccionada));
            }
        }
        private Paciente paciente;
        public Paciente Paciente
        {
            get { return paciente; }
            set
            {
                paciente = value;
                OnPropertyChanged(nameof(Paciente));
            }
        }

        public List<Diagnostico> Diagnosticos;

        public ICommand VerConsultaCommand { get; private set; }
        public ICommand ExportarConsultaCommand { get; private set; }
        public ICommand VolverAlMenuCommand { get; private set; }
        public VerConsultasVM(string userFullName, HistoriaClinica historiaSelecionado)
        {
            UserFullName = userFullName;
            HistoriaSeleccionada = historiaSelecionado;
            Diagnosticos = new List<Diagnostico>();
            CargarDatosPacientes();
            ConsultasFiltradas = new ObservableCollection<Consulta>();
            CargarConsultasDeHistoria();

            VerConsultaCommand = new CommandVM(VerConsulta);
            ExportarConsultaCommand = new CommandVM(ExportarConsulta);
            VolverAlMenuCommand = new CommandVM(VolverAlMenu);
        }
        private void CargarDatosPacientes()
        {
            using (var dbContext = new CMJC_Context())
            {
                var historiaSeleccionada = dbContext.HistoriasClinicas
                    .Include(h => h.Paciente)  // Incluye la relación con el paciente
                    .FirstOrDefault(h => h.ID_HistoriaClinica == HistoriaSeleccionada.ID_HistoriaClinica);

                Paciente = historiaSeleccionada.Paciente;
            }
        }
        private void VerConsulta(object parameter)
        {            
            if (ConsultaSeleccionada != null)
            {
                using(var dbContext = new CMJC_Context())
                {
                 // Obtener diagnósticos de la ConsultaSeleccionada
                 var consultaSeleccionada = dbContext.Consultas
                .Include(c => c.Diagnosticos)  // Incluye la relación con los diagnósticos
                .FirstOrDefault(c => c.ID_Consulta == ConsultaSeleccionada.ID_Consulta);

                    Diagnosticos = consultaSeleccionada.Diagnosticos;
                }

                StringBuilder mensaje = new StringBuilder();
                mensaje.AppendLine($"Paciente: {Paciente.NombreCompleto}");
                mensaje.AppendLine($"Tipo ID.: {Paciente.TipoIdentidad}");
                mensaje.AppendLine($"Doc. ID.: {Paciente.DIdentidad}");
                mensaje.AppendLine($"Edad: {Paciente.Edad}");
                mensaje.AppendLine($"Sexo: {Paciente.Genero}");
                mensaje.AppendLine($"Tipo de Sangre: {Paciente.TipoSangre}");
                mensaje.AppendLine("\n");
                mensaje.AppendLine("\n");
                mensaje.AppendLine($"Número de Atención: {ConsultaSeleccionada.NAtencion}");
                mensaje.AppendLine($"Tipo de Atención: {ConsultaSeleccionada.TipoAtencion}");
                mensaje.AppendLine($"Fecha: {ConsultaSeleccionada.Fecha}");
                mensaje.AppendLine($"Sintomas y Signos: {ConsultaSeleccionada.SintomasySignos}");
                mensaje.AppendLine("Examen Fisico");
                mensaje.AppendLine($"FC: {ConsultaSeleccionada.FC}");
                mensaje.AppendLine($"FR: {ConsultaSeleccionada.FR}");
                mensaje.AppendLine($"T°: {ConsultaSeleccionada.Temperatura}");
                mensaje.AppendLine($"PA: {ConsultaSeleccionada.PresionArterial}");
                mensaje.AppendLine($"Peso: {ConsultaSeleccionada.Peso}");
                mensaje.AppendLine($"Talla: {ConsultaSeleccionada.Talla}");
                mensaje.AppendLine($"IMC: {ConsultaSeleccionada.IMC}");
                mensaje.AppendLine($"Descripción de Examen Fisico: {ConsultaSeleccionada.DescripcionExamenFisico}");
                mensaje.AppendLine($"Examenes Auxiliares: {ConsultaSeleccionada.ExamenesAuxiliares}");
                mensaje.AppendLine($"Tratamiento: {ConsultaSeleccionada.Tratamiento}");
                mensaje.AppendLine($"Interconsulta: {ConsultaSeleccionada.Interconsulta}");
                mensaje.AppendLine("\n");
                mensaje.AppendLine("\n");
                mensaje.AppendLine("Diagnosticos:");
                foreach (var diagnostico in Diagnosticos)
                {
                    mensaje.AppendLine($"CIE10: {diagnostico.CIE10}  ");
                    mensaje.AppendLine($"Descripción: {diagnostico.DescripcionDiagnostico}  ");
                    mensaje.AppendLine($"Tipo Diagnostico: {diagnostico.TipoDiagnostico}  ");
                    mensaje.AppendLine("\n");
                }
                // Mostrar la ventana emergente con los detalles de la consulta
                MessageBox.Show(mensaje.ToString(), "Detalles de la Consulta", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Mostrar un mensaje si no hay consulta seleccionada
                MessageBox.Show("Selecciona una consulta para ver detalles.", "Sin Consulta Seleccionada", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ExportarConsulta(object parameter)
        {
            if (ConsultaSeleccionada != null)
            {
                try
                {
                    using (var dbContext = new CMJC_Context())
                    {
                        // Obtener diagnósticos de la ConsultaSeleccionada
                        var consultaSeleccionada = dbContext.Consultas
                       .Include(c => c.Diagnosticos)  // Incluye la relación con los diagnósticos
                       .FirstOrDefault(c => c.ID_Consulta == ConsultaSeleccionada.ID_Consulta);

                        Diagnosticos = consultaSeleccionada.Diagnosticos;
                    }

                    // Utilizar SaveFileDialog para obtener la ruta del archivo
                    Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    saveFileDialog.Filter = "Archivos de Word|*.docx";
                    saveFileDialog.Title = "Guardar como";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string rutaArchivo = saveFileDialog.FileName;
                        // Crear un nuevo documento Word
                        using (var doc = DocX.Create(rutaArchivo))
                        {
                            // Agregar contenido al documento
                            AgregarParrafo(doc, $"Paciente: {Paciente.NombreCompleto}");
                            AgregarParrafo(doc, $"Tipo ID.: {Paciente.TipoIdentidad}");
                            AgregarParrafo(doc, $"Doc. ID.: {Paciente.DIdentidad}");
                            AgregarParrafo(doc, $"Edad: {Paciente.Edad}");
                            AgregarParrafo(doc, $"Sexo: {Paciente.Genero}");
                            AgregarParrafo(doc, $"Tipo de Sangre: {Paciente.TipoSangre}");
                            AgregarParrafo(doc, "\n");
                            AgregarParrafo(doc, "\n");
                            AgregarParrafo(doc, $"Número de Atención: {ConsultaSeleccionada.NAtencion}");
                            AgregarParrafo(doc, $"Tipo de Atención: {ConsultaSeleccionada.TipoAtencion}");
                            AgregarParrafo(doc, $"Fecha: {ConsultaSeleccionada.Fecha}");
                            AgregarParrafo(doc, $"Síntomas y Signos: {ConsultaSeleccionada.SintomasySignos}");
                            AgregarParrafo(doc, "Examen Fisico");
                            AgregarParrafo(doc, $"FC: {ConsultaSeleccionada.FC}");
                            AgregarParrafo(doc, $"FR: {ConsultaSeleccionada.FR}");
                            AgregarParrafo(doc, $"T°: {ConsultaSeleccionada.Temperatura}");
                            AgregarParrafo(doc, $"PA: {ConsultaSeleccionada.PresionArterial}");
                            AgregarParrafo(doc, $"Peso: {ConsultaSeleccionada.Peso}");
                            AgregarParrafo(doc, $"Talla: {ConsultaSeleccionada.Talla}");
                            AgregarParrafo(doc, $"IMC: {ConsultaSeleccionada.IMC}");
                            AgregarParrafo(doc, $"Descripción de Examen Fisico: {ConsultaSeleccionada.DescripcionExamenFisico}");
                            AgregarParrafo(doc, $"Examenes Auxiliares: {ConsultaSeleccionada.ExamenesAuxiliares}");
                            AgregarParrafo(doc, $"Tratamiento: {ConsultaSeleccionada.Tratamiento}");
                            AgregarParrafo(doc, $"Interconsulta: {ConsultaSeleccionada.Interconsulta}");
                            AgregarParrafo(doc, "\n");
                            AgregarParrafo(doc, "\n");
                            AgregarParrafo(doc, "Diagnosticos:");
                            foreach (var diagnostico in Diagnosticos)
                            {
                                AgregarParrafo(doc, $"CIE10: {diagnostico.CIE10}");
                                AgregarParrafo(doc, $"Descripción: {diagnostico.DescripcionDiagnostico}");
                                AgregarParrafo(doc, $"Tipo Diagnostico: {diagnostico.TipoDiagnostico}");
                                AgregarParrafo(doc, "\n");
                            }

                            // Guardar el documento
                            doc.Save();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar la consulta a Word: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Mostrar un mensaje si no hay consulta seleccionada
                MessageBox.Show("Selecciona una consulta para exportar a Word.", "Sin Consulta Seleccionada", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void AgregarParrafo(DocX doc, string texto)
        {
            // Agregar un párrafo al documento
            doc.InsertParagraph(texto);
        }
        private void VolverAlMenu(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea volver al menú? Los datos no guardados se perderán.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {

                var verConsultasWindow = Application.Current.Windows.OfType<VerConsultas>().FirstOrDefault();
                if (verConsultasWindow != null)
                {
                    verConsultasWindow.Close();
                }

            }
        }
        private void CargarConsultasDeHistoria()
        {
            using (var dbContext = new CMJC_Context())
            {
                // Carga la historia seleccionada incluyendo las consultas desde la base de datos
                var historiaConConsultas = dbContext.HistoriasClinicas
                    .Include(h => h.Consultas)
                    .FirstOrDefault(h => h.ID_HistoriaClinica == HistoriaSeleccionada.ID_HistoriaClinica);

                // Obtiene todas las consultas de la historia con consultas
                var consultas = historiaConConsultas.Consultas;

                // Limpia la colección existente (si la hay)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ConsultasFiltradas.Clear();
                    foreach (var cons in consultas)
                    {
                        ConsultasFiltradas.Add(cons);
                    }
                });
            }
        }

        // Método de Búsqueda
        public void FiltrarConsultas(string criterioBusqueda)
        {
            criterioBusqueda = TextBusqueda.ToLower();

            if (string.IsNullOrWhiteSpace(criterioBusqueda))
            {
                CargarConsultasDeHistoria();
            }
            else
            {
                // Filtra las consultas según el criterioBusqueda
                var filtrado = ConsultasFiltradas
                    .Where(c =>
                        c.NAtencion.ToLower().Contains(criterioBusqueda) ||
                        c.TipoAtencion.ToLower().Contains(criterioBusqueda))
                    .ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Limpiar la colección actual y agregar los resultados de la búsqueda.
                    ConsultasFiltradas.Clear();
                    foreach (var consulta in filtrado)
                    {
                        ConsultasFiltradas.Add(consulta);
                    }
                });
            }
        }
    }
}
