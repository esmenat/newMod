using System;
using System.Collections.Generic;
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
    public class TravelRoutesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TravelRoutesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TravelRoutes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelRout>>> GetTravelRout()
        {
            return await _context.TravelRout
                .Include(t => t.Reservations) // Include related Reservations data
                .ToListAsync();
        }

        // GET: api/TravelRoutes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelRout>> GetTravelRout(int id)
        {
            var travelRout = await _context.TravelRout.FindAsync(id);

            if (travelRout == null)
            {
                return NotFound();
            }

            return travelRout;
        }

        // PUT: api/TravelRoutes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTravelRout(int id, TravelRout travelRout)
        {
            if (id != travelRout.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(travelRout).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelRoutExists(id))
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

        // POST: api/TravelRoutes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TravelRout>> PostTravelRout(TravelRout travelRout)
        {
            _context.TravelRout.Add(travelRout);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTravelRout", new { id = travelRout.Codigo }, travelRout);
        }

        // DELETE: api/TravelRoutes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravelRout(int id)
        {
            var travelRout = await _context.TravelRout.FindAsync(id);
            if (travelRout == null)
            {
                return NotFound();
            }

            _context.TravelRout.Remove(travelRout);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TravelRoutExists(int id)
        {
            return _context.TravelRout.Any(e => e.Codigo == id);
        }
    }
}
