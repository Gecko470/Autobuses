using Autobuses.Clases;
using Autobuses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Autobuses.Filters;

namespace Autobuses.Controllers
{
    [Acceso]
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public async Task<ActionResult> Index()
        {
            await personas();
            await roles();

            List<UsuarioCLS> listUsuarioClientes = new List<UsuarioCLS>();
            List<UsuarioCLS> listUsuarioEmpleados = new List<UsuarioCLS>();

            List<UsuarioCLS> list = new List<UsuarioCLS>();

            using (var bd = new BDPasajeEntities())
            {
                listUsuarioClientes = await (from usuario in bd.Usuario
                                             join cliente in bd.Cliente
                                             on usuario.IID equals cliente.IIDCLIENTE
                                             join rol in bd.Rol
                                             on usuario.IIDROL equals rol.IIDROL
                                             where usuario.bhabilitado == 1 && usuario.TIPOUSUARIO == "C"
                                             select new UsuarioCLS()
                                             {
                                                 IIDUSUARIO = usuario.IIDUSUARIO,
                                                 NOMBREUSUARIO = usuario.NOMBREUSUARIO,
                                                 Persona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                 TIPOUSUARIO = "Cliente",
                                                 Rol = rol.NOMBRE

                                             }).ToListAsync();

                listUsuarioEmpleados = await (from usuario in bd.Usuario
                                              join empleado in bd.Empleado
                                              on usuario.IID equals empleado.IIDEMPLEADO
                                              join rol in bd.Rol
                                              on usuario.IIDROL equals rol.IIDROL
                                              where usuario.bhabilitado == 1 && usuario.TIPOUSUARIO == "E"
                                              select new UsuarioCLS()
                                              {
                                                  IIDUSUARIO = usuario.IIDUSUARIO,
                                                  NOMBREUSUARIO = usuario.NOMBREUSUARIO,
                                                  Persona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                  TIPOUSUARIO = "Empleado",
                                                  Rol = rol.NOMBRE

                                              }).ToListAsync();
            }

            list.AddRange(listUsuarioEmpleados);
            list.AddRange(listUsuarioClientes);
            list = list.OrderBy(x => x.IIDUSUARIO).ToList();

            return View(list);
        }

