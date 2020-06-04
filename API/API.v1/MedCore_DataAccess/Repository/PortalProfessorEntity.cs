using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using System.Linq;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Contracts.Enums;
using System;

namespace MedCore_DataAccess.Repository
{
    public class PortalProfessorEntity
    {
        private readonly int[] ID_CLASSIFICACAO_APOSTILAS = { (int)ClassificacaoApostilaEnum.MedCurso,
            (int)ClassificacaoApostilaEnum.Med,
            (int)ClassificacaoApostilaEnum.MedEletro,
            (int)ClassificacaoApostilaEnum.R3Clinica,
            (int)ClassificacaoApostilaEnum.R3Cirurgia,
            (int)ClassificacaoApostilaEnum.R3Pediatria,
            (int)ClassificacaoApostilaEnum.R4GO,
            (int)ClassificacaoApostilaEnum.TEGO,
            (int)ClassificacaoApostilaEnum.MASTO};

        private readonly int[] ID_TIPO_CLASSIFICACAO_ESPECIALIDADES = { 2, 3, 9 };
        //Para essa etapa, retornamos todas as questões, exceto as que já foram classificadas em grande áreas e as marcadas como pendente
        public int[] TiposNaoUsados { get; set; }

        public PortalProfessorEntity()
        {
            TiposNaoUsados = new int[3]{(int)PPQuestao.TipoClassificacao.Pendente,
				                           (int)PPQuestao.TipoClassificacao.GrandeArea,
				                           (int)PPQuestao.TipoClassificacao.EspecialidadeClinica};
        }

