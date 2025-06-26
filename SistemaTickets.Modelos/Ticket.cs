using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTickets.Modelos
{
    public enum TypeSeat
    {
        Normal = 1,
        Premium = 2
    }
    public enum TypeUser
    {
        Niño = 1,
        Adulto = 2,
        TerceraEdad = 3
    }
    public class Ticket
    {
        [Key] public int Codigo { get; set; }
        public string ClientName { get; set; }
        public int SeatNumber { get; set; }
        public TypeUser UserType { get; set; }
        public TypeSeat SeatType { get; set; }
        public int ReservationCodigo { get; set; }
        public Reservation? Reservation { get; set; }

    }
}
