namespace Sistema_Alertas.Entites
{
    public sealed class Incidente
    {
        public Guid Id { get; set; }
        public string Longitud { get; set; } = string.Empty;
        public string Latitud { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Evidencia { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
