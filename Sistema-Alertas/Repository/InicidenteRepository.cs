using Sistema_Alertas.Database;
using Sistema_Alertas.Entites;

namespace Sistema_Alertas.Repository
{
    public class InicidenteRepository(ApplicationDbContext dbContext) : IIncidenteRepository
    {
        public async Task SaveAsync(Incidente incidente, CancellationToken cancellationToken)
        {
            dbContext.Add(incidente);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
