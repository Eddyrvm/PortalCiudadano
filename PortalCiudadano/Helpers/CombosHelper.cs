using PortalCiudadano.Models;
using PortalCiudadano.Models.ServiciosPublicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PortalCiudadano.Helpers
{
    public class CombosHelper : IDisposable
    {
        private static PortalCiudadanoContext db = new PortalCiudadanoContext();
        public static List<TipoServicio> GetTipoServicios()
        {
            var tipoServicios = db.TipoServicios.ToList();
            tipoServicios.Add(new TipoServicio
            {
                TipoServicioId = 0,
                NombreServicio = "[Seleccione el tipo de Servicio...]",
            });
            return tipoServicios = tipoServicios.OrderBy(b => b.NombreServicio).ToList();
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}