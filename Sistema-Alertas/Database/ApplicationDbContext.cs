using Microsoft.EntityFrameworkCore;
using Sistema_Alertas.Database.Configuration;
using Sistema_Alertas.Entites;

namespace Sistema_Alertas.Database
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        internal DbSet<User> Users { get; set; }
        internal DbSet<Incidente> Incidentes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new IncidenteConfiguration());
        }
    }
}
