using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProyectoATECA.Hubs;
using ProyectoATECA.Models;




namespace ProyectoATECA.Controllers
{
    public class FichasController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();

        // GET: Fichas
        public ActionResult Index()
        {
            var fichas = db.Fichas.Include(f => f.Servicio);
            return View(
                db.Fichas.Where(f => f.atendido == "No" && f.fecha.DayOfYear == DateTime.Now.DayOfYear).OrderBy(f=>f.fecha));
        }

        public ActionResult GetFichasData()
        {
            int id_rol = Convert.ToInt32(System.Web.HttpContext.Current.Session["ROL"]);
            if(id_rol == 3) { 
                return PartialView("_FichasData", db.Fichas.Where(f => f.atendido == "No" && f.tipoFicha =="Ley 7600" &&
                f.fecha.Day == DateTime.Now.Day &&
                f.fecha.Month == DateTime.Now.Month &&
                f.fecha.Year == DateTime.Now.Year
                ).ToList().OrderBy(f => f.fecha));
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
            ViewData["codigoFicha"] = DateTime.Now.Hour+"-"+DateTime.Now.Minute+"-"+DateTime.Now.Second;
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
        public  ActionResult Llamar([Bind(Include = "ID_ficha,ID_servicio,codigoFicha,fecha,atendido,llamado,tipoFicha")] Ficha ficha)
        {
            if (ModelState.IsValid)
            {

                db.Entry(ficha).State = EntityState.Modified;
                db.SaveChanges();
                string nombreServicio = (from s in db.Servicios
                                        where s.ID_servicio == ficha.ID_servicio
                                        select s.nombre).FirstOrDefault();
                TTS("Ficha: "+ficha.codigoFicha+". Pase a caja: "+nombreServicio);

                FichasHub.BroadcastData();
                FichasHub.BroadcastDataFILA();
                FichasHub.BroadcastDataSonido();
                
                return RedirectToAction("Index");
            };
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre", ficha.ID_servicio);

            return View(ficha);
        }


        [HttpPost]
        public async Task<ActionResult> TTS(string text)
        {
            // you can set output file name as method argument or generated from text
            string fileName = "fileName";
            Task<ViewResult> task = Task.Run(() =>
            {
                using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
                {
                    speechSynthesizer.SetOutputToWaveFile(Server.MapPath("~/Sonidos/") + fileName + ".mp3");
                    speechSynthesizer.Speak(text);

                    ViewBag.FileName = fileName + ".mp3";
                    FichasHub.BroadcastDataSonido();
                    return View();
                }
            });
            return await task;
        }


        //string fileName = "fileName";
        //Task<ViewResult> task = Task.Run(() =>
        //{
        //    using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
        //    {
        //        speechSynthesizer.SetOutputToWaveFile(Server.MapPath("~/Sonidos/") + fileName + ".wav");
        //        speechSynthesizer.Speak("Ficha: " + ficha.codigoFicha.ToString());

        //        ViewBag.FileName = fileName + ".wav";

        //        return View();
        //    }

        //});


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
