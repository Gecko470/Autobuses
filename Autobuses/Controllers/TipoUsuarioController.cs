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
    public class TipoUsuarioController : Controller
    {
        private TipoUsuarioCLS oTipoVal = new TipoUsuarioCLS();
        // GET: TipoUsuario
        public async Task<ActionResult> Index(TipoUsuarioCLS oTipoUsuarioCLS)
        {
            List<TipoUsuarioCLS> list = new List<TipoUsuarioCLS>();
            List<TipoUsuarioCLS> listaFiltrada = new List<TipoUsuarioCLS>();
            

            oTipoVal = oTipoUsuarioCLS;

            using (var bd = new BDPasajeEntities())
            {
                list = await (from tipoUsuario in bd.TipoUsuario
                              where tipoUsuario.BHABILITADO == 1
                              select new TipoUsuarioCLS()
                              {
                                  IIDTIPOUSUARIO = tipoUsuario.IIDTIPOUSUARIO,
                                  NOMBRE = tipoUsuario.NOMBRE,
                                  DESCRIPCION = tipoUsuario.DESCRIPCION

                              }).ToListAsync();

                if (oTipoUsuarioCLS.IIDTIPOUSUARIO == 0 && string.IsNullOrEmpty(oTipoUsuarioCLS.NOMBRE) && string.IsNullOrEmpty(oTipoUsuarioCLS.DESCRIPCION))
                {
                    listaFiltrada = list;
                }
                else
                {
                    Predicate<TipoUsuarioCLS> pred = new Predicate<TipoUsuarioCLS>(buscarTipoUsuario);

                    listaFiltrada = list.FindAll(pred);

                }
            }
            return View(listaFiltrada);
        }

        private bool buscarTipoUsuario(TipoUsuarioCLS oTipoUsuarioCLS)
        {
            bool busquedaId = true;
            bool busquedaNombre = true;
            bool busquedaDescripcion = true;

           if(oTipoVal.IIDTIPOUSUARIO != 0)
            {
                busquedaId = oTipoUsuarioCLS.IIDTIPOUSUARIO.ToString().Contains(oTipoVal.IIDTIPOUSUARIO.ToString());
            }

            if(!string.IsNullOrEmpty(oTipoVal.NOMBRE))
            {
                busquedaNombre = oTipoUsuarioCLS.NOMBRE.Contains(oTipoVal.NOMBRE);
            }

             if(!string.IsNullOrEmpty(oTipoVal.DESCRIPCION))
            {
                busquedaDescripcion = oTipoUsuarioCLS.DESCRIPCION.Contains(oTipoVal.DESCRIPCION);
            }

             return (busquedaId && busquedaNombre && busquedaDescripcion);
        }
    }
}