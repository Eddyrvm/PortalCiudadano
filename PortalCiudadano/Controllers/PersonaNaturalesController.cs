using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortalCiudadano.Models;
using PortalCiudadano.Models.LiquidacionPatente;

namespace PortalCiudadano.Controllers
{
    public class PersonaNaturalesController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();

        // GET: PersonaNaturales
        public async Task<ActionResult> Index()
        {
            return View(await db.PersonaNaturales.ToListAsync());
        }

        // GET: PersonaNaturales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaNatural personaNatural = await db.PersonaNaturales.FindAsync(id);
            if (personaNatural == null)
            {
                return HttpNotFound();
            }
            return View(personaNatural);
        }

        // GET: PersonaNaturales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonaNaturales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PersonaNaturalId,PersonaNaturalCedula,PersonaNaturalRUC,PersonaNaturalNombres,PersonaNaturalApellidos,DireccionContribuyente,TelefonoContribuyente,FaxContribuyente,CasillaContribuyente,ObligadoContabilidad,InicioActividad,CapitalPropio,FechaCreacion")] PersonaNatural personaNatural)
        {
            if (ModelState.IsValid)
            {
                db.PersonaNaturales.Add(personaNatural);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(personaNatural);
        }

        // GET: PersonaNaturales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaNatural personaNatural = await db.PersonaNaturales.FindAsync(id);
            if (personaNatural == null)
            {
                return HttpNotFound();
            }
            return View(personaNatural);
        }

        // POST: PersonaNaturales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PersonaNaturalId,PersonaNaturalCedula,PersonaNaturalRUC,PersonaNaturalNombres,PersonaNaturalApellidos,DireccionContribuyente,TelefonoContribuyente,FaxContribuyente,CasillaContribuyente,ObligadoContabilidad,InicioActividad,CapitalPropio,FechaCreacion")] PersonaNatural personaNatural)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personaNatural).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(personaNatural);
        }

        // GET: PersonaNaturales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaNatural personaNatural = await db.PersonaNaturales.FindAsync(id);
            if (personaNatural == null)
            {
                return HttpNotFound();
            }
            return View(personaNatural);
        }

        // POST: PersonaNaturales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PersonaNatural personaNatural = await db.PersonaNaturales.FindAsync(id);
            db.PersonaNaturales.Remove(personaNatural);
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