        public List<PPQuestao> GetQuestoesPorEtapaPortal(PPQuestao pp, int maxRegistros = 1000, int pagina = 1, bool somenteID = false, bool somenteQtdMaxima = false)
        {
            using (var ctx = new DesenvContext())
            {
                //ctx.SetCommandTimeOut(90);

                ExcluirQuestoesEmClassificacao();

                var favoritos = (from f in ctx.tblQuestao_Favoritas
                                 join pes in ctx.tblPersons on f.intProfessorID equals pes.intContactID
                                 where f.bitAtivo
                                 select new
                                        {
                                            IdQuestao = f.intQuestaoID,
                                            Professor = pes.txtName.Trim()
                                        });

                var questoes = ctx.tblConcursoQuestoes.AsQueryable();

                if (pp.FiltroIDQuestao > 0)
                {
                    var questao = ObterQuestaoConcursoCompleta(pp.FiltroIDQuestao, pp.FiltroIntEmployeeID);

                    questao.FavoritadaPor = favoritos.Where(f => f.IdQuestao == pp.FiltroIDQuestao).Select(f => f.Professor).FirstOrDefault();
                    questao.ProtocoladaPara = ProtocoloQuestao(ctx, pp.FiltroIDQuestao);

                    return new List<PPQuestao>() { questao };
                }
                else if (pp.FiltroEtapa != PPQuestao.EtapaPortal.IgnorarFiltros)
                {
                    questoes = FiltrarQuestoes(pp, questoes, somenteQtdMaxima, ctx);
                }

                if (somenteQtdMaxima)
                {
                    return questoes.Select(q => new PPQuestao() { Id = q.intQuestaoID }).Distinct().ToList();
                }

                if (somenteID)
                {
                    
                    List<string> queryConcursosPremium = (from q in ctx.tblConcursoPremium
                                                         select                                                 
                                                            q.txtSigla                                                            
                                                        ).ToList();


                    var queryQuestoes = (from q in questoes
                                         join p in ctx.tblConcurso_Provas on q.intProvaID equals p.intProvaID
                                         join c in ctx.tblConcurso on p.ID_CONCURSO equals c.ID_CONCURSO
                                         join favorita1 in favoritos on q.intQuestaoID equals favorita1.IdQuestao into favorita2
                                         from favorita in favorita2.DefaultIfEmpty()
                                         select new
                                         {
                                             Id = q.intQuestaoID,
                                             Ano = q.intYear.Value,
                                             Ordem = q.intOrder.Value,
                                             Tipo = q.bitDiscursiva == true ? 2 : 1,
                                             ProvaID = p.intProvaID,
                                             AnoConcurso = c.VL_ANO_CONCURSO.Value,
                                             Sigla = c.SG_CONCURSO.Trim(),
                                             UF = c.CD_UF.Trim(),
                                             FavoritadaPor = favorita.Professor.Trim(),
                                             Premium = queryConcursosPremium.Contains(c.SG_CONCURSO.Trim()),
                                             OrdemPremium = 0,
                                             Anulada = q.bitAnulada == true ? q.bitAnulada : (q.bitAnuladaPosRecurso == true ? true: false)
                                         }).Distinct()
                                         .OrderByDescending(x => x.AnoConcurso).ThenBy(x => x.Sigla).ThenBy(x => x.Ordem)
                                         .Take(4000)
                                         .ToList();

                    var questoesRetorno = new List<PPQuestao>();

                    queryQuestoes.ForEach(x =>
                    {
                        var item = new PPQuestao()
                        {
                            Id = x.Id,
                            Ano = x.Ano,
                            Ordem = x.Ordem,
                            Tipo = x.Tipo,
                            Anulada = x.Anulada,
                            Prova = new Prova()
                            {
                                ID = x.ProvaID
                            },
                            Concurso = new Concurso()
                            {
                                Ano = x.AnoConcurso,
                                Sigla = x.Sigla,
                                UF = x.UF
                            },
                            FavoritadaPor = x.FavoritadaPor,
                            Premium = x.Premium,
                            OrdemPremium = getOrdemPremium(x.UF)
                        };

                        questoesRetorno.Add(item);
                    });

                    var questaoIds = questoesRetorno.Select(q => q.Id).ToList();

                    GetProfessoresEmClassificacao(pp, ctx, questoesRetorno, questaoIds);

                    GetProfessoresProtocolados(ctx, questoesRetorno, questaoIds);

                    return OrdenarQuestoes(questoesRetorno)
                        .Skip((pagina - 1) * maxRegistros)
                        .Take(maxRegistros)
                        .ToList();
                }

                ExcluirQuestoesEmClassificacao(pp.FiltroIntEmployeeID);

                var pessoasClassificacao = (from classificacao in ctx.tblConcursoQuestao_Classificacao
                                            join pessoa in ctx.tblPersons on classificacao.intEmployeeID equals pessoa.intContactID
                                            select new
                                                   {
                                                       QuestaoID = classificacao.intQuestaoID,
                                                       Nome = pessoa.txtName.Trim(),
                                                       TipoClassificacaoID = classificacao.intTipoDeClassificacao,
                                                       ProfessorID = pessoa.intContactID
                                                   });
                //TODO: REFATORAR
                var pessoasComentario = (from log in ctx.tblLogConcursoQuestaoComentario
                                         join pessoa in ctx.tblPersons on log.intEmployeeAlterou equals pessoa.intContactID
                                         where
                                         !(from roles in ctx.tblCtrlPanel_AccessControl_Persons_X_Roles
                                           where roles.intRoleId == 14
                                           select roles).Select(r => r.intContactId).Contains(pessoa.intContactID)
                                         &&
                                         pessoa.intContactID != 96409
                                         select new
                                                {
                                                    QuestaoID = log.intQuestaoID.Value,
                                                    Nome = pessoa.txtName.Trim(),
                                                    DataAlteracao = log.dteDateAlteracao,
                                                    ProfessorID = pessoa.intContactID
                                                });

                var consulta = (from questao in questoes
                                join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                                join concurso in ctx.tblConcurso on prova.ID_CONCURSO equals concurso.ID_CONCURSO
                                join tipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals tipo.intProvaTipoID
                                join favorita1 in favoritos on questao.intQuestaoID equals favorita1.IdQuestao into favorita2
                                from favorita in favorita2.DefaultIfEmpty()
                                orderby questao.intQuestaoID
                                select new PPQuestao
                                       {
                                           Id = questao.intQuestaoID,
                                           Ordem = questao.intOrder ?? 0,
                                           Ano = questao.intYear ?? 0,
                                           FavoritadaPor = favorita.Professor,
                                           Concurso = new Concurso
                                                      {
                                                          Id = concurso.ID_CONCURSO,
                                                          Sigla = concurso.SG_CONCURSO.Trim(),
                                                          Nome = concurso.NM_CONCURSO.Trim(),
                                                          BitDiscursiva = tipo.bitDiscursiva ?? false
                                                      },
                                           Prova = new Prova()
                                                   {
                                                       ID = prova.intProvaID,
                                                       Nome = prova.txtName,
                                                       Descricao = prova.txtDescription
                                                   },
                                           SemGabaritoOficial = questao.bitSemGabarito,
                                           ExercicioTipo = tipo.txtDescription.ToUpper().Contains("R3") ? "R3" : "R1",
                                           Enunciado = questao.txtEnunciado,
                                           PPImagens = ctx.tblQuestaoConcurso_Imagem
                                               .Where(i => i.intQuestaoID == questao.intQuestaoID)
                                               .Select(imagem => new PPQuestaoImagem
                                                                 {
                                                                     Id = imagem.intID,
                                                                     Url = Constants.URLIMAGEMQUESTAO.Replace("IDQUESTAOIMAGEM", imagem.intID.ToString().Trim()),
                                                                     Nome = imagem.txtName,
                                                                     Label = imagem.txtLabel,
                                                                     QuestaoId = imagem.intQuestaoID
                                                                 }),
                                           Comentario = questao.txtComentario,
                                           Rascunho = ctx.tblComentario_Rascunho.Where(x => x.intQuestaoID == questao.intQuestaoID && x.intEmployeeID == pp.FiltroIntEmployeeID).Select(r => new Rascunho()
                                                                                                                                                                                             {
                                                                                                                                                                                                 EmployeeId = r.intEmployeeID,
                                                                                                                                                                                                 QuestaoId = r.intQuestaoID,
                                                                                                                                                                                                 TextoRascunho = r.txtRascunho
                                                                                                                                                                                             }).FirstOrDefault(),
                                           PPImagensComentario = ctx.tblQuestoesConcursoImagem_Comentario
                                               .Where(i => i.intQuestaoID == questao.intQuestaoID)
                                               .Select(imagemComentario => new PPQuestaoImagem
                                                                           {
                                                                               Id = imagemComentario.intImagemComentarioID,
                                                                               Url = Constants.URLCOMENTARIOIMAGEMCONCURSO.Replace("IDCOMENTARIOIMAGEM", imagemComentario.intImagemComentarioID.ToString().Trim()),
                                                                               Nome = imagemComentario.txtName,
                                                                               Label = imagemComentario.txtLabel,
                                                                               QuestaoId = imagemComentario.intQuestaoID
                                                                           }),
                                           Observacao = questao.txtObservacao,
                                           Especialidades = (from classificacao in questao.tblConcursoQuestao_Classificacao
                                                             join area in ctx.tblConcursoQuestaoCatologoDeClassificacoes on classificacao.intClassificacaoID equals area.intClassificacaoID
                                                             where ID_TIPO_CLASSIFICACAO_ESPECIALIDADES.Contains(classificacao.intTipoDeClassificacao)
                                                             select new Especialidade
                                                                    {
                                                                        Id = classificacao.intClassificacaoID,
                                                                        Descricao = area.intParent != null ? string.Concat(area.txtTipoDeClassificacao, " - ", area.txtSubTipoDeClassificacao) : area.txtSubTipoDeClassificacao,
                                                                        DataClassificacao = classificacao.dteDate,
                                                                        Editavel = !(from apostila in questao.tblConcursoQuestao_Classificacao
                                                                                     join produto in ctx.tblProducts on apostila.intClassificacaoID equals produto.intProductID
                                                                                     join validacao in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on produto.intProductGroup3 equals validacao.intProductGroupID
                                                                                     where validacao.intClassificacaoID == area.intClassificacaoID
                                                                                     select 1).Any()
                                                                    }),
                                           Alternativas = (from alternativa in questao.tblConcursoQuestoes_Alternativas
                                                           select new Alternativa
                                                                  {
                                                                      Id = alternativa.intAlternativaID,
                                                                      Correta = alternativa.bitCorreta ?? false,
                                                                      CorretaPreliminar = alternativa.bitCorretaPreliminar ?? false,
                                                                      Nome = alternativa.txtAlternativa,
                                                                      LetraStr = alternativa.txtLetraAlternativa,
                                                                      Gabarito = alternativa.txtResposta,
                                                                      Imagem = alternativa.txtImagem,
                                                                      ImagemOtimizada = alternativa.txtImagemOtimizada
                                                                  }),
                                           Apostilas = (from classificacao in questao.tblConcursoQuestao_Classificacao
                                                        join produto in ctx.tblProducts on classificacao.intClassificacaoID equals produto.intProductID
                                                        join apostila in ctx.tblBooks on produto.intProductID equals apostila.intBookID
                                                        join validacao in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on produto.intProductGroup3 equals validacao.intProductGroupID
                                                        where ID_CLASSIFICACAO_APOSTILAS.Contains(classificacao.intTipoDeClassificacao)
                                                        select new Apostila
                                                               {
                                                                   ID = classificacao.intClassificacaoID,
                                                                   Ano = (apostila.intYear ?? 0),
                                                                   Codigo = produto.txtCode,
                                                                   NomeCompleto = string.Concat(produto.txtCode, " - ", ((produto.txtName ?? "").Trim().Replace(apostila.intYear.ToString(), ""))),
                                                                   IdGrandeArea = validacao.intClassificacaoID,
                                                                   IdProduto = classificacao.intTipoDeClassificacao == (int)Utilidades.CatalogoClassificacao.MEDCURSO ? (int)Produto.Cursos.MEDCURSO :
                                                                       (classificacao.intTipoDeClassificacao == (int)Utilidades.CatalogoClassificacao.MED) || (classificacao.intTipoDeClassificacao == (int)Utilidades.CatalogoClassificacao.CPMED) ? (int)Produto.Cursos.MED
                                                                           : produto.intProductGroup2 ?? 0,
                                                                   IdProdutoGrupo = produto.intProductGroup2 ?? 0,
                                                                   IdSubEspecialidade = produto.intProductGroup3 ?? 0,
                                                                   QuestaoAutorizada = false,
                                                                   QuestaoImpressa = (from autorizacao in questao.tblConcursoQuestao_Classificacao_Autorizacao
                                                                                      join apostilaLiberada in ctx.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada on autorizacao.intMaterialID equals apostilaLiberada.intProductID
                                                                                      where (autorizacao.bitAutorizacao ?? false) && apostilaLiberada.bitActive &&
                                                                                            autorizacao.intMaterialID == classificacao.intClassificacaoID
                                                                                      select 1).Any()
                                                               }).Distinct(),
                                           ApostilasAutorizacao = (from autorizacao in questao.tblConcursoQuestao_Classificacao_Autorizacao
                                                                   join produto in ctx.tblProducts on autorizacao.intMaterialID equals produto.intProductID
                                                                   join apostila in ctx.tblBooks on produto.intProductID equals apostila.intBookID
                                                                   join validacao in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on produto.intProductGroup3 equals validacao.intProductGroupID
                                                                   select new Apostila
                                                                          {
                                                                              ID = autorizacao.intMaterialID,
                                                                              Ano = (apostila.intYear ?? 0),
                                                                              Codigo = produto.txtCode,
                                                                              NomeCompleto = string.Concat(produto.txtCode, " - ", (produto.txtName ?? "").Trim().Replace(apostila.intYear.ToString(), "")),
                                                                              IdGrandeArea = validacao.intClassificacaoID,
                                                                              IdProduto = (produto.intProductGroup2 == ((int)Produto.Cursos.MEDMEDCURSO)) ?
                                                                                  (int)Produto.Cursos.MEDCURSO : (produto.intProductGroup2 == ((int)Produto.Cursos.MEDCPMED)) ?
                                                                                      (int)Produto.Cursos.MED : produto.intProductGroup2 ?? 0,
                                                                              IdProdutoGrupo = produto.intProductGroup2 ?? 0,
                                                                              IdSubEspecialidade = produto.intProductGroup3 ?? 0,
                                                                              QuestaoAutorizada = (autorizacao.bitAutorizacao ?? false),
                                                                              QuestaoImpressa = (from apostilaLiberada in ctx.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada
                                                                                                 where apostilaLiberada.intProductID == autorizacao.intMaterialID && (autorizacao.bitAutorizacao ?? false) && apostilaLiberada.bitActive
                                                                                                 select 1).Any()
                                                                          }),
                                           TextoRecurso = questao.txtRecurso,
                                           Anulada = questao.bitAnulada,
                                           AnuladaPosRecursos = questao.bitAnuladaPosRecurso ?? false,
                                           ComentarioBanca = questao.txtComentario_banca_recurso,
                                           DecisaoMedgrupo = ctx
                                               .tblConcurso_Recurso_Status
                                               .Where(h => h.ID_CONCURSO_RECURSO_STATUS == questao.ID_CONCURSO_RECURSO_STATUS)
                                               .Select(h => new StatusRecurso()
                                                            {
                                                                ID = h.ID_CONCURSO_RECURSO_STATUS,
                                                                Descricao = h.txtConcursoQuestao_Status
                                                            })
                                               .FirstOrDefault(),
                                           DecisaoBanca = ctx
                                               .tblConcurso_Recurso_Status
                                               .Where(h => h.ID_CONCURSO_RECURSO_STATUS == questao.intStatus_Banca_Recurso)
                                               .Select(h => new StatusRecurso()
                                                            {
                                                                ID = h.ID_CONCURSO_RECURSO_STATUS,
                                                                Descricao = h.txtConcursoQuestao_Status
                                                            })
                                               .FirstOrDefault(),
                                           Video = (from videoQuestao in ctx.tblVideo_Questao_Concurso
                                                    where videoQuestao.intQuestaoID == questao.intQuestaoID
                                                    select 1).Any(),
                                           PrimeiroComentario = (from pessoaComentario in pessoasComentario
                                                                 where pessoaComentario.QuestaoID == questao.intQuestaoID
                                                                 orderby pessoaComentario.DataAlteracao
                                                                 select new Professor
                                                                        {
                                                                            ID = pessoaComentario.ProfessorID,
                                                                            Nome = pessoaComentario.Nome,
                                                                            DataAcao = pessoaComentario.DataAlteracao
                                                                        }).FirstOrDefault(),
                                           RegistradaPor = (from pessoaClassificacao in pessoasClassificacao
                                                            where pessoaClassificacao.QuestaoID == questao.intQuestaoID && pessoaClassificacao.TipoClassificacaoID == 2
                                                            select new Professor
                                                                   {
                                                                       ID = pessoaClassificacao.ProfessorID,
                                                                       Nome = pessoaClassificacao.Nome,
                                                                       DataAcao = new Nullable<DateTime>()
                                                                   }).FirstOrDefault(),
                                           ClassificadaPor = (from pessoaClassificacao in pessoasClassificacao
                                                              where pessoaClassificacao.QuestaoID == questao.intQuestaoID && ID_CLASSIFICACAO_APOSTILAS.Contains(pessoaClassificacao.TipoClassificacaoID)
                                                              select new Professor
                                                                     {
                                                                         ID = pessoaClassificacao.ProfessorID,
                                                                         Nome = pessoaClassificacao.Nome,
                                                                         DataAcao = new Nullable<DateTime>()
                                                                     }),
                                           ProtocoladaPara = (from protocolo in ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes
                                                              join pessoa in ctx.tblPersons on protocolo.intEmployeeID equals pessoa.intContactID
                                                              where protocolo.intQuestaoID == questao.intQuestaoID
                                                              orderby protocolo.intProtocoloID descending
                                                              select new Professor
                                                                     {
                                                                         ID = pessoa.intContactID,
                                                                         Nome = pessoa.txtName.Trim(),
                                                                         DataAcao = new Nullable<DateTime>()
                                                                     }).FirstOrDefault(),
                                           UltimoComentario = (from pessoaComentario in pessoasComentario
                                                               where pessoaComentario.QuestaoID == questao.intQuestaoID
                                                               orderby pessoaComentario.DataAlteracao descending
                                                               select new Professor
                                                                      {
                                                                          ID = pessoaComentario.ProfessorID,
                                                                          Nome = pessoaComentario.Nome,
                                                                          DataAcao = pessoaComentario.DataAlteracao
                                                                      }).FirstOrDefault(),
                                           EmClassificacaoPor = (from emClassificacao in ctx.tblConcursoQuestaoEmClassificacao
                                                                 join pessoa in ctx.tblPersons on emClassificacao.intEmployeeID equals pessoa.intContactID
                                                                 where emClassificacao.intQuestaoID == questao.intQuestaoID
                                                                       && emClassificacao.intEmployeeID != pp.FiltroIntEmployeeID
                                                                 select new Professor
                                                                        {
                                                                            ID = pessoa.intContactID,
                                                                            Nome = pessoa.txtName,
                                                                            DataAcao = emClassificacao.dteDateTime
                                                                        }).FirstOrDefault()
                                       })
                    .ToList();

                InserirQuestaoEmClassificacao(pp.FiltroIDQuestao, pp.FiltroIntEmployeeID);

                var questaoPreenchida = consulta.FirstOrDefault();

                PreencherGabarito(questaoPreenchida);

                LimparComentario(questaoPreenchida);

                return consulta;
            }
        }

