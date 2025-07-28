using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sofware_CMJC_Version_1._0.Modelo
{
    public class Usuario
    {
        [Key]
        public int ID_Usuario { get; set; }
        [Required]
        public string LoginName { get; set; }
        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        public string TipoUsuario { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public int NHistoria {  get; set; }
        public string UserFullName
        {
            get { return $"{Nombres} {Apellidos}"; }
        }


        // Relación uno a muchos con HistoriaUsuario
        public virtual ICollection<HistoriaUsuario> HistoriasUsuarios { get; set; }
    }
}