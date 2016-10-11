using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    [Serializable]
    public class FornecedorController : Controller
    {
        //
        // GET: /Fornecedor/
       
        public ActionResult Fornecedor()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca Operadoras na tabela
                    var fornecedor = from a in up.fornecedores
                                join b in up.usuarios on a.sisusuarioi equals b.id into g
                                join c in up.usuarios on a.sisusuarioa equals c.id into h
                                from x in g.DefaultIfEmpty()
                                from y in h.DefaultIfEmpty()
                                select new ListaFornecedor
                                {
                                    id = a.id,
                                    c_codigo = a.c_codigo,
                                    c_cnpj = a.c_cnpj,
                                    c_email_principal = a.c_email_principal,
                                    c_razao_social = a.c_razao_social,
                                    c_responsavel = a.c_responsavel,
                                    f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                                    f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                                    operadora = a.operadora1.c_nome,
                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                };
                    ViewData["listafornecedor"] = fornecedor.ToList();

                    var operadora = from a in up.operadoras1
                                    select new ListaOperadora1
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome
                                    };
                    
                    ViewData["listaoperadora"] = operadora.ToList();

              
               
                }
                ViewBag.Titulo = "Cadastro de Fornecedores";
                ViewBag.Message = TempData["mensagem"];
                TempData["mensagem"] = "";

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = new fornecedore(),
                    vfornecedores_anexos = new fornecedores_anexos(),
                    vfornecedores_telefones = new fornecedores_telefones(),
                    vfornecedores_regioes = new fornecedores_regioes(),
                    vfornecedores_materiais = new fornecedores_materiais(),
                    vfornecedores_enderecos = new fornecedores_enderecos(),
                    vfornecedores_emails = new fornecedores_emails(),
                    vfornecedores_bancos = new fornecedores_bancos()
                };
               
                return View(vDetalheFornecedor);
               


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
                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                

                //carrega lista de operadoras
                var listafornecedor = from a in dg.fornecedores
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 select new ListaFornecedor
                                 {
                                     id = a.id,
                                     c_codigo = a.c_codigo,
                                     c_responsavel = a.c_responsavel,
                                     c_razao_social = a.c_razao_social,
                                     c_cnpj = a.c_cnpj,
                                     c_email_principal = a.c_email_principal,
                                     f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                                     f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                                     operadora = a.operadora1.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                ViewData["listafornecedor"] = listafornecedor.ToList();

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
                return View("Fornecedor", dadosfornecedor);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    var lg = from a in dg.fornecedores
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             select new ListaFornecedor
                             {
                                 id = a.id,
                                 c_codigo = a.c_codigo,
                                 c_responsavel = a.c_responsavel,
                                 c_razao_social = a.c_razao_social,
                                 c_cnpj = a.c_cnpj,
                                 c_email_principal = a.c_email_principal,
                                 f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                                 f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                                 operadora = a.operadora1.c_nome,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listafornecedor"] = lg.ToList();
                }
                if (campo == "codigo" && pesquisa != string.Empty)
                {
                    int idoperadora = int.Parse(pesquisa);
                    var lg = from a in dg.fornecedores
                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                             from x in g.DefaultIfEmpty()
                             from y in h.DefaultIfEmpty()
                             where a.id == idoperadora
                             select new ListaFornecedor
                             {
                                 id = a.id,
                                 c_codigo = a.c_codigo,
                                 c_responsavel = a.c_responsavel,
                                 c_razao_social = a.c_razao_social,
                                 c_cnpj = a.c_cnpj,
                                 c_email_principal = a.c_email_principal,
                                 f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                                 f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                                 operadora = a.operadora1.c_nome,
                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                             };
                    ViewData["listafornecedor"] = lg.ToList();
                }
                else
                {
                    if (tipo == "inicia")
                    {
                        var lg = from a in dg.fornecedores
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 select new ListaFornecedor
                                 {
                                     id = a.id,
                                     c_codigo = a.c_codigo,
                                     c_responsavel = a.c_responsavel,
                                     c_razao_social = a.c_razao_social,
                                     c_cnpj = a.c_cnpj,
                                     c_email_principal = a.c_email_principal,
                                     f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                                     f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                                     operadora = a.operadora1.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listafornecedor"] = lg.ToList();
                    }
                    else if (tipo == "termina")
                    {
                        var lg = from a in dg.fornecedores
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 select new ListaFornecedor
                                 {
                                     id = a.id,
                                     c_codigo = a.c_codigo,
                                     c_responsavel = a.c_responsavel,
                                     c_razao_social = a.c_razao_social,
                                     c_cnpj = a.c_cnpj,
                                     c_email_principal = a.c_email_principal,
                                     f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                                     f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                                     operadora = a.operadora1.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listafornecedor"] = lg.ToList();
                    }
                    else
                    {
                        var lg = from a in dg.fornecedores
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 select new ListaFornecedor
                                 {
                                     id = a.id,
                                     c_codigo = a.c_codigo,
                                     c_responsavel = a.c_responsavel,
                                     c_razao_social = a.c_razao_social,
                                     c_cnpj = a.c_cnpj,
                                     c_email_principal = a.c_email_principal,
                                     f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                                     f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                                     operadora = a.operadora1.c_nome,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                 };
                        ViewData["listafornecedor"] = lg.ToList();
                    }
                }

            }
            return PartialView("ListaFornecedor");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Fornecedores";

                //carrega lista de grupo
                var lg = from a in dg.fornecedores
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaFornecedor
                         {
                             id = a.id,
                             c_codigo = a.c_codigo,
                             c_responsavel = a.c_responsavel,
                             c_razao_social = a.c_razao_social,
                             c_cnpj = a.c_cnpj,
                             c_email_principal = a.c_email_principal,
                             f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                             f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                             operadora = a.operadora1.c_nome,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listafornecedor"] = lg.ToList();

                //prepara model para inserção
                var fornecedor = new fornecedore();
                //var fornecedor_anexo = new fornecedores_anexos();
                //var fornecedor_banco = new fornecedores_bancos();
                //var fornecedor_email = new fornecedores_emails();
                //var fornecedor_endereco = new fornecedores_enderecos();
                //var fornecedor_material = new fornecedores_materiais();
                //var fornecedor_regiao = new fornecedores_regioes();
                //var fornecedor_telefone = new fornecedores_telefones();

                //var vDetalheFornecedores = new vModelDetalheFornecedor
                //{
                //    vfornecedor = fornecedor,
                //    vfornecedores_anexos = fornecedor_anexo,
                //    vfornecedores_bancos = fornecedor_banco,
                //    vfornecedores_emails = fornecedor_email,
                //    vfornecedores_enderecos = fornecedor_endereco,
                //    vfornecedores_materiais = fornecedor_material,
                //    vfornecedores_regioes = fornecedor_regiao,
                //    vfornecedores_telefones = fornecedor_telefone
                //};

                ViewBag.Action = "Inserir";

                return View("Fornecedor", fornecedor);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(fornecedore u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.fornecedores_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.c_razao_social = (u.c_razao_social.ToUpper());
                            u.c_responsavel = u.c_responsavel.ToUpper();
                            u.c_codigo = u.c_codigo.ToUpper();
                            u.c_email_principal = u.c_email_principal.ToLower();
                            u.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.sisdatai = DateTime.Today;
                            dg.fornecedores.Add(u);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            //cria lista de grupo
                            var lg = from a in dg.fornecedores
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     select new ListaFornecedor
                                     {
                                         id = a.id,
                                         c_codigo = a.c_codigo,
                                         c_responsavel = a.c_responsavel,
                                         c_razao_social = a.c_razao_social,
                                         c_cnpj = a.c_cnpj,
                                         c_email_principal = a.c_email_principal,
                                         f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                                         f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                                         operadora = a.operadora1.c_nome,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                            ViewData["listafornecedor"] = lg.ToList();
                            ViewBag.Titulo = "Cadastro de Fornecedores";
                            return RedirectToAction("Fornecedor");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Fornecedor Inserido com Sucesso!</font>";
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
                var lg = from a in dg.fornecedores
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaFornecedor
                         {
                             id = a.id,
                             c_codigo = a.c_codigo,
                             c_responsavel = a.c_responsavel,
                             c_razao_social = a.c_razao_social,
                             c_cnpj = a.c_cnpj,
                             c_email_principal = a.c_email_principal,
                             f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                             f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                             operadora = a.operadora1.c_nome,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listafornecedor"] = lg.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Fornecedores";
            return View("Fornecedor", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(fornecedore u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.fornecedores_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        fornecedore fornecedor = dg.fornecedores.Find(u.id);
                        fornecedor.c_razao_social = u.c_razao_social.ToUpper();
                        fornecedor.c_email_principal = u.c_email_principal.ToLower();
                        fornecedor.c_codigo = u.c_codigo.ToUpper();
                        fornecedor.c_responsavel = u.c_responsavel.ToUpper();
                        fornecedor.sisdataa = DateTime.Today;
                        fornecedor.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(fornecedor))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Fornecedor Atualizado com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Fornecedor</font>";
                        }
                        return RedirectToAction("Fornecedor");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar o Fornecedor</font>";
                        return RedirectToAction("Fornecedor");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                //cria lista de grupo
                var lg = from a in dg.fornecedores
                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                         from x in g.DefaultIfEmpty()
                         from y in h.DefaultIfEmpty()
                         select new ListaFornecedor
                         {
                             id = a.id,
                             c_codigo = a.c_codigo,
                             c_responsavel = a.c_responsavel,
                             c_razao_social = a.c_razao_social,
                             c_cnpj = a.c_cnpj,
                             c_email_principal = a.c_email_principal,
                             f_situacao = a.f_situacao == 1 ? "Ativo" : "Inativo",
                             f_tipo_fornecedor = a.f_tipo_fornecedor == 1 ? "Distribuidor/Atacadista" : a.f_tipo_fornecedor == 2 ? "Fabricante" : a.f_tipo_fornecedor == 3 ? "Importador" : "Varegista",
                             operadora = a.operadora1.c_nome,
                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                         };
                ViewData["listafornecedor"] = lg.ToList();
            }
            ViewBag.Action = "Editar";
            ViewBag.Titulo = "Cadastro de Fornecedores";
            return View("Fornecedor", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.fornecedores_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        fornecedore fornecedor = dg.fornecedores.Find(id);
                        dg.fornecedores.Remove(fornecedor);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Fornecedor");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Fornecedor Excluído com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir o Fornecedor</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Fornecedor");
        }
    }
}