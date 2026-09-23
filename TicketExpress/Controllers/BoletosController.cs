using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TicketExpress.Common;
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
            var error = ValidarFormatoBoleto(boleto);
            if (error != null)
                return BadRequest(error);

            var evento = await _context.Eventos.FindAsync(boleto.EventoId);
            if (evento == null)
                return NotFound("El Evento indicado no existe.");

            boleto.NombreComprador = TextNormalizer.Normalizar(boleto.NombreComprador);
            boleto.FechaCompra = DateTime.Now;

            _context.Boletos.Add(boleto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBoleto), new { id = boleto.Id }, boleto);
        }

        // Validaciones de formato de Boleto
        private string? ValidarFormatoBoleto(Boleto boleto)
        {
            if (string.IsNullOrWhiteSpace(boleto.NombreComprador) || boleto.NombreComprador.Trim().Length < 2 || boleto.NombreComprador.Trim().Length > 100)
                return "El NombreComprador es obligatorio y debe tener entre 2 y 100 caracteres.";

            if (!Regex.IsMatch(boleto.NombreComprador, @"^[a-zA-ZÀ-ÿ\s'-]+$"))
                return "El NombreComprador solo puede contener letras, espacios, guiones o apostrofes.";

            if (string.IsNullOrWhiteSpace(boleto.CorreoComprador) ||
                !Regex.IsMatch(boleto.CorreoComprador, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "El CorreoComprador es obligatorio y debe tener un formato de correo valido.";

            if (boleto.Cantidad <= 0)
                return "La Cantidad debe ser un entero mayor a 0.";

            return null;
        }
    }
}