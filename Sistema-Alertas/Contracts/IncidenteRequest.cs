namespace Sistema_Alertas.Contracts
{
    public sealed record IncidenteRequest(string Longitud, string Latitud, string Tipo, string Evidencia, string Descripcion);
}
