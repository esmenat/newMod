using Microsoft.AspNetCore.Mvc;

namespace SistemaTickets.MVC.Controllers
{
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
