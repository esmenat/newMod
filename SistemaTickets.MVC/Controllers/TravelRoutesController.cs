using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Consumer;
using SistemaTickets.Modelos;

namespace SistemaTickets.MVC.Controllers
{
    public class TravelRoutesController : Controller
    {
        // GET: TravelRoutesController
        public ActionResult Index()
        {
            var data = Crud<TravelRout>.GetAll();
            return View(data);
        }

        // GET: TravelRoutesController/Details/5
        public ActionResult Details(int id)
        {
            var travelRoute = Crud<TravelRout>.GetById(id);
            return View();
        }

        // GET: TravelRoutesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TravelRoutesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Crud<TravelRout>.EndPoint = "https://localhost:7087/api/TravelRoutes";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error creating travel route. Please try again.");
                return View();
            }
        }

        // GET: TravelRoutesController/Edit/5
        public ActionResult Edit(int id)
        {
            var travelRoute = Crud<TravelRout>.GetById(id);
            return View(travelRoute);
        }

        // POST: TravelRoutesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TravelRout travelRouts)
        {
            try
            {
                Crud<TravelRout>.Update(id, travelRouts);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error updating travel route. Please try again.");
                return View();
            }
        }

        // GET: TravelRoutesController/Delete/5
        public ActionResult Delete(int id)
        {
            var travelRoute = Crud<TravelRout>.GetById(id);
            return View();
        }

        // POST: TravelRoutesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Crud<TravelRout>.EndPoint = "https://localhost:7087/api/TravelRoutes";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error deleting travel route. Please try again.");
                return View();
            }
        }
    }
}
