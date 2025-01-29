using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortalCiudadano.Models.ServiciosPublicos
{
    public class TipoServicio
    {
        [Key]
        public int TipoServicioId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Display(Name = "Tipo de Servicio")]
        public string NombreServicio { get; set; }

        public virtual ICollection<ServicioLimpeza> ServicioLimpiezas { get; set; }
    }
}