using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class MarcaController : Controller
    {
        //
        // GET: /Marca/

        public ActionResult Marca()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca Operadoras na tabela
                    var marca = from a in up.marcas
                                join b in up.usuarios on a.sisusuarioi equals b.id into g
                                join c in up.usuarios on a.sisusuarioa equals c.id into h
                                from x in g.DefaultIfEmpty()
                                from y in h.DefaultIfEmpty()
                                select new ListaMarca
                                {
                                    id = a.id,
                                    f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                                    c_nome = a.c_nome,
                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                };
                    ViewData["listamarca"] = marca.ToList();

                    var operadoras = from a in up.operadoras1
                                     orderby a.c_nome
                                     select a;
                    ViewData["listaoperadoras"] = operadoras.ToList();
                }
                ViewBag.Titulo = "Cadastro de Marcas";
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
                var dadosmarca = dg.marcas.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosoperadora = dg.marcas_operadoras.Where(a => a.marca.Equals(id)).FirstOrDefault();               
                var dados = new ViewModelDetalheMarcaOp
                {
                    Vmarca = dadosmarca,
                    VmarcaOperadora = dadosoperadora
                };

                var marcaoperadora = from a in dg.marcas_operadoras
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g                                     
                                     where a.marca.Equals(id)
                                     from x in g.DefaultIfEmpty()                                                                         
                                     select new ListaMarcaOperadora
                                     {
                                         c_cod_operadora = a.operadora1.c_cod_operadora,
                                         id = a.id,
                                         marca = a.marca1.c_nome,
                                         Operadora = a.operadora1.c_nome,
                                         sisdatai = a.sisdatai,
                                         sisusuarioi = x == null ? "" : x.nome_usuario
                                     };
                ViewData["listaMarcaOp"] = marcaoperadora.ToList();

                //carrega lista de operadoras
                var listamarca = from a in dg.marcas
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 select new ListaMarca
                                 {
                                     id = a.id,
                                     f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                                     c_nome = a.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                ViewData["listamarca"] = listamarca.ToList();

                //atualiza permissao de usuários
                var id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                var operadoras = from a in dg.operadoras1
                                 orderby a.c_nome
                                 select a;
                ViewData["listaoperadoras"] = operadoras.ToList();

                if (TempData["mensagem"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagem"];
                    TempData["mensagem"] = string.Empty;
                }

                //Altera status para editar
                ViewBag.Action = "Editar";

                return View("Marca", dados);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.marcas
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             select new ListaMarca
                             {
                                 id = a.id,
                                 f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                                 c_nome = a.c_nome,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listamarca"] = lg.ToList();
                }
                if (campo == "codigo" && pesquisa != string.Empty)
                {
                    int idoperadora = int.Parse(pesquisa);
                    var lg = from a in dg.marcas
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             where a.id == idoperadora
                             select new ListaMarca
                             {
                                 id = a.id,
                                 f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                                 c_nome = a.c_nome,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listamarca"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia")
                    {
                        var lg = from a in dg.marcas
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.StartsWith(pesquisa)
                                 select new ListaMarca
                                 {
                                     id = a.id,
                                     f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                                     c_nome = a.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listamarca"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.marcas
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.EndsWith(pesquisa)
                                 select new ListaMarca
                                 {
                                     id = a.id,
                                     f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                                     c_nome = a.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listamarca"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.marcas
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.c_nome.Contains(pesquisa)
                                 select new ListaMarca
                                 {
                                     id = a.id,
                                     f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                                     c_nome = a.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listamarca"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaMarca");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Marcas";

                //carrega lista de grupo
                var lg = from a in dg.marcas
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaMarca
                         {
                             id = a.id,
                             f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                             c_nome = a.c_nome,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listamarca"] = lg.ToList();

                //prepara model para inserção
                var marca = new marca();
                var marca_operadora = new marcas_operadoras();
                var VdetalheMarcaOperadora = new ViewModelDetalheMarcaOp
                {
                    Vmarca = marca,
                    VmarcaOperadora = marca_operadora
                };

                var operadoras = from a in dg.operadoras1
                                 orderby a.c_nome
                                 select a;
                ViewData["listaoperadoras"] = operadoras.ToList();

                ViewBag.Action = "Inserir";

                return View("Marca", VdetalheMarcaOperadora);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(ViewModelDetalheMarcaOp u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.marcas_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.Vmarca.c_nome = (u.Vmarca.c_nome.ToUpper());
                            u.Vmarca.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.Vmarca.sisdatai = DateTime.Today;
                            dg.marcas.Add(u.Vmarca);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.marcas
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaMarca
                                     {
                                         id = a.id,
                                         f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                                         c_nome = a.c_nome,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                            ViewData["listamarca"] = lg.ToList();
                            var operadoras = from a in dg.operadoras1
                                             orderby a.c_nome
                                             select a;
                            ViewData["listaoperadoras"] = operadoras.ToList();
                            ViewBag.Titulo = "Cadastro de Marcas";
                            return RedirectToAction("Marca");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Marca Inserida com Sucesso!</font>";
                        ViewBag.Action = "";
                        var id = u.Vmarca.id;
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
                var lg = from a in dg.marcas
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaMarca
                         {
                             id = a.id,
                             f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                             c_nome = a.c_nome,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listamarca"] = lg.ToList();
                var operadoras = from a in dg.operadoras1
                                 orderby a.c_nome
                                 select a;
                ViewData["listaoperadoras"] = operadoras.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Marcas";
            return View("Marca", u.Vmarca);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(ViewModelDetalheMarcaOp u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.marcas_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        marca marca = dg.marcas.Find(u.Vmarca.id);
                        marca.c_nome = u.Vmarca.c_nome.ToUpper();
                        marca.sisdataa = DateTime.Today;
                        marca.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(marca))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Marca Atualizada com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Marca</font>";
                        }
                        return RedirectToAction("Marca");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar a Marca</font>";
                        return RedirectToAction("Marca");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.marcas
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaMarca
                         {
                             id = a.id,
                             f_situacao = (a.f_situacao == 1 ? "Ativa" : "Inativa"),
                             c_nome = a.c_nome,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listamarca"] = lg.ToList();
                var operadoras = from a in dg.operadoras1
                                 orderby a.c_nome
                                 select a;
                ViewData["listaoperadoras"] = operadoras.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Marcas";
            return View("Marca", u.Vmarca);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.marcas_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        marca marca = dg.marcas.Find(id);
                        dg.marcas.Remove(marca);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return PartialView("Marca");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Marca Excluída com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir a Marca</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Marca");
        }
        public ActionResult IncluirMarcaOp(int marca, int operadora)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.marcas_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            marcas_operadoras marcaop = new marcas_operadoras();
                            marcaop.marca = marca;
                            marcaop.operadora = operadora;
                            dg.marcas_operadoras.Add(marcaop);
                            dg.SaveChanges();
                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            return PartialView("ListaMarcaOperadora");
                        }
                       
                    }
                    var marcaoperadora = from a in dg.marcas_operadoras
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         where a.marca.Equals(marca)
                                         from x in g.DefaultIfEmpty()
                                         select new ListaMarcaOperadora
                                         {
                                             c_cod_operadora = a.operadora1.c_cod_operadora,
                                             id = a.id,
                                             marca = a.marca1.c_nome,
                                             Operadora = a.operadora1.c_nome,
                                             sisdatai = a.sisdatai,
                                             sisusuarioi = x == null ? "" : x.nome_usuario
                                         };
                    ViewData["listaMarcaOp"] = marcaoperadora.ToList();
                }
            }
            ViewBag.Titulo = "Cadastro de Marcas";
            return PartialView("ListaMarcaOperadora");
        }
        public ActionResult DeleteMarcaOperadora(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var idMarcaop = (from a in dg.marcas_operadoras
                                where a.id == id
                                select new { a.marca }).First();
                var up = dg.usuario_permissao.Where(a => a.marcas_d.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                if (up >= 1)
                {
                    try
                    {
                        marcas_operadoras marcaop = dg.marcas_operadoras.Find(id);
                        dg.marcas_operadoras.Remove(marcaop);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return PartialView("ListaMarcaOperadora");
                    }
                }
                var marcaoperadora = from a in dg.marcas_operadoras
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     where a.marca.Equals(idMarcaop.marca)
                                     from x in g.DefaultIfEmpty()
                                     select new ListaMarcaOperadora
                                     {
                                         c_cod_operadora = a.operadora1.c_cod_operadora,
                                         id = a.id,
                                         marca = a.marca1.c_nome,
                                         Operadora = a.operadora1.c_nome,
                                         sisdatai = a.sisdatai,
                                         sisusuarioi = x == null ? "" : x.nome_usuario
                                     };
                ViewData["listaMarcaOp"] = marcaoperadora.ToList();
            }
            ViewBag.Titulo = "Cadastro de Marcas";
            return PartialView("ListaMarcaOperadora");
        }
        
        //public ActionResult ListaMarcaOperadora()
        //{
        //    using (UnimedEntities1 dg = new UnimedEntities1())
        //    {
        //        var marcaop = 
        //    }
        //}
    }
}