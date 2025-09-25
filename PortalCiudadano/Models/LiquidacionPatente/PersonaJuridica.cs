using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalCiudadano.Models.LiquidacionPatente
{
    public class PersonaJuridica
    {
        [Key]
        public int PersonaJuridicalId { get; set; }

        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [StringLength(300, ErrorMessage = "La razón social no debe exceder los 200 caracteres.")]
        public string PersonaJuridicaRazonSocial { get; set; }

        [Required(ErrorMessage = "El RUC es obligatorio.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El RUC debe tener exactamente 13 dígitos.")]
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "El RUC solo puede contener números.")]
        public string PersonaJuridicaRUC { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los nombres no deben exceder los 100 caracteres.")]
        public string PersonaJuridicaNombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los apellidos no deben exceder los 100 caracteres.")]
        public string PersonaJuridicaApellidos { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no debe exceder los 200 caracteres.")]
        public string DireccionContribuyente { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [StringLength(15, ErrorMessage = "El teléfono no debe exceder los 15 caracteres.")]
        public string TelefonoContribuyente { get; set; }

        [StringLength(20, ErrorMessage = "El fax no debe exceder los 20 caracteres.")]
        public string FaxContribuyente { get; set; }

        [StringLength(50, ErrorMessage = "La casilla no debe exceder los 50 caracteres.")]
        public string CasillaContribuyente { get; set; }

        [Required(ErrorMessage = "Debe especificar si está obligado a llevar contabilidad.")]
        public bool ObligadoContabilidad { get; set; }

        [Required(ErrorMessage = "La fecha de inicio de actividad es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Debe ingresar una fecha válida.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InicioActividad { get; set; }

        [Required(ErrorMessage = "El capital propio es obligatorio.")]
        [Column(TypeName = "decimal")]
        [Range(0, double.MaxValue, ErrorMessage = "El capital propio debe ser un valor positivo.")]
        public decimal CapitalPropio { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [NotMapped]
        public string NombreCompleto => $"{PersonaJuridicaApellidos} {PersonaJuridicaNombres}";

        [NotMapped]
        public string ObligadoContabilidadTexto => ObligadoContabilidad ? "Sí" : "No";
    }
}