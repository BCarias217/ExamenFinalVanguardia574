using Microsoft.EntityFrameworkCore;
using TicketExpress.Data;

namespace TicketExpress.Common
{
    public static class DisponibilidadHelper
    {
        public static async Task<int> BoletosDisponibles(TicketExpressDbContext context, int eventoId, int capacidadTotal)
        {
            var vendidos = await context.Boletos
                .Where(b => b.EventoId == eventoId)
                .SumAsync(b => (int?)b.Cantidad) ?? 0;

            return capacidadTotal - vendidos;
        }
    }
}