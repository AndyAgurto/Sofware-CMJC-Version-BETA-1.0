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
    public class EditPacienteVM : BaseVM
    {
        private Paciente pacienteSeleccionado;
        private string dIdentidad;
        private string nombres;
        private string apellidos;
        private DateTime fechaNacimiento;
        private string genero;
        private string direccion;
        private string telefono;
        private string aseguradora;
        private string estadoCivil;
        private string ocupacion;
        private string docIdentidadSeleccionado;
        private string sangreSeleccionado;
        public string DocIdentidadSeleccionado
        {
            get { return docIdentidadSeleccionado; }
            set
            {
                docIdentidadSeleccionado = value;
                OnPropertyChanged(nameof(DocIdentidadSeleccionado));
            }
        }
        public string SangreSeleccionado
        {
            get { return sangreSeleccionado; }
            set
            {
                sangreSeleccionado = value;
                OnPropertyChanged(nameof(SangreSeleccionado));
            }
        }
        public string DIdentidad
        {
            get { return dIdentidad; }
            set
            {
                if (value != null)
                {
                    // Obtén el tipo de documento seleccionado
                    string tipoDocumento = DocIdentidadSeleccionado;

                    // Determina la cantidad de dígitos permitidos según el tipo de documento
                    int maxDigitos = 0;

                    switch (tipoDocumento)
                    {
                        case "DNI":
                            maxDigitos = 8;
                            break;
                        case "Cédula de Identidad":
                            maxDigitos = 12;
                            break;
                        case "Cédula de Ciudadanía":
                            maxDigitos = 12;
                            break;
                        case "Pasaporte":
                            break;

                    }

                    // Validación específica para pasaportes (permite letras)
                    if (tipoDocumento == "Pasaporte")
                    {

                        if (System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z0-9]+$"))
                        {
                            dIdentidad = value;
                        }
                    }
                    else
                    {
                        // Validación para otros tipos de documentos (solo números)
                        if (System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d{0," + maxDigitos + "}$"))
                        {
                            dIdentidad = value;
                        }
                        else
                        {
                            MessageBox.Show($"Por favor, ingrese un número de {tipoDocumento} válido ({maxDigitos} dígitos).");
                        }
                    }
                }
                else
                {
                    dIdentidad = value;
                }

                OnPropertyChanged(nameof(dIdentidad));
            }
        }

        public string Nombres
        {
            get { return nombres; }
            set
            {
                nombres = value;
                OnPropertyChanged(nameof(Nombres));
            }
        }
        public string Apellidos
        {
            get { return apellidos; }
            set
            {
                apellidos = value;
                OnPropertyChanged(nameof(Apellidos));
            }
        }
        public DateTime FechaNacimiento
        {
            get { return fechaNacimiento; }
            set
            {
                fechaNacimiento = value;
                OnPropertyChanged(nameof(FechaNacimiento));
            }
        }
        public string Genero
        {
            get { return genero; }
            set
            {
                genero = value;
                OnPropertyChanged(nameof(Genero));
            }
        }
        public string Direccion
        {
            get { return direccion; }
            set
            {
                direccion = value;
                OnPropertyChanged(nameof(Direccion));
            }
        }
        public string Telefono
        {
            get { return telefono; }
            set
            {
                if (value != null)
                {
                    // Utiliza una expresión regular para verificar si la entrada es un número de 8 dígitos
                    if (System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d{0,9}$"))
                    {
                        telefono = value;
                    }
                    else
                    {
                        // Muestra una advertencia o mensaje de error si la entrada no es válida
                        MessageBox.Show("Ingreso de solo 9 digitos.");
                    }
                }
                else
                {
                    telefono = value;
                }

                OnPropertyChanged(nameof(Telefono));
            }
        }
        public string Aseguradora
        {
            get { return aseguradora; }
            set
            {
                aseguradora = value;
                OnPropertyChanged(nameof(Aseguradora));
            }
        }
        public string EstadoCivil
        {
            get { return estadoCivil; }
            set
            {
                estadoCivil = value;
                OnPropertyChanged(nameof(estadoCivil));
            }
        }
        public string Ocupacion
        {
            get { return ocupacion; }
            set
            {
                ocupacion = value;
                OnPropertyChanged(nameof(Ocupacion));
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
        private ObservableCollection<string> generos;
        public ObservableCollection<string> Generos
        {
            get => generos;
            set
            {
                generos = value;
                OnPropertyChanged(nameof(Generos));
            }
        }
        private ObservableCollection<string> estadosCiviles;
        public ObservableCollection<string> EstadosCiviles
        {
            get => estadosCiviles;
            set
            {
                estadosCiviles = value;
                OnPropertyChanged(nameof(EstadosCiviles));
            }
        }
        private ObservableCollection<string> sangres;
        public ObservableCollection<string> Sangres
        {
            get => sangres;
            set
            {
                sangres = value;
                OnPropertyChanged(nameof(Sangres));
            }
        }
        private ObservableCollection<string> docIdentidad;
        public ObservableCollection<string> DocIdentidad
        {
            get => docIdentidad;
            set
            {
                docIdentidad = value;
                OnPropertyChanged(nameof(DocIdentidad));
            }
        }
        public ICommand ActualizarCommand { get; private set; }
        public EditPacienteVM(Paciente pacienteSeleccionado)
        {
            PacienteSeleccionado= pacienteSeleccionado;
            CargarDatosPacienteEnVM(PacienteSeleccionado);

            Generos = new ObservableCollection<string>
            {
                "Masculino",
                "Femenino",
                "Otro"
            };

            EstadosCiviles = new ObservableCollection<string>
            {
                "Soltero(a)",
                "Casado(a)",
                "Divorciado(a)",
                "Viudo(a)"
            };
            DocIdentidad = new ObservableCollection<string>
            {
                "DNI",
                "Cédula de Identidad",
                "Cédula de Ciudadanía",
                "Pasaporte"
            };
            Sangres = new ObservableCollection<string>
            {
                "A+",
                "A-",
                "B+",
                "B-",
                "AB+",
                "AB-",
                "O+",
                "O-"
            };
            ActualizarCommand = new CommandVM(ActualizarPaciente, PuedeActualizarPaciente);
        }
        public void CargarDatosPacienteEnVM(Paciente paciente)
        {
            // Asigna los valores del paciente a las propiedades de la ViewModel
            DocIdentidadSeleccionado = paciente.TipoIdentidad;
            DIdentidad = paciente.DIdentidad;
            Nombres = paciente.Nombres;
            Apellidos = paciente.Apellidos;
            FechaNacimiento = paciente.FechaNacimiento;
            Genero = paciente.Genero;
            Direccion = paciente.Direccion;
            Telefono = paciente.Telefono;
            Aseguradora = paciente.Aseguradora;
            EstadoCivil = paciente.EstadoCivil;
            Ocupacion = paciente.Ocupacion;
            SangreSeleccionado = paciente.TipoSangre;

        }
        private void ActualizarPaciente(object parameter)
        {
            if (PacienteSeleccionado != null)
            {
                try
                {
                    // Actualiza el paciente en la base de datos
                    using (var dbContext = new CMJC_Context())
                    {
                        var paciente = dbContext.Pacientes.FirstOrDefault(p => p.ID_Paciente == PacienteSeleccionado.ID_Paciente);

                        if (paciente != null)
                        {
                            // Actualiza las propiedades del paciente con las nuevas del PacienteSeleccionado
                            paciente.TipoIdentidad = DocIdentidadSeleccionado;
                            paciente.DIdentidad = DIdentidad;
                            paciente.Nombres =Nombres;
                            paciente.Apellidos = Apellidos;
                            paciente.FechaNacimiento = FechaNacimiento;
                            paciente.Genero = Genero;
                            paciente.Direccion = Direccion;
                            paciente.Telefono = Telefono;
                            paciente.Aseguradora = Aseguradora;
                            paciente.EstadoCivil = EstadoCivil;
                            paciente.Ocupacion = Ocupacion;

                            dbContext.SaveChanges();
                        }
                    }

                    MessageBox.Show("Paciente actualizado exitosamente.", "Mensaje");

                    PacienteSeleccionado = null;
                }
                catch (Exception ex)
                {
                    // Maneja cualquier excepción que pueda ocurrir durante la actualización
                    MessageBox.Show($"Error al actualizar el paciente: {ex.Message}", "Error");
                }
            }
            else
            {
                // Muestra un mensaje indicando que no hay paciente seleccionado
                MessageBox.Show("No existe paciente seleccionado para actualizar.", "Mensaje");
            }
        }
        private bool PuedeActualizarPaciente(object parameter)
        {
            // Verifica si hay un paciente seleccionado en la lista
            return PacienteSeleccionado != null;
        }
    }
}
