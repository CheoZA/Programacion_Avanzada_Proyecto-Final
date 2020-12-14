using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVideojuegos.Models;

namespace TiendaVideojuegos.Controllers
{
    public class VendedorController : Controller
    {
        TiendaVideojuegosDB db = new TiendaVideojuegosDB();
        // GET: Vendedor
        public ActionResult Index()
        {
            var vendedores = db.Vendedor.ToList();
            return View(vendedores);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MensajeInicio = "Ingrese datos Del vendedor";
            return View();
        }

        [HttpPost]
        public ActionResult Create(Vendedor vendedor)
        {
            ViewBag.MensajeInicio = "Ingrese datos del vendedor";
            try
            {
                Validar(vendedor);
                db.Vendedor.Add(vendedor);
                db.SaveChanges();
                ViewBag.MensajeExito = "Vendedor [" + vendedor.Nombre + "] fue registrado en el sistema";
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            return View("Create");
        }

        public void Validar(Vendedor vendedor)
        {
            string errores = "";
            if (string.IsNullOrEmpty(vendedor.Nombre))
                errores += "Se debe asignar el nombre del vendedor\n";
            if (string.IsNullOrEmpty(vendedor.ApellidoPaterno))
                errores += "Se debe asignar el apellido paterno del vendedor\n";
            if (string.IsNullOrEmpty(vendedor.ApellidoMaterno))
                errores += "Se debe asignar el apellido materno del vendedor\n";
            if (string.IsNullOrEmpty(vendedor.Telefono))
                errores += "Se debe asignar el teléfono del vendedor\n";


            if (!string.IsNullOrEmpty(errores))
                throw new Exception(errores);
        }

        [HttpGet]
        public ActionResult Update(string id)
        {
            ViewBag.MensajeInicio = "Ingrese datos del vendedor";
            var vendedor = (from v in db.Vendedor.ToList()
                           where v.IdVendedor == id
                           select v).SingleOrDefault();
            return View(vendedor);
        }
        [HttpPost]
        public ActionResult Update(Vendedor vendedor)
        {
            ViewBag.MensajeInicio = "Ingrese datos del vendedor";
            var otroVendedor = (from v in db.Vendedor.ToList()
                               where v.IdVendedor == vendedor.IdVendedor
                               select v).SingleOrDefault();
            try
            {
                Validar(otroVendedor);
                otroVendedor.IdVendedor = vendedor.IdVendedor;
                otroVendedor.Nombre = vendedor.Nombre;
                otroVendedor.ApellidoPaterno = vendedor.ApellidoPaterno;
                otroVendedor.ApellidoMaterno = vendedor.ApellidoMaterno;
                otroVendedor.Telefono = vendedor.Telefono;

                db.SaveChanges();
                ViewBag.MensajeExito = "Datos del vendedor [" + vendedor.Nombre + " " + vendedor.ApellidoPaterno + " " + vendedor.ApellidoMaterno + "] fueron actualizados";
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }
            return View();
        }


        [HttpGet]
        public ActionResult Delete(string id)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar vendedor";
            var vendedor = (from v in db.Vendedor.ToList()
                           where v.IdVendedor == id
                           select v).SingleOrDefault();
            return View(vendedor);
        }


        [HttpPost]
        public ActionResult Delete(Vendedor vendedor)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar vendedor";
            try
            {
                var otroVendedor = (from v in db.Vendedor.ToList()
                                   where v.IdVendedor == vendedor.IdVendedor
                                   select v).SingleOrDefault();
                ValidarEliminar(otroVendedor);
                db.Vendedor.Remove(otroVendedor);
                db.SaveChanges();
                ViewBag.MensajeExito = "El cliente se ha eliminado correctamente";
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }
            Vendedor temp = new Vendedor();
            return View(temp);
        }

        public void ValidarEliminar(Vendedor vendedor)
        {
            if (vendedor == null)
                throw new Exception("Vendedor inexistente");
        }

        public ActionResult Find(string id)
        {
            var vendedor = (from v in db.Vendedor.ToList()
                           where v.IdVendedor == id
                           select v).SingleOrDefault();
            return View(vendedor);
        }
    }
}