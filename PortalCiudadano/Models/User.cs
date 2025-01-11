using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Web;

namespace PortalCiudadano.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido..")]
        [MaxLength(256, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "E-Mail")]
        [Index("User_E-Mail_Index", 1, IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [StringLength(255, ErrorMessage = "La contraseña debe tener minimo 5 hasta 255 Caracteres", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [StringLength(255, ErrorMessage = "La contraseña debe tener minimo 5 hasta 255 Caracteres", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

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

        [Display(Name = "Usuario")]
        public string UsuarioLogin
        {
            get
            {
                string primerNombre = Regex.Match(Nombres ?? string.Empty, @"^\w+").Value;
                string primerApellido = Regex.Match(Apellidos ?? string.Empty, @"^\w+").Value;
                return $"{primerNombre} {primerApellido}";
            }
        }

        [DataType(DataType.PhoneNumber)]
        [MaxLength(16, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Foto { get; set; }

        [Display(Name = "Foto Usuario")]
        [NotMapped]
        public HttpPostedFileBase FotoFile { get; set; }
    }
}