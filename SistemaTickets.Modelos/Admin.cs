using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTickets.Modelos
{
    public class Admin
    {
        [Key] public int Codigo { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}
