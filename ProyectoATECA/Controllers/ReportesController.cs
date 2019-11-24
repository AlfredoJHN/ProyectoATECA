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
    public class ReportesController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CantidadAtendidos()
        {
            return View();
        }

        public ActionResult DuracionPromedio()
        {
            return View();
        }

        public ActionResult CantidadAtendidosMes()
        {
            return View();
        }

        public ActionResult GetCantidadAtendidos()
        {

            var query = db.HistorialAtendidos.Include("Servicios")
                    .GroupBy(s => new { s.Servicio.ID_servicio, s.Servicio.nombre })
                   .Select(g => new { name = g.Key.nombre, count = db.HistorialAtendidos.Where(x => x.ID_servicio == g.Key.ID_servicio).Count() }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetDuracionPromedio()
        {

            var query = db.HistorialAtendidos.Include("Servicios")
                    .GroupBy(s => new { s.Servicio.ID_servicio, s.Servicio.nombre })
                   .Select(g => new { name = g.Key.nombre, prom = g.Average(s => s.duracion) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetAtendidosMes(int? mes, int? year)
        {
            var query = db.HistorialAtendidos
                .GroupBy(s => new { mes })
                   .Select(g => new {
                       name = mes,
                       count = db.HistorialAtendidos.Where(x => x.fecha.Month == mes
&& x.fecha.Year == year).Count()
                   }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CantidadNoAtendidosMes()
        {
            return View();
        }

        public ActionResult GetNoAtendidosMes(int? mes, int? year)
        {
            var query = db.HistorialAtendidos
                .GroupBy(s => new { mes })
                   .Select(g => new {
                       name = mes,
                       count = db.HistorialAtendidos.Where(x => x.fecha.Month == mes
                        && x.fecha.Year == year).Count()
                   }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
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
