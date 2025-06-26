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

            // Configuración de los endpoints para los controladores CRUD
            Crud<Reservation>.EndPoint = "https://localhost:7087/api/Reservations";
            Crud<Ticket>.EndPoint = "https://localhost:7087/api/Tickets";
            Crud<Client>.EndPoint = "https://localhost:7087/api/Clients";
            Crud<Admin>.EndPoint = "https://localhost:7087/api/Admins";
            Crud<TravelRout>.EndPoint = "https://localhost:7087/api/TravelRoutes";

            // Registrar HttpClient para la inyección de dependencias
            builder.Services.AddHttpClient();  // Usa esta línea una sola vez

            // Agregar servicios al contenedor
            


            // Configuración de almacenamiento en memoria para las sesiones
            builder.Services.AddDistributedMemoryCache();

            // Configuración de sesión (Tiempo de expiración, 10 minutos)
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);  // Personaliza el tiempo de expiración
            });

            var app = builder.Build();

            // Configurar el canal de solicitud HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

          

            // Rutas para los controladores
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Start}/{action=Index}/{id?}");  // Modificar para que la página predeterminada sea la que desees

            // Rutas específicas para ClientsController
            app.MapControllerRoute(
                name: "routes",
                pattern: "Client/{action=ViewRoutes}/{id?}",
                defaults: new { controller = "Clients", action = "ViewRoutes" });

            app.MapControllerRoute(
                name: "seats",
                pattern: "Client/{action=ViewSeats}/{id?}",
                defaults: new { controller = "Clients", action = "ViewSeats" });
            app.MapControllerRoute(
                name: "login-client",
                pattern: "Client/Login", // URL personalizada
                defaults: new { controller = "Clients", action = "Login" });

            app.MapControllerRoute(
    name: "register-client",
    pattern: "Clients/RegisterClient",  // URL personalizada
    defaults: new { controller = "Clients", action = "RegisterClient" });
            app.Run();
        }
    }
}
