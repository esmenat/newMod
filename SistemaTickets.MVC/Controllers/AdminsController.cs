using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaTickets.MVC.Controllers
{
    public class AdminsController : Controller
    {
        // GET: AdminsController
        public ActionResult Index()
        {
            return View();
        }

    }
}
