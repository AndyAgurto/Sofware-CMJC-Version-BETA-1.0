using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sofware_CMJC_Version_1._0.Modelo
{
    public class Paciente
    {
        [Key]
        public int ID_Paciente { get; set; }
        [Required]
        public string DIdentidad {  get; set; }
        [Required]
        public string TipoIdentidad { get; set; }
        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime FechaNacimiento { get; set; }
        [NotMapped]
        public int Edad
        {
            get
            {
                // Calcular la edad basándose en la fecha de nacimiento
                DateTime now = DateTime.Now;
                int age = now.Year - FechaNacimiento.Year;

                // Reducir la edad si aún no ha pasado su cumpleaños este año
                if (now.Month < FechaNacimiento.Month || (now.Month == FechaNacimiento.Month && now.Day < FechaNacimiento.Day))
                {
                    age--;
                }

                return age;
            }
        }

        [Required]
        public string Genero { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string Aseguradora { get; set; }
        [Required]
        public string EstadoCivil { get; set; }
        [Required]
        public string Ocupacion { get; set; }
        [Required]
        public string TipoSangre { get; set;}
        [Required]
        public string Estado {  get; set; }
        [Required]
        public DateTime FechaAgregado {  get; set; }

        [NotMapped]
        public string NombreCompleto
        {
            get { return $"{Nombres} {Apellidos}"; }

        }
        [Required]
        public int Consultas {  get; set; }
        [Required]
        public string AsignacionHistoria { get; set; }//Asignado / Sin Asignar

        // Relación uno a uno con HistoriasClinicas
        public virtual HistoriaClinica HistoriaClinica { get; set; }
    }
}
