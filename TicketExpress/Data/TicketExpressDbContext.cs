using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress.Data
{
    public class TicketExpressDbContext : DbContext
    {
        public TicketExpressDbContext(DbContextOptions<TicketExpressDbContext> options) : base(options) { }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Boleto> Boletos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.Evento)
                .WithMany(e => e.Boletos)
                .HasForeignKey(b => b.EventoId);
        }
    }
}