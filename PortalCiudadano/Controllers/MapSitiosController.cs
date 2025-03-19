using PortalCiudadano.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class MapSitiosController : Controller
    {
        // GET: MapSitios
        private readonly PortalCiudadanoContext db = new PortalCiudadanoContext();

        public ActionResult Index()
        {
            var coordinates = db.MapSitios.ToList();
            return View(coordinates);
        }
    }
}