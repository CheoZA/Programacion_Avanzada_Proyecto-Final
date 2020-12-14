using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVideojuegos.Models;

namespace TiendaVideojuegos.Controllers
{
    public class FacturaController : Controller
    {

        TiendaVideojuegosDB db = new TiendaVideojuegosDB();
        [HttpGet]
        public ActionResult ObtenerClientes()
        {
            var clientes = db.Cliente.ToList();
            return View(clientes);
        }

        [HttpPost]
        public ActionResult ObtenerClientes(string txtNombre, string txtApe)
        {

            var predicado = PredicateBuilder.New<Cliente>();

            if (!string.IsNullOrEmpty(txtNombre))
                predicado = predicado.And(C => C.Nombre.Contains(txtNombre));
            if (!string.IsNullOrEmpty(txtApe))
                predicado = predicado.And(C => C.Apellido.Contains(txtApe));


            try
            {
                var juegos = db.Cliente.Where(predicado).ToList();
                return View(juegos);
            }
            catch (Exception e)
            {
                ViewBag.MensajeError = "Error al buscar el cliente";
            }

            
            return View(db.Juego.ToList());
        }

        [HttpPost]
        public ActionResult Seleccionar(int idJuego)
        {
            var juego = (from J in db.Juego.ToList()
                         where J.IdJuego == idJuego
                         select J).SingleOrDefault();
            return Json(juego, JsonRequestBehavior.AllowGet);

        }

        public void cargarjuego()
        {
            List<Juego> data = db.Juego.ToList();
            SelectList lista = new SelectList(data, "idJuego", "NombreJuego");
            ViewBag.ListaJuego = lista;
        }


        public ActionResult NuevaVenta()
        {
            cargarjuego();
            return View();
        }
        [HttpPost]
        public ActionResult GuardarFactura(string Fecha, string importe,  string Total, string IdCliente, List<FacturaDetalle> detalles)
        {
            string mensaje = "";
            decimal iva = 16;
            int codigoCliente = 0;
            decimal imp = 0;
            decimal total = 0;
            DateTime fecha = new DateTime();

            if (string.IsNullOrEmpty(Fecha) || string.IsNullOrEmpty(importe) || string.IsNullOrEmpty(Total) || string.IsNullOrEmpty(IdCliente))
            {
                if (string.IsNullOrEmpty(Fecha)) mensaje = "ERROR EN EL CAMPO FECHA";
                if (string.IsNullOrEmpty(importe)) mensaje = "ERROR EN EL IMPORTE";
                if (string.IsNullOrEmpty(Total)) mensaje = "ERROR EN EL CAMPO TOTAL";
                if (string.IsNullOrEmpty(IdCliente)) mensaje = "ERROR CON EL CODIGO DEL CLIENTE";
                
            }
            else
            {
                fecha = Convert.ToDateTime(Fecha);
                codigoCliente = Convert.ToInt32(IdCliente);
                total = Convert.ToDecimal(Total);
                imp = Convert.ToDecimal(importe);

                Factura factura = new Factura(fecha, imp, total, codigoCliente, iva);

                try
                {
                    db.Factura.Add(factura);
                    db.SaveChanges();
                    foreach (var data in detalles)
                    {
                        int idJuego = Convert.ToInt32(data.IdJuego.ToString());
                        byte cantidad = Convert.ToByte(data.CantidadFacturada.ToString());
                        FacturaDetalle detalle = new FacturaDetalle(getSecuencial(factura.IdFactura), factura.IdFactura, idJuego, cantidad);
                        db.FacturaDetalle.Add(detalle);

                    }
                    mensaje = "Factura guardada con éxito...";

                }
                catch(Exception e)
                {
                    mensaje = "Error al registrar la factura";
                }

            }

            return Json(mensaje);
        }

        public int getSecuencial(int idFactura)
        {
            var secuencial = (from d in db.FacturaDetalle.ToList()
                              where d.IdFactura == idFactura
                              select d).ToList().Max(s => s.Secuencial);

            if (secuencial == null || secuencial == 0)
                return 1;
            else
                return secuencial++;
        }

        public ActionResult DetallesFactura(int id)
        {
            var detalles = (from d in db.FacturaDetalle.ToList()
                            where d.IdFactura == id
                            select d).ToList();

            return View(detalles);
        }

        public ActionResult VentaFactura()
        {
            var facturas = db.Factura.ToList();
            return View(facturas);
        }
    }
}