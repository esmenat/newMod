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
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClient()
        {
            return await _context.Client.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Client.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Client.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Codigo }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Codigo == id);
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<Client>> AuthenticateClient([FromBody] LoginRequest loginRequest)
        {
            // Buscar el cliente por email y contraseña
            var client = await _context.Client
                .FirstOrDefaultAsync(c => c.Email == loginRequest.Email && c.Password == loginRequest.Password);

            if (client == null)
            {
                return Unauthorized();
            }

            return Ok(client);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Client>> RegisterClient([FromBody] RegisterRequest registerRequest)
        {
            // Verificar si el email ya está registrado
            var existingClient = await _context.Client
                .FirstOrDefaultAsync(c => c.Email == registerRequest.Email);

            if (existingClient != null)
            {
                return Conflict("Email is already registered.");
            }

            var newClient = new Client
            {
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                Password = registerRequest.Password
            };

            // Agregar el nuevo cliente a la base de datos
            _context.Client.Add(newClient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = newClient.Codigo }, newClient);
        }

    }
}
