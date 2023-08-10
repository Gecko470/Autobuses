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
    public class RolController : Controller
    {
        // GET: Rol
        public async Task<ActionResult> Index()
        {
            List<RolCLS> list = new List<RolCLS>();

            using (var bd = new BDPasajeEntities())
            {
                list = await (from rol in bd.Rol
                              where rol.BHABILITADO == 1
                              select new RolCLS()
                              {
                                  IIDROL = rol.IIDROL,
                                  NOMBRE = rol.NOMBRE,
                                  DESCRIPCION = rol.DESCRIPCION

                              }).ToListAsync();
            }
            return View(list);
        }

        public async Task<ActionResult> Filtro(string nombreRol)
        {
            List<RolCLS> list = new List<RolCLS>();

            using (var bd = new BDPasajeEntities())
            {
                if (string.IsNullOrEmpty(nombreRol))
                {
                    list = await (from rol in bd.Rol
                                  where rol.BHABILITADO == 1
                                  select new RolCLS()
                                  {
                                      IIDROL = rol.IIDROL,
                                      NOMBRE = rol.NOMBRE,
                                      DESCRIPCION = rol.DESCRIPCION

                                  }).ToListAsync();
                }
                else
                {
                    list = await (from rol in bd.Rol
                                  where rol.BHABILITADO == 1 && rol.NOMBRE.Contains(nombreRol)
                                  select new RolCLS()
                                  {
                                      IIDROL = rol.IIDROL,
                                      NOMBRE = rol.NOMBRE,
                                      DESCRIPCION = rol.DESCRIPCION

                                  }).ToListAsync();
                }
            }

            return PartialView("_TablaRol", list);
        }

        public async Task<string> Guardar(RolCLS oRolCLS, int operacion)
        {
            int r = 0;
            string resp = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();

                    resp += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        resp += "<li class='list-group-item text-danger'>" + item + "</li>";
                    }
                    resp += "</ul>";
                }
                else
                {
                    using (var bd = new BDPasajeEntities())
                    {
                        if (operacion == -1)
                        {
                            Rol rol = new Rol();
                            rol.NOMBRE = oRolCLS.NOMBRE;
                            rol.DESCRIPCION = oRolCLS.DESCRIPCION;
                            rol.BHABILITADO = 1;

                            bd.Rol.Add(rol);
                            r = await bd.SaveChangesAsync();
                            if (r == 0) resp = "";
                            else resp = r.ToString();
                        }
                        else
                        {
                            Rol rol = await bd.Rol.FirstOrDefaultAsync(x => x.IIDROL == operacion);
                            rol.NOMBRE = oRolCLS.NOMBRE;
                            rol.DESCRIPCION = oRolCLS.DESCRIPCION;

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
            RolCLS oRolCLS = new RolCLS();

            using (var bd = new BDPasajeEntities())
            {
                Rol rol = await bd.Rol.FirstOrDefaultAsync(x => x.IIDROL == id);

                oRolCLS.NOMBRE = rol.NOMBRE;
                oRolCLS.DESCRIPCION = rol.DESCRIPCION;

            }
            return Json(oRolCLS, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> Delete(int idEliminar)
        {
            int r = 0;
            string resp = "";

            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    Rol rol = await bd.Rol.FirstOrDefaultAsync(x => x.IIDROL == idEliminar);
                    rol.BHABILITADO = 0;

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