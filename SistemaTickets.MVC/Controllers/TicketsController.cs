using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Consumer;
using SistemaTickets.Modelos;

namespace SistemaTickets.MVC.Controllers
{
    public class TicketsController : Controller
    {
        // GET: TicketsController
        public ActionResult Index()
        {
            var data = Crud<Ticket>.GetAll();
            return View(data);
        }

        // GET: TicketsController/Details/5
        public ActionResult Details(int id)
        {
            var ticket = Crud<Ticket>.GetById(id);
            return View(ticket);
        }

        // GET: TicketsController/Create
        public ActionResult Create()
        {
            Crud<Ticket>.EndPoint = "https://localhost:7087/api/Tickets";
            return View();
        }

        // POST: TicketsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Crud<Ticket>.EndPoint = "https://localhost:7087/api/Tickets";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error creating ticket. Please try again.");
                return View();
            }
        }

        // GET: TicketsController/Edit/5
        public ActionResult Edit(int id)
        {
            var ticket = Crud<Ticket>.GetById(id);
            return View(ticket);
        }

        // POST: TicketsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Crud<Ticket>.EndPoint = "https://localhost:7087/api/Tickets";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error updating ticket. Please try again.");
                return View();
            }
        }

        // GET: TicketsController/Delete/5
        public ActionResult Delete(int id)
        {
            var ticket = Crud<Ticket>.GetById(id);
            return View(ticket);
        }

        // POST: TicketsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Crud<Ticket>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error deleting ticket. Please try again.");
                return View();
            }
        }
    }
}
