using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalCiudadano.Models.LiquidacionPatente
{
    public class LiquidarPatentePJ
    {
        [Key]
        public int LiquidarPatentePJId { get; set; }

        [Required(ErrorMessage = "El contador es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El contador debe ser mayor a cero.")]
        [DisplayName("Contador")]
        public int Contador { get; set; }

        /// <summary>
        /// 1 = Primera vez, 2 = Renovación
        /// </summary>
        [Required(ErrorMessage = "El tipo de solicitud es obligatorio.")]
        [Range(1, 2, ErrorMessage = "Tipo de solicitud inválido (1 = Primera vez, 2 = Renovación).")]
        [DisplayName("Tipo de solicitud")]
        public int TipoSolicitud { get; set; }

        [NotMapped]
        public string TipoSolicitudDescripcion =>
            TipoSolicitud == 1 ? "Primera vez" :
            TipoSolicitud == 2 ? "Renovación" : "Desconocido";

        [Required(ErrorMessage = "El número de patente asignada es obligatorio.")]
        [StringLength(50, ErrorMessage = "El número de patente no debe exceder {1} caracteres.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números (0-9).")]
        [DisplayName("N° de Patente Asignada")]
        public string NumPatenteAsignada { get; set; }

        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [ScaffoldColumn(false)]
        [DisplayName("Fecha de creación")]
        public DateTime FechaCreada { get; set; }

        // =========================
        //       RELACIONES (FK)
        // =========================

        // Persona Natural
        [Required(ErrorMessage = "La persona Juridica es obligatoria.")]
        [DisplayName("Persona Juridica")]
        public int PersonaJuridicalId { get; set; }

        [ForeignKey(nameof(PersonaJuridicalId))]
        public virtual PersonaJuridica PersonaJuridicas { get; set; }

        // Clasificación
        [Required(ErrorMessage = "La clasificación es obligatoria.")]
        [DisplayName("Clasificación")]
        public int ClasificacionId { get; set; }

        [ForeignKey(nameof(ClasificacionId))]
        public virtual Clasificacion Clasificacion { get; set; }

        // Actividad
        [Required(ErrorMessage = "La actividad es obligatoria.")]
        [DisplayName("Actividad")]
        public int ActividadId { get; set; }

        [ForeignKey(nameof(ActividadId))]
        public virtual Actividad Actividad { get; set; }

        // Información Estadística de Producción
        [Required(ErrorMessage = "La información estadística es obligatoria.")]
        [DisplayName("Información estadística")]
        public int InfoEstadisticaProducId { get; set; }

        [ForeignKey(nameof(InfoEstadisticaProducId))]
        public virtual InfoEstadisticaProduc InfoEstadisticaProduc { get; set; }

        // Cantidad de Empleados
        [Required(ErrorMessage = "La cantidad de empleados es obligatoria.")]
        [DisplayName("Cantidad de empleados")]
        public int CantidadEmpleadoId { get; set; }

        [ForeignKey(nameof(CantidadEmpleadoId))]
        public virtual CantidadEmpleado CantidadEmpleado { get; set; }
    }
}