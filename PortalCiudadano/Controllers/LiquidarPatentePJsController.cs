using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;
using System.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PortalCiudadano.ViewModels.Reports;
using System.IO;

namespace PortalCiudadano.Controllers
{
    public class LiquidarPatentePJsController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: LiquidarPatentePJs/GetLiquidacionPJReport/5
        public ActionResult GetLiquidacionPJReport(int? id)
        {
            if (id == null) return HttpNotFound();

            var datos = db.LiquidarPatentePJs
                .Where(p => p.LiquidarPatentePJId == id)
                .Select(p => new LiquidarPatentePJReportDTO
                {
                    // Cabecera
                    LiquidarPatentePJId = p.LiquidarPatentePJId,
                    Contador = p.Contador,
                    TipoSolicitud = p.TipoSolicitud,
                    TipoSolicitudNombre = (p.TipoSolicitud == 1 ? "Primera vez"
                                           : p.TipoSolicitud == 2 ? "Renovación" : "N/D"),
                    NumPatenteAsignada = p.NumPatenteAsignada,

                    // Descomenta si tu entidad PJ tiene esta propiedad:
                    // FechaCreada = p.FechaCreada,

                    // Persona Jurídica (ajusta nombres según tu modelo)
                    PersonaJuridicalId = p.PersonaJuridicalId,
                    PersonaJuridicaRUC = p.PersonaJuridicas.PersonaJuridicaRUC,
                    PersonaJuridicaRazonSocial = p.PersonaJuridicas.PersonaJuridicaRazonSocial,
                    PersonaJuridicaNombres = p.PersonaJuridicas.PersonaJuridicaNombres,
                    PersonaJuridicaApellidos = p.PersonaJuridicas.PersonaJuridicaApellidos,
                    DireccionContribuyente = p.PersonaJuridicas.DireccionContribuyente,
                    TelefonoContribuyente = p.PersonaJuridicas.TelefonoContribuyente,
                    FaxContribuyente = p.PersonaJuridicas.FaxContribuyente,
                    CasillaContribuyente = p.PersonaJuridicas.CasillaContribuyente,

                    // Si ObligadoContabilidad es bool:
                    ObligadoContabilidadTexto = (p.PersonaJuridicas.ObligadoContabilidad ? "Sí" : "No"),
                    // Si fuera string "Si"/"No", usa:
                    // ObligadoContabilidadTexto = (p.PersonaJuridicas.ObligadoContabilidad == "Si" ? "Sí" : "No"),

                    InicioActividad = p.PersonaJuridicas.InicioActividad,
                    CapitalPropio = p.PersonaJuridicas.CapitalPropio,
                    FechaCreacionPersona = p.PersonaJuridicas.FechaCreacion,

                    // Relacionados
                    NumeroEmpleados = p.CantidadEmpleado.NumeroEmpleados,
                    IndoEstadisticaProducName = p.InfoEstadisticaProduc.IndoEstadisticaProducName,
                    NombreClasificacion = p.Clasificacion.NombreClasificacion,
                    NombreActividad = p.Actividad.NombreActividad
                })
                .ToList();

            if (datos.Count == 0) return HttpNotFound("No existe la liquidación solicitada.");

            // Carga del .rpt diseñado contra LiquidarPatentePJReportDTO
            var reportPath = Server.MapPath("~/Reports/LiquidacionPatente/LiquidPatenPersonaJ.rpt"); // ajusta el nombre si usaste otro
            var fileName = $"Patente_PJ_{id}.pdf";
            byte[] bytes;

            var rd = new ReportDocument();
            try
            {
                rd.Load(reportPath);
                rd.SetDataSource(datos);

                // (Opcional) limpia headers/buffer de Crystal
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                using (var cr = rd.ExportToStream(ExportFormatType.PortableDocFormat))
                using (var ms = new MemoryStream())
                {
                    cr.CopyTo(ms);
                    bytes = ms.ToArray();
                }
            }
            finally
            {
                rd.Close();
                rd.Dispose();
            }

            // Mostrar inline (en pestaña/iframe)
            Response.AppendHeader("Content-Disposition", $"inline; filename={fileName}");
            return File(bytes, "application/pdf");
        }

        // GET: LiquidarPatentePJs
        public async Task<ActionResult> Index()
        {
            var liquidarPatentePJs = db.LiquidarPatentePJs.Include(l => l.Actividad).Include(l => l.CantidadEmpleado).Include(l => l.Clasificacion).Include(l => l.InfoEstadisticaProduc).Include(l => l.PersonaJuridicas);
            return View(await liquidarPatentePJs.ToListAsync());
        }

