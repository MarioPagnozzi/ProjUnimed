using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class RegiaoController : Controller
    {
        //
        // GET: /Regiao/

        public ActionResult Regiao()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca Operadoras na tabela
                    var regiao = from a in up.regioes
                                join b in up.usuarios on a.sisusuarioi equals b.id into g
                                join c in up.usuarios on a.sisusuarioa equals c.id into h
                                from x in g.DefaultIfEmpty()
                                from y in h.DefaultIfEmpty()
                                select new ListaRegiao
                                {
                                    id = a.id,
                                    c_descricao = a.c_descricao,                                    
                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                };
                    ViewData["listaregiao"] = regiao.ToList();
                }
                ViewBag.Titulo = "Cadastro de Regiões";
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
                var dadosregioes = dg.regioes.Where(a => a.id.Equals(id)).FirstOrDefault();

                //carrega lista de operadoras
                var listaregiao = from a in dg.regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 select new ListaRegiao
                                 {
                                     id = a.id,
                                     c_descricao = a.c_descricao,                                     
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                ViewData["listaregiao"] = listaregiao.ToList();

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
                return View("Regiao", dadosregioes);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.regioes
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             select new ListaRegiao
                             {
                                 id = a.id,
                                 c_descricao = a.c_descricao,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaregiao"] = lg.ToList();
                }
                if (campo == "codigo" && pesquisa != string.Empty)
                {
                    int idoperadora = int.Parse(pesquisa);
                    var lg = from a in dg.regioes
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             where a.id == idoperadora
                             select new ListaRegiao
                             {
                                 id = a.id,
                                 c_descricao = a.c_descricao,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaregiao"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia")
                    {
                        var lg = from a in dg.regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.StartsWith(pesquisa)
                                 select new ListaRegiao
                                 {
                                     id = a.id,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaregiao"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.EndsWith(pesquisa)
                                 select new ListaRegiao
                                 {
                                     id = a.id,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaregiao"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.Contains(pesquisa)
                                 select new ListaRegiao
                                 {
                                     id = a.id,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaregiao"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaRegiao");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Regiões";

                //carrega lista de grupo
                var lg = from a in dg.regioes
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaRegiao
                         {
                             id = a.id,
                             c_descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaregiao"] = lg.ToList();

                //prepara model para inserção
                var regiao = new regio();

                ViewBag.Action = "Inserir";

                return View("Regiao", regiao);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(regio u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.regioes_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.c_descricao = (u.c_descricao.ToUpper());
                            u.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.sisdatai = DateTime.Today;
                            dg.regioes.Add(u);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.regioes
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaRegiao
                                     {
                                         id = a.id,
                                         c_descricao = a.c_descricao,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                            ViewData["listaregiao"] = lg.ToList();
                            ViewBag.Titulo = "Cadastro de Regiões";
                            return RedirectToAction("Regiao");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Região Inserida com Sucesso!</font>";
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
                var lg = from a in dg.regioes
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaRegiao
                         {
                             id = a.id,
                             c_descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaregiao"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Regiões";
            return View("Regiao", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(regio u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.regioes_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        regio regiao = dg.regioes.Find(u.id);
                        regiao.c_descricao = u.c_descricao.ToUpper();
                        regiao.sisdataa = DateTime.Today;
                        regiao.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(regiao))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Região Atualizada com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Região</font>";
                        }
                        return RedirectToAction("Região");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar a Região</font>";
                        return RedirectToAction("Regiao");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.regioes
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaRegiao
                         {
                             id = a.id,
                             c_descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaregiao"] = lg.ToList();
            }
            ViewBag.Action = "Editar";
            ViewBag.Titulo = "Cadastro de Regiões";
            return View("Regiao", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.regioes_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        regio regiao = dg.regioes.Find(id);
                        dg.regioes.Remove(regiao);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Regiao");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Região Excluída com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir a Região</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Regiao");
        }
    }
}