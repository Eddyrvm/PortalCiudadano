using Newtonsoft.Json.Linq;
using PortalCiudadano.Models.ConsultaDeudas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PortalCiudadano.Controllers
{
    public class ObtenerDeudasController : Controller
    {
        private static decimal ToDecimalSafe(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return 0m;

            // Si ya viene numérico (int/float), tomarlo directo
            if (token.Type == JTokenType.Float || token.Type == JTokenType.Integer)
                return token.Value<decimal>();

            // Si viene como string, intentar con es-EC (coma) y luego Invariant (punto)
            var s = token.Value<string>();
            if (string.IsNullOrWhiteSpace(s))
                return 0m;

            decimal d;
            if (decimal.TryParse(s, NumberStyles.Any, new CultureInfo("es-EC"), out d))
                return d;

            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return d;

            return 0m;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new DeudasVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ObtenerDeudas(DeudasVM model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            // Sanitiza: solo dígitos, máx 13
            model.ValorB = new string((model.ValorB ?? string.Empty).Where(char.IsDigit).ToArray());
            if (model.ValorB.Length == 0 || model.ValorB.Length > 13)
            {
                ModelState.AddModelError("ValorB", "Solo números, máximo 13 dígitos.");
                return View("Index", model);
            }

            var vm = new DeudasVM { ValorB = model.ValorB };

            try
            {
                using (var http = new HttpClient())
                {
                    var url = "https://api.gadbolivar.gob.ec/api/public/deudas?criterio=CE&valor=" + model.ValorB;
                    var resp = await http.GetAsync(url);

                    if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        vm.Mensaje = await resp.Content.ReadAsStringAsync();
                        return View("Index", vm);
                    }

                    resp.EnsureSuccessStatusCode();

                    var json = await resp.Content.ReadAsStringAsync();
                    var root = JObject.Parse(json);

                    vm.TotalGeneral = ToDecimalSafe(root["valor"]);
                    // fechaHora
                    var fechaStr = (string)root["fechaHora"];
                    DateTimeOffset dto;
                    if (!string.IsNullOrEmpty(fechaStr) && DateTimeOffset.TryParse(fechaStr, out dto))
                        vm.FechaConsulta = dto;

                    // persona (preferir "persona", si no, primer "personas")
                    var personaObj = root["persona"] as JObject;
                    if (personaObj == null)
                    {
                        var personasArr = root["personas"] as JArray;
                        if (personasArr != null)
                            personaObj = personasArr.FirstOrDefault() as JObject;
                    }

                    if (personaObj != null)
                    {
                        var p = new PersonaVM
                        {
                            Apellidos = (string)personaObj["apellidos"],
                            Nombres = (string)personaObj["nombres"],
                            ApellidosNombres = (string)personaObj["apellidosNombres"],
                            NombresApellidos = (string)personaObj["nombresApellidos"],
                            Identificador = (string)personaObj["identificador"],
                            Direccion = (string)personaObj["direccion"],
                            Email = (string)personaObj["email"],
                            Telefono = (string)personaObj["telefono"]
                        };
                        vm.Persona = p;
                        vm.Contribuyente = p.ApellidosNombres ?? string.Empty;
                        vm.Cedula = p.Identificador ?? string.Empty;
                    }

                    // componentes
                    // --- COMPONENTES ---
                    var compArr = root["componentes"] as JArray ?? new JArray();

                    foreach (JObject cTok in compArr)
                    {
                        var comp = new ComponenteVM
                        {
                            Key = (string)cTok["key"],
                            NombreMiembro = (string)cTok["nombreMiembro"],
                            Valor = ToDecimalSafe(cTok["valor"]),
                            Titulos = new List<TituloVM>() // asegura que no sea null
                        };

                        // --- TITULOS ---
                        var titArr = cTok["titulos"] as JArray ?? new JArray();

                        foreach (JObject tTok in titArr)
                        {
                            var t = new TituloVM
                            {
                                ClavePredial = (string)tTok["clavePredial"],
                                Descripcion = (string)tTok["descripcion"],
                                FechaEmision = (DateTime?)tTok["fechaEmision"],
                                FechaObligacion = (DateTime?)tTok["fechaObligacion"],
                                FechaPago = (DateTime?)tTok["fechaPago"],
                                Year = (int?)tTok["year"] ?? 0,
                                Month = (int?)tTok["month"] ?? 0,
                                Periodo = (string)tTok["periodo"],
                                ValorNeto = ToDecimalSafe(tTok["valorNeto"]),
                                ValorBruto = ToDecimalSafe(tTok["valorBruto"]),
                                ValorTitulo = ToDecimalSafe(tTok["valorTitulo"]),
                                ValorAbono = ToDecimalSafe(tTok["valorAbono"]),
                                ValorInteres = ToDecimalSafe(tTok["valorInteres"]),
                                ValorEmision = ToDecimalSafe(tTok["valorEmision"]),
                                ValorTotalTexto = (string)tTok["valorTotal"],
                                Rubros = new List<RubroVM>() // asegura que no sea null
                            };

                            // Parsear valorTotalTexto ("14,34") a decimal con cultura es-EC
                            if (!string.IsNullOrWhiteSpace(t.ValorTotalTexto))
                            {
                                var ec = new CultureInfo("es-EC");
                                decimal vt;
                                if (decimal.TryParse(t.ValorTotalTexto, NumberStyles.Any, ec, out vt))
                                    t.ValorTotalDecimal = vt;
                            }

                            // rubros
                            var rubArr = tTok["rubros"] as JArray;
                            if (rubArr != null && rubArr.Count > 0)
                            {
                                if (t.Rubros == null) t.Rubros = new List<RubroVM>(); // por si tu clase antigua no tenía ctor

                                foreach (var rTok in rubArr.Children<JObject>())
                                {
                                    t.Rubros.Add(new RubroVM
                                    {
                                        NombreRubro = (string)(rTok["nombreRubro"] ?? string.Empty),
                                        CodigoRubro = (int?)(rTok["codigoRubro"]) ?? 0,
                                        Valor = ToDecimalSafe(rTok["valor"]),
                                        Emision = ToDecimalSafe(rTok["emision"]),
                                        Interes = ToDecimalSafe(rTok["interes"]),
                                        Total = ToDecimalSafe(rTok["total"]),
                                        Key = (string)(rTok["key"] ?? string.Empty)
                                    });
                                }
                            }

                            // interes
                            var interesObj = tTok["interes"] as JObject;
                            if (interesObj != null)
                            {
                                t.Interes = new RubroVM
                                {
                                    NombreRubro = (string)(interesObj["nombreRubro"] ?? string.Empty),
                                    CodigoRubro = (int?)(interesObj["codigoRubro"]) ?? 0,
                                    Valor = ToDecimalSafe(interesObj["valor"]),
                                    Total = ToDecimalSafe(interesObj["total"]),
                                    Key = (string)(interesObj["key"] ?? string.Empty)
                                };
                            }

                            // emision
                            var emisionObj = tTok["emision"] as JObject;
                            if (emisionObj != null)
                            {
                                t.Emision = new RubroVM
                                {
                                    NombreRubro = (string)(emisionObj["nombreRubro"] ?? string.Empty),
                                    CodigoRubro = (int?)(emisionObj["codigoRubro"]) ?? 0,
                                    Valor = ToDecimalSafe(emisionObj["valor"]),
                                    Total = ToDecimalSafe(emisionObj["total"]),
                                    Key = (string)(emisionObj["key"] ?? string.Empty)
                                };
                            }

                            comp.Titulos.Add(t);
                        }

                        vm.Componentes.Add(comp);
                    }
                }
            }
            catch
            {
                vm.Mensaje = "Ocurrió un error al procesar la solicitud.";
            }

            return View("Index", vm);
        }
    }
}