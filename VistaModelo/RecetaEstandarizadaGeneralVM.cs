using Microsoft.Win32;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.Vistas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class RecetaEstandarizadaGeneralVM :BaseVM
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
        public ICommand EliminarRecetaCommand { get; private set; }
        public ICommand VolverAlMenuCommand { get; private set; }
        public ICommand ExportarFormatoCommand { get; private set; }

        public RecetaEstandarizadaGeneralVM()
        {
            EliminarRecetaCommand = new CommandVM(EliminarReceta);
            VolverAlMenuCommand = new CommandVM(VolverAlMenu);
            ExportarFormatoCommand = new CommandVM(ExportarAPDF);

            // Inicializar la lista de recetas
            ListaRecetas = new ObservableCollection<Receta>();

            // Llenar ListaRecetas al inicio
            CargarRecetas();
        }
        private void CargarRecetas()
        {
            using (var dbContext = new CMJC_Context())
            {
                // Consultar todas las recetas desde la base de datos
                var todasLasRecetas = dbContext.Recetas.ToList();

                // Asignar la lista de recetas a la propiedad Recetas
                ListaRecetas = new ObservableCollection<Receta>(todasLasRecetas);
            }
        }

        private void EliminarReceta(object parameter)
        {
            try
            {
                   // Obtén la receta que deseas eliminar (por ejemplo, la última receta agregada)
                    Receta recetaAEliminar = ListaRecetas.Last();

                    // Elimina la receta de la lista de recetas de la consulta
                    ListaRecetas.Remove(recetaAEliminar);

                    // Elimina la receta de la base de datos
                    using (var dbContext = new CMJC_Context())
                    {
                        dbContext.Recetas.Remove(recetaAEliminar);
                        dbContext.SaveChanges();
                    }

                    MessageBox.Show("Receta eliminada correctamente.", "Éxito");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"No hay recetas para eliminar: {ex.Message}", "Error");
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

        private void VolverAlMenu(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea volver al menú? Los datos no guardados se perderán.", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {

                var recetaWindow = Application.Current.Windows.OfType<RecetaEstandarListaGeneral>().FirstOrDefault();
                if (recetaWindow != null)
                {
                    recetaWindow.Close();
                }
            }
        }
    }
}
