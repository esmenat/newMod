using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaTickets.Modelos;


public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<SistemaTickets.Modelos.Admin> Admin { get; set; } = default!;

public DbSet<SistemaTickets.Modelos.Client> Client { get; set; } = default!;

public DbSet<SistemaTickets.Modelos.Reservation> Reservation { get; set; } = default!;



public DbSet<SistemaTickets.Modelos.Ticket> Ticket { get; set; } = default!;

public DbSet<SistemaTickets.Modelos.TravelRout> TravelRout { get; set; } = default!;


    }