        public async Task<ActionResult> Filtrar(string nombre)
        {
            await personas();
            await roles();

            List<UsuarioCLS> listUsuarioClientes = new List<UsuarioCLS>();
            List<UsuarioCLS> listUsuarioEmpleados = new List<UsuarioCLS>();

            List<UsuarioCLS> list = new List<UsuarioCLS>();

            using (var bd = new BDPasajeEntities())
            {
                if (string.IsNullOrEmpty(nombre))
                {
                    listUsuarioClientes = await (from usuario in bd.Usuario
                                                 join cliente in bd.Cliente
                                                 on usuario.IID equals cliente.IIDCLIENTE
                                                 join rol in bd.Rol
                                                 on usuario.IIDROL equals rol.IIDROL
                                                 where usuario.bhabilitado == 1 && usuario.TIPOUSUARIO == "C"
                                                 select new UsuarioCLS()
                                                 {
                                                     IIDUSUARIO = usuario.IIDUSUARIO,
                                                     NOMBREUSUARIO = usuario.NOMBREUSUARIO,
                                                     Persona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                     TIPOUSUARIO = "Cliente",
                                                     Rol = rol.NOMBRE

                                                 }).ToListAsync();

                    listUsuarioEmpleados = await (from usuario in bd.Usuario
                                                  join empleado in bd.Empleado
                                                  on usuario.IID equals empleado.IIDEMPLEADO
                                                  join rol in bd.Rol
                                                  on usuario.IIDROL equals rol.IIDROL
                                                  where usuario.bhabilitado == 1 && usuario.TIPOUSUARIO == "E"
                                                  select new UsuarioCLS()
                                                  {
                                                      IIDUSUARIO = usuario.IIDUSUARIO,
                                                      NOMBREUSUARIO = usuario.NOMBREUSUARIO,
                                                      Persona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                      TIPOUSUARIO = "Empleado",
                                                      Rol = rol.NOMBRE

                                                  }).ToListAsync();
                }
                else
                {
                    listUsuarioClientes = await (from usuario in bd.Usuario
                                                 join cliente in bd.Cliente
                                                 on usuario.IID equals cliente.IIDCLIENTE
                                                 join rol in bd.Rol
                                                 on usuario.IIDROL equals rol.IIDROL
                                                 where usuario.bhabilitado == 1 && usuario.TIPOUSUARIO == "C" && (cliente.NOMBRE.Contains(nombre) || cliente.APPATERNO.Contains(nombre) || cliente.APMATERNO.Contains(nombre))
                                                 select new UsuarioCLS()
                                                 {
                                                     IIDUSUARIO = usuario.IIDUSUARIO,
                                                     NOMBREUSUARIO = usuario.NOMBREUSUARIO,
                                                     Persona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                     TIPOUSUARIO = "Cliente",
                                                     Rol = rol.NOMBRE

                                                 }).ToListAsync();

                    listUsuarioEmpleados = await (from usuario in bd.Usuario
                                                  join empleado in bd.Empleado
                                                  on usuario.IID equals empleado.IIDEMPLEADO
                                                  join rol in bd.Rol
                                                  on usuario.IIDROL equals rol.IIDROL
                                                  where usuario.bhabilitado == 1 && usuario.TIPOUSUARIO == "E" && (empleado.NOMBRE.Contains(nombre) || empleado.APPATERNO.Contains(nombre) || empleado.APMATERNO.Contains(nombre))
                                                  select new UsuarioCLS()
                                                  {
                                                      IIDUSUARIO = usuario.IIDUSUARIO,
                                                      NOMBREUSUARIO = usuario.NOMBREUSUARIO,
                                                      Persona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                      TIPOUSUARIO = "Empleado",
                                                      Rol = rol.NOMBRE

                                                  }).ToListAsync();
                }

            }

            list.AddRange(listUsuarioEmpleados);
            list.AddRange(listUsuarioClientes);
            list = list.OrderBy(x => x.IIDUSUARIO).ToList();

            return PartialView("_TablaUsuario", list);
        }

        public async Task<string> Guardar(int operacion, string persona, UsuarioCLS oUsuarioCLS)
        {
            string resp = "";

            Usuario usuario = new Usuario();

            try
            {
                if (!ModelState.IsValid)
                {
                    var errores = (from state in ModelState.Values
                                   from error in state.Errors
                                   select error.ErrorMessage).ToList();

                    resp += "<ul class='list-group'>";
                    foreach (var item in errores)
                    {
                        resp += "<li class='list-group-item text-danger'>" + item + "</li>";
                    }
                    resp += "</ul>";
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        using (var bd = new BDPasajeEntities())
                        {

                            if (operacion == -1)
                            {
                                var existe = await bd.Usuario.AnyAsync(x => x.NOMBREUSUARIO == oUsuarioCLS.NOMBREUSUARIO);
                                if (existe)
                                {
                                    resp += "<ul class='list-group'>";
                                    resp += "<li class='list-group-item text-danger'>Ya existe ese Usuario en la BD..</li>";
                                    resp += "</ul>";
                                }
                                else
                                {
                                    usuario.NOMBREUSUARIO = oUsuarioCLS.NOMBREUSUARIO;
                                    SHA256Managed sha = new SHA256Managed();
                                    byte[] passBytes = Encoding.Default.GetBytes(oUsuarioCLS.CONTRA);
                                    string pass = BitConverter.ToString(sha.ComputeHash(passBytes)).Replace("-", "");
                                    usuario.CONTRA = pass;
                                    usuario.TIPOUSUARIO = persona.Substring(persona.Length - 2, 1);
                                    usuario.IID = oUsuarioCLS.IID;
                                    usuario.IIDROL = oUsuarioCLS.IIDROL;
                                    usuario.bhabilitado = 1;

                                    bd.Usuario.Add(usuario);

                                    if (usuario.TIPOUSUARIO == "C")
                                    {
                                        Cliente cliente = await bd.Cliente.FirstOrDefaultAsync(x => x.IIDCLIENTE == oUsuarioCLS.IID);
                                        cliente.bTieneUsuario = 1;
                                    }
                                    else
                                    {
                                        Empleado empleado = await bd.Empleado.FirstOrDefaultAsync(x => x.IIDEMPLEADO == oUsuarioCLS.IID);
                                        empleado.bTieneUsuario = 1;
                                    }

                                    await bd.SaveChangesAsync();

                                    resp = "1";
                                }

                            }
                            else
                            {
                                var existe = await bd.Usuario.AnyAsync(x => x.NOMBREUSUARIO == oUsuarioCLS.NOMBREUSUARIO && x.IIDUSUARIO != operacion);
                                if (existe)
                                {
                                    resp += "<ul class='list-group'>";
                                    resp += "<li class='list-group-item text-danger'>Ya existe ese Usuario en la BD..</li>";
                                    resp += "</ul>";
                                }
                                else
                                {
                                    Usuario usuarioBD = await bd.Usuario.FirstOrDefaultAsync(x => x.IIDUSUARIO == operacion);

                                    usuarioBD.NOMBREUSUARIO = oUsuarioCLS.NOMBREUSUARIO;
                                    usuarioBD.IIDROL = oUsuarioCLS.IIDROL;

                                    await bd.SaveChangesAsync();
                                    resp = "1";
                                }
                            }
                        }

                        scope.Complete();
                    }

                }
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }

