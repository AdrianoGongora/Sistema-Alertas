using Comercial.Api.Endpoints;
using Sistema_Alertas.Contracts;
using Sistema_Alertas.Entites;
using Sistema_Alertas.Repository;
using Sistema_Alertas.Services.Jwt;

namespace Sistema_Alertas.Endpoints;

public static class UserEnpoint
{
    public static void MapUsuarioEndpoins(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/login", async (
            LoginRequest request,
            IUsuarioRepository usuarioRepository,
            IJwtTokenGenerator jwtTokenGenerator) =>
        {
            if (string.IsNullOrWhiteSpace(request.Dni))
            {
                return Results.BadRequest("DNI y Password son requeridos.");
            }

            if (request.Dni.Length < 8)
            {
                return Results.BadRequest("El DNI debe tener al menos 8 caracteres.");
            }

            var userExist = await usuarioRepository.GetByDniAsync(request.Dni);

            if (userExist is null)
            {
                return Results.NotFound("Usuario no encontrado.");
            }

            var token = jwtTokenGenerator.GenerateToken(userExist);

            return Results.Ok(token);

        }).WithTags(Tags.Usuarios);

        app.MapPost("api/register", async (
            RegisterRequest request,
            IUsuarioRepository usuarioRepository,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(request.Nombre) || string.IsNullOrWhiteSpace(request.Dni))
            {
                return Results.BadRequest("Nombre, DNI y Password son requeridos.");
            }

            if (request.Dni.Length <= 8 && request.Dni.Length >= 8)
            {
                return Results.BadRequest("El DNI debe tener al menos 8 caracteres.");
            }

            var userExist = await usuarioRepository.GetByDniAsync(request.Dni);

            if (userExist is not null)
            {
                return Results.Conflict("Ya existe una cuenta con este DNI.");
            }

            var usuario = new User
            {
                Name = request.Nombre,
                Dni = request.Dni,
            };

            await usuarioRepository.SaveAsync(usuario, cancellationToken);

            return Results.Ok("Registro exitoso.");

        }).WithTags(Tags.Usuarios);

    }
}
