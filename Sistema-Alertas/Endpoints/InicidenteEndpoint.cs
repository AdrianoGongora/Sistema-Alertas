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

            app.MapGet("api/last/incidente", async (IIncidenteRepository inic, CancellationToken cancellationToken) =>
            {
                // Obtener la lista completa de incidentes
                var incidentes = await inic.GetAsync(cancellationToken);

                // Verificar que la lista no esté vacía
                if (incidentes == null || !incidentes.Any())
                {
                    return Results.NotFound("No se encontraron incidentes.");
                }

                // Obtener el último incidente
                var ultimoIncidente = incidentes.Last();

                // Extraer la latitud y longitud del último incidente
                var latitud = ultimoIncidente.Latitud;
                var longitud = ultimoIncidente.Longitud;

                // Obtener el nombre de la dirección a partir de la latitud y longitud
                var direccion = await ObtenerDisplayName(latitud, longitud);

                // Crear la respuesta incluyendo el incidente y la dirección
                var respuesta = new
                {
                    Incidente = ultimoIncidente,
                    Direccion = direccion
                };

                return Results.Ok(respuesta);
            }).WithTags("Incidente");
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

                // Leer contenido de la respuesta como texto para diagnóstico
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Contenido de la respuesta: " + content); // Ver el contenido en bruto

                var data = await response.Content.ReadFromJsonAsync<NominatimResponse>();
                return data?.display_name ?? "Nombre de ubicación desconocido";
            }
            catch (HttpRequestException ex)
            {
                return $"Error al conectar con el servicio de geolocalización: {ex.Message}";
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
