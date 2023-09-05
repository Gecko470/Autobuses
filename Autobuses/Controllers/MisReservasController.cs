using Autobuses.Clases;
using Autobuses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Autobuses.Controllers
{
    public class MisReservasController : Controller
    {
        // GET: MisReservas
        public ActionResult Index()
        {
            var idsViajes = ControllerContext.HttpContext.Request.Cookies["idsViajes"];
            var datosViajes = ControllerContext.HttpContext.Request.Cookies["datosViajes"];

            List<ReservaCLS> list = new List<ReservaCLS>();

            if (idsViajes != null && datosViajes != null)
            {
                string idsViajesString = idsViajes.Value;
                string datosViajesString = datosViajes.Value;

                string[] arrayIds = idsViajesString.Split('!');
                string[] arrayDatos = datosViajesString.Split('!');
                string[] datosReserva = { };

                for (int i = 0; i < arrayIds.Length; i++)
                {
                    ReservaCLS oReservaCLS = new ReservaCLS();
                    datosReserva = arrayDatos[i].Split('#');

                    oReservaCLS.iidViaje = int.Parse(arrayIds[i]);
                    oReservaCLS.cantidad = int.Parse(datosReserva[0]);
                    oReservaCLS.fechaViaje = DateTime.Parse(datosReserva[1]);
                    oReservaCLS.lugarOrigen = datosReserva[2];
                    oReservaCLS.lugarDestino = datosReserva[3];
                    oReservaCLS.precio = decimal.Parse(datosReserva[4]);
                    oReservaCLS.matricula = datosReserva[5];

                    list.Add(oReservaCLS);
                }
            }

            return View(list);
        }

        public ActionResult Filtrar()
        {
            var idsViajes = ControllerContext.HttpContext.Request.Cookies["idsViajes"];
            var datosViajes = ControllerContext.HttpContext.Request.Cookies["datosViajes"];

            List<ReservaCLS> list = new List<ReservaCLS>();

            if (idsViajes != null && datosViajes != null)
            {
                string idsViajesString = idsViajes.Value;
                string datosViajesString = datosViajes.Value;

                string[] arrayIds = idsViajesString.Split('!');
                string[] arrayDatos = datosViajesString.Split('!');
                string[] datosReserva = { };

                for (int i = 0; i < arrayIds.Length; i++)
                {
                    ReservaCLS oReservaCLS = new ReservaCLS();
                    datosReserva = arrayDatos[i].Split('#');

                    oReservaCLS.iidViaje = int.Parse(arrayIds[i]);
                    oReservaCLS.cantidad = int.Parse(datosReserva[0]);
                    oReservaCLS.fechaViaje = DateTime.Parse(datosReserva[1]);
                    oReservaCLS.lugarOrigen = datosReserva[2];
                    oReservaCLS.lugarDestino = datosReserva[3];
                    oReservaCLS.precio = decimal.Parse(datosReserva[4]);
                    oReservaCLS.matricula = datosReserva[5];

                    list.Add(oReservaCLS);
                }
            }

            return PartialView("_TablaMisReservas", list);
        }

        public async Task<string> Guardar(string total)
        {
            string resp = "";
            try
            {
                var idsViajes = ControllerContext.HttpContext.Request.Cookies["idsViajes"];
                var datosViajes = ControllerContext.HttpContext.Request.Cookies["datosViajes"];

                string idsViajesString = idsViajes.Value;
                string datosViajesString = datosViajes.Value;

                string[] arrayIds = idsViajesString.Split('!');
                string[] arrayDatos = datosViajesString.Split('!');
                string[] reserva = { };

                using (var transaccion = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Usuario oUsuario = (Usuario)Session["user"];
                    VENTA oVenta = new VENTA();

                    using (var bd = new BDPasajeEntities())
                    {
                        oVenta.IIDUSUARIO = oUsuario.IIDUSUARIO;
                        oVenta.FECHAVENTA = DateTime.Now;
                        oVenta.TOTAL = decimal.Parse(total);
                        oVenta.BHABILITADO = 1;

                        bd.VENTA.Add(oVenta);
                        await bd.SaveChangesAsync();

                        int iidVenta = oVenta.IIDVENTA;

                        for (int i = 0; i < arrayDatos.Length; i++)
                        {
                            reserva = arrayDatos[i].Split('#');
                            DETALLEVENTA oDetalleVenta = new DETALLEVENTA();

                            oDetalleVenta.IIDVENTA = iidVenta;
                            oDetalleVenta.IIDVIAJE = int.Parse(arrayIds[i]);
                            oDetalleVenta.IIDBUS = int.Parse(reserva[5]);
                            oDetalleVenta.CANTIDAD = int.Parse(reserva[0]);
                            oDetalleVenta.PRECIO = decimal.Parse(reserva[4]);
                            oDetalleVenta.BHABILITADO = 1;

                            bd.DETALLEVENTA.Add(oDetalleVenta);
                            await bd.SaveChangesAsync();

                            Viaje oViaje = await bd.Viaje.FirstOrDefaultAsync(x => x.IIDVIAJE == oDetalleVenta.IIDVIAJE);
                            oViaje.NUMEROASIENTOSDISPONIBLES = oViaje.NUMEROASIENTOSDISPONIBLES - int.Parse(reserva[0]);

                            await bd.SaveChangesAsync();
                        }
                    }

                      transaccion.Complete();
                }

                HttpCookie cookieId = new HttpCookie("idsViajes", "");
                HttpCookie cookieDatos = new HttpCookie("datosViajes", "");

                ControllerContext.HttpContext.Response.SetCookie(cookieId);
                ControllerContext.HttpContext.Response.SetCookie(cookieDatos);

                resp = "1";
            }
            catch (Exception ex)
            {
                resp = "";
            }

            return resp;
        }
    }
}