using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.DTO;
using StackExchange.Profiling;
using System.Threading.Tasks;

namespace MedCore_DataAccess.Business
{
    public class CartaoRespostaBusiness : ICartaoRespostaBusiness
    {
        private ICartaoRespostaData _repository;
        private IQuestaoData _questaoRepository;
        private IAulaEntityData _aulaEntityData;

        public CartaoRespostaBusiness(ICartaoRespostaData repository, IQuestaoData questaoRepository, IAulaEntityData aulaEntityData)
        {
            this._repository = repository;
            this._questaoRepository = questaoRepository;
            this._aulaEntityData = aulaEntityData;
        }

        //Medsoft Pro
        public CartoesResposta GetCartaoResposta(Int32 ExercicioID, Int32 ClientID, Exercicio.tipoExercicio tipo, int ano = 0)
        {
            CartoesResposta retorno = new CartoesResposta();
            using(MiniProfiler.Current.Step("Obtendo cartão resposta"))
            {
                ano = ano == 0 ? (int)_repository.GetYearCache() : ano;

                switch (tipo)
                {
                    case Exercicio.tipoExercicio.SIMULADO:
                        retorno = _repository.GetCartaoRespostaSimulado(ExercicioID, ClientID, 0);
                        break;
                    case Exercicio.tipoExercicio.CONCURSO:
                        retorno = _repository.GetCartaoRespostaConcurso(ExercicioID, ClientID);
                        break;
                    case Exercicio.tipoExercicio.APOSTILA:
                        retorno = GetCartaoRespostaApostilaPorAno(ExercicioID, ClientID, ano);
                        break;
                    case Exercicio.tipoExercicio.MONTAPROVA:
                        retorno = _repository.GetCartaoRespostaMontaProva(ExercicioID, ClientID);
                        break;
                    default:
                        break;
                }

                return retorno;
            }
        }

