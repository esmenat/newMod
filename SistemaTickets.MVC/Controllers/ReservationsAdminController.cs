using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Consumer;
using SistemaTickets.Modelos;

namespace SistemaTickets.MVC.Controllers
{
    public class ReservationsAdminController : Controller
    {
        // GET: ReservationsAdminController
        public ActionResult Index()
        {
            var data = Crud <Reservation>.GetAll();
            return View(data);
        }

        // GET: ReservationsAdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReservationsAdminController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: ReservationsAdminController/Create
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

        // GET: ReservationsAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservationsAdminController/Edit/5
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

        // GET: ReservationsAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            var reservation = Crud<Reservation>.GetById(id);
            return View(reservation);
        }

        // POST: ReservationsAdminController/Delete/5
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
                ModelState.AddModelError("", "Error deleting reservation. Please try again.");
                return View();
            }
        }
    }
}
