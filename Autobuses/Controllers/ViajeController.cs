using Autobuses.Clases;
using Autobuses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Autobuses.Controllers
{
    public class ViajeController : Controller
    {
        // GET: Viaje
        public async Task<ActionResult> Index()
        {
            await Lugares();
            await Buses();

            List<ViajeCLS> list = new List<ViajeCLS>();

            using (var db = new BDPasajeEntities())
            {
                list = await (from viaje in db.Viaje
                              join lugarO in db.Lugar
                              on viaje.IIDLUGARORIGEN equals lugarO.IIDLUGAR
                              join lugarD in db.Lugar
                              on viaje.IIDLUGARDESTINO equals lugarD.IIDLUGAR
                              join bus in db.Bus
                              on viaje.IIDBUS equals bus.IIDBUS
                              where viaje.BHABILITADO == 1
                              select new ViajeCLS()
                              {
                                  IIDVIAJE = viaje.IIDVIAJE,
                                  Origen = lugarO.NOMBRE,
                                  Destino = lugarD.NOMBRE,
                                  PRECIO = (decimal)viaje.PRECIO,
                                  FECHAVIAJE = (DateTime)viaje.FECHAVIAJE,
                                  Bus = bus.PLACA,
                                  NUMEROASIENTOSDISPONIBLES = (int)viaje.NUMEROASIENTOSDISPONIBLES

                              }).ToListAsync();
            }
            return View(list);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await Lugares();
            await Buses();

            return View();
        }

        public async Task<ActionResult> Filtro(int? lugarDestino)
        {
            List<ViajeCLS> list = new List<ViajeCLS>();

            using (var db = new BDPasajeEntities())
            {
                if (lugarDestino == null)
                {
                    list = await (from viaje in db.Viaje
                                  join lugarO in db.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarO.IIDLUGAR
                                  join lugarD in db.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarD.IIDLUGAR
                                  join bus in db.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  select new ViajeCLS()
                                  {
                                      IIDVIAJE = viaje.IIDVIAJE,
                                      Origen = lugarO.NOMBRE,
                                      Destino = lugarD.NOMBRE,
                                      PRECIO = (decimal)viaje.PRECIO,
                                      FECHAVIAJE = (DateTime)viaje.FECHAVIAJE,
                                      Bus = bus.PLACA,
                                      NUMEROASIENTOSDISPONIBLES = (int)viaje.NUMEROASIENTOSDISPONIBLES

                                  }).ToListAsync();
                }
                else
                {
                    list = await (from viaje in db.Viaje
                                  join lugarO in db.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarO.IIDLUGAR
                                  join lugarD in db.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarD.IIDLUGAR
                                  join bus in db.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1 && viaje.IIDLUGARDESTINO == lugarDestino
                                  select new ViajeCLS()
                                  {
                                      IIDVIAJE = viaje.IIDVIAJE,
                                      Origen = lugarO.NOMBRE,
                                      Destino = lugarD.NOMBRE,
                                      PRECIO = (decimal)viaje.PRECIO,
                                      FECHAVIAJE = (DateTime)viaje.FECHAVIAJE,
                                      Bus = bus.PLACA,
                                      NUMEROASIENTOSDISPONIBLES = (int)viaje.NUMEROASIENTOSDISPONIBLES

                                  }).ToListAsync();
                }

            }

            return PartialView("_TablaViaje", list);
        }

        public async Task<string> Guardar(ViajeCLS oViajeCLS, HttpPostedFileBase fotografia, int op)
        {
            int r = 0;
            string resp = "";

            try
            {
                if (!ModelState.IsValid || (fotografia == null && op == -1))
                {
                    var errores = (from state in ModelState.Values
                                   from error in state.Errors
                                   select error.ErrorMessage).ToList();

                    resp += "<ul class='list-group'>";
                    if (fotografia == null && op == -1) resp += "<li class='list-group-item text-danger'>La foto es obligatoria..</li>";
                    foreach (var item in errores)
                    {
                        resp += "<li class='list-group-item text-danger'>" + item + "</li>";
                    }
                    resp += "</ul>";
                }
                else
                {
                    byte[] fotoBD = null;
                    if (fotografia != null)
                    {
                        BinaryReader reader = new BinaryReader(fotografia.InputStream);
                        fotoBD = reader.ReadBytes((int)fotografia.ContentLength);
                    }
                    Viaje viaje = new Viaje();

                    using (var bd = new BDPasajeEntities())
                    {
                        if (op == -1)
                        {
                            viaje.IIDLUGARORIGEN = oViajeCLS.IIDLUGARORIGEN;
                            viaje.IIDLUGARDESTINO = oViajeCLS.IIDLUGARDESTINO;
                            viaje.PRECIO = oViajeCLS.PRECIO;
                            viaje.FECHAVIAJE = oViajeCLS.FECHAVIAJE;
                            viaje.IIDBUS = oViajeCLS.IIDBUS;
                            viaje.NUMEROASIENTOSDISPONIBLES = oViajeCLS.NUMEROASIENTOSDISPONIBLES;
                            viaje.FOTO = fotoBD;
                            viaje.nombrefoto = oViajeCLS.nombrefoto;
                            viaje.BHABILITADO = 1;

                            bd.Viaje.Add(viaje);
                            r = await bd.SaveChangesAsync();
                            resp = r.ToString();
                            if (resp == "0") resp = "";
                        }
                        else
                        {
                            viaje = await bd.Viaje.FirstOrDefaultAsync(x => x.IIDVIAJE == op);

                            viaje.IIDLUGARORIGEN = oViajeCLS.IIDLUGARORIGEN;
                            viaje.IIDLUGARDESTINO = oViajeCLS.IIDLUGARDESTINO;
                            viaje.PRECIO = oViajeCLS.PRECIO;
                            viaje.FECHAVIAJE = oViajeCLS.FECHAVIAJE;
                            viaje.IIDBUS = oViajeCLS.IIDBUS;
                            viaje.NUMEROASIENTOSDISPONIBLES = oViajeCLS.NUMEROASIENTOSDISPONIBLES;
                            if (fotografia != null) viaje.FOTO = fotoBD;
                            viaje.nombrefoto = oViajeCLS.nombrefoto;

                            r = await bd.SaveChangesAsync();
                            resp = r.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                resp = "";
            }

            return resp;
        }


        public async Task<JsonResult> Edit(int id)
        {

            ViajeCLS oViajeCLS = new ViajeCLS();

            using (var bd = new BDPasajeEntities())
            {
                Viaje viaje = await bd.Viaje.FirstOrDefaultAsync(x => x.IIDVIAJE == id);

                oViajeCLS.IIDVIAJE = viaje.IIDVIAJE;
                oViajeCLS.IIDLUGARORIGEN = (int)viaje.IIDLUGARORIGEN;
                oViajeCLS.IIDLUGARDESTINO = (int)viaje.IIDLUGARDESTINO;
                oViajeCLS.IIDBUS = (int)viaje.IIDBUS;
                oViajeCLS.PRECIO = (int)viaje.PRECIO;
                oViajeCLS.FechaViajeString = ((DateTime)viaje.FECHAVIAJE).ToString("yyyy-MM-dd");
                oViajeCLS.NUMEROASIENTOSDISPONIBLES = (int)viaje.NUMEROASIENTOSDISPONIBLES;
                oViajeCLS.nombrefoto = viaje.nombrefoto;
                oViajeCLS.extension = Path.GetExtension(viaje.nombrefoto);
                oViajeCLS.FotoString = Convert.ToBase64String(viaje.FOTO);

            }
            return Json(oViajeCLS, JsonRequestBehavior.AllowGet);
        }


        public async Task<int> Delete(int id)
        {
            int resp = 0;
            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    Viaje viaje = await bd.Viaje.FirstOrDefaultAsync(x => x.IIDVIAJE == id);
                    viaje.BHABILITADO = 0;

                    resp = await bd.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                resp = 0;
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

        private async Task Buses()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                lista = await (from item in bd.Bus
                               where item.BHABILITADO == 1
                               select new SelectListItem()
                               {
                                   Text = item.PLACA,
                                   Value = item.IIDBUS.ToString()

                               }).ToListAsync();
            }

            lista.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaBuses = lista;
        }
    }
}