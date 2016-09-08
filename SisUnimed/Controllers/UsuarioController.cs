using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    [Serializable]    
    public class UsuarioController : Controller
    {
        //
        // GET: /Usuario/

        public ActionResult Usuario()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();
                    ViewData["usuario_permissao"] = resultado;
                    
                    
                }
                ViewBag.Titulo = "Cadastro de Usuário";

                using (UnimedEntities1 lu = new UnimedEntities1())
                {
                    var md = (from a in lu.usuarios select a).ToList();
                    ViewData["LitaUsuario"] = md;
                    return View(md);
                }


            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

    }
}
