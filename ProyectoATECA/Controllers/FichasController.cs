using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
            return View(fichas.ToList());
        }

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

        // GET: Fichas/Create
        public ActionResult Create()
        {
            ViewData["codigoFicha"] = DateTime.Now.Hour+"-"+DateTime.Now.Minute+"-"+DateTime.Now.Second;
            ViewData["fecha"] = DateTime.Now;
            ViewData["atendido"] = "No";
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre");
            return View();
        }

        // POST: Fichas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_ficha,ID_servicio,codigoFicha,fecha,atendido")] Ficha ficha)
        {
            if (ModelState.IsValid)
            {
                db.Fichas.Add(ficha);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

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
