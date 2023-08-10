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
    public class ClienteController : Controller
    {
        // GET: Cliente
        public async Task<ActionResult> Index(ClienteCLS oClienteCLS)
        {
            await Sexos();

            List<ClienteCLS> list = new List<ClienteCLS>();

            using (var db = new BDPasajeEntities())
            {
                if(oClienteCLS.IIDSEXO  == 0)
                {
                    list = await (from cliente in db.Cliente
                              where cliente.BHABILITADO == 1
                              select new ClienteCLS()
                              {
                                  IIDCLIENTE = cliente.IIDCLIENTE,
                                  NOMBRE = cliente.NOMBRE,
                                  APPATERNO = cliente.APPATERNO,
                                  APMATERNO = cliente.APMATERNO,
                                  EMAIL = cliente.EMAIL,
                                  TELEFONOCELULAR = cliente.TELEFONOCELULAR,
                                  TELEFONOFIJO = cliente.TELEFONOFIJO,
                                  DIRECCION = cliente.DIRECCION,
                                  IIDSEXO = (int)cliente.IIDSEXO,
                                  BHABILITADO = (int)cliente.BHABILITADO

                              }).ToListAsync();
                }
                else
                {
                    list = await (from cliente in db.Cliente
                              where cliente.BHABILITADO == 1 && cliente.IIDSEXO == oClienteCLS.IIDSEXO
                              select new ClienteCLS()
                              {
                                  IIDCLIENTE = cliente.IIDCLIENTE,
                                  NOMBRE = cliente.NOMBRE,
                                  APPATERNO = cliente.APPATERNO,
                                  APMATERNO = cliente.APMATERNO,
                                  EMAIL = cliente.EMAIL,
                                  TELEFONOCELULAR = cliente.TELEFONOCELULAR,
                                  TELEFONOFIJO = cliente.TELEFONOFIJO,
                                  DIRECCION = cliente.DIRECCION,
                                  IIDSEXO = (int)cliente.IIDSEXO,
                                  BHABILITADO = (int)cliente.BHABILITADO

                              }).ToListAsync();
                }
            }
            return View(list);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await Sexos();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClienteCLS oClienteCLS)
        {
            bool existe = false;

            using(var bd = new BDPasajeEntities())
            {
                existe = await bd.Cliente.AnyAsync(x => x.NOMBRE == oClienteCLS.NOMBRE && x.APPATERNO == oClienteCLS.APPATERNO && x.APMATERNO == oClienteCLS.APMATERNO);
            }

            if (!ModelState.IsValid || existe)
            {
                await Sexos();
                oClienteCLS.MensajeError = "Ya existe ese cliente en la BD..";

                return View(oClienteCLS);
            }

            Cliente cliente = new Cliente();
            cliente.NOMBRE = oClienteCLS.NOMBRE;
            cliente.APPATERNO = oClienteCLS.APPATERNO;
            cliente.APMATERNO = oClienteCLS.APMATERNO;
            cliente.DIRECCION = oClienteCLS.DIRECCION;
            cliente.EMAIL = oClienteCLS.EMAIL;
            cliente.TELEFONOCELULAR = oClienteCLS.TELEFONOCELULAR;
            cliente.TELEFONOFIJO = oClienteCLS.TELEFONOFIJO;
            cliente.IIDSEXO = oClienteCLS.IIDSEXO;
            cliente.BHABILITADO = 1;

            using (var db = new BDPasajeEntities())
            {
                db.Cliente.Add(cliente);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            ClienteCLS oClienteCLS = new ClienteCLS();

            using (var bd = new BDPasajeEntities())
            {
                Cliente cliente = await bd.Cliente.FirstOrDefaultAsync(x => x.IIDCLIENTE == id);

                oClienteCLS.IIDCLIENTE = cliente.IIDCLIENTE;
                oClienteCLS.NOMBRE = cliente.NOMBRE;
                oClienteCLS.APPATERNO = cliente.APPATERNO;
                oClienteCLS.APMATERNO = cliente.APMATERNO;
                oClienteCLS.EMAIL = cliente.EMAIL;
                oClienteCLS.TELEFONOFIJO = cliente.TELEFONOFIJO;
                oClienteCLS.TELEFONOCELULAR = cliente.TELEFONOCELULAR;
                oClienteCLS.DIRECCION = cliente.DIRECCION;
                oClienteCLS.IIDSEXO = (int)cliente.IIDSEXO;
            }

            await Sexos();
            return View(oClienteCLS);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ClienteCLS oClienteCLS)
        {
            bool existe = false;

            using(var bd = new BDPasajeEntities())
            {
                existe = await bd.Cliente.AnyAsync(x => x.NOMBRE == oClienteCLS.NOMBRE && x.APPATERNO == oClienteCLS.APPATERNO && x.APMATERNO == oClienteCLS.APMATERNO && x.IIDCLIENTE != oClienteCLS.IIDCLIENTE);
            }

            if (!ModelState.IsValid || existe)
            {
                await Sexos();
                oClienteCLS.MensajeError = "Ya existe ese cliente en la BD..";

                return View(oClienteCLS);
            }

            using (var bd = new BDPasajeEntities())
            {
                Cliente cliente = await bd.Cliente.FirstOrDefaultAsync(x => x.IIDCLIENTE == oClienteCLS.IIDCLIENTE);

                cliente.IIDCLIENTE = oClienteCLS.IIDCLIENTE;
                cliente.NOMBRE = oClienteCLS.NOMBRE;
                cliente.APPATERNO = oClienteCLS.APPATERNO;
                cliente.APMATERNO = oClienteCLS.APMATERNO;
                cliente.EMAIL = oClienteCLS.EMAIL;
                cliente.TELEFONOFIJO = oClienteCLS.TELEFONOFIJO;
                cliente.TELEFONOCELULAR = oClienteCLS.TELEFONOCELULAR;
                cliente.DIRECCION = oClienteCLS.DIRECCION;
                cliente.IIDSEXO = oClienteCLS.IIDSEXO;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Cliente cliente = await bd.Cliente.FirstOrDefaultAsync(x => x.IIDCLIENTE == id);

                cliente.BHABILITADO = 0;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private async Task Sexos()
        {
            List<SelectListItem> listaSexos = new List<SelectListItem>();

            using (var db = new BDPasajeEntities())
            {
                listaSexos = await (from sexo in db.Sexo
                                    where sexo.BHABILITADO == 1
                                    select new SelectListItem()
                                    {
                                        Text = sexo.NOMBRE,
                                        Value = sexo.IIDSEXO.ToString()

                                    }).ToListAsync();

                listaSexos.Insert(0, new SelectListItem()
                {
                    Text = "Seleccione..",
                    Value = ""
                });
            }

            ViewBag.listaSexos = listaSexos;
        }
    }
}