using Microsoft.EntityFrameworkCore;
using Sistema_Alertas.Database;
using Sistema_Alertas.Entites;

namespace Sistema_Alertas.Repository
{
    public class UserRepository(ApplicationDbContext dbContext) : IUsuarioRepository
    {
        public async Task<User?> GetByDniAsync(string Correo)
        {
            return await dbContext.Users
                .Where(u => u.Dni == Correo)
                .FirstOrDefaultAsync();
        }

        public async Task SaveAsync(User usuario, CancellationToken cancellationToken)
        {
            dbContext.Add(usuario);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
