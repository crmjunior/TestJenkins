using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Model;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business
{
    public class MontaProvaBusiness : IMontaProvaBusiness
    {
        private IMontaProvaData _montaProvasRepository;
        
        public MontaProvaBusiness(IMontaProvaData repository)
        {
            this._montaProvasRepository = repository;
        }

		public ProvasAluno GetProvasFiltroContador(List<ProvaAluno> provasContadorQuestoes, int matricula, int idFiltro)
		{
			try
			{
                using(MiniProfiler.Current.Step("Obtendo provas filtro contador"))
                {
                    var retorno = new ProvasAluno();

                    var provasAluno = _montaProvasRepository.ObterProvasAluno(idFiltro);

                    foreach (var prova in provasAluno)
                    {
                        prova.DataCriacao = Utilidades.DateTimeToUnixTimestamp(prova.Criacao);
                        var contador = provasContadorQuestoes.Where(c => (c.ID == prova.ID)).FirstOrDefault();

                        if (contador != null)
                        {
                            prova.QuantidadeQuestoes = contador.QuantidadeQuestoes;
                            prova.NaoRealizadas = contador.NaoRealizadas;
                            prova.Acertos = contador.Acertos;
                            prova.Erros = contador.Erros;
                        }
                        else
                        {
                            var questoes = _montaProvasRepository.GetQuestoesProva(prova);

                            prova.QuantidadeQuestoes = questoes.Count();

                            var questoesSimulado = ObterProvaSimulado(matricula, questoes);
                            var questoesConcurso = ObterProvaConcurso(matricula, questoes);

                            prova.NaoRealizadas = questoesSimulado.NaoRealizadas + questoesConcurso.NaoRealizadas;
                            prova.Acertos = questoesSimulado.Acertos + questoesConcurso.Acertos;
                            prova.Erros = questoesSimulado.Erros + questoesConcurso.Erros;

                            _montaProvasRepository.InserirContadorDeQuestoes(prova, matricula);
                        }

                    }

                    retorno.AddRange(provasAluno);

                    return retorno;
                }
			}
			catch
			{
				throw;
			}

		}

        public ProvasAluno GetProvasFiltro(int matricula, int idFiltro)
        {
            try
            {
                var prvsAluno = new ProvasAluno();

                var provasAluno = _montaProvasRepository.ObterProvasAluno(idFiltro);

                foreach (var prova in provasAluno)
                {
                    prova.DataCriacao = Utilidades.DateTimeToUnixTimestamp(prova.Criacao);

                    //var questoes = _montaProvasRepository.ObterQuestoesMontaProva(prova);
                    var questoes = _montaProvasRepository.GetQuestoesProva(prova);

                    prova.QuantidadeQuestoes = questoes.Count();

                    var questoesSimulado = ObterProvaSimulado(matricula, questoes);
                    var questoesConcurso = ObterProvaConcurso(matricula, questoes);

                    prova.NaoRealizadas = questoesSimulado.NaoRealizadas + questoesConcurso.NaoRealizadas;
                    prova.Acertos = questoesSimulado.Acertos + questoesConcurso.Acertos;
                    prova.Erros = questoesSimulado.Erros + questoesConcurso.Erros;
                }

                prvsAluno.AddRange(provasAluno);

                return prvsAluno;
            }
            catch
            {
                throw;
            }

        }

        public ProvaAlunoDTO ObterProvaSimulado(int matricula, List<KeyValuePair<int, int?>> questoes)
        {
            var prova = new ProvaAlunoDTO();

            var questoesSimulado = questoes.Where(x => x.Value == 1).Select(x => x.Key).ToArray();

            if (questoesSimulado.Any())
            {
                var g =  _montaProvasRepository.ObterRespostasSimulado(matricula, questoesSimulado);
                var respostasSimulado = _montaProvasRepository.ObterRespostasSimulado(matricula, questoesSimulado)
                                                              .OrderByDescending(resposta => resposta.DteCadastro)
                                                              .GroupBy(x => x.QuestaoId).Select(y => y.First()).ToList();

                var respondidas = respostasSimulado.Select(x => x.QuestaoId).Distinct();

                prova.NaoRealizadas = (questoesSimulado.Count()) - (from a in questoesSimulado
                                                                    join b in respondidas on a equals b
                                                                    select a
                                           ).ToList().Count();
                prova.Acertos = respostasSimulado.Count(x => x.Alternativa == x.AlternativaRespondida && !x.Anulada && (x.Correta ?? false));
                prova.Erros = questoesSimulado.Count() - (prova.NaoRealizadas + prova.Acertos);
            }

            return prova;
        }

        public ProvaAlunoDTO ObterProvaConcurso(int matricula, List<KeyValuePair<int, int?>> questoes)
        {
            var prova = new ProvaAlunoDTO();

            var questoesConcurso = questoes.Where(x => x.Value == 2).Select(x => x.Key).ToArray();

            if (questoesConcurso.Any())
            {
                var queryRespostasConcurso = _montaProvasRepository.ObterRespostasConcurso(matricula, questoesConcurso)
                                                                   .OrderByDescending(resposta => resposta.DteCadastro)
                                                                   .GroupBy(x => x.QuestaoId).Select(y => y.First()).ToList();

                var respondidas = queryRespostasConcurso.Select(x => x.QuestaoId).Distinct();


                prova.NaoRealizadas = (questoesConcurso.Count()) - (from a in questoesConcurso
                                                             join b in respondidas on a equals b 
                                                             select a
                                           ).ToList().Count();
                prova.Acertos = queryRespostasConcurso.Count(x => x.Alternativa == x.AlternativaRespondida && !x.Anulada && (x.Correta));
                prova.Erros = questoesConcurso.Count() - (prova.NaoRealizadas + prova.Acertos);
            }
            return prova;
        }

        
        public FiltrosAluno GetProvasAluno(int matricula, int idAplicacao)
        {

            // ======================== LOG
            new Util.Log().SetLog(new LogMsPro
            {
                Matricula = matricula,
                IdApp = (Aplicacoes)idAplicacao,
                Tela = Util.Log.MsProLog_Tela.MinhasProvas,
                Acao = Util.Log.MsProLog_Acao.Abriu
            });
            // ======================== 

            List<FiltroAluno> listaFiltros;

            using (var ctx = new DesenvContext())
            {
                listaFiltros = ctx.tblFiltroAluno_MontaProva.Where(x => x.intClientId == matricula && (bool)x.bitAtivo).Select(x => new FiltroAluno()
                {
                    Anos = x.txtAnos,
                    Concursos = x.txtConcursos,
                    Criacao = x.dteDataCriacao,
                    FiltrosEspeciais = x.txtFiltrosEspeciais,
                    Especialidades = x.txtEspecialidades,
                    Matricula = x.intClientId,
                    PalavraChave = x.txtPalavraChave,
                    Id = x.intID,
                    Nome = x.txtNome
                }).OrderByDescending(x => x.Id).ToList();
            }

			var provasContadorQuestoes = _montaProvasRepository.ObterContadorDeQuestoes(matricula);
			var filtrosAluno = new FiltrosAluno();
            filtrosAluno.AddRange(listaFiltros);

            filtrosAluno.ForEach(x =>
            {
                x.ProvasAluno = GetProvasFiltroContador(provasContadorQuestoes, matricula, x.Id);
                x.QuantidadeQuestoesNaoAssociadas = _montaProvasRepository.GetQuantidadeQuestoesNaoAssociadas(x.Id);
                x.QuantidadeQuestoesAssociadas = _montaProvasRepository.GetQuantidadeQuestoesFiltro(x.Id);
            });

            return filtrosAluno;
        }


        public FiltrosAluno GetProvasAlunoNovo(int matricula, int idAplicacao , int page , int limit)
        {
            using(MiniProfiler.Current.Step("Obtendo provas aluno novo"))
            {

                // ======================== LOG
                new Util.Log().SetLog(new LogMsPro
                {
                    Matricula = matricula,
                    IdApp = (Aplicacoes)idAplicacao,
                    Tela = Util.Log.MsProLog_Tela.MinhasProvas,
                    Acao = Util.Log.MsProLog_Acao.Abriu
                });
                // ======================== 

                List<FiltroAluno> listaFiltros = _montaProvasRepository.GetFiltrosAluno(matricula, page, limit);
                var provasContadorQuestoes = _montaProvasRepository.ObterContadorDeQuestoes(matricula);
                var filtrosAluno = new FiltrosAluno();
                filtrosAluno.AddRange(listaFiltros);
                
                filtrosAluno.ForEach(x =>
                {
                    int qtdQuestoesNaoAssociadas = _montaProvasRepository.GetQuantidadeQuestoesNaoAssociadas(x.Id);
                    int qtdQuestoesFiltro = _montaProvasRepository.GetQuantidadeQuestoesFiltro(x.Id);
                    
                    
                    int qtdQuestoes = x.QuantidadeQuestoesOrNull.HasValue ? x.QuantidadeQuestoesOrNull.Value : qtdQuestoesFiltro;
                    x.QuantidadeQuestoes = qtdQuestoes;
                    x.ProvasAluno = GetProvasFiltroContador(provasContadorQuestoes, matricula, x.Id);
                    x.QuantidadeQuestoesAssociadas = qtdQuestoes;
                    qtdQuestoesNaoAssociadas = x.QuantidadeQuestoesOrNull.HasValue ? qtdQuestoes - qtdQuestoesFiltro : qtdQuestoesNaoAssociadas;
                    x.QuantidadeQuestoesNaoAssociadas = qtdQuestoesNaoAssociadas;
                            });

                return filtrosAluno;
            }
        }

        public bool SincronizarRepositorioQuestoes(int ano)
        {
            using(MiniProfiler.Current.Step("Sincronizando repositório questões"))
            {
                var montaProvaMongoManager = new MontaProvaManager();

                var anosConcursos = new List<int?>();
                if (ano != 0)
                    anosConcursos.Add(ano);
                else
                    anosConcursos = montaProvaMongoManager.GetListaAnosElegiveisSincronizar();

                foreach (var anoConcurso in anosConcursos)
                {
                    var questoes = montaProvaMongoManager.GetQuestoesElegiveisToMongo(ano);
                    if (questoes.Count() > 0)
                    {
                        montaProvaMongoManager.RemoverQuestoesMongo(questoes);
                        montaProvaMongoManager.InsertQuestoesBatchMongo(questoes);
                        montaProvaMongoManager.Update_ConcursoEntidadeId_QuestoesMongo();
                    }
                }

                return true;
            }
        }
        public int DeleteNovo(ProvaAluno provaAluno) 
        {
            using(MiniProfiler.Current.Step("Deleta prova nova"))
            {
                return _montaProvasRepository.DeleteNovo(provaAluno);
            }
        }

        public int AlterarQuestoesProvaNovo(int idFiltro, int idProva, int quantidade)
        {
            return _montaProvasRepository.AlterarQuestoesProvaNovo(idFiltro, idProva, quantidade);
        }
    }
}