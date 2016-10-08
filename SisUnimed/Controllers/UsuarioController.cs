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

                    var md = from a in lu.usuarios                             
                             select new ResultadoLista
                             {
                                 id = a.id,
                                 id_grupo = a.id_grupo,
                                 id_operadora = a.id_operadora,
                                 nome_usuario = a.nome_usuario,
                                 senha_usuario = a.senha_usuario,
                                 email_usuario = a.email_usuario,
                                 nome_grupo = a.grupo.nome_grupo,
                                 nome_operadora = a.operadora.nome_operadora
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
                    ViewData["ListaUsuario"] = md.ToList();
                    ViewBag.Message = TempData["mensagem"];
                    TempData["mensagem"] = "";
                    return View();
                }


            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
                    
        public ActionResult PreencheCampos(int id)
        {
            using (UnimedEntities1 lu = new UnimedEntities1())
            {
                ViewBag.Message = "";
                int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                var resultado = lu.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();
                ViewData["usuario_permissao"] = resultado;
                var md = from a in lu.usuarios                         
                         select new ResultadoLista
                         {
                             id = a.id,
                             id_grupo = a.id_grupo,
                             id_operadora = a.id_operadora,
                             nome_usuario = a.nome_usuario,
                             senha_usuario = a.senha_usuario,
                             email_usuario = a.email_usuario,
                             nome_grupo = a.grupo.nome_grupo,
                             nome_operadora = a.operadora.nome_operadora
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
                ViewData["ListaUsuario"] = md.ToList();

                var dados = (lu.usuarios.Where(a => a.id.Equals(id))).FirstOrDefault();
                ViewData["usuario"] = dados;

                var usuPermissao = (from up in lu.usuario_permissao
                                    where up.id_usuario == id
                                    select up).FirstOrDefault();
                
                var vDetalheUsuarioPermissao = new ViewModelDetalhePermisao
                {
                    VusuarioPermissao = usuPermissao,
                    Vusuario = dados

                };
                ViewData["usoPermissao"] = usuPermissao;
                if (TempData["mensagem"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagem"];
                    TempData["mensagem"] = string.Empty;
                }
                ViewBag.Action = "Editar";               
               
                return View("Usuario",vDetalheUsuarioPermissao);
                             
            }            
        }
        
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 lu = new UnimedEntities1())
            {
                var usuario_id = int.Parse(Session["usuariologadoid"].ToString());
                var usuario_d = lu.usuario_permissao.Where(a => a.usuario_d.Equals(1) && a.id_usuario.Equals(usuario_id)).Count();
                if (usuario_d >= 1)
                {
                    usuario usuario = lu.usuarios.Find(id);
                    lu.usuarios.Remove(usuario);
                    lu.SaveChanges();
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Usuário Excluído com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Você não Tem Permissão para Excluir</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Usuario");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(Models.ViewModelDetalhePermisao u)
        {
            if (ModelState.IsValid) {
                using  (UnimedEntities1 lu = new UnimedEntities1())
                {
                    var id = int.Parse(Session["usuariologadoid"].ToString());
                    var up = lu.usuario_permissao.Where(a => a.usuario_i.Equals(id)).FirstOrDefault();
                    if (up.usuario_i != 1)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão</font>";
                    } else {
                        u.Vusuario.nome_usuario = u.Vusuario.nome_usuario.ToUpper();
                        u.Vusuario.email_usuario = u.Vusuario.email_usuario.ToLower();
                        lu.usuarios.Add(u.Vusuario);
                        lu.SaveChanges();
                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Usuário Salvo com Sucesso!</font>";
                    }
                
                }
                ViewBag.Action = "";
                return RedirectToAction("PreencheCampos", new { id = u.Vusuario.id });
            }
            using (UnimedEntities1 lu = new UnimedEntities1())
            {
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
                int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                var resultado = lu.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();
                ViewData["usuario_permissao"] = resultado;
                var md = from a in lu.usuarios
                         select new ResultadoLista
                         {
                             id = a.id,
                             id_grupo = a.id_grupo,
                             id_operadora = a.id_operadora,
                             nome_usuario = a.nome_usuario,
                             senha_usuario = a.senha_usuario,
                             email_usuario = a.email_usuario,
                             nome_grupo = a.grupo.nome_grupo,
                             nome_operadora = a.operadora.nome_operadora
                         };
                ViewData["ListaUsuario"] = md.ToList();               

                var usuPermissao = (from up in lu.usuario_permissao
                                    where up.id_usuario == u.Vusuario.id
                                    select up).FirstOrDefault();
                
               
                ViewData["usoPermissao"] = usuPermissao;

            }
            ViewBag.Action = "Inserir";
            return View("Usuario", u);
        }
        public ActionResult Incluir()
        {
            TempData["mensagem"] = "";
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
                             select new ResultadoLista
                             {
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
                    usuario usuario = new usuario();
                    usuario_permissao usupermissao = new usuario_permissao();
                    var vDetalheUsuarioPermissao = new ViewModelDetalhePermisao
                    {
                        VusuarioPermissao = usupermissao,
                        Vusuario = usuario

                    };
                    ViewData["listagrupo"] = gp.ToList();
                    ViewData["ListaUsuario"] = md.ToList();
                    ViewBag.Action = "Inserir";
                    return View("Usuario",vDetalheUsuarioPermissao);
                }
           
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Models.ViewModelDetalhePermisao u)
        {
            using (UnimedEntities1 lu = new UnimedEntities1())
            {
                var id = int.Parse(Session["usuariologadoid"].ToString());
                var up = lu.usuario_permissao.Where(a => a.id_usuario.Equals(id)).FirstOrDefault();
                if (up.usuario_i != 1)
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão</font>";
                }
                else
                {
                    if (ModelState.IsValid) {
                        usuario usuario = lu.usuarios.Find(u.Vusuario.id);
                        usuario.id_grupo = u.Vusuario.id_grupo;
                        usuario.id_operadora = u.Vusuario.id_operadora;
                        usuario.nome_usuario = u.Vusuario.nome_usuario.ToUpper();
                        usuario.email_usuario = u.Vusuario.email_usuario.ToLower();
                        usuario.senha_usuario = u.Vusuario.senha_usuario;
                    

                        if (TryUpdateModel(usuario))
                        {
                            lu.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Usuário Salvo com Sucesso!</font>";
                        } else {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Error ao Atualizar Usuário</font>";
                        }
                    }
                }

            }
            ViewBag.Action = "";
            return RedirectToAction("Usuario"); ;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPermissao(Models.ViewModelDetalhePermisao u)
        {
            using (UnimedEntities1 lu = new UnimedEntities1())
            {
                var id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = lu.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                if (up.usuario_permissao_a != 1)
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão</font>";
                }
                else
                {
                    usuario_permissao usuariopermissao = lu.usuario_permissao.Find(u.VusuarioPermissao.id);
                    usuariopermissao.operadora = u.VusuarioPermissao.operadora;
                    usuariopermissao.operadora_i = u.VusuarioPermissao.operadora_i;
                    usuariopermissao.operadora_a = u.VusuarioPermissao.operadora_a;
                    usuariopermissao.operadora_d = u.VusuarioPermissao.operadora_d;

                    usuariopermissao.usuario = u.VusuarioPermissao.usuario;
                    usuariopermissao.usuario_i = u.VusuarioPermissao.usuario_i;
                    usuariopermissao.usuario_a = u.VusuarioPermissao.usuario_a;
                    usuariopermissao.usuario_d = u.VusuarioPermissao.usuario_d;

                    usuariopermissao.grupo = u.VusuarioPermissao.grupo;
                    usuariopermissao.grupo_i = u.VusuarioPermissao.grupo_i;
                    usuariopermissao.grupo_a = u.VusuarioPermissao.grupo_a;
                    usuariopermissao.grupo_d = u.VusuarioPermissao.grupo_d;

                    usuariopermissao.usuario_permissao1 = u.VusuarioPermissao.usuario_permissao1;
                    usuariopermissao.usuario_permissao_i = u.VusuarioPermissao.usuario_permissao_i;
                    usuariopermissao.usuario_permissao_a = u.VusuarioPermissao.usuario_permissao_a;
                    usuariopermissao.usuario_permissao_d = u.VusuarioPermissao.usuario_permissao_d;

                    usuariopermissao.grupo_permissao = u.VusuarioPermissao.grupo_permissao;
                    usuariopermissao.grupo_permissao_i = u.VusuarioPermissao.grupo_permissao_i;
                    usuariopermissao.grupo_permissao_a = u.VusuarioPermissao.grupo_permissao_a;
                    usuariopermissao.grupo_permissao_d = u.VusuarioPermissao.grupo_permissao_d;
                    if (TryUpdateModel(usuariopermissao))
                    {
                        lu.SaveChanges();
                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Usuário Salvo com Sucesso!</font>";
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Alterar as Permissões</font>";
                    }
                }

            }
            ViewBag.Action = "";
            return RedirectToAction("Usuario"); ;
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 lu = new UnimedEntities1())
            {
                
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = lu.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();
                    ViewData["usuario_permissao"] = resultado;

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
                    ViewData["ListaGrupo"] = gp.ToList();
                
                if (campo == "codigo")
                {


                    var codigo = int.Parse(pesquisa);
                    var md = from a in lu.usuarios
                             where a.id == codigo
                             select new ResultadoLista
                             {
                                 id = a.id,
                                 id_grupo = a.id_grupo,
                                 id_operadora = a.id_operadora,
                                 nome_usuario = a.nome_usuario,
                                 senha_usuario = a.senha_usuario,
                                 email_usuario = a.email_usuario,
                                 nome_grupo = a.grupo.nome_grupo,
                                 nome_operadora = a.operadora.nome_operadora
                             };
                    ViewData["ListaUsuario"] = md.ToList();
                    
                    


                }
                if (campo == "nome")
                {
                    if (tipo == "contem")
                    {
                        var md = from a in lu.usuarios
                                 where a.nome_usuario.Contains(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                    if (tipo == "inicia")
                    {
                        var md = from a in lu.usuarios
                                 where a.nome_usuario.StartsWith(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();

                    }
                    if (tipo == "termina")
                    {
                        var md = from a in lu.usuarios
                                 where a.nome_usuario.EndsWith(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                };
                if (campo == "email")
                {
                    if (tipo == "contem")
                    {
                        var md = from a in lu.usuarios
                                 where a.email_usuario.Contains(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                    if (tipo == "inicia")
                    {
                        var md = from a in lu.usuarios
                                 where a.email_usuario.StartsWith(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                    if (tipo == "termina")
                    {
                        var md = from a in lu.usuarios
                                 where a.email_usuario.EndsWith(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                }
                if (campo == "operadora")
                {
                    if (tipo == "contem")
                    {
                        var md = from a in lu.usuarios
                                 where a.operadora.nome_operadora.Contains(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                    if (tipo == "inicia")
                    {
                        var md = from a in lu.usuarios
                                 where a.operadora.nome_operadora.StartsWith(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                    if (tipo == "termina")
                    {
                        var md = from a in lu.usuarios
                                 where a.operadora.nome_operadora.EndsWith(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                }
                if (campo == "grupo")
                {
                    if (tipo == "contem")
                    {
                        var md = from a in lu.usuarios
                                 where a.grupo.nome_grupo.Contains(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                    if (tipo == "inicia")
                    {
                        var md = from a in lu.usuarios
                                 where a.grupo.nome_grupo.StartsWith(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                    if (tipo == "termina")
                    {
                        var md = from a in lu.usuarios
                                 where a.grupo.nome_grupo.EndsWith(pesquisa)
                                 select new ResultadoLista
                                 {
                                     id = a.id,
                                     id_grupo = a.id_grupo,
                                     id_operadora = a.id_operadora,
                                     nome_usuario = a.nome_usuario,
                                     senha_usuario = a.senha_usuario,
                                     email_usuario = a.email_usuario,
                                     nome_grupo = a.grupo.nome_grupo,
                                     nome_operadora = a.operadora.nome_operadora
                                 };
                        ViewData["ListaUsuario"] = md.ToList();
                    }
                }
                
            }
            
            return PartialView("ListaUsuario");
        }

    }
}
