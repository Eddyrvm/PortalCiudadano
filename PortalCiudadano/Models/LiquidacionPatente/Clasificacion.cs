using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PortalCiudadano.Models.LiquidacionPatente
{
    public class Clasificacion
    {
        [Key]
        public int ClasificacionId { get; set; }

        [Required(ErrorMessage = "El nombre de la clasificación es obligatorio.")]
        [StringLength(220, ErrorMessage = "El nombre no debe exceder los {1} caracteres.")]
        [DisplayName("Clasificación domiciliaria")]
        public string NombreClasificacion { get; set; }
    }
}