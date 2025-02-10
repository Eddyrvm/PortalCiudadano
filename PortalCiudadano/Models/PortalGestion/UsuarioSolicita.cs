using PortalCiudadano.Models.ServiciosPublicos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalCiudadano.Models.PortalGestion
{
    public class UsuarioSolicita
    {
        [Key]
        public int UsuarioSolicitaId { get; set; }

        [Required(ErrorMessage = "La cédula es requerida")]
        [MaxLength(11, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "Cédula")]
        [Index("User_Cedula_Index", 1, IsUnique = true)]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido..")]
        [MaxLength(60, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido..")]
        [MaxLength(60, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Display(Name = "Usuario")]
        public String FullName { get { return string.Format("{0} {1}", Apellidos, Nombres); } }

        public virtual ICollection<SolicitudGestion> SolicitudGestions { get; set; }

    }
}