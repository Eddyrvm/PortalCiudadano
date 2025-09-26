using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class InfoEstadisticaProducsController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: InfoEstadisticaProducs
        public async Task<ActionResult> Index()
        {
            return View(await db.InfoEstadisticaProducs.ToListAsync());
        }

        // GET: InfoEstadisticaProducs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoEstadisticaProduc infoEstadisticaProduc = await db.InfoEstadisticaProducs.FindAsync(id);
            if (infoEstadisticaProduc == null)
            {
                return HttpNotFound();
            }
            return View(infoEstadisticaProduc);
        }

        // GET: InfoEstadisticaProducs/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InfoEstadisticaProducId,IndoEstadisticaProducName")] InfoEstadisticaProduc infoEstadisticaProduc)
        {
            if (ModelState.IsValid)
            {
                db.InfoEstadisticaProducs.Add(infoEstadisticaProduc);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(infoEstadisticaProduc);
        }

        // GET: InfoEstadisticaProducs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoEstadisticaProduc infoEstadisticaProduc = await db.InfoEstadisticaProducs.FindAsync(id);
            if (infoEstadisticaProduc == null)
            {
                return HttpNotFound();
            }
            return View(infoEstadisticaProduc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InfoEstadisticaProducId,IndoEstadisticaProducName")] InfoEstadisticaProduc infoEstadisticaProduc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(infoEstadisticaProduc).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(infoEstadisticaProduc);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoEstadisticaProduc infoEstadisticaProduc = await db.InfoEstadisticaProducs.FindAsync(id);
            if (infoEstadisticaProduc == null)
            {
                return HttpNotFound();
            }
            return View(infoEstadisticaProduc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InfoEstadisticaProduc infoEstadisticaProduc = await db.InfoEstadisticaProducs.FindAsync(id);
            db.InfoEstadisticaProducs.Remove(infoEstadisticaProduc);
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