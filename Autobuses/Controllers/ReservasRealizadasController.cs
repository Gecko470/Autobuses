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
    public class ReservasRealizadasController : Controller
    {
        // GET: ReservasRealizadas
        public async Task<ActionResult> Index()
        { 
            Usuario usuario = (Usuario)Session["user"];
            List<ReservaRealizadaCLS> list = new List<ReservaRealizadaCLS>();

            using(var bd = new BDPasajeEntities())
            {
                list = await (from venta in bd.VENTA
                              where venta.BHABILITADO == 1 && venta.IIDUSUARIO == usuario.IIDUSUARIO
                              select new ReservaRealizadaCLS()
                              {
                                  iidVenta = venta.IIDVENTA,
                                  fechaVenta = (DateTime) venta.FECHAVENTA,
                                  total = (decimal) venta.TOTAL

                              }).ToListAsync();
            }
            return View(list);
        }
    }
}