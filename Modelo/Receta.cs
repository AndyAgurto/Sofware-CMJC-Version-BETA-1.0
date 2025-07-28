using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofware_CMJC_Version_1._0.Modelo
{
    public class Receta
    {
        [Key]
        public int ID_Receta { get; set; }

        // Clave foránea a Consulta
        [ForeignKey("Consulta")]
        public int ID_Consulta{ get; set; }
        public virtual Consulta Consulta { get; set; }

        public string Paciente { get; set; }
        public string NAtencion { get; set; }
        public string RutaArchivo { get; set; }
        // Propiedades para el archivo Word
        public byte[] ?ArchivoWord { get; set; }
        public string NombreArchivo { get; set; }
        public string TipoMIME { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaCaducidad { get; set; }
    }
}
