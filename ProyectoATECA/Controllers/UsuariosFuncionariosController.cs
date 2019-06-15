using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoATECA.Models;
//aaa
namespace ProyectoATECA.Controllers
{
    public class UsuariosFuncionariosController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();

        // GET: UsuariosFuncionarios
        public ActionResult Index()
        {
            return View(db.UsuariosFuncionarios.ToList());
        }

        // GET: UsuariosFuncionarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosFuncionario usuariosFuncionario = db.UsuariosFuncionarios.Find(id);
            if (usuariosFuncionario == null)
            {
                return HttpNotFound();
            }
            return View(usuariosFuncionario);
        }

        // GET: UsuariosFuncionarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuariosFuncionarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_usuarioFuncionario,nombre,cedula,fechaNacimiento,apellidos,correo,contraseña,cargo")] UsuariosFuncionario usuariosFuncionario)
        {
            if (ModelState.IsValid)
            {
                db.UsuariosFuncionarios.Add(usuariosFuncionario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuariosFuncionario);
        }

        // GET: UsuariosFuncionarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosFuncionario usuariosFuncionario = db.UsuariosFuncionarios.Find(id);
            if (usuariosFuncionario == null)
            {
                return HttpNotFound();
            }
            return View(usuariosFuncionario);
        }

        // POST: UsuariosFuncionarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_usuarioFuncionario,nombre,cedula,fechaNacimiento,apellidos,correo,contraseña,cargo")] UsuariosFuncionario usuariosFuncionario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuariosFuncionario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuariosFuncionario);
        }

        // GET: UsuariosFuncionarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosFuncionario usuariosFuncionario = db.UsuariosFuncionarios.Find(id);
            if (usuariosFuncionario == null)
            {
                return HttpNotFound();
            }
            return View(usuariosFuncionario);
        }

        // POST: UsuariosFuncionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UsuariosFuncionario usuariosFuncionario = db.UsuariosFuncionarios.Find(id);
            db.UsuariosFuncionarios.Remove(usuariosFuncionario);
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
