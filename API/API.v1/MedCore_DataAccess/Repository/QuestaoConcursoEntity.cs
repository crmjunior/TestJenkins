using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using System;
using System.Linq;

namespace MedCore_DataAccess.Repository
{
    public class QuestaoConcursoEntity
    {
        private readonly int[] ID_CLASSIFICACAO_APOSTILAS = { (int)ClassificacaoApostilaEnum.MedCurso,
            (int)ClassificacaoApostilaEnum.Med,
            (int)ClassificacaoApostilaEnum.MedEletro,
            (int)ClassificacaoApostilaEnum.R3Clinica,
            (int)ClassificacaoApostilaEnum.R3Cirurgia,
            (int)ClassificacaoApostilaEnum.R3Pediatria,
            (int)ClassificacaoApostilaEnum.R4GO,
            (int)ClassificacaoApostilaEnum.TEGO,
            (int)ClassificacaoApostilaEnum.MASTO
        };

        private readonly int[] ID_TIPO_CLASSIFICACAO_ESPECIALIDADES = { 2, 3, 9 };
        
        public PPQuestao ObterQuestaoCompleta(int idQuestao, int idFuncionario)
        {
            using (var ctx = new DesenvContext())
            {
                var pessoasClassificacao = (from classificacao in ctx.tblConcursoQuestao_Classificacao
                                            join pessoa in ctx.tblPersons on classificacao.intEmployeeID equals pessoa.intContactID
                                            where classificacao.intQuestaoID == idQuestao
                                            select new
                                            {
                                                QuestaoID = classificacao.intQuestaoID,
                                                Nome = pessoa.txtName.Trim(),
                                                TipoClassificacaoID = classificacao.intTipoDeClassificacao,
                                                ProfessorID = pessoa.intContactID
                                            }).ToList();

                var questaoConcurso = (from questao in ctx.tblConcursoQuestoes
                                       join prova in ctx.tblConcurso_Provas on questao.intProvaID equals prova.intProvaID
                                       join concurso in ctx.tblConcurso on prova.ID_CONCURSO equals concurso.ID_CONCURSO
                                       join tipo in ctx.tblConcurso_Provas_Tipos on prova.intProvaTipoID equals tipo.intProvaTipoID
                                       where questao.intQuestaoID == idQuestao
                                       select new PPQuestao
                                       {
                                           Id = questao.intQuestaoID,
                                           Ordem = questao.intOrder ?? 0,
                                           Ano = questao.intYear ?? 0,
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
                                           Comentario = questao.txtComentario,
                                           Observacao = questao.txtObservacao,
                                           TextoRecurso = questao.txtRecurso,
                                           Anulada = questao.bitAnulada,
                                           AnuladaPosRecursos = questao.bitAnuladaPosRecurso ?? false,
                                           ComentarioBanca = questao.txtComentario_banca_recurso
                                       }).FirstOrDefault();

                questaoConcurso.Rascunho = ctx.tblComentario_Rascunho
                    .Where(x => x.intQuestaoID == idQuestao && x.intEmployeeID == idFuncionario)
                    .Select(r => new Rascunho()
                            {
                                EmployeeId = r.intEmployeeID,
                                QuestaoId = r.intQuestaoID,
                                TextoRascunho = r.txtRascunho
                            }).FirstOrDefault();

                questaoConcurso.PPImagens = ctx.tblQuestaoConcurso_Imagem
                                                        .Where(i => i.intQuestaoID == idQuestao)
                                                        .Select(imagem => new PPQuestaoImagem
                                                        {
                                                            Id = imagem.intID,
                                                            Url = Constants.URLIMAGEMQUESTAO.Replace("IDQUESTAOIMAGEM", imagem.intID.ToString().Trim()),
                                                            Nome = imagem.txtName,
                                                            Label = imagem.txtLabel,
                                                            QuestaoId = imagem.intQuestaoID
                                                        }).ToList();

                questaoConcurso.PPImagensComentario = ctx.tblQuestoesConcursoImagem_Comentario
                                                  .Where(i => i.intQuestaoID == idQuestao)
                                                  .Select(imagemComentario => new PPQuestaoImagem
                                                  {
                                                      Id = imagemComentario.intImagemComentarioID,
                                                      Url = Constants.URLCOMENTARIOIMAGEMCONCURSO.Replace("IDCOMENTARIOIMAGEM", imagemComentario.intImagemComentarioID.ToString().Trim()),
                                                      Nome = imagemComentario.txtName,
                                                      Label = imagemComentario.txtLabel,
                                                      QuestaoId = imagemComentario.intQuestaoID
                                                  }).ToList();

                questaoConcurso.Especialidades = (from classificacao in ctx.tblConcursoQuestao_Classificacao
                                                  join area in ctx.tblConcursoQuestaoCatologoDeClassificacoes on classificacao.intClassificacaoID equals area.intClassificacaoID
                                                  where ID_TIPO_CLASSIFICACAO_ESPECIALIDADES.Contains(classificacao.intTipoDeClassificacao)
                                                  where classificacao.intQuestaoID == idQuestao
                                                  select new Especialidade
                                                  {
                                                      Id = classificacao.intClassificacaoID,
                                                      Descricao = area.intParent != null ? string.Concat(area.txtTipoDeClassificacao, " - ", area.txtSubTipoDeClassificacao) : area.txtSubTipoDeClassificacao,
                                                      DataClassificacao = classificacao.dteDate,
                                                      Editavel = !(from apostila in ctx.tblConcursoQuestao_Classificacao
                                                                   join produto in ctx.tblProducts on apostila.intClassificacaoID equals produto.intProductID
                                                                   join validacao in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on produto.intProductGroup3 equals validacao.intProductGroupID
                                                                   where validacao.intClassificacaoID == area.intClassificacaoID
                                                                   select 1).Any()
                                                  }).ToList();

                questaoConcurso.Alternativas = (from alternativa in ctx.tblConcursoQuestoes_Alternativas
                                                where alternativa.intQuestaoID == idQuestao
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
                                                }).ToList();

                questaoConcurso.Apostilas = (from classificacao in ctx.tblConcursoQuestao_Classificacao
                                             join produto in ctx.tblProducts on classificacao.intClassificacaoID equals produto.intProductID
                                             join apostila in ctx.tblBooks on produto.intProductID equals apostila.intBookID
                                             join validacao in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on produto.intProductGroup3 equals validacao.intProductGroupID
                                             where ID_CLASSIFICACAO_APOSTILAS.Contains(classificacao.intTipoDeClassificacao)
                                             && classificacao.intQuestaoID == idQuestao
                                             select new Apostila
                                             {
                                                 ID = classificacao.intClassificacaoID,
                                                 Ano = (apostila.intYear ?? 0),
                                                 Codigo = produto.txtCode,

                                                 NomeCompleto = string.Concat(produto.txtCode, " - ", (produto.txtName ?? "").Trim().Replace(apostila.intYear.ToString(), "")),
                                                 IdGrandeArea = validacao.intClassificacaoID,
                                                 IdProduto = classificacao.intTipoDeClassificacao == (int)Utilidades.CatalogoClassificacao.MEDCURSO ? (int)Produto.Cursos.MEDCURSO :
                                                            (classificacao.intTipoDeClassificacao == (int)Utilidades.CatalogoClassificacao.MED) || (classificacao.intTipoDeClassificacao == (int)Utilidades.CatalogoClassificacao.CPMED) ? (int)Produto.Cursos.MED
                                                            : produto.intProductGroup2 ?? 0,
                                                 IdProdutoGrupo = produto.intProductGroup2 ?? 0,
                                                 IdSubEspecialidade = produto.intProductGroup3 ?? 0,
                                                 QuestaoAutorizada = false,
                                                 QuestaoImpressa = (from autorizacao in ctx.tblConcursoQuestao_Classificacao_Autorizacao
                                                                    join apostilaLiberada in ctx.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada on autorizacao.intMaterialID equals apostilaLiberada.intProductID
                                                                    where (autorizacao.bitAutorizacao ?? false) && apostilaLiberada.bitActive &&
                                                                    autorizacao.intMaterialID == classificacao.intClassificacaoID
                                                                    select 1).Any()
                                             }).Distinct().ToList();

                questaoConcurso.ApostilasAutorizacao = (from autorizacao in ctx.tblConcursoQuestao_Classificacao_Autorizacao
                                                        join produto in ctx.tblProducts on autorizacao.intMaterialID equals produto.intProductID
                                                        join apostila in ctx.tblBooks on produto.intProductID equals apostila.intBookID
                                                        join validacao in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on produto.intProductGroup3 equals validacao.intProductGroupID
                                                        where autorizacao.intQuestaoID == idQuestao
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
                                                        }).ToList();

