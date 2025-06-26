using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeVentasDeTicketsDeTrenIbarra.FactoryMethod;
using SistemaDeVentasDeTicketsDeTrenIbarra.Strategy;
using SistemaTickets.Modelos;

namespace SistemaDeVentasDeTicketsDeTrenIbarra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PricesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<string, IStrategyPrice> _pricingStrategies;

        public PricesController(AppDbContext context)
        {
            _context = context;
            var calculatePriceFactory = new CalculatePriceFactory();
            // Inicializa el diccionario con 3 estrategias de precios
            _pricingStrategies = new Dictionary<string, IStrategyPrice>
            {
                { TypeUser.Adulto.ToString(), calculatePriceFactory.CreateCalculatePrice(TypeUser.Adulto.ToString())},
                { TypeUser.Niño.ToString(),calculatePriceFactory.CreateCalculatePrice(TypeUser.Niño.ToString()) },
                { TypeUser.TerceraEdad.ToString(), calculatePriceFactory.CreateCalculatePrice(TypeUser.TerceraEdad.ToString()) }
            };
        }//Baul Strategy

        // EndPoint para calcular el precio de una reservación según sus tickets
        [HttpGet("calculate-price/{reservationId}")]
        public IActionResult CalculatePrice(int reservationId)
        {
            var reservation = _context.Reservation
                .Include(r => r.Tickets)
                .Include(r => r.Route
                )
                .FirstOrDefault(r => r.Codigo == reservationId);

            if (reservation == null)
            {
                return NotFound("Reservation not found");
            }

            double totalPrice = 0;

            foreach (var ticket in reservation.Tickets)
            {
                string userTypeKey = ticket.UserType.ToString(); // Usa el tipo de usuario (Niño, Adulto, Tercera Edad)

                if (_pricingStrategies.ContainsKey(userTypeKey))
                {
                    var pricingStrategy = _pricingStrategies[userTypeKey];
                    totalPrice += pricingStrategy.CalculatePrice(ticket, reservation.Route);
                }
                else
                {
                    return BadRequest($"No pricing strategy found for {userTypeKey}");
                }
            }

            reservation.TotalPrice = totalPrice;
            _context.SaveChanges(); // Guarda el precio total en la base de datos si es necesario

            return Ok(new { TotalPrice = totalPrice });
        }
    }
}
