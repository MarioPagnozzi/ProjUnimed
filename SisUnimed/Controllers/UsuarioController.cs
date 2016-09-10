using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;
using System.Dynamic;

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
                    var md = from a in lu.usuarios
                                  join g in lu.grupoes on a.id_grupo equals g.id
                                  join o in lu.operadoras on a.id_operadora equals o.id
                                  select new ResultadoLista {
                                      id = a.id,
                                      id_grupo = a.id_grupo,
                                      id_operadora = a.id_operadora,
                                      nome_usuario = a.nome_usuario,
                                      senha_usuario = a.senha_usuario,
                                      email_usuario = a.email_usuario,
                                      nome_grupo = g.nome_grupo,
                                      nome_operadora = o.nome_operadora
                                  };
                    var op = from a in lu.operadoras
                             select new ListaOperadora
                             {
                                 cod_op = a.id,
                                 desc_op = a.nome_operadora
                             };
                    ViewData["listaOperadora"] = op.ToList();
                    var gp = from a in lu.grupoes
                             select new ListaGrupo
                             {
                                 cod_grupo = a.id,
                                 desc_grupo = a.nome_grupo
                             };
                    ViewData["listagrupo"] = gp.ToList();
                    ViewData["LitaUsuario"] = md.ToList();
                    return View();
                }


            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }        

    }
}