                var concursoRecursos = ctx.tblConcursoQuestoes
                    .Where(c => c.intQuestaoID == idQuestao)
                    .Select(c => new {
                        status = c.ID_CONCURSO_RECURSO_STATUS ?? -1,
                        statusBanca = c.intStatus_Banca_Recurso ?? -1
                    }).First();

                questaoConcurso.DecisaoMedgrupo = ctx.tblConcurso_Recurso_Status
                                              .Where(h => h.ID_CONCURSO_RECURSO_STATUS == concursoRecursos.status)
                                              .Select(h => new StatusRecurso()
                                              {
                                                  ID = h.ID_CONCURSO_RECURSO_STATUS,
                                                  Descricao = h.txtConcursoQuestao_Status
                                              })
                                              .FirstOrDefault();

                questaoConcurso.DecisaoBanca = ctx.tblConcurso_Recurso_Status
                                          .Where(h => h.ID_CONCURSO_RECURSO_STATUS == concursoRecursos.statusBanca)
                                          .Select(h => new StatusRecurso()
                                          {
                                              ID = h.ID_CONCURSO_RECURSO_STATUS,
                                              Descricao = h.txtConcursoQuestao_Status
                                          })
                                          .FirstOrDefault();

                questaoConcurso.Video = (from videoQuestao in ctx.tblVideo_Questao_Concurso
                                         where videoQuestao.intQuestaoID == idQuestao
                                         select 1).Any();

