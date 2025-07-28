using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class AddPacienteVM : BaseVM
    {
        
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
                docIdentidadSeleccionado=value;
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
        public ICommand GuardarCommand { get; private set; }

        public AddPacienteVM()
        {
            Generos = new ObservableCollection<string>
            {
                "Masculino",
                "Femenino",
                "Otro"
            };

            DocIdentidad = new ObservableCollection<string>
            {
                "DNI",
                "Cédula de Identidad",
                "Cédula de Ciudadanía",
                "Pasaporte"
            };

            EstadosCiviles = new ObservableCollection<string>
            {
                "Soltero(a)",
                "Casado(a)",
                "Divorciado(a)",
                "Viudo(a)"
            };
            Sangres= new ObservableCollection<string>
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

            // Inicializa tus propiedades y otros datos necesarios.
            GuardarCommand = new CommandVM(Guardar, PuedeGuardar);
         }

        // Método para verificar si se puede ejecutar el comando Guardar
        private bool PuedeGuardar(object parameter)
        {

            return !string.IsNullOrWhiteSpace(DIdentidad)
                && !string.IsNullOrWhiteSpace(Nombres)
                && !string.IsNullOrWhiteSpace(Apellidos)
                && !string.IsNullOrWhiteSpace(Genero)
                && !string.IsNullOrWhiteSpace(Direccion)
                && !string.IsNullOrWhiteSpace(Telefono)
                && !string.IsNullOrWhiteSpace(Aseguradora)
                && !string.IsNullOrWhiteSpace(EstadoCivil)
                && !string.IsNullOrWhiteSpace(Ocupacion);
        }

        // Método para ejecutar la acción de guardar (PROBLEMAS)
        private void Guardar(object parameter)
        {
            try
            {
                using (var dbContext = new CMJC_Context())
                {
                    var nuevoPaciente = new Paciente
                    {
                        TipoIdentidad = DocIdentidadSeleccionado,
                        DIdentidad = DIdentidad,
                        Nombres = Nombres,
                        Apellidos = Apellidos,
                        FechaNacimiento = FechaNacimiento,
                        Genero = Genero,
                        Direccion = Direccion,
                        Telefono = Telefono,
                        Aseguradora = Aseguradora,
                        EstadoCivil = EstadoCivil,
                        Ocupacion = Ocupacion,
                        TipoSangre = SangreSeleccionado,
                        FechaAgregado = DateTime.Now,
                        Estado = "Activo",
                        AsignacionHistoria = "Sin Asignar",
                        Consultas = 0
                    };

                    // Agrega el nuevo paciente a la base de datos
                    dbContext.Pacientes.Add(nuevoPaciente);
                    dbContext.SaveChanges();

                }

                MessageBox.Show("Paciente guardado exitosamente.", "Éxito");

                Limpiar();
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir durante el proceso de guardado
                MessageBox.Show($"Error al guardar el paciente: {ex.Message}", "Error");
            }
        }
        public void Limpiar()
        {
            DIdentidad = string.Empty;
            Nombres = string.Empty;
            Apellidos = string.Empty;
            FechaNacimiento = DateTime.Now;
            Genero = string.Empty;
            Direccion = string.Empty;
            Telefono = string.Empty;
            Aseguradora = string.Empty;
            EstadoCivil = string.Empty;
            Ocupacion = string.Empty;
        }
    }
}
