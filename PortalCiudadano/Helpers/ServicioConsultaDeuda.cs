using Newtonsoft.Json.Linq;
using PortalCiudadano.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class ServicioConsultaDeuda
{
    private readonly HttpClient _httpClient;

    public ServicioConsultaDeuda()
    {
        _httpClient = new HttpClient();
    }

    public async Task<(List<RubroAPI> ListRubro, List<ComponenteAPI> ListComp, PersonasAPI Personas, string FechaHora, string ErrorMessage)> ObtenerDeudasAsync(string valorB)
    {
        List<RubroAPI> listRubro = new List<RubroAPI>();
        List<ComponenteAPI> listComp = new List<ComponenteAPI>();
        PersonasAPI personas = new PersonasAPI();
        string fechaHora = null;
        string errorMessage = null;

        if (string.IsNullOrWhiteSpace(valorB))
        {
            errorMessage = "Por favor, ingresa un valor para buscar.";
            return (listRubro, listComp, personas, fechaHora, errorMessage);
        }

        try
        {
            var response = await _httpClient.GetAsync($"https://api.gadbolivar.gob.ec/api/public/deudas?criterio=CE&valor={valorB}");

            //AYUDAR CON ESTE CODIGO A CARLIN PARA CAPTURAR EL ESTADO DE LA RESPUESTA
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                errorMessage = await response.Content.ReadAsStringAsync();
                return (listRubro, listComp, personas, fechaHora, errorMessage);
            }

            response.EnsureSuccessStatusCode();

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
        catch (Exception ex)
        {
            errorMessage = "Ocurrió un error al procesar la solicitud.";
            Console.WriteLine(ex.Message);
        }

        return (listRubro, listComp, personas, fechaHora, errorMessage);
    }
}