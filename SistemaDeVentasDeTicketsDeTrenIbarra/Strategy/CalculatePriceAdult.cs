using SistemaTickets.Modelos;

namespace SistemaDeVentasDeTicketsDeTrenIbarra.Strategy
{
    public class CalculatePriceAdult : IStrategyPrice
    {
        public double CalculatePrice(Ticket ticket, TravelRout rout)
        {
            if(ticket == null || rout == null)
            {
                throw new ArgumentNullException("Ticket or route cannot be null");
            }
            if(ticket.SeatType  == TypeSeat.Premium)
            {
                var premiumPrice = rout.Price * 1.2; // 20% premium for premium seats
                return premiumPrice; // Full price for adults
            }
            else
            {
                return  rout.Price; // Full price for normal seats

            }
        }
    }
}
