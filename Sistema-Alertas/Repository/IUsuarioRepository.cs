using Sistema_Alertas.Entites;

namespace Sistema_Alertas.Repository;

public interface IUsuarioRepository
{
    Task SaveAsync(User usuario, CancellationToken cancellationToken);
    Task<User?> GetByDniAsync(string Correo);
}
