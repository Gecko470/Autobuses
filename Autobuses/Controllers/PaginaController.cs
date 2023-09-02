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
using static iTextSharp.text.pdf.AcroFields;

namespace Autobuses.Controllers
{
    [Acceso]
    public class PaginaController : Controller
    {
        // GET: Pagina
        public async Task<ActionResult> Index()
        {
            List<PaginaCLS> list = new List<PaginaCLS>();

            using (var bd = new BDPasajeEntities())
            {
                list = await (from pagina in bd.Pagina
                              where pagina.BHABILITADO == 1
                              select new PaginaCLS()
                              {
                                  IIDPAGINA = pagina.IIDPAGINA,
                                  MENSAJE = pagina.MENSAJE,
                                  ACCION = pagina.ACCION,
                                  CONTROLADOR = pagina.CONTROLADOR

                              }).ToListAsync();
            }
            return View(list);
        }

        public async Task<ActionResult> Filtro(string mensaje)
        {
            List<PaginaCLS> list = new List<PaginaCLS>();

            using (var bd = new BDPasajeEntities())
            {
                if (string.IsNullOrEmpty(mensaje))
                {
                    list = await (from pagina in bd.Pagina
                                  where pagina.BHABILITADO == 1
                                  select new PaginaCLS()
                                  {
                                      IIDPAGINA = pagina.IIDPAGINA,
                                      MENSAJE = pagina.MENSAJE,
                                      ACCION = pagina.ACCION,
                                      CONTROLADOR = pagina.CONTROLADOR

                                  }).ToListAsync();
                }
                else
                {
                    list = await (from pagina in bd.Pagina
                                  where pagina.BHABILITADO == 1 && pagina.MENSAJE.Contains(mensaje)
                                  select new PaginaCLS()
                                  {
                                      IIDPAGINA = pagina.IIDPAGINA,
                                      MENSAJE = pagina.MENSAJE,
                                      ACCION = pagina.ACCION,
                                      CONTROLADOR = pagina.CONTROLADOR

                                  }).ToListAsync();
                }
            }

            return PartialView("_TablaPagina", list);
        }

        public async Task<string> Guardar(PaginaCLS oPaginaCLS, int operacion)
        {
            string resp = "";
            int r = 0;

            try
            {
                if (!ModelState.IsValid)
                {
                    var errores = (from state in ModelState.Values
                                   from error in state.Errors
                                   select error.ErrorMessage).ToList();

                    resp += "<ul class = 'list-group'>";
                    foreach (var item in errores)
                    {
                        resp += "<li class = 'list-group-item text-danger'>" + item + "</li>";
                    }

                    resp += "</ul>";
                }
                else
                {
                    Pagina pagina = new Pagina();

                    using (var bd = new BDPasajeEntities())
                    {
                        if (operacion == -1)
                        {
                            var existe = await bd.Pagina.AnyAsync(x => x.MENSAJE == oPaginaCLS.MENSAJE);

                            if (existe)
                            {
                                resp += "<ul class = 'list-group'>";
                                resp += "<li class = 'list-group-item text-danger'>Esa Página ya existe en la BD..</li>";
                                resp += "</ul>";
                            }
                            else
                            {
                                pagina.MENSAJE = oPaginaCLS.MENSAJE;
                                pagina.ACCION = oPaginaCLS.ACCION;
                                pagina.CONTROLADOR = oPaginaCLS.CONTROLADOR;
                                pagina.BHABILITADO = 1;

                                bd.Pagina.Add(pagina);
                                r = await bd.SaveChangesAsync();
                                if (r == 0) resp = "";
                                else resp = r.ToString();
                            }

                        }
                        else
                        {
                            var existe = await bd.Pagina.AnyAsync(x => x.MENSAJE == oPaginaCLS.MENSAJE && x.IIDPAGINA != operacion);

                            if (existe)
                            {
                                resp += "<ul class = 'list-group'>";
                                resp += "<li class = 'list-group-item text-danger'>Esa Página ya existe en la BD..</li>";
                                resp += "</ul>";
                            }
                            else
                            {
                                pagina = await bd.Pagina.FirstOrDefaultAsync(x => x.IIDPAGINA == operacion);
                                pagina.MENSAJE = oPaginaCLS.MENSAJE;
                                pagina.ACCION = oPaginaCLS.ACCION;
                                pagina.CONTROLADOR = oPaginaCLS.CONTROLADOR;

                                r = await bd.SaveChangesAsync();
                                resp = r.ToString();
                            }
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
            PaginaCLS paginaCLS = new PaginaCLS();

            using (var bd = new BDPasajeEntities())
            {
                Pagina pagina = await bd.Pagina.FirstOrDefaultAsync(x => x.IIDPAGINA == id);

                paginaCLS.ACCION = pagina.ACCION;
                paginaCLS.CONTROLADOR = pagina.CONTROLADOR;
                paginaCLS.MENSAJE = pagina.MENSAJE;
            }

            return Json(paginaCLS, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> Delete(int idEliminar)
        {
            int r = 0;
            string resp = "";

            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    Pagina pagina = await bd.Pagina.FirstOrDefaultAsync(x => x.IIDPAGINA == idEliminar);
                    pagina.BHABILITADO = 0;

                    r = await bd.SaveChangesAsync();
                    resp = r.ToString();
                }
            }
            catch (Exception ex)
            {
                resp = "";
            }

            return resp;
        }
    }
}