                var primeiro = new QuestaoEntity().GetPrimeiroComentario(idQuestao);
                var ultimo = new QuestaoEntity().GetUltimoComentario(idQuestao);
                questaoConcurso.PrimeiroComentario = primeiro.ID != 0 ? primeiro : null;
                questaoConcurso.UltimoComentario = ultimo.ID != 0 ? ultimo : null;

                questaoConcurso.RegistradaPor = (from pessoaClassificacao in pessoasClassificacao
                                                 where pessoaClassificacao.QuestaoID == idQuestao && pessoaClassificacao.TipoClassificacaoID == 2
                                                 select new Professor
                                                 {
                                                     ID = pessoaClassificacao.ProfessorID,
                                                     Nome = pessoaClassificacao.Nome,
                                                     DataAcao = new Nullable<DateTime>()
                                                 }).FirstOrDefault();

                questaoConcurso.ClassificadaPor = (from pessoaClassificacao in pessoasClassificacao
                                                   where pessoaClassificacao.QuestaoID == idQuestao && ID_CLASSIFICACAO_APOSTILAS.Contains(pessoaClassificacao.TipoClassificacaoID)
                                                   select new Professor
                                                   {
                                                       ID = pessoaClassificacao.ProfessorID,
                                                       Nome = pessoaClassificacao.Nome,
                                                       DataAcao = new Nullable<DateTime>()
                                                   }).ToList();

                questaoConcurso.ProtocoladaPara = (from protocolo in ctx.tblConcursoQuestoesGravacaoProtocolo_Questoes
                                                   join pessoa in ctx.tblPersons on protocolo.intEmployeeID equals pessoa.intContactID
                                                   where protocolo.intQuestaoID == idQuestao
                                                   orderby protocolo.intProtocoloID descending
                                                   select new Professor
                                                   {
                                                       ID = pessoa.intContactID,
                                                       Nome = pessoa.txtName.Trim(),
                                                       DataAcao = new Nullable<DateTime>()
                                                   }).FirstOrDefault();

                questaoConcurso.EmClassificacaoPor = (from emClassificacao in ctx.tblConcursoQuestaoEmClassificacao
                                                      join pessoa in ctx.tblPersons on emClassificacao.intEmployeeID equals pessoa.intContactID
                                                      where emClassificacao.intQuestaoID == idQuestao
                                                            && emClassificacao.intEmployeeID != idFuncionario
                                                      select new Professor
                                                      {
                                                          ID = pessoa.intContactID,
                                                          Nome = pessoa.txtName,
                                                          DataAcao = emClassificacao.dteDateTime
                                                      }).FirstOrDefault();

                PreencherGabarito(ref questaoConcurso);

                LimparComentario(ref questaoConcurso);

                return questaoConcurso;
            }
        }     

        private void LimparComentario(ref PPQuestao questaoPreenchida)
        {
            if (!string.IsNullOrEmpty(questaoPreenchida.Comentario))
                questaoPreenchida.Comentario = Utilidades.CleanHtml(questaoPreenchida.Comentario);

            if (questaoPreenchida.Rascunho != null && !string.IsNullOrEmpty(questaoPreenchida.Rascunho.TextoRascunho))
                questaoPreenchida.Rascunho.TextoRascunho = Utilidades.CleanHtml(questaoPreenchida.Rascunho.TextoRascunho);
        }

        private void PreencherGabarito(ref PPQuestao questaoPreenchida)
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
    }
}