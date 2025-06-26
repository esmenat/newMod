using SistemaTickets.Modelos;

namespace SistemaDeVentasDeTicketsDeTrenIbarra.Strategy
{
    public interface IStrategyPrice
    {
        public double CalculatePrice(Ticket ticket,TravelRout rout);
    }
}
