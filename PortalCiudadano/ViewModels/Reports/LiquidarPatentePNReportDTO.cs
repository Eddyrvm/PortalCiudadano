using System;

namespace PortalCiudadano.ViewModels.Reports
{
    public class LiquidarPatentePNReportDto
    {
        public int LiquidarPatentePNId { get; set; }
        public int Contador { get; set; }
        public int TipoSolicitud { get; set; }
        public string TipoSolicitudNombre { get; set; }
        public string NumPatenteAsignada { get; set; }
        public DateTime FechaCreada { get; set; }

        // Persona Natural
        public string PersonaNaturalCedula { get; set; }

        public string PersonaNaturalRUC { get; set; }
        public string PersonaNaturalNombres { get; set; }
        public string PersonaNaturalApellidos { get; set; }
        public string DireccionContribuyente { get; set; }
        public string TelefonoContribuyente { get; set; }
        public string FaxContribuyente { get; set; }
        public string CasillaContribuyente { get; set; }
        public string ObligadoContabilidadTexto { get; set; } // "Sí" / "No"
        public DateTime InicioActividad { get; set; }
        public decimal CapitalPropio { get; set; }
        public DateTime FechaCreacionPersona { get; set; }

        // Relacionados
        public string NumeroEmpleados { get; set; }

        public string IndoEstadisticaProducName { get; set; }
        public string NombreClasificacion { get; set; }
        public string NombreActividad { get; set; }
    }
}