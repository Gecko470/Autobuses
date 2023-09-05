
using Autobuses.Clases;
using Autobuses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Autobuses.Controllers
{
    public class ReservaController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var idsViajes = ControllerContext.HttpContext.Request.Cookies["idsViajes"];
            var datosViajes = ControllerContext.HttpContext.Request.Cookies["datosViajes"];

            if (idsViajes != null && datosViajes != null)
            {
                ViewBag.idsViajes = idsViajes.Value;
                ViewBag.datosViajes = datosViajes.Value;
            }

            await Lugares();

            List<ReservaCLS> listaReservas = new List<ReservaCLS>();

            using (var bd = new BDPasajeEntities())
            {
                listaReservas = await (from viaje in bd.Viaje
                                       join bus in bd.Bus
                                       on viaje.IIDBUS equals bus.IIDBUS
                                       join lugarOrg in bd.Lugar
                                       on viaje.IIDLUGARORIGEN equals lugarOrg.IIDLUGAR
                                       join lugarDest in bd.Lugar
                                       on viaje.IIDLUGARDESTINO equals lugarDest.IIDLUGAR
                                       where viaje.BHABILITADO == 1
                                       select new ReservaCLS()
                                       {
                                           iidViaje = viaje.IIDVIAJE,
                                           nombreFoto = viaje.nombrefoto,
                                           foto = viaje.FOTO,
                                           lugarOrigen = lugarOrg.NOMBRE,
                                           lugarDestino = lugarDest.NOMBRE,
                                           precio = (decimal)viaje.PRECIO,
                                           fechaViaje = (DateTime)viaje.FECHAVIAJE,
                                           matricula = bus.PLACA,
                                           descripcion = bus.DESCRIPCION,
                                           asientosDisponibles = (int)viaje.NUMEROASIENTOSDISPONIBLES,
                                           totalAsientos = (int)(bus.NUMEROCOLUMNAS * bus.NUMEROFILAS),
                                           iidBus = bus.IIDBUS

                                       }).ToListAsync();
            }

            return View(listaReservas);
        }

        public string GenerarCookies(string iidViaje, string cantidad, string fechaViaje, string lugarOrigen, string lugarDestino, string precio, string iidBus)
        {
            string resp = "";

            try
            {
                var idsViajes = ControllerContext.HttpContext.Request.Cookies["idsViajes"];
                var datosViajes = ControllerContext.HttpContext.Request.Cookies["datosViajes"];

                if (idsViajes != null && datosViajes != null && idsViajes.Value != "" && datosViajes.Value != "")
                {
                    string idCookieString = idsViajes.Value + "!" + iidViaje;
                    string datosCookieString = datosViajes.Value + "!" + cantidad + "#" + fechaViaje + "#" + lugarOrigen + "#" + lugarDestino + "#" + precio + "#" + iidBus;

                    HttpCookie cookieId = new HttpCookie("idsViajes", idCookieString);
                    HttpCookie cookieDatos = new HttpCookie("datosViajes", datosCookieString);

                    ControllerContext.HttpContext.Response.SetCookie(cookieId);
                    ControllerContext.HttpContext.Response.SetCookie(cookieDatos);
                }
                else
                {
                    string datosString = cantidad + "#" + fechaViaje + "#" + lugarOrigen + "#" + lugarDestino + "#" + precio + "#" + iidBus;

                    HttpCookie cookieId = new HttpCookie("idsViajes", iidViaje);
                    HttpCookie cookieDatos = new HttpCookie("datosViajes", datosString);

                    ControllerContext.HttpContext.Response.SetCookie(cookieId);
                    ControllerContext.HttpContext.Response.SetCookie(cookieDatos);
                }
            }
            catch (Exception ex)
            {

            }

            return resp;

        }

        public string RemoverCookies(string id)
        {
            string resp = "";
            try
            {
                var idsViajes = ControllerContext.HttpContext.Request.Cookies["idsViajes"];
                var datosViajes = ControllerContext.HttpContext.Request.Cookies["datosViajes"];

                string idsViajesString = idsViajes.Value;
                string datosViajesString = datosViajes.Value;

                string[] idsViajesArray = idsViajesString.Split('!');
                int indice = Array.IndexOf(idsViajesArray, id);

                string nuevoIdsViajes;

                if (idsViajesString.Contains("!" + id))
                {
                    nuevoIdsViajes = idsViajesString.Replace("!" + id, "");
                }
                else if (idsViajesString.Contains(id + "!"))
                {
                    nuevoIdsViajes = idsViajesString.Replace(id + "!", "");
                }
                else
                {
                    nuevoIdsViajes = idsViajesString.Replace(id, "");
                }

                List<string> listaDatosViajes = datosViajesString.Split('!').ToList();
                listaDatosViajes.RemoveAt(indice);
                string[] nuevoArrayDatos = listaDatosViajes.ToArray();
                string nuevoStringDatos = string.Join("!", nuevoArrayDatos);

                HttpCookie cookieId = new HttpCookie("idsViajes", nuevoIdsViajes);
                HttpCookie cookieDatos = new HttpCookie("datosViajes", nuevoStringDatos);

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

        private async Task Lugares()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                lista = await (from item in bd.Lugar
                               where item.BHABILITADO == 1
                               select new SelectListItem()
                               {
                                   Text = item.NOMBRE,
                                   Value = item.IIDLUGAR.ToString()

                               }).ToListAsync();
            }

            lista.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaLugares = lista;
        }
    }
}