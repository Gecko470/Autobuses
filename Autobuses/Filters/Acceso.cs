
using Autobuses.Clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Autobuses.Filters
{
    internal class Acceso : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool existe = false;

            var user = HttpContext.Current.Session["user"];
            List<MenuCLS> paginas = (List<MenuCLS>) HttpContext.Current.Session["paginas"];

            var controlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var accion = filterContext.ActionDescriptor.ActionName;
            
            if(user != null && paginas != null)
            {
                existe = paginas.Any(x => x.Controlador == controlador);
            }
            
            if (user == null || !existe)
            {
                 filterContext.Result = new RedirectResult("/Login/Logout");
            }
        }
    }
}