using System.ComponentModel.DataAnnotations;

namespace PortalCiudadano.Models.MapaSitios
{
    public class MapSitio
    {
        public int Id { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}