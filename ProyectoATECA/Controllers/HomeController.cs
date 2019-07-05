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
    public class HomeController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();

        public ActionResult Index()
        {
            var fichas = db.Fichas.Include(f => f.Servicio);
            return View(
                db.Fichas.Where(f => f.llamado == "Si" && f.atendido == "No"));

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}