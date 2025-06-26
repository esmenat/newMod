using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaTickets.Modelos;

namespace SistemaTickets.MVC.Controllers
{
    public class ClientsController : Controller
    {
       
        private readonly HttpClient _httpClient;

        // Inyección de HttpClient
        public ClientsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult LoginClient()
        {
            return View();
        }
        public IActionResult RegisterClient()
        {
            return View();
        }
        // GET: ClientsController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult ViewSeats()
        {
            return View();
        }
        public IActionResult ViewRoutes()
        {
            return View();
        }
        // GET: ClientsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClientsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: ClientsController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var loginRequest = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7087/api/Clients/authenticate", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var client = JsonConvert.DeserializeObject<Client>(jsonString);
                   
                    HttpContext.Session.SetString("ClientEmail", client.Email); 

                    // Redirige al dashboard o la página principal del cliente
                    return RedirectToAction("Home", "Index");  // Asegúrate de tener la acción 'Dashboard' en tu controlador
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View("LoginClient");
        }

        // GET: ClientsController/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpia la sesión del cliente
            return RedirectToAction(nameof(Login)); // Redirige al login
        }


        // POST: ClientsController/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(string name, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View("RegisterClient");
            }

            var registerRequest = new RegisterRequest
            {
                Name = name,
                Email = email,
                Password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7087/api/Clients/register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(LoginClient), "Clients"); // Redirige a la página de login
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorContent);
            return View("RegisterClient");
        }

  
    }
}