            return resp;
        }

        public async Task<JsonResult> Edit(int id)
        {
            UsuarioCLS usuarioCLS = new UsuarioCLS();

            using (var bd = new BDPasajeEntities())
            {
                Usuario usuario = await bd.Usuario.FirstOrDefaultAsync(x => x.IIDUSUARIO == id);

                usuarioCLS.IIDUSUARIO = usuario.IIDUSUARIO;
                usuarioCLS.NOMBREUSUARIO = usuario.NOMBREUSUARIO;
                usuarioCLS.IIDROL = (int)usuario.IIDROL;

            }
            return Json(usuarioCLS, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<string> Delete(int idEliminar)
        {
            string resp = "";
            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    Usuario usuario = await bd.Usuario.FirstOrDefaultAsync(x => x.IIDUSUARIO == idEliminar);
                    usuario.bhabilitado = 0;

                    await bd.SaveChangesAsync();

                    resp = "1";
                }
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }

            return resp;
        }

        private async Task personas()
        {
            List<SelectListItem> listaPersonas = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                List<SelectListItem> listaClientes = await (from item in bd.Cliente
                                                            where item.BHABILITADO == 1 && item.bTieneUsuario != 1
                                                            select new SelectListItem()
                                                            {
                                                                Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO + " (C)",
                                                                Value = item.IIDCLIENTE.ToString()

                                                            }).ToListAsync();

                List<SelectListItem> listaEmpleados = await (from item in bd.Empleado
                                                             where item.BHABILITADO == 1 && item.bTieneUsuario != 1
                                                             select new SelectListItem()
                                                             {
                                                                 Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO + " (E)",
                                                                 Value = item.IIDEMPLEADO.ToString()

                                                             }).ToListAsync();

                listaPersonas.AddRange(listaClientes);
                listaPersonas.AddRange(listaEmpleados);
                listaPersonas.OrderBy(x => x.Text).ToList();
                listaPersonas.Insert(0, new SelectListItem()
                {
                    Text = "Seleccione..",
                    Value = ""
                });
            }

            ViewBag.listaPersonas = listaPersonas;
        }

        private async Task roles()
        {
            List<SelectListItem> listaRoles = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                listaRoles = await (from item in bd.Rol
                                    where item.BHABILITADO == 1
                                    select new SelectListItem()
                                    {
                                        Text = item.NOMBRE,
                                        Value = item.IIDROL.ToString()

                                    }).ToListAsync();

                listaRoles.Insert(0, new SelectListItem()
                {
                    Text = "Seleccione..",
                    Value = ""
                });
            }

            ViewBag.listaRoles = listaRoles;
        }
    }
}