using System;
using System.Collections.Generic;

namespace PortalCiudadano.Models.ConsultaDeudas
{
    public class TituloVM
    {
        public TituloVM()
        {
            Rubros = new List<RubroVM>();
        }

        public string ClavePredial { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaObligacion { get; set; }
        public DateTime? FechaPago { get; set; }
        public string Periodo { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal ValorNeto { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal ValorTitulo { get; set; }
        public decimal ValorAbono { get; set; }
        public decimal ValorInteres { get; set; }
        public decimal ValorEmision { get; set; }

        public List<RubroVM> Rubros { get; set; }  // <- inicializada en ctor
        public RubroVM Interes { get; set; }
        public RubroVM Emision { get; set; }

        public string ValorTotalTexto { get; set; }
        public decimal ValorTotalDecimal { get; set; }
    }
}