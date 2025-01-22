using Newtonsoft.Json.Linq;
using PortalCiudadano.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

public class DeudasController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> ObtenerDeudas(string valorB)
    {
        if (string.IsNullOrWhiteSpace(valorB))
        {
            ViewBag.Message = "Por favor, ingresa un valor para buscar.";
            return View("Index");
        }

        List<RubroAPI> listRubro = new List<RubroAPI>();
        List<ComponenteAPI> listComp = new List<ComponenteAPI>();
        PersonasAPI personas = new PersonasAPI();
        string fechaHora = null;

        try
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://api.gadbolivar.gob.ec/api/public/deudas?criterio=CE&valor={valorB}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Leer el contenido del mensaje de error en el body
                    string mensajeError = await response.Content.ReadAsStringAsync();

                    ViewBag.Message = mensajeError;
                    return View("Index");
                }

                response.EnsureSuccessStatusCode(); // Lanza excepción si el código de estado no es exitoso

                var json = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(json);

                fechaHora = (string)obj["fechaHora"];

                JArray _persona = (JArray)obj["personas"];
                JArray _componentes = (JArray)obj["componentes"];

                foreach (var item in _persona)
                {
                    personas = new PersonasAPI
                    {
                        NombresApellidos = (string)item.SelectToken("apellidosNombres"),
                        Identificador = (string)item.SelectToken("identificador"),
                        Direccion = (string)item.SelectToken("direccion"),
                        Email = (string)item.SelectToken("email"),
                        Telefono = (string)item.SelectToken("telefono"),
                    };
                }

                foreach (var itemComp in _componentes)
                {
                    var componente = new ComponenteAPI
                    {
                        NombreMiembro = (string)itemComp.SelectToken("nombreMiembro"),
                        Valor = (decimal)itemComp.SelectToken("valor"),
                    };
                    listComp.Add(componente);
                }

                if (_componentes.Count > 0)
                {
                    foreach (var y in _componentes)
                    {
                        JArray _titulos = (JArray)y["titulos"];
                        foreach (var i in _titulos)
                        {
                            JArray _rubros = (JArray)i["rubros"];
                            JObject _intereses = (JObject)i["interes"];
                            JObject _emision = (JObject)i["emision"];

                            foreach (var item in _rubros)
                            {
                                var rubro = new RubroAPI
                                {
                                    NombreRubro = (string)item.SelectToken("nombreRubro") + " Periodo: " + (string)i.SelectToken("periodo"),
                                    Valor = (decimal)item.SelectToken("valor"),
                                    ValorTotalInteres = (decimal)_intereses.SelectToken("total"),
                                    GastosAdministrativos = (decimal)_emision.SelectToken("total"),
                                    Total = (decimal)item.SelectToken("total"),
                                };
                                listRubro.Add(rubro);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ViewBag.Message = "Ocurrió un error al procesar la solicitud.";
            return View("Index");
        }

        ViewBag.FechaHora = fechaHora;
        ViewBag.Personas = personas;
        ViewBag.ListComp = listComp;
        ViewBag.ListRubro = listRubro;

        return View("Index");
    }

}