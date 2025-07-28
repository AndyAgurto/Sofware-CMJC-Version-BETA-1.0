using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sofware_CMJC_Version_1._0.Modelo
{
    public class HistoriaClinica
    {
        [Key]
        public int ID_HistoriaClinica { get; set; }
        [Required]
        public string Codigo {  get; set; }//CMJCX donde x es el numero
        [Required]
        public string AntecedentesFamiliares {  get; set; }
        [Required]
        public string AntecedentesPatologicos {  get; set; }
        [Required]
        public string AntecedentesQuirurgicos {  get; set; }
        [Required]
        public string Alergias {  get; set; }
       
        // Clave foránea
        [ForeignKey("Paciente")]
        public int ID_Paciente { get; set; }
        public virtual Paciente Paciente { get; set; }

        // Clave foránea a Usuario
        [ForeignKey("Usuario")]
        public int ID_Usuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        // Relación uno a muchos con Consultas
        public virtual ICollection<Consulta> Consultas { get; set; }

        // Relación uno a muchos con HistoriaUsuario
        public virtual ICollection<HistoriaUsuario> HistoriasUsuarios { get; set; }
    }
}
