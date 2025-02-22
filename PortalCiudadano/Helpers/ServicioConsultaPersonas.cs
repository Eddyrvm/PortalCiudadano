using Newtonsoft.Json;
using PortalCiudadano.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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
            try
            {
                using (var client = new HttpClient())
                {
                    var url = $"http://192.168.10.204:8181/api/Persona/buscar?filtro={filtro}";
                    var response = client.GetAsync(url).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            throw new ArgumentException("El filtro ingresado no es válido.");
                        }
                        else
                        {
                            throw new Exception("Error al obtener los datos. La API respondió con un estado no exitoso.");
                        }
                    }

                    var json = response.Content.ReadAsStringAsync().Result;
                    var personas = JsonConvert.DeserializeObject<List<PersonaViewModelAPI>>(json);
                    return personas ?? new List<PersonaViewModelAPI>();
                }
            }
            catch (HttpRequestException)
            {
                throw new Exception("No se pudo conectar con el servidor. Verifique su conexión.");
            }
            catch (TaskCanceledException)
            {
                throw new Exception("La solicitud tardó demasiado en responder. Intente nuevamente.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}");
            }
        }



    }
}