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
    public class HistorialAtendidosController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();

        // GET: HistorialAtendidos
        public ActionResult Index()
        {
            var historialAtendidos = db.HistorialAtendidos.Include(h => h.Ficha).Include(h => h.Servicio).Include(h => h.Usuario);
            return View(historialAtendidos.ToList());
        }


        // GET: HistorialAtendidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialAtendido historialAtendido = db.HistorialAtendidos.Find(id);
            if (historialAtendido == null)
            {
                return HttpNotFound();
            }
            return View(historialAtendido);
        }

        // GET: HistorialAtendidos/Create
        public ActionResult Create()
        {
            ViewData["horaInicio"] = DateTime.Now;
            ViewData["horaFin"] = DateTime.Now;
            ViewData["fecha"] = DateTime.Now;
            ViewData["duracion"] = 0;
            ViewBag.ID_ficha = new SelectList(db.Fichas, "ID_ficha", "codigoFicha");
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre");
            ViewBag.ID_usuario = new SelectList(db.Usuarios, "ID_usuario", "cedula");
            FichasHub.BroadcastDataFILA();
            FichasHub.BroadcastData();
            return View();
        }

        // POST: HistorialAtendidos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_historialAtendido,ID_servicio,ID_ficha,horaInicio,horaFin,duracion,fecha,ID_usuario")] HistorialAtendido historialAtendido)
        {

            if (ModelState.IsValid)
            {
                db.HistorialAtendidos.Add(historialAtendido);
                db.SaveChanges();
                FichasHub.BroadcastDataFILA();
                FichasHub.BroadcastData();
                return RedirectToAction("Index");
            }

            ViewBag.ID_ficha = new SelectList(db.Fichas, "ID_ficha", "codigoFicha", historialAtendido.ID_ficha);
            ViewBag.ID_servicio = new SelectList(db.Servicios, "ID_servicio", "nombre", historialAtendido.ID_servicio);
            ViewBag.ID_usuario = new SelectList(db.Usuarios, "ID_usuario", "cedula", historialAtendido.ID_usuario);
            FichasHub.BroadcastDataFILA();
            FichasHub.BroadcastData();
            return View(historialAtendido);
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
