using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class EspecialidadeController : Controller
    {
        //
        // GET: /Especialidade/

        public ActionResult Especialidade()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca Operadoras na tabela
                    var especialidades = from a in up.especialidades
                                     join b in up.usuarios on a.sisusuarioi equals b.id into g
                                     join c in up.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaEspecialidade
                                     {
                                         id = a.id,
                                         c_nome = a.c_nome,
                                         c_codigo = a.c_codigo,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                    ViewData["listaespecialidade"] = especialidades.ToList();
                }
                ViewBag.Titulo = "Cadastro de Especialidades";
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
                var dadosespecialidade = dg.especialidades.Where(a => a.id.Equals(id)).FirstOrDefault();

                //carrega lista de operadoras
                var listaespecialidade = from a in dg.especialidades
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaEspecialidade
                                     {
                                         id = a.id,
                                         c_nome = a.c_nome,
                                         c_codigo = a.c_codigo,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                ViewData["listaespecialidade"] = listaespecialidade.ToList();

                //atualiza permissao de usuários
                var id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                //Altera status para editar
                ViewBag.Action = "Editar";
                return View("Especialidade", dadosespecialidade);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.especialidades
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             select new ListaEspecialidade
                             {
                                 id = a.id,
                                 c_nome = a.c_nome,
                                 c_codigo = a.c_codigo,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaespecialidade"] = lg.ToList();
                }
                if (campo == "codigo")
                {
                    int idoperadora = int.Parse(pesquisa);
                    var lg = from a in dg.especialidades
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             where a.id.Equals(idoperadora)
                             select new ListaEspecialidade
                             {
                                 id = a.id,
                                 c_nome = a.c_nome,
                                 c_codigo = a.c_codigo,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaespecialidade"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia")
                    {
                        var lg = from a in dg.especialidades
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.StartsWith(pesquisa)
                                 select new ListaEspecialidade
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_codigo = a.c_codigo,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaespecialidade"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.especialidades
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.EndsWith(pesquisa)
                                 select new ListaEspecialidade
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_codigo = a.c_codigo,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaespecialidade"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.especialidades
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.Contains(pesquisa)
                                 select new ListaEspecialidade
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_codigo = a.c_codigo,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaespecialidade"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaEspecialidade");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Especialidade";

                //carrega lista de grupo
                var lg = from a in dg.especialidades
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaEspecialidade
                         {
                             id = a.id,
                             c_nome = a.c_nome,
                             c_codigo = a.c_codigo,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaespecialidade"] = lg.ToList();

                //prepara model para inserção
                var especialidade = new especialidade();

                ViewBag.Action = "Inserir";

                return View("Especialidade", especialidade);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(especialidade u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.especialidade_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.c_nome = (u.c_nome.ToUpper());
                            u.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.sisdatai = DateTime.Today;
                            dg.especialidades.Add(u);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.especialidades
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaEspecialidade
                                     {
                                         id = a.id,
                                         c_nome = a.c_nome,
                                         c_codigo = a.c_codigo,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                            ViewData["listaespecialidade"] = lg.ToList();
                            ViewBag.Titulo = "Cadastro de Especialidade";
                            return View("Especialidade");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Especialidade Inserida com Sucesso!</font>";
                        ViewBag.Action = "";

                        return RedirectToAction("Especialidade");

                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.especialidades
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaEspecialidade
                         {
                             id = a.id,
                             c_nome = a.c_nome,
                             c_codigo = a.c_codigo,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaespecialidade"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Especialidade";
            return View("Especialidade", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(especialidade u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.especialidade_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        especialidade especialidade = dg.especialidades.Find(u.id);
                        especialidade.c_nome = u.c_nome.ToUpper();
                        especialidade.sisdataa = DateTime.Today;
                        especialidade.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(especialidade))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Especialidade Atualizada com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Especialidade</font>";
                        }
                        return RedirectToAction("Especialiade");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar a Especialidade</font>";
                        return RedirectToAction("Especialiade");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.especialidades
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaEspecialidade
                         {
                             id = a.id,
                             c_nome = a.c_nome,
                             c_codigo = a.c_codigo,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaespecialidade"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Especialidades";
            return View("Especialidade", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.especialidade_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        especialidade especialidade = dg.especialidades.Find(id);
                        dg.especialidades.Remove(especialidade);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Grupo");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Especialidade Excluída com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir a Especialidade</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Especialidade");
        }
    }
}