        // GET: LiquidarPatentePJs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiquidarPatentePJ liquidarPatentePJ = await db.LiquidarPatentePJs.FindAsync(id);
            if (liquidarPatentePJ == null)
            {
                return HttpNotFound();
            }
            return View(liquidarPatentePJ);
        }

        // GET: LiquidarPatentePJs/Create
        public ActionResult Create()
        {
            const int inicioContador = 800;

            var maxContador = db.LiquidarPatentePJs
                .Select(x => (int?)x.Contador)
                .Max();

            var previewContador = (maxContador.HasValue ? maxContador.Value + 1 : inicioContador);

            ViewBag.ActividadId = new SelectList(db.Actividades, "ActividadId", "NombreActividad");
            ViewBag.CantidadEmpleadoId = new SelectList(db.CantidadEmpleados, "CantidadEmpleadoId", "NumeroEmpleados");
            ViewBag.ClasificacionId = new SelectList(db.Clasificaciones, "ClasificacionId", "NombreClasificacion");
            ViewBag.InfoEstadisticaProducId = new SelectList(db.InfoEstadisticaProducs, "InfoEstadisticaProducId", "IndoEstadisticaProducName");
            ViewBag.PersonaJuridicalId = new SelectList(db.PersonaJuridicas, "PersonaJuridicalId", "RUCApellidosNombres");

            var model = new LiquidarPatentePJ
            {
                Contador = previewContador
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LiquidarPatentePJ liquidarPatentePJ)
        {
            // Asignados en servidor (no vienen del form)
            liquidarPatentePJ.FechaCreada = DateTime.Now;

            // Evita que el estado previo del binder invalide el Required
            ModelState.Remove("FechaCreada");
            ModelState.Remove("Contador");

            // Valor base si no hay registros (ajústalo a tu necesidad)
            const int inicioContador = 400;

            using (var tx = db.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                // LINQ: obtiene el MAX(Contador) de forma segura dentro de la transacción
                var maxContador = await db.LiquidarPatentePJs
                .Select(x => (int?)x.Contador)
                .MaxAsync();

                liquidarPatentePJ.Contador = (maxContador.HasValue ? maxContador.Value + 1 : inicioContador);

                if (ModelState.IsValid)
                {
                    db.LiquidarPatentePJs.Add(liquidarPatentePJ);
                    await db.SaveChangesAsync();
                    tx.Commit();
                    return RedirectToAction("Index");
                }

                tx.Rollback();
            }

            if (ModelState.IsValid)
            {
                db.LiquidarPatentePJs.Add(liquidarPatentePJ);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ActividadId = new SelectList(db.Actividades, "ActividadId", "NombreActividad", liquidarPatentePJ.ActividadId);
            ViewBag.CantidadEmpleadoId = new SelectList(db.CantidadEmpleados, "CantidadEmpleadoId", "NumeroEmpleados", liquidarPatentePJ.CantidadEmpleadoId);
            ViewBag.ClasificacionId = new SelectList(db.Clasificaciones, "ClasificacionId", "NombreClasificacion", liquidarPatentePJ.ClasificacionId);
            ViewBag.InfoEstadisticaProducId = new SelectList(db.InfoEstadisticaProducs, "InfoEstadisticaProducId", "IndoEstadisticaProducName", liquidarPatentePJ.InfoEstadisticaProducId);
            ViewBag.PersonaJuridicalId = new SelectList(db.PersonaJuridicas, "PersonaJuridicalId", "RUCApellidosNombres", liquidarPatentePJ.PersonaJuridicalId);
            return View(liquidarPatentePJ);
        }

        // GET: LiquidarPatentePJs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiquidarPatentePJ liquidarPatentePJ = await db.LiquidarPatentePJs.FindAsync(id);
            if (liquidarPatentePJ == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActividadId = new SelectList(db.Actividades, "ActividadId", "NombreActividad", liquidarPatentePJ.ActividadId);
            ViewBag.CantidadEmpleadoId = new SelectList(db.CantidadEmpleados, "CantidadEmpleadoId", "NumeroEmpleados", liquidarPatentePJ.CantidadEmpleadoId);
            ViewBag.ClasificacionId = new SelectList(db.Clasificaciones, "ClasificacionId", "NombreClasificacion", liquidarPatentePJ.ClasificacionId);
            ViewBag.InfoEstadisticaProducId = new SelectList(db.InfoEstadisticaProducs, "InfoEstadisticaProducId", "IndoEstadisticaProducName", liquidarPatentePJ.InfoEstadisticaProducId);
            ViewBag.PersonaJuridicalId = new SelectList(db.PersonaJuridicas, "PersonaJuridicalId", "RUCApellidosNombres", liquidarPatentePJ.PersonaJuridicalId);
            return View(liquidarPatentePJ);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LiquidarPatentePJ liquidarPatentePJ)
        {
            liquidarPatentePJ.FechaCreada = DateTime.Now;

            // Evita que el estado previo del binder invalide el Required
            ModelState.Remove("FechaCreada");
            if (ModelState.IsValid)
            {
                db.Entry(liquidarPatentePJ).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ActividadId = new SelectList(db.Actividades, "ActividadId", "NombreActividad", liquidarPatentePJ.ActividadId);
            ViewBag.CantidadEmpleadoId = new SelectList(db.CantidadEmpleados, "CantidadEmpleadoId", "NumeroEmpleados", liquidarPatentePJ.CantidadEmpleadoId);
            ViewBag.ClasificacionId = new SelectList(db.Clasificaciones, "ClasificacionId", "NombreClasificacion", liquidarPatentePJ.ClasificacionId);
            ViewBag.InfoEstadisticaProducId = new SelectList(db.InfoEstadisticaProducs, "InfoEstadisticaProducId", "IndoEstadisticaProducName", liquidarPatentePJ.InfoEstadisticaProducId);
            ViewBag.PersonaJuridicalId = new SelectList(db.PersonaJuridicas, "PersonaJuridicalId", "RUCApellidosNombres", liquidarPatentePJ.PersonaJuridicalId);
            return View(liquidarPatentePJ);
        }

        // GET: LiquidarPatentePJs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiquidarPatentePJ liquidarPatentePJ = await db.LiquidarPatentePJs.FindAsync(id);
            if (liquidarPatentePJ == null)
            {
                return HttpNotFound();
            }
            return View(liquidarPatentePJ);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LiquidarPatentePJ liquidarPatentePJ = await db.LiquidarPatentePJs.FindAsync(id);
            db.LiquidarPatentePJs.Remove(liquidarPatentePJ);
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