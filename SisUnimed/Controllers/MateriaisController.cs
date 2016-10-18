using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class MateriaisController : Controller
    {
        //
        // GET: /Materiais/

        public ActionResult Materiais()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;

                    // busca fornecedores na tabela
                    var materiais = from a in up.materiais
                                    join b in up.usuarios on a.sisusuarioi equals b.id into g
                                    join c in up.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    select new ListaMaterial
                                    {
                                        id = a.id,
                                        c_cod_ref_fabr = a.c_cod_ref_fabr,
                                        c_codigo_tuss = a.c_codigo_tuss,
                                        c_composicao = a.c_composicao,
                                        c_descricao_generica = a.c_descricao_generica,
                                        c_justificativa_alteracao = a.c_justificativa_alteracao,
                                        c_nome_comercial = a.c_nome_comercial,
                                        c_nome_material = a.c_nome_material,
                                        c_registro_anvisa = a.c_registro_anvisa,
                                        c_tnumm = a.c_tnumm,
                                        classificacao = a.classificaco.c_descricao,
                                        d_fim_vigencia = a.d_fim_vigencia,
                                        d_inicio_vigencia = a.d_inicio_vigencia,
                                        d_validade = a.d_validade,
                                        f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                        f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                        f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                        f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                        fornecedor = a.fornecedore.c_razao_social,
                                        marca = a.marca1.c_nome,
                                        v_preco = a.v_preco,
                                        unidade = a.unidade1.c_sigla,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                    };
                    ViewData["listamaterial"] = materiais.ToList();

                    var classificacao = from a in up.classificacoes
                                        select new ListaClassificacao
                                        {
                                            id = a.id,
                                            descricao = a.c_descricao
                                        };

                    ViewData["listaclassificacao"] = classificacao.ToList();

                    var fornecedor = from a in up.fornecedores
                                     select new ListaFornecedor
                                     {
                                         c_razao_social = a.c_razao_social,
                                         id = a.id
                                     };
                    ViewData["listafornecedor"] = fornecedor.ToList();

                    var marca = from a in up.marcas
                                select new ListaMarca
                                {
                                    c_nome = a.c_nome,
                                    id = a.id
                                };
                    ViewData["listamarca"] = marca.ToList();

                    var unidade = from a in up.unidades
                                  select new ListaUnidade
                                  {
                                      c_sigla = a.c_sigla,
                                      id = a.id
                                  };
                    ViewData["listaunidade"] = unidade.ToList();

                    //lista informacao
                    var listainformacao = from a in up.materiais_informacoes
                                          join b in up.usuarios on a.sisusuarioi equals b.id into g
                                          join c in up.usuarios on a.sisusuarioa equals c.id into h
                                          from x in g.DefaultIfEmpty()
                                          from y in h.DefaultIfEmpty()
                                          where a.material == 0
                                          select new ListaMatInformacao
                                          {
                                              id = a.id,
                                              c_informacoes = a.c_informacoes,
                                              material = a.material,
                                              sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                              sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                              sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                              sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                          };
                    ViewData["listainformacao"] = listainformacao.ToList();

                    // lista procedimento
                    var listaprocedimentos = from a in up.materiais_procedimentos
                                             join b in up.usuarios on a.sisusuarioi equals b.id into g
                                             join c in up.usuarios on a.sisusuarioa equals c.id into h
                                             from x in g.DefaultIfEmpty()
                                             from y in h.DefaultIfEmpty()
                                             where a.material == 0
                                             select new ListaMatProcedimento
                                             {
                                                 id = a.id,
                                                 procedimento = a.procedimento1.c_descricao,
                                                 material = a.material,
                                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                             };
                    ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                    var lstprocedimento = from a in up.procedimentos
                                          select new ListaProcedimentos
                                          {
                                              id = a.id,
                                              c_codigo = a.c_codigo,
                                              c_descricao = a.c_descricao
                                          };
                    ViewData["lstprocedimento"] = lstprocedimento.ToList();

                    // lista similares
                    var listasimilares = from a in up.materiais_similares
                                         join b in up.usuarios on a.sisusuarioi equals b.id into g
                                         join c in up.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == 0
                                         select new ListaMatSimilar
                                         {
                                             id = a.id,
                                             material_similar = a.materiai1.c_nome_material,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                    ViewData["listasimilares"] = listasimilares.ToList();

                    var lstmaterial = from a in up.materiais
                                      select new ListaMaterial
                                      {
                                          id = a.id,
                                          c_nome_material = a.c_nome_material

                                      };
                    ViewData["lstmaterial"] = lstmaterial.ToList();

                }
                ViewBag.Titulo = "Cadastro de Materiais";
                ViewBag.Message = TempData["mensagem"];
                TempData["mensagem"] = "";

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = new materiai(),
                    vmateriais_informacoes = new materiais_informacoes(),
                    vmateriais_procedimentos = new materiais_procedimentos(),
                    vmateriais_similares = new materiais_similares()
                };

                return View(vDetalheMaterial);

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
                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                // busca fornecedores na tabela
                var materiais = from a in dg.materiais
                                join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                from x in g.DefaultIfEmpty()
                                from y in h.DefaultIfEmpty()
                                select new ListaMaterial
                                {
                                    id = a.id,
                                    c_cod_ref_fabr = a.c_cod_ref_fabr,
                                    c_codigo_tuss = a.c_codigo_tuss,
                                    c_composicao = a.c_composicao,
                                    c_descricao_generica = a.c_descricao_generica,
                                    c_justificativa_alteracao = a.c_justificativa_alteracao,
                                    c_nome_comercial = a.c_nome_comercial,
                                    c_nome_material = a.c_nome_material,
                                    c_registro_anvisa = a.c_registro_anvisa,
                                    c_tnumm = a.c_tnumm,
                                    classificacao = a.classificaco.c_descricao,
                                    d_fim_vigencia = a.d_fim_vigencia,
                                    d_inicio_vigencia = a.d_inicio_vigencia,
                                    d_validade = a.d_validade,
                                    f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                    f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                    f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                    f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                    fornecedor = a.fornecedore.c_razao_social,
                                    marca = a.marca1.c_nome,
                                    v_preco = a.v_preco,
                                    unidade = a.unidade1.c_sigla,
                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                };
                ViewData["listamaterial"] = materiais.ToList();

                var classificacao = from a in dg.classificacoes
                                    select new ListaClassificacao
                                    {
                                        id = a.id,
                                        descricao = a.c_descricao
                                    };

                ViewData["listaclassificacao"] = classificacao.ToList();

                var fornecedor = from a in dg.fornecedores
                                 select new ListaFornecedor
                                 {
                                     c_razao_social = a.c_razao_social,
                                     id = a.id
                                 };
                ViewData["listafornecedor"] = fornecedor.ToList();

                var marca = from a in dg.marcas
                            select new ListaMarca
                            {
                                c_nome = a.c_nome,
                                id = a.id
                            };
                ViewData["listamarca"] = marca.ToList();

                var unidade = from a in dg.unidades
                              select new ListaUnidade
                              {
                                  c_sigla = a.c_sigla,
                                  id = a.id
                              };
                ViewData["listaunidade"] = unidade.ToList();

                //lista informacao
                var listainformacao = from a in dg.materiais_informacoes
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.material == id
                                      select new ListaMatInformacao
                                      {
                                          id = a.id,
                                          c_informacoes = a.c_informacoes,
                                          material = a.material,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                      };
                ViewData["listainformacao"] = listainformacao.ToList();

                // lista procedimento
                var listaprocedimentos = from a in dg.materiais_procedimentos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == id
                                         select new ListaMatProcedimento
                                         {
                                             id = a.id,
                                             procedimento = a.procedimento1.c_descricao,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();

                // lista similares
                var listasimilares = from a in dg.materiais_similares
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.material == id
                                     select new ListaMatSimilar
                                     {
                                         id = a.id,
                                         material_similar = a.materiai1.c_nome_material,
                                         material = a.material,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      id = a.id,
                                      c_nome_material = a.c_nome_material

                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

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
                return View("Materiais", vDetalheMaterial);
            }
        }
        public ActionResult Pesquisa(string tipo, string campo, string pesquisa)
        {
            TempData["mensagem"] = "";
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                if (pesquisa == "")
                {
                    // busca fornecedores na tabela
                    var materiais = from a in dg.materiais
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    select new ListaMaterial
                                    {
                                        id = a.id,
                                        c_cod_ref_fabr = a.c_cod_ref_fabr,
                                        c_codigo_tuss = a.c_codigo_tuss,
                                        c_composicao = a.c_composicao,
                                        c_descricao_generica = a.c_descricao_generica,
                                        c_justificativa_alteracao = a.c_justificativa_alteracao,
                                        c_nome_comercial = a.c_nome_comercial,
                                        c_nome_material = a.c_nome_material,
                                        c_registro_anvisa = a.c_registro_anvisa,
                                        c_tnumm = a.c_tnumm,
                                        classificacao = a.classificaco.c_descricao,
                                        d_fim_vigencia = a.d_fim_vigencia,
                                        d_inicio_vigencia = a.d_inicio_vigencia,
                                        d_validade = a.d_validade,
                                        f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                        f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                        f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                        f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                        fornecedor = a.fornecedore.c_razao_social,
                                        marca = a.marca1.c_nome,
                                        v_preco = a.v_preco,
                                        unidade = a.unidade1.c_sigla,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                    };
                    ViewData["listamaterial"] = materiais.ToList();


                }
                if (campo == "codigo" && pesquisa != string.Empty)
                {
                    int idoperadora = int.Parse(pesquisa);
                    // busca fornecedores na tabela
                    var materiais = from a in dg.materiais
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.id == idoperadora
                                    select new ListaMaterial
                                    {
                                        id = a.id,
                                        c_cod_ref_fabr = a.c_cod_ref_fabr,
                                        c_codigo_tuss = a.c_codigo_tuss,
                                        c_composicao = a.c_composicao,
                                        c_descricao_generica = a.c_descricao_generica,
                                        c_justificativa_alteracao = a.c_justificativa_alteracao,
                                        c_nome_comercial = a.c_nome_comercial,
                                        c_nome_material = a.c_nome_material,
                                        c_registro_anvisa = a.c_registro_anvisa,
                                        c_tnumm = a.c_tnumm,
                                        classificacao = a.classificaco.c_descricao,
                                        d_fim_vigencia = a.d_fim_vigencia,
                                        d_inicio_vigencia = a.d_inicio_vigencia,
                                        d_validade = a.d_validade,
                                        f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                        f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                        f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                        f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                        fornecedor = a.fornecedore.c_razao_social,
                                        marca = a.marca1.c_nome,
                                        v_preco = a.v_preco,
                                        unidade = a.unidade1.c_sigla,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                    };
                    ViewData["listamaterial"] = materiais.ToList();


                }
                else
                {
                    if (tipo == "inicia")
                    {
                        // busca fornecedores na tabela
                        var materiais = from a in dg.materiais
                                        join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                        join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                        from x in g.DefaultIfEmpty()
                                        from y in h.DefaultIfEmpty()
                                        where a.c_nome_material.StartsWith(pesquisa)
                                        select new ListaMaterial
                                        {
                                            id = a.id,
                                            c_cod_ref_fabr = a.c_cod_ref_fabr,
                                            c_codigo_tuss = a.c_codigo_tuss,
                                            c_composicao = a.c_composicao,
                                            c_descricao_generica = a.c_descricao_generica,
                                            c_justificativa_alteracao = a.c_justificativa_alteracao,
                                            c_nome_comercial = a.c_nome_comercial,
                                            c_nome_material = a.c_nome_material,
                                            c_registro_anvisa = a.c_registro_anvisa,
                                            c_tnumm = a.c_tnumm,
                                            classificacao = a.classificaco.c_descricao,
                                            d_fim_vigencia = a.d_fim_vigencia,
                                            d_inicio_vigencia = a.d_inicio_vigencia,
                                            d_validade = a.d_validade,
                                            f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                            f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                            f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                            f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                            fornecedor = a.fornecedore.c_razao_social,
                                            marca = a.marca1.c_nome,
                                            v_preco = a.v_preco,
                                            unidade = a.unidade1.c_sigla,
                                            sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                            sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                            sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                            sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                        };
                        ViewData["listamaterial"] = materiais.ToList();


                    }
                    else if (tipo == "termina")
                    {
                        // busca fornecedores na tabela
                        var materiais = from a in dg.materiais
                                        join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                        join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                        from x in g.DefaultIfEmpty()
                                        from y in h.DefaultIfEmpty()
                                        where a.c_nome_material.EndsWith(pesquisa)
                                        select new ListaMaterial
                                        {
                                            id = a.id,
                                            c_cod_ref_fabr = a.c_cod_ref_fabr,
                                            c_codigo_tuss = a.c_codigo_tuss,
                                            c_composicao = a.c_composicao,
                                            c_descricao_generica = a.c_descricao_generica,
                                            c_justificativa_alteracao = a.c_justificativa_alteracao,
                                            c_nome_comercial = a.c_nome_comercial,
                                            c_nome_material = a.c_nome_material,
                                            c_registro_anvisa = a.c_registro_anvisa,
                                            c_tnumm = a.c_tnumm,
                                            classificacao = a.classificaco.c_descricao,
                                            d_fim_vigencia = a.d_fim_vigencia,
                                            d_inicio_vigencia = a.d_inicio_vigencia,
                                            d_validade = a.d_validade,
                                            f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                            f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                            f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                            f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                            fornecedor = a.fornecedore.c_razao_social,
                                            marca = a.marca1.c_nome,
                                            v_preco = a.v_preco,
                                            unidade = a.unidade1.c_sigla,
                                            sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                            sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                            sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                            sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                        };
                        ViewData["listamaterial"] = materiais.ToList();


                    }
                    else
                    {
                        // busca fornecedores na tabela
                        var materiais = from a in dg.materiais
                                        join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                        join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                        from x in g.DefaultIfEmpty()
                                        from y in h.DefaultIfEmpty()
                                        where a.c_nome_material.Contains(pesquisa)
                                        select new ListaMaterial
                                        {
                                            id = a.id,
                                            c_cod_ref_fabr = a.c_cod_ref_fabr,
                                            c_codigo_tuss = a.c_codigo_tuss,
                                            c_composicao = a.c_composicao,
                                            c_descricao_generica = a.c_descricao_generica,
                                            c_justificativa_alteracao = a.c_justificativa_alteracao,
                                            c_nome_comercial = a.c_nome_comercial,
                                            c_nome_material = a.c_nome_material,
                                            c_registro_anvisa = a.c_registro_anvisa,
                                            c_tnumm = a.c_tnumm,
                                            classificacao = a.classificaco.c_descricao,
                                            d_fim_vigencia = a.d_fim_vigencia,
                                            d_inicio_vigencia = a.d_inicio_vigencia,
                                            d_validade = a.d_validade,
                                            f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                            f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                            f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                            f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                            fornecedor = a.fornecedore.c_razao_social,
                                            marca = a.marca1.c_nome,
                                            v_preco = a.v_preco,
                                            unidade = a.unidade1.c_sigla,
                                            sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                            sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                            sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                            sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                        };
                        ViewData["listamaterial"] = materiais.ToList();


                    }
                }

            }
            return PartialView("ListaMateriais");
        }
        public ActionResult Incluir()
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                //carrega permissao de usuários
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;

                ViewBag.Titulo = "Cadastro de Materiais";

                // busca fornecedores na tabela
                var materiais = from a in dg.materiais
                                join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                from x in g.DefaultIfEmpty()
                                from y in h.DefaultIfEmpty()
                                select new ListaMaterial
                                {
                                    id = a.id,
                                    c_cod_ref_fabr = a.c_cod_ref_fabr,
                                    c_codigo_tuss = a.c_codigo_tuss,
                                    c_composicao = a.c_composicao,
                                    c_descricao_generica = a.c_descricao_generica,
                                    c_justificativa_alteracao = a.c_justificativa_alteracao,
                                    c_nome_comercial = a.c_nome_comercial,
                                    c_nome_material = a.c_nome_material,
                                    c_registro_anvisa = a.c_registro_anvisa,
                                    c_tnumm = a.c_tnumm,
                                    classificacao = a.classificaco.c_descricao,
                                    d_fim_vigencia = a.d_fim_vigencia,
                                    d_inicio_vigencia = a.d_inicio_vigencia,
                                    d_validade = a.d_validade,
                                    f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                    f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                    f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                    f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                    fornecedor = a.fornecedore.c_razao_social,
                                    marca = a.marca1.c_nome,
                                    v_preco = a.v_preco,
                                    unidade = a.unidade1.c_sigla,
                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                };
                ViewData["listamaterial"] = materiais.ToList();

                var classificacao = from a in dg.classificacoes
                                    select new ListaClassificacao
                                    {
                                        id = a.id,
                                        descricao = a.c_descricao
                                    };

                ViewData["listaclassificacao"] = classificacao.ToList();

                var fornecedor = from a in dg.fornecedores
                                 select new ListaFornecedor
                                 {
                                     c_razao_social = a.c_razao_social,
                                     id = a.id
                                 };
                ViewData["listafornecedor"] = fornecedor.ToList();

                var marca = from a in dg.marcas
                            select new ListaMarca
                            {
                                c_nome = a.c_nome,
                                id = a.id
                            };
                ViewData["listamarca"] = marca.ToList();

                var unidade = from a in dg.unidades
                              select new ListaUnidade
                              {
                                  c_sigla = a.c_sigla,
                                  id = a.id
                              };
                ViewData["listaunidade"] = unidade.ToList();

                //lista informacao
                var listainformacao = from a in dg.materiais_informacoes
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.material == 0
                                      select new ListaMatInformacao
                                      {
                                          id = a.id,
                                          c_informacoes = a.c_informacoes,
                                          material = a.material,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                      };
                ViewData["listainformacao"] = listainformacao.ToList();

                // lista procedimento
                var listaprocedimentos = from a in dg.materiais_procedimentos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == 0
                                         select new ListaMatProcedimento
                                         {
                                             id = a.id,
                                             procedimento = a.procedimento1.c_descricao,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();

                // lista similares
                var listasimilares = from a in dg.materiais_similares
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.material == 0
                                     select new ListaMatSimilar
                                     {
                                         id = a.id,
                                         material_similar = a.materiai1.c_nome_material,
                                         material = a.material,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      id = a.id,
                                      c_nome_material = a.c_nome_material

                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                //prepara model para inserção
                var material = new materiai();
                var materiais_informacoes = new materiais_informacoes();
                var materiais_similares = new materiais_similares();
                var materiais_procedimentos = new materiais_procedimentos();


                var vDetalheMateriais = new vModelDetalheMateriais
                {
                    vmateriais = material,
                    vmateriais_informacoes = materiais_informacoes,
                    vmateriais_similares = materiais_similares,
                    vmateriais_procedimentos = materiais_procedimentos
                };

                ViewBag.Action = "Inserir";

                return View("Materiais", vDetalheMateriais);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inserir(Models.vModelDetalheMateriais u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.materiais_i.Equals(1) && a.id_usuario.Equals(id_usuario)).Count();
                    if (up >= 1)
                    {
                        try
                        {
                            u.vmateriais.c_cod_ref_fabr = u.vmateriais.c_cod_ref_fabr.ToUpper();
                            u.vmateriais.c_codigo_tuss = u.vmateriais.c_codigo_tuss.ToUpper();
                            u.vmateriais.c_composicao = u.vmateriais.c_composicao.ToUpper();
                            u.vmateriais.c_descricao_generica = u.vmateriais.c_descricao_generica.ToUpper();
                            u.vmateriais.c_nome_comercial = u.vmateriais.c_nome_comercial.ToUpper();
                            u.vmateriais.c_nome_material = u.vmateriais.c_nome_material.ToUpper();
                            u.vmateriais.c_registro_anvisa = u.vmateriais.c_registro_anvisa.ToUpper();
                            u.vmateriais.c_tnumm = u.vmateriais.c_tnumm.ToUpper();

                            u.vmateriais.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());
                            u.vmateriais.sisdatai = DateTime.Today;
                            dg.materiais.Add(u.vmateriais);
                            dg.SaveChanges();

                        }
                        catch (SystemException e)
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                            var up1 = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                            ViewData["usuario_permissao"] = up1;
                            // busca fornecedores na tabela
                            var materiais = from a in dg.materiais
                                            join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                            join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                            from x in g.DefaultIfEmpty()
                                            from y in h.DefaultIfEmpty()
                                            select new ListaMaterial
                                            {
                                                id = a.id,
                                                c_cod_ref_fabr = a.c_cod_ref_fabr,
                                                c_codigo_tuss = a.c_codigo_tuss,
                                                c_composicao = a.c_composicao,
                                                c_descricao_generica = a.c_descricao_generica,
                                                c_justificativa_alteracao = a.c_justificativa_alteracao,
                                                c_nome_comercial = a.c_nome_comercial,
                                                c_nome_material = a.c_nome_material,
                                                c_registro_anvisa = a.c_registro_anvisa,
                                                c_tnumm = a.c_tnumm,
                                                classificacao = a.classificaco.c_descricao,
                                                d_fim_vigencia = a.d_fim_vigencia,
                                                d_inicio_vigencia = a.d_inicio_vigencia,
                                                d_validade = a.d_validade,
                                                f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                                f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                                f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                                f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                                fornecedor = a.fornecedore.c_razao_social,
                                                marca = a.marca1.c_nome,
                                                v_preco = a.v_preco,
                                                unidade = a.unidade1.c_sigla,
                                                sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                            };
                            ViewData["listamaterial"] = materiais.ToList();

                            var classificacao = from a in dg.classificacoes
                                                select new ListaClassificacao
                                                {
                                                    id = a.id,
                                                    descricao = a.c_descricao
                                                };

                            ViewData["listaclassificacao"] = classificacao.ToList();

                            var fornecedor = from a in dg.fornecedores
                                             select new ListaFornecedor
                                             {
                                                 c_razao_social = a.c_razao_social,
                                                 id = a.id
                                             };
                            ViewData["listafornecedor"] = fornecedor.ToList();

                            var marca = from a in dg.marcas
                                        select new ListaMarca
                                        {
                                            c_nome = a.c_nome,
                                            id = a.id
                                        };
                            ViewData["listamarca"] = marca.ToList();

                            var unidade = from a in dg.unidades
                                          select new ListaUnidade
                                          {
                                              c_sigla = a.c_sigla,
                                              id = a.id
                                          };
                            ViewData["listaunidade"] = unidade.ToList();

                            //lista informacao
                            var listainformacao = from a in dg.materiais_informacoes
                                                  join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                  join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                  from x in g.DefaultIfEmpty()
                                                  from y in h.DefaultIfEmpty()
                                                  where a.material == u.vmateriais.id
                                                  select new ListaMatInformacao
                                                  {
                                                      id = a.id,
                                                      c_informacoes = a.c_informacoes,
                                                      material = a.material,
                                                      sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                      sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                      sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                      sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                                  };
                            ViewData["listainformacao"] = listainformacao.ToList();

                            // lsita procedimento
                            var listaprocedimentos = from a in dg.materiais_procedimentos
                                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                     from x in g.DefaultIfEmpty()
                                                     from y in h.DefaultIfEmpty()
                                                     where a.material == u.vmateriais.id
                                                     select new ListaMatProcedimento
                                                     {
                                                         id = a.id,
                                                         procedimento = a.procedimento1.c_descricao,
                                                         material = a.material,
                                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                                     };
                            ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                            var lstprocedimento = from a in dg.procedimentos
                                                  select new ListaProcedimentos
                                                  {
                                                      id = a.id,
                                                      c_codigo = a.c_codigo,
                                                      c_descricao = a.c_descricao
                                                  };
                            ViewData["lstprocedimento"] = lstprocedimento.ToList();

                            // lista similares
                            var listasimilares = from a in dg.materiais_similares
                                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                 from x in g.DefaultIfEmpty()
                                                 from y in h.DefaultIfEmpty()
                                                 where a.material == u.vmateriais.id
                                                 select new ListaMatSimilar
                                                 {
                                                     id = a.id,
                                                     material_similar = a.materiai1.c_nome_material,
                                                     material = a.material,
                                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                                 };
                            ViewData["listasimilares"] = listasimilares.ToList();

                            var lstmaterial = from a in dg.materiais
                                              select new ListaMaterial
                                              {
                                                  id = a.id,
                                                  c_nome_material = a.c_nome_material

                                              };
                            ViewData["lstmaterial"] = lstmaterial.ToList();

                            ViewBag.Titulo = "Cadastro de Materiais";
                            return RedirectToAction("Materiais");
                        }

                        TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Material Inserido com Sucesso!</font>";
                        ViewBag.Action = "";
                        var id = u.vmateriais.id;
                        return RedirectToAction("PreencheCampos", new { id = id });

                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                // busca fornecedores na tabela
                var materiais = from a in dg.materiais
                                join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                from x in g.DefaultIfEmpty()
                                from y in h.DefaultIfEmpty()
                                select new ListaMaterial
                                {
                                    id = a.id,
                                    c_cod_ref_fabr = a.c_cod_ref_fabr,
                                    c_codigo_tuss = a.c_codigo_tuss,
                                    c_composicao = a.c_composicao,
                                    c_descricao_generica = a.c_descricao_generica,
                                    c_justificativa_alteracao = a.c_justificativa_alteracao,
                                    c_nome_comercial = a.c_nome_comercial,
                                    c_nome_material = a.c_nome_material,
                                    c_registro_anvisa = a.c_registro_anvisa,
                                    c_tnumm = a.c_tnumm,
                                    classificacao = a.classificaco.c_descricao,
                                    d_fim_vigencia = a.d_fim_vigencia,
                                    d_inicio_vigencia = a.d_inicio_vigencia,
                                    d_validade = a.d_validade,
                                    f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                    f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                    f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                    f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                    fornecedor = a.fornecedore.c_razao_social,
                                    marca = a.marca1.c_nome,
                                    v_preco = a.v_preco,
                                    unidade = a.unidade1.c_sigla,
                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                };
                ViewData["listamaterial"] = materiais.ToList();

                var classificacao = from a in dg.classificacoes
                                    select new ListaClassificacao
                                    {
                                        id = a.id,
                                        descricao = a.c_descricao
                                    };

                ViewData["listaclassificacao"] = classificacao.ToList();

                var fornecedor = from a in dg.fornecedores
                                 select new ListaFornecedor
                                 {
                                     c_razao_social = a.c_razao_social,
                                     id = a.id
                                 };
                ViewData["listafornecedor"] = fornecedor.ToList();

                var marca = from a in dg.marcas
                            select new ListaMarca
                            {
                                c_nome = a.c_nome,
                                id = a.id
                            };
                ViewData["listamarca"] = marca.ToList();

                var unidade = from a in dg.unidades
                              select new ListaUnidade
                              {
                                  c_sigla = a.c_sigla,
                                  id = a.id
                              };
                ViewData["listaunidade"] = unidade.ToList();

                // lista informacao
                var listainformacao = from a in dg.materiais_informacoes
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.material == u.vmateriais.id
                                      select new ListaMatInformacao
                                      {
                                          id = a.id,
                                          c_informacoes = a.c_informacoes,
                                          material = a.material,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                      };
                ViewData["listainformacao"] = listainformacao.ToList();

                // lsita procedimento
                var listaprocedimentos = from a in dg.materiais_procedimentos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == u.vmateriais.id
                                         select new ListaMatProcedimento
                                         {
                                             id = a.id,
                                             procedimento = a.procedimento1.c_descricao,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();

                // lista similares
                var listasimilares = from a in dg.materiais_similares
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.material == u.vmateriais.id
                                     select new ListaMatSimilar
                                     {
                                         id = a.id,
                                         material_similar = a.materiai1.c_nome_material,
                                         material = a.material,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      id = a.id,
                                      c_nome_material = a.c_nome_material

                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();
            }
            ViewBag.Action = "Inserir";
            ViewBag.Titulo = "Cadastro de Materiais";
            return View("Materiais", u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Models.vModelDetalheMateriais u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                    var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.materiais_a.Equals(1)).Count();
                    if (up >= 1)
                    {
                        materiai material = dg.materiais.Find(u.vmateriais.id);

                        material.c_cod_ref_fabr = u.vmateriais.c_cod_ref_fabr.ToUpper();
                        material.c_codigo_tuss = u.vmateriais.c_codigo_tuss.ToUpper();
                        material.c_composicao = u.vmateriais.c_composicao.ToUpper();
                        material.c_descricao_generica = u.vmateriais.c_descricao_generica.ToUpper();
                        material.c_justificativa_alteracao = u.vmateriais.c_justificativa_alteracao.ToUpper();
                        material.c_nome_comercial = u.vmateriais.c_nome_comercial.ToUpper();
                        material.c_nome_material = u.vmateriais.c_nome_material.ToUpper();
                        material.c_registro_anvisa = u.vmateriais.c_registro_anvisa.ToUpper();
                        material.c_tnumm = u.vmateriais.c_tnumm.ToUpper();
                        material.classificacao = u.vmateriais.classificacao;
                        material.d_fim_vigencia = u.vmateriais.d_fim_vigencia;
                        material.d_inicio_vigencia = u.vmateriais.d_inicio_vigencia;
                        material.d_validade = u.vmateriais.d_validade;
                        material.f_distribuidor_exclusivo = u.vmateriais.f_distribuidor_exclusivo;
                        material.f_nao_negociado = u.vmateriais.f_nao_negociado;
                        material.f_origem = u.vmateriais.f_origem;
                        material.f_status = u.vmateriais.f_status;
                        material.fornecedor = u.vmateriais.fornecedor;

                        material.sisdataa = DateTime.Today;
                        material.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());
                        if (TryUpdateModel(material))
                        {
                            dg.SaveChanges();
                            TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Material Atualizado com Sucesso!</font>";
                        }
                        else
                        {
                            TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Material</font>";
                        }
                        return RedirectToAction("Materiais");
                    }
                    else
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Alterar o Material</font>";
                        return RedirectToAction("Materiais");
                    }
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario)).FirstOrDefault();
                ViewData["usuario_permissao"] = up;
                // busca fornecedores na tabela
                var materiais = from a in dg.materiais
                                join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                from x in g.DefaultIfEmpty()
                                from y in h.DefaultIfEmpty()
                                select new ListaMaterial
                                {
                                    id = a.id,
                                    c_cod_ref_fabr = a.c_cod_ref_fabr,
                                    c_codigo_tuss = a.c_codigo_tuss,
                                    c_composicao = a.c_composicao,
                                    c_descricao_generica = a.c_descricao_generica,
                                    c_justificativa_alteracao = a.c_justificativa_alteracao,
                                    c_nome_comercial = a.c_nome_comercial,
                                    c_nome_material = a.c_nome_material,
                                    c_registro_anvisa = a.c_registro_anvisa,
                                    c_tnumm = a.c_tnumm,
                                    classificacao = a.classificaco.c_descricao,
                                    d_fim_vigencia = a.d_fim_vigencia,
                                    d_inicio_vigencia = a.d_inicio_vigencia,
                                    d_validade = a.d_validade,
                                    f_distribuidor_exclusivo = a.f_distribuidor_exclusivo == true ? "Sim" : "Não",
                                    f_nao_negociado = a.f_nao_negociado == true ? "Sim" : "Não",
                                    f_origem = a.f_origem == 1 ? "Nacional" : "Importado",
                                    f_status = a.f_status == 1 ? "Disponível" : "Indisponível",
                                    fornecedor = a.fornecedore.c_razao_social,
                                    marca = a.marca1.c_nome,
                                    v_preco = a.v_preco,
                                    unidade = a.unidade1.c_sigla,
                                    sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                    sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                    sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                    sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                };
                ViewData["listamaterial"] = materiais.ToList();

                var classificacao = from a in dg.classificacoes
                                    select new ListaClassificacao
                                    {
                                        id = a.id,
                                        descricao = a.c_descricao
                                    };

                ViewData["listaclassificacao"] = classificacao.ToList();

                var fornecedor = from a in dg.fornecedores
                                 select new ListaFornecedor
                                 {
                                     c_razao_social = a.c_razao_social,
                                     id = a.id
                                 };
                ViewData["listafornecedor"] = fornecedor.ToList();

                var marca = from a in dg.marcas
                            select new ListaMarca
                            {
                                c_nome = a.c_nome,
                                id = a.id
                            };
                ViewData["listamarca"] = marca.ToList();

                var unidade = from a in dg.unidades
                              select new ListaUnidade
                              {
                                  c_sigla = a.c_sigla,
                                  id = a.id
                              };
                ViewData["listaunidade"] = unidade.ToList();

                // lista informcao
                var listainformacao = from a in dg.materiais_informacoes
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.material == u.vmateriais.id
                                      select new ListaMatInformacao
                                      {
                                          id = a.id,
                                          c_informacoes = a.c_informacoes,
                                          material = a.material,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                      };
                ViewData["listainformacao"] = listainformacao.ToList();

                // lista procedimento
                var listaprocedimentos = from a in dg.materiais_procedimentos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == u.vmateriais.id
                                         select new ListaMatProcedimento
                                         {
                                             id = a.id,
                                             procedimento = a.procedimento1.c_descricao,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();

                // lista similares
                var listasimilares = from a in dg.materiais_similares
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.material == u.vmateriais.id
                                     select new ListaMatSimilar
                                     {
                                         id = a.id,
                                         material_similar = a.materiai1.c_nome_material,
                                         material = a.material,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      id = a.id,
                                      c_nome_material = a.c_nome_material

                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();

            }
            ViewBag.Action = "Editar";
            ViewBag.Titulo = "Cadastro de Materiais";
            return View("Materiais", u);
        }
        public ActionResult Delete(int? id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                int id_usuario = int.Parse(Session["usuariologadoid"].ToString());
                var up = dg.usuario_permissao.Where(a => a.id_usuario.Equals(id_usuario) && a.materiais_d.Equals(1)).Count();
                if (up >= 1)
                {
                    try
                    {
                        materiai material = dg.materiais.Find(id);
                        dg.materiais.Remove(material);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                        return RedirectToAction("Materiais");
                    }
                    TempData["mensagem"] = "<font style='color: green;text-align:right;font-size:11px'>Material Excluído com Sucesso!</font>";

                }
                else
                {
                    TempData["mensagem"] = "<font style='color: red;text-align:right;font-size:11px'>Usuário Não Tem Permissão para Excluir o Material</font>";
                }
            }
            ViewBag.Action = "";
            return RedirectToAction("Materiais");
        }

        // métodos informacao    
        public ActionResult IncluirInformacao(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = new materiais_informacoes();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };
                vDetalheMaterial.vmateriais_informacoes.material = id;

                //Lista de Enderecos
                var listainformacao = from a in dg.materiais_informacoes
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.material == id
                                    select new ListaMatInformacao
                                    {
                                        id = a.id,
                                        c_informacoes = a.c_informacoes,
                                        material = a.material,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                    };
                ViewData["listainformacao"] = listainformacao.ToList();

                ViewBag.Action = "InserirInformacao";

                return PartialView("MateriaisInformacao", vDetalheMaterial);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirInformacao(Models.vModelDetalheMateriais u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vmateriais_informacoes.c_informacoes = u.vmateriais_informacoes.c_informacoes.ToUpper();
                    u.vmateriais_informacoes.sisdatai = DateTime.Today;
                    u.vmateriais_informacoes.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());

                    try
                    {
                        dg.materiais_informacoes.Add(u.vmateriais_informacoes);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";



                        var dadosmateriais = dg.materiais.Where(a => a.id.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                        var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                        var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                        var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();

                        var vDetalheMaterial = new vModelDetalheMateriais()
                        {
                            vmateriais = dadosmateriais,
                            vmateriais_informacoes = dadosmateriais_informacoes,
                            vmateriais_procedimentos = dadosmateriais_procedimentos,
                            vmateriais_similares = dadosmateriais_similares
                        };



                        var listainformacao = from a in dg.materiais_informacoes
                                            join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                            join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                            from x in g.DefaultIfEmpty()
                                            from y in h.DefaultIfEmpty()
                                            where a.material == u.vmateriais_informacoes.material
                                            select new ListaMatInformacao
                                            {
                                                id = a.id,
                                                c_informacoes = a.c_informacoes,
                                                material = a.material,
                                                sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                               
                                            };
                        ViewData["listainformacao"] = listainformacao.ToList();

                        return PartialView("MateriaisInformacao", vDetalheMaterial);
                    }

                    TempData["mensageminformacao"] = "<font style='color: green;text-align:right;font-size:11px'>Informação inserida com sucesso!</font>";
                    return RedirectToAction("PreencheCamposInformacao", new { id_info = u.vmateriais_informacoes.id, id = u.vmateriais_informacoes.material });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {



                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                var listainformacao = from a in dg.materiais_informacoes
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.material == u.vmateriais_informacoes.material
                                    select new ListaMatInformacao
                                    {
                                        id = a.id,
                                        c_informacoes = a.c_informacoes,
                                        material = a.material,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                    };
                ViewData["listainformacao"] = listainformacao.ToList();

                return PartialView("MateriaisInformacao", vDetalheMaterial);
            }
        }
        public ActionResult PreencheCamposInformacao(int id_info, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id) && a.id.Equals(id_info)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                //carrega lista de operadoras

                var listainformacao = from a in dg.materiais_informacoes
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.material == id
                                    select new ListaMatInformacao
                                    {
                                        id = a.id,
                                        c_informacoes = a.c_informacoes,
                                        material = a.material,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                    };
                ViewData["listainformacao"] = listainformacao.ToList();

                               
                if (TempData["mensageminformacao"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensageminformacao"];
                    TempData["mensageminformacao"] = string.Empty;
                }


                //Altera status para editar
                ViewBag.Action = "EditarInformacao";
                return View("MateriaisInformacao", vDetalheMaterial);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarInformacao(Models.vModelDetalheMateriais u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    u.vmateriais_informacoes.c_informacoes = u.vmateriais_informacoes.c_informacoes.ToUpper();

                    materiais_informacoes MatInformacao = dg.materiais_informacoes.Find(u.vmateriais_informacoes.id);

                    MatInformacao.c_informacoes = u.vmateriais_informacoes.c_informacoes;

                    var id = MatInformacao.material;

                    if (TryUpdateModel(MatInformacao))
                    {
                        dg.SaveChanges();
                        TempData["mensageminformacao"] = "<font style='color: green;text-align:right;font-size:11px'>Informação Atualizada com Sucesso!</font>";
                    }
                    else
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Informação!</font>";

                        var listainformacao = from a in dg.materiais_informacoes
                                            join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                            join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                            from x in g.DefaultIfEmpty()
                                            from y in h.DefaultIfEmpty()
                                            where a.material == id
                                            select new ListaMatInformacao
                                            {
                                                id = a.id,
                                                c_informacoes = a.c_informacoes,
                                                material = a.material,
                                                sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                            };
                        ViewData["listainformacao"] = listainformacao.ToList();

                                              
                        return PartialView("MateriaisInformacao", u);
                    }
                    return RedirectToAction("PreencheCamposInformacao", new { id_info = u.vmateriais_informacoes.id, id = u.vmateriais_informacoes.material });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listainformacao = from a in dg.materiais_informacoes
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.material == u.vmateriais_informacoes.material
                                    select new ListaMatInformacao
                                    {
                                        id = a.id,
                                       c_informacoes = a.c_informacoes,
                                       material =a.material,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                    };
                ViewData["listainformacao"] = listainformacao.ToList();

              
            }
            return PartialView("MateriaisInformacao", u);
        }
        public ActionResult DeleteInformacao(int id_info, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                try
                {
                    materiais_informacoes MatInformacao = dg.materiais_informacoes.Find(id_info);
                    dg.materiais_informacoes.Remove(MatInformacao);
                    dg.SaveChanges();
                }
                catch (SystemException e)
                {
                    ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                    var listainformacao = from a in dg.materiais_informacoes
                                        join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                        join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                        from x in g.DefaultIfEmpty()
                                        from y in h.DefaultIfEmpty()
                                        where a.material == id
                                        select new ListaMatInformacao
                                        {
                                            id = a.id,
                                            c_informacoes = a.c_informacoes,
                                            material = a.material,
                                            sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                            sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                            sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                            sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                        };
                    ViewData["listainformacao"] = listainformacao.ToList();



                    var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                    var dadosmateriais_informacoes = new materiais_informacoes();
                    var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                    var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                    var vDetalheMaterial = new vModelDetalheMateriais()
                    {
                        vmateriais = dadosmateriais,
                        vmateriais_informacoes = dadosmateriais_informacoes,
                        vmateriais_procedimentos = dadosmateriais_procedimentos,
                        vmateriais_similares = dadosmateriais_similares
                    };
                    vDetalheMaterial.vmateriais_informacoes.material = id;

                    ViewBag.Action = "EditarInformacao";

                    return PartialView("MateriaisInformacao", vDetalheMaterial);
                }

            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listainformacao = from a in dg.materiais_informacoes
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.material == id
                                    select new ListaMatInformacao
                                    {
                                        id = a.id,
                                        c_informacoes = a.c_informacoes,
                                        material = a.material,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                    };
                ViewData["listainformacao"] = listainformacao.ToList();



                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = new materiais_informacoes();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };
                vDetalheMaterial.vmateriais_informacoes.material = id;

                ViewBag.Action = string.Empty;
                ViewBag.Message = "<font style='color: green;text-align:right;font-size:11px'>Informação Excluída com sucesso!</font>";

                return PartialView("MateriaisInformacao", vDetalheMaterial);
            }

        }
        public ActionResult Informacao(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listainformacao = from a in dg.materiais_informacoes
                                    join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                    join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                    from x in g.DefaultIfEmpty()
                                    from y in h.DefaultIfEmpty()
                                    where a.material == id
                                    select new ListaMatInformacao
                                    {
                                        id = a.id,
                                        c_informacoes = a.c_informacoes,
                                        material = a.material,
                                        sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                        sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                        sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                        sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                    };
                ViewData["listainformacao"] = listainformacao.ToList();



                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = new materiais_informacoes();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                ViewBag.Action = string.Empty;
                ViewBag.Message = string.Empty;

                return PartialView("MateriaisInformacao", vDetalheMaterial);
            }
        }
        // métodos Procedimentos    
        public ActionResult IncluirProcedimento(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes =dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_procedimentos = new materiais_procedimentos();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };
                vDetalheMaterial.vmateriais_procedimentos.material = id;

                //Lista de Enderecos
                var listaprocedimentos = from a in dg.materiais_procedimentos
                                      join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                      join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                      from x in g.DefaultIfEmpty()
                                      from y in h.DefaultIfEmpty()
                                      where a.material == id
                                      select new ListaMatProcedimento
                                      {
                                          id = a.id,
                                          procedimento = a.procedimento1.c_descricao,
                                          material = a.material,
                                          sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                          sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                          sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                          sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                      };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();

                ViewBag.Action = "InserirProcedimento";

                return PartialView("MateriaisProcedimentos", vDetalheMaterial);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirProcedimento(Models.vModelDetalheMateriais u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                    
                    u.vmateriais_procedimentos.sisdatai = DateTime.Today;
                    u.vmateriais_procedimentos.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());

                    try
                    {
                        dg.materiais_procedimentos.Add(u.vmateriais_procedimentos);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";



                        var dadosmateriais = dg.materiais.Where(a => a.id.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                        var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                        var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                        var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();

                        var vDetalheMaterial = new vModelDetalheMateriais()
                        {
                            vmateriais = dadosmateriais,
                            vmateriais_informacoes = dadosmateriais_informacoes,
                            vmateriais_procedimentos = dadosmateriais_procedimentos,
                            vmateriais_similares = dadosmateriais_similares
                        };



                        var listaprocedimentos = from a in dg.materiais_procedimentos
                                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                 from x in g.DefaultIfEmpty()
                                                 from y in h.DefaultIfEmpty()
                                                 where a.material == u.vmateriais_procedimentos.material
                                                 select new ListaMatProcedimento
                                                 {
                                                     id = a.id,
                                                     procedimento = a.procedimento1.c_descricao,
                                                     material = a.material,
                                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                                 };
                        ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                        var lstprocedimento = from a in dg.procedimentos
                                              select new ListaProcedimentos
                                              {
                                                  id = a.id,
                                                  c_codigo = a.c_codigo,
                                                  c_descricao = a.c_descricao
                                              };
                        ViewData["lstprocedimento"] = lstprocedimento.ToList();

                        return PartialView("MateriaisProcedimentos", vDetalheMaterial);
                    }

                    TempData["mensagemprocedimento"] = "<font style='color: green;text-align:right;font-size:11px'>Procedimento inserido com sucesso!</font>";
                    return RedirectToAction("PreencheCamposProcedimento", new { id_proc = u.vmateriais_procedimentos.id, id = u.vmateriais_procedimentos.material });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {



                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                var listaprocedimentos = from a in dg.materiais_procedimentos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == u.vmateriais_procedimentos.material
                                         select new ListaMatProcedimento
                                         {
                                             id = a.id,
                                             procedimento = a.procedimento1.c_descricao,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();

                return PartialView("MateriaisProcedimentos", vDetalheMaterial);
            }
        }
        public ActionResult PreencheCamposProcedimento(int id_proc, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id) && a.id.Equals(id_proc)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                //carrega lista de operadoras

                var listaprocedimentos = from a in dg.materiais_procedimentos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == id
                                         select new ListaMatProcedimento
                                         {
                                             id = a.id,
                                             procedimento = a.procedimento1.c_descricao,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();


                if (TempData["mensagemprocedimento"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagemprocedimento"];
                    TempData["mensagemprocedimento"] = string.Empty;
                }


                //Altera status para editar
                ViewBag.Action = "EditarProcedimento";
                return View("MateriaisProcedimentos", vDetalheMaterial);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProcedimento(Models.vModelDetalheMateriais u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {
                   
                    materiais_procedimentos MatProcedimento = dg.materiais_procedimentos.Find(u.vmateriais_procedimentos.id);

                    MatProcedimento.procedimento = u.vmateriais_procedimentos.procedimento;
                    MatProcedimento.sisdataa = DateTime.Today;
                    MatProcedimento.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());

                    var id = MatProcedimento.material;

                    if (TryUpdateModel(MatProcedimento))
                    {
                        dg.SaveChanges();
                        TempData["mensagemprocedimento"] = "<font style='color: green;text-align:right;font-size:11px'>Procedimento Atualizado com Sucesso!</font>";
                    }
                    else
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Procedimento!</font>";

                        var listaprocedimentos = from a in dg.materiais_procedimentos
                                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                 from x in g.DefaultIfEmpty()
                                                 from y in h.DefaultIfEmpty()
                                                 where a.material == id
                                                 select new ListaMatProcedimento
                                                 {
                                                     id = a.id,
                                                     procedimento = a.procedimento1.c_descricao,
                                                     material = a.material,
                                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                                 };
                        ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                        var lstprocedimento = from a in dg.procedimentos
                                              select new ListaProcedimentos
                                              {
                                                  id = a.id,
                                                  c_codigo = a.c_codigo,
                                                  c_descricao = a.c_descricao
                                              };
                        ViewData["lstprocedimento"] = lstprocedimento.ToList();


                        return PartialView("MateriaisProcedimentos", u);
                    }
                    return RedirectToAction("PreencheCamposProcedimentos", new { id_info = u.vmateriais_procedimentos.id, id = u.vmateriais_procedimentos.material });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listaprocedimentos = from a in dg.materiais_procedimentos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == u.vmateriais_procedimentos.material
                                         select new ListaMatProcedimento
                                         {
                                             id = a.id,
                                             procedimento = a.procedimento1.c_descricao,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();


            }
            return PartialView("MateriaisProcedimentos", u);
        }
        public ActionResult DeleteProcedimento(int id_proc, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                try
                {
                    materiais_procedimentos MatProcedimento = dg.materiais_procedimentos.Find(id_proc);
                    dg.materiais_procedimentos.Remove(MatProcedimento);
                    dg.SaveChanges();
                }
                catch (SystemException e)
                {
                    ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                    var listaprocedimentos = from a in dg.materiais_procedimentos
                                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                             from x in g.DefaultIfEmpty()
                                             from y in h.DefaultIfEmpty()
                                             where a.material == id
                                             select new ListaMatProcedimento
                                             {
                                                 id = a.id,
                                                 procedimento = a.procedimento1.c_descricao,
                                                 material = a.material,
                                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                             };
                    ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                    var lstprocedimento = from a in dg.procedimentos
                                          select new ListaProcedimentos
                                          {
                                              id = a.id,
                                              c_codigo = a.c_codigo,
                                              c_descricao = a.c_descricao
                                          };
                    ViewData["lstprocedimento"] = lstprocedimento.ToList();



                    var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                    var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                    var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                    var dadosmateriais_procedimentos = new materiais_procedimentos();

                    var vDetalheMaterial = new vModelDetalheMateriais()
                    {
                        vmateriais = dadosmateriais,
                        vmateriais_informacoes = dadosmateriais_informacoes,
                        vmateriais_procedimentos = dadosmateriais_procedimentos,
                        vmateriais_similares = dadosmateriais_similares
                    };
                    vDetalheMaterial.vmateriais_procedimentos.material = id;

                    ViewBag.Action = "EditarProcedimento";

                    return PartialView("MateriaisProcedimentos", vDetalheMaterial);
                }

            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listaprocedimentos = from a in dg.materiais_procedimentos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == id
                                         select new ListaMatProcedimento
                                         {
                                             id = a.id,
                                             procedimento = a.procedimento1.c_descricao,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();



                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_procedimentos = new materiais_procedimentos();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };
                vDetalheMaterial.vmateriais_procedimentos.material = id;

                ViewBag.Action = string.Empty;
                ViewBag.Message = "<font style='color: green;text-align:right;font-size:11px'>Procedimento Excluído com sucesso!</font>";

                return PartialView("MateriaisProcedimentos", vDetalheMaterial);
            }

        }
        public ActionResult Procedimento(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listaprocedimentos = from a in dg.materiais_procedimentos
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == id
                                         select new ListaMatProcedimento
                                         {
                                             id = a.id,
                                             procedimento = a.procedimento1.c_descricao,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listaprocedimentos"] = listaprocedimentos.ToList();

                var lstprocedimento = from a in dg.procedimentos
                                      select new ListaProcedimentos
                                      {
                                          id = a.id,
                                          c_codigo = a.c_codigo,
                                          c_descricao = a.c_descricao
                                      };
                ViewData["lstprocedimento"] = lstprocedimento.ToList();



                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_procedimentos = new materiais_procedimentos();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                ViewBag.Action = string.Empty;
                ViewBag.Message = string.Empty;

                return PartialView("MateriaisProcedimentos", vDetalheMaterial);
            }
        }
        // métodos similares
        public ActionResult IncluirSimilar(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {


                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_similares = new materiais_similares();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };
                vDetalheMaterial.vmateriais_similares.material = id;

                //Lista de Enderecos
                var listasimilares = from a in dg.materiais_similares
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == id
                                         select new ListaMatSimilar
                                         {
                                             id = a.id,
                                             material_similar = a.materiai1.c_nome_material,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                      select new ListaMaterial
                                      {
                                          id = a.id,
                                          c_nome_material = a.c_nome_material
                                         
                                      };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                ViewBag.Action = "InserirSimilar";

                return PartialView("MateriaisSimilares", vDetalheMaterial);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InserirSimilar(Models.vModelDetalheMateriais u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {

                    u.vmateriais_similares.sisdatai = DateTime.Today;
                    u.vmateriais_similares.sisusuarioi = int.Parse(Session["usuariologadoid"].ToString());

                    try
                    {
                        dg.materiais_similares.Add(u.vmateriais_similares);
                        dg.SaveChanges();
                    }
                    catch (SystemException e)
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";



                        var dadosmateriais = dg.materiais.Where(a => a.id.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                        var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                        var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                        var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();

                        var vDetalheMaterial = new vModelDetalheMateriais()
                        {
                            vmateriais = dadosmateriais,
                            vmateriais_informacoes = dadosmateriais_informacoes,
                            vmateriais_procedimentos = dadosmateriais_procedimentos,
                            vmateriais_similares = dadosmateriais_similares
                        };



                        var listasimilares = from a in dg.materiais_similares
                                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                 from x in g.DefaultIfEmpty()
                                                 from y in h.DefaultIfEmpty()
                                                 where a.material == u.vmateriais_procedimentos.material
                                                 select new ListaMatSimilar
                                                 {
                                                     id = a.id,
                                                     material_similar = a.materiai1.c_nome_material,
                                                     material = a.material,
                                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                                 };
                        ViewData["listasimilares"] = listasimilares.ToList();

                        var lstmaterial = from a in dg.materiais
                                              select new ListaMaterial
                                              {
                                                  id = a.id,
                                                  c_nome_material = a.c_nome_material
                                                  
                                              };
                        ViewData["lstmaterial"] = lstmaterial.ToList();

                        return PartialView("MateriaisSimilares", vDetalheMaterial);
                    }

                    TempData["mensagemsimilar"] = "<font style='color: green;text-align:right;font-size:11px'>Material Similar inserido com sucesso!</font>";
                    return RedirectToAction("PreencheCamposSimilar", new { id_similar = u.vmateriais_similares.id, id = u.vmateriais_similares.material });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {



                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(u.vmateriais_informacoes.material)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                var listasimilares = from a in dg.materiais_similares
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == u.vmateriais_procedimentos.material
                                         select new ListaMatSimilar
                                         {
                                             id = a.id,
                                             material_similar = a.materiai1.c_nome_material,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                      select new ListaMaterial
                                      {
                                          id = a.id,
                                          c_nome_material = a.c_nome_material
                                          
                                      };
                ViewData["lstmaterial"] = lstmaterial.ToList();

                return PartialView("MateriaisSimilares", vDetalheMaterial);
            }
        }
        public ActionResult PreencheCamposSimilar(int id_similar, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_similares = dg.materiais_similares.Where(a => a.material.Equals(id) && a.id.Equals(id_similar)).FirstOrDefault();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                //carrega lista de operadoras

                var listasimilares = from a in dg.materiais_similares
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == id
                                         select new ListaMatSimilar
                                         {
                                             id = a.id,
                                             material_similar = a.materiai1.c_nome_material,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                      select new ListaMaterial
                                      {
                                          id = a.id,
                                          c_nome_material = a.c_nome_material
                                         
                                      };
                ViewData["lstmaterial"] = lstmaterial.ToList();


                if (TempData["mensagemsimilar"] != string.Empty)
                {
                    ViewBag.Message = TempData["mensagemsimilar"];
                    TempData["mensagemsimilar"] = string.Empty;
                }


                //Altera status para editar
                ViewBag.Action = "EditarSimilar";
                return View("MateriaisSimilares", vDetalheMaterial);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarSimilar(Models.vModelDetalheMateriais u)
        {
            if (ModelState.IsValid)
            {
                using (UnimedEntities1 dg = new UnimedEntities1())
                {

                    materiais_similares MatSimilar = dg.materiais_similares.Find(u.vmateriais_similares.id);

                    MatSimilar.material_similar = u.vmateriais_similares.material_similar;
                    MatSimilar.sisdataa = DateTime.Today;
                    MatSimilar.sisusuarioa = int.Parse(Session["usuariologadoid"].ToString());

                    var id = MatSimilar.material;

                    if (TryUpdateModel(MatSimilar))
                    {
                        dg.SaveChanges();
                        TempData["mensagemsimilar"] = "<font style='color: green;text-align:right;font-size:11px'>Material Similar Atualizado com Sucesso!</font>";
                    }
                    else
                    {
                        ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>Erro ao Atualizar Material Similar!</font>";

                        var listasimilares = from a in dg.materiais_similares
                                                 join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                                 join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                                 from x in g.DefaultIfEmpty()
                                                 from y in h.DefaultIfEmpty()
                                                 where a.material == id
                                                 select new ListaMatSimilar
                                                 {
                                                     id = a.id,
                                                     material_similar = a.materiai1.c_nome_material,
                                                     material = a.material,
                                                     sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                     sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                     sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                     sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                                 };
                        ViewData["listasimilares"] = listasimilares.ToList();

                        var lstmaterial = from a in dg.materiais
                                              select new ListaMaterial
                                              {
                                                  id = a.id,
                                                  c_nome_material = a.c_nome_material
                                                  
                                              };
                        ViewData["lstmaterial"] = lstmaterial.ToList();


                        return PartialView("MateriaisSimilares", u);
                    }
                    return RedirectToAction("PreencherCamposSimilar", new { id_similar = u.vmateriais_similares.id, id = u.vmateriais_similares.material });
                }
            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listasimilares = from a in dg.materiais_similares
                                         join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                         join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                         from x in g.DefaultIfEmpty()
                                         from y in h.DefaultIfEmpty()
                                         where a.material == u.vmateriais_similares.material
                                         select new ListaMatSimilar
                                         {
                                             id = a.id,
                                             material_similar = a.materiai1.c_nome_material,
                                             material = a.material,
                                             sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                             sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                             sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                             sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                         };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                      select new ListaMaterial
                                      {
                                          id = a.id,
                                          c_nome_material = a.c_nome_material
                                      };
                ViewData["lstmaterial"] = lstmaterial.ToList();


            }
            return PartialView("MateriaisSimilares", u);
        }
        public ActionResult DeleteSimilar(int id_similar, int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                try
                {
                    materiais_similares MatSimilar = dg.materiais_similares.Find(id_similar);
                    dg.materiais_similares.Remove(MatSimilar);
                    dg.SaveChanges();
                }
                catch (SystemException e)
                {
                    ViewBag.Message = "<font style='color: red;text-align:right;font-size:11px'>" + e.Message + "</font>";
                    var listasimilares = from a in dg.materiais_similares
                                             join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                             join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                             from x in g.DefaultIfEmpty()
                                             from y in h.DefaultIfEmpty()
                                             where a.material == id
                                             select new ListaMatSimilar
                                             {
                                                 id = a.id,
                                                 material_similar = a.materiai1.c_nome_material,
                                                 material = a.material,
                                                 sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                                 sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                                 sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                                 sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                             };
                    ViewData["listasimilares"] = listasimilares.ToList();

                    var lstmaterial = from a in dg.materiais
                                          select new ListaMaterial
                                          {
                                              id = a.id,
                                              c_nome_material = a.c_nome_material
                                              
                                          };
                    ViewData["lstmaterial"] = lstmaterial.ToList();



                    var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                    var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                    var dadosmateriais_similares = new materiais_similares();
                    var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                    var vDetalheMaterial = new vModelDetalheMateriais()
                    {
                        vmateriais = dadosmateriais,
                        vmateriais_informacoes = dadosmateriais_informacoes,
                        vmateriais_procedimentos = dadosmateriais_procedimentos,
                        vmateriais_similares = dadosmateriais_similares
                    };
                    vDetalheMaterial.vmateriais_similares.material = id;

                    ViewBag.Action = "";

                    return PartialView("MateriaisSimilares", vDetalheMaterial);
                }

            }
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listasimilares = from a in dg.materiais_similares
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.material == id
                                     select new ListaMatSimilar
                                     {
                                         id = a.id,
                                         material_similar = a.materiai1.c_nome_material,
                                         material = a.material,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      id = a.id,
                                      c_nome_material = a.c_nome_material

                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();



                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_similares = new materiais_similares();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };
                vDetalheMaterial.vmateriais_similares.material = id;

                ViewBag.Action = string.Empty;
                ViewBag.Message = "<font style='color: green;text-align:right;font-size:11px'>Material Similar Excluído com sucesso!</font>";

                return PartialView("MateriaisSimilares", vDetalheMaterial);
            }

        }
        public ActionResult Similar(int id)
        {
            using (UnimedEntities1 dg = new UnimedEntities1())
            {
                var listasimilares = from a in dg.materiais_similares
                                     join b in dg.usuarios on a.sisusuarioi equals b.id into g
                                     join c in dg.usuarios on a.sisusuarioa equals c.id into h
                                     from x in g.DefaultIfEmpty()
                                     from y in h.DefaultIfEmpty()
                                     where a.material == id
                                     select new ListaMatSimilar
                                     {
                                         id = a.id,
                                         material_similar = a.materiai1.c_nome_material,
                                         material = a.material,
                                         sisdatai = a.sisdatai == null ? DateTime.Today : a.sisdatai,
                                         sisusuarioi = (x == null ? "Sem Dados" : x.nome_usuario),
                                         sisdataa = a.sisdataa == null ? DateTime.Today : a.sisdataa,
                                         sisusuarioa = (y == null ? "Sem Dados" : y.nome_usuario)
                                     };
                ViewData["listasimilares"] = listasimilares.ToList();

                var lstmaterial = from a in dg.materiais
                                  select new ListaMaterial
                                  {
                                      id = a.id,
                                      c_nome_material = a.c_nome_material

                                  };
                ViewData["lstmaterial"] = lstmaterial.ToList();



                var dadosmateriais = dg.materiais.Where(a => a.id.Equals(id)).FirstOrDefault();
                var dadosmateriais_informacoes = dg.materiais_informacoes.Where(a => a.material.Equals(id)).FirstOrDefault();
                var dadosmateriais_similares = new materiais_similares();
                var dadosmateriais_procedimentos = dg.materiais_procedimentos.Where(a => a.material.Equals(id)).FirstOrDefault();

                var vDetalheMaterial = new vModelDetalheMateriais()
                {
                    vmateriais = dadosmateriais,
                    vmateriais_informacoes = dadosmateriais_informacoes,
                    vmateriais_procedimentos = dadosmateriais_procedimentos,
                    vmateriais_similares = dadosmateriais_similares
                };

                ViewBag.Action = string.Empty;
                ViewBag.Message = string.Empty;

                return PartialView("MateriaisSimilares", vDetalheMaterial);
            }
        }
    }
}
