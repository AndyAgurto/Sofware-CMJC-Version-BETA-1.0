using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
    public class RecetaEstandarVM : BaseVM
    {
        public string NombreCompletoPaciente;
        public string NAtencion;

        private Receta recetaSeleccionada;
        public Receta RecetaSeleccionada
        {
            get { return recetaSeleccionada; }
            set
            {
                recetaSeleccionada = value;
                OnPropertyChanged(nameof(RecetaSeleccionada));
            }
        }
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
        private Consulta nuevaConsulta;
        public Consulta NuevaConsulta
        {
            get { return nuevaConsulta; }
            set
            {
                nuevaConsulta = value;
                OnPropertyChanged(nameof(NuevaConsulta));
            }
        }
        private ObservableCollection<Receta> listaRecetas;
        public ObservableCollection<Receta> ListaRecetas
        {
            get { return listaRecetas; }
            set
            {
                listaRecetas = value;
                OnPropertyChanged(nameof(ListaRecetas));
            }
        }
        public ICommand SubirRecetaCommand { get; private set; }
        public ICommand EliminarRecetaCommand { get; private set; }
        public ICommand VolverAlMenuCommand { get; private set; }
        public ICommand ExportarFormatoCommand { get; private set; }

        public RecetaEstandarVM(string userFullname, Consulta nuevaConsulta)
        {
            UserFullName = userFullname;
            NuevaConsulta = nuevaConsulta;

            SubirRecetaCommand = new CommandVM(SubirReceta);
            EliminarRecetaCommand = new CommandVM(EliminarReceta);
            VolverAlMenuCommand = new CommandVM(VolverAlMenu);
            ExportarFormatoCommand = new CommandVM(ExportarAPDF);

            // Inicializar la lista de recetas
            ListaRecetas = new ObservableCollection<Receta>();

            // Llenar ListaRecetas al inicio
            CargarRecetas();
            NombreCompletoPaciente = ObtenerNombreCompletoPaciente(NuevaConsulta);
            NAtencion = ObtenerNumeroAtencion(NuevaConsulta);
        }
        private void CargarRecetas()
        {
            ListaRecetas.Clear();
            using (var dbContext = new CMJC_Context())
            {
                // Consultar todas las recetas desde la base de datos
                var todasLasRecetas = dbContext.Recetas.ToList();

                // Asignar la lista de recetas a la propiedad Recetas
                ListaRecetas = new ObservableCollection<Receta>(todasLasRecetas);
            }
        }

        private string ObtenerNombreCompletoPaciente(Consulta consulta)
        {
            if (consulta != null)
            {
                using (var dbContext = new CMJC_Context())
                {
                    // Busca la historia clínica de la consulta
                    var historia = dbContext.HistoriasClinicas
                        .Include(h => h.Paciente)
                        .FirstOrDefault(h => h.ID_HistoriaClinica == consulta.ID_HistoriaClinica);

                    if (historia != null && historia.Paciente != null)
                    {
                        // Retorna el nombre completo del paciente
                        return $"{historia.Paciente.Nombres} {historia.Paciente.Apellidos}";
                    }
                }
            }

            return string.Empty;
        }
        private string ObtenerNumeroAtencion(Consulta consulta)
        {
            if (consulta != null)
            {
                using (var dbContext = new CMJC_Context())
                {
                    // Busca la consultas de la consulta
                    var consultas= dbContext.Consultas
                        .FirstOrDefault(c => c.ID_Consulta == consulta.ID_Consulta);

                    if (consultas != null)
                    {
                        return consultas.NAtencion;
                    }
                }
            }

            return string.Empty;
        }

        private void SubirReceta(object parameter)
        {
            try
            {
                // Mostrar el cuadro de diálogo para seleccionar un archivo
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Archivos PDF (*.pdf)|*.pdf|Archivos Word (*.doc;*.docx)|*.doc;*.docx|Todos los archivos (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    // Obtener la ruta del archivo seleccionado
                    string rutaArchivo = openFileDialog.FileName;

                    // Crear una nueva instancia de tu ventana personalizada (ajusta según tu implementación)
                    VentanaInformacionReceta ventanaInformacion = new VentanaInformacionReceta();

                    // Mostrar la ventana y esperar hasta que se cierre
                    if (ventanaInformacion.ShowDialog() == true)
                    {
                        DateTime fechaCaducidad = ventanaInformacion.FechaCaducidad;
                        string nombreArchivo = ventanaInformacion.NombreArchivo;
                        string tipoMIME = ventanaInformacion.TipoMIME;

                        GuardarRecetaEnBaseDeDatos(rutaArchivo, fechaCaducidad, nombreArchivo, tipoMIME);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show($"Error al subir la receta: {ex.Message}", "Error");
            }
        }
        private void EliminarReceta(object parameter)
        {
            try
            {
                if (NuevaConsulta != null && NuevaConsulta.Recetas.Count > 0)
                {
                    // Obtén la receta que deseas eliminar (por ejemplo, la última receta agregada)
                    Receta recetaAEliminar = NuevaConsulta.Recetas.Last();

                    // Elimina la receta de la lista de recetas de la consulta
                    NuevaConsulta.Recetas.Remove(recetaAEliminar);

                    // Elimina la receta de la base de datos
                    using (var dbContext = new CMJC_Context())
                    {
                        dbContext.Recetas.Remove(recetaAEliminar);
                        dbContext.SaveChanges();
                    }
                    CargarRecetas();
                    MessageBox.Show("Receta eliminada correctamente.", "Éxito");
                }
                else
                {
                    MessageBox.Show("No hay recetas para eliminar.", "Advertencia");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar la receta: {ex.Message}", "Error");
            }
        }
        private void ExportarAPDF(object parameter)
        {
            try
            {
                // Verificar que hay una receta seleccionada
                if (RecetaSeleccionada == null)
                {
                    MessageBox.Show("Seleccione una receta para descargar.", "Advertencia");
                    return;
                }

                // Abrir un diálogo para seleccionar la ubicación de guardado
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Documentos PDF|*.pdf",
                    Title = "Descargar Receta PDF"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Obtener la ruta del archivo seleccionada por el usuario
                    string rutaArchivo = saveFileDialog.FileName;

                    // Guardar el array de bytes como un archivo PDF
                    File.WriteAllBytes(rutaArchivo, RecetaSeleccionada.ArchivoWord);

                    // Mensaje de éxito
                    MessageBox.Show("Receta descargada exitosamente como PDF.", "Éxito");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show($"Error al descargar la receta como PDF: {ex.Message}", "Error");
            }
        }

        private void GuardarRecetaEnBaseDeDatos(string rutaArchivo, DateTime fechaCaducidad, string nombreArchivo, string tipoMIME)
        {
            try
            {
                using (var dbContext = new CMJC_Context())
                {
                    byte[] contenidoArchivo = File.ReadAllBytes(rutaArchivo);
                    // Crea una nueva receta
                    Receta nuevaReceta = new Receta
                    {
                        RutaArchivo = rutaArchivo,
                        Paciente = NombreCompletoPaciente,
                        NAtencion = NAtencion,
                        FechaCreacion = DateTime.Now,
                        FechaCaducidad = fechaCaducidad,
                        NombreArchivo = nombreArchivo,
                        TipoMIME = tipoMIME,
                        ArchivoWord=contenidoArchivo
                    };

                    // Asignar la receta a la consulta existente
                    NuevaConsulta.Recetas.Add(nuevaReceta);
                    NuevaConsulta.CRecetas++;

                    // Guardar la nueva receta en la base de datos
                    dbContext.Recetas.Add(nuevaReceta);

                    // Guardar la consulta con las recetas en la base de datos
                    dbContext.Consultas.Update(NuevaConsulta);
                    dbContext.SaveChanges();

                    // Muestra un mensaje de éxito o realiza cualquier otra acción que necesites
                    MessageBox.Show("Receta guardada exitosamente.", "Éxito");
                    CargarRecetas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la receta: {ex.ToString()}", "Error");
            }
        }

        private void VolverAlMenu(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea volver al menú? Los datos no guardados se perderán.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {

                var recetaWindow = Application.Current.Windows.OfType<RecetaEstandar>().FirstOrDefault();
                if (recetaWindow != null)
                {
                    recetaWindow.Close();
                }
            }
        }
    }
}
