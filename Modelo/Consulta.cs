using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Sofware_CMJC_Version_1._0.Modelo
{
    public class Consulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Consulta { get; set; }
        [Required]
        public int FC { get; set; }  // bpm
        [Required]
        public string FR { get; set; }  
        [Required]
        public double Temperatura { get; set; }
        [Required]
        public string PresionArterial { get; set; }
        [Required]
        public double Peso { get; set; }  
        [Required]
        public double Talla { get; set; }  
        [Required]
        public double IMC { get; set; }  
        [Required]
        public string DescripcionExamenFisico { get; set; }
        [Required]
        public string SintomasySignos { get; set; }
        [Required]
        public string ExamenesAuxiliares { get; set; }  
        [Required]
        public string Tratamiento { get; set; }  
        [Required]
        public string Interconsulta { get; set; }
        [Required]
        public string NAtencion { get; set; }// Numero de atencion Generar(N°1)
        [Required]
        public string TipoAtencion { get; set; }// Ambulatorio o Emergencia
        [Required]
        public DateTime Fecha { get; set; } //fecha de atencion
        public int CRecetas {  get; set; }

        public Byte[]? HistoriaConsultaDiagnosticosWord { get; set; }

        // Clave foránea
        [ForeignKey("HistoriaClinica")]
        public int ID_HistoriaClinica { get; set; }
        public virtual HistoriaClinica HistoriaClinica { get; set; }

        // Relación uno a muchos con Diagnosticos
        public List<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();

        // Relación uno a muchos con Recetas
        public List<Receta> Recetas { get; set; } = new List<Receta>();


    }
}
