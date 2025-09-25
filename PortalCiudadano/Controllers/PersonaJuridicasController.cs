using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class PersonaJuridicasController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: PersonaJuridicas
        public async Task<ActionResult> Index()
        {
            return View(await db.PersonaJuridicas.ToListAsync());
        }

        // GET: PersonaJuridicas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaJuridica personaJuridica = await db.PersonaJuridicas.FindAsync(id);
            if (personaJuridica == null)
            {
                return HttpNotFound();
            }
            return View(personaJuridica);
        }

        // GET: PersonaJuridicas/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PersonaJuridica personaJuridica)
        {
            personaJuridica.FechaCreacion = DateTime.Now;
            ModelState.Remove("FechaCreacion"); // evita fallo de [Required] por el binder

            if (ModelState.IsValid)
            {
                db.PersonaJuridicas.Add(personaJuridica);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(personaJuridica);
        }

        // GET: PersonaJuridicas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaJuridica personaJuridica = await db.PersonaJuridicas.FindAsync(id);
            if (personaJuridica == null)
            {
                return HttpNotFound();
            }
            return View(personaJuridica);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PersonaJuridica personaJuridica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personaJuridica).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(personaJuridica);
        }

        // GET: PersonaJuridicas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaJuridica personaJuridica = await db.PersonaJuridicas.FindAsync(id);
            if (personaJuridica == null)
            {
                return HttpNotFound();
            }
            return View(personaJuridica);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PersonaJuridica personaJuridica = await db.PersonaJuridicas.FindAsync(id);
            db.PersonaJuridicas.Remove(personaJuridica);
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