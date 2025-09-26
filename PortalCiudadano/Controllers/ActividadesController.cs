using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class ActividadesController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: Actividades
        public async Task<ActionResult> Index()
        {
            return View(await db.Actividades.ToListAsync());
        }

        // GET: Actividades/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ActividadId,NombreActividad")] Actividad actividad)
        {
            if (ModelState.IsValid)
            {
                db.Actividades.Add(actividad);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(actividad);
        }

        // GET: Actividades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividad actividad = await db.Actividades.FindAsync(id);
            if (actividad == null)
            {
                return HttpNotFound();
            }
            return View(actividad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ActividadId,NombreActividad")] Actividad actividad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actividad).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(actividad);
        }

        // GET: Actividades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actividad actividad = await db.Actividades.FindAsync(id);
            if (actividad == null)
            {
                return HttpNotFound();
            }
            return View(actividad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Actividad actividad = await db.Actividades.FindAsync(id);
            db.Actividades.Remove(actividad);
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