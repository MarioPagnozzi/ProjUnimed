using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class GrupoController : Controller
    {
        //
        // GET: /Grupo/

        public ActionResult Grupo()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca grupos na tabela
                    var grupos = from a in up.grupoes
                                  select new ListaGrupo
                                  {
                                      cod_grupo = a.id,
                                      desc_grupo = a.nome_grupo
                                  };
                    ViewData["listagrupo"] = grupos.ToList();
                }
                ViewBag.Titulo = "Cadastro de Grupo";
                ViewBag.Message = TempData["mensagem"];
                TempData["mensagem"] = "";
                return View();


            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult PreencheCampos(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosgrupo = dg.grupoes.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosgrupopermissao = dg.grupo_permissao.Where(a => a.id_grupo.Equals(id)).FirstOrDefault();
                var VDetalhePermissaoGrupo = new ViewModelDetalhePermisaoGrupo
                {
                    Vgrupo = dadosgrupo,
                    Vgrupo_permissao = dadosgrupopermissao
                };
                //carrega lista de grupo
                var listagrupo = from a in dg.grupoes
                                 select new ListaGrupo
                                 {
                                     cod_grupo = a.id,
                                     desc_grupo = a.nome_grupo
                                 };
                ViewData["listagrupo"] = listagrupo.ToList();

                //atualiza permissao de usuários
                var id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                if (TempData["mensagem"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagem"];
                    TempData["mensagem"] = string.Empty;
                }

                //Altera status para editar
                ViewBag.Action = "Editar";
                return View("Grupo",VDetalhePermissaoGrupo);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.grupoes
                             select new ListaGrupo
                             {
                                 cod_grupo = a.id,
                                 desc_grupo = a.nome_grupo
                             };
                    ViewData["listagrupo"] = lg.ToList();
                }
                if (campo == "codigo")
                {
                    int idgrupo = int.Parse(pesquisa);
                    var lg = from a in dg.grupoes
                             where a.id == idgrupo
                             select new ListaGrupo
                             {
                                 cod_grupo = a.id,
                                 desc_grupo = a.nome_grupo
                             };
                    ViewData["listagrupo"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia") {
                        var lg = from a in dg.grupoes
                                 where a.nome_grupo.StartsWith(pesquisa)
                                 select new ListaGrupo
                                 {
                                     cod_grupo = a.id,
                                     desc_grupo = a.nome_grupo
                                 };
                        ViewData["listagrupo"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.grupoes
                                 where a.nome_grupo.EndsWith(pesquisa)
                                 select new ListaGrupo
                                 {
                                     cod_grupo = a.id,
                                     desc_grupo = a.nome_grupo
                                 };
                        ViewData["listagrupo"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.grupoes
                                 where a.nome_grupo.Contains(pesquisa)
                                 select new ListaGrupo
                                 {
                                     cod_grupo = a.id,
                                     desc_grupo = a.nome_grupo
                                 };
                        ViewData["listagrupo"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaGrupo");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
               
                    ViewBag.Titulo = "Cadastro de Usuário";

                    //carrega lista de grupo
                    var lg = from a in dg.grupoes
                             select new ListaGrupo
                             {
                                 cod_grupo = a.id,
                                 desc_grupo = a.nome_grupo
                             };
                    ViewData["listagrupo"] = lg.ToList();

                    //prepara model para inserção
                    var dadosgrupo = new grupo();
                    var dadospermissaogrupo = new grupo_permissao();
                    var VDetalheGrupo = new ViewModelDetalhePermisaoGrupo()
                    {
                        Vgrupo = dadosgrupo,
                        Vgrupo_permissao = dadospermissaogrupo
                    };

                    ViewBag.Action = "Inserir";                    
               
                return View("Grupo", VDetalheGrupo);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(Models.ViewModelDetalhePermisaoGrupo u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.grupo_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.Vgrupo.nome_grupo = (u.Vgrupo.nome_grupo.ToUpper());                            
                            dg.grupoes.Add(u.Vgrupo);
                            dg.SaveChanges();
                            
                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.grupoes
                                     select new ListaGrupo
                                     {
                                         cod_grupo = a.id,
                                         desc_grupo = a.nome_grupo
                                     };
                            ViewData["listagrupo"] = lg.ToList();
                            ViewBag.Titulo = "Cadastro de Grupo";
                            return RedirectToAction("Grupo", u);
                        }
                        
                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Grupo Inserido com Sucesso!</font>";
                        ViewBag.Action = "";

                        return RedirectToAction("PreencheCampos", new { id = u.Vgrupo.id});

                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.grupoes
                         select new ListaGrupo
                         {
                             cod_grupo = a.id,
                             desc_grupo = a.nome_grupo
                         };
                ViewData["listagrupo"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Grupo";
            return View("Grupo", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPermissao(Models.ViewModelDetalhePermisaoGrupo u)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.grupo_permissao_a.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                if (up >= 1)
                {
                    grupo_permissao grupopermissao = dg.grupo_permissao.Find(u.Vgrupo_permissao.id);

                    grupopermissao.operadora    =   u.Vgrupo_permissao.operadora;
                    grupopermissao.operadora_i  =   u.Vgrupo_permissao.operadora_i;
                    grupopermissao.operadora_a = u.Vgrupo_permissao.operadora_a;
                    grupopermissao.operadora_d = u.Vgrupo_permissao.operadora_d;

                    grupopermissao.grupo = u.Vgrupo_permissao.grupo;
                    grupopermissao.grupo_i = u.Vgrupo_permissao.grupo_i;
                    grupopermissao.grupo_a = u.Vgrupo_permissao.grupo_a;
                    grupopermissao.grupo_d = u.Vgrupo_permissao.grupo_d;

                    grupopermissao.usuario = u.Vgrupo_permissao.usuario;
                    grupopermissao.usuario_i = u.Vgrupo_permissao.usuario_i;
                    grupopermissao.usuario_a = u.Vgrupo_permissao.usuario_a;
                    grupopermissao.usuario_d = u.Vgrupo_permissao.usuario_d;

                    grupopermissao.grupo_permissao1 = u.Vgrupo_permissao.grupo_permissao1;
                    grupopermissao.grupo_permissao_i = u.Vgrupo_permissao.grupo_permissao_i;
                    grupopermissao.grupo_permissao_a = u.Vgrupo_permissao.grupo_permissao_a;
                    grupopermissao.grupo_permissao_d = u.Vgrupo_permissao.grupo_permissao_d;

                    grupopermissao.usuario_permissao = u.Vgrupo_permissao.usuario_permissao;
                    grupopermissao.usuario_permissao_i = u.Vgrupo_permissao.usuario_permissao_i;
                    grupopermissao.usuario_permissao_a = u.Vgrupo_permissao.usuario_permissao_a;
                    grupopermissao.usuario_permissao_d = u.Vgrupo_permissao.usuario_permissao_d;

                    if (TryUpdateModel(grupopermissao)) {
                        dg.SaveChanges();
                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Permissã de Grupo Alterado com Sucesso!</font>";
                    }
                    else{
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Alterar Permissão de Grupo</font>";
                    }
   
                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Grupo Inserido com Sucesso!</font>";
                                    }
            }
            return RedirectToAction("Grupo");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Models.ViewModelDetalhePermisaoGrupo u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.grupo_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        grupo grupo = dg.grupoes.Find(u.Vgrupo.id);
                        grupo.nome_grupo = u.Vgrupo.nome_grupo;

                        if (TryUpdateModel(grupo)) {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Grupo Atualizado com Sucesso!</font>";
                        } else {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Grupo</font>";
                        }
                        return RedirectToAction("Grupo");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar o Grupo</font>";
                        return RedirectToAction("Grupo");
                    }
                }
            }
                 using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.grupoes
                         select new ListaGrupo
                         {
                             cod_grupo = a.id,
                             desc_grupo = a.nome_grupo
                         };
                ViewData["listagrupo"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Grupo";
            return View("Grupo", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.grupo_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        grupo grupo = dg.grupoes.Find(id);
                        dg.grupoes.Remove(grupo);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Grupo");
                    }                    
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Grupo Excluído com Sucesso!</font>";
                    
                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir o Grupo</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Grupo");
        }
    }
}