        private void LimparComentario(PPQuestao questaoPreenchida)
        {
            if (!string.IsNullOrEmpty(questaoPreenchida.Comentario))
                questaoPreenchida.Comentario = Utilidades.CleanHtml(questaoPreenchida.Comentario);

            if (questaoPreenchida.Rascunho != null && !string.IsNullOrEmpty(questaoPreenchida.Rascunho.TextoRascunho))
                questaoPreenchida.Rascunho.TextoRascunho = Utilidades.CleanHtml(questaoPreenchida.Rascunho.TextoRascunho);
        }

        private void PreencherGabarito(PPQuestao questaoPreenchida)
        {
            if (questaoPreenchida != null)
            {
                var alternativaGabarito = questaoPreenchida.Alternativas.Where(a => !string.IsNullOrEmpty(a.Gabarito)).FirstOrDefault();

                var gabarito = alternativaGabarito != null ? alternativaGabarito.Gabarito : string.Empty;

                var indexGabarito = questaoPreenchida.Enunciado.ToUpper().IndexOf("GABARITO");

                if (!string.IsNullOrEmpty(gabarito))
                {
                    questaoPreenchida.Alternativas.FirstOrDefault().Gabarito = gabarito;

                    if (indexGabarito > 0) questaoPreenchida.Enunciado = questaoPreenchida.Enunciado.Remove(indexGabarito);
                }
                else if (questaoPreenchida.Enunciado.ToUpper().Contains("GABARITO"))
                {
                    gabarito = questaoPreenchida.Enunciado.Substring(indexGabarito);

                    questaoPreenchida.Enunciado = questaoPreenchida.Enunciado.Remove(indexGabarito);

                    questaoPreenchida.Alternativas.FirstOrDefault().Gabarito = gabarito;
                };
            }
        }

