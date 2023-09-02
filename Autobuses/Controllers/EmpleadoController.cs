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
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public async Task<ActionResult> Index(int idTU = 0)
        {
            await TiposContrato();
            await TiposUsuario();
            await Sexos();

            List<EmpleadoCLS> lista = new List<EmpleadoCLS>();
            using (var db = new BDPasajeEntities())
            {
                if(idTU == 0)
                {
                    lista = await (from empleado in db.Empleado
                               join tipoUsuario in db.TipoUsuario
                               on empleado.IIDTIPOUSUARIO equals tipoUsuario.IIDTIPOUSUARIO
                               join tipoContrato in db.TipoContrato
                               on empleado.IIDTIPOCONTRATO equals tipoContrato.IIDTIPOCONTRATO
                               join sexo in db.Sexo
                               on empleado.IIDSEXO equals sexo.IIDSEXO
                               where empleado.BHABILITADO == 1
                               select new EmpleadoCLS()
                               {
                                   IIDEMPLEADO = empleado.IIDEMPLEADO,
                                   IIDSEXO = empleado.IIDEMPLEADO,
                                   NOMBRE = empleado.NOMBRE,
                                   APPATERNO = empleado.APPATERNO,
                                   APMATERNO = empleado.APMATERNO,
                                   FECHACONTRATO = (DateTime)empleado.FECHACONTRATO,
                                   SUELDO = (decimal)empleado.SUELDO,
                                   TipoUsuario = tipoUsuario.NOMBRE,
                                   TipoContrato = tipoContrato.NOMBRE,
                                   Sexo = sexo.NOMBRE

                               }).ToListAsync();
                }
                else
                {
                    lista = await (from empleado in db.Empleado
                               join tipoUsuario in db.TipoUsuario
                               on empleado.IIDTIPOUSUARIO equals tipoUsuario.IIDTIPOUSUARIO
                               join tipoContrato in db.TipoContrato
                               on empleado.IIDTIPOCONTRATO equals tipoContrato.IIDTIPOCONTRATO
                               join sexo in db.Sexo
                               on empleado.IIDSEXO equals sexo.IIDSEXO
                               where empleado.BHABILITADO == 1 && empleado.IIDTIPOUSUARIO == idTU
                               select new EmpleadoCLS()
                               {
                                   IIDEMPLEADO = empleado.IIDEMPLEADO,
                                   IIDSEXO = empleado.IIDEMPLEADO,
                                   NOMBRE = empleado.NOMBRE,
                                   APPATERNO = empleado.APPATERNO,
                                   APMATERNO = empleado.APMATERNO,
                                   FECHACONTRATO = (DateTime)empleado.FECHACONTRATO,
                                   SUELDO = (decimal)empleado.SUELDO,
                                   TipoUsuario = tipoUsuario.NOMBRE,
                                   TipoContrato = tipoContrato.NOMBRE,
                                   Sexo = sexo.NOMBRE

                               }).ToListAsync();
                }
            }

            return View(lista);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await TiposContrato();
            await TiposUsuario();
            await Sexos();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(EmpleadoCLS oEmpleadoCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Empleado.AnyAsync(x => x.NOMBRE == oEmpleadoCLS.NOMBRE && x.APPATERNO == oEmpleadoCLS.APPATERNO && x.APMATERNO == oEmpleadoCLS.APMATERNO);
            }

            if (!ModelState.IsValid || existe)
            {

                oEmpleadoCLS.MensajeError = "Ya existe ese empleado en la BD..";

                await TiposContrato();
                await TiposUsuario();
                await Sexos();

                return View(oEmpleadoCLS);
            }

            Empleado empleado = new Empleado();
            empleado.NOMBRE = oEmpleadoCLS.NOMBRE;
            empleado.APPATERNO = oEmpleadoCLS.APPATERNO;
            empleado.APMATERNO = oEmpleadoCLS.APMATERNO;
            empleado.FECHACONTRATO = oEmpleadoCLS.FECHACONTRATO;
            empleado.SUELDO = oEmpleadoCLS.SUELDO;
            empleado.IIDTIPOUSUARIO = oEmpleadoCLS.IIDTIPOUSUARIO;
            empleado.IIDTIPOCONTRATO = oEmpleadoCLS.IIDTIPOCONTRATO;
            empleado.IIDSEXO = oEmpleadoCLS.IIDSEXO;
            empleado.BHABILITADO = 1;

            using (var bd = new BDPasajeEntities())
            {
                bd.Empleado.Add(empleado);
                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            await TiposContrato();
            await TiposUsuario();
            await Sexos();

            EmpleadoCLS oEmpleadoCLS = new EmpleadoCLS();

            using (var bd = new BDPasajeEntities())
            {
                Empleado empleado = await bd.Empleado.FirstOrDefaultAsync(x => x.IIDEMPLEADO == id);

                oEmpleadoCLS.IIDEMPLEADO = empleado.IIDEMPLEADO;
                oEmpleadoCLS.NOMBRE = empleado.NOMBRE;
                oEmpleadoCLS.APPATERNO = empleado.APPATERNO;
                oEmpleadoCLS.APMATERNO = empleado.APMATERNO;
                oEmpleadoCLS.FECHACONTRATO = (DateTime)empleado.FECHACONTRATO;
                oEmpleadoCLS.SUELDO = (decimal)empleado.SUELDO;
                oEmpleadoCLS.IIDTIPOUSUARIO = (int)empleado.IIDTIPOUSUARIO;
                oEmpleadoCLS.IIDTIPOCONTRATO = (int)empleado.IIDTIPOCONTRATO;
                oEmpleadoCLS.IIDSEXO = (int)empleado.IIDSEXO;
            }

            return View(oEmpleadoCLS);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EmpleadoCLS oEmpleadoCLS)
        {
            bool existe = false;

            using (var bd = new BDPasajeEntities())
            {
                existe = await bd.Empleado.AnyAsync(x => x.NOMBRE == oEmpleadoCLS.NOMBRE && x.APPATERNO == oEmpleadoCLS.APPATERNO && x.APMATERNO == oEmpleadoCLS.APMATERNO && x.IIDEMPLEADO != oEmpleadoCLS.IIDEMPLEADO);
            }

            if (!ModelState.IsValid || existe)
            {

                oEmpleadoCLS.MensajeError = "Ya existe ese empleado en la BD..";

                await TiposContrato();
                await TiposUsuario();
                await Sexos();

                return View(oEmpleadoCLS);
            }

            using (var bd = new BDPasajeEntities())
            {
                Empleado empleado = await bd.Empleado.FirstOrDefaultAsync(x => x.IIDEMPLEADO == oEmpleadoCLS.IIDEMPLEADO);

                empleado.IIDEMPLEADO = oEmpleadoCLS.IIDEMPLEADO;
                empleado.NOMBRE = oEmpleadoCLS.NOMBRE;
                empleado.APPATERNO = oEmpleadoCLS.APPATERNO;
                empleado.APMATERNO = oEmpleadoCLS.APMATERNO;
                empleado.FECHACONTRATO = oEmpleadoCLS.FECHACONTRATO;
                empleado.SUELDO = oEmpleadoCLS.SUELDO;
                empleado.IIDTIPOUSUARIO = oEmpleadoCLS.IIDTIPOUSUARIO;
                empleado.IIDTIPOCONTRATO = oEmpleadoCLS.IIDTIPOCONTRATO;
                empleado.IIDSEXO = oEmpleadoCLS.IIDSEXO;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Empleado empleado = await bd.Empleado.FirstOrDefaultAsync(x => x.IIDEMPLEADO == id);

                empleado.BHABILITADO = 0;

                await bd.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        private async Task TiposContrato()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                lista = await (from tipoContrato in bd.TipoContrato
                               where tipoContrato.BHABILITADO == 1
                               select new SelectListItem()
                               {
                                   Text = tipoContrato.NOMBRE,
                                   Value = tipoContrato.IIDTIPOCONTRATO.ToString()

                               }).ToListAsync();
            }

            lista.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.tiposContratos = lista;
        }

        private async Task TiposUsuario()
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                lista = await (from tipoUsuario in bd.TipoUsuario
                               where tipoUsuario.BHABILITADO == 1
                               select new SelectListItem()
                               {
                                   Text = tipoUsuario.NOMBRE,
                                   Value = tipoUsuario.IIDTIPOUSUARIO.ToString()

                               }).ToListAsync();
            }

            lista.Insert(0, new SelectListItem()
            {
                Text = "Seleccione..",
                Value = ""
            });

            ViewBag.tiposUsuarios = lista;
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