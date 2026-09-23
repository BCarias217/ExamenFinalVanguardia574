using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Data;
using TicketExpress.Models;

namespace TicketExpress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoletosController : ControllerBase
    {
        private readonly TicketExpressDbContext _context;

        public BoletosController(TicketExpressDbContext context)
        {
            _context = context;
        }

        // GET: api/Boletos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Boleto>>> GetBoletos()
        {
            return await _context.Boletos.ToListAsync();
        }

        // GET: api/Boletos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Boleto>> GetBoleto(int id)
        {
            var boleto = await _context.Boletos.FindAsync(id);
            if (boleto == null)
                return NotFound();

            return boleto;
        }

        // POST: api/Boletos
        [HttpPost]
        public async Task<ActionResult<Boleto>> PostBoleto(Boleto boleto)
        {
            boleto.FechaCompra = DateTime.Now;
            _context.Boletos.Add(boleto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBoleto), new { id = boleto.Id }, boleto);
        }
    }
}