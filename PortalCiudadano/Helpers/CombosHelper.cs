using PortalCiudadano.Models;
using PortalCiudadano.Models.PortalGestion;
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
        private static PortalGestionDbContext db2 = new PortalGestionDbContext();
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

        public static List<UsuarioSolicita> GetUsuarioSolicita()
        {
            var usuarioSolicita = db2.UsuarioSolicitas.ToList();
            usuarioSolicita.Add(new UsuarioSolicita
            {
                UsuarioSolicitaId = 0,
                Apellidos = "[Seleccione Solicitante...]",
            });
            return usuarioSolicita.OrderBy(u => u.FullName).ToList();
        }
        public void Dispose()
        {
            db.Dispose();
            db2.Dispose();
        }
    }
}