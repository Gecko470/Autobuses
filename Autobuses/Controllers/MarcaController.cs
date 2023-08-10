using Autobuses.Clases;
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
    public class MarcaController : Controller
    {
        public async Task<ActionResult> Index(MarcaCLS oMarcaCLS)
        {
            List<MarcaCLS> list = new List<MarcaCLS>();

            using (var context = new BDPasajeEntities())
            {
                if (string.IsNullOrEmpty(oMarcaCLS.NOMBRE))
                {
                    list = await (from marca in context.Marca
                                  where marca.BHABILITADO == 1
                                  select new MarcaCLS()
                                  {
                                      IIDMARCA = marca.IIDMARCA,
                                      NOMBRE = marca.NOMBRE,
                                      DESCRIPCION = marca.DESCRIPCION

                                  }).ToListAsync();
                }
                else
                {
                    list = await (from marca in context.Marca
                                  where marca.BHABILITADO == 1 && marca.NOMBRE.Contains(oMarcaCLS.NOMBRE)
                                  select new MarcaCLS()
                                  {
                                      IIDMARCA = marca.IIDMARCA,
                                      NOMBRE = marca.NOMBRE,
                                      DESCRIPCION = marca.DESCRIPCION

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
        public async Task<ActionResult> Create(MarcaCLS oMarcaCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Marca.AnyAsync(x => x.NOMBRE == oMarcaCLS.NOMBRE);
            }

            if (!ModelState.IsValid || existe)
            {
                oMarcaCLS.MensajeError = "Ya existe ese nombre en la BD..";
                return View(oMarcaCLS);
            }

            Marca marca = new Marca();
            marca.NOMBRE = oMarcaCLS.NOMBRE;
            marca.DESCRIPCION = oMarcaCLS.DESCRIPCION;
            marca.BHABILITADO = 1;

            using (var db = new BDPasajeEntities())
            {
                db.Marca.Add(marca);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            MarcaCLS oMarcaCLS = new MarcaCLS();

            using (var bd = new BDPasajeEntities())
            {
                /*oMarcaCLS = await (from marca in bd.Marca
                                   where marca.IIDMARCA == id
                                   select new MarcaCLS()
                                   {
                                        IIDMARCA = marca.IIDMARCA,
                                        NOMBRE = marca.NOMBRE,
                                        DESCRIPCION = marca.DESCRIPCION

                                   }).FirstOrDefaultAsync();*/

                Marca marca = await bd.Marca.FirstOrDefaultAsync(x => x.IIDMARCA == id);

                oMarcaCLS.IIDMARCA = marca.IIDMARCA;
                oMarcaCLS.NOMBRE = marca.NOMBRE;
                oMarcaCLS.DESCRIPCION = marca.DESCRIPCION;
            }

            return View(oMarcaCLS);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MarcaCLS oMarcaCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Marca.AnyAsync(x => x.NOMBRE == oMarcaCLS.NOMBRE && x.IIDMARCA != oMarcaCLS.IIDMARCA);
            }

            if (!ModelState.IsValid || existe)
            {
                oMarcaCLS.MensajeError = "Ya existe ese nombre en la BD..";
                return View(oMarcaCLS);
            }

            using (var bd = new BDPasajeEntities())
            {
                Marca marca = await bd.Marca.FirstOrDefaultAsync(x => x.IIDMARCA == oMarcaCLS.IIDMARCA);

                marca.NOMBRE = oMarcaCLS.NOMBRE;
                marca.DESCRIPCION = oMarcaCLS.DESCRIPCION;

                await bd.SaveChangesAsync();
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Marca marca = await bd.Marca.FirstOrDefaultAsync(x => x.IIDMARCA == id);

                marca.BHABILITADO = 0;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}