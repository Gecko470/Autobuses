
using Autobuses.Clases;
using Autobuses.Helpers;
using Autobuses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Autobuses.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<string> Login(UsuarioCLS usuarioCLS)
        {
            string resp = "";

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
                    SHA256Managed sha = new SHA256Managed();
                    byte[] passBytes = Encoding.Default.GetBytes(usuarioCLS.CONTRA);
                    string pass = BitConverter.ToString(sha.ComputeHash(passBytes)).Replace("-", "");

                    var user = await bd.Usuario.FirstOrDefaultAsync(x => x.NOMBREUSUARIO == usuarioCLS.NOMBREUSUARIO && x.CONTRA == pass);

                    if (user != null)
                    {
                        resp = "";
                        Session["user"] = user;

                        if (user.TIPOUSUARIO == "C")
                        {
                            Cliente cliente = await bd.Cliente.FirstOrDefaultAsync(x => x.IIDCLIENTE == user.IID);
                            Session["Nombre"] = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO;
                        }
                        else
                        {
                            Empleado empleado = await bd.Empleado.FirstOrDefaultAsync(x => x.IIDEMPLEADO == user.IID);
                            Session["Nombre"] = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO;
                        }

                        List<MenuCLS> lista = await (from usuario in bd.Usuario
                                                     join rolPag in bd.RolPagina
                                                     on usuario.IIDROL equals rolPag.IIDROL
                                                     join pagina in bd.Pagina
                                                     on rolPag.IIDPAGINA equals pagina.IIDPAGINA
                                                     where usuario.IIDUSUARIO == user.IIDUSUARIO && usuario.IIDROL == user.IIDROL && rolPag.BHABILITADO == 1
                                                     select new MenuCLS()
                                                     {
                                                         Mensaje = pagina.MENSAJE,
                                                         Accion = pagina.ACCION,
                                                         Controlador = pagina.CONTROLADOR

                                                     }).ToListAsync();

                        Session["paginas"] = lista;
                    }
                    else
                    {
                        resp += "<ul class='list-group'>";
                        resp += "<li class='list-group-item text-danger'>Usuario/Password no válidos..</li>";
                        resp += "</ul>";
                    }
                }
            }

            return resp;
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["paginas"] = null;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<string> Password(string nombreUsuario, string email, string tlfMovil)
        {
            string resp = "";
            Cliente cliente = new Cliente();

            using (var bd = new BDPasajeEntities())
            {
                cliente = await bd.Cliente.FirstOrDefaultAsync(x => x.EMAIL == email && x.TELEFONOCELULAR == tlfMovil);

                if (cliente == null) resp = "No existe un usuario en la BD con esos datos..";
                else
                {
                    Usuario usuario = await bd.Usuario.FirstOrDefaultAsync(x => x.IID == cliente.IIDCLIENTE && x.TIPOUSUARIO == "C" && x.NOMBREUSUARIO == nombreUsuario);

                    if (usuario == null)
                    {
                        resp = "No existe un usuario en la BD con esos datos..";
                    }
                    else
                    {
                        Random random = new Random();
                        int n1 = random.Next(0, 9);
                        int n2 = random.Next(0, 9);
                        int n3 = random.Next(0, 9);
                        int n4 = random.Next(0, 9);
                        string nuevoPass = n1.ToString() + n2.ToString() + n3.ToString() + n4.ToString();

                        SHA256Managed sha = new SHA256Managed();
                        byte[] passBytes = Encoding.Default.GetBytes(nuevoPass);
                        byte[] passCifradoBytes = sha.ComputeHash(passBytes);
                        string pass = BitConverter.ToString(passCifradoBytes).Replace("-", "");
                        usuario.CONTRA = pass;
                        await bd.SaveChangesAsync();

                        Mail.Send(email, "Recuperar password", $"Hemos generado un nuevo password para tu cuenta: {nuevoPass}");
                    }

                }
            }

            return resp;
        }
    }
}