        private void ExcluirQuestoesEmClassificacao(int intEmployeeID = 0)
        {
            using (var ctx = new DesenvContext())
            {
                var lstQuestaoExcluir = new List<tblConcursoQuestaoEmClassificacao>();

                if (intEmployeeID != 0)
                    lstQuestaoExcluir = ctx.tblConcursoQuestaoEmClassificacao
                        .Where(c => c.intEmployeeID == intEmployeeID)
                        .ToList();
                else
                    lstQuestaoExcluir = ctx.tblConcursoQuestaoEmClassificacao
                        .Where(c => c.dteDateTime.AddHours(2) < DateTime.Now)
                        .ToList();


                if (lstQuestaoExcluir.Count > 0)
                {
                    lstQuestaoExcluir.ForEach(c => ctx.tblConcursoQuestaoEmClassificacao.Remove(c));
                    ctx.SaveChanges();
                }
            }
        }

        public PPQuestao ObterQuestaoConcursoCompleta(int idQuestao, int idFuncionario)
        {
            try
            {
                ExcluirQuestoesEmClassificacao(idFuncionario);

                var questaoConcurso = (new QuestaoConcursoEntity()).ObterQuestaoCompleta(idQuestao, idFuncionario);

                InserirQuestaoEmClassificacao(idQuestao, idFuncionario);

                return questaoConcurso;
            }
            catch
            {
                throw;
            }
        }

        private void InserirQuestaoEmClassificacao(int idQuestao, int intEmployeeID)
        {
            using (var ctx = new DesenvContext())
            {
                if (intEmployeeID > 0 && ctx.tblConcursoQuestaoEmClassificacao.Where(c => c.intQuestaoID == idQuestao).Count() == 0)
                {
                    ctx.tblConcursoQuestaoEmClassificacao.Add(new tblConcursoQuestaoEmClassificacao()
                                                              {
                                                                  intQuestaoID = idQuestao,
                                                                  intEmployeeID = intEmployeeID,
                                                                  dteDateTime = DateTime.Now
                                                              });

                    ctx.SaveChanges();
                }
            }
        }      

        private Professor ProtocoloQuestao(DesenvContext ctx, int idQuestao)
        {
            return (from protocolo in ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes
                    join pessoa in ctx.tblPersons on protocolo.intEmployeeID equals pessoa.intContactID
                    where protocolo.intQuestaoID == idQuestao
                    orderby protocolo.intProtocoloID descending
                    select new Professor
                           {
                               ID = pessoa.intContactID,
                               Nome = pessoa.txtName.Trim(),
                               DataAcao = new Nullable<DateTime>()
                           }).FirstOrDefault();
        } 

        private IQueryable<tblConcursoQuestoes> FiltrarQuestoes(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, bool somenteQtdMaxima, DesenvContext ctx)
        {
            questoes = questoes.Where(questao => (questao.txtEnunciado ?? "").Trim() != "");

            questoes = FiltrarPorLabels(pp, questoes, ctx);

            questoes = FiltrarPorAno(pp, questoes);

            questoes = FiltrarPorGrandeArea(pp, questoes, ctx);

            questoes = FiltrarPorProva(pp, questoes, ctx);

            questoes = FiltrarPorApostila(pp, questoes, ctx);

            questoes = FiltrarPorProduto(pp, questoes, ctx);

            questoes = FiltrarPorR1R3(pp, questoes, ctx);

            questoes = FiltrarPorConcurso(pp, questoes, ctx);

            questoes = FiltrarPorConcursoPremium(pp, questoes, ctx);

            questoes = FiltrarPorAnoImpressao(pp, questoes, ctx);

            questoes = FiltrarPorPalavraChave(pp, questoes, ctx);

            questoes = FiltrarPorConcursoBloqueado(pp, questoes, ctx);

            questoes = FiltrarRetirandoProtocolo(pp, questoes, ctx);

            questoes = FiltrarRetirandoFavoritas(pp, questoes, ctx);

            questoes = FiltrarPorGabaritoPos(pp, questoes, ctx);

            questoes = FiltrarPorEtapa(pp, questoes, somenteQtdMaxima, ctx);

            questoes = FiltrarPorQuestoesBloqueadas(pp, questoes, ctx);

            return questoes;
        }   

