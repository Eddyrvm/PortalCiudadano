using System;

namespace PortalCiudadano.ViewModels.Reports
{
    public class LiquidarPatentePJReportDTO
    {
        // Cabecera / Liquidación
        public int LiquidarPatentePJId { get; set; }

        public int Contador { get; set; }
        public int TipoSolicitud { get; set; }            // 1=Primera vez, 2=Renovación
        public string TipoSolicitudNombre { get; set; }   // "Primera vez" / "Renovación" / "N/D"
        public string NumPatenteAsignada { get; set; }
        public DateTime FechaCreada { get; set; }         // Si tu entidad la maneja

        // Persona Jurídica
        public int PersonaJuridicalId { get; set; }

        public string PersonaJuridicaRUC { get; set; }
        public string PersonaJuridicaRazonSocial { get; set; }
        public string PersonaJuridicaNombres { get; set; }    // si aplican en tu modelo
        public string PersonaJuridicaApellidos { get; set; }  // si aplican en tu modelo
        public string DireccionContribuyente { get; set; }
        public string TelefonoContribuyente { get; set; }
        public string FaxContribuyente { get; set; }
        public string CasillaContribuyente { get; set; }
        public string ObligadoContabilidadTexto { get; set; } // "Sí" / "No"
        public DateTime InicioActividad { get; set; }
        public decimal CapitalPropio { get; set; }
        public DateTime FechaCreacionPersona { get; set; }

        // Relacionados
        public String NumeroEmpleados { get; set; }

        public string IndoEstadisticaProducName { get; set; }
        public string NombreClasificacion { get; set; }
        public string NombreActividad { get; set; }

        // Derivados útiles en el .rpt (opcionales)
        public string RUCRazonSocial =>
            string.IsNullOrWhiteSpace(PersonaJuridicaRazonSocial)
                ? PersonaJuridicaRUC
                : $"{PersonaJuridicaRUC} - {PersonaJuridicaRazonSocial}";

        public string NombreContacto =>
            $"{PersonaJuridicaApellidos} {PersonaJuridicaNombres}".Trim();
    }
}