using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalCiudadano.ViewModels
{
    public class PersonaViewModelAPI
    {
        public int IdPersona { get; set; }
        public string identificador { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public int? IdConyuge { get; set; }
        public string natural_apellidos { get; set; }
        public string natural_nombres { get; set; }
        public DateTime? fechanacimiento { get; set; }
        public string estadocivil { get; set; }
        public string extranjera_apellidos { get; set; }
        public string extranjera_nombres { get; set; }
        public string Sexo { get; set; }
        public string grupo_descripcion { get; set; }
        public string representante { get; set; }
        public string razonsocial { get; set; }
        public string objeto { get; set; }
        public string otros_nombres { get; set; }
        public string otros_apellidos { get; set; }
    }

}