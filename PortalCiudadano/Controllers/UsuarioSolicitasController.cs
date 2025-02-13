using PortalCiudadano.Helpers;
using PortalCiudadano.Models;
using PortalCiudadano.Models.PortalGestion;
using PortalCiudadano.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class UsuarioSolicitasController : Controller
    {
        private PortalGestionDbContext db = new PortalGestionDbContext();
        private ServicioConsultaPersonas _servicioConsultaPersonas = new ServicioConsultaPersonas();

        // GET: UsuarioSolicitas
        public ActionResult Index()
        {
            return View(db.UsuarioSolicitas.ToList());
        }

        public JsonResult ObtenerPersonasFiltro(string search)
        {
            if (string.IsNullOrEmpty(search) || search.Length < 3)
            {
                return Json(new { error = "Ingrese al menos 3 caracteres." }, JsonRequestBehavior.AllowGet);
            }

            var personas = _servicioConsultaPersonas.ObtenerPersonasPorFiltro(search);

            var lista = personas.Select(p => new
            {
                id = p.identificador, // Se usará en Select2
                text =p.identificador +" - "+ p.natural_apellidos + " " + p.natural_nombres // Nombre en Select2
            }).ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersona(string identificador)
        {
            if (string.IsNullOrEmpty(identificador))
            {
                return Json(new { error = "Debe seleccionar una persona." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var persona = _servicioConsultaPersonas.ObtenerPersonaPorIdentificador(identificador);

                if (persona == null)
                {
                    return Json(new { error = "Persona no encontrada." }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    persona.identificador,
                    persona.natural_nombres,
                    persona.natural_apellidos
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Ocurrió un error inesperado: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
