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
    public class RolPaginaController : Controller
    {
        // GET: RolPagina
        public async Task<ActionResult> Index()
        {
            await Roles();
            await Paginas();

            List<RolPaginaCLS> list = new List<RolPaginaCLS>();

            using (var bd = new BDPasajeEntities())
            {
                list = await (from rolPagina in bd.RolPagina
                              join rol in bd.Rol
                              on rolPagina.IIDROL equals rol.IIDROL
                              join pagina in bd.Pagina
                              on rolPagina.IIDPAGINA equals pagina.IIDPAGINA
                              where rolPagina.BHABILITADO == 1
                              select new RolPaginaCLS()
                              {
                                  IIDROLPAGINA = rolPagina.IIDROLPAGINA,
                                  Pagina = pagina.MENSAJE,
                                  Rol = rol.NOMBRE

                              }).ToListAsync();
            }
            return View(list);
        }

        public async Task<ActionResult> Filtro(int? idRol)
        {
            if(idRol == null) idRol = 0;

            List<RolPaginaCLS> list = new List<RolPaginaCLS>();

            using (var bd = new BDPasajeEntities())
            {
                if (idRol == 0)
                {
                    list = await (from rolPagina in bd.RolPagina
                                  join rol in bd.Rol
                                  on rolPagina.IIDROL equals rol.IIDROL
                                  join pagina in bd.Pagina
                                  on rolPagina.IIDPAGINA equals pagina.IIDPAGINA
                                  where rolPagina.BHABILITADO == 1
                                  select new RolPaginaCLS()
                                  {
                                      IIDROLPAGINA = rolPagina.IIDROLPAGINA,
                                      Pagina = pagina.MENSAJE,
                                      Rol = rol.NOMBRE

                                  }).ToListAsync();
                }
                else
                {
                    list = await (from rolPagina in bd.RolPagina
                                  join rol in bd.Rol
                                  on rolPagina.IIDROL equals rol.IIDROL
                                  join pagina in bd.Pagina
                                  on rolPagina.IIDPAGINA equals pagina.IIDPAGINA
                                  where rolPagina.BHABILITADO == 1 && rolPagina.IIDROL == idRol
                                  select new RolPaginaCLS()
                                  {
                                      IIDROLPAGINA = rolPagina.IIDROLPAGINA,
                                      Pagina = pagina.MENSAJE,
                                      Rol = rol.NOMBRE

                                  }).ToListAsync();
                }
            }


            return PartialView("_TablaRolPagina", list);
        }

        public async Task<string> Guardar(int operacion, RolPaginaCLS oRolPaginaCLS)
        {
            int r = 0;
            string resp = "";

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
                    RolPagina rolPagina = new RolPagina();

                    using (var bd = new BDPasajeEntities())
                    {
                        if (operacion == -1)
                        {
                            rolPagina.IIDROL = oRolPaginaCLS.IIDROL;
                            rolPagina.IIDPAGINA = oRolPaginaCLS.IIDPAGINA;
                            rolPagina.BHABILITADO = 1;

                            bd.RolPagina.Add(rolPagina);
                            r = await bd.SaveChangesAsync();
                            if (r == 0) resp = "";
                            else resp = r.ToString();
                        }
                        else
                        {
                            rolPagina = await bd.RolPagina.FirstOrDefaultAsync(x => x.IIDPAGINA == operacion);
                            rolPagina.IIDROL = oRolPaginaCLS.IIDROL;
                            rolPagina.IIDPAGINA = oRolPaginaCLS.IIDPAGINA;

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

        public async Task<JsonResult>Edit(int id)
        {
            RolPaginaCLS oRolPaginaCLS = new RolPaginaCLS();

            using(var bd = new BDPasajeEntities())
            {
                RolPagina rolPagina = await bd.RolPagina.FirstOrDefaultAsync(x => x.IIDROLPAGINA == id);

                oRolPaginaCLS.IIDPAGINA = (int) rolPagina.IIDPAGINA;
                oRolPaginaCLS.IIDROL = (int) rolPagina.IIDROL;
            }

            return Json(oRolPaginaCLS, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> Delete(int idEliminar)
        {
            int r = 0;
            string resp = "";

            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    RolPagina rolPagina = await bd.RolPagina.FirstOrDefaultAsync(x => x.IIDROLPAGINA == idEliminar);
                    rolPagina.BHABILITADO = 0;

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

        private async Task Roles()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                list = await (from rol in bd.Rol
                              where rol.BHABILITADO == 1
                              select new SelectListItem()
                              {
                                  Text = rol.NOMBRE,
                                  Value = rol.IIDROL.ToString()

                              }).ToListAsync();

                list.Insert(0, new SelectListItem()
                {
                    Text = "Seleccione..",
                    Value = ""
                });
            }

            ViewBag.listaRoles = list;
        }

        private async Task Paginas()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                list = await (from pagina in bd.Pagina
                              where pagina.BHABILITADO == 1
                              select new SelectListItem()
                              {
                                  Text = pagina.MENSAJE,
                                  Value = pagina.IIDPAGINA.ToString()

                              }).ToListAsync();

                list.Insert(0, new SelectListItem()
                {
                    Text = "Seleccione..",
                    Value = ""
                });
            }

            ViewBag.listaPaginas = list;
        }
    }
}