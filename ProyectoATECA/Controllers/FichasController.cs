using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Speech.AudioFormat;
using System.Text;
//using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using ProyectoATECA.Hubs;
using ProyectoATECA.Models;




namespace ProyectoATECA.Controllers
{
    public class FichasController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();
        public FichasController() { }
        // GET: Fichas
        public ActionResult Index()
        {
            var fichas = db.Fichas.Include(f => f.Servicio);
            return View(
                db.Fichas.Where(f => f.atendido == "No" && f.fecha.DayOfYear == DateTime.Now.DayOfYear).OrderBy(f => f.fecha));
        }

        public ActionResult GetFichasData()
        {
            int id_rol = Convert.ToInt32(System.Web.HttpContext.Current.Session["ROL"]);
            int id_servicio = Convert.ToInt32(System.Web.HttpContext.Current.Session["SERV"]);
            if (id_rol == 3)
            {
                return PartialView("_FichasData", db.Fichas.Where(f => f.atendido == "No" && f.tipoFicha == "Ley 7600" &&
                f.fecha.Day == DateTime.Now.Day &&
                f.fecha.Month == DateTime.Now.Month &&
                f.fecha.Year == DateTime.Now.Year
                ).ToList().OrderBy(f => f.fecha));
            }
            if (id_rol == 2)
            {
                if (id_servicio == 0)
                {
                    return PartialView("_FichasData", db.Fichas.Where(f => f.atendido == "No" && f.tipoFicha == "Regular" &&
                     f.fecha.Day == DateTime.Now.Day &&
                     f.fecha.Month == DateTime.Now.Month &&
                     f.fecha.Year == DateTime.Now.Year
                     ).ToList().OrderBy(f => f.fecha));
                }
                else
                {
                    return PartialView("_FichasData", db.Fichas.Where(f => f.atendido == "No" && f.tipoFicha == "Regular" &&
                    f.fecha.Day == DateTime.Now.Day &&
                    f.fecha.Month == DateTime.Now.Month &&
                    f.fecha.Year == DateTime.Now.Year &&
                    f.ID_servicio == id_servicio
                    ).ToList().OrderBy(f => f.fecha));
                }


            }
            else
            {
                return PartialView("_FichasData", db.Fichas.Where(f => f.atendido == "No" &&
                f.fecha.Day == DateTime.Now.Day &&
                f.fecha.Month == DateTime.Now.Month &&
                f.fecha.Year == DateTime.Now.Year
                ).ToList().OrderBy(f => f.fecha));
            }

        }

        //public ViewResult Index()
        //{
        //    var countries = new SelectList(
        //        db.Racers.Select(r => r.Country).Distinct().ToList());

        //    ViewBag.Countries = countries;
        //    return View();
        //}

        //public PartialViewResult RacersByCountryPartial(string id)
        //{
        //    return PartialView(
        //        db.Racers.Where(r => r.Country == id).OrderByDescending(r => r.Wins)
        //            .ThenBy(r => r.Lastname).ToList());
        //}

