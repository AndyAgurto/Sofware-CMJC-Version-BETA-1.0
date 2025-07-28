using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using Sofware_CMJC_Version_1._0.Contexto_CMJC;
using Sofware_CMJC_Version_1._0.Modelo;
using Sofware_CMJC_Version_1._0.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sofware_CMJC_Version_1._0.VistaModelo
{
    public class FormularioHistoriaPrincipalVM : BaseVM
    {
        //Cargar datos del Paciente
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
        private string _nAtencion;
        public string NAtencion
        {
            get { return _nAtencion; }
            set
            {
                if (_nAtencion != value)
                {
                    _nAtencion = value;
                    OnPropertyChanged(nameof(NAtencion));
                }
            }
        }

        private string _tipoAtencion;
        public string TipoAtencion
        {
            get { return _tipoAtencion; }
            set
            {
                if (_tipoAtencion != value)
                {
                    _tipoAtencion = value;
                    OnPropertyChanged(nameof(TipoAtencion));
                }
            }
        }
        private string _familiares;
        public string Familiares
        {
            get { return _familiares; }
            set
            {
                if (_familiares != value)
                {
                    _familiares = value;
                    OnPropertyChanged(nameof(Familiares));
                }
            }
        }

        private string _patologicos;
        public string Patologicos
        {
            get { return _patologicos; }
            set
            {
                if (_patologicos != value)
                {
                    _patologicos = value;
                    OnPropertyChanged(nameof(Patologicos));
                }
            }
        }

        private string _quirurgicos;
        public string Quirurgicos
        {
            get { return _quirurgicos; }
            set
            {
                if (_quirurgicos != value)
                {
                    _quirurgicos = value;
                    OnPropertyChanged(nameof(Quirurgicos));
                }
            }
        }

        private string _alergias;
        public string Alergias
        {
            get { return _alergias; }
            set
            {
                if (_alergias != value)
                {
                    _alergias = value;
                    OnPropertyChanged(nameof(Alergias));
                }
            }
        }

        private string _fechaNacimientoConEdad;
        public string FechaNacimientoConEdad
        {
            get { return _fechaNacimientoConEdad; }
            set
            {
                if (_fechaNacimientoConEdad != value)
                {
                    _fechaNacimientoConEdad = value;
                    OnPropertyChanged(nameof(FechaNacimientoConEdad));
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
        // Método para asignar valores a las propiedades
        private void AsignarValores()
        {
            if (PacienteSeleccionado != null && HistoriaClinica!=null)
            {
                Familiares = HistoriaClinica.AntecedentesFamiliares;
                Patologicos = HistoriaClinica.AntecedentesPatologicos;
                Quirurgicos = HistoriaClinica.AntecedentesQuirurgicos;
                Alergias = HistoriaClinica.Alergias;
                FechaNacimientoConEdad = $"{PacienteSeleccionado.FechaNacimiento:dd/MM/yyyy} / {PacienteSeleccionado.Edad} años";
            }
        }
        //Para guardar Consulta (Clase)
        private int fc;
        public int FC
        {
            get { return fc; }
            set
            {
                if (value >= 0)
                {
                    fc = value;
                    OnPropertyChanged(nameof(FC));
                }
            }
        }
        private string fr;
        public string FR
        {
            get { return fr; }
            set
            {
                fr = value;
                OnPropertyChanged(nameof(FR));
            }
        }
        private double temperatura;
        public double Temperatura
        {
            get { return temperatura; }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    temperatura = value;
                    OnPropertyChanged(nameof(Temperatura));
                }
            }
        }
        private string presionArterial;
        public string PresionArterial
        {
            get { return presionArterial; }
            set
            {
                presionArterial = value;
                OnPropertyChanged(nameof(PresionArterial));
            }
        }
        private double peso;
        public double Peso
        {
            get { return peso; }
            set
            {
                if (value >= 0)
                {
                    peso = value;
                    OnPropertyChanged(nameof(Peso));
                }
            }
        }
        private double talla;
        public double Talla
        {
            get { return talla; }
            set
            {
                if (value >= 0)
                {
                    talla = value;
                    OnPropertyChanged(nameof(Talla));
                }
            }
        }
        private double imc;
        public double IMC
        {
            get { return imc; }
            set
            {
                imc = value;
                OnPropertyChanged(nameof(IMC));
            }
        }

        private string descripcionEF;
        public string DescripcionEF
        {
            get { return descripcionEF; }
            set
            {
                descripcionEF = value;
                OnPropertyChanged(nameof(DescripcionEF));
            }
        }
        private string sintomasYSignos;
        public string SintomasYSignos
        {
            get { return sintomasYSignos; }
            set
            {
                sintomasYSignos = value;
                OnPropertyChanged(nameof(SintomasYSignos));
            }
        }
        private string examenesA;
        public string ExamenesA
        {
            get {return examenesA;}
            set
            {
                examenesA = value;
                OnPropertyChanged(nameof(ExamenesA));
            }
        }
        private string tratamiento;
        public string Tratamiento
        {
            get { return tratamiento;}
            set
            {
                tratamiento = value;
                OnPropertyChanged(nameof(Tratamiento));
            }
        }
        private string interConsulta;
        public string InterConsulta
        {
            get { return interConsulta; }
            set
            {
                interConsulta = value;
                OnPropertyChanged(nameof(InterConsulta));
            }
        }
        public DateTime fechaActual=DateTime.Now;
        public ICommand AddConsultaCommand { get;private set; }

        public FormularioHistoriaPrincipalVM(string userFullName,Paciente pacienteSeleccionado,HistoriaClinica historiaClinica,string tipoAtencion, string numeroAtencion)
        {
            UserFullName = userFullName;
            PacienteSeleccionado = pacienteSeleccionado;
            HistoriaClinica= historiaClinica;

            AsignarValores();
            TipoAtencion = tipoAtencion;
            NAtencion = numeroAtencion;
            AddConsultaCommand = new CommandVM(AddConsulta);
        }

        private void AddConsulta(object parameter)
        {
            try
            {
                // Verificar si los campos requeridos están llenos
                if (!CamposRequeridosLlenos())
                {
                    return;
                }
                int idHistoriaClinica = HistoriaClinica.ID_HistoriaClinica;
                Consulta nuevaConsulta = new Consulta
                {
                    FC = FC,
                    FR = FR,
                    Temperatura = Temperatura,
                    PresionArterial = PresionArterial,
                    Peso = Peso,
                    Talla = Talla,
                    IMC = IMC,
                    DescripcionExamenFisico = DescripcionEF,
                    SintomasySignos = SintomasYSignos,
                    ExamenesAuxiliares = ExamenesA,
                    Tratamiento = Tratamiento,
                    Interconsulta = InterConsulta,
                    NAtencion = NAtencion,
                    TipoAtencion = TipoAtencion,
                    Fecha = fechaActual,
                    ID_HistoriaClinica = idHistoriaClinica,
                    CRecetas = 0
                };

                MessageBox.Show("Agregado correctamente.", "Éxito");

                var historiaPrincipalWindow = Application.Current.Windows.OfType<FormularioHistoriaPrincipal>().FirstOrDefault();
                if (historiaPrincipalWindow != null)
                {
                    historiaPrincipalWindow.Close();
                }

                // Abrir la ventana FormularioHistoriaClinicaSecundaria llevando consigo el UserFullName 
                var formularioHistoriaSecundariaVM = new FormularioHistoriaSecundariaVM(UserFullName, nuevaConsulta, PacienteSeleccionado);
                var formularioHistoriaSecundariaWindow = new FormularioHistoriaSecundaria
                {
                    DataContext = formularioHistoriaSecundariaVM
                };

                // Mostrar la ventana FormularioHistoriaClinicaSecundaria
                formularioHistoriaSecundariaWindow.ShowDialog();              
            }
            catch (Exception ex)
            {
                // Manejo de errores
                MessageBox.Show($"Error al añadir la consulta: {ex.Message}", "Error");
                return;
            }

        }
        private bool CamposRequeridosLlenos()
        {
            // Validar que todos los campos requeridos estén llenos
            if (FC<=0 || string.IsNullOrWhiteSpace(FR) ||
                Temperatura <= 0 || string.IsNullOrWhiteSpace(PresionArterial) ||
                Peso <= 0 || Talla <= 0 || IMC <= 0 || string.IsNullOrWhiteSpace(DescripcionEF) ||
                string.IsNullOrWhiteSpace(SintomasYSignos) || string.IsNullOrWhiteSpace(ExamenesA) ||
                string.IsNullOrWhiteSpace(Tratamiento) || string.IsNullOrWhiteSpace(InterConsulta) ||
                string.IsNullOrWhiteSpace(NAtencion) || string.IsNullOrWhiteSpace(TipoAtencion))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Error");
                return false;
            }

            return true;
        }
        private string ObtenerLoginName(string userFullName)
        {
            using (var dbContext = new CMJC_Context())
            {
                // Realizar la consulta en la base de datos para obtener el LoginName
                var usuario = dbContext.Usuarios.AsEnumerable().FirstOrDefault(u => u.UserFullName == userFullName);

                if (usuario != null)
                {
                    return usuario.LoginName;
                }

                // Puedes manejar el caso en que no se encuentre el usuario, por ejemplo, lanzando una excepción o devolviendo un valor predeterminado.
                throw new InvalidOperationException($"No se encontró Usuario o No existe: {userFullName}");
            }
        }

    }
}
