using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_API.Academico;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class CartaoRespostaEntity : ICartaoRespostaData
    {
        public bool ExisteRespostasDiscursivas(int clientID, int exercicioID, int questaoID)
        {
            using (var ctx = new AcademicoContext())
            {
                return (
                    from c in ctx.tblCartaoResposta_Discursiva
                    join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                    where
                        h.intClientID == clientID &&
                        h.intExercicioID == exercicioID &&
                        c.intQuestaoDiscursivaID == questaoID
                    select c
                ).Any();
            }
        }

        public CartoesResposta GetCartaoRespostaConcurso(int ExercicioID, int ClientID)
        {
            try
            {
                int[] tipoExercicio = new int[] { (int)Exercicio.tipoExercicio.CONCURSO, (int)Exercicio.tipoExercicio.MONTAPROVA };
                using (var ctx = new AcademicoContext())
                {//TODO: melhorar performance nas notações abaixo 
                    using (var ctxMatDir = new DesenvContext())
                    {
                        var CartaoRespostaRetorno = new CartoesResposta();

                        var questoes = (from q in ctxMatDir.tblConcursoQuestoes
                                        orderby q.intOrder
                                        where q.intProvaID == ExercicioID
                                        select new Questao()
                                        {
                                            Id = q.intQuestaoID,
                                            Anulada = q.bitAnulada == true ? q.bitAnulada : q.bitAnuladaPosRecurso ?? false, //buscar na classificacao se tiver tipo 6 é pq tem questao com problema, nao exibe-- como se fosse anulada
                                            Tipo = q.bitDiscursiva ? 2 : 1
                                        }).ToList();

                        var idsQuestoes = questoes.Select(q => q.Id).ToList();

                        var todasAlternativas = (from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                                                 where idsQuestoes.Contains(a.intQuestaoID)
                                                 select new Alternativa()
                                                 {
                                                     Correta = (bool)a.bitCorreta,
                                                     CorretaPreliminar = (bool)a.bitCorretaPreliminar,
                                                     IdQuestao = a.intQuestaoID,
                                                     Imagem = a.txtImagem,
                                                     ImagemOtimizada = a.txtImagemOtimizada
                                                 }).ToList();



                        // _________________________________ MARCAÇOES  OBJETIVAS
                        var ultimasMarcacoesObjetivaProva = (from c in ctx.tblCartaoResposta_objetiva
                                                             where c.intClientID == ClientID //&& h.intExercicioID == ExercicioID
                                                             && tipoExercicio.Any(tiposIds => tiposIds == c.intExercicioTipoId)
                                                             && idsQuestoes.Any(ids => ids == c.intQuestaoID)
                                                             orderby c.intID descending
                                                             select new 
                                                                {   intQuestaoID = c.intClientID,
                                                                    Resposta = c.txtLetraAlternativa// g.Where(c => c.intID == g.Max(y => y.intID)).Select(c => c.txtLetraAlternativa).FirstOrDefault()
                                                                }).ToList();

                        var idsQuestoesMarcacoes = questoes.Select(q => q.Id).ToList();
                        var MarcacoesObjetivasComGabarito = (from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                                                             where idsQuestoesMarcacoes.Any(idQuestoesMarcacoes => idQuestoesMarcacoes == a.intQuestaoID)
                                                             select a).ToList();

                        // _________________________________ RESPOSTAS  DISCURSIVAS
                        var ultimasRespostasDiscursivasProva = (from c in ctx.tblCartaoResposta_Discursiva
                                                                join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                                                where h.intClientID == ClientID //&& h.intExercicioID == ExercicioID
                                                                && tipoExercicio.Any(tipoExecicio => tipoExecicio == c.intExercicioTipoId)
                                                                && idsQuestoes.Any(idsQuestoes => idsQuestoes == c.intQuestaoDiscursivaID)
                                                                group c by c.intQuestaoDiscursivaID into g
                                                                select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) }).ToList();

                        // _________________________________ FAVORITOS
                        //var favoritosProva = (from m in ctx.tblQuestao_Marcacao
                        //                      join cq in ctx.tblConcursoQuestoes on m.intQuestaoID equals cq.intQuestaoID
                        //                      where m.intClientID == ClientID && cq.intProvaID == ExercicioID
                        //                      select m).ToList();

                        foreach (Questao res in questoes)
                        {
                            var alternativas = todasAlternativas.Where(q => q.IdQuestao == res.Id);

                            res.Anulada = res.Anulada;
                            res.Tipo = res.Tipo; // (alternativas.Any(a => a.Correta) || alternativas.Any(a => a.CorretaPreliminar)) ? 1 : 2;
                            res.ExercicioTipoID = (int)Exercicio.tipoExercicio.CONCURSO;

                            if (res.Tipo == 1)
                            {
                                var respostaAluno = ultimasMarcacoesObjetivaProva.Where(q => q.intQuestaoID == res.Id);
                                if (respostaAluno.Any())
                                {
                                    res.Respondida = true;

                                    var resp = MarcacoesObjetivasComGabarito
                                        .Where(q => q.intQuestaoID == res.Id && ((q.bitCorreta ?? false) || (q.bitCorretaPreliminar ?? false))).FirstOrDefault();

                                    if (resp != null)
                                    {
                                        res.Correta = resp.txtLetraAlternativa == respostaAluno.FirstOrDefault().Resposta;
                                    }

                                }
                            }
                            else
                                res.Respondida = ultimasRespostasDiscursivasProva.Any(q => q.intQuestaoID == res.Id);


                            // Marcação
                            //var marcacao = favoritosProva.FirstOrDefault(m => m.intQuestaoID == res.Id);
                            //if (marcacao != null)
                            //    res.Anotacoes = new List<QuestaoAnotacao> { new QuestaoAnotacao { Favorita = true } };

                        }

                        foreach (Questao cart in questoes)
                            CartaoRespostaRetorno.Questoes.Add(cart);

                        return CartaoRespostaRetorno;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CartoesResposta GetCartaoRespostaMontaProva(int ExercicioID, int ClientID)
        {
            try
            {
                using (var ctx = new AcademicoContext())
                {
                    using (var ctxMatDir = new DesenvContext())
                    {
                        //ctx.SetCommandTimeOut(60);

                        var CartaoRespostaRetorno = new CartoesResposta();

                        var listaMontaProvaIntQuestaoId = (
                            from q in ctxMatDir.tblQuestao_MontaProva
                            where q.intProvaId == ExercicioID
                                && q.intTipoExercicioId == (int)Exercicio.tipoExercicio.SIMULADO
                            select q.intQuestaoId
                        ).ToList();

                        var questoesSimulado = (
                            from qu in ctx.tblQuestoes
                            join qs in ctx.tblQuestao_Simulado on qu.intQuestaoID equals qs.intQuestaoID
                            where listaMontaProvaIntQuestaoId.Contains(qu.intQuestaoID)
                            //orderby qu.bitCasoClinico , q.intID
                            select new Questao()
                            {
                                Id = qu.intQuestaoID,
                                Anulada = qu.bitAnulada,
                                Tipo = qu.bitCasoClinico == "1" ? 2 : 1,
                                ExercicioTipoID = (int)Exercicio.tipoExercicio.SIMULADO
                            });

                        var questoesConcurso = (
                                       from q in ctxMatDir.tblQuestao_MontaProva
                                       join qu in ctxMatDir.tblConcursoQuestoes
                                       on q.intQuestaoId equals qu.intQuestaoID
                                       //orderby qu.bitCasoClinico , q.intID
                                       where q.intProvaId == ExercicioID
                                       && q.intTipoExercicioId == (int)Exercicio.tipoExercicio.CONCURSO
                                       select new Questao()
                                       {
                                           Id = q.intQuestaoId,
                                           Anulada = qu.bitAnulada,
                                           Tipo = qu.bitDiscursiva ? 2 : 1,
                                           ExercicioTipoID = (int)Exercicio.tipoExercicio.CONCURSO

                                       });



                        if (questoesSimulado.Any())
                        {

                            var idsQuestoesSimulado = questoesSimulado.Select(q => q.Id).ToList();

                            //Pegamos a última resposta de cada questão desse usuário
                            var respostas = (from c in ctx.tblCartaoResposta_objetiva
                                             where c.intClientID == ClientID
                                             && idsQuestoesSimulado.Contains(c.intQuestaoID)
                                             && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                                             group c by c.intQuestaoID into g
                                             select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) });

                            var queryRespostas = (from r in respostas
                                                  join c in ctx.tblCartaoResposta_objetiva on r.ID equals c.intID
                                                  join a in ctx.tblQuestaoAlternativas on c.intQuestaoID equals a.intQuestaoID
                                                  where c.txtLetraAlternativa == a.txtLetraAlternativa
                                                  select new { c, a })
                                                  .ToLookup(p => p.c.intQuestaoID);

                            //var respostasDiscursivas = (from c in ctx.tblCartaoResposta_Discursiva
                            //                            join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                            //                            where h.intClientID == ClientID
                            //                            && idsQuestoesSimulado.Contains(c.intQuestaoDiscursivaID)
                            //                            && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                            //                            group c by c.intQuestaoDiscursivaID into g
                            //                            select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) });

                            var respostasDiscursivasNoGroup = (from c in ctx.tblCartaoResposta_Discursiva
                                                               join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                                               where h.intClientID == ClientID
                                                               && idsQuestoesSimulado.Contains(c.intQuestaoDiscursivaID)
                                                               && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                                                               select c);

                            var respostasDiscursivas = (from c in respostasDiscursivasNoGroup
                                                        group c by c.intQuestaoDiscursivaID into g
                                                        select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) });

                            var queryRespostasDiscursivas = (from r in respostasDiscursivas
                                                             join c in ctx.tblCartaoResposta_Discursiva on r.ID equals c.intID
                                                             join a in ctx.tblQuestaoAlternativas on c.intQuestaoDiscursivaID equals a.intQuestaoID
                                                             where c.intDicursivaId == a.intAlternativaID
                                                             select new { c, a })
                                                             .ToLookup(p => p.c.intQuestaoDiscursivaID);



                            foreach (Questao res in questoesSimulado)
                            {
                                var query = queryRespostas[res.Id].Select(p => new { p.a.bitCorreta }).FirstOrDefault();
                                var query1 = queryRespostasDiscursivas[res.Id].Select(p => new { p.a.bitCorreta, p.c.txtResposta }).FirstOrDefault();

                                if (query != null)
                                {
                                    var resposta = query;
                                    res.Respondida = true;
                                    res.Correta = resposta.bitCorreta ?? false;
                                }
                                if (query1 != null)
                                {
                                    var resposta = query1;
                                    if (!string.IsNullOrWhiteSpace(resposta.txtResposta))
                                        res.Respondida = true;
                                }

                                CartaoRespostaRetorno.Questoes.Add(res);
                            }
                        }


                        if (questoesConcurso.Any())
                        {


                            var idsQuestoes = questoesConcurso.Select(q => q.Id).ToArray();

                            // _________________________________ MARCAÇOES  OBJETIVAS
                            var listaConcursoQuestoesIntQuestaoID = (from q in ctxMatDir.tblConcursoQuestoes
                                                                     join m in ctxMatDir.tblQuestao_MontaProva on q.intQuestaoID equals m.intQuestaoId
                                                                     where m.intProvaId == ExercicioID
                                                                     select q.intQuestaoID).ToList();

                            var Retorno = (from c in ctx.tblCartaoResposta_objetiva
                                           where c.intClientID == ClientID
                                           && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.CONCURSO
                                           group c by c.intQuestaoID into g
                                           select new
                                           {
                                               intQuestaoID = g.Key,
                                               ID = g.Max(c => c.intID),
                                               Resposta = g.Max(c => c.txtLetraAlternativa)
                                           }).ToList();

                            var ultimasMarcacoesObjetivaProva = (from c in Retorno
                                                                 join a in idsQuestoes on c.intQuestaoID equals a
                                                                 join b in listaConcursoQuestoesIntQuestaoID on c.intQuestaoID equals b
                                                                 select c);
                            var ultimasMarcacoesObjetivasProvaLookup = ultimasMarcacoesObjetivaProva.ToLookup(p => p.intQuestaoID);

                            var idsQuestoesMarcacoes = ultimasMarcacoesObjetivaProva.Select(q => q.intQuestaoID).ToList();
                            var MarcacoesObjetivasComGabarito = (from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                                                                 where idsQuestoesMarcacoes.Contains(a.intQuestaoID)
                                                                 && (a.bitCorreta == true || a.bitCorretaPreliminar == true)
                                                                 select a)
                                                                 .ToLookup(p => p.intQuestaoID);

                            // _________________________________ RESPOSTAS  DISCURSIVAS
                            //var ultimasRespostasDiscursivasProvaRetorno = (from c in ctx.tblCartaoResposta_Discursiva
                            //                                               join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                            //                                               where h.intClientID == ClientID
                            //                                                   // && idsQuestoes.Contains(c.intQuestaoDiscursivaID)
                            //                                               && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.CONCURSO
                            //                                               group c by c.intQuestaoDiscursivaID into g
                            //                                               select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) })
                            //                                        .ToList();

                            var ultimasRespostasDiscursivasProvaRetornoNoGroup = (from c in ctx.tblCartaoResposta_Discursiva
                                                                                  join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                                                                  where h.intClientID == ClientID
                                                                                      // && idsQuestoes.Contains(c.intQuestaoDiscursivaID)
                                                                                  && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.CONCURSO
                                                                                  select c).ToList();

                            var ultimasRespostasDiscursivasProvaRetorno = (from c in ultimasRespostasDiscursivasProvaRetornoNoGroup
                                                                           group c by c.intQuestaoDiscursivaID into g
                                                                           select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) }).ToList();


                            //   .ToLookup(p => p.intQuestaoID);
                            var ultimasRespostasDiscursivasProva = (from c in ultimasRespostasDiscursivasProvaRetorno
                                                                    join a in idsQuestoes on c.intQuestaoID equals a
                                                                    select c).ToLookup(p => p.intQuestaoID);

                            foreach (Questao res in questoesConcurso)
                            {
                                res.ExercicioTipoID = 2;

                                if (res.Tipo == 1)
                                {
                                    var respostaAluno = ultimasMarcacoesObjetivasProvaLookup[res.Id]
                                        .FirstOrDefault();
                                    if (respostaAluno != null)
                                    {
                                        res.Respondida = true;
                                        var resp = MarcacoesObjetivasComGabarito[res.Id]
                                            .FirstOrDefault();
                                        if (resp != null)
                                        {
                                            res.Correta = resp.txtLetraAlternativa == respostaAluno.Resposta;
                                        }

                                    }
                                }
                                else
                                    res.Respondida = ultimasRespostasDiscursivasProva[res.Id]
                                        .FirstOrDefault() != null;

                                CartaoRespostaRetorno.Questoes.Add(res);
                            }
                        }

                        CartaoRespostaRetorno.Questoes = CartaoRespostaRetorno.Questoes.OrderBy(x => x.Tipo).ThenBy(x => x.Id).ToList();
                        return CartaoRespostaRetorno;

                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public CartoesResposta GetCartaoRespostaModoProva(Int32 ExercicioID, Int32 ClientID, Exercicio.tipoExercicio tipo, int historicoExercicioId)
        {
            using(MiniProfiler.Current.Step("Obtendo simulado agendado"))
            {
                CartoesResposta retorno = new CartoesResposta();
                retorno = GetCartaoRespostaSimulado(ExercicioID, ClientID, historicoExercicioId);

                return retorno;
            }
        }

        public CartoesResposta GetCartaoRespostaSimulado(int ExercicioID, int ClientID, int historicoExercicioId)
        {
            try
            {
                CartoesResposta cartaoresposta = new CartoesResposta();
                List<Questao> questoes = new List<Questao>();

                using (var ctx = new AcademicoContext())
                {
                    var possuiOrdem = this.ObterOrdemSimulado(ExercicioID).Count() > 0;

                    questoes = possuiOrdem
                        ? this.ObterQuestoesSimuladoComOrdem(ExercicioID)
                        : this.ObterQuestoesSimuladoSemOrdem(ExercicioID);

                    var questoesIds = questoes.Select(x => x.Id).ToArray();

                    IQueryable<RetornoAnonimo<int, int>> respostas;

                    var respostasQuery = new List<RespostasObjetivasCartaoRespostaSimuladoDTO>();


                    //Pegamos a última resposta de cada questão desse usuário
                    respostas = (from c in ctx.tblCartaoResposta_objetiva
                                 where c.intClientID == ClientID && (historicoExercicioId == 0 || c.intHistoricoExercicioID == historicoExercicioId)
                                 && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                                 && questoesIds.Contains(c.intQuestaoID)
                                 group c by c.intQuestaoID into g
                                 select new RetornoAnonimo<int, int> { Valor1 = g.Key, Valor2 = g.Max(c => c.intID) });


                    respostasQuery = (from r in respostas
                                      join c in ctx.tblCartaoResposta_objetiva on r.Valor2 equals c.intID
                                      join a in ctx.tblQuestaoAlternativas on c.intQuestaoID equals a.intQuestaoID
                                      where c.txtLetraAlternativa == a.txtLetraAlternativa
                                      select new RespostasObjetivasCartaoRespostaSimuladoDTO
                                      {
                                          respostas = r,
                                          cartaoRespostaObjetiva = new CartaoRespostaObjetivaDTO()
                                          {
                                              intID = c.intID,
                                              dteCadastro = c.dteCadastro,
                                              guidQuestao = c.guidQuestao,
                                              intExercicioTipoId = c.intExercicioTipoId,
                                              intHistoricoExercicioID = c.intHistoricoExercicioID,
                                              intQuestaoID = c.intQuestaoID,
                                              txtLetraAlternativa = c.txtLetraAlternativa
                                          },
                                          questaoAlternativa = new QuestaoSimuladoAlternativaDTO()
                                          {
                                              intQuestaoID = a.intQuestaoID,
                                              txtResposta = a.txtResposta,
                                              txtLetraAlternativa = a.txtLetraAlternativa,
                                              bitCorreta = a.bitCorreta,
                                              intAlternativaID = a.intAlternativaID,
                                              txtAlternativa = a.txtAlternativa
                                          }
                                      }).ToList();

                    var respostasDiscursivas = (from c in ctx.tblCartaoResposta_Discursiva
                                                join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                                where h.intClientID == ClientID && (historicoExercicioId == 0 || h.intHistoricoExercicioID == historicoExercicioId)
                                                && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                                                && questoesIds.Contains(c.intQuestaoDiscursivaID)
                                                group c by c.intQuestaoDiscursivaID into g
                                                select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) });

                    var queryRespostasDiscursivas = (from r in respostasDiscursivas
                                                     join c in ctx.tblCartaoResposta_Discursiva on r.ID equals c.intID
                                                     join a in ctx.tblQuestaoAlternativas on c.intQuestaoDiscursivaID equals a.intQuestaoID
                                                     where c.intDicursivaId == a.intAlternativaID
                                                     select new { c, a }).ToList();

                    foreach (Questao res in questoes)
                    {
                        var query = (from r in respostasQuery
                                     where r.cartaoRespostaObjetiva.intQuestaoID == res.Id
                                     select new
                                     {
                                         r.questaoAlternativa.bitCorreta,
                                         r.cartaoRespostaObjetiva.intExercicioTipoId,
                                         r.cartaoRespostaObjetiva.txtLetraAlternativa
                                     }).ToList();

                        var query1 = (from r in queryRespostasDiscursivas where r.c.intQuestaoDiscursivaID == res.Id select new { r.a.bitCorreta, r.c.intExercicioTipoId, r.c.txtResposta }).ToList();

                        if (query.Count() > 0)
                        {
                            var resposta = query.First();
                            res.Respondida = true;
                            res.Correta = resposta.bitCorreta ?? false;
                            res.RespostaAluno = resposta.txtLetraAlternativa;
                        }
                        if (query1.Count() > 0)
                        {
                            var resposta = query1.First();
                            if (!string.IsNullOrWhiteSpace(resposta.txtResposta))
                                res.Respondida = true;
                        }

                        res.ExercicioTipoID = (int)Exercicio.tipoExercicio.SIMULADO;
                    }

                    foreach (Questao cart in questoes)
                        cartaoresposta.Questoes.Add(cart);

                }
                return cartaoresposta;
            }
            catch
            {
                throw;
            }
        }

        public List<Questao> ObterQuestoesSimuladoSemOrdem(int ExercicioID)
        {
            var retorno = new List<Questao>();

            using (var ctx = new AcademicoContext())
            {
                retorno = (
                    from q in ctx.tblQuestoes
                    join qs in ctx.tblQuestao_Simulado on q.intQuestaoID equals qs.intQuestaoID
                    where qs.intSimuladoID == ExercicioID
                    select new Questao()
                    {
                        Id = q.intQuestaoID,
                        Anulada = q.bitAnulada,
                        Tipo = (q.bitCasoClinico == "1") ? 2 : 1,
                        Enunciado = q.txtEnunciado
                    }
                ).ToList();
            }

            return retorno;
        }

        public List<OrdemSimuladoDTO> ObterOrdemSimulado(int ExercicioID)
        {
            var retorno = new List<OrdemSimuladoDTO>();

            using (var ctx = new AcademicoContext())
            {
                retorno = this.ObterOrdemSimulado(ctx, ExercicioID).ToList();
            }

            return retorno;
        }

        private IQueryable<OrdemSimuladoDTO> ObterOrdemSimulado(AcademicoContext ctx, int ExercicioID)
        {
            IQueryable<OrdemSimuladoDTO> retorno = null;

            retorno = (from o in ctx.tblSimuladoOrdenacao
                       where o.intSimuladoID == ExercicioID
                       select new OrdemSimuladoDTO
                       {
                           IntSimuladoID = o.intSimuladoID,
                           IntQuestaoID = o.intQuestaoID,
                           IntVersaoID = 1,
                           IntOrdem = o.intOrdem ?? 0
                       }).Union(
                from v in ctx.tblSimuladoVersao
                where v.intSimuladoID == ExercicioID && v.intVersaoID == 1
                select new OrdemSimuladoDTO
                {
                    IntSimuladoID = v.intSimuladoID,
                    IntQuestaoID = v.intQuestaoID,
                    IntVersaoID = v.intVersaoID,
                    IntOrdem = v.intQuestao
                }
            );

            return retorno;
        }

        public List<Questao> GetQuestoesComVideosCache(int ExercicioID, List<int> idsPPQuestoes, List<int> idsQuestoesImpressas)
        {
            try
            {
                if (RedisCacheManager.CannotCache(RedisCacheConstants.Questao.KeyGetQuestoesComVideos))
                    return GetQuestoesComVideos(ExercicioID, idsPPQuestoes, idsQuestoesImpressas);

                var key = String.Format("{0}:{1}", RedisCacheConstants.Questao.KeyGetQuestoesComVideos, ExercicioID);
                var questoesVideos = RedisCacheManager.GetItemObject<List<Questao>>(key);

                if (questoesVideos == null)
                {
                    questoesVideos = GetQuestoesComVideos(ExercicioID, idsPPQuestoes, idsQuestoesImpressas);
                    if (questoesVideos != null)
                        RedisCacheManager.SetItemObject(key, questoesVideos);
                }

                return questoesVideos;
            }
            catch 
            {
                return GetQuestoesComVideos(ExercicioID, idsPPQuestoes, idsQuestoesImpressas);
            }
        }

        protected List<Questao> GetQuestoesComVideos(int ExercicioID, List<int> idsPPQuestoes, List<int> idsQuestoesImpressas)
        {
            using (var ctx = new DesenvContext())
            {
                var questoesVideos = (from q in ctx.tblConcursoQuestoes
                                      join v in ctx.tblVideo_Questao_Concurso on q.intQuestaoID equals v.intQuestaoID
                                      where idsPPQuestoes.Contains(q.intQuestaoID)
                                          && !idsQuestoesImpressas.Contains(q.intQuestaoID)
                                      select new Questao
                                      {
                                          Id = q.intQuestaoID,
                                          Anulada = q.bitAnulada == true ? q.bitAnulada : q.bitAnuladaPosRecurso ?? false,
                                          Ano = q.intYear ?? 0,
                                          Tipo = q.bitDiscursiva == true ? 2 : 1
                                      })
                                      .Distinct()
                                      .OrderByDescending(q => q.Ano)
                                      .ToList();
                return questoesVideos;
            }
        }

        public List<Questao> GetQuestoesSomenteImpressasComOuSemVideoCache(int ExercicioID, int? anoAtual)
        {
            try
            {
                if (RedisCacheManager.CannotCache(RedisCacheConstants.Questao.KeyGetQuestoesSomenteImpressasComOuSemVideo))
                    return GetQuestoesSomenteImpressasComOuSemVideo(ExercicioID, anoAtual);

                var key = String.Format("{0}:{1}:{2}", RedisCacheConstants.Questao.KeyGetQuestoesSomenteImpressasComOuSemVideo, ExercicioID, anoAtual);
                var questoesImpressas = RedisCacheManager.GetItemObject<List<Questao>>(key);

                if (questoesImpressas == null)
                {

                    questoesImpressas = GetQuestoesSomenteImpressasComOuSemVideo(ExercicioID, anoAtual);
                    if (questoesImpressas != null)
                        RedisCacheManager.SetItemObject(key, questoesImpressas);
                }

                return questoesImpressas;
            }
            catch
            {
                return GetQuestoesSomenteImpressasComOuSemVideo(ExercicioID, anoAtual);
            }
        }

        private List<Questao> GetQuestoesSomenteImpressasComOuSemVideo(int ExercicioID, int? anoAtual)
        {
            using (var ctx = new DesenvContext())
            {

                var questoesImpressas = (from q in ctx.tblConcursoQuestoes
                                         join cqca in ctx.tblConcursoQuestao_Classificacao_Autorizacao on q.intQuestaoID equals cqca.intQuestaoID
                                         join al in ctx.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada on cqca.intMaterialID equals al.intProductID
                                         join b in ctx.tblBooks on al.intProductID equals b.intBookID
                                         join cc in ctx.tblCodigoComentario on new { idApostila = al.intProductID, idQuestao = cqca.intQuestaoID } equals new { idApostila = cc.intApostilaID, idQuestao = cc.intQuestaoID }
                                         join be in ctx.tblBooks_Entities on b.intBookEntityID equals be.intID
                                         where be.intID == ExercicioID
                                             && (bool)cqca.bitAutorizacao.Value
                                             && al.bitActive
                                             && b.intYear == anoAtual
                                         select new Questao
                                         {
                                             Id = q.intQuestaoID,
                                             Anulada = q.bitAnulada ? q.bitAnulada : q.bitAnuladaPosRecurso ?? false,
                                             Ano = q.intYear ?? 0,
                                             Tipo = q.bitDiscursiva ? 2 : 1,
                                             Ordem = cc.IntNumQuestaoID,
                                             Impressa = true
                                         })
                               .Distinct()
                               .OrderBy(y => y.Ordem)
                               .ToList();
                return questoesImpressas;
            }
        }

        public int? GetYearCache()
        {
            try
            {
                if (RedisCacheManager.CannotCache(RedisCacheConstants.Questao.KeyGetYear))
                    return Utilidades.GetYear();

                var keyGetYear = String.Format("{0}", RedisCacheConstants.Questao.KeyGetYear);
                var anoAtual = RedisCacheManager.GetItemObject<int?>(keyGetYear);
                if (anoAtual == null)
                {
                    anoAtual = Utilidades.GetYear();
                    RedisCacheManager.SetItemObject(keyGetYear, anoAtual, new TimeSpan(1, 0, 0, 0));
                }

                return anoAtual;
            }
            catch 
            {
                return Utilidades.GetYear();
            }
        }

        public int? GetYear()
        {
            return Utilidades.GetYear();
        }

        public List<csp_ListaMaterialDireitoAluno_Result> ListaMaterialDireitoAluno(int clientID, int ano, int? intProductGroup1)
        {
            using (var ctxMatDir = new DesenvContext())
            {
                return ctxMatDir.csp_ListaMaterialDireitoAluno(clientID, ano, intProductGroup1).ToList();
            }
        }

        public List<ConcursoQuestoes_Alternativas> ListarMarcacoesObjetivasComGabarito(List<int> listaIdsQuestoes)
        {
            using (var ctxMatDir = new DesenvContext())
            {
                return (
                    from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                    where listaIdsQuestoes.Contains(a.intQuestaoID)
                    select new ConcursoQuestoes_Alternativas
                    {
                        intAlternativaID = a.intAlternativaID,
                        intQuestaoID = a.intQuestaoID,
                        intQuestaoIDOld = a.intQuestaoIDOld,
                        txtLetraAlternativa = a.txtLetraAlternativa,
                        txtAlternativa = a.txtAlternativa,
                        bitCorreta = a.bitCorreta,
                        bitCorretaPreliminar = a.bitCorretaPreliminar,
                        txtResposta = a.txtResposta,
                        txtImagem = a.txtImagem,
                        txtImagemOtimizada = a.txtImagemOtimizada
                    }
                ).ToList();
            }
        }

        public List<MarcacoesObjetivaDTO> ListarUltimasMarcacoesObjetiva(int clientID, List<int> listaIdsQuestoes)
        {
            using (var ctx = new AcademicoContext())
            {
                var query = (from c in ctx.tblCartaoResposta_objetiva
                            where c.intClientID == clientID &&
                            listaIdsQuestoes.Contains(c.intQuestaoID)
                            select c).ToList();

                var queryGrouped = query.GroupBy(g => g.intQuestaoID).Select(s => new MarcacoesObjetivaDTO()
                            {
                                IntQuestaoID = s.Key,
                                ID = s.Max(m => m.intID),
                                Resposta = s.Where(w => w.intID == s.Max(m => m.intID))
                                            .Select(c => c.txtLetraAlternativa)
                                            .FirstOrDefault()
                            });

                return queryGrouped.ToList();
            }
        }

        public List<Questao> ObterQuestoesSimuladoComOrdem(int ExercicioID)
        {
            var retorno = new List<Questao>();

            using (var ctx = new AcademicoContext())
            {
                var queryOrdem = ObterOrdemSimulado(ctx, ExercicioID);

                retorno = (
                    from q in ctx.tblQuestoes
                    join qs in ctx.tblQuestao_Simulado on q.intQuestaoID equals qs.intQuestaoID
                    join ordem in queryOrdem on q.intQuestaoID equals ordem.IntQuestaoID
                    orderby ordem.IntOrdem
                    where qs.intSimuladoID == ExercicioID
                    select new Questao()
                    {
                        Id = q.intQuestaoID,
                        Anulada = q.bitAnulada,
                        Tipo = (q.bitCasoClinico == "1") ? 2 : 1,
                        Enunciado = q.txtEnunciado
                    }
                ).ToList();
            }

            return retorno;
        }

        public List<RespostasObjetivasCartaoRespostaSimuladoDTO> GetRespostasObjetivasSimuladoAgendado(int exercicioHistoricoID, int[] questoesIds)
        {
            using (var ctx = new AcademicoContext())
            {
                //Pegamos a última resposta de cada questão desse usuário
                var respostas = (from c in ctx.tblCartaoResposta_objetiva_Simulado_Online
                                 where c.intHistoricoExercicioID == exercicioHistoricoID
                                 && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                                 && questoesIds.Contains(c.intQuestaoID)
                                 group c by c.intQuestaoID into g
                                 select new RetornoAnonimo<int, int>
                                 {
                                     Valor1 = g.Key,
                                     Valor2 = g.Max(c => c.intID)
                                 });


                var listaQuestoesResposta = (from r in respostas
                                             join c in ctx.tblCartaoResposta_objetiva_Simulado_Online on r.Valor2 equals c.intID
                                             join a in ctx.tblQuestaoAlternativas on c.intQuestaoID equals a.intQuestaoID
                                             where c.txtLetraAlternativa == a.txtLetraAlternativa
                                             select new RespostasObjetivasCartaoRespostaSimuladoDTO
                                             {
                                                 respostas = r,
                                                 cartaoRespostaObjetiva = new CartaoRespostaObjetivaDTO()
                                                 {
                                                     intID = c.intID,
                                                     dteCadastro = c.dteCadastro,
                                                     guidQuestao = c.guidQuestao,
                                                     intExercicioTipoId = c.intExercicioTipoId,
                                                     intHistoricoExercicioID = c.intHistoricoExercicioID,
                                                     intQuestaoID = c.intQuestaoID,
                                                     txtLetraAlternativa = c.txtLetraAlternativa
                                                 },
                                                 questaoAlternativa = new QuestaoSimuladoAlternativaDTO()
                                                 {
                                                     intQuestaoID = a.intQuestaoID,
                                                     txtResposta = a.txtResposta,
                                                     txtLetraAlternativa = a.txtLetraAlternativa,
                                                     bitCorreta = a.bitCorreta,
                                                     intAlternativaID = a.intAlternativaID,
                                                     txtAlternativa = a.txtAlternativa
                                                 }
                                             }).ToList();
                return listaQuestoesResposta;
            }
        }

        public List<RespostasDiscursivasCartaoRespostaSimuladoDTO> GetRespostasDiscursivasSimuladoAgendado(int exercicioHistoricoID, int[] questoesIds)
        {
            using (var ctx = new AcademicoContext())
            {
                var respostasDiscursivas = (from c in ctx.tblCartaoResposta_Discursiva
                                            join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                            where c.intHistoricoExercicioID == exercicioHistoricoID
                                            && c.intExercicioTipoId == (int)Exercicio.tipoExercicio.SIMULADO
                                            && questoesIds.Contains(c.intQuestaoDiscursivaID)
                                            group c by c.intQuestaoDiscursivaID into g
                                            select new
                                            {
                                                intQuestaoID = g.Key,
                                                ID = g.Max(c => c.intID)
                                            });

                var listaQuestoesResposta = (from r in respostasDiscursivas
                                             join c in ctx.tblCartaoResposta_Discursiva on r.ID equals c.intID
                                             join a in ctx.tblQuestaoAlternativas on c.intQuestaoDiscursivaID equals a.intQuestaoID
                                             where c.intDicursivaId == a.intAlternativaID
                                             select new RespostasDiscursivasCartaoRespostaSimuladoDTO
                                             {
                                                 cartaoRespostaDiscursiva = new CartaoRespostaDiscursivaDTO()
                                                 {
                                                     intID = c.intID,
                                                     dblNota = c.dblNota,
                                                     dteCadastro = c.dteCadastro,
                                                     intDicursivaId = c.intDicursivaId,
                                                     intExercicioTipoId = c.intExercicioTipoId,
                                                     intHistoricoExercicioID = c.intHistoricoExercicioID,
                                                     intQuestaoDiscursivaID = c.intQuestaoDiscursivaID,
                                                     txtResposta = c.txtResposta
                                                 },
                                                 questaoAlternativa = new QuestaoSimuladoAlternativaDTO()
                                                 {
                                                     txtResposta = a.txtResposta,
                                                     bitCorreta = a.bitCorreta,
                                                     intAlternativaID = a.intAlternativaID,
                                                     intQuestaoID = a.intQuestaoID,
                                                     txtAlternativa = a.txtAlternativa,
                                                     txtLetraAlternativa = a.txtLetraAlternativa
                                                 }
                                             }).ToList();
                return listaQuestoesResposta;
            }
        }

        public CartoesResposta GetCartaoRespostaApostila(int ExercicioID, int ClientID, int idApostila)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obter cartão resposta da apostila"))
                {
                    using (var ctx = new AcademicoContext())
                    {
                        using (var ctxMatDir = new DesenvContext())
                        {
                            var cartaoRespostaRetorno = new CartoesResposta();

                            var ppQuestoes = new QuestaoEntity().GetQuestoesComComentarioApostilaCache(ExercicioID);

                            var idsPPQuestoes = ppQuestoes.Select(p => p.Id).ToList();
                            var questoesImpressas = GetQuestoesSomenteImpressasComOuSemVideoPorApostila(ExercicioID, ctxMatDir, idApostila);

                            var idsQuestoesImpressas = questoesImpressas.Select(a => a.Id).ToList();

                            var questoesVideos = GetQuestoesComVideos(ExercicioID, idsPPQuestoes, idsQuestoesImpressas);

                            questoesImpressas.AddRange(questoesVideos);
                            var questoes = questoesImpressas;

                            var idsQuestoes = questoes.Select(q => q.Id).ToList();

                            //var ultimasMarcacoesObjetiva = (from c in ctx.tblCartaoResposta_objetiva
                            //                                join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                            //                                where h.intClientID == ClientID && h.intExercicioID == ExercicioID
                            //                                group c by c.intQuestaoID into g
                            //                                select new
                            //                                {
                            //                                    intQuestaoID = g.Key,
                            //                                    ID = g.Max(c => c.intID),
                            //                                    Resposta = g.Where(c => c.intID == g.Max(y => y.intID))
                            //                                                .Select(c => c.txtLetraAlternativa)
                            //                                                .FirstOrDefault()
                            //                                })
                            //    .ToList();

                            var idsHistoricoExercicio = (from h in ctx.tblExercicio_Historico
                                                        where h.intClientID == ClientID && h.intExercicioID == ExercicioID
                                                        select h.intHistoricoExercicioID).ToList();

                            var ultimasMarcacoesObjetivaNoGroup = (from c in ctx.tblCartaoResposta_objetiva
                                                                where c.intClientID == ClientID && idsHistoricoExercicio.Contains(c.intHistoricoExercicioID)
                                                                select c).ToList();

                            var ultimasMarcacoesObjetiva = (from c in ultimasMarcacoesObjetivaNoGroup
                                                            group c by c.intQuestaoID into g
                                                            select new
                                                            {
                                                                intQuestaoID = g.Key,
                                                                ID = g.Max(c => c.intID),
                                                                Resposta = g.Where(c => c.intID == g.Max(y => y.intID))
                                                                            .Select(c => c.txtLetraAlternativa)
                                                                            .FirstOrDefault()
                                                            }).ToList();

                            var marcacoesObjetivasComGabarito = (from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                                                                where idsQuestoes.Contains(a.intQuestaoID)
                                                                select a
                                )
                                .ToList();

                            //var ultimasRespostasDiscursivas = (from c in ctx.tblCartaoResposta_Discursiva
                            //                                   join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                            //                                   where h.intClientID == ClientID && h.intExercicioID == ExercicioID
                            //                                   group c by c.intQuestaoDiscursivaID into g
                            //                                   select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) }
                            //    )
                            //    .ToList();

                            var ultimasRespostasDiscursivasNoGroup = (from c in ctx.tblCartaoResposta_Discursiva
                                                                    join h in ctx.tblExercicio_Historico on c.intHistoricoExercicioID equals h.intHistoricoExercicioID
                                                                    where h.intClientID == ClientID && h.intExercicioID == ExercicioID
                                                                    select c).ToList();

                            var ultimasRespostasDiscursivas = (from c in ultimasRespostasDiscursivasNoGroup
                                                            group c by c.intQuestaoDiscursivaID into g
                                                            select new { intQuestaoID = g.Key, ID = g.Max(c => c.intID) }).ToList();

                            foreach (var res in questoes)
                            {
                                res.Anulada = res.Anulada;
                                res.ExercicioTipoID = (int)Exercicio.tipoExercicio.CONCURSO;

                                if (res.Tipo == 1)
                                {
                                    var respostaAluno = ultimasMarcacoesObjetiva.Where(q => q.intQuestaoID == res.Id).ToList();
                                    if (respostaAluno.Any())
                                    {
                                        res.Respondida = true;
                                        var tblConcursoQuestoesAlternativas = marcacoesObjetivasComGabarito
                                            .FirstOrDefault(q => q.intQuestaoID == res.Id && (q.bitCorreta ?? false));

                                        if (tblConcursoQuestoesAlternativas != null)
                                        {
                                            res.Correta = tblConcursoQuestoesAlternativas.txtLetraAlternativa == respostaAluno.FirstOrDefault().Resposta;
                                        }
                                        //Questão não tem Gabarito
                                        else
                                        {

                                            //Verifica se tem pré (gabarito preliminar)
                                            tblConcursoQuestoesAlternativas = marcacoesObjetivasComGabarito
                                            .FirstOrDefault(q => q.intQuestaoID == res.Id && (q.bitCorretaPreliminar ?? false));

                                            if (tblConcursoQuestoesAlternativas != null)
                                            {
                                                res.Correta = tblConcursoQuestoesAlternativas.txtLetraAlternativa == respostaAluno.FirstOrDefault().Resposta;
                                            }

                                        }
                                    }
                                }
                                else
                                    res.Respondida = ultimasRespostasDiscursivas.Any(q => q.intQuestaoID == res.Id);

                                cartaoRespostaRetorno.Questoes.Add(res);
                            }

                            cartaoRespostaRetorno.Questoes = OrdenarQuestoes(cartaoRespostaRetorno).ToList();

                            return cartaoRespostaRetorno;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        protected List<Questao> GetQuestoesSomenteImpressasComOuSemVideoPorApostila(int ExercicioID, DesenvContext ctx, int idApostila)
        {
            return (from q in ctx.tblConcursoQuestoes
                    join cqca in ctx.tblConcursoQuestao_Classificacao_Autorizacao on q.intQuestaoID equals cqca.intQuestaoID
                    join al in ctx.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada on cqca.intMaterialID equals al.intProductID
                    join b in ctx.tblBooks on al.intProductID equals b.intBookID
                    join cc in ctx.tblCodigoComentario on new { idApostila = al.intProductID, idQuestao = cqca.intQuestaoID } equals new { idApostila = cc.intApostilaID, idQuestao = cc.intQuestaoID }
                    join be in ctx.tblBooks_Entities on b.intBookEntityID equals be.intID
                    where be.intID == ExercicioID
                          && (bool)cqca.bitAutorizacao.Value
                          && al.bitActive
                          && cc.intApostilaID == idApostila
                    select new Questao
                    {
                        Id = q.intQuestaoID,
                        Anulada = q.bitAnulada ? q.bitAnulada : q.bitAnuladaPosRecurso ?? false,
                        Ano = q.intYear ?? 0,
                        Tipo = q.bitDiscursiva ? 2 : 1,
                        Ordem = cc.IntNumQuestaoID,
                        Impressa = true
                    })
                .Distinct()
                .OrderBy(y => y.Ordem)
                .ToList();
        }

        protected IEnumerable<Questao> OrdenarQuestoes(CartoesResposta cartaoRespostaRetorno)
        {
            return cartaoRespostaRetorno.Questoes
             .OrderByDescending(q => q.Impressa)
             .ThenByDescending(q => q.Premium)
             .ThenByDescending(q => q.PossuiComentario)
             .ThenBy(q => q.Ordem)
             .ThenByDescending(q => q.Ano)
             .ToList();
        }
    }
}