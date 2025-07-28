using Microsoft.EntityFrameworkCore;
using Sofware_CMJC_Version_1._0.Modelo;

namespace Sofware_CMJC_Version_1._0.Contexto_CMJC
{
    public class CMJC_Context: DbContext
    {
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Diagnostico> Diagnosticos { get; set; }
        public DbSet<HistoriaClinica> HistoriasClinicas { get; set; }
        public DbSet<HistoriaUsuario> HistoriasUsuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Esto permite establecer restricciones para evitar la eliminación en cascada y garantizar la integridad referencial entre las entidades
            modelBuilder.Entity<HistoriaUsuario>()
                .HasOne(hu => hu.Usuario)
                .WithMany(u => u.HistoriasUsuarios)
                .HasForeignKey(hu => hu.ID_Usuario)
                .OnDelete(DeleteBehavior.Restrict); // o .OnDelete(DeleteBehavior.NoAction)

            modelBuilder.Entity<HistoriaUsuario>()
                .HasOne(hu => hu.HistoriaClinica)
                .WithMany(hc => hc.HistoriasUsuarios)
                .HasForeignKey(hu => hu.ID_HistoriaClinica)
                .OnDelete(DeleteBehavior.Restrict); // o .OnDelete(DeleteBehavior.NoAction)

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:DESKTOP-NEKBELD,49500;Database=CMJC; User Id=CMJC; Password=CMJC2024;TrustServerCertificate=True");
        }


    }
}
