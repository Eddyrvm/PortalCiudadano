using PortalCiudadano.Clases;
using PortalCiudadano.Helpers;
using PortalCiudadano.Models;
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

        //[Authorize(Roles = "Admin")]
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


        //[Authorize(Roles = "Admin")]
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
                        var file = string.Format("{0}.jpg", user.Id);
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

        //[Authorize(Roles = "Admin")]
        // GET: Users/Details/5
        public ActionResult Details(int? id)
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

        public ActionResult DetailsUser()
        {
            string currentUserName = User.Identity.Name;

            if (string.IsNullOrEmpty(currentUserName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            User user = db.Users.FirstOrDefault(u => u.UserName == currentUserName);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        //[Authorize(Roles = "Admin")]
        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles = "Editar, Admin")]
        public ActionResult Edit(User user)
        {
            if (user == null || user.Id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Validar que la ID coincida con un usuario existente
            User currentUser = db.Users.Find(user.Id);
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
                var file = string.Format("{0}.jpg", user.Id); // Usamos la ID directamente
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




        //[Authorize(Roles = "Admin")]
        // GET: Users/Delete/5
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

        //[Authorize(Roles = "Admin")]
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
