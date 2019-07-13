using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            FichasHub.BroadcastData();
            return View(
                db.Fichas.Where(f => f.atendido == "No"));
        }

        public ActionResult GetFichasData()
        {
            return PartialView("_FichasData", db.Fichas.Where(f => f.atendido == "No").ToList());
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
            FichasHub.BroadcastData();
            return View(ficha);
        }

        // GET: Fichas/Create
        public ActionResult Create()
        {
            ViewData["codigoFicha"] = DateTime.Now.Hour+"-"+DateTime.Now.Minute+"-"+DateTime.Now.Second;
            ViewData["fecha"] = DateTime.Now;
            ViewData["atendido"] = "No";
            ViewData["llamado"] = "No";
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre");
            FichasHub.BroadcastData();
            return View();
        }

        // POST: Fichas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_ficha,ID_servicio,codigoFicha,fecha,atendido,llamado")] Ficha ficha)
        {
            if (ModelState.IsValid)
            {
                db.Fichas.Add(ficha);
                db.SaveChanges();
                FichasHub.BroadcastData();
                return RedirectToAction("Create");
            }

            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre", ficha.ID_servicio);
            FichasHub.BroadcastData();
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
            FichasHub.BroadcastData();
            return View(ficha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Llamar([Bind(Include = "ID_ficha,ID_servicio,codigoFicha,fecha,atendido,llamado")] Ficha ficha)
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
