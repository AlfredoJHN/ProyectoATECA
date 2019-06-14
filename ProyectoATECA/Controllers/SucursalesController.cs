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
    public class SucursalesController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();

        // GET: Sucursales
        public ActionResult Index()
        {
            return View(db.Sucursales.ToList());
        }

        // GET: Sucursales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sucursale sucursale = db.Sucursales.Find(id);
            if (sucursale == null)
            {
                return HttpNotFound();
            }
            return View(sucursale);
        }

        // GET: Sucursales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sucursales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_sucursal,nombre,direccion,canton,distrito,provincia,telefono,correo")] Sucursale sucursale)
        {
            if (ModelState.IsValid)
            {
                db.Sucursales.Add(sucursale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sucursale);
        }

        // GET: Sucursales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sucursale sucursale = db.Sucursales.Find(id);
            if (sucursale == null)
            {
                return HttpNotFound();
            }
            return View(sucursale);
        }

        // POST: Sucursales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_sucursal,nombre,direccion,canton,distrito,provincia,telefono,correo")] Sucursale sucursale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sucursale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sucursale);
        }

        // GET: Sucursales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sucursale sucursale = db.Sucursales.Find(id);
            if (sucursale == null)
            {
                return HttpNotFound();
            }
            return View(sucursale);
        }

        // POST: Sucursales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sucursale sucursale = db.Sucursales.Find(id);
            db.Sucursales.Remove(sucursale);
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
