using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PortalCiudadano.Models.LiquidacionPatente
{
    public class InfoEstadisticaProduc
    {
        [Key]
        public int InfoEstadisticaProducId { get; set; }

        [Required(ErrorMessage = "El nombre de la información estadistica es obligatorio.")]
        [StringLength(220, ErrorMessage = "El nombre no debe exceder los {1} caracteres.")]
        [DisplayName("Nombre de información estadistica")]
        public string IndoEstadisticaProducName { get; set; }

        public virtual ICollection<LiquidarPatentePN> LiquidarPatentePNs { get; set; }
    }
}