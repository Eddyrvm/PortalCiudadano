using PortalCiudadano.Helpers;
using PortalCiudadano.Models;
using PortalCiudadano.Models.ServiciosPublicos;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class ServicioLimpezasController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: ServicioLimpezas
        public ActionResult Index()
        {
            var servicioLimpezas = db.ServicioLimpezas.Include(s => s.TipoServicio);
            return View(servicioLimpezas.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var servicioLimpeza = db.ServicioLimpezas.Find(id);
            if (servicioLimpeza == null)
            {
                return HttpNotFound();
            }
            return View(servicioLimpeza);
        }

        public ActionResult Create()
        {
            ViewBag.TipoServicioId = new SelectList(CombosHelper.GetTipoServicios(), "TipoServicioId", "NombreServicio");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServicioLimpeza servicioLimpeza)
        {
            servicioLimpeza.FechaSolicitud = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.ServicioLimpezas.Add(servicioLimpeza);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoServicioId = new SelectList(CombosHelper.GetTipoServicios(), "TipoServicioId", "NombreServicio");
            return View(servicioLimpeza);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var servicioLimpeza = db.ServicioLimpezas.Find(id);
            if (servicioLimpeza == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoServicioId = new SelectList(CombosHelper.GetTipoServicios(), "TipoServicioId", "NombreServicio");
            return View(servicioLimpeza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServicioLimpeza servicioLimpeza)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicioLimpeza).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoServicioId = new SelectList(CombosHelper.GetTipoServicios(), "TipoServicioId", "NombreServicio");
            return View(servicioLimpeza);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var servicioLimpeza = db.ServicioLimpezas.Find(id);
            if (servicioLimpeza == null)
            {
                return HttpNotFound();
            }
            return View(servicioLimpeza);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var servicioLimpeza = db.ServicioLimpezas.Find(id);
            db.ServicioLimpezas.Remove(servicioLimpeza);
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
