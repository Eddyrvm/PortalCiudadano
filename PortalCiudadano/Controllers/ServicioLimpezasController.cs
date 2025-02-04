using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PortalCiudadano.Clases;
using PortalCiudadano.Helpers;
using PortalCiudadano.Models;
using PortalCiudadano.Models.ServiciosPublicos;
using PortalCiudadano.ViewModels.Reports;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class ServicioLimpezasController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: ServicioLimpezas
        public ActionResult Index()
        {
            var servicioLimpezas = db.ServicioLimpezas
                .Include(s => s.User)
                .Include(s => s.TipoServicio);
            return View(servicioLimpezas.ToList());

        }

        public ActionResult PrintServicioLimpiezas(int? Id)
        {
            var datosFiltrados = db.ServicioLimpezas
                                   .Where(t => t.ServicioLimpezaId == Id)
                                   .Include(t => t.TipoServicio)
                                   .Include(t => t.User)
                                   .AsEnumerable()  // Se ejecuta en memoria después de este punto
                                   .Select(t => new ServicioLimpiezaReportDTO
                                   {
                                       ServicioLimpiezaId = t.ServicioLimpezaId,
                                       FechaSolicitud = t.FechaSolicitud,
                                       Institucion = t.Institucion,
                                       Calle = t.Calle,
                                       Referencia = t.Referencia,
                                       DetalleActividad = t.DetalleActividad,
                                       Telefono = t.Telefono,
                                       ActividadRealizada = t.ActividadRealizada,
                                       EstadoSolicitud = t.EstadoSolicitud,
                                       TipoServicioId = t.TipoServicioId,

                                       NombreTipoServicio = t.TipoServicio.NombreServicio,
                                       UserId = t.UserId,

                                       // Se calcula FullName en memoria
                                       FullName = $"{t.User.Apellidos} {t.User.Nombres}",
                                       correo = t.User.UserName
                                   })
                                   .ToList();  // Se ejecuta la consulta aquí

            ReportDocument rpt = new ReportDocument();
            string path = Server.MapPath("~/Reports/ServiciosPublicos/ServicioLimpiezas.rpt");
            rpt.Load(path);

            rpt.SetDataSource(datosFiltrados);

            Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf", "Reporte.pdf");
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
            // Obtener el usuario autenticado
            string loggedUserName = User.Identity.Name;

            // Buscar al usuario en la tabla User por UserName
            var usuario = db.Users.FirstOrDefault(u => u.UserName == loggedUserName);

            if (usuario != null)
            {
                // Asignar el Id del usuario al modelo
                servicioLimpeza.UserId = usuario.UserId;
            }
            else
            {
                // Manejar el caso donde no se encuentra el usuario
                ModelState.AddModelError("", "No se pudo encontrar el usuario autenticado.");
            }

            // Asignar el estado inicial de la solicitud
            servicioLimpeza.EstadoSolicitud = "Pendiente";

            // Asignar la fecha actual
            servicioLimpeza.FechaSolicitud = DateTime.Now;
            ModelState.Remove("ActividadRealizada");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                db.ServicioLimpezas.Add(servicioLimpeza);
                db.SaveChanges();

                if (servicioLimpeza.FotoFile != null)
                {
                    var folder = "~/Content/ServicioLimpieza";
                    var file = string.Format("{0}.jpg", servicioLimpeza.ServicioLimpezaId);
                    var response = FileHelper.UploadPhoto(servicioLimpeza.FotoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        servicioLimpeza.Foto = pic;
                        db.Entry(servicioLimpeza).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
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

            ViewBag.TipoServicioId = new SelectList(db.TipoServicios, "TipoServicioId", "NombreServicio", servicioLimpeza.TipoServicioId);

            var user = db.Users.Find(servicioLimpeza.UserId);
            if (user != null)
            {
                ViewBag.FullName = user.FullName;
            }

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
