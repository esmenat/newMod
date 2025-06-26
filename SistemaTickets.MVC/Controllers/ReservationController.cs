using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SistemaTickets.Consumer;
using SistemaTickets.Modelos;
using static System.Net.WebRequestMethods;

namespace SistemaTickets.MVC.Controllers
{
    public class ReservationController : Controller
    {
        // GET:
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7087/api/prices/calculate-price"; // URL de la API para calcular el precio.

        // Inyección de HttpClient
        public ReservationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       public async Task<ActionResult> Index()
{
            var clientCodigo = 1; // Aquí deberías obtener el código del cliente logueado, por ejemplo, desde la sesión o el token de autenticación.
                                  // Construye la URL con el clienteCodigo al final
            var url = $"https://localhost:7087/api/Reservations/Client/{clientCodigo}";

    // Realiza la solicitud HTTP GET
    var response = await _httpClient.GetAsync(url);

            // Verifica si la respuesta fue exitosa
            if (response.IsSuccessStatusCode)
            {
                // Mapea la respuesta a una lista de reservaciones
                var data = JsonConvert.DeserializeObject<List<Reservation>>(await response.Content.ReadAsStringAsync());


                // Devuelve la vista con los datos
                return View(data);
    }
    else
    {
        // Si no se obtuvo una respuesta exitosa, maneja el error (por ejemplo, 404 o 500)
        // Puedes redirigir a una página de error o devolver un mensaje adecuado
        return View("Error");
    }
}


        // GET: ReservationController/Details/5
        public ActionResult Details(int id)
        {
            var taller = Crud<Reservation>.GetById(id);
            return View(taller);
        }

        // GET: ReservationController/Create
        public ActionResult Create()
        {
            ViewBag.Routes = GetRoutes();
            return View();
        }
        private List<SelectListItem> GetRoutes()
        {
            var paises = Crud<TravelRout>.GetAll();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Codigo.ToString(),
                Text = p.Origin + " - " + p.Destination
            }).ToList();

        }


[HttpPost]
    [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Reservation reservation, string TicketsJson)
        {
            try
            {
                // Deserializar los tickets desde el JSON usando Newtonsoft.Json
                List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(TicketsJson);

                // Llamar a la API para obtener los tickets confirmados (ruta estática)
                var response = await _httpClient.GetAsync("https://localhost:7087/api/Tickets/confirmed-tickets/" + reservation.RouteCodigo);

                if (!response.IsSuccessStatusCode)
                {
                    // Si la respuesta no es exitosa, mostramos un error
                    TempData["ErrorMessage"] = "Error fetching confirmed tickets. Please try again.";
                    return RedirectToAction("Index", "Home");
                }

                // Deserializar la respuesta de la API usando Newtonsoft.Json
                var confirmedTicketsJson = await response.Content.ReadAsStringAsync();
                var confirmedTickets = JsonConvert.DeserializeObject<List<Ticket>>(confirmedTicketsJson);

                // Verificar si algún asiento ya está confirmado
                foreach (var ticket in tickets)
                {
                    var existingTicket = confirmedTickets.FirstOrDefault(t => t.SeatNumber == ticket.SeatNumber);

                    if (existingTicket != null)
                    {
                        // Si el asiento ya está confirmado, retornamos un error
                        TempData["ErrorMessage"] = $"Seat number {ticket.SeatNumber} is already confirmed. Please choose another seat.";
                        ViewBag.Routes = GetRoutes();
                        return View();
                    }
                }

                // Establecer fecha y estado de la reserva
                reservation.BookingDate = DateTime.Now;
                reservation.Status = Status.Pending;
                reservation.ClientCodigo = 1; // Aquí deberías poner el ID del usuario logueado.

                // Crear la reserva
                var createReservation = Crud<Reservation>.Create(reservation);

                // Asignar los tickets a la reserva
                foreach (var ticket in tickets)
                {
                    ticket.ReservationCodigo = createReservation.Codigo; // Asociamos el ticket a la reserva
                    Crud<Ticket>.Create(ticket); // Guardamos cada ticket en la base de datos
                }

                // Llamar al endpoint para calcular el precio de los tickets (ruta estática)
                var priceResponse = await _httpClient.GetAsync("https://localhost:7087/api/Prices/calculate-price/" + createReservation.Codigo);

                if (!priceResponse.IsSuccessStatusCode)
                {
                    // Si la respuesta no es exitosa, mostramos un mensaje de error
                    TempData["ErrorMessage"] = "Error calculating price. Please try again.";
                    return RedirectToAction("Index", "Home");
                }

                // Usar TempData para mostrar un mensaje de éxito
                TempData["SuccessMessage"] = "Reservation and tickets created successfully";

                // Redirigir al Index (o página principal)
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                // Si hay un error, se reestablecen las rutas y se muestra el error
                ViewBag.Routes = GetRoutes();
                ModelState.AddModelError("", "Error creating reservation or tickets. Please try again.");
                return View();
            }
        }


        // GET: ReservationController/Edit/5
        public ActionResult Edit(int id)
        {
            var reservation = Crud<Reservation>.GetById(id);
            ViewBag.Routes = GetRoutes();
            return View(reservation);
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Reservation reservation)
        {
            try
            {
                Crud<Reservation>.Update(id, reservation);
                return RedirectToAction(nameof(Index));
            }
            catch
            {

                ViewBag.Routes = GetRoutes();
                ModelState.AddModelError("", "Error updating reservation. Please try again.");
                return View();
            }
        }

        // GET: ReservationController/Delete/5
        public ActionResult Delete(int id)
        {
            var reservation = Crud<Reservation>.GetById(id);
            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                // Llamada a la API para confirmar la reserva
                var url = $"https://localhost:7087/api/Reservations/Confirm/{id}"; // URL completa para confirmar
                var response = await _httpClient.PostAsync(url, null); // Método PUT
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Reservation confirmed successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error confirming the reservation.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // Acción para cancelar una reserva
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                // Llamada a la API para cancelar la reserva
                var url = $"https://localhost:7087/api/Reservations/Cancel/{id}"; // URL completa para cancelar
                var response = await _httpClient.PostAsync(url, null); // Método PUT
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Reservation canceled successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error canceling the reservation.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Crud<Reservation>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {

                ViewBag.Routes = GetRoutes();
                ModelState.AddModelError("", "Error deleting reservation. Please try again.");
                return View();
            }
        }
    }
}
