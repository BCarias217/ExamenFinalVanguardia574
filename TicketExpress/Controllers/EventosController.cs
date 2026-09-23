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
    public class EventosController : ControllerBase
    {
        private readonly TicketExpressDbContext _context;

        public EventosController(TicketExpressDbContext context)
        {
            _context = context;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            return await _context.Eventos.ToListAsync();
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                return NotFound();

            return evento;
        }

        // POST: api/Eventos
        [HttpPost]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            var error = ValidarFormatoEvento(evento);
            if (error != null)
                return BadRequest(error);

            evento.Nombre = TextNormalizer.Normalizar(evento.Nombre);
            evento.Ciudad = TextNormalizer.Normalizar(evento.Ciudad);

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
        }

        // PUT: api/Eventos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.Id)
                return BadRequest("El Id de la ruta no coincide con el Id del evento.");

            var error = ValidarFormatoEvento(evento);
            if (error != null)
                return BadRequest(error);

            evento.Nombre = TextNormalizer.Normalizar(evento.Nombre);
            evento.Ciudad = TextNormalizer.Normalizar(evento.Ciudad);

            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Eventos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                return NotFound();

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Validaciones de formato de Evento
        private string? ValidarFormatoEvento(Evento evento)
        {
            if (string.IsNullOrWhiteSpace(evento.Nombre) || evento.Nombre.Trim().Length < 3 || evento.Nombre.Trim().Length > 100)
                return "El Nombre es obligatorio y debe tener entre 3 y 100 caracteres.";

            if (string.IsNullOrWhiteSpace(evento.Ciudad) || evento.Ciudad.Trim().Length < 2 || evento.Ciudad.Trim().Length > 60)
                return "La Ciudad es obligatoria y debe tener entre 2 y 60 caracteres.";

            if (!Regex.IsMatch(evento.Ciudad, @"^[a-zA-ZÀ-ÿ\s'-]+$"))
                return "La Ciudad solo puede contener letras, espacios, guiones o apostrofes.";

            if (evento.Fecha.Date < DateTime.Now.Date)
                return "La Fecha del evento no puede ser una fecha ya pasada.";

            if (evento.CapacidadTotal <= 0)
                return "La CapacidadTotal debe ser un entero mayor a 0.";

            if (evento.PrecioBoleto < 0)
                return "El PrecioBoleto no puede ser negativo.";

            return null;
        }
    }
}