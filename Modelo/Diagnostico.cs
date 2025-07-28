using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sofware_CMJC_Version_1._0.Modelo
{
    public class Diagnostico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Diagnostico { get; set; }
        [Required]
        public string TipoDiagnostico { get; set; }
        [Required]
        public string CIE10 { get; set; } //Implementar una API DE ENFERMEDADES EN LA VERSION 1.1
        [Required]
        public string DescripcionDiagnostico {  get; set; }

        // Clave foránea
        [ForeignKey("Consulta")]
        public int ID_Consulta { get; set; }
        public virtual Consulta Consulta { get; set; }
    }
}
