using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PortalCiudadano.Models;
using PortalCiudadano.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class SettingUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private PortalCiudadanoContext db2 = new PortalCiudadanoContext();
        // GET: Users

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            // Obtiene todos los usuarios
            var users = userManager.Users.ToList();
            var usersView = new List<UserView>();

            foreach (var user in users)
            {
                // Busca el User correspondiente donde el UserName coincida
                var userEntity = db2.Users.FirstOrDefault(u => u.UserName == user.UserName);

                if (userEntity != null) // Si se encuentra el usuario
                {
                    // Obtiene los roles del usuario
                    var userRoles = userManager.GetRoles(user.Id);

                    var userView = new UserView
                    {
                        EMail = user.Email,
                        Name = user.UserName,
                        UserId = user.Id,
                        Role = new RoleView(), // Asigna un rol si es necesario
                        Roles = userRoles.Select(role => new RoleView { Name = role }).ToList(), // Asigna los roles
                        User = userEntity, // Almacena la entidad User si es necesario
                        NombreCompleto = $"{userEntity.Nombres} {userEntity.Apellidos}" // Concatenar directamente de userEntity
                    };
                    usersView.Add(userView);
                }
            }

            return View(usersView);
        }


        //[Authorize(Roles = "Admin")]
        public ActionResult Roles(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Obtener el contexto de la base de datos para acceder a la entidad User
            var userEntity = db2.Users.FirstOrDefault(u => u.UserName == user.UserName);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                var role = roles.Find(r => r.Id == item.RoleId);
                if (role != null)
                {
                    var roleView = new RoleView
                    {
                        Name = role.Name,
                        RoleId = role.Id
                    };
                    rolesView.Add(roleView);
                }
            }

            var userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                UserId = user.Id,
                Roles = rolesView,
                // Carga el Nombre Completo desde userEntity
                NombreCompleto = userEntity != null ? $"{userEntity.Nombres} {userEntity.Apellidos}" : string.Empty
            };

            return View(userView);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddRole(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                UserId = user.Id
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var rolesList = roleManager.Roles.ToList();
            rolesList.Add(new IdentityRole { Id = "", Name = "[Seleccione un rol...]" });
            rolesList = rolesList.OrderBy(r => r.Name).ToList();

            ViewBag.RoleId = new SelectList(rolesList, "Id", "Name");

            // Verificar si la solicitud es AJAX
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AddRolePartial", userView); // Retorna la vista parcial
            }

            return View(userView); // Retorna la vista completa si no es AJAX
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddRole(string userId, FormCollection form)
        {
            var roleId = Request["RoleId"];

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userId);

            var userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                UserId = user.Id
            };

            if (string.IsNullOrEmpty(roleId))
            {

                ViewBag.Error = "Debe seleccionar un rol";
                var List = roleManager.Roles.ToList();
                List.Add(new IdentityRole { Id = "", Name = "[Seleccione un rol...]" });
                List = List.OrderBy(r => r.Name).ToList();
                ViewBag.RoleId = new SelectList(List, "Id", "Name");
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_AddRolePartial", new UserView { UserId = userId });
                }

                // Si no es una solicitud AJAX, retornar la vista completa
                return View(new UserView { UserId = userId });

            }

            var roles = roleManager.Roles.ToList();
            var role = roles.Find(r => r.Id == roleId);

            if (!userManager.IsInRole(userId, role.Name))
            {
                userManager.AddToRole(userId, role.Name);
            }

            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {

                role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    Name = role.Name,
                    RoleId = role.Id
                };
                rolesView.Add(roleView);
            }
            userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                Roles = rolesView,
                UserId = user.Id

            };

            return View("Roles", userView);

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var user = userManager.Users.ToList().Find(u => u.Id == userId);
            var role = roleManager.Roles.ToList().Find(r => r.Id == roleId);

            if (userManager.IsInRole(user.Id, role.Name))
            {
                userManager.RemoveFromRole(user.Id, role.Name);
            }

            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    Name = role.Name,
                    RoleId = role.Id
                };
                rolesView.Add(roleView);
            }
            var userView = new UserView
            {
                EMail = user.Email,
                Name = user.UserName,
                Roles = rolesView,
                UserId = user.Id
            };
            return View("Roles", userView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            db2.Dispose();
            base.Dispose(disposing);
        }
    }
}