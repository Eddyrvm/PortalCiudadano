using PortalCiudadano.Models;
using PortalCiudadano.Models.PortalGestion;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class SolicitudGestionsController : Controller
    {
        private PortalGestionDbContext db = new PortalGestionDbContext();

        // GET: SolicitudGestions
        public ActionResult Index()
        {
            var solicitudGestions = db.SolicitudGestions.Include(s => s.UsuarioSolicita);
            return View(solicitudGestions.ToList());
        }

        // GET: SolicitudGestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var solicitudGestion = db.SolicitudGestions.Find(id);
            if (solicitudGestion == null)
            {
                return HttpNotFound();
            }
            return View(solicitudGestion);
        }

        // GET: SolicitudGestions/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioSolicitaId = new SelectList(db.UsuarioSolicitas, "UsuarioSolicitaId", "Cedula");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SolicitudGestion solicitudGestion)
        {
            if (ModelState.IsValid)
            {
                db.SolicitudGestions.Add(solicitudGestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioSolicitaId = new SelectList(db.UsuarioSolicitas, "UsuarioSolicitaId", "Cedula", solicitudGestion.UsuarioSolicitaId);
            return View(solicitudGestion);
        }

        // GET: SolicitudGestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var solicitudGestion = db.SolicitudGestions.Find(id);
            if (solicitudGestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioSolicitaId = new SelectList(db.UsuarioSolicitas, "UsuarioSolicitaId", "Cedula", solicitudGestion.UsuarioSolicitaId);
            return View(solicitudGestion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SolicitudGestion solicitudGestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitudGestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioSolicitaId = new SelectList(db.UsuarioSolicitas, "UsuarioSolicitaId", "Cedula", solicitudGestion.UsuarioSolicitaId);
            return View(solicitudGestion);
        }

        // GET: SolicitudGestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var solicitudGestion = db.SolicitudGestions.Find(id);
            if (solicitudGestion == null)
            {
                return HttpNotFound();
            }
            return View(solicitudGestion);
        }

        // POST: SolicitudGestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SolicitudGestion solicitudGestion = db.SolicitudGestions.Find(id);
            db.SolicitudGestions.Remove(solicitudGestion);
            db.SaveChanges();
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
