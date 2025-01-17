using System;
using System.ComponentModel.DataAnnotations;

namespace PortalCiudadano.Models.ServiciosPublicos
{
    public class ServicioLimpeza
    {
        [Key]
        public int ServicioLimpezaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de la Solicitud")]
        public DateTime FechaSolicitud { get; set; }

        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Institución(Opcional)")]
        public string Institucion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Calle(donde solicita)")]
        public string Calle { get; set; }

        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Referencia")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Detalle una descripción breve de su solicitud")]
        public string DetalleActividad { get; set; }

        [MaxLength(350, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Detalle de la actividad realizada")]
        public string ActividadRealizada { get; set; }

        [Display(Name = "Estado de Solicitud")]
        public string EstadoSolicitud { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe de seleccionar un {0}")]
        [Display(Name = "Persona")]
        public int PersonaId { get; set; }
        public virtual User User { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe de seleccionar un {0}")]
        [Display(Name = "Tipo de Servicio")]
        public int TipoServicioId { get; set; }
        public virtual TipoServicio TipoServicio { get; set; }
    }
}