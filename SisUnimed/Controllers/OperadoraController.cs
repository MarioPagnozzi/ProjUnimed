using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class OperadoraController : Controller
    {
        //
        // GET: /Grupo/

        public ActionResult Operadora()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca Operadoras na tabela
                    var operadoras = from a in up.operadoras1                                    
                                     join b in up.usuarios on a.sisusuarioi equals b.id into g
                                     join c in up.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaOperadora1
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_cod_operadora = a.c_cod_operadora,
                                     data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                                     data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                    ViewData["listaoperadora"] = operadoras.ToList();
                }
                ViewBag.Titulo = "Cadastro de Operadoras";
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
                var dadosoperadora = dg.operadoras1.Where(a => a.id.Equals(id)).FirstOrDefault();                
               
                //carrega lista de operadoras
                var listaoperadora = from a in dg.operadoras1
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaOperadora1
                                     {
                                         id = a.id,
                                         c_nome = a.c_nome,
                                         c_cod_operadora = a.c_cod_operadora,
                                         data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                                         data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                ViewData["listaoperadora"] = listaoperadora.ToList();

                //atualiza permissao de usuários
                var id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                //Altera status para editar
                ViewBag.Action = "Editar";
                return View("Operadora", dadosoperadora);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.operadoras1
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()                             
                             select new ListaOperadora1
                             {
                                 id = a.id,
                                 c_nome = a.c_nome,
                                 c_cod_operadora = a.c_cod_operadora,
                                 data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                                 data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaoperadora"] = lg.ToList();
                }
                if (campo == "codigo")
                {
                    int idoperadora = int.Parse(pesquisa);
                    var lg = from a in dg.operadoras1
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             where a.id.Equals(idoperadora)
                             select new ListaOperadora1
                             {
                                 id = a.id,
                                 c_nome = a.c_nome,
                                 c_cod_operadora = a.c_cod_operadora,
                                 data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                                 data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listaoperadora"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia")
                    {
                        var lg = from a in dg.operadoras1
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.StartsWith(pesquisa)
                                 select new ListaOperadora1
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_cod_operadora = a.c_cod_operadora,
                                     data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                                     data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaoperadora"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.operadoras1
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.EndsWith(pesquisa)
                                 select new ListaOperadora1
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_cod_operadora = a.c_cod_operadora,
                                     data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                                     data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaoperadora"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.operadoras1
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.Contains(pesquisa)
                                 select new ListaOperadora1
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_cod_operadora = a.c_cod_operadora,
                                     data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                                     data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listaoperadora"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaOperadora");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Operadora";

                //carrega lista de grupo
                var lg = from a in dg.operadoras1
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaOperadora1
                         {
                             id = a.id,
                             c_nome = a.c_nome,
                             c_cod_operadora = a.c_cod_operadora,
                             data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                             data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaoperadora"] = lg.ToList();

                //prepara model para inserção
                var operadora = new operadora1();
                
                ViewBag.Action = "Inserir";

                return View("Operadora", operadora);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(operadora1 u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.operadora_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.c_nome = (u.c_nome.ToUpper());
                            u.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.sisdatai = DateTime.Today;
                            dg.operadoras1.Add(u);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.operadoras1
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaOperadora1
                                     {
                                         id = a.id,
                                         c_nome = a.c_nome,
                                         c_cod_operadora = a.c_cod_operadora,
                                         data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                                         data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                            ViewData["listaoperadora"] = lg.ToList();
                            ViewBag.Titulo = "Cadastro de Operadora";
                            return View("Operadora");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Operadora Inserida com Sucesso!</font>";
                        ViewBag.Action = "";

                        return RedirectToAction("Operadora");

                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.operadoras1
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaOperadora1
                         {
                             id = a.id,
                             c_nome = a.c_nome,
                             c_cod_operadora = a.c_cod_operadora,
                             data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                             data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaoperadora"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Operadora";
            return View("Operadora", u);
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(operadora1 u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.operadora_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        operadora1 operadora = dg.operadoras1.Find(u.id);
                        operadora.c_nome = u.c_nome.ToUpper();
                        operadora.sisdataa = DateTime.Today;
                        operadora.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(operadora))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Operadora Atualizada com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Operadora</font>";
                        }
                        return RedirectToAction("Operadora");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar a Operadora</font>";
                        return RedirectToAction("Operadora");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.operadoras1
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaOperadora1
                         {
                             id = a.id,
                             c_nome = a.c_nome,
                             c_cod_operadora = a.c_cod_operadora,
                             data_Inclusao = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             usuario_Inclusao = (x == null ? "Sem Dados" : x.nome_usuario),
                             data_alteracao = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             usuario_alteracao = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listaoperadora"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Operadora";
            return View("Operadora", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.operadora_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        operadora1 operadora = dg.operadoras1.Find(id);
                        dg.operadoras1.Remove(operadora);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Grupo");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Operadora Excluída com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir a Operadora</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Operadora");
        }
    }
}
