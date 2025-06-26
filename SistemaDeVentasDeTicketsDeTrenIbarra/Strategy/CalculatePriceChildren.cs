using SistemaTickets.Modelos;

namespace SistemaDeVentasDeTicketsDeTrenIbarra.Strategy
{
    public class CalculatePriceChildren : IStrategyPrice
    {
        public double CalculatePrice(Ticket ticket, TravelRout rout)
        {
           var price = rout.Price * 0.5; // 50% discount for children
            if(ticket == null || rout == null)
            {
                throw new ArgumentNullException("Ticket or route cannot be null");
            }
            if (ticket.SeatType == TypeSeat.Premium)
            {
                var premiumPrice = price * 1.2; // 20% premium for premium seats
                return premiumPrice; // Discounted price for children in premium seats
            }
            else
            {
                return price; // Discounted price for normal seats
            }
        }
    }
}
