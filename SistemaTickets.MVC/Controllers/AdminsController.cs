using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaTickets.Modelos;

namespace SistemaTickets.MVC.Controllers
{
    public class AdminsController : Controller
    {
        private readonly HttpClient _httpClient;

        // Inyección de HttpClient
        public AdminsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: AdminsController/Login
        public ActionResult Login()
        {
            return View("LoginAdmin");
        }

        // POST: AdminsController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                // Crear el objeto LoginRequest para enviar las credenciales
                var loginRequest = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7087/api/Admins/authenticate", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // Verificar si la respuesta es un array
                    if (jsonString.StartsWith("[") && jsonString.EndsWith("]"))
                    {
                        var admins = JsonConvert.DeserializeObject<List<Admin>>(jsonString);
                        var admin = admins.FirstOrDefault();

                        if (admin != null)
                        {
                            HttpContext.Session.SetString("AdminEmail", admin.Email);
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        // Si es un solo objeto, deserializar directamente
                        var admin = JsonConvert.DeserializeObject<Admin>(jsonString);
                        if (admin != null)
                        {
                            HttpContext.Session.SetString("AdminEmail", admin.Email);
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View("LoginAdmin");
        }


        // GET: AdminsController/Index
        public ActionResult Index()
        {
            var adminEmail = HttpContext.Session.GetString("AdminEmail");
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction(nameof(Login));  // Solo redirige al login si no hay sesión activa
            }
            ViewData["AdminEmail"] = adminEmail;

            return View();
        }


        // GET: AdminsController/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
