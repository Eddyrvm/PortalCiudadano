using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalCiudadano.ViewModels
{
    public class RubroAPI
    {
        public string NombreRubro { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorTotalInteres { get; set; }
        public decimal GastosAdministrativos { get; set; }
        public decimal Total { get; set; }
    }
}