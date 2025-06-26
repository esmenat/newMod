using System.ComponentModel.DataAnnotations;

namespace SistemaTickets.Modelos
{
    public class Client
    {
        [Key] public int Codigo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Reservation>? Reservations { get; set; }

    }
}
