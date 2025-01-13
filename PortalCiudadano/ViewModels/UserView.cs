using PortalCiudadano.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortalCiudadano.ViewModels
{
    public class UserView
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        public RoleView Role { get; set; }

        public List<RoleView> Roles { get; set; }

        public string NombreCompleto { get; set; }

        public User User { get; set; }

        public List<RoleView> Users { get; set; }
    }
}