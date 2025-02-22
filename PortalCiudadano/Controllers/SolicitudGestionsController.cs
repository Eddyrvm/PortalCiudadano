using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using PortalCiudadano.Clases;
using PortalCiudadano.Helpers;
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
        private ApplicationDbContext identityContext = new ApplicationDbContext();

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
            ViewBag.UsuarioSolicitaId = new SelectList(CombosHelper.GetUsuarioSolicita(), "UsuarioSolicitaId", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SolicitudGestion solicitudGestion)
        {
            if (ModelState.IsValid)
            {
                // Usamos un contexto de identidad (si ya tienes uno, por ejemplo ApplicationDbContext)
                // Usar un contexto de identidad separado para obtener la información del usuario.
                using (var identityContext = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(identityContext));
                    var currentUser = userManager.FindByName(User.Identity.Name);
                    if (currentUser != null)
                    {
                        // Asigna el nombre del usuario a QuienRegistraGestion.
                        solicitudGestion.QuienRegistraGestion = currentUser.UserName; // O usa otra propiedad, ej: currentUser.FullName

                        // Asigna el primer rol obtenido a DireccionFuncionario.
                        var roles = userManager.GetRoles(currentUser.Id);
                        solicitudGestion.DireccionFuncionario = roles.FirstOrDefault();
                    }
                }

                // Primero agregamos el registro sin las imágenes
                db.SolicitudGestions.Add(solicitudGestion);
                db.SaveChanges();

                // Definir la carpeta donde se almacenarán las imágenes (puedes ajustar según tu proyecto)
                var folder = "~/Content/SolicitudGestion";

                // Manejo de Foto1 y su archivo (FotoFile1)
                if (solicitudGestion.FotoFile1 != null)
                {
                    // Usamos el id del registro para generar un nombre único
                    var fileName1 = string.Format("{0}_1.jpg", solicitudGestion.SolicitudId);
                    var response1 = FileHelper.UploadPhoto(solicitudGestion.FotoFile1, folder, fileName1);
                    if (response1)
                    {
                        var pic1 = string.Format("{0}/{1}", folder, fileName1);
                        solicitudGestion.Foto1 = pic1;
                    }
                }

                // Manejo de Foto2 y su archivo (FotoFile12)
                if (solicitudGestion.FotoFile12 != null)
                {
                    var fileName2 = string.Format("{0}_2.jpg", solicitudGestion.SolicitudId);
                    var response2 = FileHelper.UploadPhoto(solicitudGestion.FotoFile12, folder, fileName2);
                    if (response2)
                    {
                        var pic2 = string.Format("{0}/{1}", folder, fileName2);
                        solicitudGestion.Foto2 = pic2;
                    }
                }

                // Actualizamos el registro con las rutas de las imágenes si fueron subidas
                db.Entry(solicitudGestion).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.UsuarioSolicitaId = new SelectList(CombosHelper.GetUsuarioSolicita(), "UsuarioSolicitaId", "FullName", solicitudGestion.UsuarioSolicitaId);
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
            ViewBag.UsuarioSolicitaId = new SelectList(CombosHelper.GetUsuarioSolicita(), "UsuarioSolicitaId", "Cedula", solicitudGestion.UsuarioSolicitaId);
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
            ViewBag.UsuarioSolicitaId = new SelectList(CombosHelper.GetUsuarioSolicita(), "UsuarioSolicitaId", "Cedula", solicitudGestion.UsuarioSolicitaId);
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
            var solicitudGestion = db.SolicitudGestions.Find(id);
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