        public CartoesResposta GetCartaoRespostaApostilaPorAno(int exercicioID, int clientID, int ano)
        {
            try
            {
                var anoAtual = _repository.GetYear();
                var cartaoRespostaRetorno = new CartoesResposta();
                int semanaAtual = Utilidades.GetNumeroSemanaAtual(DateTime.Now);

                var ppQuestoes = _questaoRepository.GetQuestoesComComentarioApostilaCache(exercicioID);
                
                var listaQuestoes = (from q in ppQuestoes
                                     select new Questao
                                     {
                                        Id = q.Id,
                                        Anulada = q.Anulada,
                                        Ano = q.Ano,
                                        Tipo = q.Tipo,
                                        Ordem = q.Ordem,
                                        Premium = q.Premium,
                                        PossuiComentario = q.PossuiComentario,
                                        OrdemPremium = q.OrdemPremium
                                     });

                var ResultMaterialDireito = _repository.ListaMaterialDireitoAluno(clientID, ano, null);
                var lstMaterialDireito = ResultMaterialDireito.Where(w => w.intBookEntityID == exercicioID && w.anoCronograma == ano).Distinct().ToList();
                
                if (lstMaterialDireito.Count() == 0)
                {
                    lstMaterialDireito = ResultMaterialDireito.Where(w => w.intBookEntityID == exercicioID).Distinct().ToList();
                    var Permitido = _aulaEntityData.GetApostilasLiberadasSeHouveAulaCronograma(exercicioID, ano).Any() ? 1 : 0;
                    if (lstMaterialDireito.Count() > 0)
                    {
                        lstMaterialDireito.ForEach(x => x.blnPermitido = Permitido);
                    }
                }

                List<Questao> questoesImpressas;
                if (anoAtual == ano)
                {
                    if (lstMaterialDireito.Any(x => x.blnPermitido == 1))
                    {
                        questoesImpressas = _repository.GetQuestoesSomenteImpressasComOuSemVideoCache(exercicioID, ano);
                    }
                    else
                    {
                        questoesImpressas = new List<Questao>();
                    }
                }
                else
                {
                    questoesImpressas = _repository.GetQuestoesSomenteImpressasComOuSemVideoCache(exercicioID, ano);
                }

                var idsQuestoesImpressas = questoesImpressas.Select(a => a.Id).ToList();
                var idsPPQuestoes = ppQuestoes.Select(p => p.Id).ToList();

                var questoesVideos = _repository.GetQuestoesComVideosCache(exercicioID, idsPPQuestoes, idsQuestoesImpressas);
                var questoesAssociadas = listaQuestoes.Where(w => !idsQuestoesImpressas.Contains(w.Id))
                                                    .OrderByDescending(q => q.PossuiComentario)
                                                    .ThenByDescending(q => q.Premium)
                                                    .ThenByDescending(q => q.Ano)
                                                    .ThenBy(q => q.OrdemPremium)
                                                    .ThenByDescending(q => q.PossuiComentario)
                                                    .ThenBy(q => q.Ordem).ToList();
                
                questoesImpressas = questoesImpressas.OrderBy(q => q.Ordem)
                                                    .ThenByDescending(q => q.Ano)
                                                    .ToList();

                questoesImpressas.AddRange(questoesAssociadas);

                var questoes = questoesImpressas;

                var listaIdsQuestoes = questoes.Select(q => q.Id).ToList();

                var ultimasMarcacoesObjetiva = _repository.ListarUltimasMarcacoesObjetiva(clientID, listaIdsQuestoes);
                var marcacoesObjetivasComGabarito = _repository.ListarMarcacoesObjetivasComGabarito(listaIdsQuestoes);
                
                foreach (var res in questoes)
                {
                    res.Anulada = res.Anulada;
                    res.ExercicioTipoID = (int)Exercicio.tipoExercicio.APOSTILA;

                    if (res.Tipo == (int)Questao.tipoQuestao.OBJETIVA)
                    {
                        var respostaAluno = ultimasMarcacoesObjetiva.Where(q => q.IntQuestaoID == res.Id).ToList();
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
                    {
                        res.Respondida = _repository.ExisteRespostasDiscursivas(clientID, exercicioID, res.Id);
                    }

                    cartaoRespostaRetorno.Questoes.Add(res);
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(cartaoRespostaRetorno);
                return cartaoRespostaRetorno;
            }
            catch
            {
                throw;
            }
        }

        public CartaoRespostaSimuladoAgendadoDTO GetCartaoRespostaSimuladoAgendado(int ClientID, int ExercicioID, int ExercicioHistoricoID)
        {
            CartaoRespostaSimuladoAgendadoDTO cartaoresposta = new CartaoRespostaSimuladoAgendadoDTO();
            List<RespostasObjetivasCartaoRespostaSimuladoDTO> respostasObjetivas = new List<RespostasObjetivasCartaoRespostaSimuladoDTO>();
            List<RespostasDiscursivasCartaoRespostaSimuladoDTO> respostasDiscursivas = new List<RespostasDiscursivasCartaoRespostaSimuladoDTO>();

            using(MiniProfiler.Current.Step("Obtendo cartão resposta simulado agendado"))
            {
                var possuiOrdem = _repository.ObterOrdemSimulado(ExercicioID).Count > 0;

                var questoes = possuiOrdem
                    ? _repository.ObterQuestoesSimuladoComOrdem(ExercicioID)
                    : _repository.ObterQuestoesSimuladoSemOrdem(ExercicioID);

                var questoesIds = questoes.Select(x => x.Id).ToArray();

                if(ExercicioHistoricoID > 0)
                {
                    respostasObjetivas = _repository.GetRespostasObjetivasSimuladoAgendado(ExercicioHistoricoID, questoesIds);
                    respostasDiscursivas = _repository.GetRespostasDiscursivasSimuladoAgendado(ExercicioHistoricoID, questoesIds);
                }
                

                foreach (Questao res in questoes)
                {
                    var resObjetiva = (from r in respostasObjetivas
                                        where r.cartaoRespostaObjetiva.intQuestaoID == res.Id
                                        select new
                                        {
                                            r.questaoAlternativa.bitCorreta,
                                            r.cartaoRespostaObjetiva.intExercicioTipoId,
                                            r.cartaoRespostaObjetiva.txtLetraAlternativa
                                        }).ToList();

                    var resDiscursiva = (from r in respostasDiscursivas
                                        where r.cartaoRespostaDiscursiva.intQuestaoDiscursivaID == res.Id
                                        select new
                                        {
                                            r.questaoAlternativa.bitCorreta,
                                            r.cartaoRespostaDiscursiva.intExercicioTipoId,
                                            r.cartaoRespostaDiscursiva.txtResposta
                                        }).ToList();

                    if (resObjetiva.Count() > 0)
                    {
                        var resposta = resObjetiva.First();
                        res.Respondida = true;
                        res.Correta = resposta.bitCorreta ?? false;
                        res.RespostaAluno = resposta.txtLetraAlternativa;
                    }
                    if (resDiscursiva.Count() > 0)
                    {
                        var resposta = resDiscursiva.First();
                        if (!string.IsNullOrWhiteSpace(resposta.txtResposta))
                            res.Respondida = true;
                    }

                    res.ExercicioTipoID = (int)Exercicio.tipoExercicio.SIMULADO;
                }

                foreach (var cart in questoes)
                {
                    cartaoresposta.Questoes.Add(new QuestaoSimuladoAgendadoDTO()
                    {
                        Id = cart.Id,                    
                        Enunciado = cart.Enunciado,                    
                        ExercicioTipoID = cart.ExercicioTipoID,
                        MediaComentario = cart.MediaComentario,
                        Respondida = cart.Respondida,
                        RespostaAluno = cart.RespostaAluno,
                        Tipo = cart.Tipo                    
                    });
                }
                    

                cartaoresposta.HistoricoId = ExercicioHistoricoID;
                cartaoresposta.ClientID = ClientID;

                return cartaoresposta;
            }
        }

        public CartaoRespostaFiltro GetCartaoRespostaFiltro(int ExercicioId, int ClientID, int AppId, int tipoExericioId, int ano = 0, int apostilaId = 0)
        {
            using(MiniProfiler.Current.Step("Obter filtro de cartão resposta"))
            {
                var cartaoResposta = GetCartaoRespostaDetalhado(ExercicioId, ClientID, tipoExericioId, ano, apostilaId);         

                var questoesDTO = cartaoResposta.Questoes.Select(x => new QuestaoFiltroDTO
                {
                    QuestaoId = x.Id,
                    Favorita = x.Anotacoes.Any(y => y.Favorita),
                    Anotada = x.Anotacoes.Any(y => !string.IsNullOrEmpty(y.Anotacao)),
                    Impressa = x.Impressa,
                    NaoRespondida = !x.Respondida,
                    Incorreta = (x.Respondida && !x.Correta),
                    ConcursoSigla = x.Concurso.Nome,
                    Estado = x.Concurso.UF,
                    Ano = x.Concurso.Ano
                    
                }).ToList();

                var filtro = CriarNovoFiltro(questoesDTO, ExercicioId, ClientID, tipoExericioId, ano, apostilaId);

                return filtro;    
            }
        }

        public CartoesResposta GetCartaoRespostaDetalhado(int ExercicioId, int ClientID, int tipoExericioId, int ano, int apostilaId)
        {
            var cartoesResposta = new CartoesResposta();

            if((Exercicio.tipoExercicio)tipoExericioId == Exercicio.tipoExercicio.MEDCODE)
            {
                cartoesResposta = _repository.GetCartaoRespostaApostila(ExercicioId, ClientID, apostilaId);
            }
            else
            {
                cartoesResposta = GetCartaoResposta(ExercicioId, ClientID, (Exercicio.tipoExercicio)tipoExericioId, ano );
            }

            cartoesResposta.ClientID = ClientID;

            cartoesResposta = GetMarcacoesQuestoes(cartoesResposta);
            cartoesResposta = GetDetalhesQuestoes(cartoesResposta);

            for (int i = 0; i < cartoesResposta.Questoes.Count() ; i++)
            {
                cartoesResposta.Questoes[i].Ordem = i + 1;
            }

            return cartoesResposta;
        }

        public CartoesResposta GetMarcacoesQuestoes(CartoesResposta cartoesReposta)
        {
            var questoesMarcadas = _questaoRepository.GetMarcacoesQuestoesAluno(cartoesReposta.ClientID);

            foreach (var questao in cartoesReposta.Questoes)
            {
                questao.Anotacoes = new List<QuestaoAnotacao>();
                var marcacoes = questoesMarcadas.Where(x => x.QuestaoId == questao.Id).Select(m => new QuestaoAnotacao
                {
                    Favorita = m.Favorita,
                    Anotacao = m.Anotacao
                }).ToList();

                questao.Anotacoes = marcacoes;
            }
            return cartoesReposta;
        }

        public CartoesResposta GetDetalhesQuestoes(CartoesResposta cartoesReposta)
        {
            var questaoId = cartoesReposta.Questoes.Select(x => x.Id).ToArray();
            var questoesDetalhes = _questaoRepository.GetQuestoesIds(questaoId);

            Parallel.ForEach(cartoesReposta.Questoes, questao =>
            { 
                var questaoDetalhe = questoesDetalhes.FirstOrDefault(x => x.QuestaoId == questao.Id);
                questao.Concurso = new Concurso();

                if (questaoDetalhe != null)
                {
                questao.Concurso = new Concurso
                {
                    Ano = questaoDetalhe.Ano,
                    Nome = questaoDetalhe.ConcursoSigla,
                    UF = questaoDetalhe.Estado
                };
                }

            });
            return cartoesReposta;
        }

        public CartoesResposta GetCartoesRespostaFiltrado(CartaoRespostaFiltro filtro)
        {
            using(MiniProfiler.Current.Step("Obter cartões respostas filtrados"))
            {
                var cartaoResposta = GetCartaoRespostaDetalhado(filtro.ExercicioId, filtro.ClientId, filtro.TipoExercicioID, filtro.AnoProduto, filtro.ApostilaId);

                var cartaoRespostaFiltrado = FiltrarQuestoes(filtro, cartaoResposta);

                return cartaoResposta;    
            }
        }

        private CartaoRespostaFiltro CriarNovoFiltro(List<QuestaoFiltroDTO> questoes, int ExercicioId, int ClientId, int TipoExercicioID, int ano, int ApostilaId)
        {
            CartaoRespostaFiltro filtro = new CartaoRespostaFiltro { ExercicioId = ExercicioId, ClientId = ClientId, TipoExercicioID = TipoExercicioID , AnoProduto = ano, ApostilaId = ApostilaId };

            filtro.Ano = questoes.GroupBy(a => a.Ano).ToList()
              .Select(x => new CartaoRespostaFiltroItem<int>
              {
                  Item = x.Key,
                  Quantidade = x.ToList().Count(),
                  Questoes = x.Select(y => y.QuestaoId).ToList()
              }).OrderByDescending(o => o.Item).ToList();

            filtro.Estado = questoes.GroupBy(a => a.Estado).ToList()
                .Select(x => new CartaoRespostaFiltroItem<string>
                {
                    Item = x.Key,
                    Quantidade = x.ToList().Count(),
                    Questoes = x.Select(y => y.QuestaoId).ToList()
                }).OrderBy(o => o.Item).ToList();

            filtro.Concurso = questoes.GroupBy(a => a.ConcursoSigla).ToList()
               .Select(x => new CartaoRespostaFiltroItem<string>
               {
                   Item = x.Key,
                   Quantidade = x.ToList().Count(),
                   Questoes = x.Select(y => y.QuestaoId).ToList()
               }).OrderBy(o => o.Item).ToList();

            var impressas = questoes.Where(a => a.Impressa).Select(x => x.QuestaoId).ToList();
            filtro.Impressas = new CartaoRespostaFiltroItem<string>
            {
                Quantidade = impressas.Count(),
                Questoes = impressas
            };

            var favoritas = questoes.Where(a => a.Favorita).Select(x => x.QuestaoId).Distinct().ToList();
            filtro.Favoritas = new CartaoRespostaFiltroItem<string>
            {
                Quantidade = favoritas.Count(),
                Questoes = favoritas
            };

            var anotacoes = questoes.Where(a => a.Anotada).Select(x => x.QuestaoId).Distinct().ToList();
            filtro.Anotacoes = new CartaoRespostaFiltroItem<string>
            {
                Quantidade = anotacoes.Count(),
                Questoes = anotacoes
            };

            var incorretas = questoes.Where(a => a.Incorreta).Select(x => x.QuestaoId).Distinct().ToList();
            filtro.Incorretas = new CartaoRespostaFiltroItem<string>
            {
                Quantidade = incorretas.Count(),
                Questoes = incorretas
            };

            var naoRespondidas = questoes.Where(a => a.NaoRespondida).Select(x => x.QuestaoId).Distinct().ToList();
            filtro.NaoRespondidas = new CartaoRespostaFiltroItem<string>
            {
                Quantidade = naoRespondidas.Count(),
                Questoes = naoRespondidas
            };
            
            filtro.Questoes = questoes.Select(x => x.QuestaoId).ToList();
            filtro.QuantidadeTotal = questoes.Count();

            return filtro;
        }

        public CartoesResposta FiltrarQuestoes(CartaoRespostaFiltro filtro, CartoesResposta cartoesResposta)
        {
            var questoesAnos = filtro.Ano.Where(x => x.Ativo).SelectMany(x => x.Questoes).ToList();
            var questoesEstado = filtro.Estado.Where(x => x.Ativo).SelectMany(x => x.Questoes).ToList();
            var questoesConcurso = filtro.Concurso.Where(x => x.Ativo).SelectMany(x => x.Questoes).ToList();

            var questoesFiltradas = (from q in cartoesResposta.Questoes
                                     where (!filtro.Anotacoes.Ativo || q.Anotacoes.Any(y =>  y.Anotacao != null))
                                        && (!filtro.Favoritas.Ativo || q.Anotacoes.Any(y => y.Favorita))
                                        && (!filtro.Impressas.Ativo || q.Impressa)
                                        && (!filtro.Incorretas.Ativo || (q.Respondida && !q.Correta))
                                        && (!filtro.NaoRespondidas.Ativo || !q.Respondida)
                                        && (!filtro.Ano.Any(x => x.Ativo) || questoesAnos.Contains(q.Id))
                                        && (!filtro.Estado.Any(x => x.Ativo) || questoesEstado.Contains(q.Id))
                                        && (!filtro.Concurso.Any(x => x.Ativo) || questoesConcurso.Contains(q.Id))
                                     select q).ToList();

            cartoesResposta.Questoes = questoesFiltradas;

            return cartoesResposta;
        }
    }
}