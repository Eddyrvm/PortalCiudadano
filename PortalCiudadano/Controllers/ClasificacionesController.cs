using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class ClasificacionesController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: Clasificaciones
        public async Task<ActionResult> Index()
        {
            return View(await db.Clasificaciones.ToListAsync());
        }

        // GET: Clasificaciones/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Clasificacion clasificacion)
        {
            if (ModelState.IsValid)
            {
                db.Clasificaciones.Add(clasificacion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(clasificacion);
        }

        // GET: Clasificaciones/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clasificacion clasificacion = await db.Clasificaciones.FindAsync(id);
            if (clasificacion == null)
            {
                return HttpNotFound();
            }
            return View(clasificacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Clasificacion clasificacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clasificacion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(clasificacion);
        }

        // GET: Clasificaciones/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clasificacion clasificacion = await db.Clasificaciones.FindAsync(id);
            if (clasificacion == null)
            {
                return HttpNotFound();
            }
            return View(clasificacion);
        }

        // POST: Clasificaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Clasificacion clasificacion = await db.Clasificaciones.FindAsync(id);
            db.Clasificaciones.Remove(clasificacion);
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