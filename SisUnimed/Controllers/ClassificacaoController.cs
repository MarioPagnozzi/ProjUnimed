using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class ClassificacaoController : Controller
    {
        //
        // GET: /Classificacao/

        public ActionResult Classificacao()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca Operadoras na tabela
                    var classificacao = from a in up.classificacoes
                                         join b in up.usuarios on a.sisusuarioi equals b.id into g
                                         join c in up.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         select new ListaClassificacao
                                         {
                                             id = a.id,
                                             descricao = a.c_descricao,                                            
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                    ViewData["listaclassificacao"] = classificacao.ToList();
                }
                ViewBag.Titulo = "Cadastro de Classificacao";
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
                var dadosclassificacao = dg.classificacoes.Where(a => a.id.Equals(id)).FirstOrDefault();

                //carrega lista de operadoras
                var listaclassificacao = from a in dg.classificacoes
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         select new ListaClassificacao
                                         {
                                             id = a.id,
                                             descricao = a.c_descricao,                                            
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaclassificacao"] = listaclassificacao.ToList();

                //atualiza permissao de usuários
                var id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                //Altera status para editar
                ViewBag.Action = "Editar";
                return View("Classificacao", dadosclassificacao);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.classificacoes
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             select new ListaClassificacao
                             {
                                 id = a.id,
                                 descricao = a.c_descricao,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaclassificacao"] = lg.ToList();
                }
                if (campo == "codigo" && pesquisa != string.Empty)
                {
                    int idoperadora = int.Parse(pesquisa);
                    var lg = from a in dg.classificacoes
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             where a.id == idoperadora
                             select new ListaClassificacao
                             {
                                 id = a.id,
                                 descricao = a.c_descricao,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaclassificacao"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia")
                    {
                        var lg = from a in dg.classificacoes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.StartsWith(pesquisa)
                                 select new ListaClassificacao
                                 {
                                     id = a.id,
                                     descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaclassificacao"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.classificacoes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.EndsWith(pesquisa)
                                 select new ListaClassificacao
                                 {
                                     id = a.id,
                                     descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaclassificacao"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.classificacoes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.Contains(pesquisa)
                                 select new ListaClassificacao
                                 {
                                     id = a.id,
                                     descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaclassificacao"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaClassificacao");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Classificação";

                //carrega lista de grupo
                var lg = from a in dg.classificacoes
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaClassificacao
                         {
                             id = a.id,
                             descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaclassificacao"] = lg.ToList();

                //prepara model para inserção
                var classificacao = new classificaco();

                ViewBag.Action = "Inserir";

                return View("Classificacao", classificacao);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(classificaco u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.classificacao_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.c_descricao = (u.c_descricao.ToUpper());
                            u.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.sisdatai = DateTime.Today;
                            dg.classificacoes.Add(u);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.classificacoes
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaClassificacao
                                     {
                                         id = a.id,
                                         descricao = a.c_descricao,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                            ViewData["listaclassificacao"] = lg.ToList();
                            ViewBag.Titulo = "Cadastro de Classificações";
                            return View("Classificacao");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Classificação Inserida com Sucesso!</font>";
                        ViewBag.Action = "";

                        return RedirectToAction("Classificacao");

                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.classificacoes
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaClassificacao
                         {
                             id = a.id,
                             descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaclassificacao"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Classificações";
            return View("Classificacao", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(classificaco u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.classificacao_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        classificaco classificacao = dg.classificacoes.Find(u.id);
                        classificacao.c_descricao = u.c_descricao.ToUpper();
                        classificacao.sisdataa = DateTime.Today;
                        classificacao.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(classificacao))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Classificacão Atualizada com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Classificação</font>";
                        }
                        return RedirectToAction("Classificacao");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar a Classificação</font>";
                        return RedirectToAction("Classificacao");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.classificacoes
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaClassificacao
                         {
                             id = a.id,
                             descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaclassificacao"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Classificações";
            return View("Classificacao", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.classificacao_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        classificaco classificacao = dg.classificacoes.Find(id);
                        dg.classificacoes.Remove(classificacao);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Classificacao");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Classificação Excluída com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir a Classificação</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Classificacao");
        }
    }
}
