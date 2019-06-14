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
    public class TiposReportesController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();

        // GET: TiposReportes
        public ActionResult Index()
        {
            return View(db.TiposReportes.ToList());
        }

        // GET: TiposReportes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposReporte tiposReporte = db.TiposReportes.Find(id);
            if (tiposReporte == null)
            {
                return HttpNotFound();
            }
            return View(tiposReporte);
        }

        // GET: TiposReportes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposReportes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_tipo,nombreTipo")] TiposReporte tiposReporte)
        {
            if (ModelState.IsValid)
            {
                db.TiposReportes.Add(tiposReporte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposReporte);
        }

        // GET: TiposReportes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposReporte tiposReporte = db.TiposReportes.Find(id);
            if (tiposReporte == null)
            {
                return HttpNotFound();
            }
            return View(tiposReporte);
        }

        // POST: TiposReportes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_tipo,nombreTipo")] TiposReporte tiposReporte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposReporte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposReporte);
        }

        // GET: TiposReportes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposReporte tiposReporte = db.TiposReportes.Find(id);
            if (tiposReporte == null)
            {
                return HttpNotFound();
            }
            return View(tiposReporte);
        }

        // POST: TiposReportes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposReporte tiposReporte = db.TiposReportes.Find(id);
            db.TiposReportes.Remove(tiposReporte);
            db.SaveChanges();
            return RedirectToAction("Index");
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
