using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCiudadano.Models.ConsultaDeudas
{
    public class DeudasVM
    {
        // Búsqueda
        [Display(Name = "Cédula / Identificador")]
        [Required(ErrorMessage = "Ingrese un valor de búsqueda.")]
        [RegularExpression(@"^\d{1,13}$", ErrorMessage = "Solo números, máximo 13 dígitos.")]
        [StringLength(13, MinimumLength = 1)]
        public string ValorB { get; set; } = string.Empty;

        // Cabecera resultado
        public DateTimeOffset? FechaConsulta { get; set; }

        public string Contribuyente { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public PersonaVM Persona { get; set; } // puede quedar null, la vista lo verifica

        // Detalle
        public List<ComponenteVM> Componentes { get; set; } = new List<ComponenteVM>();

        // Mensaje
        public string Mensaje { get; set; } // puede ser null

        public decimal TotalGeneral { get; set; }
    }
}