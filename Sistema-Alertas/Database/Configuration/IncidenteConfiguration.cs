using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema_Alertas.Entites;

namespace Sistema_Alertas.Database.Configuration
{
    public class IncidenteConfiguration : IEntityTypeConfiguration<Incidente>
    {
        public void Configure(EntityTypeBuilder<Incidente> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Descripcion);
            builder.Property(x => x.Evidencia);
            builder.Property(x => x.Latitud);
            builder.Property(x => x.Longitud);
            builder.Property(x => x.Tipo);
        }
    }
}
