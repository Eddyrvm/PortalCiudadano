using System.Collections.Generic;

namespace PortalCiudadano.Models.ConsultaDeudas
{
    public class ComponenteVM
    {
        public ComponenteVM()
        {
            Titulos = new List<TituloVM>();
        }

        public string Key { get; set; }
        public string NombreMiembro { get; set; }
        public decimal Valor { get; set; }
        public List<TituloVM> Titulos { get; set; }  // ¡con setter!
    }
}