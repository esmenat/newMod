using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaTickets.Modelos;

namespace SistemaDeVentasDeTicketsDeTrenIbarra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

            builder.Services
                .AddControllers()
                .AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            CreateDefaultTravelRoutes(app);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        private static void CreateDefaultTravelRoutes(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Verificar si ya existen las rutas predeterminadas
                var existingRoutes = dbContext.TravelRout
                    .FirstOrDefault(r => r.Origin == "Ibarra" && r.Destination == "Salinas (Esmeraldas)");

                var existingRoute2 = dbContext.TravelRout
                    .FirstOrDefault(r => r.Origin == "Ibarra" && r.Destination == "Antonio Ante");

                if (existingRoutes == null)
                {
                    // Crear la ruta Ibarra - Salinas (Esmeraldas)
                    var newRoute1 = new TravelRout
                    {
                        Origin = "Ibarra",
                        Destination = "Salinas",
                        TravelTime = "2 horas", // Ajusta según lo que necesites
                        Price = 10.0 // Ajusta el precio según corresponda
                    };

                    dbContext.TravelRout.Add(newRoute1);
                }

                if (existingRoute2 == null)
                {
                    // Crear la ruta Ibarra - Antonio Ante
                    var newRoute2 = new TravelRout
                    {
                        Origin = "Ibarra",
                        Destination = "Antonio Ante",
                        TravelTime = "40 minutos", // Ajusta según lo que necesites
                        Price = 8.0 // Ajusta el precio según corresponda
                    };

                    dbContext.TravelRout.Add(newRoute2);
                }

                // Guardar los cambios en la base de datos
                dbContext.SaveChanges();
            }
        }

    }
}
