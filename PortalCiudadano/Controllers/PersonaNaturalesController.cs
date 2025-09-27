using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class PersonaNaturalesController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: PersonaNaturales

        [HttpGet]
        public ActionResult CreateModal()
        {
            // Devuelve solo el contenido del formulario
            return PartialView("_CreatePersonaNaturalModal", new PersonaNatural());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateModal(PersonaNatural model)
        {
            if (!ModelState.IsValid)
            {
                // Reenvía el formulario con errores para renderizar dentro del modal
                return PartialView("_CreatePersonaNaturalModal", model);
            }

            db.PersonaNaturales.Add(model);
            await db.SaveChangesAsync();

            // Texto que verás en el combo
            var text = $"{model.PersonaNaturalCedula} - {model.PersonaNaturalApellidos} {model.PersonaNaturalNombres}";
            return Json(new { ok = true, id = model.PersonaNaturalId, text = text });
        }

        public async Task<ActionResult> Index()
        {
            return View(await db.PersonaNaturales.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaNatural personaNatural = await db.PersonaNaturales.FindAsync(id);
            if (personaNatural == null)
            {
                return HttpNotFound();
            }
            return View(personaNatural);
        }

        // GET: PersonaNaturales/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PersonaNatural personaNatural)
        {
            personaNatural.FechaCreacion = DateTime.Now;
            ModelState.Remove("FechaCreacion"); // por si el Required valida contra el binder

            if (ModelState.IsValid)
            {
                db.PersonaNaturales.Add(personaNatural);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(personaNatural);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaNatural personaNatural = await db.PersonaNaturales.FindAsync(id);
            if (personaNatural == null)
            {
                return HttpNotFound();
            }
            return View(personaNatural);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PersonaNatural personaNatural)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personaNatural).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(personaNatural);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaNatural personaNatural = await db.PersonaNaturales.FindAsync(id);
            if (personaNatural == null)
            {
                return HttpNotFound();
            }
            return View(personaNatural);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PersonaNatural personaNatural = await db.PersonaNaturales.FindAsync(id);
            db.PersonaNaturales.Remove(personaNatural);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}