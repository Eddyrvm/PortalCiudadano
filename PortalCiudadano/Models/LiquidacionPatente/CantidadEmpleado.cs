using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PortalCiudadano.Models.LiquidacionPatente
{
    public class CantidadEmpleado
    {
        [Key]
        public int CantidadEmpleadoId { get; set; }

        [Required(ErrorMessage = "El nombre de la Actividad es obligatorio.")]
        [StringLength(220, ErrorMessage = "El nombre no debe exceder los {1} caracteres.")]
        [DisplayName("Actividad")]
        public string NumeroEmpleados { get; set; }

        public virtual ICollection<LiquidarPatentePN> LiquidarPatentePNs { get; set; }
    }
}