        // GET: Fichas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ficha ficha = db.Fichas.Find(id);
            if (ficha == null)
            {
                return HttpNotFound();
            }
            return View(ficha);
        }
        public ActionResult Escoger()
        {

            return View();
        }
        // GET: Fichas/Create

        public ActionResult Create()
        {
            ViewData["codigoFicha"] = DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
            ViewData["fecha"] = DateTime.Now;
            ViewData["atendido"] = "No";
            ViewData["llamado"] = "No";
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre");
            return View();
        }

        // POST: Fichas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_ficha,ID_servicio,codigoFicha,fecha,atendido,llamado,tipoFicha")] Ficha ficha)
        {
            if (ModelState.IsValid)
            {
                db.Fichas.Add(ficha);
                db.SaveChanges();
                FichasHub.BroadcastData();
                return RedirectToAction("Create");
            }

            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre", ficha.ID_servicio);
            return View(ficha);
        }

        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ficha ficha = db.Fichas.Find(id);
            if (ficha == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre", ficha.ID_servicio);
            return View(ficha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar([Bind(Include = "ID_ficha,ID_servicio,codigoFicha,fecha,atendido,llamado,tipoFicha")] Ficha ficha)
        {
            if (ModelState.IsValid)
            {

                db.Entry(ficha).State = EntityState.Modified;
                db.SaveChanges();


                FichasHub.BroadcastData();
                FichasHub.BroadcastDataFILA();

                return RedirectToAction("Index");
            };
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre", ficha.ID_servicio);

            return View(ficha);
        }



        public ActionResult Llamar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ficha ficha = db.Fichas.Find(id);
            if (ficha == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre", ficha.ID_servicio);
            return View(ficha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Llamar([Bind(Include = "ID_ficha,ID_servicio,codigoFicha,fecha,atendido,llamado,tipoFicha")] Ficha ficha)
        {
            if (ModelState.IsValid)
            {

                db.Entry(ficha).State = EntityState.Modified;
                db.SaveChanges();
                string nombreServicio = (from s in db.Servicios
                                         where s.ID_servicio == ficha.ID_servicio
                                         select s.nombre).FirstOrDefault();
                TTS("Ficha: " + ficha.codigoFicha + ". Caja: " + nombreServicio);

                FichasHub.BroadcastData();
                FichasHub.BroadcastDataFILA();
                FichasHub.BroadcastDataSonido();

                return RedirectToAction("Index");
            };
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre", ficha.ID_servicio);

            return View(ficha);
        }



        private string subscriptionKey;
        private string tokenFetchUri;
        public FichasController(string tokenFetchUri, string subscriptionKey)
        {
            if (string.IsNullOrWhiteSpace(tokenFetchUri))
            {
                throw new ArgumentNullException(nameof(tokenFetchUri));
            }
            if (string.IsNullOrWhiteSpace(subscriptionKey))
            {
                throw new ArgumentNullException(nameof(subscriptionKey));
            }
            this.tokenFetchUri = tokenFetchUri;
            this.subscriptionKey = subscriptionKey;
        }

        public async Task<string> FetchTokenAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.subscriptionKey);
                UriBuilder uriBuilder = new UriBuilder(this.tokenFetchUri);
                HttpResponseMessage result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null).ConfigureAwait(false);
                return await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }


        public async Task TTS(string text)
        {
            // Prompts the user to input text for TTS conversion
            Console.Write("What would you like to convert to speech? ");
            //string text = "hello trump";

            // Gets an access token
            string accessToken;
            Console.WriteLine("Attempting token exchange. Please wait...\n");

            // Add your subscription key here
            // If your resource isn't in WEST US, change the endpoint
            FichasController auth = new FichasController("https://westus.api.cognitive.microsoft.com/sts/v1.0/issueToken", "dcce377c0a8a440f807423636ed9f506");
            try
            {

                accessToken = await auth.FetchTokenAsync().ConfigureAwait(false);
                Console.WriteLine("Successfully obtained an access token. \n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to obtain an access token.");
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
                return;
            }

            string host = "https://westus.tts.speech.microsoft.com/cognitiveservices/v1";

            // Create SSML document.
            XDocument body = new XDocument(
                    new XElement("speak",
                        new XAttribute("version", "1.0"),
                        new XAttribute(XNamespace.Xml + "lang", "es-MX"),
                        new XElement("voice",
                            new XAttribute(XNamespace.Xml + "lang", "es-MX"),
                            new XAttribute(XNamespace.Xml + "gender", "Female"),
                            new XAttribute("name", "es-MX-HildaRUS"), // Short name for "Microsoft Server Speech Text to Speech Voice (en-US, Jessa24KRUS)"
                            text)));

            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    // Set the HTTP method
                    request.Method = HttpMethod.Post;
                    // Construct the URI
                    request.RequestUri = new Uri(host);
                    // Set the content type header
                    request.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/ssml+xml");
                    // Set additional header, such as Authorization and User-Agent
                    request.Headers.Add("Authorization", "Bearer " + accessToken);
                    request.Headers.Add("Connection", "Keep-Alive");
                    // Update your resource name
                    request.Headers.Add("User-Agent", "ateca");
                    // Audio output format. See API reference for full list.
                    request.Headers.Add("X-Microsoft-OutputFormat", "riff-24khz-16bit-mono-pcm");
                    // Create a request
                    Console.WriteLine("Calling the TTS service. Please wait... \n");
                    using (HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        // Asynchronously read the response
                        using (Stream dataStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            Console.WriteLine("Your speech file is being written to file...");
                            using (FileStream fileStream = new FileStream(Server.MapPath("~/Sonidos/fileName.wav"), FileMode.Create, FileAccess.Write, FileShare.Write))
                            {
                                await dataStream.CopyToAsync(fileStream).ConfigureAwait(false);
                                fileStream.Close();
                            }
                            Console.WriteLine("\nYour file is ready. Press any key to exit.");
                            Console.ReadLine();
                        }
                    }
                }
            }
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
