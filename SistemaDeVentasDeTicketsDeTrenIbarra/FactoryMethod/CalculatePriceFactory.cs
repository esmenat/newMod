using System.Runtime.InteropServices;
using SistemaDeVentasDeTicketsDeTrenIbarra.Strategy;
using SistemaTickets.Modelos;

namespace SistemaDeVentasDeTicketsDeTrenIbarra.FactoryMethod
{
    public class CalculatePriceFactory
    {
        //El Factory Se encarga de crear las estrategias que calculan los precios , y el Strategy son las diferentes formas de calcular los precios
        public IStrategyPrice CreateCalculatePrice(string type)
        {
            return type switch
            {
                "Adulto"=> new CalculatePriceAdult(),
                 "Niño" => new CalculatePriceChildren(),
                "TerceraEdad" => new CalculatePriceSenior(),
                _ => throw new ArgumentException("Invalid price calculation type")
            };
        }
    }
}
