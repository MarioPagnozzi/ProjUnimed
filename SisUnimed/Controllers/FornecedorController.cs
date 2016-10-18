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

                    // busca fornecedores na tabela
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

                    var listabanco = from a in up.bancos
                                     select new ListaBanco
                                     {
                                         c_codigo = a.c_codigo,
                                         c_nome = a.c_descricao,
                                         id = a.id
                                     };
                    ViewData["listabanco"] = listabanco.ToList();

                    var listaestado = from a in up.estados
                                      select new ListaEstado
                                      {
                                          c_sigla = a.c_sigla,
                                          c_nome = a.c_nome,
                                          id = a.id
                                      };
                    ViewData["listaestado"] = listaestado.ToList();

                    // lista materiais
                    var listamateriais = from a in up.fornecedores_materiais
                                         join b in up.usuarios on a.sisusuarioi equals b.id into g
                                         join c in up.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.fornecedor == 0
                                         select new ListaFornecedorMateriais
                                         {
                                             id = a.id,
                                             material = a.materiai.c_nome_material,
                                             v_preco_ctnpm = a.v_preco_ctnpm,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                             fornecedor = a.fornecedor
                                         };
                    ViewData["listamateriais"] = listamateriais.ToList();

                    var lstmaterial = from a in up.materiais
                                      select new ListaMaterial
                                      {
                                          c_nome_material = a.c_nome_material,
                                          id = a.id
                                      };
                    ViewData["lstmaterial"] = lstmaterial.ToList();

                    // lista banco
                    var listafornbanco = from a in up.fornecedores_bancos
                                         join b in up.usuarios on a.sisusuarioi equals b.id into g
                                         join c in up.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.fornecedor == 0
                                         select new ListaFornecedorBanco
                                         {
                                             id = a.id,
                                             banco = a.banco1.c_descricao,
                                             c_agencia = a.c_agencia,
                                             c_conta = a.c_conta,
                                             c_titular = a.c_titular,
                                             c_cidade = a.c_cidade,
                                             estado = a.estado1.c_sigla,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                             fornecedor = a.fornecedor
                                         };
                    ViewData["listafornbanco"] = listafornbanco.ToList();

                    //Lista de Enderecos
                    var listaendereco = from a in up.fornecedores_enderecos
                                         join b in up.usuarios on a.sisusuarioi equals b.id into g
                                         join c in up.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.fornecedor == 0
                                         select new ListaEndereco
                                         {
                                             id = a.id,
                                             c_logradoro = a.c_logradouro,
                                             c_descricao = a.c_descricao,
                                             c_bairro = a.c_bairro,
                                             c_cep = a.c_cep,
                                             c_cidade = a.c_cidade,
                                             c_estado = a.estado1.c_sigla,
                                             c_numero = a.c_numero,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                             fornecedor = a.fornecedor
                                         };
                    ViewData["listaendereco"] = listaendereco.ToList();

                    //lista telefone
                    var listatelefone = from a in up.fornecedores_telefones
                                        join b in up.usuarios on a.sisusuarioi equals b.id into g
                                        join c in up.usuarios on a.sisusuarioa equals c.id into h
                                        from x in g.DefaultIfEmpty()
                                        from y in h.DefaultIfEmpty()
                                        where a.fornecedor == 0
                                        select new ListaFornecedorTelefone
                                        {
                                            id = a.id,
                                            c_nome = a.c_nome,
                                            c_setor = a.c_setor,
                                            c_telefone = a.c_telefone,
                                            sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                            sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                            sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                            sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                            fornecedor = a.fornecedor,
                                            fornecedor_nome = a.fornecedore.c_razao_social
                                        };
                    ViewData["listatelefone"] = listatelefone.ToList();

                    //lista email
                    var listaemail = from a in up.fornecedores_emails
                                        join b in up.usuarios on a.sisusuarioi equals b.id into g
                                        join c in up.usuarios on a.sisusuarioa equals c.id into h
                                        from x in g.DefaultIfEmpty()
                                        from y in h.DefaultIfEmpty()
                                        where a.fornecedor == 0
                                        select new ListaEmail
                                        {
                                            id = a.id,
                                            c_nome = a.c_nome,
                                            c_email = a.c_email,                                           
                                            sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                            sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                            sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                            sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                            fornecedor = a.fornecedor
                                        };
                    ViewData["listaemail"] = listaemail.ToList();

                    //lista regiao
                    var listafornregiao = from a in up.fornecedores_regioes
                                          join b in up.usuarios on a.sisusuarioi equals b.id into g
                                          join c in up.usuarios on a.sisusuarioa equals c.id into h
                                          from x in g.DefaultIfEmpty()
                                          from y in h.DefaultIfEmpty()
                                          where a.fornecedor == 0
                                          select new ListaFornecedorRegiao
                                          {
                                              id = a.id,
                                              c_distribuidor = a.c_distribuidor,
                                              c_email = a.c_email,
                                              c_telefone = a.c_telefone,
                                              c_responsavel = a.c_responsavel,
                                              regiao = a.regio.c_descricao,
                                              sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                              sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                              sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                              sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                              fornecedor = a.fornecedor
                                          };
                    ViewData["listafornregiao"] = listafornregiao.ToList();

                    var listaregiao = from a in up.regioes
                                      select new ListaRegiao
                                      {
                                          c_descricao = a.c_descricao,
                                          id = a.id
                                      };
                    ViewData["listaregiao"] = listaregiao.ToList();

              
               
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
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

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

                // lista materiais
                var listamateriais = from a in dg.fornecedores_materiais
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == id
                                     select new ListaFornecedorMateriais
                                     {
                                         id = a.id,
                                         material = a.materiai.c_nome_material,
                                         v_preco_ctnpm = a.v_preco_ctnpm,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                // lista banco
                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == vDetalheFornecedor.vfornecedor.id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();

                var operadora = from a in dg.operadoras1
                                select new ListaOperadora1
                                {
                                    id = a.id,
                                    c_nome = a.c_nome
                                };

                ViewData["listaoperadora"] = operadora.ToList();

                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();

                //lista telefone
                var listatelefone = from a in dg.fornecedores_telefones
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaFornecedorTelefone
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_setor = a.c_setor,
                                        c_telefone = a.c_telefone,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor,
                                        fornecedor_nome = a.fornecedore.c_razao_social
                                    };
                ViewData["listatelefone"] = listatelefone.ToList();

                //lista email
                var listaemail = from a in dg.fornecedores_emails
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.fornecedor == id
                                 select new ListaEmail
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_email = a.c_email,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listaemail"] = listaemail.ToList();

                var listafornregiao = from a in dg.fornecedores_regioes
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.fornecedor == id
                                      select new ListaFornecedorRegiao
                                      {
                                          id = a.id,
                                          c_distribuidor = a.c_distribuidor,
                                          c_email = a.c_email,
                                          c_telefone = a.c_telefone,
                                          c_responsavel = a.c_responsavel,
                                          regiao = a.regio.c_descricao,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                          fornecedor = a.fornecedor
                                      };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
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
                return View("Fornecedor", vDetalheFornecedor);
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
                                 where a.c_razao_social.StartsWith(pesquisa)
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
                                 where a.c_razao_social.EndsWith(pesquisa)
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
                                 where a.c_razao_social.Contains(pesquisa)
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
                var fornecedor_anexo = new fornecedores_anexos();
                var fornecedor_banco = new fornecedores_bancos();
                var fornecedor_email = new fornecedores_emails();
                var fornecedor_endereco = new fornecedores_enderecos();
                var fornecedor_material = new fornecedores_materiais();
                var fornecedor_regiao = new fornecedores_regioes();
                var fornecedor_telefone = new fornecedores_telefones();

                var vDetalheFornecedores = new vModelDetalheFornecedor
                {
                    vfornecedor = fornecedor,
                    vfornecedores_anexos = fornecedor_anexo,
                    vfornecedores_bancos = fornecedor_banco,
                    vfornecedores_emails = fornecedor_email,
                    vfornecedores_enderecos = fornecedor_endereco,
                    vfornecedores_materiais = fornecedor_material,
                    vfornecedores_regioes = fornecedor_regiao,
                    vfornecedores_telefones = fornecedor_telefone
                };

                var listafornregiao = from a in dg.fornecedores_regioes
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.fornecedor == vDetalheFornecedores.vfornecedor.id
                                      select new ListaFornecedorRegiao
                                      {
                                          id = a.id,
                                          c_distribuidor = a.c_distribuidor,
                                          c_email = a.c_email,
                                          c_telefone = a.c_telefone,
                                          c_responsavel = a.c_responsavel,
                                          regiao = a.regio.c_descricao,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                          fornecedor = a.fornecedor
                                      };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
                                  };
                ViewData["listaregiao"] = listaregiao.ToList();

                // lista materiais
                var listamateriais = from a in dg.fornecedores_materiais
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == vDetalheFornecedores.vfornecedor.id
                                     select new ListaFornecedorMateriais
                                     {
                                         id = a.id,
                                         material = a.materiai.c_nome_material,
                                         v_preco_ctnpm = a.v_preco_ctnpm,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                //lista email
                var listaemail = from a in dg.fornecedores_emails
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.fornecedor == vDetalheFornecedores.vfornecedor.id
                                 select new ListaEmail
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_email = a.c_email,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listaemail"] = listaemail.ToList();

                //lista telefone
                var listatelefone = from a in dg.fornecedores_telefones
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == vDetalheFornecedores.vfornecedor.id
                                    select new ListaFornecedorTelefone
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_setor = a.c_setor,
                                        c_telefone = a.c_telefone,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor,
                                        fornecedor_nome = a.fornecedore.c_razao_social
                                    };
                ViewData["listatelefone"] = listatelefone.ToList();

                //Lista de Enderecos
                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == vDetalheFornecedores.vfornecedor.id
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();

                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == vDetalheFornecedores.vfornecedor.id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();

                var operadora = from a in dg.operadoras1
                                select new ListaOperadora1
                                {
                                    id = a.id,
                                    c_nome = a.c_nome
                                };

                ViewData["listaoperadora"] = operadora.ToList();

                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                ViewBag.Action = "Inserir";

                return View("Fornecedor", vDetalheFornecedores);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(Models.vModelDetalheFornecedor u)
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
                            u.vfornecedor.c_razao_social = (u.vfornecedor.c_razao_social.ToUpper());
                            u.vfornecedor.c_responsavel = u.vfornecedor.c_responsavel.ToUpper();
                            u.vfornecedor.c_codigo = u.vfornecedor.c_codigo.ToUpper();
                            u.vfornecedor.c_email_principal = u.vfornecedor.c_email_principal.ToLower();
                            u.vfornecedor.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.vfornecedor.sisdatai = DateTime.Today;
                            dg.fornecedores.Add(u.vfornecedor);
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

                            // lista regiao
                            var listafornregiao = from a in dg.fornecedores_regioes
                                                  join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                  join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                  from x in g.DefaultIfEmpty()
                                                  from y in h.DefaultIfEmpty()
                                                  where a.fornecedor == u.vfornecedor.id
                                                  select new ListaFornecedorRegiao
                                                  {
                                                      id = a.id,
                                                      c_distribuidor = a.c_distribuidor,
                                                      c_email = a.c_email,
                                                      c_telefone = a.c_telefone,
                                                      c_responsavel = a.c_responsavel,
                                                      regiao = a.regio.c_descricao,
                                                      sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                      sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                      sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                      sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                      fornecedor = a.fornecedor
                                                  };
                            ViewData["listafornregiao"] = listafornregiao.ToList();

                            var listaregiao = from a in dg.regioes
                                              select new ListaRegiao
                                              {
                                                  c_descricao = a.c_descricao,
                                                  id = a.id
                                              };
                            ViewData["listaregiao"] = listaregiao.ToList();

                            // lista material
                            var listamateriais = from a in dg.fornecedores_materiais
                                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                 from x in g.DefaultIfEmpty()
                                                 from y in h.DefaultIfEmpty()
                                                 where a.fornecedor == u.vfornecedor.id
                                                 select new ListaFornecedorMateriais
                                                 {
                                                     id = a.id,
                                                     material = a.materiai.c_nome_material,
                                                     v_preco_ctnpm = a.v_preco_ctnpm,
                                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                     fornecedor = a.fornecedor
                                                 };
                            ViewData["listamateriais"] = listamateriais.ToList();

                            var lstmaterial = from a in dg.materiais
                                              select new ListaMaterial
                                              {
                                                  c_nome_material = a.c_nome_material,
                                                  id = a.id
                                              };
                            ViewData["lstmaterial"] = lstmaterial.ToList();

                            //lista email
                            var listaemail = from a in dg.fornecedores_emails
                                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                             from x in g.DefaultIfEmpty()
                                             from y in h.DefaultIfEmpty()
                                             where a.fornecedor == u.vfornecedor.id
                                             select new ListaEmail
                                             {
                                                 id = a.id,
                                                 c_nome = a.c_nome,
                                                 c_email = a.c_email,
                                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                 fornecedor = a.fornecedor
                                             };
                            ViewData["listaemail"] = listaemail.ToList();

                            //lista telefone
                            var listatelefone = from a in dg.fornecedores_telefones
                                                join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                from x in g.DefaultIfEmpty()
                                                from y in h.DefaultIfEmpty()
                                                where a.fornecedor == u.vfornecedor.id
                                                select new ListaFornecedorTelefone
                                                {
                                                    id = a.id,
                                                    c_nome = a.c_nome,
                                                    c_setor = a.c_setor,
                                                    c_telefone = a.c_telefone,
                                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                    fornecedor = a.fornecedor,
                                                    fornecedor_nome = a.fornecedore.c_razao_social
                                                };
                            ViewData["listatelefone"] = listatelefone.ToList();

                            //Lista de Enderecos
                            var listaendereco = from a in dg.fornecedores_enderecos
                                                join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                from x in g.DefaultIfEmpty()
                                                from y in h.DefaultIfEmpty()
                                                where a.fornecedor == u.vfornecedor.id
                                                select new ListaEndereco
                                                {
                                                    id = a.id,
                                                    c_logradoro = a.c_logradouro,
                                                    c_descricao = a.c_descricao,
                                                    c_bairro = a.c_bairro,
                                                    c_cep = a.c_cep,
                                                    c_cidade = a.c_cidade,
                                                    c_estado = a.estado1.c_sigla,
                                                    c_numero = a.c_numero,
                                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                    fornecedor = a.fornecedor
                                                };
                            ViewData["listaendereco"] = listaendereco.ToList();

                            var listafornbanco = from a in dg.fornecedores_bancos
                                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                 from x in g.DefaultIfEmpty()
                                                 from y in h.DefaultIfEmpty()
                                                 where a.fornecedor == u.vfornecedor.id
                                                 select new ListaFornecedorBanco
                                                 {
                                                     id = a.id,
                                                     banco = a.banco1.c_descricao,
                                                     c_agencia = a.c_agencia,
                                                     c_conta = a.c_conta,
                                                     c_titular = a.c_titular,
                                                     c_cidade = a.c_cidade,
                                                     estado = a.estado1.c_sigla,
                                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                     fornecedor = a.fornecedor
                                                 };
                            ViewData["listafornbanco"] = listafornbanco.ToList();

                            var listabanco = from a in dg.bancos
                                             select new ListaBanco
                                             {
                                                 c_codigo = a.c_codigo,
                                                 c_nome = a.c_descricao,
                                                 id = a.id
                                             };
                            ViewData["listabanco"] = listabanco.ToList();

                            var listaestado = from a in dg.estados
                                              select new ListaEstado
                                              {
                                                  c_sigla = a.c_sigla,
                                                  c_nome = a.c_nome,
                                                  id = a.id
                                              };
                            ViewData["listaestado"] = listaestado.ToList();

                            var operadora = from a in dg.operadoras1
                                            select new ListaOperadora1
                                            {
                                                id = a.id,
                                                c_nome = a.c_nome
                                            };

                            ViewBag.Titulo = "Cadastro de Fornecedores";
                            return RedirectToAction("Fornecedor");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Fornecedor Inserido com Sucesso!</font>";
                        ViewBag.Action = "";
                        var id = u.vfornecedor.id;
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

                // lista material
                var listamateriais = from a in dg.fornecedores_materiais
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == u.vfornecedor.id
                                     select new ListaFornecedorMateriais
                                     {
                                         id = a.id,
                                         material = a.materiai.c_nome_material,
                                         v_preco_ctnpm = a.v_preco_ctnpm,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                // lista regiao
                var listafornregiao = from a in dg.fornecedores_regioes
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.fornecedor == u.vfornecedor.id
                                      select new ListaFornecedorRegiao
                                      {
                                          id = a.id,
                                          c_distribuidor = a.c_distribuidor,
                                          c_email = a.c_email,
                                          c_telefone = a.c_telefone,
                                          c_responsavel = a.c_responsavel,
                                          regiao = a.regio.c_descricao,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                          fornecedor = a.fornecedor
                                      };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
                                  };
                ViewData["listaregiao"] = listaregiao.ToList();

                //lista email
                var listaemail = from a in dg.fornecedores_emails
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.fornecedor == u.vfornecedor.id
                                 select new ListaEmail
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_email = a.c_email,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listaemail"] = listaemail.ToList();

                //lista telefone
                var listatelefone = from a in dg.fornecedores_telefones
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == u.vfornecedor.id
                                    select new ListaFornecedorTelefone
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_setor = a.c_setor,
                                        c_telefone = a.c_telefone,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor,
                                        fornecedor_nome = a.fornecedore.c_razao_social
                                    };
                ViewData["listatelefone"] = listatelefone.ToList();

                //Lista de Enderecos
                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == u.vfornecedor.id
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();

                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == u.vfornecedor.id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();

                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var operadora = from a in dg.operadoras1
                                select new ListaOperadora1
                                {
                                    id = a.id,
                                    c_nome = a.c_nome
                                };
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Fornecedores";
            return View("Fornecedor", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.fornecedores_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        fornecedore fornecedor = dg.fornecedores.Find(u.vfornecedor.id);
                        fornecedor.c_razao_social = u.vfornecedor.c_razao_social.ToUpper();
                        fornecedor.c_email_principal = u.vfornecedor.c_email_principal.ToLower();
                        fornecedor.c_codigo = u.vfornecedor.c_codigo.ToUpper();
                        fornecedor.c_responsavel = u.vfornecedor.c_responsavel.ToUpper();
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

                //lista material
                var listamateriais = from a in dg.fornecedores_materiais
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == u.vfornecedor.id
                                     select new ListaFornecedorMateriais
                                     {
                                         id = a.id,
                                         material = a.materiai.c_nome_material,
                                         v_preco_ctnpm = a.v_preco_ctnpm,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                //lista regiao
                var listafornregiao = from a in dg.fornecedores_regioes
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.fornecedor == u.vfornecedor.id
                                      select new ListaFornecedorRegiao
                                      {
                                          id = a.id,
                                          c_distribuidor = a.c_distribuidor,
                                          c_email = a.c_email,
                                          c_telefone = a.c_telefone,
                                          c_responsavel = a.c_responsavel,
                                          regiao = a.regio.c_descricao,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                          fornecedor = a.fornecedor
                                      };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
                                  };
                ViewData["listaregiao"] = listaregiao.ToList();

                //lista email
                var listaemail = from a in dg.fornecedores_emails
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.fornecedor == u.vfornecedor.id
                                 select new ListaEmail
                                 {
                                     id = a.id,
                                     c_nome = a.c_nome,
                                     c_email = a.c_email,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listaemail"] = listaemail.ToList();

                //Lista de Enderecos
                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == u.vfornecedor.id
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();

                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == u.vfornecedor.id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();

                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var operadora = from a in dg.operadoras1
                                select new ListaOperadora1
                                {
                                    id = a.id,
                                    c_nome = a.c_nome
                                };
               
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
        public ActionResult IncluirBanco(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = new fornecedores_bancos();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_bancos.fornecedor = id;

                //Lista de Enderecos
                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();

                
                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();

                ViewBag.Action = "InserirBanco";

                return PartialView("FonecedorBanco",vDetalheFornecedor);
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirBanco(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_bancos.c_titular = u.vfornecedores_bancos.c_titular.ToUpper();
                    u.vfornecedores_bancos.c_cidade = u.vfornecedores_bancos.c_cidade.ToUpper();
                    u.vfornecedores_bancos.sisdatai = DateTime.Today;
                    u.vfornecedores_bancos.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());

                    try
                    {
                        dg.fornecedores_bancos.Add(u.vfornecedores_bancos);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        var listabanco = from a in dg.bancos
                                         select new ListaBanco
                                         {
                                             c_codigo = a.c_codigo,
                                             c_nome = a.c_descricao,
                                             id = a.id
                                         };
                        ViewData["listabanco"] = listabanco.ToList();

                        var listaestado = from a in dg.estados
                                          select new ListaEstado
                                          {
                                              c_sigla = a.c_sigla,
                                              c_nome = a.c_nome,
                                              id = a.id
                                          };
                        ViewData["listaestado"] = listaestado.ToList();

                        var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_banco = new fornecedores_bancos();
                        var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();

                        var vDetalheFornecedor = new vModelDetalheFornecedor()
                        {
                            vfornecedor = dadosfornecedor,
                            vfornecedores_anexos = dadosfornecedor_anexo,
                            vfornecedores_bancos = dadosfornecedor_banco,
                            vfornecedores_emails = dadosfornecedor_email,
                            vfornecedores_enderecos = dadosfornecedor_endereco,
                            vfornecedores_materiais = dadosfornecedor_materiais,
                            vfornecedores_regioes = dadosfornecedor_regiao,
                            vfornecedores_telefones = dadosfornecedor_telefone
                        };

                        var listafornbanco = from a in dg.fornecedores_bancos
                                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                             from x in g.DefaultIfEmpty()
                                             from y in h.DefaultIfEmpty()
                                             where a.fornecedor == u.vfornecedor.id
                                             select new ListaFornecedorBanco
                                             {
                                                 id = a.id,
                                                 banco = a.banco1.c_descricao,
                                                 c_agencia = a.c_agencia,
                                                 c_conta = a.c_conta,
                                                 c_titular = a.c_titular,
                                                 c_cidade = a.c_cidade,
                                                 estado = a.estado1.c_sigla,
                                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                 fornecedor = a.fornecedor
                                             };
                        ViewData["listafornbanco"] = listafornbanco.ToList();

                        return PartialView("FonecedorBanco");
                    }

                    TempData["mensagembanco"] = "<font style='color: green;text-align:right;font-size:11px'>Banco inserido com sucesso!</font>";
                    return RedirectToAction("PreencheCamposBanco", new { id_banco = u.vfornecedores_bancos.id, id = u.vfornecedores_bancos.fornecedor});
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_banco = new fornecedores_bancos();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_bancos.fornecedor)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == u.vfornecedor.id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();

                return PartialView("FonecedorBanco", vDetalheFornecedor);
            }
        }
        public ActionResult PreencheCamposBanco(int id_banco, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id) && a.id.Equals(id_banco)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                //carrega lista de operadoras
               
                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == vDetalheFornecedor.vfornecedor.id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();

                

                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();


                if (TempData["mensagembanco"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagembanco"];
                    TempData["mensagembanco"] = string.Empty;
                }
                 

                //Altera status para editar
                ViewBag.Action = "EditarBanco";
                return View("FonecedorBanco", vDetalheFornecedor);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarBanco(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_bancos.c_cidade = u.vfornecedores_bancos.c_cidade.ToUpper();
                    u.vfornecedores_bancos.c_titular = u.vfornecedores_bancos.c_titular.ToUpper();

                    fornecedores_bancos fornbanco = dg.fornecedores_bancos.Find(u.vfornecedores_bancos.id);
                    fornbanco.banco = u.vfornecedores_bancos.banco;
                    fornbanco.c_agencia = u.vfornecedores_bancos.c_agencia;
                    fornbanco.c_cidade = u.vfornecedores_bancos.c_cidade;
                    fornbanco.c_conta = u.vfornecedores_bancos.c_conta;
                    fornbanco.c_titular = u.vfornecedores_bancos.c_titular;
                    fornbanco.estado = u.vfornecedores_bancos.estado;
                   
                    var id = fornbanco.fornecedor;

                    if (TryUpdateModel(fornbanco))
                    {
                        dg.SaveChanges();
                        TempData["mensagembanco"] = "<font style='color: green;text-align:right;font-size:11px'>Banco Atualizado com Sucesso!</font>";
                    }
                    else
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Banco!</font>";

                        var listafornbanco = from a in dg.fornecedores_bancos
                                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                             from x in g.DefaultIfEmpty()
                                             from y in h.DefaultIfEmpty()
                                             where a.fornecedor == id
                                             select new ListaFornecedorBanco
                                             {
                                                 id = a.id,
                                                 banco = a.banco1.c_descricao,
                                                 c_agencia = a.c_agencia,
                                                 c_conta = a.c_conta,
                                                 c_titular = a.c_titular,
                                                 c_cidade = a.c_cidade,
                                                 estado = a.estado1.c_sigla,
                                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                 fornecedor = a.fornecedor
                                             };
                        ViewData["listafornbanco"] = listafornbanco.ToList();



                        var listabanco = from a in dg.bancos
                                         select new ListaBanco
                                         {
                                             c_codigo = a.c_codigo,
                                             c_nome = a.c_descricao,
                                             id = a.id
                                         };
                        ViewData["listabanco"] = listabanco.ToList();

                        var listaestado = from a in dg.estados
                                          select new ListaEstado
                                          {
                                              c_sigla = a.c_sigla,
                                              c_nome = a.c_nome,
                                              id = a.id
                                          };
                        ViewData["listaestado"] = listaestado.ToList();

                        return PartialView("FonecedorBanco", u);
                    }
                    return RedirectToAction("PreencheCamposBanco", new { id_banco = u.vfornecedores_bancos.id, id = u.vfornecedores_bancos.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == u.vfornecedor.id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();



                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();
            }
            return PartialView("FonecedorBanco", u);
        }
        public ActionResult DeleteBanco(int id_banco,int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                try
                {
                    fornecedores_bancos fornbanco = dg.fornecedores_bancos.Find(id_banco);
                    dg.fornecedores_bancos.Remove(fornbanco);
                    dg.SaveChanges();
                }
                catch (SystemException e)
                {
                    ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                    var listafornbanco = from a in dg.fornecedores_bancos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.fornecedor == id
                                         select new ListaFornecedorBanco
                                         {
                                             id = a.id,
                                             banco = a.banco1.c_descricao,
                                             c_agencia = a.c_agencia,
                                             c_conta = a.c_conta,
                                             c_titular = a.c_titular,
                                             c_cidade = a.c_cidade,
                                             estado = a.estado1.c_sigla,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                             fornecedor = a.fornecedor
                                         };
                    ViewData["listafornbanco"] = listafornbanco.ToList();



                    var listabanco = from a in dg.bancos
                                     select new ListaBanco
                                     {
                                         c_codigo = a.c_codigo,
                                         c_nome = a.c_descricao,
                                         id = a.id
                                     };
                    ViewData["listabanco"] = listabanco.ToList();

                    var listaestado = from a in dg.estados
                                      select new ListaEstado
                                      {
                                          c_sigla = a.c_sigla,
                                          c_nome = a.c_nome,
                                          id = a.id
                                      };
                    ViewData["listaestado"] = listaestado.ToList();

                    var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_banco = new fornecedores_bancos();
                    var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                    var vDetalheFornecedor = new vModelDetalheFornecedor()
                    {
                        vfornecedor = dadosfornecedor,
                        vfornecedores_anexos = dadosfornecedor_anexo,
                        vfornecedores_bancos = dadosfornecedor_banco,
                        vfornecedores_emails = dadosfornecedor_email,
                        vfornecedores_enderecos = dadosfornecedor_endereco,
                        vfornecedores_materiais = dadosfornecedor_materiais,
                        vfornecedores_regioes = dadosfornecedor_regiao,
                        vfornecedores_telefones = dadosfornecedor_telefone
                    };
                    vDetalheFornecedor.vfornecedores_bancos.fornecedor = id;
                                       
                    ViewBag.Action = "EditarBanco";

                    return PartialView("FonecedorBanco",vDetalheFornecedor);
                }
                
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();



                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = new fornecedores_bancos();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_bancos.fornecedor = id;                

                ViewBag.Action = string.Empty;
                ViewBag.Message = "<font style='color: green;text-align:right;font-size:11px'>Banco Excluído com sucesso!</font>";

                return PartialView("FonecedorBanco", vDetalheFornecedor);
            }
            
        }
        public ActionResult Banco(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listafornbanco = from a in dg.fornecedores_bancos
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == id
                                     select new ListaFornecedorBanco
                                     {
                                         id = a.id,
                                         banco = a.banco1.c_descricao,
                                         c_agencia = a.c_agencia,
                                         c_conta = a.c_conta,
                                         c_titular = a.c_titular,
                                         c_cidade = a.c_cidade,
                                         estado = a.estado1.c_sigla,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listafornbanco"] = listafornbanco.ToList();



                var listabanco = from a in dg.bancos
                                 select new ListaBanco
                                 {
                                     c_codigo = a.c_codigo,
                                     c_nome = a.c_descricao,
                                     id = a.id
                                 };
                ViewData["listabanco"] = listabanco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = new fornecedores_bancos();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };               

                ViewBag.Action = string.Empty;
                ViewBag.Message = string.Empty;

                return PartialView("FonecedorBanco", vDetalheFornecedor);
            }
        }
        // métodos endereços
        public ActionResult IncluirEndereco(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                  var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = new fornecedores_enderecos();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_enderecos.fornecedor = id;

                //Lista de Enderecos
                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();                

                ViewBag.Action = "InserirEndereco";

                return PartialView("FornecedorEndereco", vDetalheFornecedor);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirEndereco(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_enderecos.c_logradouro = u.vfornecedores_enderecos.c_logradouro.ToUpper();
                    u.vfornecedores_enderecos.c_descricao = u.vfornecedores_enderecos.c_descricao.ToUpper();
                    u.vfornecedores_enderecos.c_complemento = u.vfornecedores_enderecos.c_complemento == null ? string.Empty : u.vfornecedores_enderecos.c_complemento.ToUpper();
                    u.vfornecedores_enderecos.c_cidade = u.vfornecedores_enderecos.c_cidade.ToUpper();
                    u.vfornecedores_enderecos.c_bairro = u.vfornecedores_enderecos.c_bairro.ToUpper();
                    u.vfornecedores_enderecos.c_numero = u.vfornecedores_enderecos.c_numero.ToUpper();
                    u.vfornecedores_enderecos.sisdatai = DateTime.Today;
                    u.vfornecedores_enderecos.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());

                    try
                    {
                        dg.fornecedores_enderecos.Add(u.vfornecedores_enderecos);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        
                        var listaestado = from a in dg.estados
                                          select new ListaEstado
                                          {
                                              c_sigla = a.c_sigla,
                                              c_nome = a.c_nome,
                                              id = a.id
                                          };
                        ViewData["listaestado"] = listaestado.ToList();

                        var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_endereco = new fornecedores_enderecos();
                        var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();

                        var vDetalheFornecedor = new vModelDetalheFornecedor()
                        {
                            vfornecedor = dadosfornecedor,
                            vfornecedores_anexos = dadosfornecedor_anexo,
                            vfornecedores_bancos = dadosfornecedor_banco,
                            vfornecedores_emails = dadosfornecedor_email,
                            vfornecedores_enderecos = dadosfornecedor_endereco,
                            vfornecedores_materiais = dadosfornecedor_materiais,
                            vfornecedores_regioes = dadosfornecedor_regiao,
                            vfornecedores_telefones = dadosfornecedor_telefone
                        };

                       

                        var listaendereco = from a in dg.fornecedores_enderecos
                                            join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                            join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                            from x in g.DefaultIfEmpty()
                                            from y in h.DefaultIfEmpty()
                                            where a.fornecedor == u.vfornecedores_enderecos.fornecedor
                                            select new ListaEndereco
                                            {
                                                id = a.id,
                                                c_logradoro = a.c_logradouro,
                                                c_descricao = a.c_descricao,
                                                c_bairro = a.c_bairro,
                                                c_cep = a.c_cep,
                                                c_cidade = a.c_cidade,
                                                c_estado = a.estado1.c_sigla,
                                                c_numero = a.c_numero,
                                                sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                fornecedor = a.fornecedor
                                            };
                        ViewData["listaendereco"] = listaendereco.ToList();

                        return PartialView("FornecedorEndereco", vDetalheFornecedor);
                    }

                    TempData["mensagemendereco"] = "<font style='color: green;text-align:right;font-size:11px'>Endereço inserido com sucesso!</font>";
                    return RedirectToAction("PreencheCamposEndereco", new { id_end = u.vfornecedores_enderecos.id, id = u.vfornecedores_enderecos.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                
                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_endereco = new fornecedores_enderecos();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_enderecos.fornecedor)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == u.vfornecedores_enderecos.fornecedor
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();

                return PartialView("FornecedorEndereco", vDetalheFornecedor);
            }
        }
        public ActionResult PreencheCamposEndereco(int id_end, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id) && a.id.Equals(id_end)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                //carrega lista de operadoras

                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();
                                               

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();


                if (TempData["mensagemendereco"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagemendereco"];
                    TempData["mensagemendereco"] = string.Empty;
                }


                //Altera status para editar
                ViewBag.Action = "EditarEndereco";
                return View("FornecedorEndereco", vDetalheFornecedor);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEndereco(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_enderecos.c_cidade = u.vfornecedores_enderecos.c_cidade.ToUpper();
                    u.vfornecedores_enderecos.c_logradouro = u.vfornecedores_enderecos.c_logradouro.ToUpper();
                    u.vfornecedores_enderecos.c_numero = u.vfornecedores_enderecos.c_numero.ToUpper();
                    u.vfornecedores_enderecos.c_descricao = u.vfornecedores_enderecos.c_descricao.ToUpper();
                    u.vfornecedores_enderecos.c_bairro = u.vfornecedores_enderecos.c_bairro.ToUpper();
                    u.vfornecedores_enderecos.c_complemento = u.vfornecedores_enderecos.c_complemento == null ? string.Empty : u.vfornecedores_enderecos.c_complemento.ToUpper();
                   
                    fornecedores_enderecos fornEndereco = dg.fornecedores_enderecos.Find(u.vfornecedores_enderecos.id);

                    fornEndereco.c_bairro = u.vfornecedores_enderecos.c_bairro;
                    fornEndereco.c_cep = u.vfornecedores_enderecos.c_cep;
                    fornEndereco.c_cidade = u.vfornecedores_enderecos.c_cidade;
                    fornEndereco.c_complemento = u.vfornecedores_enderecos.c_complemento;
                    fornEndereco.c_descricao = u.vfornecedores_enderecos.c_descricao;
                    fornEndereco.c_logradouro = u.vfornecedores_enderecos.c_logradouro;
                    fornEndereco.c_numero = u.vfornecedores_enderecos.c_numero;
                    fornEndereco.estado = u.vfornecedores_enderecos.estado;

                    var id = fornEndereco.fornecedor;

                    if (TryUpdateModel(fornEndereco))
                    {
                        dg.SaveChanges();
                        TempData["mensagemendereco"] = "<font style='color: green;text-align:right;font-size:11px'>Endereço Atualizado com Sucesso!</font>";
                    }
                    else
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Endereço!</font>";

                        var listaendereco = from a in dg.fornecedores_enderecos
                                            join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                            join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                            from x in g.DefaultIfEmpty()
                                            from y in h.DefaultIfEmpty()
                                            where a.fornecedor == id
                                            select new ListaEndereco
                                            {
                                                id = a.id,
                                                c_logradoro = a.c_logradouro,
                                                c_descricao = a.c_descricao,
                                                c_bairro = a.c_bairro,
                                                c_cep = a.c_cep,
                                                c_cidade = a.c_cidade,
                                                c_estado = a.estado1.c_sigla,
                                                c_numero = a.c_numero,
                                                sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                fornecedor = a.fornecedor
                                            };
                        ViewData["listaendereco"] = listaendereco.ToList();


                        var listaestado = from a in dg.estados
                                          select new ListaEstado
                                          {
                                              c_sigla = a.c_sigla,
                                              c_nome = a.c_nome,
                                              id = a.id
                                          };
                        ViewData["listaestado"] = listaestado.ToList();

                        return PartialView("FornecedorEndereco", u);
                    }
                    return RedirectToAction("PreencheCamposEndereco", new { id_end = u.vfornecedores_enderecos.id, id = u.vfornecedores_enderecos.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == u.vfornecedores_enderecos.fornecedor
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();
            }
            return PartialView("FornecedorEndereco", u);
        }
        public ActionResult DeleteEndereco(int id_end, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                try
                {
                    fornecedores_enderecos fornEndereco = dg.fornecedores_enderecos.Find(id_end);
                    dg.fornecedores_enderecos.Remove(fornEndereco);
                    dg.SaveChanges();
                }
                catch (SystemException e)
                {
                    ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                    var listaendereco = from a in dg.fornecedores_enderecos
                                        join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                        join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                        from x in g.DefaultIfEmpty()
                                        from y in h.DefaultIfEmpty()
                                        where a.fornecedor == id
                                        select new ListaEndereco
                                        {
                                            id = a.id,
                                            c_logradoro = a.c_logradouro,
                                            c_descricao = a.c_descricao,
                                            c_bairro = a.c_bairro,
                                            c_cep = a.c_cep,
                                            c_cidade = a.c_cidade,
                                            c_estado = a.estado1.c_sigla,
                                            c_numero = a.c_numero,
                                            sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                            sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                            sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                            sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                            fornecedor = a.fornecedor
                                        };
                    ViewData["listaendereco"] = listaendereco.ToList();

                    var listaestado = from a in dg.estados
                                      select new ListaEstado
                                      {
                                          c_sigla = a.c_sigla,
                                          c_nome = a.c_nome,
                                          id = a.id
                                      };
                    ViewData["listaestado"] = listaestado.ToList();

                    var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_endereco = new fornecedores_enderecos();
                    var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                    var vDetalheFornecedor = new vModelDetalheFornecedor()
                    {
                        vfornecedor = dadosfornecedor,
                        vfornecedores_anexos = dadosfornecedor_anexo,
                        vfornecedores_bancos = dadosfornecedor_banco,
                        vfornecedores_emails = dadosfornecedor_email,
                        vfornecedores_enderecos = dadosfornecedor_endereco,
                        vfornecedores_materiais = dadosfornecedor_materiais,
                        vfornecedores_regioes = dadosfornecedor_regiao,
                        vfornecedores_telefones = dadosfornecedor_telefone
                    };
                    vDetalheFornecedor.vfornecedores_bancos.fornecedor = id;

                    ViewBag.Action = "EditarEndereco";

                    return PartialView("FornecedorEndereco", vDetalheFornecedor);
                }

            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = new fornecedores_enderecos();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_bancos.fornecedor = id;

                ViewBag.Action = string.Empty;
                ViewBag.Message = "<font style='color: green;text-align:right;font-size:11px'>Endereço Excluído com sucesso!</font>";

                return PartialView("FornecedorEndereco", vDetalheFornecedor);
            }

        }
        public ActionResult Endereco(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listaendereco = from a in dg.fornecedores_enderecos
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEndereco
                                    {
                                        id = a.id,
                                        c_logradoro = a.c_logradouro,
                                        c_descricao = a.c_descricao,
                                        c_bairro = a.c_bairro,
                                        c_cep = a.c_cep,
                                        c_cidade = a.c_cidade,
                                        c_estado = a.estado1.c_sigla,
                                        c_numero = a.c_numero,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaendereco"] = listaendereco.ToList();

                var listaestado = from a in dg.estados
                                  select new ListaEstado
                                  {
                                      c_sigla = a.c_sigla,
                                      c_nome = a.c_nome,
                                      id = a.id
                                  };
                ViewData["listaestado"] = listaestado.ToList();

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = new fornecedores_enderecos();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                ViewBag.Action = string.Empty;
                ViewBag.Message = string.Empty;

                return PartialView("FornecedorEndereco", vDetalheFornecedor);
            }
        }
        //métodos telefone
        public ActionResult IncluirTelefone(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = new fornecedores_telefones();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_telefones.fornecedor = id;

                //Lista de telefone
                var listatelefone = from a in dg.fornecedores_telefones
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaFornecedorTelefone
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_setor = a.c_setor,
                                        c_telefone = a.c_telefone,                                      
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor,
                                        fornecedor_nome = a.fornecedore.c_razao_social
                                    };
                ViewData["listatelefone"] = listatelefone.ToList();

                ViewBag.Action = "InserirTelefone";

                return PartialView("FornecedorTelefone", vDetalheFornecedor);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirTelefone(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_telefones.c_nome = u.vfornecedores_telefones.c_nome.ToUpper();
                    u.vfornecedores_telefones.c_setor = u.vfornecedores_telefones.c_setor.ToUpper();                   
                    u.vfornecedores_telefones.sisdatai = DateTime.Today;
                    u.vfornecedores_telefones.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());


                    try
                    {
                        dg.fornecedores_telefones.Add(u.vfornecedores_telefones);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";



                        var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_telefone = new fornecedores_telefones();

                        var vDetalheFornecedor = new vModelDetalheFornecedor()
                        {
                            vfornecedor = dadosfornecedor,
                            vfornecedores_anexos = dadosfornecedor_anexo,
                            vfornecedores_bancos = dadosfornecedor_banco,
                            vfornecedores_emails = dadosfornecedor_email,
                            vfornecedores_enderecos = dadosfornecedor_endereco,
                            vfornecedores_materiais = dadosfornecedor_materiais,
                            vfornecedores_regioes = dadosfornecedor_regiao,
                            vfornecedores_telefones = dadosfornecedor_telefone
                        };

                        var listatelefone = from a in dg.fornecedores_telefones
                                            join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                            join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                            from x in g.DefaultIfEmpty()
                                            from y in h.DefaultIfEmpty()
                                            where a.fornecedor == u.vfornecedores_telefones.fornecedor
                                            select new ListaFornecedorTelefone
                                            {
                                                id = a.id,
                                                c_nome = a.c_nome,
                                                c_setor = a.c_setor,
                                                c_telefone = a.c_telefone,
                                                sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                fornecedor = a.fornecedor,
                                                fornecedor_nome = a.fornecedore.c_razao_social
                                            };
                        ViewData["listatelefone"] = listatelefone.ToList();

                        return PartialView("FornecedorTelefone", vDetalheFornecedor);
                    }

                    TempData["mensagemtelefone"] = "<font style='color: green;text-align:right;font-size:11px'>Telefone inserido com sucesso!</font>";
                    return RedirectToAction("PreencheCamposTelefone", new { id_tel = u.vfornecedores_telefones.id, id = u.vfornecedores_telefones.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_telefones.fornecedor)).FirstOrDefault();
                var dadosfornecedor_telefone = new fornecedores_telefones();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                var listatelefone = from a in dg.fornecedores_telefones
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == u.vfornecedores_telefones.fornecedor
                                    select new ListaFornecedorTelefone
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_setor = a.c_setor,
                                        c_telefone = a.c_telefone,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor,
                                        fornecedor_nome = a.fornecedore.c_razao_social
                                    };
                ViewData["listatelefone"] = listatelefone.ToList();

                return PartialView("FornecedorTelefone", vDetalheFornecedor);
            }
        }
        public ActionResult PreencheCamposTelefone(int id_tel, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id) && a.id.Equals(id_tel)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                //carrega lista de operadoras

                var listatelefone = from a in dg.fornecedores_telefones
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaFornecedorTelefone
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_setor = a.c_setor,
                                        c_telefone = a.c_telefone,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor,
                                        fornecedor_nome = a.fornecedore.c_razao_social
                                    };
                ViewData["listatelefone"] = listatelefone.ToList();

                if (TempData["mensagemtelefone"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagemtelefone"];
                    TempData["mensagemtelefone"] = string.Empty;
                }


                //Altera status para editar
                ViewBag.Action = "EditarTelefone";
                return View("FornecedorTelefone", vDetalheFornecedor);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTelefone(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_telefones.c_nome = u.vfornecedores_telefones.c_nome.ToUpper();
                    u.vfornecedores_telefones.c_setor = u.vfornecedores_telefones.c_setor.ToUpper();
                    u.vfornecedores_telefones.sisdataa = DateTime.Today;
                    u.vfornecedores_telefones.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());

                    fornecedores_telefones forntelefone = dg.fornecedores_telefones.Find(u.vfornecedores_telefones.id);

                    forntelefone.c_nome = u.vfornecedores_telefones.c_nome;
                    forntelefone.c_setor = u.vfornecedores_telefones.c_setor;
                    forntelefone.c_telefone = u.vfornecedores_telefones.c_telefone;
                    forntelefone.sisdataa = u.vfornecedores_telefones.sisdataa;
                    forntelefone.sisusuarioa = u.vfornecedores_telefones.sisusuarioa;

                    var id = forntelefone.fornecedor;

                    if (TryUpdateModel(forntelefone))
                    {
                        dg.SaveChanges();
                        TempData["mensagemtelefone"] = "<font style='color: green;text-align:right;font-size:11px'>Telefone Atualizado com Sucesso!</font>";
                    }
                    else
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Endereço!</font>";

                        var listatelefone = from a in dg.fornecedores_telefones
                                            join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                            join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                            from x in g.DefaultIfEmpty()
                                            from y in h.DefaultIfEmpty()
                                            where a.fornecedor == id
                                            select new ListaFornecedorTelefone
                                            {
                                                id = a.id,
                                                c_nome = a.c_nome,
                                                c_setor = a.c_setor,
                                                c_telefone = a.c_telefone,
                                                sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                fornecedor = a.fornecedor,
                                                fornecedor_nome = a.fornecedore.c_razao_social
                                            };
                        ViewData["listatelefone"] = listatelefone.ToList();


                       return PartialView("FornecedorTelefone", u);
                    }
                    return RedirectToAction("PreencheCamposTelefone", new { id_tel = u.vfornecedores_telefones.id, id = u.vfornecedores_telefones.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listatelefone = from a in dg.fornecedores_telefones
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == u.vfornecedores_telefones.fornecedor
                                    select new ListaFornecedorTelefone
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_setor = a.c_setor,
                                        c_telefone = a.c_telefone,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor,
                                        fornecedor_nome = a.fornecedore.c_razao_social
                                    };
                ViewData["listatelefone"] = listatelefone.ToList();

                
            }
            return PartialView("FornecedorTelefone", u);
        }
        public ActionResult DeleteTelefone(int id_tel, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                try
                {
                    fornecedores_telefones fornTelefone = dg.fornecedores_telefones.Find(id_tel);
                    dg.fornecedores_telefones.Remove(fornTelefone);
                    dg.SaveChanges();
                }
                catch (SystemException e)
                {
                    ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                    var listatelefone = from a in dg.fornecedores_telefones
                                        join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                        join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                        from x in g.DefaultIfEmpty()
                                        from y in h.DefaultIfEmpty()
                                        where a.fornecedor == id
                                        select new ListaFornecedorTelefone
                                        {
                                            id = a.id,
                                            c_nome = a.c_nome,
                                            c_setor = a.c_setor,
                                            c_telefone = a.c_telefone,
                                            sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                            sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                            sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                            sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                            fornecedor = a.fornecedor,
                                            fornecedor_nome = a.fornecedore.c_razao_social
                                        };
                    ViewData["listatelefone"] = listatelefone.ToList();

                   
                    var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_telefone = new fornecedores_telefones();

                    var vDetalheFornecedor = new vModelDetalheFornecedor()
                    {
                        vfornecedor = dadosfornecedor,
                        vfornecedores_anexos = dadosfornecedor_anexo,
                        vfornecedores_bancos = dadosfornecedor_banco,
                        vfornecedores_emails = dadosfornecedor_email,
                        vfornecedores_enderecos = dadosfornecedor_endereco,
                        vfornecedores_materiais = dadosfornecedor_materiais,
                        vfornecedores_regioes = dadosfornecedor_regiao,
                        vfornecedores_telefones = dadosfornecedor_telefone
                    };
                    vDetalheFornecedor.vfornecedores_telefones.fornecedor = id;

                    ViewBag.Action = "EditarTelefone";

                    return PartialView("FornecedorTelefone", vDetalheFornecedor);
                }

            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listatelefone = from a in dg.fornecedores_telefones
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaFornecedorTelefone
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_setor = a.c_setor,
                                        c_telefone = a.c_telefone,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor,
                                        fornecedor_nome = a.fornecedore.c_razao_social
                                    };
                ViewData["listatelefone"] = listatelefone.ToList();

               
                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = new fornecedores_telefones();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_telefones.fornecedor = id;

                ViewBag.Action = string.Empty;
                ViewBag.Message = "<font style='color: green;text-align:right;font-size:11px'>Telefone Excluído com sucesso!</font>";

                return PartialView("FornecedorTelefone", vDetalheFornecedor);
            }

        }
        public ActionResult Telefone(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listatelefone = from a in dg.fornecedores_telefones
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaFornecedorTelefone
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_setor = a.c_setor,
                                        c_telefone = a.c_telefone,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor,
                                        fornecedor_nome = a.fornecedore.c_razao_social
                                    };
                ViewData["listatelefone"] = listatelefone.ToList();
               

                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = new fornecedores_telefones();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                ViewBag.Action = string.Empty;
                ViewBag.Message = string.Empty;

                return PartialView("FornecedorTelefone", vDetalheFornecedor);
            }
        }
        // métodos Emails
        public ActionResult IncluirEmail(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = new fornecedores_emails();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_emails.fornecedor = id;

                //Lista de telefone
                var listaemail = from a in dg.fornecedores_emails
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEmail
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_email = a.c_email,                                        
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor                                        
                                    };
                ViewData["listaemail"] = listaemail.ToList();

                ViewBag.Action = "InserirEmail";

                return PartialView("FornecedorEmail", vDetalheFornecedor);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirEmail(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_emails.c_nome = u.vfornecedores_emails.c_nome.ToUpper();
                    u.vfornecedores_emails.c_email = u.vfornecedores_emails.c_email.ToLower();
                    u.vfornecedores_emails.sisdatai = DateTime.Today;
                    u.vfornecedores_emails.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());


                    try
                    {
                        dg.fornecedores_emails.Add(u.vfornecedores_emails);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";



                        var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_email = new fornecedores_emails();
                        var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();

                        var vDetalheFornecedor = new vModelDetalheFornecedor()
                        {
                            vfornecedor = dadosfornecedor,
                            vfornecedores_anexos = dadosfornecedor_anexo,
                            vfornecedores_bancos = dadosfornecedor_banco,
                            vfornecedores_emails = dadosfornecedor_email,
                            vfornecedores_enderecos = dadosfornecedor_endereco,
                            vfornecedores_materiais = dadosfornecedor_materiais,
                            vfornecedores_regioes = dadosfornecedor_regiao,
                            vfornecedores_telefones = dadosfornecedor_telefone
                        };

                        var listaemail = from a in dg.fornecedores_emails
                                            join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                            join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                            from x in g.DefaultIfEmpty()
                                            from y in h.DefaultIfEmpty()
                                            where a.fornecedor == u.vfornecedores_telefones.fornecedor
                                            select new ListaEmail
                                            {
                                                id = a.id,
                                                c_nome = a.c_nome,
                                                c_email = a.c_email,                                                
                                                sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                fornecedor = a.fornecedor
                                            };
                        ViewData["listaemail"] = listaemail.ToList();

                        return PartialView("FornecedorEmail", vDetalheFornecedor);
                    }

                    TempData["mensagememail"] = "<font style='color: green;text-align:right;font-size:11px'>E-mail inserido com sucesso!</font>";
                    return RedirectToAction("PreencheCamposEmail", new { id_email = u.vfornecedores_emails.id, id = u.vfornecedores_emails.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                var dadosfornecedor_email = new fornecedores_emails();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_emails.fornecedor)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                var listaemail = from a in dg.fornecedores_emails
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == u.vfornecedores_telefones.fornecedor
                                    select new ListaEmail
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_email = a.c_email,                                       
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaemail"] = listaemail.ToList();

                return PartialView("FornecedorEmail", vDetalheFornecedor);
            }
        }
        public ActionResult PreencheCamposEmail(int id_email, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id) && a.id.Equals(id_email)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                //carrega lista de operadoras

                var listaemail = from a in dg.fornecedores_emails
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEmail
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_email = a.c_email,                                        
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaemail"] = listaemail.ToList();

                if (TempData["mensagememail"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagememail"];
                    TempData["mensagememail"] = string.Empty;
                }


                //Altera status para editar
                ViewBag.Action = "EditarEmail";
                return View("FornecedorEmail", vDetalheFornecedor);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEmail(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_emails.c_nome = u.vfornecedores_emails.c_nome.ToUpper();
                    u.vfornecedores_emails.c_email = u.vfornecedores_emails.c_email.ToLower();
                    u.vfornecedores_emails.sisdataa = DateTime.Today;
                    u.vfornecedores_emails.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());

                    fornecedores_emails fornemail = dg.fornecedores_emails.Find(u.vfornecedores_emails.id);

                    fornemail.c_nome = u.vfornecedores_emails.c_nome;
                    fornemail.c_email = u.vfornecedores_emails.c_email;                    
                    fornemail.sisdataa = DateTime.Today;
                    fornemail.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());

                    var id = fornemail.fornecedor;

                    if (TryUpdateModel(fornemail))
                    {
                        dg.SaveChanges();
                        TempData["mensagemtelefone"] = "<font style='color: green;text-align:right;font-size:11px'>E-mail Atualizado com Sucesso!</font>";
                    }
                    else
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar E-mail!</font>";

                        var listaemail = from a in dg.fornecedores_emails
                                            join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                            join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                            from x in g.DefaultIfEmpty()
                                            from y in h.DefaultIfEmpty()
                                            where a.fornecedor == id
                                            select new ListaEmail
                                            {
                                                id = a.id,
                                                c_nome = a.c_nome,
                                                c_email = a.c_email,                                                
                                                sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                fornecedor = a.fornecedor
                                            };
                        ViewData["listaemail"] = listaemail.ToList();


                        return PartialView("FornecedorEmail", u);
                    }
                    return RedirectToAction("PreencheCamposEmail", new { id_email = u.vfornecedores_emails.id, id = u.vfornecedores_emails.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listaemail = from a in dg.fornecedores_emails
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == u.vfornecedores_telefones.fornecedor
                                    select new ListaEmail
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_email = a.c_email,                                        
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaemail"] = listaemail.ToList();


            }
            return PartialView("FornecedorEmail", u);
        }
        public ActionResult DeleteEmail(int id_email, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                try
                {
                    fornecedores_emails fornemail = dg.fornecedores_emails.Find(id_email);
                    dg.fornecedores_emails.Remove(fornemail);
                    dg.SaveChanges();
                }
                catch (SystemException e)
                {
                    ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                    var listaemail = from a in dg.fornecedores_emails
                                        join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                        join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                        from x in g.DefaultIfEmpty()
                                        from y in h.DefaultIfEmpty()
                                        where a.fornecedor == id
                                        select new ListaEmail
                                        {
                                            id = a.id,
                                            c_nome = a.c_nome,
                                            c_email = a.c_email,                                           
                                            sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                            sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                            sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                            sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                            fornecedor = a.fornecedor
                                        };
                    ViewData["listaemail"] = listaemail.ToList();


                    var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_email = new fornecedores_emails();
                    var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                    var vDetalheFornecedor = new vModelDetalheFornecedor()
                    {
                        vfornecedor = dadosfornecedor,
                        vfornecedores_anexos = dadosfornecedor_anexo,
                        vfornecedores_bancos = dadosfornecedor_banco,
                        vfornecedores_emails = dadosfornecedor_email,
                        vfornecedores_enderecos = dadosfornecedor_endereco,
                        vfornecedores_materiais = dadosfornecedor_materiais,
                        vfornecedores_regioes = dadosfornecedor_regiao,
                        vfornecedores_telefones = dadosfornecedor_telefone
                    };
                    vDetalheFornecedor.vfornecedores_emails.fornecedor = id;

                    ViewBag.Action = "EditarEmail";

                    return PartialView("FornecedorEmail", vDetalheFornecedor);
                }

            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listaemail = from a in dg.fornecedores_emails
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEmail
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_email = a.c_email,                                        
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaemail"] = listaemail.ToList();


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = new fornecedores_emails();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_emails.fornecedor = id;

                ViewBag.Action = string.Empty;
                ViewBag.Message = "<font style='color: green;text-align:right;font-size:11px'>E-mail Excluído com sucesso!</font>";

                return PartialView("FornecedorEmail", vDetalheFornecedor);
            }

        }
        public ActionResult Email(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listaemail = from a in dg.fornecedores_emails
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.fornecedor == id
                                    select new ListaEmail
                                    {
                                        id = a.id,
                                        c_nome = a.c_nome,
                                        c_email = a.c_email,                                        
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                        fornecedor = a.fornecedor
                                    };
                ViewData["listaemail"] = listaemail.ToList();


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = new fornecedores_emails();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                ViewBag.Action = string.Empty;
                ViewBag.Message = string.Empty;

                return PartialView("FornecedorEmail", vDetalheFornecedor);
            }
        }
        // métodos Regiões
        public ActionResult IncluirRegiao(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = new fornecedores_regioes();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_regioes.fornecedor = id;

                //Lista de telefone
                var listafornregiao = from a in dg.fornecedores_regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.fornecedor == id
                                 select new ListaFornecedorRegiao
                                 {
                                     id = a.id,
                                     c_distribuidor = a.c_distribuidor,
                                     c_email = a.c_email,
                                     c_responsavel = a.c_responsavel,
                                     c_telefone = a.c_telefone,
                                     regiao = a.regio.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
                                  };
                ViewData["listaregiao"] = listaregiao.ToList();

                ViewBag.Action = "InserirRegiao";

                return PartialView("FornecedorRegiao", vDetalheFornecedor);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirRegiao(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_regioes.c_distribuidor = u.vfornecedores_regioes.c_distribuidor.ToUpper();
                    u.vfornecedores_regioes.c_email = u.vfornecedores_regioes.c_email.ToLower();
                    u.vfornecedores_regioes.c_responsavel = u.vfornecedores_regioes.c_responsavel.ToUpper();
                    
                    u.vfornecedores_regioes.sisdatai = DateTime.Today;
                    u.vfornecedores_regioes.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());


                    try
                    {
                        dg.fornecedores_regioes.Add(u.vfornecedores_regioes);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";



                        var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_regiao = new fornecedores_regioes();
                        var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();

                        var vDetalheFornecedor = new vModelDetalheFornecedor()
                        {
                            vfornecedor = dadosfornecedor,
                            vfornecedores_anexos = dadosfornecedor_anexo,
                            vfornecedores_bancos = dadosfornecedor_banco,
                            vfornecedores_emails = dadosfornecedor_email,
                            vfornecedores_enderecos = dadosfornecedor_endereco,
                            vfornecedores_materiais = dadosfornecedor_materiais,
                            vfornecedores_regioes = dadosfornecedor_regiao,
                            vfornecedores_telefones = dadosfornecedor_telefone
                        };

                        var listafornregiao = from a in dg.fornecedores_regioes
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.fornecedor == u.vfornecedores_regioes.fornecedor
                                         select new ListaFornecedorRegiao
                                         {
                                             id = a.id,
                                             c_distribuidor = a.c_distribuidor,
                                             c_email = a.c_email,
                                             c_responsavel = a.c_responsavel,
                                             c_telefone = a.c_telefone,
                                             regiao = a.regio.c_descricao,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                             fornecedor = a.fornecedor
                                         };
                        ViewData["listafornregiao"] = listafornregiao.ToList();

                        var listaregiao = from a in dg.regioes
                                          select new ListaRegiao
                                          {
                                              c_descricao = a.c_descricao,
                                              id = a.id
                                          };
                        ViewData["listaregiao"] = listaregiao;

                        return PartialView("FornecedorRegiao", vDetalheFornecedor);
                    }

                    TempData["mensagemregiao"] = "<font style='color: green;text-align:right;font-size:11px'>Região inserida com sucesso!</font>";
                    return RedirectToAction("PreencheCamposRegiao", new { id_reg = u.vfornecedores_regioes.id, id = u.vfornecedores_regioes.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                var dadosfornecedor_regiao = new fornecedores_regioes();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_regioes.fornecedor)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                var listafornregiao = from a in dg.fornecedores_regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                      where a.fornecedor == u.vfornecedores_regioes.fornecedor
                                 select new ListaFornecedorRegiao
                                 {
                                     id = a.id,
                                     c_distribuidor = a.c_distribuidor,
                                     c_email = a.c_email,
                                     c_responsavel = a.c_responsavel,
                                     c_telefone = a.c_telefone,
                                     regiao = a.regio.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
                                  };
                ViewData["listaregiao"] = listaregiao.ToList();

                return PartialView("FornecedorRegiao", vDetalheFornecedor);
            }
        }
        public ActionResult PreencheCamposRegiao(int id_reg, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id) && a.id.Equals(id_reg)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                //carrega lista de operadoras

                var listafornregiao = from a in dg.fornecedores_regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.fornecedor == id
                                 select new ListaFornecedorRegiao
                                 {
                                     id = a.id,
                                     c_distribuidor = a.c_distribuidor,
                                     c_email = a.c_email,
                                     c_responsavel = a.c_responsavel,
                                     c_telefone = a.c_telefone,
                                     regiao = a.regio.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
                                  };
                ViewData["listaregiao"] = listaregiao.ToList();

                if (TempData["mensagemregiao"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagemregiao"];
                    TempData["mensagemregiao"] = string.Empty;
                }


                //Altera status para editar
                ViewBag.Action = "EditarRegiao";
                return View("FornecedorRegiao", vDetalheFornecedor);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRegiao(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vfornecedores_regioes.c_distribuidor = u.vfornecedores_regioes.c_distribuidor.ToUpper();
                    u.vfornecedores_regioes.c_email = u.vfornecedores_regioes.c_email.ToLower();
                    u.vfornecedores_regioes.c_responsavel = u.vfornecedores_regioes.c_responsavel.ToUpper();
                    
                    u.vfornecedores_regioes.sisdataa = DateTime.Today;
                    u.vfornecedores_regioes.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());

                    fornecedores_regioes fornregiao = dg.fornecedores_regioes.Find(u.vfornecedores_regioes.id);

                    fornregiao.c_distribuidor = u.vfornecedores_regioes.c_distribuidor;
                    fornregiao.c_email = u.vfornecedores_regioes.c_email;
                    fornregiao.c_responsavel = u.vfornecedores_regioes.c_responsavel;
                    fornregiao.c_telefone = u.vfornecedores_regioes.c_telefone;
                    fornregiao.regiao = u.vfornecedores_regioes.regiao;
                    fornregiao.sisdataa = DateTime.Today;
                    fornregiao.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());

                    var id = fornregiao.fornecedor;

                    if (TryUpdateModel(fornregiao))
                    {
                        dg.SaveChanges();
                        TempData["mensagemregiao"] = "<font style='color: green;text-align:right;font-size:11px'>Região Atualizada com Sucesso!</font>";
                    }
                    else
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Região!</font>";

                        var listafornregiao = from a in dg.fornecedores_regioes
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.fornecedor == id
                                         select new ListaFornecedorRegiao
                                         {
                                             id = a.id,
                                             c_distribuidor = a.c_distribuidor,
                                             c_email = a.c_email,
                                             c_responsavel = a.c_responsavel,
                                             c_telefone = a.c_telefone,
                                             regiao = a.regio.c_descricao,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                             fornecedor = a.fornecedor
                                         };
                        ViewData["listafornregiao"] = listafornregiao.ToList();

                        var listaregiao = from a in dg.regioes
                                          select new ListaRegiao
                                          {
                                              c_descricao = a.c_descricao,
                                              id = a.id
                                          };
                        ViewData["listaregiao"] = listaregiao.ToList();


                        return PartialView("FornecedorRegiao", u);
                    }
                    return RedirectToAction("PreencheCamposRegiao", new { id_reg = u.vfornecedores_regioes.id, id = u.vfornecedores_regioes.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listafornregiao = from a in dg.fornecedores_regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.fornecedor == u.vfornecedores_regioes.fornecedor
                                 select new ListaFornecedorRegiao
                                 {
                                     id = a.id,
                                     c_distribuidor = a.c_distribuidor,
                                     c_email = a.c_email,
                                     c_responsavel = a.c_responsavel,
                                     c_telefone = a.c_telefone,
                                     regiao = a.regio.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
                                  };
                ViewData["listaregiao"] = listaregiao.ToList();


            }
            return PartialView("FornecedorRegiao", u);
        }
        public ActionResult DeleteRegiao(int id_reg, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                try
                {
                    fornecedores_regioes fornregiao = dg.fornecedores_regioes.Find(id_reg);
                    dg.fornecedores_regioes.Remove(fornregiao);
                    dg.SaveChanges();
                }
                catch (SystemException e)
                {
                    ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                    var listafornregiao = from a in dg.fornecedores_regioes
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == id
                                     select new ListaFornecedorRegiao
                                     {
                                         id = a.id,
                                         c_distribuidor = a.c_distribuidor,
                                         c_email = a.c_email,
                                         c_responsavel = a.c_responsavel,
                                         c_telefone = a.c_telefone,
                                         regiao = a.regio.c_descricao,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                    ViewData["listafornregiao"] = listafornregiao.ToList();

                    var listaregiao = from a in dg.regioes
                                      select new ListaRegiao
                                      {
                                          c_descricao = a.c_descricao,
                                          id = a.id
                                      };
                    ViewData["listaregiao"] = listaregiao.ToList();


                    var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_regiao = new fornecedores_regioes();
                    var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                    var vDetalheFornecedor = new vModelDetalheFornecedor()
                    {
                        vfornecedor = dadosfornecedor,
                        vfornecedores_anexos = dadosfornecedor_anexo,
                        vfornecedores_bancos = dadosfornecedor_banco,
                        vfornecedores_emails = dadosfornecedor_email,
                        vfornecedores_enderecos = dadosfornecedor_endereco,
                        vfornecedores_materiais = dadosfornecedor_materiais,
                        vfornecedores_regioes = dadosfornecedor_regiao,
                        vfornecedores_telefones = dadosfornecedor_telefone
                    };
                    vDetalheFornecedor.vfornecedores_regioes.fornecedor = id;

                    ViewBag.Action = "EditarRegiao";

                    return PartialView("FornecedorRegiao", vDetalheFornecedor);
                }

            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listafornregiao = from a in dg.fornecedores_regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.fornecedor == id
                                 select new ListaFornecedorRegiao
                                 {
                                     id = a.id,
                                     c_distribuidor = a.c_distribuidor,
                                     c_email = a.c_email,
                                     c_responsavel = a.c_responsavel,
                                     c_telefone = a.c_telefone,
                                     regiao = a.regio.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
                                  };
                ViewData["listaregiao"] = listaregiao.ToList();


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = new fornecedores_regioes();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_emails.fornecedor = id;

                ViewBag.Action = string.Empty;
                ViewBag.Message = "<font style='color: green;text-align:right;font-size:11px'>Região Excluída com sucesso!</font>";

                return PartialView("FornecedorRegiao", vDetalheFornecedor);
            }

        }
        public ActionResult Regiao(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listafornregiao = from a in dg.fornecedores_regioes
                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                 from x in g.DefaultIfEmpty()
                                 from y in h.DefaultIfEmpty()
                                 where a.fornecedor == id
                                 select new ListaFornecedorRegiao
                                 {
                                     id = a.id,
                                     c_distribuidor = a.c_distribuidor,
                                     c_email = a.c_email,
                                     c_telefone = a.c_telefone,
                                     c_responsavel = a.c_responsavel,
                                     regiao = a.regio.c_descricao,
                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                     fornecedor = a.fornecedor
                                 };
                ViewData["listafornregiao"] = listafornregiao.ToList();

                var listaregiao = from a in dg.regioes
                                  select new ListaRegiao
                                  {
                                      c_descricao = a.c_descricao,
                                      id = a.id
                                  };
                ViewData["listaregiao"] = listaregiao.ToList();


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = new fornecedores_regioes();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                ViewBag.Action = string.Empty;
                ViewBag.Message = string.Empty;

                return PartialView("FornecedorRegiao", vDetalheFornecedor);
            }
        }
        // métodos materiais
        public ActionResult IncluirMaterial(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = new fornecedores_materiais();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_materiais.fornecedor = id;

                //Lista de telefone
                var listamateriais = from a in dg.fornecedores_materiais
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.fornecedor == id
                                      select new ListaFornecedorMateriais
                                      {
                                          id = a.id,
                                          material = a.materiai.c_nome_material,
                                          v_preco_ctnpm = a.v_preco_ctnpm,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                          fornecedor = a.fornecedor
                                      };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                ViewBag.Action = "InserirMaterial";

                return PartialView("FornecedorMateriais", vDetalheFornecedor);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirMaterial(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                   
                    u.vfornecedores_materiais.sisdatai = DateTime.Today;
                    u.vfornecedores_materiais.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());


                    try
                    {
                        dg.fornecedores_materiais.Add(u.vfornecedores_materiais);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";



                        var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                        var dadosfornecedor_materiais = new fornecedores_materiais();
                        var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();

                        var vDetalheFornecedor = new vModelDetalheFornecedor()
                        {
                            vfornecedor = dadosfornecedor,
                            vfornecedores_anexos = dadosfornecedor_anexo,
                            vfornecedores_bancos = dadosfornecedor_banco,
                            vfornecedores_emails = dadosfornecedor_email,
                            vfornecedores_enderecos = dadosfornecedor_endereco,
                            vfornecedores_materiais = dadosfornecedor_materiais,
                            vfornecedores_regioes = dadosfornecedor_regiao,
                            vfornecedores_telefones = dadosfornecedor_telefone
                        };

                        var listamateriais = from a in dg.fornecedores_materiais
                                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                             from x in g.DefaultIfEmpty()
                                             from y in h.DefaultIfEmpty()
                                             where a.fornecedor == u.vfornecedores_materiais.fornecedor
                                             select new ListaFornecedorMateriais
                                             {
                                                 id = a.id,
                                                 material = a.materiai.c_nome_material,
                                                 v_preco_ctnpm = a.v_preco_ctnpm,
                                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                 fornecedor = a.fornecedor
                                             };
                        ViewData["listamateriais"] = listamateriais.ToList();

                        var lstmaterial = from a in dg.materiais
                                          select new ListaMaterial
                                          {
                                              c_nome_material = a.c_nome_material,
                                              id = a.id
                                          };
                        ViewData["lstmaterial"] = lstmaterial.ToList();

                        return PartialView("FornecedorMateriais", vDetalheFornecedor);
                    }

                    TempData["mensagemmaterial"] = "<font style='color: green;text-align:right;font-size:11px'>Material inserido com sucesso!</font>";
                    return RedirectToAction("PreencheCamposMaterial", new { id_mat = u.vfornecedores_materiais.id, id = u.vfornecedores_materiais.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();
                var dadosfornecedor_materiais = new fornecedores_materiais();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(u.vfornecedores_materiais.fornecedor)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                var listamateriais = from a in dg.fornecedores_materiais
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == u.vfornecedores_materiais.fornecedor
                                     select new ListaFornecedorMateriais
                                     {
                                         id = a.id,
                                         material = a.materiai.c_nome_material,
                                         v_preco_ctnpm = a.v_preco_ctnpm,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                return PartialView("FornecedorMateriais", vDetalheFornecedor);
            }
        }
        public ActionResult PreencheCamposMaterial(int id_mat, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = dg.fornecedores_materiais.Where(a => a.fornecedor.Equals(id) && a.id.Equals(id_mat)).FirstOrDefault();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                //carrega lista de operadoras

                var listamateriais = from a in dg.fornecedores_materiais
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == id
                                     select new ListaFornecedorMateriais
                                     {
                                         id = a.id,
                                         material = a.materiai.c_nome_material,
                                         v_preco_ctnpm = a.v_preco_ctnpm,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                if (TempData["mensagemmaterial"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagemmaterial"];
                    TempData["mensagemmaterial"] = string.Empty;
                }


                //Altera status para editar
                ViewBag.Action = "EditarMaterial";
                return View("FornecedorMateriais", vDetalheFornecedor);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarMaterial(Models.vModelDetalheFornecedor u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    

                    fornecedores_materiais fornmaterial = dg.fornecedores_materiais.Find(u.vfornecedores_materiais.id);

                    fornmaterial.v_preco_ctnpm = u.vfornecedores_materiais.v_preco_ctnpm;
                    fornmaterial.material = u.vfornecedores_materiais.material;
                    fornmaterial.sisdataa = DateTime.Today;
                    fornmaterial.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());

                    var id = fornmaterial.fornecedor;

                    if (TryUpdateModel(fornmaterial))
                    {
                        dg.SaveChanges();
                        TempData["mensagemmaterial"] = "<font style='color: green;text-align:right;font-size:11px'>Material Atualizado com Sucesso!</font>";
                    }
                    else
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Material!</font>";

                        var listamateriais = from a in dg.fornecedores_materiais
                                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                             from x in g.DefaultIfEmpty()
                                             from y in h.DefaultIfEmpty()
                                             where a.fornecedor == id
                                             select new ListaFornecedorMateriais
                                             {
                                                 id = a.id,
                                                 material = a.materiai.c_nome_material,
                                                 v_preco_ctnpm = a.v_preco_ctnpm,
                                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                                 fornecedor = a.fornecedor
                                             };
                        ViewData["listamateriais"] = listamateriais.ToList();

                        var lstmaterial = from a in dg.materiais
                                          select new ListaMaterial
                                          {
                                              c_nome_material = a.c_nome_material,
                                              id = a.id
                                          };
                        ViewData["lstmaterial"] = lstmaterial.ToList();


                        return PartialView("FornecedorMateriais", u);
                    }
                    return RedirectToAction("PreencheCamposMaterial", new { id_mat = u.vfornecedores_materiais.id, id = u.vfornecedores_materiais.fornecedor });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listamateriais = from a in dg.fornecedores_materiais
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == u.vfornecedores_materiais.fornecedor
                                     select new ListaFornecedorMateriais
                                     {
                                         id = a.id,
                                         material = a.materiai.c_nome_material,
                                         v_preco_ctnpm = a.v_preco_ctnpm,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();


            }
            return PartialView("FornecedorMateriais", u);
        }
        public ActionResult DeleteMaterial(int id_mat, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                try
                {
                    fornecedores_materiais fornmaterial = dg.fornecedores_materiais.Find(id_mat);
                    dg.fornecedores_materiais.Remove(fornmaterial);
                    dg.SaveChanges();
                }
                catch (SystemException e)
                {
                    ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                    var listamateriais = from a in dg.fornecedores_materiais
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.fornecedor == id
                                         select new ListaFornecedorMateriais
                                         {
                                             id = a.id,
                                             material = a.materiai.c_nome_material,
                                             v_preco_ctnpm = a.v_preco_ctnpm,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                             fornecedor = a.fornecedor
                                         };
                    ViewData["listamateriais"] = listamateriais.ToList();

                    var lstmaterial = from a in dg.materiais
                                      select new ListaMaterial
                                      {
                                          c_nome_material = a.c_nome_material,
                                          id = a.id
                                      };
                    ViewData["lstmaterial"] = lstmaterial.ToList();


                    var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                    var dadosfornecedor_materiais = new fornecedores_materiais();
                    var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                    var vDetalheFornecedor = new vModelDetalheFornecedor()
                    {
                        vfornecedor = dadosfornecedor,
                        vfornecedores_anexos = dadosfornecedor_anexo,
                        vfornecedores_bancos = dadosfornecedor_banco,
                        vfornecedores_emails = dadosfornecedor_email,
                        vfornecedores_enderecos = dadosfornecedor_endereco,
                        vfornecedores_materiais = dadosfornecedor_materiais,
                        vfornecedores_regioes = dadosfornecedor_regiao,
                        vfornecedores_telefones = dadosfornecedor_telefone
                    };
                    vDetalheFornecedor.vfornecedores_materiais.fornecedor = id;

                    ViewBag.Action = "EditarMaterial";

                    return PartialView("FornecedorMateriais", vDetalheFornecedor);
                }

            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listamateriais = from a in dg.fornecedores_materiais
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == id
                                     select new ListaFornecedorMateriais
                                     {
                                         id = a.id,
                                         material = a.materiai.c_nome_material,
                                         v_preco_ctnpm = a.v_preco_ctnpm,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = new fornecedores_materiais();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };
                vDetalheFornecedor.vfornecedores_materiais.fornecedor = id;

                ViewBag.Action = string.Empty;
                ViewBag.Message = "<font style='color: green;text-align:right;font-size:11px'>Material Excluído com sucesso!</font>";

                return PartialView("FornecedorMateriais", vDetalheFornecedor);
            }

        }
        public ActionResult Materiais(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listamateriais = from a in dg.fornecedores_materiais
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.fornecedor == id
                                     select new ListaFornecedorMateriais
                                     {
                                         id = a.id,
                                         material = a.materiai.c_nome_material,
                                         v_preco_ctnpm = a.v_preco_ctnpm,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario),
                                         fornecedor = a.fornecedor
                                     };
                ViewData["listamateriais"] = listamateriais.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      c_nome_material = a.c_nome_material,
                                      id = a.id
                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();


                var dadosfornecedor = dg.fornecedores.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosfornecedor_banco = dg.fornecedores_bancos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_endereco = dg.fornecedores_enderecos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_email = dg.fornecedores_emails.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_anexo = dg.fornecedores_anexos.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_regiao = dg.fornecedores_regioes.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();
                var dadosfornecedor_materiais = new fornecedores_materiais();
                var dadosfornecedor_telefone = dg.fornecedores_telefones.Where(a => a.fornecedor.Equals(id)).FirstOrDefault();

                var vDetalheFornecedor = new vModelDetalheFornecedor()
                {
                    vfornecedor = dadosfornecedor,
                    vfornecedores_anexos = dadosfornecedor_anexo,
                    vfornecedores_bancos = dadosfornecedor_banco,
                    vfornecedores_emails = dadosfornecedor_email,
                    vfornecedores_enderecos = dadosfornecedor_endereco,
                    vfornecedores_materiais = dadosfornecedor_materiais,
                    vfornecedores_regioes = dadosfornecedor_regiao,
                    vfornecedores_telefones = dadosfornecedor_telefone
                };

                ViewBag.Action = string.Empty;
                ViewBag.Message = string.Empty;

                return PartialView("FornecedorMateriais", vDetalheFornecedor);
            }
        }
    }
}