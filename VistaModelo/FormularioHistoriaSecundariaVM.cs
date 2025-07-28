using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.Vistas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class FormularioHistoriaSecundariaVM : BaseVM
    {
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
        private List<Diagnostico> diagnosticos;
        private Paciente pacienteSeleccionado;
        public Paciente PacienteSeleccionado
        {
            get { return pacienteSeleccionado; }
            set
            {
                pacienteSeleccionado = value;
                OnPropertyChanged(nameof(PacienteSeleccionado));
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
        public ICommand ExportarCommand { get; private set; }
        public ICommand AddRecetaEstandarCommand { get; private set; }
        public ICommand VolverAlMenuCommand { get; private set; }
        public FormularioHistoriaSecundariaVM(string userFullName,Consulta nuevaConsulta,Paciente pacienteSeleccionado) 
        {
            UserFullName= userFullName;
            NuevaConsulta= nuevaConsulta;
            PacienteSeleccionado=pacienteSeleccionado;

            int idpaciente = pacienteSeleccionado.ID_Paciente;
            using (var context = new CMJC_Context())
            {
                // Consulta para obtener la historia clínica del paciente seleccionado
                var historiaClinica = context.HistoriasClinicas
                    .Where(hc => hc.ID_Paciente == idpaciente)
                    .FirstOrDefault();
                HistoriaClinica = historiaClinica;
            }

            ExportarCommand = new CommandVM(ExportarAWord);
            VolverAlMenuCommand = new CommandVM(VolverAlMenu);
            AddRecetaEstandarCommand = new CommandVM(AddReceta);
        }

        public void GuardarDiagnosticos(List<UniformGrid> grids)
        {
            try
            {
                diagnosticos = new List<Diagnostico>();
                foreach (var grid in grids)
                {
                    TextBox cie10TextBox = grid.Children.OfType<TextBox>().FirstOrDefault();
                    TextBox descripcionTextBox = grid.Children.OfType<TextBox>().Skip(1).FirstOrDefault();
                    CheckBox tipoDiagnosticoCheckBox = grid.Children.OfType<CheckBox>().FirstOrDefault();

                    if (EsDiagnosticoValido(cie10TextBox, descripcionTextBox, tipoDiagnosticoCheckBox))
                    {
                        string cie10 = cie10TextBox.Text;
                        string descripcion = descripcionTextBox.Text;
                        string tipoDiagnostico = ObtenerTipoDiagnostico(grid);

                        Diagnostico nuevoDiagnostico = new Diagnostico
                        {
                            CIE10 = cie10,
                            DescripcionDiagnostico = descripcion,
                            TipoDiagnostico = tipoDiagnostico
                        };
                        // Configurar la relación con la consulta actual
                        nuevoDiagnostico.Consulta = NuevaConsulta;

                        diagnosticos.Add(nuevoDiagnostico);
                    }
                }

                using (var dbContext = new CMJC_Context())
                {
                    try
                    {
                        // Agregar la nueva consulta a la base de datos
                        dbContext.Consultas.Add(NuevaConsulta);

                        // Agregar los diagnósticos a la base de datos
                        dbContext.Diagnosticos.AddRange(diagnosticos);

                        PacienteSeleccionado.Consultas++;

                        // Guardar los cambios en el paciente seleccionado y los diagnósticos
                        dbContext.Entry(PacienteSeleccionado).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        // Mostrar mensaje de éxito
                        MessageBox.Show("La consulta se guardó correctamente.", "Éxito");
                    }
                    catch (DbUpdateException dbEx)
                    {
                        // Detalles específicos de Entity Framework
                        Exception innerEx = dbEx.InnerException;

                        while (innerEx != null)
                        {
                            MessageBox.Show($"Error de base de datos: {innerEx.Message}", "Error");
                            innerEx = innerEx.InnerException;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al Procesar el diagnostico: {ex.Message}", "Error");
            }
        }        

        private bool EsDiagnosticoValido(TextBox cie10TextBox, TextBox descripcionTextBox, CheckBox tipoDiagnosticoCheckBox)
        {

            if (cie10TextBox == null || descripcionTextBox == null || tipoDiagnosticoCheckBox == null)
            {
                MessageBox.Show("Por favor, complete todos los campos para el diagnóstico.", "Error");
                return false;
            }

            return true;
        }
        // Método para obtener el tipo desde los CheckBox
        private string ObtenerTipoDiagnostico(UniformGrid grid)
        {
            // Crear una cadena para almacenar los tipos seleccionados
            StringBuilder tiposSeleccionados = new StringBuilder();

            foreach (var child in grid.Children)
            {
                // Verificar si el control hijo es un CheckBox
                if (child is CheckBox checkBox)
                {
                    // Verificar si el CheckBox está marcado
                    if (checkBox.IsChecked == true)
                    {
                        // Obtener el contenido del CheckBox (tipo)
                        string tipo = checkBox.Content.ToString();

                        // Mapear el contenido del CheckBox a la cadena deseada
                        switch (tipo)
                        {
                            case "P":
                                tiposSeleccionados.Clear();
                                tiposSeleccionados.Append("Presuntivo");
                                break;
                            case "D":
                                tiposSeleccionados.Clear();
                                tiposSeleccionados.Append("Definitivo");
                                break;
                            case "R":
                                tiposSeleccionados.Clear();
                                tiposSeleccionados.Append("Repetitivo");
                                break;
                        }
                        // Romper el bucle después de encontrar un CheckBox marcado
                        break;
                    }
                }
            }

            // Devolver la cadena resultante
            return tiposSeleccionados.ToString();
        }
        // Método para exportar la información a Word (Formato Basico)
        public void ExportarAWord(object parameter)
        {
            try
            {
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
                        // Agregar un título al documento
                        var fecha = doc.InsertParagraph($"\nFecha: {NuevaConsulta.Fecha}");
                        fecha.Font("Times New Roman").FontSize(12).Bold().Alignment = Alignment.right;
                        var titulo = doc.InsertParagraph($"\nHistoria Clinica N°: {HistoriaClinica.Codigo}");
                        titulo.Font("Times New Roman").FontSize(16).Bold().Alignment = Alignment.center;
                        doc.InsertParagraph("\n");
                        doc.InsertParagraph("\n");

                        // Agregar información del paciente
                        doc.InsertParagraph("\nInformación del Paciente:");
                        doc.InsertParagraph($"\nNombre: {PacienteSeleccionado.Nombres} {PacienteSeleccionado.Apellidos}");
                        doc.InsertParagraph($"\nFecha de Nacimiento: {PacienteSeleccionado.FechaNacimiento}");
                        doc.InsertParagraph($"\nEdad: {PacienteSeleccionado.Edad}");
                        doc.InsertParagraph($"\nOcupación: {PacienteSeleccionado.Ocupacion}");
                        doc.InsertParagraph($"\nDirección: {PacienteSeleccionado.Direccion}");
                        doc.InsertParagraph($"\nAseguradora: {PacienteSeleccionado.Aseguradora}");
                        doc.InsertParagraph($"\nSexo: {PacienteSeleccionado.Genero}");
                        doc.InsertParagraph($"\nDocumento de ID.: {PacienteSeleccionado.TipoIdentidad}");
                        doc.InsertParagraph($"\nN° ID: {PacienteSeleccionado.DIdentidad}");
                        doc.InsertParagraph($"\nEstado Civil: {PacienteSeleccionado.EstadoCivil}");
                        doc.InsertParagraph($"\nTipo Atención: {NuevaConsulta.TipoAtencion}");
                        doc.InsertParagraph($"\nN° Atención: {NuevaConsulta.NAtencion}");
                        doc.InsertParagraph("\nMedico: Dr.Julio César Agurto Urcia");
                        doc.InsertParagraph("\nEspecialidad: Traumatología");
                        doc.InsertParagraph("\n");
                        doc.InsertParagraph("\n");

                        // Agregar información de la historia clínica Basica
                        doc.InsertParagraph("\nAntecedentes:");
                        doc.InsertParagraph($"\nAntecedentes Familiares: {HistoriaClinica.AntecedentesFamiliares}");
                        doc.InsertParagraph($"\nAntecedentes Patológicos: {HistoriaClinica.AntecedentesPatologicos}");
                        doc.InsertParagraph($"\nAntecedentes Quirúrgicos: {HistoriaClinica.AntecedentesQuirurgicos}");
                        doc.InsertParagraph($"\nAlergias: {HistoriaClinica.Alergias}");
                        doc.InsertParagraph("\n");
                        doc.InsertParagraph("\n");
                        // Agregar información de la consulta actual
                        if (NuevaConsulta!=null)
                        {
                            var consultaActual = NuevaConsulta;
                            doc.InsertParagraph($"\nSintomas y Signos: {consultaActual.SintomasySignos}");
                            doc.InsertParagraph("\n");
                            doc.InsertParagraph("\nExamen Físico");
                            doc.InsertParagraph($"FC: {consultaActual.FC}  ");
                            doc.InsertParagraph($"FR: {consultaActual.FR}  ");
                            doc.InsertParagraph($"T°: {consultaActual.Temperatura}  ");
                            doc.InsertParagraph($"PA: {consultaActual.PresionArterial} ");
                            doc.InsertParagraph($"Peso: {consultaActual.Peso}  ");
                            doc.InsertParagraph($"Talla: {consultaActual.Talla}  ");
                            doc.InsertParagraph($"IMC: {consultaActual.IMC}  ");
                            doc.InsertParagraph("\n");
                            doc.InsertParagraph($"\nDescripción Adicional: {consultaActual.DescripcionExamenFisico}");
                            doc.InsertParagraph($"\nExamenes Auxiliares: {consultaActual.ExamenesAuxiliares}");
                            doc.InsertParagraph("\n");
                            doc.InsertParagraph("\n");

                            // Agregar información de los diagnósticos de la consulta actual
                            doc.InsertParagraph("\nDiagnósticos:");
                            foreach (var diagnostico in consultaActual.Diagnosticos)
                            {
                                doc.InsertParagraph($"CIE10: {diagnostico.CIE10}  ");
                                doc.InsertParagraph($"Descripción: {diagnostico.DescripcionDiagnostico}  ");
                                doc.InsertParagraph($"Tipo Diagnostico: {diagnostico.TipoDiagnostico}  ");
                                doc.InsertParagraph("\n");
                            }
                            doc.InsertParagraph("\n");
                            doc.InsertParagraph("\n");
                            doc.InsertParagraph($"\nTratamiento: {consultaActual.Tratamiento}");
                            doc.InsertParagraph($"\nInterconsulta: {consultaActual.Interconsulta}");
                        }
                        else
                        {
                            doc.InsertParagraph("No hay información de consultas disponibles para mostrar.");
                        }

                        doc.InsertParagraph("\n");
                        var usuario = doc.InsertParagraph($"\nUsuario: {UserFullName}");
                        usuario.Font("Times New Roman").FontSize(12).Bold().Alignment = Alignment.both;
                        doc.InsertParagraph("\n");
                        var firma = doc.InsertParagraph($"\n--------------------------");
                        firma.Font("Times New Roman").FontSize(12).Bold().Alignment = Alignment.both;
                        // Guardar el documento Word
                        doc.Save();

                    }

                    byte[] archivoWordBytes = File.ReadAllBytes(rutaArchivo);
                    // Actualizar la propiedad en la base de datos?? pendiente
                    NuevaConsulta.HistoriaConsultaDiagnosticosWord = archivoWordBytes;

                    using (var dbContext = new CMJC_Context())
                    {
                        // Guardar los cambios en la consulta actual
                        dbContext.Entry(NuevaConsulta).Property(x => x.HistoriaConsultaDiagnosticosWord).IsModified = true;
                        dbContext.SaveChanges(); // Guarda los cambios en la base de datos
                    }

                    Console.WriteLine($"Exportación exitosa a: {rutaArchivo}");
                    MessageBox.Show("Se ha exportado correctamente.", "Éxito");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al exportar a Word: {ex.Message}");
                MessageBox.Show($"Error Exportar: {ex.Message}", "Error");
            }
        }
        private void VolverAlMenu(object parameter)
        {

                var historiaSecundarialWindow = Application.Current.Windows.OfType<FormularioHistoriaSecundaria>().FirstOrDefault();
                if (historiaSecundarialWindow != null)
                {
                    historiaSecundarialWindow.Close();
                }
         }

        private void AddReceta(object parameter)
        {
            //Comprueba si hay una consulta.
            if (NuevaConsulta!= null)
            {
                //Ocultar la venta Histria Secundaria
                var historiaSecundarialWindow = Application.Current.Windows.OfType<FormularioHistoriaSecundaria>().FirstOrDefault();
                if (historiaSecundarialWindow != null)
                {
                    historiaSecundarialWindow.Close();
                }

                // Abre la ventana RecetaEstandar llevando consigo el UserFullName y la Consulta.
                var recetaEstandarVM = new RecetaEstandarVM(UserFullName, NuevaConsulta);
                var recetaEstandarWindow = new RecetaEstandar
                {
                    DataContext = recetaEstandarVM
                };

                // Muestra la ventana Ventana Receta
                recetaEstandarWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error al abrir la ventana.", "Advertencia");
            }
        }
    }
}
