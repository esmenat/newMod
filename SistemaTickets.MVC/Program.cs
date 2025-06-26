using SistemaTickets.Consumer;
using SistemaTickets.Modelos;
using SistemaTickets.MVC.Controllers;

namespace SistemaTickets.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Crud<Reservation>.EndPoint = "https://localhost:7087/api/Reservations";
            Crud<Ticket>.EndPoint = "https://localhost:7087/api/Tickets";
            Crud<Client>.EndPoint = "https://localhost:7087/api/Clients";
            Crud<Admin>.EndPoint = "https://localhost:7087/api/Admins";
            Crud<TravelRout>.EndPoint = "https://localhost:7087/api/TravelRoutes";
            // Registrar HttpClient para la inyección de dependencias
            builder.Services.AddHttpClient();

            // Agregar servicios al contenedor
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient<ReservationController>();

            var app = builder.Build();

            // Configurar el canal de solicitud HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Ruta para ver rutas en ClientsController
            app.MapControllerRoute(
                name: "routes",
                pattern: "Client/{action=ViewRoutes}/{id?}",
                defaults: new { controller = "Clients", action = "ViewRoutes" });

            // Ruta para ver asientos en ClientsController
            app.MapControllerRoute(
                name: "seats",
                pattern: "Client/{action=ViewSeats}/{id?}",
                defaults: new { controller = "Clients", action = "ViewSeats" });


            // Ruta para ReservationsController
            //app.MapControllerRoute(
            //    name: "reservations",
            //    pattern: "Reservations/{action=Create}/{id?}",
            //    defaults: new { controller = "Reservations" });

            app.Run();
        }
    }
}
