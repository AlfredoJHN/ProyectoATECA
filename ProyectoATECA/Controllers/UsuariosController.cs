using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ProyectoATECA.Models;

namespace ProyectoATECA.Controllers
{
    public class UsuariosController : Controller
    {
        private ATECA_BDEntities db = new ATECA_BDEntities();





        // GET: Usuarios
        //public ActionResult Index()
        //{
        //    var usuarios = db.Usuarios.Include(u => u.Role);
        //    return View(usuarios.ToList());
        //}

        //// GET: Usuarios/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Usuario usuario = db.Usuarios.Find(id);
        //    if (usuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(usuario);
        //}

        //// GET: Usuarios/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ID_rol = new SelectList(db.Roles, "ID_rol", "nombre");
        //    return View();
        //}

        //// POST: Usuarios/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID_usuario,nombre,cedula,apellidos,fechaNacimiento,correo,contraseña,ID_rol,estado")] Usuario usuario)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Usuarios.Add(usuario);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ID_rol = new SelectList(db.Roles, "ID_rol", "nombre", usuario.ID_rol);
        //    return View(usuario);
        //}

        //// GET: Usuarios/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Usuario usuario = db.Usuarios.Find(id);
        //    if (usuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ID_rol = new SelectList(db.Roles, "ID_rol", "nombre", usuario.ID_rol);
        //    return View(usuario);
        //}

        //// POST: Usuarios/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID_usuario,nombre,cedula,apellidos,fechaNacimiento,correo,contraseña,ID_rol,estado")] Usuario usuario)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(usuario).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ID_rol = new SelectList(db.Roles, "ID_rol", "nombre", usuario.ID_rol);
        //    return View(usuario);
        //}

        //// GET: Usuarios/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Usuario usuario = db.Usuarios.Find(id);
        //    if (usuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(usuario);
        //}

        //// POST: Usuarios/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Usuario usuario = db.Usuarios.Find(id);
        //    db.Usuarios.Remove(usuario);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [HttpGet]

       public ActionResult Registration()
        {
            ViewBag.ID_rol = new SelectList(db.Roles, "ID_rol", "nombre");
            return View();
        }
        //Registration POST action 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "correoVerificado,codigoActivacion")] Usuario usuario)
        {
            ViewBag.ID_rol = new SelectList(db.Roles, "ID_rol", "nombre");
            bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {

                #region //Email already Exist 
                var correoExist = IsEmailExist(usuario.correo);
                var cedulaExist = CedulaExist(usuario.cedula);
                if (correoExist )
                {
                    ModelState.AddModelError("EmailExist", "El correo ya esta registrado");
                    return View(usuario);
                }

                if (cedulaExist)
                {
                    ModelState.AddModelError("CedulaExist", "Esta cédula ya se encuentra registrada");
                    return View(usuario);
                }
                #endregion

                #region Generate Activation Code 
                usuario.codigoActivacion = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                usuario.contraseña = Crypto.Hash(usuario.contraseña);
                usuario.confirmarcontrasena = Crypto.Hash(usuario.confirmarcontrasena); //
                #endregion
                usuario.correoVerificado = false;

                #region Save to Database
                using (ATECA_BDEntities db = new ATECA_BDEntities())
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();

                    //Send Email to User
                    SendVerificationLinkEmail(usuario.correo, usuario.codigoActivacion.ToString());
                    message = " Se ha registrado exitosamente. Se le ha enviado un mensaje de correo para verificar la activación de cuenta " +
                        " a: " + usuario.correo;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(usuario);
        }
        //Verify Account  

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (ATECA_BDEntities db = new ATECA_BDEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = db.Usuarios.Where(a => a.codigoActivacion == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.correoVerificado = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        //Login 
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUsuario login, string ReturnUrl = "")
        {
            string message = "";
            using (ATECA_BDEntities db = new ATECA_BDEntities())
            {
                var v = db.Usuarios.Where(a => a.correo == login.correo).FirstOrDefault();
                if (v != null)
                {
                    if (!v.correoVerificado)
                    {
                        ViewBag.Message = "Verifique su email";
                        return View();
                    }

                    if (string.Compare(Crypto.Hash(login.contraseña), v.contraseña) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.correo, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Acceso denegado";
                    }
                }
                else
                {
                    message = "Acceso denegado";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }


        [NonAction]
        public bool IsEmailExist(string correo)
        {
            using (ATECA_BDEntities db = new ATECA_BDEntities())
            {
                var v = db.Usuarios.Where(a => a.correo == correo).FirstOrDefault();
                return v != null;
            }
        }


        [NonAction]
        public bool CedulaExist(string cedula)
        {
            using (ATECA_BDEntities db = new ATECA_BDEntities())
            {
                var v = db.Usuarios.Where(a => a.cedula == cedula).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/Usuarios/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("pruebaatecadb@gmail.com", "TEST");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "ATECADB123"; // Replace with actual password
            string subject = "Su cuenta ha sido creada exitosamente";

            string body = "<br/><br/>Su cuenta en ATECA ha sido creada satisfactoriamente. Por favor acceda al siguiente enlace para " +
                "activar su cuenta. Le recordamos que dicha cuenta la puede utilizar para acceder a la app móvil y agendar su " +
                "próxima cita con anticipación." +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
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
