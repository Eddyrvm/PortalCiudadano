using PortalCiudadano.Models.ServiciosPublicos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace PortalCiudadano.Models.PortalGestion
{
    public class SolicitudGestion
    {
        [Key]
        public int SolicitudId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de la Solicitud")]
        public DateTime FechaSolicitud { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Detalle una descripción breve de solicitud")]
        public string SolicitudUsuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Obervaciones")]
        public string observaciones { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Foto1 { get; set; }

        [Display(Name = "Foto Gestion 1")]
        [NotMapped]
        public HttpPostedFileBase FotoFile1 { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Foto2 { get; set; }

        [Display(Name = "Foto Gestion 2")]
        [NotMapped]
        public HttpPostedFileBase FotoFile12 { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe de seleccionar un {0}")]
        [Display(Name = "Tipo de Servicio")]
        public int UsuarioSolicitaId { get; set; }
        public virtual UsuarioSolicita UsuarioSolicita { get; set; }

    }
}