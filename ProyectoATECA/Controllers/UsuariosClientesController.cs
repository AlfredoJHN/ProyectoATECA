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
    public class UsuariosClientesController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();

        // GET: UsuariosClientes
        public ActionResult Index()
        {
            return View(db.UsuariosClientes.ToList());
        }

        // GET: UsuariosClientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosCliente usuariosCliente = db.UsuariosClientes.Find(id);
            if (usuariosCliente == null)
            {
                return HttpNotFound();
            }
            return View(usuariosCliente);
        }

        // GET: UsuariosClientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuariosClientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_usuarioCliente,nombre,cedula,apellidos,fechaNacimiento,correo,contraseña,asegurado")] UsuariosCliente usuariosCliente)
        {
            if (ModelState.IsValid)
            {
                db.UsuariosClientes.Add(usuariosCliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuariosCliente);
        }

        // GET: UsuariosClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosCliente usuariosCliente = db.UsuariosClientes.Find(id);
            if (usuariosCliente == null)
            {
                return HttpNotFound();
            }
            return View(usuariosCliente);
        }

        // POST: UsuariosClientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_usuarioCliente,nombre,cedula,apellidos,fechaNacimiento,correo,contraseña,asegurado")] UsuariosCliente usuariosCliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuariosCliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuariosCliente);
        }

        // GET: UsuariosClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosCliente usuariosCliente = db.UsuariosClientes.Find(id);
            if (usuariosCliente == null)
            {
                return HttpNotFound();
            }
            return View(usuariosCliente);
        }

        // POST: UsuariosClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UsuariosCliente usuariosCliente = db.UsuariosClientes.Find(id);
            db.UsuariosClientes.Remove(usuariosCliente);
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
