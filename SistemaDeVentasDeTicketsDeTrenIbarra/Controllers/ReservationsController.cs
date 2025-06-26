using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaTickets.Modelos;

namespace SistemaDeVentasDeTicketsDeTrenIbarra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservation()
        {
            return await _context.Reservation
                .Include(r => r.Client) // Include related Client data
                .Include(r => r.Route) // Include related TravelRout data
                .Include(r => r.Tickets) // Include related Ticket data
                .ToListAsync();
        }
        
        [HttpGet("Client/{ClienteCodigo}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationByClient(int ClienteCodigo)
        {
            // Busca las reservas asociadas al ClienteCodigo
            var reservations = await _context.Reservation
                .Include(r => r.Client) 
                .Include(r => r.Route) 
                .Include(r => r.Tickets) 
                .Where(r => r.Client.Codigo == ClienteCodigo) // Filtra por el código del cliente
                .ToListAsync();

            // Verifica si se encontraron reservas
            if (reservations == null || !reservations.Any())
            {
                return NotFound(); // Si no se encuentran, devuelve un 404
            }

            return Ok(reservations); // Devuelve las reservas encontradas
        }


        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservation
                .Include(r => r.Client) // Include related Client data
                .Include(r => r.Route) // Include related TravelRout data
                .Include(r => r.Tickets) // Include related Ticket data
                .Where(r => r.Codigo == id)
                .FirstOrDefaultAsync();

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            _context.Reservation.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Codigo }, reservation);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            var tickets = await _context.Ticket
                .Where(t => t.ReservationCodigo == id)
                .ToListAsync();
            foreach (var ticket in tickets)
            {
                _context.Ticket.Remove(ticket);
                await _context.SaveChangesAsync();

            }

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Confirm/{id}")]
        public async Task<IActionResult> ConfirmReservation(int id)
        {
            var reservation = _context.Reservation.Where(r => r.Codigo == id).FirstOrDefault();

            if(reservation == null)
            {
                return NotFound();
            }

            try
            {
                reservation.Status = Status.Confirmed;
                reservation.PurchasDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        [HttpPost("Cancel/{id}")]
        public async Task<IActionResult> CancelarReservation(int id)
        {
            var reservation = _context.Reservation.Where(r => r.Codigo == id).FirstOrDefault();

            if (reservation == null)
            {
                return NotFound();
            }

            try
            {
                reservation.Status = Status.Cancelled;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
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

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Codigo == id);
        }
    }
}
