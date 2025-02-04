using System;

namespace PortalCiudadano.ViewModels.Reports
{
    public class ServicioLimpiezaReportDTO
    {
        public int ServicioLimpiezaId { get; set; }

        // En lugar de la clave foránea, incluimos directamente el nombre
        public string NombreTipoServicio { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public string Institucion { get; set; }

        public string Calle { get; set; }

        public string Referencia { get; set; }

        public string DetalleActividad { get; set; }

        public string Telefono { get; set; }

        public string ActividadRealizada { get; set; }

        public string EstadoSolicitud { get; set; }

        public int UserId { get; set; }
        public string FullName { get; set; }

        public string correo { get; set; }

        public int TipoServicioId { get; set; }

        public byte[] Foto { get; set; }
    }

}