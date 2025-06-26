using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTickets.Modelos
{
    public enum Status
    {
        Pending = 1,
        Confirmed = 2,
        Cancelled = 3
    }
    public class Reservation
    {
        [Key] public int Codigo { get; set; }
        public DateTime BookingDate { get; set; }
        public Status Status { get; set; }
        public DateTime? PurchasDate { get; set; }
        public DateTime AssignedDate { get; set; }
        public double? TotalPrice { get; set; }
        public int RouteCodigo { get; set; }
        public int ClientCodigo { get; set; }
        public TravelRout? Route { get; set; }
        public Client? Client { get; set; }    
        public List<Ticket>? Tickets { get; set; } 


    }
}
