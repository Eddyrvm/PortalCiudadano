namespace PortalCiudadano.Models.ConsultaDeudas
{
    public class RubroVM
    {
        public string NombreRubro { get; set; }
        public int CodigoRubro { get; set; }
        public decimal Valor { get; set; }
        public decimal Emision { get; set; }
        public decimal Interes { get; set; }
        public decimal Total { get; set; }
        public string Key { get; set; }
    }
}