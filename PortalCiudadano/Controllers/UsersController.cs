using Microsoft.AspNet.Identity.EntityFramework;
using PortalCiudadano.Clases;
using PortalCiudadano.Helpers;
using PortalCiudadano.Models;
using PortalCiudadano.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class UsersController : Controller
    {
        private PortalCiudadanoContext db = new PortalCiudadanoContext();
        private ApplicationDbContext _identityContext = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public async Task<JsonResult> BuscarPersonaPorIdentificador(string identificador)
        {
            // Instancia de la clase ServicioConsultaDeuda
            var servicioConsultaDeuda = new ServicioConsultaDeuda();

            // Llama al método ObtenerDeudasAsync pasando el identificador como valorB
            var (listRubro, listComp, personas, fechaHora, errorMessage) = await servicioConsultaDeuda.ObtenerDeudasAsync(identificador);

            if (errorMessage == null)
            {
                return Json(new
                {
                    success = true,
                    deudas = new
                    {
                        FechaHora = fechaHora,
                        Personas = personas,
                        ListComp = listComp,
                        ListRubro = listRubro
                    }
                },
                JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = errorMessage // Error del servicio de consulta de deudas
                },
                JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPersona(string identificador)
        {
            var _servicioConsultaPersonas = new ServicioConsultaPersonas();
            try
            {
                // Consumir el servicio y obtener los datos de la persona
                var persona = _servicioConsultaPersonas.ObtenerPersonaPorIdentificador(identificador);

                if (persona == null)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                // Mapear los datos de la API al modelo utilizado en la vista
                var personaViewModel = new PersonaViewModelAPI
                {
                    IdPersona = persona.IdPersona,
                    identificador = persona.identificador,
                    direccion = persona.direccion,
                    telefono = persona.telefono,
                    email = persona.email,
                    natural_nombres = persona.natural_nombres,
                    natural_apellidos = persona.natural_apellidos
                };

                return Json(personaViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (ArgumentException ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Ocurrió un error inesperado." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            // Validar la contraseña y confirmación
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.ConfirmPassword))
            {
                ModelState.AddModelError("Password", "La contraseña es requerida.");
                ModelState.AddModelError("ConfirmPassword", "La confirmación de la contraseña es requerida.");
            }
            else if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.");
            }

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                try
                {
                    db.SaveChanges();

                    if (user.FotoFile != null)
                    {
                        var folder = "~/Content/Users";
                        var file = string.Format("{0}.jpg", user.UserId);
                        var response = FileHelper.UploadPhoto(user.FotoFile, folder, file);
                        if (response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            user.Foto = pic;
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    // Llama al nuevo método CreateUserASPNuevo
                    UsersHelper.CreateUserASPNuevo(user.UserName, user.Password, "Ver");
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "El registro ya existe en la base de datos");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SetUserId(int id)
        {
            TempData["UserId"] = id;
            return RedirectToAction("Details");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details()
        {
            // Verifica si TempData tiene el UserId
            if (TempData["UserId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "El identificador de usuario no está disponible.");
            }

            // Recupera el UserId desde TempData
            if (!int.TryParse(TempData["UserId"].ToString(), out int userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "El identificador de usuario no es válido.");
            }

            // Busca el usuario en el contexto principal
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return HttpNotFound("No se encontró el usuario en el sistema.");
            }

            string userName = user.UserName;

            // Busca el usuario en el contexto de Identity usando UserName y mapea a UserView
            var userInIdentity = _identityContext.Users
                                    .Where(u => u.UserName == userName)
                                    .FirstOrDefault();

            if (userInIdentity == null)
            {
                return HttpNotFound("No se encontró el usuario en el sistema de identidad.");
            }

            // Busca los roles asociados a este usuario en el contexto de Identity
            var userRoles = (from ur in _identityContext.Set<IdentityUserRole>()
                             join r in _identityContext.Roles on ur.RoleId equals r.Id
                             where ur.UserId == userInIdentity.Id
                             select new RoleView
                             {
                                 Name = r.Name
                             }).ToList();

            // Pasar los roles a la vista
            ViewBag.Roles = userRoles;

            // Usar el modelo User para la vista
            return View(user);
        }

        [Authorize]
        public ActionResult DetailsUser()
        {
            // Obtener el nombre de usuario o el identificador del usuario logueado
            string userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Usuario no autenticado");
            }

            // Buscar el usuario en la base de datos usando el nombre de usuario
            User user = db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return HttpNotFound("Usuario no encontrado");
            }

            // Guardar la ID del usuario en TempData
            TempData["UserId"] = user.UserId;

            return View(user); // Pasar el modelo del usuario a la vista
        }

        [Authorize]
        public ActionResult EditUser()
        {
            // Verificar si TempData contiene la ID del usuario
            if (TempData["UserId"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "ID de usuario no proporcionada");
            }

            int userId = (int)TempData["UserId"];

            // Buscar el usuario en la base de datos utilizando la ID
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return HttpNotFound("Usuario no encontrado");
            }

            return View(user); // Pasar el modelo del usuario a la vista de edición
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            // Validar que la ID coincida con un usuario existente
            var currentUser = db.Users.Find(user.UserId);
            if (currentUser == null)
            {
                return HttpNotFound();
            }

            // Remover la validación de los campos no requeridos
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid)
            {
                // Opcional: Registrar errores para depuración
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(user);
            }

            // Manejo de la foto si se sube un nuevo archivo
            if (user.FotoFile != null)
            {
                var folder = "~/Content/Users";
                var file = string.Format("{0}.jpg", user.UserId); // Usamos la ID directamente
                var response = FileHelper.UploadPhoto(user.FotoFile, folder, file);
                if (response)
                {
                    currentUser.Foto = string.Format("{0}/{1}", folder, file);
                }
            }

            // Actualizar campos del usuario
            currentUser.UserName = user.UserName;
            currentUser.Nombres = user.Nombres;
            currentUser.Apellidos = user.Apellidos;
            currentUser.Cedula = user.Cedula;
            currentUser.Telefono = user.Telefono;
            // Actualizar otros campos necesarios

            try
            {
                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Los datos fueron modificados.";
                return RedirectToAction("DetailsUser");
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.InnerException?.Message.Contains("_Index") == true)
                {
                    ModelState.AddModelError(string.Empty, "El registro ya existe en la base de datos.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(User user)
        {
            if (user == null || user.UserId <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Validar que la ID coincida con un usuario existente
            User currentUser = db.Users.Find(user.UserId);
            if (currentUser == null)
            {
                return HttpNotFound();
            }

            // Remover la validación de los campos no requeridos
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid)
            {
                // Opcional: Registrar errores para depuración
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(user);
            }

            // Manejo de la foto si se sube un nuevo archivo
            if (user.FotoFile != null)
            {
                var folder = "~/Content/Users";
                var file = string.Format("{0}.jpg", user.UserId); // Usamos la ID directamente
                var response = FileHelper.UploadPhoto(user.FotoFile, folder, file);
                if (response)
                {
                    currentUser.Foto = string.Format("{0}/{1}", folder, file);
                }
            }

            // Actualizar campos del usuario
            currentUser.UserName = user.UserName;
            currentUser.Nombres = user.Nombres;
            currentUser.Apellidos = user.Apellidos;
            currentUser.Cedula = user.Cedula;
            currentUser.Telefono = user.Telefono;
            // Actualizar otros campos necesarios

            try
            {
                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Los datos fueron modificados.";
                return RedirectToAction("DetailsUser");
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.InnerException?.Message.Contains("_Index") == true)
                {
                    ModelState.AddModelError(string.Empty, "El registro ya existe en la base de datos.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            UsersHelper.DeleteUser(user.UserName, "Ver");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }

                if (_identityContext != null)
                {
                    _identityContext.Dispose();
                    _identityContext = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
