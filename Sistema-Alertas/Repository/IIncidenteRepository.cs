using Sistema_Alertas.Entites;

namespace Sistema_Alertas.Repository
{
    public interface IIncidenteRepository
    {
        Task SaveAsync(Incidente incidente, CancellationToken cancellationToken);
        Task<List<Incidente>> GetAsync(CancellationToken cancellationToken);
    }
}
