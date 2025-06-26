using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTickets.Modelos
{
    public class TravelRout
    {

        [Key] public int Codigo { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string TravelTime { get; set; }
        public double Price { get; set; }
        public List<Reservation>? Reservations { get; set; }

    }
}
