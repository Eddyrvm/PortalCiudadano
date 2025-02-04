﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PortalCiudadano.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace PortalCiudadano.Clases
{
    public class UsersHelper : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static PortalCiudadanoContext db = new PortalCiudadanoContext();

        public static bool DeleteUser(string userName, string rolName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(userName);
            if (userASP == null)
            {
                return false;
            }
            var response = userManager.RemoveFromRole(userASP.Id, rolName);
            return response.Succeeded;
        }
        public static bool UpdateUserName(string currentUserName, string newUserName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(currentUserName);
            if (userASP == null)
            {
                return false;
            }
            userASP.UserName = newUserName;
            userASP.Email = newUserName;

            var response = userManager.Update(userASP);
            return response.Succeeded;
        }
        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }
        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var email = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassWord"];
            var nombres = WebConfigurationManager.AppSettings["AdminFirstName"];
            var apellidos = WebConfigurationManager.AppSettings["AdminLastName"];
            var cedula = WebConfigurationManager.AppSettings["AdminCedula"];

            // Verifica si el usuario ya existe en ApplicationUser
            var userASP = userManager.FindByName(email);
            if (userASP == null)
            {
                // Crear usuario en ApplicationUser
                var newUserASP = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                var result = userManager.Create(newUserASP, password);
                if (result.Succeeded)
                {
                    userManager.AddToRole(newUserASP.Id, "Admin");

                    // Crear el usuario en la tabla Users
                    using (var dbContext = new PortalCiudadanoContext())
                    {
                        var newUser = new User
                        {
                            UserName = email,
                            Nombres = nombres,
                            Apellidos = apellidos,
                            Cedula = cedula,
                        };

                        dbContext.Users.Add(newUser);
                        dbContext.SaveChanges();
                    }
                }

                return;
            }

            // Verifica si el usuario ya tiene el rol "Admin"
            if (!userManager.IsInRole(userASP.Id, "Admin"))
            {
                userManager.AddToRole(userASP.Id, "Admin");
            }

            // Verifica si el usuario existe en la tabla Users
            using (var dbContext = new PortalCiudadanoContext())
            {
                var existingUser = dbContext.Users.FirstOrDefault(u => u.UserName == email);
                if (existingUser == null)
                {
                    var newUser = new User
                    {
                        UserName = email,
                        Nombres = nombres,
                        Apellidos = apellidos,
                        Cedula = cedula,
                    };

                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();
                }
            }
        }

        public static void CreateUserASPNuevo(string email, string password, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                userASP = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                // Crear el usuario con la contraseña proporcionada
                var result = userManager.Create(userASP, password);
                if (!result.Succeeded)
                {
                    // Manejar el error en caso de que no se pueda crear el usuario
                    throw new Exception(string.Join(", ", result.Errors));
                }
            }
            userManager.AddToRole(userASP.Id, roleName);
        }

        //Crear el super usuario al iniciar la aplicacion 
        public static void CreateUserASP(string email, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser
            {
                Email = email,
                UserName = email,
            };

            userManager.Create(userASP, password);
            userManager.AddToRole(userASP.Id, roleName);
        }
        public static async Task PasswordRecovery(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                return;
            }

            var user = db.Users.Where(tp => tp.UserName == email).FirstOrDefault();
            if (user == null)
            {
                return;
            }

            var random = new Random();
            var newPassword = string.Format("{0}{1}{2:04}*",
                user.Nombres.Trim().ToUpper().Substring(0, 1),
                user.Apellidos.Trim().ToLower(),
                random.Next(10000));

            userManager.RemovePassword(userASP.Id);
            userManager.AddPassword(userASP.Id, newPassword);

            var subject = "Taxes Password Recovery";
            var body = string.Format(@"
                <h1>Taxes Password Recovery</h1>
                <p>Yor new password is: <strong>{0}</strong></p>
                <p>Please change it for one, that you remember easyly",
                newPassword);

            await MailHelper.SendMail(email, subject, body);
        }
        public void Dispose()
        {
            userContext.Dispose();
            db.Dispose();
        }
    }
}