using Sistema_Alertas.Entites;

namespace Sistema_Alertas.Services.Jwt;

public interface IJwtTokenGenerator
{
    string GenerateToken(User usuario);
}
