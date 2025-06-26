using SistemaTickets.Modelos;

namespace SistemaDeVentasDeTicketsDeTrenIbarra.Strategy
{
    public class CalculatePriceSenior : IStrategyPrice
    {
        public double CalculatePrice(Ticket ticket, TravelRout rout)
        {
            if (ticket == null || rout == null)
            {
                throw new ArgumentNullException("Ticket or route cannot be null");
            }
            if (ticket.SeatType == TypeSeat.Premium)
            {
                var premiumPrice = rout.Price * 1.2; // 20% premium for premium seats
                return premiumPrice * 0.8; // 20% discount for seniors on premium seats
            }
            else
            {
                return rout.Price * 0.8; // 20% discount for seniors on normal seats
            }
        }

 

    }
}
