namespace TicketExpress.Models
{
    public class Boleto
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public Evento? Evento { get; set; }

        public string NombreComprador { get; set; } = string.Empty;
        public string CorreoComprador { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public DateTime FechaCompra { get; set; }
    }
}