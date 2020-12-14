using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TiendaVideojuegos.Models;

namespace TiendaVideojuegos.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpPost]
        public ActionResult IniciarSesion(string nombreUsuario, string password)
        {
            if(!string.IsNullOrEmpty(nombreUsuario) && !string.IsNullOrEmpty(password))
            {
                TiendaVideojuegosDB bd = new TiendaVideojuegosDB();

                var usuario = bd.Usuario.FirstOrDefault(user => 
                user.NombreUsuario == nombreUsuario && user.Contraseña == password);

                if(usuario != null)
                {
                    FormsAuthentication.SetAuthCookie(nombreUsuario, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Login", new { message = "Nombre de usuario y/o contraseña incorrectos" });
                }

            }
            else
            {
                return RedirectToAction("Login", new { message = "Debes de llenar los campos para inicar sesión" });
            }
        }

        public ActionResult Login(string message = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}