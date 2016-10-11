using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class EstadoController : Controller
    {
        //
        // GET: /Estado/

        public ActionResult Estado()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca Operadoras na tabela
                    var estado = from a in up.estados
                                join b in up.usuarios on a.sisusuarioi equals b.id into g
                                join c in up.usuarios on a.sisusuarioa equals c.id into h
                                from x in g.DefaultIfEmpty()
                                from y in h.DefaultIfEmpty()
                                select new ListaEstado
                                {
                                    id = a.id,
                                    c_sigla = a.c_sigla,
                                    c_nome = a.c_nome,
                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                };
                    ViewData["listaestado"] = estado.ToList();
                }
                ViewBag.Titulo = "Cadastro de Estados";
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
                var dadosestado = dg.estados.Where(a => a.id.Equals(id)).FirstOrDefault();

                //carrega lista de operadoras
                var listaestado = from a in dg.estados
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 select new ListaEstado
                                 {
                                     id = a.id,
                                     c_sigla = a.c_sigla,
                                     c_nome = a.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                ViewData["listaestado"] = listaestado.ToList();

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
                return View("Estado", dadosestado);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.estados
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             select new ListaEstado
                             {
                                 id = a.id,
                                 c_sigla = a.c_sigla,
                                 c_nome = a.c_nome,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaestado"] = lg.ToList();
                }
                if (campo == "codigo" && pesquisa != string.Empty)
                {
                    int idoperadora = int.Parse(pesquisa);
                    var lg = from a in dg.estados
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             where a.id == idoperadora
                             select new ListaEstado
                             {
                                 id = a.id,
                                 c_sigla = a.c_sigla,
                                 c_nome = a.c_nome,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaestado"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia")
                    {
                        var lg = from a in dg.estados
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.StartsWith(pesquisa)
                                 select new ListaEstado
                                 {
                                     id = a.id,
                                     c_sigla = a.c_sigla,
                                     c_nome = a.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaestado"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.estados
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.EndsWith(pesquisa)
                                 select new ListaEstado
                                 {
                                     id = a.id,
                                     c_sigla = a.c_sigla,
                                     c_nome = a.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaestado"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.estados
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.Contains(pesquisa)
                                 select new ListaEstado
                                 {
                                     id = a.id,
                                     c_sigla = a.c_sigla,
                                     c_nome = a.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaestado"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaEstado");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Banco";

                //carrega lista de grupo
                var lg = from a in dg.estados
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaEstado
                         {
                             id = a.id,
                             c_sigla = a.c_sigla,
                             c_nome = a.c_nome,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaestado"] = lg.ToList();

                //prepara model para inserção
                var estado = new estado();

                ViewBag.Action = "Inserir";

                return View("Estado", estado);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(estado u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.estados_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.c_nome = (u.c_nome.ToUpper());
                            u.c_sigla = (u.c_sigla.ToUpper());
                            u.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.sisdatai = DateTime.Today;
                            dg.estados.Add(u);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.estados
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaEstado
                                     {
                                         id = a.id,
                                         c_sigla = a.c_sigla,
                                         c_nome = a.c_nome,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                            ViewData["listaestado"] = lg.ToList();
                            ViewBag.Titulo = "Cadastro de Estados";
                            return RedirectToAction("Estado");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Estado Inserido com Sucesso!</font>";
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
                var lg = from a in dg.estados
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaEstado
                         {
                             id = a.id,
                             c_sigla = a.c_sigla,
                             c_nome = a.c_nome,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaestado"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Estados";
            return View("Estado", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(estado u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.estados_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        estado estado = dg.estados.Find(u.id);
                        estado.c_nome = u.c_nome.ToUpper();
                        estado.sisdataa = DateTime.Today;
                        estado.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(estado))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Estado Atualizado com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Estado</font>";
                        }
                        return RedirectToAction("Estado");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar o Estado</font>";
                        return RedirectToAction("Estado");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.estados
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaEstado
                         {
                             id = a.id,
                             c_sigla = a.c_sigla,
                             c_nome = a.c_nome,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaestado"] = lg.ToList();
            }
            ViewBag.Action = "Editar";
            ViewBag.Titulo = "Cadastro de Estados";
            return View("Estado", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.bancos_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        estado estado = dg.estados.Find(id);
                        dg.estados.Remove(estado);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Banco");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Estado Excluído com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir o Estado</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Estado");
        }
    }
}
