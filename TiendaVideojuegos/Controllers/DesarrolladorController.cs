using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVideojuegos.Models;

namespace TiendaVideojuegos.Controllers
{
    public class DesarrolladorController : Controller
    {
        TiendaVideojuegosDB db = new TiendaVideojuegosDB();
        // GET: Desarrollador
        public ActionResult Index()
        {
            var desarrolladores = db.Desarrollador.ToList();

            return View(desarrolladores);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MensajeInicio = "Ingrese datos del desarrollador";
            return View();
        }

        [HttpPost]
        public ActionResult Create(Desarrollador desarrollador)
        {
            ViewBag.MensajeInicio = "Ingrese datos del desarrollador";

            try
            {
                Validar(desarrollador);
                db.Desarrollador.Add(desarrollador);
                db.SaveChanges();
                ViewBag.MensajeExito = "Desarrollador [" + desarrollador.NombreDesarrollador + "] fue registrado en el ristema";
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            return View("Create");
        }

        public void Validar(Desarrollador desarrollador)
        {
            string errores = "";
            if (string.IsNullOrEmpty(desarrollador.NombreDesarrollador))
                errores += "Se debe asignar el nombre del desarrollador";

            if (!string.IsNullOrEmpty(errores))
                throw new Exception(errores);
        }

        public ActionResult Update(short id)
        {
            var desarrollador = (from d in db.Desarrollador.ToList()
                          where d.IdDesarrollador == id
                          select d).SingleOrDefault();
            ViewBag.MensajeInicio = "Ingrese datos del desarrollador";
            return View(desarrollador);
        }
        [HttpPost]
        public ActionResult Update(Desarrollador desarrollador)
        {
            ViewBag.MensajeInicio = "Ingrese datos del desarrollador";
            var otroDesarrollador = (from d in db.Desarrollador.ToList()
                                     where d.IdDesarrollador == desarrollador.IdDesarrollador
                                     select d).SingleOrDefault();
            try
            {
                Validar(otroDesarrollador);
                otroDesarrollador.NombreDesarrollador = desarrollador.NombreDesarrollador;

                db.SaveChanges();
                ViewBag.MensajeExito = "Datos del desarrollador [" + desarrollador.IdDesarrollador + "] fueron actualizados";
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }
            return View();
        }


        [HttpGet]
        public ActionResult Delete(short id)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar desarrollador";
            var desarrollador = (from d in db.Desarrollador.ToList()
                                 where d.IdDesarrollador == id
                                 select d).SingleOrDefault();

            return View(desarrollador);
        }

        [HttpPost]
        public ActionResult Delete(Desarrollador desarrollador)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar desarrollador";
            try
            {
                var otroDesarrollador = (from d in db.Desarrollador.ToList()
                                  where d.IdDesarrollador == desarrollador.IdDesarrollador
                                  select d).SingleOrDefault();
                ValidarEliminar(otroDesarrollador);
                db.Desarrollador.Remove(otroDesarrollador);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }

            Desarrollador temp = new Desarrollador();
            return View(temp);
        }

        public void ValidarEliminar(Desarrollador desarrollador)
        {
            string errores = "";
            if (desarrollador == null)
                throw new Exception("No existe el desarrollador");
            if (desarrollador.Juego != null && desarrollador.Juego.Count > 0)
                errores += "El desarrollador no se puede eliminar debido a que está asociado a uno o más juegos";

            if (!string.IsNullOrEmpty(errores))
            {
                throw new Exception(errores);
            }

        }

        public ActionResult Find(short id)
        {
            var juegos = (from J in db.Juego.ToList()
                          where J.IdDesarrollador == id
                          select J).ToList();

            return View(juegos);
        }
    }
}