        private IQueryable<tblConcursoQuestoes> FiltrarPorQuestoesBloqueadas(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            var listaConcursoBloqueados = ctx.tblBloqueioQuestoes.Select(b => b.intQuestaoId);

            questoes = from questao in questoes
                        where !listaConcursoBloqueados.Contains(questao.intQuestaoID)
                        select questao;
         

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarRetirandoProtocolo(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (pp.FiltroExcluirProtocolada)
                questoes = questoes.Where(a => !ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes.Any(p => p.intQuestaoID == a.intQuestaoID));

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorGabaritoPos(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx) 
        {
          if (pp.FiltroGabaritoPos) 
          {
            var listaComGabaritoPos = (from g in ctx.tblConcursoQuestoes_Alternativas
                                       where g.bitCorreta.Value
                                       select new 
                                       {
                                         g.intQuestaoID
                                       }).Distinct();

            var listaSemGabaritoPos = (from sg in ctx.tblConcursoQuestoes_Alternativas
                                       join q in ctx.tblConcursoQuestoes on sg.intQuestaoID equals q.intQuestaoID
                                       where q.bitAnuladaPosRecurso.Value &&
                                         !listaComGabaritoPos.Any(g => g.intQuestaoID == sg.intQuestaoID)
                                       select new 
                                       {
                                         sg.intQuestaoID
                                       }).Distinct();

            var listaDiscursivaComGabaritoPos = (from a in ctx.tblConcursoQuestoes_Alternativas
                                                 where a.txtResposta.Contains("GABARITO POS") || a.txtResposta.Contains("GABARITO PÓS")
                                                 select new 
                                                 {
                                                   a.intQuestaoID
                                                 }).Distinct();

            var listaGabartitoPos = listaDiscursivaComGabaritoPos.Union(listaComGabaritoPos.Union(listaSemGabaritoPos));


            questoes = (from questao in questoes
                        join g in listaGabartitoPos on questao.intQuestaoID equals g.intQuestaoID
                        select questao);
          }

          return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarRetirandoFavoritas(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (pp.FiltroExcluirFavorita)
            {
                var listaFavoritosDemaisProfessores = ctx.tblQuestao_Favoritas
                    .Where(f => f.bitAtivo &&
                                !ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes.Any(p => p.intQuestaoID == f.intQuestaoID))
                    .Select(f => f.intQuestaoID);

                questoes = from questao in questoes
                           where !listaFavoritosDemaisProfessores.Contains(questao.intQuestaoID)
                           select questao;
            }
            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorConcursoBloqueado(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (pp.FiltroConcursoBloqueado)
            {
                var listaConcursoBloqueados = ctx.tblBloqueioConcurso.Where(b => b.intBloqueioAreaId == (int)BloqueioConcursoArea.PortalProfessor).Select(b => b.intProvaId);

                questoes = from questao in questoes
                           join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                           where !listaConcursoBloqueados.Contains(prova.intProvaID)
                           select questao;
            }

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorPalavraChave(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (!string.IsNullOrEmpty(pp.FiltroPalavraChave) && pp.TipoFiltroPalavraChave > 0)
            {
                var palavraChave = pp.FiltroPalavraChave.Split(';').Where(p => p.Trim().ToLower() != string.Empty);

                if (palavraChave.Count() > 0)
                {
                    var filtrarEnunciado = pp.TipoFiltroPalavraChave.HasFlag(PPQuestao.TipoFiltroPalavraChaveLogico.Enunciado);
                    var filtrarComentario = pp.TipoFiltroPalavraChave.HasFlag(PPQuestao.TipoFiltroPalavraChaveLogico.Comentario);
                    var filtrarAlternativa = pp.TipoFiltroPalavraChave.HasFlag(PPQuestao.TipoFiltroPalavraChaveLogico.Alternativa);
                    var filtrarAlternativaCorreta = pp.TipoFiltroPalavraChave.HasFlag(PPQuestao.TipoFiltroPalavraChaveLogico.AlternativaCorreta);

                    questoes = from questao in questoes
                               where ((filtrarEnunciado && palavraChave.Any(p => questao.txtEnunciado.Trim().ToLower().Contains(p)))
                                      ||
                                      (filtrarComentario && palavraChave.Any(p => questao.txtComentario.Trim().ToLower().Contains(p)))
                                      ||
                                      (filtrarAlternativa && ctx.tblConcursoQuestoes_Alternativas.Any(a => a.intQuestaoID == questao.intQuestaoID &&
                                                                                                           palavraChave.Any(p => a.txtAlternativa.Trim().ToLower().Contains(p))))
                                      ||
                                      (filtrarAlternativaCorreta && ctx.tblConcursoQuestoes_Alternativas.Any(a => a.intQuestaoID == questao.intQuestaoID &&
                                                                                                                  palavraChave.Any(p => a.txtAlternativa.Trim().ToLower().Contains(p))
                                                                                                                  && (a.bitCorreta.Value || (!a.bitCorreta.Value && a.bitCorretaPreliminar.Value)))))
                               select questao;
                }
            }

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorApostila(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (pp.FiltroApostilaEntidade > 0)
            {
                var lstIdTodasApostilas = new ApostilaEntidadeEntity().GetApostilasComUnificacaoPorAno(pp.FiltroApostilaEntidade, pp.FiltroAnoApostila).Select(a => a.ID).ToList();

                var lstQuestaoEmApostilas = ctx.tblConcursoQuestao_Classificacao
                    .Where(c => lstIdTodasApostilas.Contains(c.intClassificacaoID))
                    .Select(c => c.intQuestaoID)
                    .Distinct().ToList();

                questoes = (from questao in questoes
                            where lstQuestaoEmApostilas.Contains(questao.intQuestaoID)
                            select questao);

                return questoes;
            }

            if (pp.FiltroAnoApostila > 0) 
            {
              var lstQuestaoEmApostilas = (from cla in ctx.tblConcursoQuestao_Classificacao
                                           join b in ctx.tblBooks on cla.intClassificacaoID equals b.intBookID
                                           where b.intYear == pp.FiltroAnoApostila
                                           select new {
                                             cla.intQuestaoID
                                           }).Distinct();

              questoes = (from questao in questoes
                          join apo in lstQuestaoEmApostilas on questao.intQuestaoID equals apo.intQuestaoID
                          select questao);

              return questoes;
            }


            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorProva(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (pp.FiltroProva > 0)
            {
                var provasPorTipo = ctx.tblConcurso_Provas.Where(x => x.intProvaTipoID == pp.FiltroProva).Select(x => x.intProvaID);

                questoes = questoes.Where(questao => provasPorTipo.Contains(questao.intProvaID.Value));
            }
            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorGrandeArea(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            // somente questões classificadas em uma área selecionada
            if (pp.FiltroArea > 0)
            {
                var lstIdGrandeArea = ObterListaIdGrandeArea(pp.FiltroArea);

                questoes = from questao in questoes
                           join classificacao in ctx.tblConcursoQuestao_Classificacao on questao.intQuestaoID equals classificacao.intQuestaoID
                           where lstIdGrandeArea.Contains(classificacao.intClassificacaoID)
                           select questao;
            }

            return questoes;
        } 

        private IQueryable<tblConcursoQuestoes> FiltrarPorLabels(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (pp.FiltroLabels.Count > 0)
                return (from questao in questoes
                        join ld in ctx.tblLabelDetails on questao.intQuestaoID equals ld.intObjetoID
                        join l in ctx.tblLabels on ld.intLabelID equals l.intLabelID
                        where pp.FiltroLabels.Contains(ld.intLabelID) && l.intLabelGroupID == 3 && ld.bitAtivo.Value //3 = PortalProfessor-Questao //Criar um Enum com Groups de Labels
                        select questao);

            return questoes;
        }

        private static IQueryable<tblConcursoQuestoes> FiltrarPorAno(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes)
        {
            if (pp.Ano > 0) questoes = questoes.Where(questao => questao.intYear.Value == pp.Ano);

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorAnoImpressao(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (pp.FiltroAnoImpressao != 0)
            {
                questoes = from questao in questoes
                           join autorizacao in ctx.tblConcursoQuestao_Classificacao_Autorizacao on questao.intQuestaoID equals autorizacao.intQuestaoID
                           where autorizacao.bitAutorizacao.Value
                           select questao;

                var autorizadas = ctx.tblConcursoQuestao_Classificacao_Autorizacao.Where(a => a.bitAutorizacao.Value);

                //AQUI
                // if (pp.FiltroAnoImpressao != -1)
                //     autorizadas = autorizadas.Where(b => b.tblProducts.tblBook.intYear == pp.FiltroAnoImpressao);

                //if (pp.FiltroApostilaEntidade > 0)
                //    autorizadas = autorizadas.Where(b => b.tblProduct.tblBook.intBookEntityID == pp.FiltroApostilaEntidade);

                questoes = questoes
                    .Where(questao => autorizadas
                               .Select(autorizada => autorizada.intQuestaoID)
                               .Contains(questao.intQuestaoID));
            }
            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorConcurso(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (!string.IsNullOrEmpty(pp.FiltroConcurso))
                questoes = from questao in questoes
                           join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                           join concurso in ctx.tblConcurso on prova.ID_CONCURSO equals concurso.ID_CONCURSO
                           where concurso.SG_CONCURSO.Trim().ToLower().Contains(pp.FiltroConcurso.Trim().ToLower())
                           select questao;

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorConcursoPremium(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (pp.FiltroBitConcursoPremium > -1 && pp.FiltroBitConcursoNaoPremium > -1)
                questoes = from questao in questoes
                           join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                           join concurso in ctx.tblConcurso on prova.ID_CONCURSO equals concurso.ID_CONCURSO
                           select questao;

            else if (pp.FiltroBitConcursoPremium > -1)
                questoes = from questao in questoes
                           join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                           join concurso in ctx.tblConcurso on prova.ID_CONCURSO equals concurso.ID_CONCURSO
                           join premium in ctx.tblConcursoPremium on concurso.SG_CONCURSO.Trim().ToLower() equals premium.txtSigla.Trim().ToLower()
                           select questao;
            else
                questoes = from questao in questoes
                           join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                           join concurso in ctx.tblConcurso on prova.ID_CONCURSO equals concurso.ID_CONCURSO
                           where !(from premium in ctx.tblConcursoPremium select premium.txtSigla.Trim().ToLower()).Contains(concurso.SG_CONCURSO.Trim().ToLower())
                           select questao;

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorProduto(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            if (pp.FiltroProduto > 0) { 
                var classificacoes = ObterClassificacoes(pp.FiltroProduto);

                var questoesPorProduto = (from q in questoes
                                          join c in ctx.tblConcursoQuestao_Classificacao on q.intQuestaoID equals c.intQuestaoID
                                          where classificacoes.Contains(c.intTipoDeClassificacao)
                                          select new
                                          {
                                              c.intQuestaoID
                                          }).Distinct().Select(x => x.intQuestaoID);

                questoes = questoes.Where(q => questoesPorProduto.Contains(q.intQuestaoID));
            }

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorR1R3(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            // Filtro de questões por R1 ou R3            
            var consultaPorTipo = new QuestaoEntity().GetQuestoesPorTipo(pp.FiltroR1R3, ref ctx);

            questoes = questoes.Where(questao => consultaPorTipo.Contains(questao.intQuestaoID));
            
            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorEtapa(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, bool somenteQtdMaxima, DesenvContext ctx)
        {
            // Filtro padrão de cada etapa
            if (pp.FiltroEtapa > 0)
            {
                switch (pp.FiltroEtapa)
                {
                    case PPQuestao.EtapaPortal.MinhasPendencias:

                        var classificadas = (from c in ctx.tblConcursoQuestao_Classificacao
                                             where TiposNaoUsados.Contains(c.intTipoDeClassificacao)
                                             select new
                                                    {
                                                        c.intQuestaoID,
                                                        c.intClassificacaoID
                                                    }).Distinct();


                        questoes = from filtro in questoes
                                   join classificada in classificadas on filtro.intQuestaoID equals classificada.intQuestaoID
                                   join professor in ctx.tblProfessor_GrandeArea on classificada.intClassificacaoID equals professor.intClassificacaoID
                                   where professor.intProfessorID == pp.FiltroIntEmployeeID
                                   select filtro;

                        break;

                    case PPQuestao.EtapaPortal.SemGrandeArea:
                        questoes = questoes.Where(questao => !ctx.tblConcursoQuestao_Classificacao
                                                      .Where(classificada => TiposNaoUsados.Contains(classificada.intTipoDeClassificacao))
                                                      .Select(classificada => classificada.intQuestaoID)
                                                      .Distinct()
                                                      .Contains(questao.intQuestaoID));
                        break;
                    case PPQuestao.EtapaPortal.SemApostila:

                        questoes = ObterQuestoesPendentesDeApostila(pp, questoes, ctx);
                        questoes = ObterQuestoesComGrandeArea(questoes, ctx);
                        questoes =  ObterQuestoesR3R4(questoes, ctx);
                        break;
                    case PPQuestao.EtapaPortal.SemComentario:
                        questoes = questoes.Where(questao => (questao.txtComentario ?? "").Trim() == "");
                        break;
                    case PPQuestao.EtapaPortal.Protocolo:
                        questoes = questoes.Where(q => ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes.Any(p => p.intQuestaoID == q.intQuestaoID && p.intProtocoloID == pp.FiltroProtocolo));
                        break;
                    case PPQuestao.EtapaPortal.SemAreaClinica:
                        questoes = ObterQuestoesPorClinicaMedicaSemAreaClinica(pp, questoes, ctx);
                        break;
                    case PPQuestao.EtapaPortal.SemVideo:
                        questoes = ObterQuestoesSemVideo(pp, questoes, ctx);
                        break;
                    case PPQuestao.EtapaPortal.SomenteFavoritas:
                        var listaFavoritos = ctx.tblQuestao_Favoritas
                            .Where(f => f.intProfessorID == pp.FiltroIntEmployeeID &&
                                        f.bitAtivo)
                            .Select(f => f.intQuestaoID);

                        questoes = from questao in questoes
                                   where listaFavoritos.Contains(questao.intQuestaoID)
                                   select questao;
                        break;

                    case PPQuestao.EtapaPortal.PendenteClassificacao:
                        questoes = FiltrarPorQuestoesPendentes(questoes, ctx);
                        break;

                    case PPQuestao.EtapaPortal.SomenteComentario:
                        questoes = questoes.Where(q => q.txtComentario != string.Empty);
                        break;

                    default:
                        break;
                }
            }

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> ObterQuestoesComGrandeArea(IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            return from questao in questoes
                   where ctx.tblConcursoQuestao_Classificacao
                       .Where(c => c.intTipoDeClassificacao == 2)
                       .Select(c => c.intQuestaoID)
                       .Contains(questao.intQuestaoID)
                   select questao;
        }

        private IQueryable<tblConcursoQuestoes> ObterQuestoesR1(IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            return from questao in questoes
                   join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                   join provaTipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals provaTipo.intProvaTipoID
                   where !provaTipo.txtDescription.ToUpper().Contains("R3")
                   select questao;
        }

        private IQueryable<tblConcursoQuestoes> ObterQuestoesSemVideo(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            return from questao in questoes
                   join video in ctx.tblVideo_Questao_Concurso on questao.intQuestaoID equals video.intQuestaoID into videosQuestoes
                   from videoQuestao in videosQuestoes.DefaultIfEmpty()
                   where videoQuestao.intQuestaoID == 0
                   select questao;
        }

        private IQueryable<tblConcursoQuestoes> ObterQuestoesPendentesDeApostila(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            var lstIdGrandeArea = ObterListaIdGrandeArea(pp.FiltroArea, true);

            var qualquerApostila = new int[4] {
				                                  (int) GrandeArea.Especiais.Cirurgia,
				                                  (int) GrandeArea.Especiais.ClinicaMedica,
				                                  (int) GrandeArea.Especiais.GO,
				                                  (int) GrandeArea.Especiais.Pediatria
			                                  };

            var classificacoes = ObterClassificacoes(pp.FiltroProduto);

            questoes = RetirarQuestoesSemPendencia(questoes, ctx);

            foreach (var idGrandeArea in lstIdGrandeArea)
            {
                if (qualquerApostila.Any(area => area == idGrandeArea))
                {
                    questoes = questoes.Where(questao => !ctx.tblConcursoQuestao_Classificacao
                                                  .Where(classificacao => classificacoes.Contains(classificacao.intTipoDeClassificacao))
                                                  .Select(classificacao => classificacao.intQuestaoID)
                                                  .Distinct()
                                                  .Contains(questao.intQuestaoID));
                }
                else
                {
                    var questoesEmApostila = from questao in ctx.tblConcursoQuestao_Classificacao
                                             join apostila in ctx.tblProducts on questao.intClassificacaoID equals apostila.intProductID
                                             join va in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on apostila.intProductGroup3 equals va.intProductGroupID
                                             where va.intClassificacaoID == idGrandeArea
                                             select questao.intQuestaoID;

                    questoes = questoes.Where(questao => !questoesEmApostila.Contains(questao.intQuestaoID));
                }
            }

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> ObterQuestoesR3R4(IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            var ApostilasR3R4 = new int[4] {
				                                  (int) PPQuestao.TipoClassificacao.R3_CLINICA.GetHashCode(),
				                                  (int) PPQuestao.TipoClassificacao.R3_CIRURGIA.GetHashCode(),
				                                  (int) PPQuestao.TipoClassificacao.R3_PEDIATRIA.GetHashCode(),
				                                  (int) PPQuestao.TipoClassificacao.R4_GO.GetHashCode()
			                                  };
             
            return from questao in questoes
                   where !ctx.tblConcursoQuestao_Classificacao
                       .Where(c => ApostilasR3R4.Contains(c.intTipoDeClassificacao))
                       .Select(c => c.intQuestaoID)
                       .Contains(questao.intQuestaoID)
                   select questao; 
        }

        private List<int> ObterListaIdGrandeArea(GrandeArea.EspeciaisFlags grandesAreas, bool validarGrandesAreas = false)
        {
            var lstIdGrandeArea = new List<int>();

            if (validarGrandesAreas && grandesAreas == 0)
                grandesAreas = GrandeArea.EspeciaisFlags.Todas;

            foreach (Enum grandeArea in Enum.GetValues(grandesAreas.GetType()))
                if (grandesAreas.HasFlag(grandeArea))
                    lstIdGrandeArea.Add(Utilidades.GetEnumDescription<GrandeArea.EspeciaisFlags>(grandeArea.ToString()));

            return lstIdGrandeArea;
        }

        private int[] ObterClassificacoes(Produto.Produtos filtroProduto)
        {
            switch (filtroProduto)
            {
                case Produto.Produtos.MEDCURSO:
                    return new int[1] { 4 };
                case Produto.Produtos.MED:
                case Produto.Produtos.CPMED:
                    return new int[1] { 5 };
                case Produto.Produtos.MEDELETRO:
                    return new int[1] { 8 };
                case Produto.Produtos.R3CLINICA:
                    return new int[1] { 13 };
                case Produto.Produtos.R3CIRURGIA:
                    return new int[1] { 14 };
                case Produto.Produtos.R3PEDIATRIA:
                    return new int[1] { 15 };
                case Produto.Produtos.R4GO:
                    return new int[1] { 16 };
                case Produto.Produtos.TEGO:
                    return new int[1] { 17 };
                case Produto.Produtos.MASTO:
                    return new int[1] { 18 };
                default:
                    return new int[3] { 4, 5, 8 };
            }
        }

        private IQueryable<tblConcursoQuestoes> RetirarQuestoesSemPendencia(IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            var questoesClassificadasClm = (from cqc in ctx.tblConcursoQuestao_Classificacao
                                            join product in ctx.tblProducts on cqc.intClassificacaoID equals product.intProductID
                                            join validacao in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on product.intProductGroup3 equals validacao.intProductGroupID
                                            where cqc.intTipoDeClassificacao == (int)Utilidades.CatalogoClassificacao.MED && validacao.intClassificacaoID == (int)GrandeArea.Especiais.ClinicaMedica
                                            select cqc.intQuestaoID)
                .Distinct();

            var questoesClassificadasPed = (from cqc in ctx.tblConcursoQuestao_Classificacao
                                            join product in ctx.tblProducts on cqc.intClassificacaoID equals product.intProductID
                                            join validacao in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on product.intProductGroup3 equals validacao.intProductGroupID
                                            where cqc.intTipoDeClassificacao == (int)Utilidades.CatalogoClassificacao.MEDCURSO && validacao.intClassificacaoID == (int)GrandeArea.Especiais.Pediatria
                                            select cqc.intQuestaoID)
                .Distinct();

            var questoesSemPendencia = questoes
                .Where(q => questoesClassificadasClm.Contains(q.intQuestaoID)
                            && questoesClassificadasPed.Contains(q.intQuestaoID))
                .Select(q => q.intQuestaoID);

            questoes = questoes.Where(q => !questoesSemPendencia.Contains(q.intQuestaoID));

            return questoes;
        }

        private IQueryable<tblConcursoQuestoes> ObterQuestoesPorClinicaMedicaSemAreaClinica(PPQuestao pp, IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            return from questao in questoes
                   join classificacao in ctx.tblConcursoQuestao_Classificacao on questao.intQuestaoID equals classificacao.intQuestaoID
                   where classificacao.intClassificacaoID == (int)GrandeArea.Especiais.ClinicaMedica &&
                         !(from classificacao2 in ctx.tblConcursoQuestao_Classificacao
                           where classificacao2.intTipoDeClassificacao == 3
                           select classificacao2.intQuestaoID).Contains(questao.intQuestaoID)
                   select questao;
        }

        private IQueryable<tblConcursoQuestoes> FiltrarPorQuestoesPendentes(IQueryable<tblConcursoQuestoes> questoes, DesenvContext ctx)
        {
            questoes = (from q in questoes
                        join p in ctx.tblConcurso_Provas on q.intProvaID equals p.intProvaID
                        join pt in ctx.tblConcurso_Provas_Tipos on p.intProvaTipoID equals pt.intProvaTipoID
                        where !ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID)
                        || !pt.txtDescription.Contains("R3") && (
                            !ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == (int)ClassificacaoApostilaEnum.MedCurso)
                            || !ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == (int)ClassificacaoApostilaEnum.Med)
                            || (
                                ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == (int)GrandeArea.Especiais.SubEspecialidadeCardiologia)
                                && !ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == (int)ClassificacaoApostilaEnum.MedEletro)
                            )
                        )
                        || pt.txtDescription.Contains("R3") && (
                            (ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == 2 && x.intClassificacaoID == (int)GrandeArea.Especiais.ClinicaMedica)
                                && !ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == (int)ClassificacaoApostilaEnum.R3Clinica))
                             ||
                            (ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == 2 && x.intClassificacaoID == (int)GrandeArea.Especiais.Cirurgia)
                                && !ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == (int)ClassificacaoApostilaEnum.R3Cirurgia))
                             ||
                            (ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == 2 && x.intClassificacaoID == (int)GrandeArea.Especiais.Pediatria)
                                && !ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == (int)ClassificacaoApostilaEnum.R3Pediatria))
                             ||
                            (ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == 2 && x.intClassificacaoID == (int)GrandeArea.Especiais.GO)
                                && !ctx.tblConcursoQuestao_Classificacao.Any(x => x.intQuestaoID == q.intQuestaoID && x.intTipoDeClassificacao == (int)ClassificacaoApostilaEnum.R4GO))
                        )
                        select q
                        );

            return questoes;
        }

        public static int getOrdemPremium(string value)
        {
            if (value == null) return Convert.ToInt32(Constants.Uf_OrdemPremium.Outros);

            switch (value.Trim())
            {
                case "SP":
                    return Convert.ToInt32(Constants.Uf_OrdemPremium.SP);
                case "RJ":
                    return Convert.ToInt32(Constants.Uf_OrdemPremium.RJ);
                default:
                    return Convert.ToInt32(Constants.Uf_OrdemPremium.Outros);
            }
        }          

        private void GetProfessoresEmClassificacao(PPQuestao pp, DesenvContext ctx, List<PPQuestao> questoesRetorno, List<int> questaoIds)
        {
            var todosOsProfessores = (from emClassificacao in ctx.tblConcursoQuestaoEmClassificacao
                                      join pessoa in ctx.tblPersons on emClassificacao.intEmployeeID equals pessoa.intContactID
                                      where emClassificacao.intEmployeeID != pp.FiltroIntEmployeeID
                                      select new
                                      {
                                          QuestaoId = emClassificacao.intQuestaoID,
                                          ID = pessoa.intContactID,
                                          Nome = pessoa.txtName,
                                          DataAcao = emClassificacao.dteDateTime
                                      }).ToList();

            var professores = todosOsProfessores.Where(x => questaoIds.Contains(x.QuestaoId)).ToList();

            foreach (var q in questoesRetorno)
            {
                var pessoa = professores.FirstOrDefault(p => p.QuestaoId == q.Id);

                q.EmClassificacaoPor = new Professor();

                if (pessoa == null)
                {
                    continue;
                }

                q.EmClassificacaoPor.ID = pessoa.ID;
                q.EmClassificacaoPor.Nome = pessoa.Nome;
                q.EmClassificacaoPor.DataAcao = pessoa.DataAcao;
            }
        }

        private void GetProfessoresProtocolados(DesenvContext ctx, List<PPQuestao> questoesRetorno, List<int> questaoIds)
        {
            var todosOsProfessores = (from protocolo in ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes
                                      join pessoa in ctx.tblPersons on protocolo.intEmployeeID equals pessoa.intContactID
                                      orderby protocolo.intProtocoloID descending
                                      select new
                                      {
                                          QuestaoId = protocolo.intQuestaoID,
                                          ID = pessoa.intContactID,
                                          Nome = pessoa.txtName.Trim(),
                                          DataAcao = new DateTime?()
                                      });

            var professoresProtocolados = todosOsProfessores.Where(x => questaoIds.Contains(x.QuestaoId)).ToList();

            foreach (var q in questoesRetorno)
            {
                var pessoa = professoresProtocolados.FirstOrDefault(p => p.QuestaoId == q.Id);

                q.ProtocoladaPara = new Professor();

                if (pessoa == null)
                {
                    continue;
                }

                q.ProtocoladaPara.ID = pessoa.ID;
                q.ProtocoladaPara.Nome = pessoa.Nome;
                q.ProtocoladaPara.DataAcao = pessoa.DataAcao;
            }
        }

        public List<PPQuestao> OrdenarQuestoes(List<PPQuestao> questoes)
        {
            using (var ctx = new DesenvContext())
            {
                var ordemConcurso = ctx.tblCriterioOrdenacao_BuscaTexto
                    .Where(c => c.intTipoOrdenacao == (int)TipoOrdenacao.Concurso)
                    .OrderBy(c => c.intOrdem)
                    .Select(c => new CriterioOrdenacaoBuscaTexto() { Ordem = c.intOrdem, Texto = c.txtTexto.Trim().ToUpper() })
                    .ToList();

                var ordemEstado = ctx.tblCriterioOrdenacao_BuscaTexto
                    .Where(c => c.intTipoOrdenacao == (int)TipoOrdenacao.UF)
                    .OrderBy(c => c.intOrdem)
                    .Select(c => new CriterioOrdenacaoBuscaTexto() { Ordem = c.intOrdem, Texto = c.txtTexto.Trim().ToUpper() })
                    .ToList();

                var ordemPadraoConcurso = new CriterioOrdenacaoBuscaTexto() { Ordem = ordemConcurso.LastOrDefault().Ordem + 1 };

                var ordemPadraoEstado = new CriterioOrdenacaoBuscaTexto() { Ordem = ordemEstado.LastOrDefault().Ordem + 1 };

                return questoes.OrderByDescending(q => q.Concurso.Ano)
                    .ThenBy(q => ordemConcurso.Where(c => c.Texto == q.Concurso.Sigla.Trim().ToUpper()).DefaultIfEmpty(ordemPadraoConcurso).First().Ordem)
                    .ThenBy(q => ordemEstado.Where(c => c.Texto == q.Concurso.UF.Trim().ToUpper()).DefaultIfEmpty(ordemPadraoEstado).First().Ordem)
                    .ThenBy(q => q.Id)
                    .ToList();
            }
        }

        public int GetQuestaoFavoritaProfessor(int aProfessorId)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblQuestao_Favoritas_Professores.Any(p => p.intProfessorID == aProfessorId && p.bitAtivo) ? 1 : 0;
            }
        }   

        private QuestaoFavorita GetQuestaoFavorita(QuestaoFavorita registro)
        {
            return GetQuestoesFavoritas(registro).FirstOrDefault();
        }

        public List<QuestaoFavorita> GetQuestoesFavoritas(QuestaoFavorita registro)
        {
            var strTipoProtocolo = ((int)TipoProtocolo.GrupoQuestoes).ToString();

            using (var ctx = new DesenvContext())
            {
                var result = (from pes in ctx.tblPersons
                              join q in ctx.tblQuestao_Favoritas on pes.intContactID equals q.intProfessorID
                              join cq in ctx.tblConcursoQuestoes on q.intQuestaoID equals cq.intQuestaoID
                              join cp in ctx.tblConcurso_Provas on cq.intProvaID equals cp.intProvaID
                              join c in ctx.tblConcurso on cp.ID_CONCURSO equals c.ID_CONCURSO
                              where
                              (cq.intYear == registro.Ano || registro.Ano == 0) &&
                              q.bitAtivo &&
                              pes.intContactID == registro.ProfessorID &&
                              (q.intQuestaoFavoritaID == registro.ID || registro.ID == 0)
                              select new QuestaoFavorita
                                     {
                                         ID = q.intQuestaoFavoritaID,
                                         ProfessorID = pes.intContactID,
                                         Professor = pes.txtName.Trim(),
                                         QuestaoID = q.intQuestaoID,
                                         Questao = cq.txtEnunciado.Trim(),
                                         //Favorito = EntityFunctions.AddHours(q.dteDataCadastro, PRAZO_EM_HORAS_FAVORITOS) > DateTime.Now,
                                         Protocolado = (from g in ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes
                                                        join cat in ctx.tblConcursoQuestoesGravacaoProtocolo_Catalogo on g.intProtocoloID equals cat.intProtocoloID
                                                        where g.intEmployeeID == registro.ProfessorID &&
                                                              g.intQuestaoID == q.intQuestaoID &&
                                                              cat.intTypeID == strTipoProtocolo
                                                        select g).Any(),
                                         DataCadastro = q.dteDataCadastro,
                                         Concurso = c.SG_CONCURSO.Trim(),
                                         Prova = cp.txtName,
                                         ProvaID = cp.intProvaID,
                                         Justificativa = q.txtJustificativa
                                     });

                return result.ToList();
            }
        }

        public QuestaoFavorita GetQuestaoFavoritaById(int idQuestao)
        {
            var strTipoProtocolo = ((int)TipoProtocolo.GrupoQuestoes).ToString();

            using (var ctx = new DesenvContext())
            {

                var protocolados = (from g in ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes
                                    join cat in ctx.tblConcursoQuestoesGravacaoProtocolo_Catalogo on g.intProtocoloID equals cat.intProtocoloID
                                    join e in ctx.tblPersons on g.intEmployeeID equals e.intContactID
                                    where g.intQuestaoID == idQuestao // && cat.intTypeID == strTipoProtocolo
                                    select new {
                                      Protocolo = g.intProtocoloID,
                                      Professor = e.txtName.Trim(),
                                      IdQuestao = g.intQuestaoID
                                    }).Take(1);

                var result = (from pes in ctx.tblPersons
                              join q in ctx.tblQuestao_Favoritas on pes.intContactID equals q.intProfessorID
                              join cq in ctx.tblConcursoQuestoes on q.intQuestaoID equals cq.intQuestaoID
                              join cp in ctx.tblConcurso_Provas on cq.intProvaID equals cp.intProvaID
                              join c in ctx.tblConcurso on cp.ID_CONCURSO equals c.ID_CONCURSO
                              join p1 in protocolados on q.intQuestaoID equals p1.IdQuestao into p2
                              from p in p2.DefaultIfEmpty()
                              where
                              q.intQuestaoID == idQuestao &&
                              q.bitAtivo
                              select new QuestaoFavorita
                                     {
                                         Ano = cq.intYear ?? 0,
                                         ID = q.intQuestaoFavoritaID,
                                         ProfessorID = pes.intContactID,
                                         Professor = pes.txtName.Trim(),
                                         QuestaoID = q.intQuestaoID,
                                         Questao = cq.txtEnunciado.Trim(),
                                         //Favorito = EntityFunctions.AddHours(q.dteDataCadastro, PRAZO_EM_HORAS_FAVORITOS) > DateTime.Now,
                                         Protocolado = protocolados.Any(),
                                         ProtocoladoPorQuem = p.Professor,
                                         DataCadastro = q.dteDataCadastro,
                                         Concurso = c.SG_CONCURSO.Trim(),
                                         Prova = cp.txtName,
                                         ProvaID = cp.intProvaID,
                                         Justificativa = q.txtJustificativa
                                     });

                return result.FirstOrDefault();
            }

        }

        public QuestaoFavorita SetQuestaoFavorita(QuestaoFavorita aRegistro)
        {
            try
            {
                if (aRegistro.ProfessorID == 0 || aRegistro.QuestaoID == 0) return null;

                using (var ctx = new DesenvContext())
                {
                    var favorita = ctx.tblQuestao_Favoritas.FirstOrDefault(f => f.intProfessorID == aRegistro.ProfessorID && f.intQuestaoID == aRegistro.QuestaoID);

                    if (favorita == null)
                    {
                        favorita = new tblQuestao_Favoritas()
                                   {
                                       intQuestaoID = aRegistro.QuestaoID,
                                       intProfessorID = aRegistro.ProfessorID,
                                       bitAtivo = true,
                                       dteDataCadastro = DateTime.Now,
                                       txtJustificativa = aRegistro.Justificativa
                                   };

                        ctx.tblQuestao_Favoritas.Add(favorita);
                    }
                    else
                    {
                        favorita.bitAtivo = true;
                        favorita.dteDataCadastro = DateTime.Now;
                        favorita.txtJustificativa = aRegistro.Justificativa;
                    }

                    ctx.SaveChanges();
                    aRegistro.ID = favorita.intQuestaoFavoritaID;
                    return GetQuestaoFavorita(aRegistro);
                }
            }

            catch
            {
                throw;
            }
        }

        public int QuestaoFavoritaExcluir(List<QuestaoFavorita> questoes) 
        {
            try 
            {
                using (var ctx = new DesenvContext()) 
                {
                    foreach (QuestaoFavorita questao in questoes)
                        if ((questao.ProfessorID > 0) && (questao.QuestaoID > 0)) 
                        {
                            var favorita = ctx.tblQuestao_Favoritas.FirstOrDefault(f => f.intProfessorID == questao.ProfessorID && f.intQuestaoID == questao.QuestaoID);
                            if (favorita != null) 
                            {
                                favorita.bitAtivo = false;
                                favorita.txtJustificativa = null;
                            }
                        }
                    ctx.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

    }
}