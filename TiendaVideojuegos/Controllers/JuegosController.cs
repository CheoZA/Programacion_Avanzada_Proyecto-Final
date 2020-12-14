using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TiendaVideojuegos.Models;
using LinqKit;

namespace TiendaVideojuegos.Controllers
{
    public class JuegosController : Controller
    {
        TiendaVideojuegosDB db = new TiendaVideojuegosDB();

        public void GenerarListas()
        {
            List<Desarrollador> desarroladores = db.Desarrollador.ToList();
            SelectList listaDes = new SelectList(desarroladores, "IdDesarrollador", "NombreDesarrollador");
            ViewBag.ListaDesarrolladores = listaDes;
            List<Genero> generos = db.Genero.ToList();
            SelectList listaGen = new SelectList(generos, "IdGenero", "NombreGenero");
            ViewBag.ListaGeneros = listaGen;
        }

        public ActionResult Index()
        {
            GenerarListas();
            var juegos = from juego in db.Juego.ToList()
                                 orderby juego.NombreJuego
                                 select juego;
            return View(juegos.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            GenerarListas();
            ViewBag.MensajeExito = "Ingrese datos del juego";
            return View();
        }
        [HttpPost]
        public ActionResult Create(Juego juego)
        {
            ViewBag.MensajeExito = "Ingrese datos del juego";
            GenerarListas();

            try
            {
                Validar(juego);
                db.Juego.Add(juego);
                db.SaveChanges();
                ViewBag.MensajeExito = "Juego [" + juego.IdJuego + "] fue registrado en el sistema con éxito";
            }
            catch(Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }
            return View("Create");
        }

        public void Validar(Juego juego)
        {
            string errores = "";
            if (juego == null)
                throw new Exception("Error inesperado");
            if (string.IsNullOrEmpty(juego.NombreJuego))
                errores += "Se debe asignar un nombre al juego\n";
            if (juego.IdGenero == 0 || juego.IdGenero == null)
                errores += "Se debe asignar un genero\n";
            if (juego.IdDesarrollador == 0 || juego.IdDesarrollador == null)
                errores += "Se debe asignar un desarrollador\n";
            if (string.IsNullOrEmpty(juego.Clasificacion))
                errores += "Se debe asignar la clasificación\n";
            if (juego.Precio == 0 || juego.Precio == null)
                errores += "Se debe asignar el precio\n";

            if (!string.IsNullOrEmpty(errores))
            {
                throw new Exception(errores);
            }
        }

        public ActionResult Update(int id)
        {
            GenerarListas();
            var juego = (from J in db.Juego.ToList()
                        where J.IdJuego == id
                        select J).SingleOrDefault();

            ViewBag.MensajeInicio = "Ingrese datos del juego";
            return View(juego);
        }
        [HttpPost]
        public ActionResult Update(Juego juego)
        {
            GenerarListas();
            ViewBag.MensajeInicio = "Ingrese datos del juego";
            var otroJuego = (from J in db.Juego.ToList()
                         where J.IdJuego == juego.IdJuego
                         select J).SingleOrDefault();

            try
            {
                Validar(juego);
                otroJuego.NombreJuego = juego.NombreJuego;
                otroJuego.IdGenero = juego.IdGenero;
                otroJuego.IdDesarrollador = juego.IdDesarrollador;
                otroJuego.Clasificacion = juego.Clasificacion;
                otroJuego.Precio = juego.Precio;
                otroJuego.CantidadEnStock = juego.CantidadEnStock;
                otroJuego.Genero = juego.Genero;
                otroJuego.Desarrollador = juego.Desarrollador;

                db.SaveChanges();
                ViewBag.MensajeExito = "Datos del juego [" + juego.IdJuego + "] fueron actualizados";
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }

            return View();
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var juego = (from J in db.Juego.ToList()
                         where J.IdJuego == id
                         select J).SingleOrDefault();
            ViewBag.MensajeInicialEliminar = "Eliminar juego";
            return View(juego);
        }

        [HttpPost]
        public ActionResult Delete(Juego juego)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar juego";

            try
            {
                var otroJuego = (from J in db.Juego.ToList()
                                 where J.IdJuego == juego.IdJuego
                                 select J).SingleOrDefault();
                ValidarEliminar(otroJuego);
                db.Juego.Remove(otroJuego);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }
            
            Juego temp = new Juego();
            return View(temp);
            
        }

        public void ValidarEliminar(Juego juego)
        {
            string errores = "";
            if (juego == null)
                throw new Exception("No existe el juego");
            if (juego.FacturaDetalle != null)
                errores += "El juego no se puede eliminar debido a que está asociado a una o más facturas";

            if (!string.IsNullOrEmpty(errores))
            {
                throw new Exception(errores);
            }
        }

        [HttpGet]
        public ActionResult BuscarJuegos()
        {
            GenerarListas();

            List<Juego> juegos = db.Juego.ToList();
            return View(juegos);
        }
        [HttpPost]
        public ActionResult BuscarJuegos(string txtNombre, string genero, string desarrollador)
        {
            GenerarListas();
            var predicado = PredicateBuilder.New<Juego>();

            if (!string.IsNullOrEmpty(txtNombre))
                predicado = predicado.And(J => J.NombreJuego.Contains(txtNombre));
            if (!string.IsNullOrEmpty(genero))
                predicado = predicado.And(J => J.Genero.NombreGenero.Equals(genero));
            if (!string.IsNullOrEmpty(desarrollador))
                predicado = predicado.And(J => J.Desarrollador.NombreDesarrollador.Equals(desarrollador));

            try
            {
                var juegos = db.Juego.Where(predicado).ToList();
                return View(juegos);
            }
            catch(Exception e)
            {
                ViewBag.MensajeError = "Error al buscar el juego";
            }

            
            return View(db.Juego.ToList());
        }

    }
}