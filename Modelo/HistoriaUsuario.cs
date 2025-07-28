using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sofware_CMJC_Version_1._0.Modelo
{
    public class HistoriaUsuario
    {
        [Key]
        public int ID_HistoriaUsuario { get; set; }
        // Clave foránea a HistoriasClinicas
        [ForeignKey("HistoriaClinica")]
        public int ID_HistoriaClinica { get; set; }
        public virtual HistoriaClinica HistoriaClinica { get; set; }

        // Clave foránea a Usuarios
        [ForeignKey("Usuario")]
        public int ID_Usuario { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
