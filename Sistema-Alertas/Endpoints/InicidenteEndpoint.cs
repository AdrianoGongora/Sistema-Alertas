using Comercial.Api.Endpoints;
using Microsoft.AspNetCore.SignalR;
using Sistema_Alertas.Contracts;
using Sistema_Alertas.Entites;
using Sistema_Alertas.Repository;
using Sistema_Alertas.Services.NewFolder;

namespace Sistema_Alertas.Endpoints
{
    public static class IncidenteEndpoint
    {
        public static void MapInicidenteEndpoins(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/incidente", async (
                IncidenteRequest request,
                IIncidenteRepository incidenteRepository,
                IHubContext<NotificationHub> hubContext,
                CancellationToken cancellationToken) =>
            {
                var incidencia = new Incidente
                {
                    Descripcion = request.Descripcion,
                    Longitud = request.Longitud,
                    Latitud = request.Latitud,
                    Tipo = request.Tipo,
                    Evidencia = request.Evidencia,
                    Fecha = DateTime.UtcNow
                };

                await incidenteRepository.SaveAsync(incidencia, cancellationToken);

                string calle = await ObtenerDisplayName(request.Latitud, request.Longitud);

                var notificationMessage = $"⚠️ Nuevo Incidente Reportado: {request.Descripcion}. " +
                                          $"Tipo: {request.Tipo}. Ubicación: {calle} ({request.Latitud}, {request.Longitud}). " +
                                          $"Fecha: {incidencia.Fecha:yyyy-MM-dd HH:mm:ss} UTC.";

                Console.WriteLine(notificationMessage);

                await hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessage);

                return Results.Ok("La incidencia se registró correctamente");

            }).WithTags(Tags.Incidente);

            app.MapGet("api/incidente", async (IIncidenteRepository incidenteRepository,
                CancellationToken cancellationToken) =>
            {
                return Results.Ok(await incidenteRepository.GetAsync(cancellationToken));
            }).WithTags(Tags.Incidente);
        }

        private static async Task<string> ObtenerDisplayName(string latitud, string longitud)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");

            string url = $"https://nominatim.openstreetmap.org/reverse?lat={latitud}&lon={longitud}&format=json";

            try
            {
                var response = await httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return "Acceso denegado: no se puede obtener la información (403 Forbidden)";
                }

                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadFromJsonAsync<NominatimResponse>();
                return data?.display_name ?? "Nombre de ubicación desconocido";
            }
            catch (HttpRequestException)
            {
                return "Error al conectar con el servicio de geolocalización";
            }
        }

        // Clase para deserializar la respuesta de Nominatim
        public class NominatimResponse
        {
            public string display_name { get; set; }
            public Address address { get; set; }
        }

        public class Address
        {
            public string road { get; set; }
        }


    }
}
