using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVideojuegos.Models;

namespace TiendaVideojuegos.Controllers
{
    public class GeneroController : Controller
    {
        TiendaVideojuegosDB db = new TiendaVideojuegosDB();
        // GET: Genero
        public ActionResult Index()
        {
            var generos = db.Genero.ToList();

            return View(generos);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MensajeInicio = "Ingrese datos del genero";
            return View();
        }

        [HttpPost]
        public ActionResult Create(Genero genero)
        {
            ViewBag.MensajeInicio = "Ingrese datos del genero";

            try
            {
                Validar(genero);
                db.Genero.Add(genero);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                ViewBag.Message = e.Message;
            }
            return View("Create");
        }

        public void Validar(Genero genero)
        {
            string errores = "";
            if (string.IsNullOrEmpty(genero.NombreGenero))
                errores += "Se debe asignar el nombre del genero";

            if (!string.IsNullOrEmpty(errores))
                throw new Exception(errores);
        }

        public ActionResult Update(byte id)
        {
            var genero = (from g in db.Genero.ToList()
                          where g.IdGenero == id
                          select g).SingleOrDefault();
            ViewBag.MensajeInicio = "Ingrese datos del género";
            return View(genero);
        }
        [HttpPost]
        public ActionResult Update(Genero genero)
        {
            ViewBag.MensajeInicio = "Ingrese datos del género";
            var otroGenero = (from g in db.Genero.ToList()
                             where g.IdGenero == genero.IdGenero
                             select g).SingleOrDefault();
            try
            {
                Validar(otroGenero);
                otroGenero.NombreGenero = genero.NombreGenero;

                db.SaveChanges();
                ViewBag.MensajeExito = "Datos del género [" + genero.IdGenero + "] fueron actualizados";
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }
            return View();
        }


        [HttpGet]
        public ActionResult Delete(byte id)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar género";
            var genero = (from g in db.Genero.ToList()
                          where g.IdGenero == id
                          select g).SingleOrDefault();

            return View(genero);
        }

        [HttpPost]
        public ActionResult Delete(Genero genero)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar género";
            try
            {
                var otroGenero = (from g in db.Genero.ToList()
                                 where g.IdGenero == genero.IdGenero
                                  select g).SingleOrDefault();
                ValidarEliminar(otroGenero);
                db.Genero.Remove(otroGenero);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }

            Genero temp = new Genero();
            return View(temp);
        }

        public void ValidarEliminar(Genero genero)
        {
            string errores = "";
            if (genero == null)
                throw new Exception("No existe el género");
            if (genero.Juego != null && genero.Juego.Count > 0)
                errores += "El género no se puede eliminar debido a que está asociado a uno o más juegos";

            if (!string.IsNullOrEmpty(errores))
            {
                throw new Exception(errores);
            }

        }

        public ActionResult Find(byte id)
        {
            var juegos = (from J in db.Juego.ToList()
                          where J.IdGenero == id
                          select J).ToList();

            return View(juegos);
        }
    }
}