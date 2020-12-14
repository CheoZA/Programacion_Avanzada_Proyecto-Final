using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVideojuegos.Models;

namespace TiendaVideojuegos.Controllers
{
    public class ClienteController : Controller
    {
        TiendaVideojuegosDB db = new TiendaVideojuegosDB();
        // GET: Cliente
        public ActionResult Index()
        {
            var clientes = db.Cliente.ToList();
            return View(clientes);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MensajeInicio = "Ingrese datos del dliente";
            return View();
        }

        [HttpPost]
        public ActionResult Create(Cliente cliente)
        {
            ViewBag.MensajeInicio = "Ingrese datos del dliente";
            try
            {
                Validar(cliente);
                db.Cliente.Add(cliente);
                db.SaveChanges();
                ViewBag.MensajeExito = "Cliente [" + cliente.Nombre + " " + cliente.Apellido + "] fue registrado en el sistema";
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }
            ModelState.Clear();
            return View("Create");
        }
        
        public void Validar(Cliente cliente)
        {
            string errores = "";
            if (string.IsNullOrEmpty(cliente.Nombre))
                errores += "Se debe asignar el nombre del cliente\n";
            if (string.IsNullOrEmpty(cliente.Apellido))
                errores += "Se debe asignar el apellido del cliente\n";


            if (!string.IsNullOrEmpty(errores))
                throw new Exception(errores);
        }


        [HttpGet]
        public ActionResult Update(int id)
        {
            var cliente = (from c in db.Cliente.ToList()
                                 where c.IdCliente == id
                                 select c).SingleOrDefault();
            ViewBag.MensajeInicio = "Ingrese datos del cliente";
            return View(cliente);
        }
        [HttpPost]
        public ActionResult Update(Cliente cliente)
        {
            ViewBag.MensajeInicio = "Ingrese datos del desarrollador";
            var otroCliente = (from c in db.Cliente.ToList()
                                     where c.IdCliente == cliente.IdCliente
                                     select c).SingleOrDefault();
            try
            {
                Validar(otroCliente);
                otroCliente.Nombre = cliente.Nombre;
                otroCliente.Apellido = cliente.Apellido;
                otroCliente.TipoCliente = cliente.TipoCliente;
                otroCliente.Telefono = cliente.Telefono;
                otroCliente.Direccion = cliente.Direccion;

                db.SaveChanges();
                ViewBag.MensajeExito = "Datos del cliente [" + cliente.Nombre + " " + cliente.Apellido  + "] fueron actualizados";
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
            ViewBag.MensajeInicialEliminar = "Eliminar cliente";
            var cliente = (from c in db.Cliente.ToList()
                                 where c.IdCliente == id
                                 select c).SingleOrDefault();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar cliente";
            var cliente = (from c in db.Cliente.ToList()
                           where c.IdCliente == id
                           select c).SingleOrDefault();
            try
            {
                ValidarEliminar(cliente);
                db.Cliente.Remove(cliente);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }
            return Redirect("~/Cliente/Index/");
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar cliente";
            var cliente = (from c in db.Cliente.ToList()
                           where c.IdCliente == id
                           select c).SingleOrDefault();

            return View(cliente);
        }

        [HttpPost]
        public ActionResult Eliminar(Cliente cliente)
        {
            ViewBag.MensajeInicialEliminar = "Eliminar cliente";
            try
            {
                var otroCliente = (from c in db.Cliente.ToList()
                               where c.IdCliente == cliente.IdCliente
                               select c).SingleOrDefault();
                ValidarEliminar(otroCliente);
                db.Cliente.Remove(otroCliente);
                db.SaveChanges();
                ViewBag.MensajeExito = "El cliente se ha eliminado correctamente";
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = e.Message;
            }

            Cliente temp = new Cliente();
            return View(temp);
        }

        public void ValidarEliminar(Cliente cliente)
        {
            string errores = "";
            if (cliente == null)
                throw new Exception("Cliente inexistente");
            if(cliente.Factura != null && cliente.Factura.Count > 0)
                errores += "El cliente no se puede eliminar debido a que está asociado a una o más facturas";


            if (!string.IsNullOrEmpty(errores))
                throw new Exception(errores);
        }


        public ActionResult Find(int id)
        {
            var cliente = (from c in db.Cliente.ToList()
                           where c.IdCliente == id
                           select c).SingleOrDefault();

            return View(cliente);
        }

    }
}