using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class LiquidarPatentePNsController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

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