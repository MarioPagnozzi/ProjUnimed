using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class ProcedimentoController : Controller
    {
        //
        // GET: /Procedimento/

        public ActionResult Procedimento()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca Operadoras na tabela
                    var procedimento = from a in up.procedimentos
                                 join b in up.usuarios on a.sisusuarioi equals b.id into g
                                 join c in up.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 select new ListaProcedimentos
                                 {
                                     id = a.id,
                                     c_codigo = a.c_codigo,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                    ViewData["listaprocedimento"] = procedimento.ToList();
                }
                ViewBag.Titulo = "Cadastro de Procedimentos";
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
                var dadosprocedimentos = dg.procedimentos.Where(a => a.id.Equals(id)).FirstOrDefault();

                //carrega lista de operadoras
                var listaprocedimento = from a in dg.procedimentos
                                  join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                  join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                  from x in g.DefaultIfEmpty()
                                  from y in h.DefaultIfEmpty()
                                  select new ListaProcedimentos
                                  {
                                      id = a.id,
                                      c_codigo = a.c_codigo,
                                      c_descricao = a.c_descricao,
                                      sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                      sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                      sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                      sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                  };
                ViewData["listaprocedimento"] = listaprocedimento.ToList();

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
                return View("Procedimento", dadosprocedimentos);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.procedimentos
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             select new ListaProcedimentos
                             {
                                 id = a.id,
                                 c_codigo = a.c_codigo,
                                 c_descricao = a.c_descricao,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaprocedimento"] = lg.ToList();
                }
                if (campo == "codigo" && pesquisa != string.Empty)
                {
                    int idoperadora = int.Parse(pesquisa);
                    var lg = from a in dg.procedimentos
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             where a.id == idoperadora
                             select new ListaProcedimentos
                             {
                                 id = a.id,
                                 c_codigo = a.c_codigo,
                                 c_descricao = a.c_descricao,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaprocedimento"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia")
                    {
                        var lg = from a in dg.procedimentos
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.StartsWith(pesquisa)
                                 select new ListaProcedimentos
                                 {
                                     id = a.id,
                                     c_codigo = a.c_codigo,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaprocedimento"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.procedimentos
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.EndsWith(pesquisa)
                                 select new ListaProcedimentos
                                 {
                                     id = a.id,
                                     c_codigo = a.c_codigo,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaprocedimento"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.procedimentos
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_descricao.Contains(pesquisa)
                                 select new ListaProcedimentos
                                 {
                                     id = a.id,
                                     c_codigo = a.c_codigo,
                                     c_descricao = a.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaprocedimento"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaProcedimento");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Procedimentos";

                //carrega lista de grupo
                var lg = from a in dg.procedimentos
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaProcedimentos
                         {
                             id = a.id,
                             c_codigo = a.c_codigo,
                             c_descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaprocedimento"] = lg.ToList();

                //prepara model para inserção
                var procedimento = new procedimento();

                ViewBag.Action = "Inserir";

                return View("Procedimento", procedimento);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(procedimento u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.procedimentos_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.c_descricao = (u.c_descricao.ToUpper());
                            u.c_codigo = u.c_codigo.ToUpper();
                            u.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.sisdatai = DateTime.Today;
                            dg.procedimentos.Add(u);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.procedimentos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaProcedimentos
                                     {
                                         id = a.id,
                                         c_codigo = a.c_codigo,
                                         c_descricao = a.c_descricao,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                            ViewData["listaprocedimento"] = lg.ToList();
                            ViewBag.Titulo = "Cadastro de Procedimentos";
                            return RedirectToAction("Procedimento");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Procedimento Inserido com Sucesso!</font>";
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
                var lg = from a in dg.procedimentos
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaProcedimentos
                         {
                             id = a.id,
                             c_codigo = a.c_codigo,
                             c_descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaprocedimento"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Procedimentos";
            return View("Procedimento", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(procedimento u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.procedimentos_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        procedimento procedimento = dg.procedimentos.Find(u.id);
                        procedimento.c_descricao = u.c_descricao.ToUpper();
                        procedimento.c_codigo = u.c_codigo.ToUpper();
                        procedimento.sisdataa = DateTime.Today;
                        procedimento.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(procedimento))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Procedimento Atualizado com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Procedimento</font>";
                        }
                        return RedirectToAction("Procedimento");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar o Procedimento</font>";
                        return RedirectToAction("Procedimento");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.procedimentos
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaProcedimentos
                         {
                             id = a.id,
                             c_codigo = a.c_codigo,
                             c_descricao = a.c_descricao,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaprocedimento"] = lg.ToList();
            }
            ViewBag.Action = "Editar";
            ViewBag.Titulo = "Cadastro de Procedimentos";
            return View("Procedimento", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.procedimentos_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        procedimento procedimento = dg.procedimentos.Find(id);
                        dg.procedimentos.Remove(procedimento);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Regiao");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Procedimento Excluído com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir o Procedimento</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Procedimento");
        }
    }
}