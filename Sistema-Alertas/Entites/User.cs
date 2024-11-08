namespace Sistema_Alertas.Entites
{
    public sealed class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
    }
}
