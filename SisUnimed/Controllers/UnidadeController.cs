using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class UnidadeController : Controller
    {
        //
        // GET: /Unidade/

        public ActionResult Unidade()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca Operadoras na tabela
                    var unidade = from a in up.unidades
                                join b in up.usuarios on a.sisusuarioi equals b.id into g
                                join c in up.usuarios on a.sisusuarioa equals c.id into h
                                from x in g.DefaultIfEmpty()
                                from y in h.DefaultIfEmpty()
                                select new ListaUnidade
                                {
                                    id = a.id,
                                    c_sigla = a.c_sigla,
                                    c_descricao = a.c_descricao,
                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                };
                    ViewData["listaunidade"] = unidade.ToList();
                }
                ViewBag.Titulo = "Cadastro de Unidades";
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
                var dadosunidade = dg.unidades.Where(a => a.id.Equals(id)).FirstOrDefault();

                //carrega lista de operadoras
                var listaunidade = from a in dg.unidades
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 select new ListaUnidade
                                 {
                                     id = a.id,
                                     c_sigla = a.c_sigla,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                ViewData["listaunidade"] = listaunidade.ToList();

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
                return View("Unidade", dadosunidade);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.unidades
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             select new ListaUnidade
                             {
                                 id = a.id,
                                 c_sigla = a.c_sigla,
                                 c_descricao = a.c_descricao,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaunidade"] = lg.ToList();
                }
                if (campo == "codigo" && pesquisa != string.Empty)
                {
                    int idoperadora = int.Parse(pesquisa);
                    var lg = from a in dg.unidades
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             where a.id == idoperadora
                             select new ListaUnidade
                             {
                                 id = a.id,
                                 c_sigla = a.c_sigla,
                                 c_descricao = a.c_descricao,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaunidade"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia")
                    {
                        var lg = from a in dg.unidades
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.StartsWith(pesquisa)
                                 select new ListaUnidade
                                 {
                                     id = a.id,
                                     c_sigla = a.c_sigla,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaunidade"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.unidades
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.EndsWith(pesquisa)
                                 select new ListaUnidade
                                 {
                                     id = a.id,
                                     c_sigla = a.c_sigla,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaunidade"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.unidades
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.Contains(pesquisa)
                                 select new ListaUnidade
                                 {
                                     id = a.id,
                                     c_sigla = a.c_sigla,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaunidade"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaUnidade");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Unidades";

                //carrega lista de grupo
                var lg = from a in dg.unidades
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaUnidade
                         {
                             id = a.id,
                             c_sigla = a.c_sigla,
                             c_descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaunidade"] = lg.ToList();

                //prepara model para inserção
                var unidade = new unidade();

                ViewBag.Action = "Inserir";

                return View("Unidade", unidade);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(unidade u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.unidades_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.c_descricao = (u.c_descricao.ToUpper());
                            u.c_sigla = (u.c_sigla.ToUpper());
                            u.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.sisdatai = DateTime.Today;
                            dg.unidades.Add(u);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.unidades
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaUnidade
                                     {
                                         id = a.id,
                                         c_sigla = a.c_sigla,
                                         c_descricao = a.c_descricao,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                            ViewData["listaunidade"] = lg.ToList();
                            ViewBag.Titulo = "Cadastro de Unidades";
                            return RedirectToAction("Unidade");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Unidade Inserida com Sucesso!</font>";
                        ViewBag.Action = "";
                        var id = u.id;
                        return RedirectToAction("PreencheCampos", new { id = id });

                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.unidades
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaUnidade
                         {
                             id = a.id,
                             c_sigla = a.c_sigla,
                             c_descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaunidade"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Unidades";
            return View("Unidade", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(unidade u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.unidades_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        unidade unidade = dg.unidades.Find(u.id);
                        unidade.c_descricao = u.c_descricao.ToUpper();
                        unidade.c_sigla = u.c_sigla.ToUpper();
                        unidade.sisdataa = DateTime.Today;
                        unidade.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(unidade))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Unidade Atualizada com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Unidade</font>";
                        }
                        return RedirectToAction("Unidade");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar a Unidade</font>";
                        return RedirectToAction("Unidade");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.unidades
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaUnidade
                         {
                             id = a.id,
                             c_sigla = a.c_sigla,
                             c_descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaunidade"] = lg.ToList();
            }
            ViewBag.Action = "Editar";
            ViewBag.Titulo = "Cadastro de Unidade";
            return View("Unidade", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.unidades_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        unidade unidade = dg.unidades.Find(id);
                        dg.unidades.Remove(unidade);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Unidade");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Unidade Excluída com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir a Unidade</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Unidade");
        }
    }
}