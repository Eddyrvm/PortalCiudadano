using PortalCiudadano.Models;
using PortalCiudadano.Models.PortalGestion;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class UsuarioSolicitasController : Controller
    {
        private PortalGestionDbContext db = new PortalGestionDbContext();

        // GET: UsuarioSolicitas
        public ActionResult Index()
        {
            return View(db.UsuarioSolicitas.ToList());
        }

        // GET: UsuarioSolicitas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usuarioSolicita = db.UsuarioSolicitas.Find(id);
            if (usuarioSolicita == null)
            {
                return HttpNotFound();
            }
            return View(usuarioSolicita);
        }

        // GET: UsuarioSolicitas/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioSolicita usuarioSolicita)
        {
            if (ModelState.IsValid)
            {
                db.UsuarioSolicitas.Add(usuarioSolicita);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuarioSolicita);
        }

        // GET: UsuarioSolicitas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usuarioSolicita = db.UsuarioSolicitas.Find(id);
            if (usuarioSolicita == null)
            {
                return HttpNotFound();
            }
            return View(usuarioSolicita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioSolicita usuarioSolicita)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarioSolicita).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuarioSolicita);
        }

        // GET: UsuarioSolicitas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usuarioSolicita = db.UsuarioSolicitas.Find(id);
            if (usuarioSolicita == null)
            {
                return HttpNotFound();
            }
            return View(usuarioSolicita);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var usuarioSolicita = db.UsuarioSolicitas.Find(id);
            db.UsuarioSolicitas.Remove(usuarioSolicita);
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
