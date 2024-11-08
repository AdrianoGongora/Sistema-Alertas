using Microsoft.EntityFrameworkCore;
using Sistema_Alertas.Database;
using Sistema_Alertas.Entites;

namespace Sistema_Alertas.Repository
{
    public class InicidenteRepository(ApplicationDbContext dbContext) : IIncidenteRepository
    {
        public async Task<List<Incidente>> GetAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Incidentes.ToListAsync(cancellationToken);
        }

        public async Task SaveAsync(Incidente incidente, CancellationToken cancellationToken)
        {
            dbContext.Add(incidente);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
