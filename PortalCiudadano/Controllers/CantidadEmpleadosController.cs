using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class CantidadEmpleadosController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: CantidadEmpleados
        public async Task<ActionResult> Index()
        {
            return View(await db.CantidadEmpleados.ToListAsync());
        }

        // GET: CantidadEmpleados/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CantidadEmpleadoId,NumeroEmpleados")] CantidadEmpleado cantidadEmpleado)
        {
            if (ModelState.IsValid)
            {
                db.CantidadEmpleados.Add(cantidadEmpleado);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cantidadEmpleado);
        }

        // GET: CantidadEmpleados/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CantidadEmpleado cantidadEmpleado = await db.CantidadEmpleados.FindAsync(id);
            if (cantidadEmpleado == null)
            {
                return HttpNotFound();
            }
            return View(cantidadEmpleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CantidadEmpleadoId,NumeroEmpleados")] CantidadEmpleado cantidadEmpleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cantidadEmpleado).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cantidadEmpleado);
        }

        // GET: CantidadEmpleados/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CantidadEmpleado cantidadEmpleado = await db.CantidadEmpleados.FindAsync(id);
            if (cantidadEmpleado == null)
            {
                return HttpNotFound();
            }
            return View(cantidadEmpleado);
        }

        // POST: CantidadEmpleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CantidadEmpleado cantidadEmpleado = await db.CantidadEmpleados.FindAsync(id);
            db.CantidadEmpleados.Remove(cantidadEmpleado);
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