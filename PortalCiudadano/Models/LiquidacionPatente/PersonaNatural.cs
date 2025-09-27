using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalCiudadano.Models.LiquidacionPatente
{
    public class PersonaNatural
    {
        [Key]
        public int PersonaNaturalId { get; set; }

        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La cédula debe tener exactamente 10 dígitos.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "La cédula solo puede contener números.")]
        public string PersonaNaturalCedula { get; set; }

        [Display(Name = "RUC")]
        [Required(ErrorMessage = "El RUC es obligatorio.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El RUC debe tener exactamente 13 dígitos.")]
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "El RUC solo puede contener números.")]
        public string PersonaNaturalRUC { get; set; }

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los nombres no deben exceder los 100 caracteres.")]
        public string PersonaNaturalNombres { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los apellidos no deben exceder los 100 caracteres.")]
        public string PersonaNaturalApellidos { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no debe exceder los 200 caracteres.")]
        public string DireccionContribuyente { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [StringLength(15, ErrorMessage = "El teléfono no debe exceder los 15 caracteres.")]
        public string TelefonoContribuyente { get; set; }

        [Display(Name = "FAX")]
        [StringLength(20, ErrorMessage = "El fax no debe exceder los 20 caracteres.")]
        public string FaxContribuyente { get; set; }

        [Display(Name = "Cesilla")]
        [StringLength(50, ErrorMessage = "La casilla no debe exceder los 50 caracteres.")]
        public string CasillaContribuyente { get; set; }

        [Display(Name = "Obligado a llevar contabilidad")]
        [Required(ErrorMessage = "Debe especificar si está obligado a llevar contabilidad.")]
        public bool ObligadoContabilidad { get; set; }

        [Display(Name = "Inicio Actividad")]
        [Required(ErrorMessage = "La fecha de inicio de actividad es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Debe ingresar una fecha válida.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InicioActividad { get; set; }

        [Display(Name = "Capital")]
        [Required(ErrorMessage = "El capital propio es obligatorio.")]
        [Column(TypeName = "decimal")]
        [Range(0, double.MaxValue, ErrorMessage = "El capital propio debe ser un valor positivo.")]
        public decimal CapitalPropio { get; set; }

        [Display(Name = "Fecha Creado")]
        [Required]
        [DataType(DataType.DateTime)]
        [ScaffoldColumn(false)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [NotMapped]
        public string NombreCompleto => $"{PersonaNaturalApellidos} {PersonaNaturalNombres}";

        [NotMapped]
        public string CedulaApellidosNombres =>
    $"{PersonaNaturalCedula?.Trim()} - {PersonaNaturalApellidos?.Trim()} {PersonaNaturalNombres?.Trim()}";

        [NotMapped]
        public string ObligadoContabilidadTexto => ObligadoContabilidad ? "Sí" : "No";

        public virtual ICollection<LiquidarPatentePN> LiquidarPatentePNs { get; set; }
    }
}