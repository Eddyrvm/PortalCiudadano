using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PortalCiudadano.Models.LiquidacionPatente
{
    public class Actividad
    {
        [Key]
        public int ActividadId { get; set; }

        [Required(ErrorMessage = "El nombre de la Actividad es obligatorio.")]
        [StringLength(220, ErrorMessage = "El nombre no debe exceder los {1} caracteres.")]
        [DisplayName("Actividad")]
        public string NombreActividad { get; set; }

        public virtual ICollection<LiquidarPatentePN> LiquidarPatentePNs { get; set; }

        public virtual ICollection<LiquidarPatentePJ> LiquidarPatentePJs { get; set; }
    }
}