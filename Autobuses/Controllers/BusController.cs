using Autobuses.Clases;
using Autobuses.Filters;
using Autobuses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Autobuses.Controllers
{
    [Acceso]
    public class BusController : Controller
    {
        // GET: Bus
        public async Task<ActionResult> Index(BusCLS oBusCLS)
        {
            await Sucursales();
            await TiposBus();
            await Modelos();
            await Marcas();

            List<BusCLS> list = new List<BusCLS>();
            List<BusCLS> listRpta = new List<BusCLS>();

            using (var db = new BDPasajeEntities())
            {
                list = await (from bus in db.Bus
                              join sucursal in db.Sucursal
                              on bus.IIDSUCURSAL equals sucursal.IIDSUCURSAL
                              join tipoBus in db.TipoBus
                              on bus.IIDTIPOBUS equals tipoBus.IIDTIPOBUS
                              join modelo in db.Modelo
                              on bus.IIDMODELO equals modelo.IIDMODELO
                              join marca in db.Marca
                              on bus.IIDMARCA equals marca.IIDMARCA
                              where bus.BHABILITADO == 1
                              select new BusCLS()
                              {
                                  IIDBUS = bus.IIDBUS,
                                  Sucursal = sucursal.NOMBRE,
                                  TipoBus = tipoBus.NOMBRE,
                                  PLACA = bus.PLACA,
                                  FECHACOMPRA = (DateTime)bus.FECHACOMPRA,
                                  Modelo = modelo.NOMBRE,
                                  Marca = marca.NOMBRE,
                                  NUMEROFILAS = (int)bus.NUMEROFILAS,
                                  NUMEROCOLUMNAS = (int)bus.NUMEROCOLUMNAS,
                                  DESCRIPCION = bus.DESCRIPCION,
                                  OBSERVACION = bus.OBSERVACION,
                                  IIDSUCURSAL = (int)bus.IIDSUCURSAL,
                                  IIDMARCA = (int)bus.IIDMARCA,
                                  IIDMODELO = (int)bus.IIDMODELO,
                                  IIDTIPOBUS = (int)bus.IIDTIPOBUS

                              }).ToListAsync();


                if (oBusCLS.IIDBUS == 0 && oBusCLS.IIDSUCURSAL == 0 && oBusCLS.IIDTIPOBUS == 0 && string.IsNullOrEmpty(oBusCLS.PLACA) && oBusCLS.FECHACOMPRA.ToString() == "01/01/0001 0:00:00" && oBusCLS.IIDMODELO == 0 && oBusCLS.IIDMARCA == 0 && oBusCLS.NUMEROFILAS == 0
                    && oBusCLS.NUMEROCOLUMNAS == 0 && string.IsNullOrEmpty(oBusCLS.DESCRIPCION) && string.IsNullOrEmpty(oBusCLS.OBSERVACION))
                {
                    listRpta = list;
                }
                else
                {
                    if (oBusCLS.IIDBUS != 0)
                    {
                        list = list.Where(x => x.IIDBUS == oBusCLS.IIDBUS).ToList();
                    }
                    if (oBusCLS.IIDSUCURSAL != 0)
                    {
                        list = list.Where(x => x.IIDSUCURSAL == oBusCLS.IIDSUCURSAL).ToList();
                    }
                    if (oBusCLS.IIDTIPOBUS != 0)
                    {
                        list = list.Where(x => x.IIDTIPOBUS == oBusCLS.IIDTIPOBUS).ToList();
                    }
                    if (!string.IsNullOrEmpty(oBusCLS.PLACA))
                    {
                        list = list.Where(x => x.PLACA.Contains(oBusCLS.PLACA)).ToList();
                    }
                    if (oBusCLS.FECHACOMPRA.ToString() != "01/01/0001 0:00:00")
                    {
                        list = list.Where(x => x.FECHACOMPRA.ToString().Contains(oBusCLS.FECHACOMPRA.ToString())).ToList();
                    }
                    if (oBusCLS.IIDMODELO != 0)
                    {
                        list = list.Where(x => x.IIDMODELO == oBusCLS.IIDMODELO).ToList();
                    }
                    if (oBusCLS.IIDMARCA != 0)
                    {
                        list = list.Where(x => x.IIDMARCA == oBusCLS.IIDMARCA).ToList();
                    }
                    if (oBusCLS.NUMEROFILAS != 0)
                    {
                        list = list.Where(x => x.NUMEROFILAS.ToString().Contains(oBusCLS.NUMEROFILAS.ToString())).ToList();
                    }
                    if (oBusCLS.NUMEROCOLUMNAS != 0)
                    {
                        list = list.Where(x => x.NUMEROCOLUMNAS.ToString().Contains(oBusCLS.NUMEROCOLUMNAS.ToString())).ToList();
                    }
                    if (!string.IsNullOrEmpty(oBusCLS.DESCRIPCION))
                    {
                        list = list.Where(x => x.DESCRIPCION.Contains(oBusCLS.DESCRIPCION)).ToList();
                    }
                    if (!string.IsNullOrEmpty(oBusCLS.OBSERVACION))
                    {
                        list = list.Where(x => x.OBSERVACION.Contains(oBusCLS.OBSERVACION)).ToList();
                    }

                    listRpta = list;
                }

            }
            return View(listRpta);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await Sucursales();
            await TiposBus();
            await Modelos();
            await Marcas();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(BusCLS oBusCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Bus.AnyAsync(x => x.PLACA == oBusCLS.PLACA);
            }

            if (!ModelState.IsValid || existe)
            {
                oBusCLS.MensajeError = "Ya existe ese cliente en la BD..";

                await Sucursales();
                await TiposBus();
                await Modelos();
                await Marcas();

                return View(oBusCLS);
            }

            Bus bus = new Bus();

            using (var bd = new BDPasajeEntities())
            {
                bus.IIDSUCURSAL = oBusCLS.IIDSUCURSAL;
                bus.IIDTIPOBUS = oBusCLS.IIDTIPOBUS;
                bus.PLACA = oBusCLS.PLACA;
                bus.FECHACOMPRA = oBusCLS.FECHACOMPRA;
                bus.IIDMODELO = oBusCLS.IIDMODELO;
                bus.NUMEROFILAS = oBusCLS.NUMEROFILAS;
                bus.NUMEROCOLUMNAS = oBusCLS.NUMEROCOLUMNAS;
                bus.DESCRIPCION = oBusCLS.DESCRIPCION;
                bus.OBSERVACION = oBusCLS.OBSERVACION;
                bus.IIDMARCA = oBusCLS.IIDMARCA;
                bus.BHABILITADO = 1;

                bd.Bus.Add(bus);
                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            BusCLS oBusCLS = new BusCLS();

            using (var bd = new BDPasajeEntities())
            {
                Bus bus = await bd.Bus.FirstOrDefaultAsync(x => x.IIDBUS == id);

                oBusCLS.IIDBUS = bus.IIDBUS;
                oBusCLS.IIDSUCURSAL = (int)bus.IIDSUCURSAL;
                oBusCLS.IIDTIPOBUS = (int)bus.IIDTIPOBUS;
                oBusCLS.PLACA = bus.PLACA;
                oBusCLS.FECHACOMPRA = (DateTime)bus.FECHACOMPRA;
                oBusCLS.IIDMODELO = (int)bus.IIDMODELO;
                oBusCLS.NUMEROFILAS = (int)bus.NUMEROFILAS;
                oBusCLS.NUMEROCOLUMNAS = (int)bus.NUMEROCOLUMNAS;
                oBusCLS.DESCRIPCION = bus.DESCRIPCION;
                oBusCLS.OBSERVACION = bus.OBSERVACION;
                oBusCLS.IIDMARCA = (int)bus.IIDMARCA;

            }

            await Sucursales();
            await TiposBus();
            await Modelos();
            await Marcas();

            return View(oBusCLS);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(BusCLS oBusCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Bus.AnyAsync(x => x.PLACA == oBusCLS.PLACA && x.IIDBUS != oBusCLS.IIDBUS);
            }

            if (!ModelState.IsValid || existe)
            {
                oBusCLS.MensajeError = "Ya existe ese bus en la BD..";

                await Sucursales();
                await TiposBus();
                await Modelos();
                await Marcas();

                return View(oBusCLS);
            }

            using (var bd = new BDPasajeEntities())
            {
                Bus bus = await bd.Bus.FirstOrDefaultAsync(x => x.IIDBUS == oBusCLS.IIDBUS);

                bus.IIDSUCURSAL = oBusCLS.IIDSUCURSAL;
                bus.IIDTIPOBUS = oBusCLS.IIDTIPOBUS;
                bus.PLACA = oBusCLS.PLACA;
                bus.FECHACOMPRA = oBusCLS.FECHACOMPRA;
                bus.IIDMODELO = oBusCLS.IIDMODELO;
                bus.NUMEROFILAS = oBusCLS.NUMEROFILAS;
                bus.NUMEROCOLUMNAS = oBusCLS.NUMEROCOLUMNAS;
                bus.DESCRIPCION = oBusCLS.DESCRIPCION;
                bus.OBSERVACION = oBusCLS.OBSERVACION;
                bus.IIDMARCA = oBusCLS.IIDMARCA;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Bus bus = await bd.Bus.FirstOrDefaultAsync(x => x.IIDBUS == id);

                bus.BHABILITADO = 0;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private async Task Sucursales()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                lista = await (from sucursal in bd.Sucursal
                               where sucursal.BHABILITADO == 1
                               select new SelectListItem()
                               {
                                   Text = sucursal.NOMBRE,
                                   Value = sucursal.IIDSUCURSAL.ToString()

                               }).ToListAsync();
            }

            lista.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaSucursales = lista;
        }

        private async Task TiposBus()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                lista = await (from bus in bd.TipoBus
                               where bus.BHABILITADO == 1
                               select new SelectListItem()
                               {
                                   Text = bus.NOMBRE,
                                   Value = bus.IIDTIPOBUS.ToString()

                               }).ToListAsync();
            }

            lista.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaTipoBus = lista;
        }

        private async Task Modelos()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                lista = await (from modelo in bd.Modelo
                               where modelo.BHABILITADO == 1
                               select new SelectListItem()
                               {
                                   Text = modelo.NOMBRE,
                                   Value = modelo.IIDMODELO.ToString()

                               }).ToListAsync();
            }

            lista.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaModelos = lista;
        }

        private async Task Marcas()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                lista = await (from marca in bd.Marca
                               where marca.BHABILITADO == 1
                               select new SelectListItem()
                               {
                                   Text = marca.NOMBRE,
                                   Value = marca.IIDMARCA.ToString()

                               }).ToListAsync();
            }

            lista.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.listaMarcas = lista;
        }
    }
}