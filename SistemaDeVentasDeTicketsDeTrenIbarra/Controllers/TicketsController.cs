using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using SistemaTickets.Modelos;

namespace SistemaDeVentasDeTicketsDeTrenIbarra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicket()
        {
            return await _context.Ticket
                .Include(t => t.Reservation) // Include related Reservation dat
                .ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Ticket
                .Include(t => t.Reservation) // Include related Reservation data
                .Where(t => t.Codigo == id)
                .FirstAsync();

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            var reservation = await _context.Reservation.FindAsync(ticket.ReservationCodigo);
            var rouT = await _context.TravelRout
                .Where(r => r.Codigo == reservation.RouteCodigo).FirstOrDefaultAsync();

            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.Codigo }, ticket);
        }

        [HttpGet("confirmed-tickets/{routeCodigo}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetConfirmedTicketsByRoute(int routeCodigo)
        {
            var confirmedTickets = await _context.Reservation
                .Where(r => r.RouteCodigo == routeCodigo )  // Filtramos por ruta y estado confirmado
                .SelectMany(r => r.Tickets)  // Seleccionamos todos los tickets asociados a las reservas confirmadas
                .ToListAsync();

            if (confirmedTickets == null || !confirmedTickets.Any())
            {
                return NotFound("No confirmed tickets found for this route.");
            }

            return Ok(confirmedTickets);
        }


        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Codigo == id);
        }
    }
}
