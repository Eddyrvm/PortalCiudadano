using Newtonsoft.Json;
using PortalCiudadano.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class PersonasController : Controller
    {
        private readonly string apiUrl = "http://192.168.10.204:8181/api/Persona/";

        public async Task<ActionResult> GetPersona(string identificador)
        {
            if (string.IsNullOrEmpty(identificador) || !long.TryParse(identificador, out _))
            {
                TempData["Error"] = "El identificador debe ser un número.";
                return RedirectToAction("Index");
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(identificador);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        PersonaViewModelAPI persona = JsonConvert.DeserializeObject<PersonaViewModelAPI>(jsonResponse);

                        return View(persona); // Devuelve la vista con el modelo PersonaViewModel
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        TempData["Error"] = "El identificador no es válido. Solo se permiten números.";
                    }
                    else
                    {
                        TempData["Error"] = $"Error al consumir la API: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al procesar la solicitud: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}