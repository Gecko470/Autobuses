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
    public class SucursalController : Controller
    {
        // GET: Sucursal
        public async Task<ActionResult> Index(SucursalCLS oSucursalCLS)
        {
            List<SucursalCLS> list = new List<SucursalCLS>();

            using (var db = new BDPasajeEntities())
            {
                if (string.IsNullOrEmpty(oSucursalCLS.NOMBRE))
                {
                    list = await (from sucursal in db.Sucursal
                        where sucursal.BHABILITADO == 1
                        select new SucursalCLS()
                        {
                            IIDSUCURSAL = sucursal.IIDSUCURSAL,
                            NOMBRE = sucursal.NOMBRE,
                            DIRECCION = sucursal.DIRECCION,
                            TELEFONO = sucursal.TELEFONO,
                            EMAIL = sucursal.EMAIL,
                            FECHAAPERTURA = sucursal.FECHAAPERTURA

                        }).ToListAsync();
                }
                else
                {
                    list = await (from sucursal in db.Sucursal
                        where sucursal.BHABILITADO == 1 && sucursal.NOMBRE.Contains(oSucursalCLS.NOMBRE)
                        select new SucursalCLS()
                        {
                            IIDSUCURSAL = sucursal.IIDSUCURSAL,
                            NOMBRE = sucursal.NOMBRE,
                            DIRECCION = sucursal.DIRECCION,
                            TELEFONO = sucursal.TELEFONO,
                            EMAIL = sucursal.EMAIL,
                            FECHAAPERTURA = sucursal.FECHAAPERTURA

                        }).ToListAsync();
                }
            }
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(SucursalCLS oSucursalCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Sucursal.AnyAsync(x => x.NOMBRE == oSucursalCLS.NOMBRE);
            }

            if (!ModelState.IsValid || existe)
            {
                oSucursalCLS.MensajeError = "Ya existe ese nombre en la BD..";
                return View(oSucursalCLS);
            }

            Sucursal sucursal = new Sucursal(); 
            sucursal.NOMBRE = oSucursalCLS.NOMBRE;
            sucursal.DIRECCION = oSucursalCLS.DIRECCION;
            sucursal.TELEFONO = oSucursalCLS.TELEFONO;
            sucursal.EMAIL = oSucursalCLS.EMAIL;
            sucursal.FECHAAPERTURA = oSucursalCLS.FECHAAPERTURA;
            sucursal.BHABILITADO = 1;

            using(var db = new BDPasajeEntities())
            {
                db.Sucursal.Add(sucursal);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            SucursalCLS oSucursalCLS = new SucursalCLS();

            using(var bd = new BDPasajeEntities())
            {
                Sucursal sucursal = await bd.Sucursal.FirstOrDefaultAsync(x => x.IIDSUCURSAL == id);

                oSucursalCLS.IIDSUCURSAL = sucursal.IIDSUCURSAL;
                oSucursalCLS.NOMBRE = sucursal.NOMBRE;
                oSucursalCLS.DIRECCION = sucursal.DIRECCION;
                oSucursalCLS.TELEFONO = sucursal.TELEFONO;
                oSucursalCLS.EMAIL = sucursal.EMAIL;
                oSucursalCLS.FECHAAPERTURA = sucursal.FECHAAPERTURA;
            }

            return View(oSucursalCLS);
        }

        [HttpPost]
        public async Task<ActionResult>Edit(SucursalCLS oSucursalCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Sucursal.AnyAsync(x => x.NOMBRE == oSucursalCLS.NOMBRE && x.IIDSUCURSAL != oSucursalCLS.IIDSUCURSAL);
            }

            if (!ModelState.IsValid || existe)
            {
                oSucursalCLS.MensajeError = "Ya existe ese nombre en la BD..";
                return View(oSucursalCLS);
            }

            using(var bd = new BDPasajeEntities())
            {
                Sucursal sucursal = await bd.Sucursal.FirstOrDefaultAsync(x => x.IIDSUCURSAL == oSucursalCLS.IIDSUCURSAL);

                sucursal.NOMBRE = oSucursalCLS.NOMBRE;
                sucursal.DIRECCION = oSucursalCLS.DIRECCION;
                sucursal.TELEFONO = oSucursalCLS.TELEFONO;
                sucursal.EMAIL = oSucursalCLS.EMAIL;
                sucursal.FECHAAPERTURA = oSucursalCLS.FECHAAPERTURA;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Sucursal sucursal = await bd.Sucursal.FirstOrDefaultAsync(x => x.IIDSUCURSAL == id);

                sucursal.BHABILITADO = 0;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}