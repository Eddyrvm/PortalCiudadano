using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;
using PortalCiudadano.ViewModels.Reports;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class LiquidarPatentePNsController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        public ActionResult GetLiquidacionPNReport(int? id)
        {
            if (id == null) return HttpNotFound();

            var datos = db.LiquidarPatentePNs
                .Where(p => p.LiquidarPatentePNId == id)
                .Select(p => new LiquidarPatentePNReportDto
                {
                    LiquidarPatentePNId = p.LiquidarPatentePNId,
                    Contador = p.Contador,
                    TipoSolicitud = p.TipoSolicitud,
                    TipoSolicitudNombre = (p.TipoSolicitud == 1 ? "Primera vez"
                                           : p.TipoSolicitud == 2 ? "Renovación" : "N/D"),
                    NumPatenteAsignada = p.NumPatenteAsignada,
                    FechaCreada = p.FechaCreada,

                    PersonaNaturalCedula = p.PersonaNatural.PersonaNaturalCedula,
                    PersonaNaturalRUC = p.PersonaNatural.PersonaNaturalRUC,
                    PersonaNaturalNombres = p.PersonaNatural.PersonaNaturalNombres,
                    PersonaNaturalApellidos = p.PersonaNatural.PersonaNaturalApellidos,
                    DireccionContribuyente = p.PersonaNatural.DireccionContribuyente,
                    TelefonoContribuyente = p.PersonaNatural.TelefonoContribuyente,
                    FaxContribuyente = p.PersonaNatural.FaxContribuyente,
                    CasillaContribuyente = p.PersonaNatural.CasillaContribuyente,
                    ObligadoContabilidadTexto = p.PersonaNatural.ObligadoContabilidad ? "Sí" : "No",
                    InicioActividad = p.PersonaNatural.InicioActividad,
                    CapitalPropio = p.PersonaNatural.CapitalPropio,
                    FechaCreacionPersona = p.PersonaNatural.FechaCreacion,

                    NumeroEmpleados = p.CantidadEmpleado.NumeroEmpleados,
                    IndoEstadisticaProducName = p.InfoEstadisticaProduc.IndoEstadisticaProducName,
                    NombreClasificacion = p.Clasificacion.NombreClasificacion,
                    NombreActividad = p.Actividad.NombreActividad
                })
                .ToList();

            if (datos.Count == 0) return HttpNotFound();

            var rpt = new ReportDocument();
            var path = Server.MapPath("~/Reports/LiquidacionPatente/LiquidPatenPersonN.rpt");
            rpt.Load(path);
            rpt.SetDataSource(datos);

            // Para ver en iframe/nueva pestaña
            Response.AppendHeader("Content-Disposition", $"inline; filename=Patente_PN_{id}.pdf");

            // NO usar using aquí; no cierres el stream antes de devolverlo
            Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf");
        }

        public async Task<ActionResult> Index()
        {
            var liquidarPatentePNs = db.LiquidarPatentePNs.Include(l => l.Actividad).Include(l => l.CantidadEmpleado).Include(l => l.Clasificacion).Include(l => l.InfoEstadisticaProduc).Include(l => l.PersonaNatural);
            return View(await liquidarPatentePNs.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiquidarPatentePN liquidarPatentePN = await db.LiquidarPatentePNs.FindAsync(id);
            if (liquidarPatentePN == null)
            {
                return HttpNotFound();
            }
            return View(liquidarPatentePN);
        }

        public ActionResult Create()
        {
            const int inicioContador = 1000;

            var maxContador = db.LiquidarPatentePNs
                .Select(x => (int?)x.Contador)
                .Max();

            var previewContador = (maxContador.HasValue ? maxContador.Value + 1 : inicioContador);

            ViewBag.ActividadId = new SelectList(db.Actividades, "ActividadId", "NombreActividad");
            ViewBag.CantidadEmpleadoId = new SelectList(db.CantidadEmpleados, "CantidadEmpleadoId", "NumeroEmpleados");
            ViewBag.ClasificacionId = new SelectList(db.Clasificaciones, "ClasificacionId", "NombreClasificacion");
            ViewBag.InfoEstadisticaProducId = new SelectList(db.InfoEstadisticaProducs, "InfoEstadisticaProducId", "IndoEstadisticaProducName");
            ViewBag.PersonaNaturalId = new SelectList(db.PersonaNaturales, "PersonaNaturalId", "CedulaApellidosNombres");

            var model = new LiquidarPatentePN
            {
                Contador = previewContador
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LiquidarPatentePN liquidarPatentePN)
        {
            // Asignados en servidor (no vienen del form)
            liquidarPatentePN.FechaCreada = DateTime.Now;

            // Evita que el estado previo del binder invalide el Required
            ModelState.Remove("FechaCreada");
            ModelState.Remove("Contador");

            // Valor base si no hay registros (ajústalo a tu necesidad)
            const int inicioContador = 400;

            using (var tx = db.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                // LINQ: obtiene el MAX(Contador) de forma segura dentro de la transacción
                var maxContador = await db.LiquidarPatentePNs
                    .Select(x => (int?)x.Contador)
                    .MaxAsync();

                liquidarPatentePN.Contador = (maxContador.HasValue ? maxContador.Value + 1 : inicioContador);

                if (ModelState.IsValid)
                {
                    db.LiquidarPatentePNs.Add(liquidarPatentePN);
                    await db.SaveChangesAsync();
                    tx.Commit();
                    return RedirectToAction("Index");
                }

                tx.Rollback();
            }

            ViewBag.ActividadId = new SelectList(db.Actividades, "ActividadId", "NombreActividad", liquidarPatentePN.ActividadId);
            ViewBag.CantidadEmpleadoId = new SelectList(db.CantidadEmpleados, "CantidadEmpleadoId", "NumeroEmpleados", liquidarPatentePN.CantidadEmpleadoId);
            ViewBag.ClasificacionId = new SelectList(db.Clasificaciones, "ClasificacionId", "NombreClasificacion", liquidarPatentePN.ClasificacionId);
            ViewBag.InfoEstadisticaProducId = new SelectList(db.InfoEstadisticaProducs, "InfoEstadisticaProducId", "IndoEstadisticaProducName", liquidarPatentePN.InfoEstadisticaProducId);
            ViewBag.PersonaNaturalId = new SelectList(db.PersonaNaturales, "PersonaNaturalId", "PersonaNaturalCedula", liquidarPatentePN.PersonaNaturalId);

            return View(liquidarPatentePN);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiquidarPatentePN liquidarPatentePN = await db.LiquidarPatentePNs.FindAsync(id);
            if (liquidarPatentePN == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActividadId = new SelectList(db.Actividades, "ActividadId", "NombreActividad", liquidarPatentePN.ActividadId);
            ViewBag.CantidadEmpleadoId = new SelectList(db.CantidadEmpleados, "CantidadEmpleadoId", "NumeroEmpleados", liquidarPatentePN.CantidadEmpleadoId);
            ViewBag.ClasificacionId = new SelectList(db.Clasificaciones, "ClasificacionId", "NombreClasificacion", liquidarPatentePN.ClasificacionId);
            ViewBag.InfoEstadisticaProducId = new SelectList(db.InfoEstadisticaProducs, "InfoEstadisticaProducId", "IndoEstadisticaProducName", liquidarPatentePN.InfoEstadisticaProducId);
            ViewBag.PersonaNaturalId = new SelectList(db.PersonaNaturales, "PersonaNaturalId", "PersonaNaturalCedula", liquidarPatentePN.PersonaNaturalId);
            return View(liquidarPatentePN);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LiquidarPatentePN liquidarPatentePN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(liquidarPatentePN).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ActividadId = new
                SelectList(db.Actividades, "ActividadId", "NombreActividad", liquidarPatentePN.ActividadId);
            ViewBag.CantidadEmpleadoId = new
                SelectList(db.CantidadEmpleados, "CantidadEmpleadoId", "NumeroEmpleados", liquidarPatentePN.CantidadEmpleadoId);
            ViewBag.ClasificacionId = new
                SelectList(db.Clasificaciones, "ClasificacionId", "NombreClasificacion", liquidarPatentePN.ClasificacionId);
            ViewBag.InfoEstadisticaProducId = new
                SelectList(db.InfoEstadisticaProducs, "InfoEstadisticaProducId", "IndoEstadisticaProducName", liquidarPatentePN.InfoEstadisticaProducId);
            ViewBag.PersonaNaturalId = new
                SelectList(db.PersonaNaturales, "PersonaNaturalId", "PersonaNaturalCedula", liquidarPatentePN.PersonaNaturalId);
            return View(liquidarPatentePN);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiquidarPatentePN liquidarPatentePN = await db.LiquidarPatentePNs.FindAsync(id);
            if (liquidarPatentePN == null)
            {
                return HttpNotFound();
            }
            return View(liquidarPatentePN);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LiquidarPatentePN liquidarPatentePN = await db.LiquidarPatentePNs.FindAsync(id);
            db.LiquidarPatentePNs.Remove(liquidarPatentePN);
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