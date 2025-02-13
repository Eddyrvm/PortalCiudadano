using Newtonsoft.Json;
using PortalCiudadano.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace PortalCiudadano.Helpers
{
    public class ServicioConsultaPersonas
    {
        public PersonaViewModelAPI ObtenerPersonaPorIdentificador(string identificador)
        {
            var client = new HttpClient();
            var response = client.GetAsync($"http://192.168.10.204:8181/api/Persona/{identificador}").Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var persona = JsonConvert.DeserializeObject<PersonaViewModelAPI>(json);
                return persona;
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("El identificador no es válido.");
            }

            return null;
        }

        public List<PersonaViewModelAPI> ObtenerPersonasPorFiltro(string filtro)
        {
            using (var client = new HttpClient())
            {
                var url = $"http://192.168.10.204:8181/api/Persona/buscar?filtro={filtro}";
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var personas = JsonConvert.DeserializeObject<List<PersonaViewModelAPI>>(json);
                    return personas ?? new List<PersonaViewModelAPI>(); // Si es null, retorna lista vacía.
                }
            }

            return new List<PersonaViewModelAPI>(); // Si hay error, retorna lista vacía.
        }


    }
}