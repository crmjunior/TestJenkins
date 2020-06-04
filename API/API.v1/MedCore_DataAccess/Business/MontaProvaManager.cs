using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Repository.MongoRepository.Repository;
using MedCore_DataAccess.Util;
using MongoDB.Bson;
using MongoDB.Driver;
using MedCore_DataAccess.Entidades.MongoDbCollections;
using MedCore_API.Academico;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Repository.MongoRepository.RepositoryManager;
using MongoDB.Driver.Builders;
using MedCore_DataAccess.Contracts.Enums;
using System.Text.RegularExpressions;
using MongoDB.Driver.Linq;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business
{
    public class MontaProvaManager
    {

        #region Metodos Publicos

        public int MontarProva(MontaProvaFiltroPost filtroPost, int? limit = null)
        {
            using(MiniProfiler.Current.Step("Montando prova"))
            {
                IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();
                filtroPost.ExerciciosPermitidos = GetExerciciosPermitidos(filtroPost);

                if (filtroPost.Especialidades.Any())
                    filtroPost.Especialidades = filtroPost.Especialidades.ToArray(); //.Where(e => e != 32)

                var filtro = Filtrar(filtroPost);

                var result = GetQuestoesFiltro(filtroPost, true, limit);

                MontaProvaEntity entity = new MontaProvaEntity();
                var id = entity.Insert(result, filtroPost, filtro);
                return id;
            }
        }

        private List<Entidades.Questao> RemoverQuestoesConcursoSomenteCLM(List<Entidades.Questao> ids, List<BsonDocument> result)
        {
            var especialidades = result.Select(item => (item["Especialidades"].AsBsonArray).ToList()).ToList();

            for (int i = 0; i < especialidades.Count; i++)
            {
                var listaEspecialidades = new List<Entidades.Especialidade>();

                for (int j = 0; j < especialidades[i].Count; j++)
                {
                    var especialidade = new Entidades.Especialidade();
                    especialidade.Id = especialidades[i][j]["EspecialidadeId"].AsInt32;
                    especialidade.ParentId = especialidades[i][j]["EspecialidadeParentId"].AsInt32;
                    listaEspecialidades.Add(especialidade);
                }

                ids.ElementAt(i).Especialidades = listaEspecialidades;
            }

            var questoesConcursoSomenteCLM = ids.Where(x => (x.Concurso != null)
                && (x.Especialidades.Where(y => y.Id == 32).Count() > 0)
                && (x.Especialidades.Where(z => z.ParentId == 32).Count() == 0)).ToList();

            return ids.Except(questoesConcursoSomenteCLM).ToList();
        }

        public List<Entidades.Questao> GetQuestoesFiltro(MontaProvaFiltroPost filtroPost, bool randomize, int? limit = null)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();
            filtroPost.ExerciciosPermitidos = GetExerciciosPermitidos(filtroPost);

            if (filtroPost.Especialidades.Any())
                filtroPost.Especialidades = filtroPost.Especialidades.ToArray();

            var filtro = Filtrar(filtroPost);

            var pipeline = GetPipelineMatch(filtroPost, limit);
            var result = questaoRep.Collection.Aggregate(pipeline).ToList();

            List<Entidades.Questao> ids;

            ids = result.Select(item => new Entidades.Questao { Id = item["QuestaoId"].AsInt32, ExercicioTipoID = item["OrigemQuestao"].AsInt32 }).OrderBy(x => x.Id).ToList();

            ids = RemoverQuestoesConcursoSomenteCLM(ids, result);

            return ids;
        }

        public MontaProvaFiltro Filtrar(MontaProvaFiltroPost filtroPost)
        {
            using(MiniProfiler.Current.Step("Filtrando"))
            {
                filtroPost = CheckHistoricoPermissao(filtroPost);
                filtroPost.ExerciciosPermitidos = GetExerciciosPermitidos(filtroPost);


                IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();
                IRepository<Entidades.MongoDbCollections.Especialidade> espRep = new MongoRepository<Entidades.MongoDbCollections.Especialidade>();

                var resultFiltro = new MontaProvaFiltro();

                //remove duplicacao de subespecialidades de clinica medica

                if (filtroPost.Especialidades.Any())
                    filtroPost.Especialidades = filtroPost.Especialidades.ToArray(); //.Where(e => e != 32)

                ProcessarFiltroEspecialidade(filtroPost, questaoRep, resultFiltro);
                ProcessarFiltroConcurso(filtroPost, questaoRep, resultFiltro);

                ProcessarFiltroAnos(filtroPost, questaoRep, espRep, resultFiltro);
                ProcessarFiltrosEspeciais(filtroPost, resultFiltro);

                resultFiltro.TotalQuestoes = CalcularTotalQuestoes(filtroPost);
                resultFiltro.TodosConcursos = filtroPost.TodosConcursos;
                resultFiltro.TodasEspecialidades = filtroPost.TodasEspecialidades;
                
                resultFiltro.QuantidadeMaximaQuestoesProva = Convert.ToInt32(ConfigurationProvider.Get("Settings:quantidadeQuestaoProva"));

                resultFiltro.ExercicioPermissaoAluno = filtroPost.ExercicioPermissaoAluno;
                resultFiltro.HistoricoQuestaoErradaAluno = filtroPost.HistoricoQuestaoErradaAluno;

                return resultFiltro;
            }

        }

        private void ProcessarFiltrosEspeciais(MontaProvaFiltroPost filtroPost, MontaProvaFiltro resultFiltro)
        {
            filtroPost.FiltroModulo = EModuloFiltro.FiltrosEspeciais;

            if (filtroPost.FiltrosEspeciais.Any())
            {
                var err = GetFiltroEspecialSomenteAsQueEuErrei(filtroPost);
                var ori = GetFiltroEspecialSomenteOriginais(filtroPost);
                var impr = GetFiltroEspcialSomenteImpressas(filtroPost);

                resultFiltro.Filtros.Add(GetFiltrosEspeciaisFiltrado(err.Concat(ori).Concat(impr).ToList(), filtroPost, true));
            }
            else
            {
                resultFiltro.Filtros.Add(GerarModuloSemFiltro(EModuloFiltro.FiltrosEspeciais));
            }
        }

        private void ProcessarFiltroAnos(MontaProvaFiltroPost filtroPost, IRepository<Entidades.MongoDbCollections.Questao> questaoRep, IRepository<Entidades.MongoDbCollections.Especialidade> espRep, MontaProvaFiltro resultFiltro)
        {
            filtroPost.FiltroModulo = EModuloFiltro.UltimosAnos;

            if (filtroPost.Anos.Any())
            {
                if (!filtroPost.Especialidades.Any())
                    filtroPost.Especialidades = espRep.Select(x => (int)x.EspecialidadeId).ToArray(); //.Where(x => x.EspecialidadeId != 32)

                var pipeline = GetPipelineGroup(filtroPost);
                var result = questaoRep.Collection.Aggregate(pipeline).ToList();

                resultFiltro.Filtros.Add(GetAnosFiltrado(result, filtroPost, true));
            }
            else
            {
                resultFiltro.Filtros.Add(GerarModuloSemFiltro(EModuloFiltro.UltimosAnos));
            }
        }

        private void ProcessarFiltroConcurso(MontaProvaFiltroPost filtroPost, IRepository<Entidades.MongoDbCollections.Questao> questaoRep, MontaProvaFiltro resultFiltro)
        {
            filtroPost.FiltroModulo = EModuloFiltro.Concursos;

            if (filtroPost.Concursos.Any())
            {
                var pipeline = GetPipelineGroup(filtroPost);
                var result = questaoRep.Collection.Aggregate(pipeline).ToList();
                resultFiltro.Filtros.Add(GetConcursosFiltrado(result, filtroPost, true));

            }
            else
            {
                resultFiltro.Filtros.Add(GerarModuloSemFiltro(EModuloFiltro.Concursos, filtroPost.TodosConcursos));
            }
        }

        private void ProcessarFiltroEspecialidade(MontaProvaFiltroPost filtroPost, IRepository<Entidades.MongoDbCollections.Questao> questaoRep, MontaProvaFiltro resultFiltro)
        {
            filtroPost.FiltroModulo = EModuloFiltro.Especialidades;

            if (filtroPost.Especialidades.Any())
            {
                var pipeline = GetPipelineGroup(filtroPost);
                var result = questaoRep.Collection.Aggregate(pipeline).ToList();
                resultFiltro.Filtros.Add(GetEspecialidadesFiltrado(result, filtroPost, true));
            }
            else
            {
                resultFiltro.Filtros.Add(GerarModuloSemFiltro(EModuloFiltro.Especialidades, filtroPost.TodasEspecialidades));
            }
        }

        public MontaProvaModuloFiltro GetModuloFiltrado(MontaProvaFiltroPost filtroPost)
        {
            using(MiniProfiler.Current.Step("Obtendo módulo filtrado"))
            {
                var resultFiltro = new MontaProvaModuloFiltro();
                bool tdsEspecialidaades = false;
                IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();
                IRepository<Entidades.MongoDbCollections.Especialidade> espRep = new MongoRepository<Entidades.MongoDbCollections.Especialidade>();

                filtroPost.ExerciciosPermitidos = GetExerciciosPermitidos(filtroPost);

                if (filtroPost.Especialidades.Any())
                    filtroPost.Especialidades = filtroPost.Especialidades.ToArray(); //Where(e => e != 32).
                else
                {
                    tdsEspecialidaades = true;
                    filtroPost.Especialidades = espRep.Select(x => (int)x.EspecialidadeId).ToArray(); //Where(x => x.EspecialidadeId != 32).
                }

                if (filtroPost.FiltroModulo != EModuloFiltro.FiltrosEspeciais)
                {
                    var pipeline = GetPipelineGroup(filtroPost);
                    var result = questaoRep.Collection.Aggregate(pipeline).ToList();

                    switch (filtroPost.FiltroModulo)
                    {

                        case EModuloFiltro.Especialidades:
                            resultFiltro = GetEspecialidadesFiltrado(result, filtroPost, false, tdsEspecialidaades);
                            break;
                        case EModuloFiltro.Concursos:
                            resultFiltro.TotalQuestoesConcurso = CalcularTotalQuestoes(filtroPost);
                            resultFiltro = GetConcursosFiltrado(result, filtroPost, false);
                            break;
                        case EModuloFiltro.UltimosAnos:
                            resultFiltro = GetAnosFiltrado(result, filtroPost, false);
                            break;
                    }
                }
                else
                {
                    var err = GetFiltroEspecialSomenteAsQueEuErrei(filtroPost);
                    var ori = GetFiltroEspecialSomenteOriginais(filtroPost);
                    var impr = GetFiltroEspcialSomenteImpressas(filtroPost);

                    resultFiltro = GetFiltrosEspeciaisFiltrado(err.Concat(ori).Concat(impr).ToList(), filtroPost, false);
                }

                resultFiltro.AnoSelecionado = filtroPost.Anos.FirstOrDefault();

                resultFiltro.SubTotalQuestoes = CalcularTotalQuestoes(filtroPost);
                resultFiltro.TodasEspecialidades = filtroPost.TodasEspecialidades;
                resultFiltro.TodosConcursos = filtroPost.TodosConcursos;

                resultFiltro.HistoricoQuestaoErradaAluno = filtroPost.HistoricoQuestaoErradaAluno;
                resultFiltro.ExercicioPermissaoAluno = filtroPost.ExercicioPermissaoAluno;


                return resultFiltro;
            }

        }

        public ProvaAluno CriarProva(int idFiltro)
        {
            MontaProvaEntity entity = new MontaProvaEntity();

            return entity.InsertProva(idFiltro);
        }

        public int AlterarQuestoesProva(int idFiltro, int idProva, QuestoesMontaProvaPost questoes)
        {
            using(MiniProfiler.Current.Step("Alterando questões da prova"))
            {
                MontaProvaEntity entity = new MontaProvaEntity();

                return entity.AlterarQuestoesProva(idFiltro, idProva, Convert.ToInt32(questoes.Quantidade));
            }
        }

        public int AlterarQuestoesProvaNovo(int idFiltro, int idProva, QuestoesMontaProvaPost questoes)
        {
            using(MiniProfiler.Current.Step("Alterando questões da prova novo"))
            {
                var entity = new MontaProvaEntity();

                //Buscar se o Filtro tem qtd de questões ou é nulo
                var qtdQuestoes = entity.GetFiltroQuantidadeQuestoes(idFiltro);

                //Se tem qtd => é feito da forma nova e pode seguir
                if (qtdQuestoes == null)
                {
                    //Adicionar Quantidade de questões filtradas no registro do filtro
                    var questoesCount = entity.GetQuantidadeQuestoesFiltro(idFiltro);
                    entity.SetFiltroQuantityCounter(idFiltro, questoesCount);

                    //Remover questões não associadas a este filtro
                    entity.DeleteQuestoesNaoAssociadas(idFiltro);
                }

                return entity.AlterarQuestoesProvaNovo(idFiltro, idProva, Convert.ToInt32(questoes.Quantidade));
            }
        }
        #endregion

        #region Modulos

        private MontaProvaModuloFiltro GetConcursosFiltrado(List<BsonDocument> concursos, MontaProvaFiltroPost filtroPost, bool home)
        {
            var filtro = new MontaProvaModuloFiltro();
            filtro.Concursos = new List<MontaProvaModuloFiltroItem>();

            foreach (var item in concursos)
            {
                var itemConcurso = new MontaProvaModuloFiltroItem();
                itemConcurso.Id = item["_id"]["ConcursoEntidadeId"].IsBsonNull ? 0 : item["_id"]["ConcursoEntidadeId"].AsInt32;
                itemConcurso.Subtitulo = item["_id"]["ConcursoNome"].IsBsonNull ? "N/A" : item["_id"]["ConcursoNome"].AsString;
                itemConcurso.Titulo = item["_id"]["ConcursoSigla"].IsBsonNull ? "N/A" : item["_id"]["ConcursoSigla"].AsString;
                itemConcurso.QuantidadeQuestoes = item["dcount"].IsBsonNull ? 0 : item["dcount"].AsInt32;
                itemConcurso.Status = filtroPost.Concursos.Contains(itemConcurso.Id) ? EFiltroMontaProvaStatus.Completo : (itemConcurso.QuantidadeQuestoes > 0 ? EFiltroMontaProvaStatus.Desabilitado : EFiltroMontaProvaStatus.Inativo);

                if (filtroPost.TodosConcursos && itemConcurso.Status != EFiltroMontaProvaStatus.Inativo)
                    itemConcurso.Status = EFiltroMontaProvaStatus.Completo;

                if (itemConcurso.Id == 0)
                    itemConcurso.Status = EFiltroMontaProvaStatus.Invisivel;

                filtro.Concursos.Add(itemConcurso);
            }

            filtro.Concursos = filtro.Concursos.OrderBy(x => x.Titulo).ToList();

            filtro.Ativo = 1;
            if (filtroPost.Concursos.Length > 0)
                filtro.SubTotalQuestoes = filtro.Concursos.Where(a => filtroPost.Concursos.Contains(a.Id)).Sum(x => x.QuantidadeQuestoes);
            else
                filtro.SubTotalQuestoes = filtro.Concursos.Sum(x => x.QuantidadeQuestoes);
            filtro.Modulo = EModuloFiltro.Concursos;
            filtro.TotalQuestoes = filtro.SubTotalQuestoes;

            if (filtroPost.Concursos.Length > 0)
            {
                filtro.Selecao = GetNomesSelecionados(filtro.Concursos);
                filtro.Selecionado = 1;
            }
            else
            {
                filtro.Selecao = "Todas Selecionadas";
                filtro.Selecionado = 0;
            }


            return filtro;
        }

        private MontaProvaModuloFiltro GetEspecialidadesFiltrado(List<BsonDocument> especialidades, MontaProvaFiltroPost filtroPost, bool home, bool todasEspecialidades = false)
        {

            var especialidadesEspeciais = Utilidades.GetEnumValues<Entidades.Especialidade.Especiais>();

            var filtro = new MontaProvaModuloFiltro();
            filtro.Especialidades = new List<MontaProvaModuloFiltroItem>();
            var subs = new List<MontaProvaModuloFiltroItem>();
            int[] bloqueadas = { 0 }; //nulas e clinica medica sem sub /, 32

            var ordem = EspecialidadesOrdenadas();

            foreach (var item in especialidades)
            {

                var itemEspecialidade = new MontaProvaModuloFiltroItem();

                itemEspecialidade.Id = item["_id"]["EspecialidadeId"].IsBsonNull ? 0 : item["_id"]["EspecialidadeId"].AsInt32;
                itemEspecialidade.Titulo = item["_id"]["EspecialidadeNome"].IsBsonNull ? "N/A" : item["_id"]["EspecialidadeNome"].AsString;
                itemEspecialidade.QuantidadeQuestoes = item["count"].IsBsonNull ? 0 : item["count"].AsInt32;
                itemEspecialidade.EspecialidadePaiId = item["_id"]["EspecialidadeParentId"].IsBsonNull ? 0 : item["_id"]["EspecialidadeParentId"].AsInt32;

                itemEspecialidade.Status = (filtroPost.Especialidades.Contains(itemEspecialidade.Id) && !todasEspecialidades)
                    ? EFiltroMontaProvaStatus.Completo
                    : (itemEspecialidade.QuantidadeQuestoes > 0 ? EFiltroMontaProvaStatus.Desabilitado : EFiltroMontaProvaStatus.Inativo);

                itemEspecialidade.Multidisciplinares = GetMultidisciplinar(itemEspecialidade.Id, filtroPost);

                var especOrdem = ordem.Where(x => x.EspecialidadeId == itemEspecialidade.Id).FirstOrDefault();

                itemEspecialidade.Ordem = especOrdem != null ? especOrdem.Ordem : 99;

                if (filtroPost.TodasEspecialidades && itemEspecialidade.Status != EFiltroMontaProvaStatus.Inativo)
                    itemEspecialidade.Status = EFiltroMontaProvaStatus.Completo;


                if (itemEspecialidade.EspecialidadePaiId != 0)
                    subs.Add(itemEspecialidade);
                else
                    filtro.Especialidades.Add(itemEspecialidade);

            }




            //SubEspecialidades

            var clm = filtro.Especialidades.Where(x => x.Id == 32).FirstOrDefault();

            var subEspecCLM = subs.Where(x => x.EspecialidadePaiId == 32).OrderBy(x => x.Ordem).ToList();
            clm.SubItens = new List<MontaProvaModuloFiltroItem>();
            clm.SubItens.AddRange(subEspecCLM);

            var clmPossuiQuestoesSemSub = clm.QuantidadeQuestoes > 0;

            clm.QuantidadeQuestoes += CalcularQuantidadeQuestoesDistintas(subEspecCLM);

            if (clmPossuiQuestoesSemSub)
                clm.Status = EFiltroMontaProvaStatus.Desabilitado;
            else
                clm.Status = GetStatusItemPai(subEspecCLM);

            filtro.Ativo = 1;
            filtro.SubTotalQuestoes = CalcularTotalQuestoes(filtroPost);
            filtro.Modulo = EModuloFiltro.Especialidades;
            filtro.TotalQuestoes = filtro.SubTotalQuestoes;

            filtro.Especialidades = filtro.Especialidades.OrderBy(x => x.Ordem).ToList();
            //SubEspecialidades

            if (filtroPost.Especialidades.Length > 0)
            {
                filtro.Selecao = GetNomesSelecionados(filtro.Especialidades);
                filtro.Selecionado = 1;
            }
            else
            {
                filtro.Selecao = "Todas Selecionadas";
                filtro.Selecionado = 0;
            }

            filtro.Especialidades.Where(x => x.Id == 32).FirstOrDefault().Multidisciplinares = null;

            return filtro;
        }

        private MontaProvaModuloFiltro GetAnosFiltrado(List<BsonDocument> ultimosAnos, MontaProvaFiltroPost filtroPost, bool home)
        {
            var filtro = new MontaProvaModuloFiltro();
            filtro.UltimosAnos = new List<MontaProvaModuloFiltroItem>();

            foreach (var item in ultimosAnos)
            {
                var itemUltimosAnos = new MontaProvaModuloFiltroItem();
                itemUltimosAnos.Id = item["_id"]["QuestaoAno"].IsBsonNull ? 0 : item["_id"]["QuestaoAno"].AsInt32;
                itemUltimosAnos.QuantidadeQuestoes = item["dcount"].IsBsonNull ? 0 : item["dcount"].AsInt32;
                itemUltimosAnos.Status = filtroPost.Anos.Contains(itemUltimosAnos.Id) ? EFiltroMontaProvaStatus.Habilitado : EFiltroMontaProvaStatus.Desabilitado;
                itemUltimosAnos.UltimosAnos = CalcularAno(itemUltimosAnos.Id);
                filtro.UltimosAnos.Add(itemUltimosAnos);

            }
            filtro.UltimosAnos = filtro.UltimosAnos.OrderBy(x => x.Id).ToList();

            var acc = 0;
            for (int i = filtro.UltimosAnos.Count - 1; i >= 0; i--)
            {
                acc += filtro.UltimosAnos[i].QuantidadeQuestoes;
                filtro.UltimosAnos[i].QuantidadeQuestoes = acc;
            }


            if (filtroPost.Anos.Length > 0)
            {
                int anoLimite = CalcularAno(filtroPost.Anos[0]);
                filtro.SubTotalQuestoes = filtro.UltimosAnos.FirstOrDefault(x => x.Id == anoLimite).QuantidadeQuestoes;


                filtro.Selecao = filtroPost.Anos[0] > 1
                    ? string.Format("Últimos {0} anos", filtroPost.Anos[0])
                    : string.Format("Último {0} ano", filtroPost.Anos[0]);

                filtro.Selecionado = 1;
                filtro.TotalQuestoes = filtro.SubTotalQuestoes;
            }
            else
            {
                filtro.SubTotalQuestoes = filtro.UltimosAnos.Max(x => x.QuantidadeQuestoes);
                filtro.Selecao = "Todos os Anos";
                filtro.Selecionado = 0;
            }

            filtro.Ativo = 1;
            filtro.Modulo = EModuloFiltro.UltimosAnos;

            return filtro;
        }

        private MontaProvaModuloFiltro GetFiltrosEspeciaisFiltrado(List<BsonDocument> erradas, MontaProvaFiltroPost filtroPost, bool home)
        {
            var filtro = new MontaProvaModuloFiltro();
            filtro.FiltrosEspeciais = new List<MontaProvaModuloFiltroItem>();
            filtro.Modulo = EModuloFiltro.FiltrosEspeciais;

            foreach (var item in erradas)
            {
                var itemEspeciais = new MontaProvaModuloFiltroItem();
                itemEspeciais.Id = item["_id"]["FiltroEspecialId"].IsBsonNull ? 0 : Convert.ToInt32(item["_id"]["FiltroEspecialId"].AsString);
                itemEspeciais.QuantidadeQuestoes = item["dcount"].IsBsonNull ? 0 : item["dcount"].AsInt32;
                itemEspeciais.Status = filtroPost.FiltrosEspeciais.Contains(itemEspeciais.Id) ? EFiltroMontaProvaStatus.Completo : EFiltroMontaProvaStatus.Desabilitado;
                itemEspeciais.Titulo = item["_id"]["FiltroEspecial"].IsBsonNull ? "N/A" : item["_id"]["FiltroEspecial"].AsString;
                itemEspeciais.Originais = GetTotalFiltro(filtroPost, itemEspeciais, EFiltroEspecialTipo.Originais);
                itemEspeciais.Impressas = GetTotalFiltro(filtroPost, itemEspeciais, EFiltroEspecialTipo.Impressas);
                itemEspeciais.Erradas = GetTotalFiltro(filtroPost, itemEspeciais, EFiltroEspecialTipo.Erradas);
                filtro.FiltrosEspeciais.Add(itemEspeciais);
            }

            filtro.TodosEspeciais = GetTotalFiltro(filtroPost, null, EFiltroEspecialTipo.Todos);

            filtro.Ativo = 1;

            if (filtroPost.FiltrosEspeciais.Length > 0 && filtroPost.FiltrosEspeciais.Length < 2)
            {

                if (filtroPost.FiltrosEspeciais.Length == 1 && filtroPost.FiltrosEspeciais.Contains(1))
                {
                    filtro.Selecao = "Somente as que Eu Errei";
                }
                else if (filtroPost.FiltrosEspeciais.Length == 1 && filtroPost.FiltrosEspeciais.Contains(3))
                {
                    filtro.Selecao = "Somente Impressas";
                }
                else
                {
                    filtro.Selecao = "Somente questões Originais";
                }

                filtro.Selecionado = 1;
                filtro.SubTotalQuestoes = CalcularTotalQuestoes(filtroPost);
            }
            else
            {
                filtro.SubTotalQuestoes = filtro.FiltrosEspeciais.Sum(x => x.QuantidadeQuestoes);
                filtro.Selecao = "Todas Selecionadas";
                filtro.Selecionado = filtroPost.FiltrosEspeciais.Length > 0 ? 1 : 0;
            }

            if (filtroPost.Concursos.Any() || filtroPost.TodosConcursos)
            {
                filtro.FiltrosEspeciais.FirstOrDefault(x => x.Id == (int)EFiltroEspecialTipo.Originais).Status = EFiltroMontaProvaStatus.Inativo;
            }


            return filtro;

        }

        private List<BsonDocument> GetFiltroEspcialSomenteImpressas(MontaProvaFiltroPost filtro)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            AggregateArgs pipeline = new AggregateArgs();
            var group = new BsonDocument();
            var group2 = new BsonDocument();
            var unwind = new BsonDocument { { "$unwind", "$Especialidades" } };
            var especFiltrada = filtro.Especialidades.Any();

            string jsonCond = "{ '$sum' : { '$cond': [{ '$gte': [ '$count', 1]}, 1, 0 ]} }";
            var distCond = new BsonDocument(BsonDocument.Parse(jsonCond));

            var cond = GetCriteria(filtro, EFiltroEspecialTipo.Impressas);
            var match = GetMatchPermitidos(filtro, true);

            group = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroEspecialId", "3" }, {"FiltroEspecial", "Somente <b>Impressas</b>" } ,  {"QuestaoId" , "$_id" } } },
                                                                        { "count", new BsonDocument("$sum", cond) } } } };

            group2 = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroEspecialId", "$_id.FiltroEspecialId" }, { "FiltroEspecial", "$_id.FiltroEspecial" } } },
                                                                        { "dcount", new BsonDocument("$sum", distCond) } } } };


            pipeline.Pipeline = especFiltrada ? new[] { match, unwind, group, group2 } : new[] { match, group, group2 };


            var result = questaoRep.Collection.Aggregate(pipeline).ToList();


            return result;
        }

        private List<BsonDocument> GetFiltroEspecialSomenteAsQueEuErrei(MontaProvaFiltroPost filtro)
        {

            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            AggregateArgs pipeline = new AggregateArgs();
            var group = new BsonDocument();
            var group2 = new BsonDocument();
            var unwind = new BsonDocument { { "$unwind", "$Especialidades" } };
            var especFiltrada = filtro.Especialidades.Any();

            string jsonCond = "{ '$sum' : { '$cond': [{ '$gte': [ '$count', 1]}, 1, 0 ]} }";
            var distCond = new BsonDocument(BsonDocument.Parse(jsonCond));

            BsonDocument match;

            if (!filtro.FiltrosEspeciais.Any())
            {
                filtro.FiltrosEspeciais = new int[1] { 1 };
                match = GetMatchCriteria(filtro);
                filtro.FiltrosEspeciais = new int[0];
            }
            else
                match = GetMatchCriteria(filtro);

            var cond = GetCriteria(filtro, EFiltroEspecialTipo.Erradas);

            group = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroEspecialId", "1" }, {"FiltroEspecial", "Somente as que <b>Eu Errei</b>" } ,  {"QuestaoId" , "$QuestaoId" } } },
                                                                        { "count", new BsonDocument("$sum", cond) } } } };


            group2 = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroEspecialId", "$_id.FiltroEspecialId" }, { "FiltroEspecial", "$_id.FiltroEspecial" } } },
                                                                        { "count" , new BsonDocument("$sum", 1)  },
                                                                        { "dcount", new BsonDocument("$sum", distCond) } } } };


            pipeline.Pipeline = especFiltrada ? new[] { match, unwind, group, group2 } : new[] { match, group, group2 };


            var result = questaoRep.Collection.Aggregate(pipeline).ToList();


            if (!result.Any())
                result.Add(new BsonDocument { { "_id", new BsonDocument
                                           { { "FiltroEspecialId", "1" }, { "FiltroEspecial", "Somente as que <b>Eu Errei</b>" } } },
                                                                        { "count", 0}, {"dcount", 0} });



            return result;
        }

        private List<BsonDocument> GetFiltroEspecialSomenteOriginais(MontaProvaFiltroPost filtro)
        {

            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            AggregateArgs pipeline = new AggregateArgs();
            var group = new BsonDocument();
            var group2 = new BsonDocument();
            var unwind = new BsonDocument { { "$unwind", "$Especialidades" } };
            var especFiltrada = filtro.Especialidades.Any();

            var cond = GetCriteria(filtro, EFiltroEspecialTipo.Originais);
            var match = GetMatchPermitidos(filtro);



            group = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroEspecialId", "2" }, {"FiltroEspecial", "Questões <b>Originais</b>" } ,  {"QuestaoId" , "$QuestaoId" } } },
                                                                        { "count", new BsonDocument("$sum", cond) } } } };


            group2 = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroEspecialId", "$_id.FiltroEspecialId" }, { "FiltroEspecial", "$_id.FiltroEspecial" } } },
                                                                        { "count" , new BsonDocument("$sum", 1)  },
                                                                        { "dcount", new BsonDocument("$sum", "$count") } } } };


            pipeline.Pipeline = especFiltrada ? new[] { match, unwind, group, group2 } : new[] { match, group, group2 };


            var result = questaoRep.Collection.Aggregate(pipeline).ToList();


            return result;
        }

        private int[] FiltrarTexto(string filtro)
        {

            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();
            AggregateArgs pipeline = new AggregateArgs();
            List<int> ids = new List<int>();

            string filter = filtro.Replace('"', '\"');
            filter = filter.Replace('+', ' ');


            string json = "{ '$match': { '$text': { '$search': '" + filter + "'}}}";


            var matchStage = BsonDocument.Parse(json);
            var projectStage = BsonDocument.Parse("{ '$project': { 'Id': '$Id' }}");

            pipeline.Pipeline = new[] { matchStage, projectStage };
            var result = questaoRep.Collection.Aggregate(pipeline).ToList();

            foreach (var item in result)
            {
                ids.Add(item["QuestaoId"].AsInt32);
            }

            return ids.ToArray();

        }

        private string[] StopWords = { "de", "a", "o", "que", "e", "do", "da", "em", "um", "para", "é", "com", "não", "uma", "os", "no", "se", "na", "por", "mais", "as", "dos", "como", "mas", "foi", "ao", "ele", "das", "tem", "à", "seu", "sua", "ou", "ser", "quando", "muito", "há", "nos", "já", "está", "eu", "também", "só", "pelo", "pela", "até", "isso", "ela", "entre", "era", "depois", "sem", "mesmo", "aos", "ter", "seus", "quem", "nas", "me", "esse", "eles", "estão", "você", "tinha", "foram", "essa", "num", "nem", "suas", "meu", "às", "minha", "têm", "numa", "pelos", "elas", "havia", "seja", "qual", "será", "nós", "tenho", "lhe", "deles", "essas", "esses", "pelas", "este", "fosse", "dele", "tu", "te", "vocês", "vos", "lhes", "meus", "minhas", "teu", "tua", "teus", "tuas", "nosso", "nossa", "nossos", "nossas", "dela", "delas", "esta", "estes", "estas", "aquele", "aquela", "aqueles", "aquelas", "isto", "aquilo", "estou", "está", "estamos", "estão", "estive", "esteve", "estivemos", "estiveram", "estava", "estávamos", "estavam", "estivera", "estivéramos", "esteja", "estejamos", "estejam", "estivesse", "estivéssemos", "estivessem", "estiver", "estivermos", "estiverem", "hei", "há", "havemos", "hão", "houve", "houvemos", "houveram", "houvera", "houvéramos", "haja", "hajamos", "hajam", "houvesse", "houvéssemos", "houvessem", "houver", "houvermos", "houverem", "houverei", "houverá", "houveremos", "houverão", "houveria", "houveríamos", "houveriam", "sou", "somos", "são", "era", "éramos", "eram", "fui", "foi", "fomos", "foram", "fora", "fôramos", "seja", "sejamos", "sejam", "fosse", "fôssemos", "fossem", "for", "formos", "forem", "serei", "será", "seremos", "serão", "seria", "seríamos", "seriam", "tenho", "tem", "temos", "tém", "tinha", "tínhamos", "tinham", "tive", "teve", "tivemos", "tiveram", "tivera", "tivéramos", "tenha", "tenhamos", "tenham", "tivesse", "tivéssemos", "tivessem", "tiver", "tivermos", "tiverem", "terei", "terá", "teremos", "terão", "teria", "teríamos", "teriam" };

        private ObjectId[] FiltroTexto(string filtro)
        {

            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();
            AggregateArgs pipeline = new AggregateArgs();
            List<ObjectId> ids = new List<ObjectId>();
            string pattern = @"\s+(?=([^""]*""[^""]*"")*[^""]*$)";
            string filter = Regex.Replace(filtro, pattern, "#####");
            filter = filter.Replace("+", "#####");
            string[] removeStopWords = filter.ToLowerInvariant().Split(new string[] { "#####" }, StringSplitOptions.RemoveEmptyEntries);
            string nfilter = "";
            for (int it = 0; it < removeStopWords.Length; it++)
            {
                if (!StopWords.Contains(removeStopWords[it].Replace("\"", String.Empty)))
                {
                    nfilter += " " + removeStopWords[it];
                }
            }
            filter = filtro.Replace('"', '\"');
            filter = nfilter.TrimStart(' ');

            string json = "{ '$match': { '$text': { '$search': '" + filter + "'}}}";
            //string json = "{ '$match' : { Enunciado:/" + filter + "/i} }";
            //string json = "{ '$match' : { '$or': [ { Enunciado:/" + filtro + "/i}, { 'Alternativas.Descricao':/" +
            //  filtro + "/i }] } }";
            var matchStage = BsonDocument.Parse(json);
            var projectStage = BsonDocument.Parse("{ '$project': { 'Id': '$Id' }}");

            pipeline.Pipeline = new[] { matchStage, projectStage };
            var result = questaoRep.Collection.Aggregate(pipeline).ToList();

            foreach (var item in result)
            {
                var id = item["_id"].AsObjectId;
                ids.Add(id);
            }

            return ids.ToArray();
        }


        private List<HistoricoQuestaoErradaAluno> FiltroHistorico(int matricula)
        {
            IRepository<Entidades.MongoDbCollections.HistoricoQuestaoErradaAluno> questaoRep = new MongoRepository<Entidades.MongoDbCollections.HistoricoQuestaoErradaAluno>();
            return questaoRep.Where(y => y.Matricula == matricula).ToList();
        }

        private List<HistoricoQuestaoErradaAluno> FiltroHistorico(MontaProvaFiltroPost filtro)
        {

            if (filtro.HistoricoQuestaoErradaAluno != null)
            {
                return (from f in filtro.HistoricoQuestaoErradaAluno
                        from i in f.Ids
                        select new HistoricoQuestaoErradaAluno { OrigemQuestao = f.Tipo, QuestaoId = i }
                       ).ToList();

            }
            else
            {
                return FiltroHistorico(filtro.Matricula);
            }

        }



        private List<ExercicioPermissaoAluno> GetExerciciosPermitidos(MontaProvaFiltroPost filtro)
        {
            if (filtro.ExercicioPermissaoAluno != null)
            {
                return (from f in filtro.ExercicioPermissaoAluno
                        from i in f.Ids
                        select new ExercicioPermissaoAluno { TipoExercicioId = f.Tipo, ExercicioId = i }
                           ).ToList();

            }
            else
            {
                return GetExerciciosPermitidos(filtro.Matricula);
            }
        }




        private MontaProvaModuloFiltro GerarModuloSemFiltro(EModuloFiltro tipo, bool selecionado = false)
        {
            var filtro = new MontaProvaModuloFiltro();
            filtro.Ativo = 1;
            filtro.SubTotalQuestoes = 0;
            filtro.Modulo = tipo;
            filtro.Selecao = "Todas Selecionadas";
            filtro.Selecionado = selecionado ? 1 : 0;

            return filtro;
        }

        #endregion

        #region Aggregates

        private BsonDocument GetCriteria(MontaProvaFiltroPost filtro, EFiltroEspecialTipo filtroEspecial = EFiltroEspecialTipo.Nenhum)
        {
            var matchArray = new BsonArray();

            if (!string.IsNullOrEmpty(filtro.FiltroTexto))
            {
                var arrIds = FiltroTexto(filtro.FiltroTexto);
                if (arrIds.Length > 0)
                {
                    string[] jObjectId = new string[arrIds.Length];
                    for (int i = 0; i < arrIds.Length; i++)
                    {
                        jObjectId[i] = string.Format("ObjectId('{0}')", arrIds[i]);
                    }

                    var arrFiltro = string.Join(",", jObjectId);
                    var json = "{ '$in' : ['$_id', [" + arrFiltro + "]]}";
                    matchArray.Add(BsonDocument.Parse(json));
                }
            }

            if (filtro.Especialidades != null && filtro.Especialidades.Length > 0 && !filtro.MultiDisciplinar && filtro.FiltroModulo != EModuloFiltro.Especialidades)
            {
                var arrEspecialidades = string.Join(",", filtro.Especialidades);
                var json = "{ '$in' : ['$Especialidades.EspecialidadeId', [" + arrEspecialidades + "]]}";
                matchArray.Add(BsonDocument.Parse(json));
            }
            if (filtro.Concursos != null && filtro.Concursos.Length > 0 && filtro.FiltroModulo != EModuloFiltro.Concursos)
            {
                var arrConcursos = string.Join(",", filtro.Concursos);
                var json = "{ '$in' : ['$ConcursoEntidadeId', [" + arrConcursos + "]]}";

                matchArray.Add(BsonDocument.Parse(json));
            }
            if (filtro.Anos != null && filtro.Anos.Length > 0 && filtro.FiltroModulo != EModuloFiltro.UltimosAnos)
            {
                var ano = CalcularAno(filtro.Anos[0]);
                var json = "{ '$gte' : ['$QuestaoAno', " + ano + "]}";
                matchArray.Add(BsonDocument.Parse(json));
            }
            if ((filtro.FiltrosEspeciais != null && filtro.FiltrosEspeciais.Length > 0) || filtro.FiltroModulo == EModuloFiltro.FiltrosEspeciais)
            {
                if (filtro.FiltrosEspeciais.Contains((int)EFiltroEspecialTipo.Erradas) || (filtro.FiltroModulo == EModuloFiltro.FiltrosEspeciais && filtroEspecial == EFiltroEspecialTipo.Erradas))
                {
                    var historico = FiltroHistorico(filtro);

                    var arrQuestoesConcurso = string.Join(",", historico.Where(x => x.OrigemQuestao == 2).Select(x => x.QuestaoId).ToArray());
                    var arrQuestoesSimulado = string.Join(",", historico.Where(x => x.OrigemQuestao == 1).Select(x => x.QuestaoId).ToArray());

                    var json = "{'$or' : [{'$and' : [{'$in' : [ '$QuestaoId' , [" + arrQuestoesConcurso + "]] } , {'OrigemQuestao' : 2} ]}," +
                                         "{'$and' : [{'$in' : [ '$QuestaoId' , [" + arrQuestoesSimulado + "]] } , {'OrigemQuestao' : 1} ]}" +
                                         "]" +
                                "}";

                    matchArray.Add(BsonDocument.Parse(json));
                }
                if (filtro.FiltrosEspeciais.Contains((int)EFiltroEspecialTipo.Originais) || (filtro.FiltroModulo == EModuloFiltro.FiltrosEspeciais && filtroEspecial == EFiltroEspecialTipo.Originais))
                {
                    var json = "{ '$in' : ['$OrigemQuestao', [1]]}";
                    matchArray.Add(BsonDocument.Parse(json));
                }
                if (filtro.FiltrosEspeciais.Contains((int)EFiltroEspecialTipo.Impressas) || (filtro.FiltroModulo == EModuloFiltro.FiltrosEspeciais && filtroEspecial == EFiltroEspecialTipo.Impressas))
                {
                    var json = "{ 'Impressa': true }";
                    matchArray.Add(BsonDocument.Parse(json));
                }

            }

            // alunos que não são R3 não podem ver mais concursos R3
            PerfilAlunoEntity PAE = new PerfilAlunoEntity();
            ConcursoEntity CE = new ConcursoEntity();
            if (filtro.ExerciciosPermitidos.Any())
            {
                int[] cR3 = new int[0];
                if (!PAE.IsAlunoR3(filtro.Matricula))
                {
                    cR3 = CE.GetConcursosR3(filtro.Matricula);
                }
                var arrFiltro = filtro.ExerciciosPermitidos.Where(p => !cR3.Contains(p.ExercicioId)).Select(x => x.ExercicioId).ToArray();
                var arrayPermitidos = string.Join(",", arrFiltro);
                var json = "{ '$in' : ['$ProvaId', [" + arrayPermitidos + "]]}";
                matchArray.Add(BsonDocument.Parse(json));
            }
            else
            {
                if (!PAE.IsAlunoR3(filtro.Matricula))
                {
                    int[] cR3 = CE.GetConcursosR3(filtro.Matricula);
                    var arrConcursos = string.Join(",", cR3);
                    var json = "{ '$not' : {'$in' : ['$ProvaId', [" + arrConcursos + "]]}}";
                    matchArray.Add(BsonDocument.Parse(json));
                }

            }

            if (filtro.MultiDisciplinar)
            {
                matchArray.Add(BsonDocument.Parse("{ '$eq' : ['$Especialidades.EspecialidadeId', " + filtro.EspecialidadeId + "]}"));
                matchArray.Add(BsonDocument.Parse("{ '$Multidisciplinar': true }"));
            }

            if (filtro.FiltroModulo != EModuloFiltro.Concursos && filtro.TodosConcursos)
            {
                var json = "{ '$in' : ['$OrigemQuestao', [2]]}";
                matchArray.Add(BsonDocument.Parse(json));
            }

            if (filtro.ExercicioJaExistenteProva != null && filtro.ExercicioJaExistenteProva.Any())
            {
                var arrFiltro = filtro.ExercicioJaExistenteProva.Select(x => x.Id).ToArray();
                var arrayJaExistentes = string.Join(",", arrFiltro);
                var json = "{ '$not' : { '$in' : ['$QuestaoId', [" + arrayJaExistentes + "]]}}";
                matchArray.Add(BsonDocument.Parse(json));
            }


            var cond = new BsonDocument { { "$cond", new BsonArray { new BsonDocument { { "$and", matchArray } }, 1, 0 } } };

            return cond;

        }

        private BsonDocument GetMatchPermitidos(MontaProvaFiltroPost filtro, bool impressa = false)
        {
            var elements = new Dictionary<string, object>();
            if (filtro.ExerciciosPermitidos.Any())
            {
                var arrFiltro = filtro.ExerciciosPermitidos.Select(x => x.ExercicioId).ToArray();
                var arrayPermitidos = new BsonArray();
                arrayPermitidos.AddRange(arrFiltro);
                elements.Add("ProvaId", new BsonDocument { { "$in", arrayPermitidos } });
            }

            if (impressa)
                elements.Add("Impressa", true);

            var match = new BsonDocument { { "$match", new BsonDocument { elements } } };
            return match;
        }

        private BsonDocument GetMatchCriteria(MontaProvaFiltroPost filtro, List<int> notInList = null)
        {
            var elements = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(filtro.FiltroTexto))
            {
                if (!string.IsNullOrEmpty(filtro.FiltroTexto))
                {
                    var arrayIds = new BsonArray();
                    arrayIds.AddRange(FiltroTexto(filtro.FiltroTexto));
                    if (arrayIds.Count > 0)
                    {
                        elements.Add("_id", new BsonDocument { { "$in", arrayIds } });
                    }
                }
            }

            if (filtro.Especialidades != null && filtro.Especialidades.Length > 0 && !filtro.MultiDisciplinar)
            {
                var arrayEspecialidades = new BsonArray();
                arrayEspecialidades.AddRange(filtro.Especialidades);
                elements.Add("Especialidades.EspecialidadeId", new BsonDocument { { "$in", arrayEspecialidades } });
            }

            if (filtro.Concursos != null && filtro.Concursos.Length > 0)
            {
                var arrayConcursos = new BsonArray();
                arrayConcursos.AddRange(filtro.Concursos);
                elements.Add("ConcursoEntidadeId", new BsonDocument { { "$in", arrayConcursos } });
            }

            if (filtro.Anos != null && filtro.Anos.Length > 0)
            {
                var anoLimite = CalcularAno(filtro.Anos[0]);
                elements.Add("QuestaoAno", new BsonDocument { { "$gte", anoLimite } });
            }

            if (filtro.FiltrosEspeciais != null && filtro.FiltrosEspeciais.Length > 0)
            {
                if (filtro.FiltrosEspeciais.Contains((int)EFiltroEspecialTipo.Erradas))
                {
                    var historico = FiltroHistorico(filtro);
                    var arr = new BsonArray();

                    var arrQuestoesConcurso = string.Join(",", historico.Where(x => x.OrigemQuestao == 2).Select(x => x.QuestaoId).ToArray());
                    var arrQuestoesSimulado = string.Join(",", historico.Where(x => x.OrigemQuestao == 1).Select(x => x.QuestaoId).ToArray());

                    var json = "[{'$and' : [{'QuestaoId' : { '$in' : [" + arrQuestoesConcurso + "]} } , {'OrigemQuestao' : 2} ]}," +
                                "{'$and' : [{'QuestaoId': { '$in' : [" + arrQuestoesSimulado + "]} } , {'OrigemQuestao' : 1} ]}" +
                                         "]";

                    var doc = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonArray>(json);

                    elements.Add("$or", doc);

                }
                if (filtro.FiltrosEspeciais.Contains((int)EFiltroEspecialTipo.Originais))
                {
                    var arrayOriginais = new BsonArray();
                    arrayOriginais.AddRange(new int[] { 1 });
                    elements.Add("OrigemQuestao", new BsonDocument { { "$in", arrayOriginais } });
                }
                if (filtro.FiltrosEspeciais.Contains((int)EFiltroEspecialTipo.Impressas))
                {
                    elements.Add("Impressa", true);
                }
            }

            // alunos que não são R3 não podem ver mais concursos R3
            PerfilAlunoEntity PAE = new PerfilAlunoEntity();
            ConcursoEntity CE = new ConcursoEntity();
            var concursosR3 = new BsonArray();
            if (filtro.ExerciciosPermitidos.Any())
            {
                int[] cR3 = new int[0];
                if (!PAE.IsAlunoR3(filtro.Matricula))
                {
                    cR3 = CE.GetConcursosR3(filtro.Matricula);
                }
                var arrFiltro = filtro.ExerciciosPermitidos.Where(p => !cR3.Contains(p.ExercicioId)).Select(x => x.ExercicioId).ToArray();
                var arrayPermitidos = new BsonArray();
                arrayPermitidos.AddRange(arrFiltro);

                elements.Add("ProvaId", new BsonDocument { { "$in", arrayPermitidos } });
            }
            else
            {
                if (!PAE.IsAlunoR3(filtro.Matricula))
                {
                    int[] cR3 = CE.GetConcursosR3(filtro.Matricula);
                    concursosR3.AddRange(cR3);
                    elements.Add("ProvaId", new BsonDocument { { "$nin", concursosR3 } });
                }
            }

            if (filtro.MultiDisciplinar)
            {
                elements.Add("Especialidades.EspecialidadeId", new BsonDocument { { "$eq", filtro.EspecialidadeId } });
                elements.Add("Multidisciplinar", new BsonDocument { { "$eq", true } });
            }
            if (filtro.TodosConcursos)
            {
                if (!filtro.FiltrosEspeciais.Contains((int)EFiltroEspecialTipo.Originais))
                {
                    var arrayOriginais = new BsonArray();
                    arrayOriginais.AddRange(new int[] { 2 });
                    elements.Add("OrigemQuestao", new BsonDocument { { "$in", arrayOriginais } });
                }

            }


            if (filtro.ExercicioJaExistenteProva != null && filtro.ExercicioJaExistenteProva.Any())
            {
                var arrFiltro = filtro.ExercicioJaExistenteProva.Select(x => x.Id).ToArray();
                var arrayExerciciosExistentesProva = new BsonArray();
                arrayExerciciosExistentesProva.AddRange(arrFiltro);
                elements.Add("QuestaoId", new BsonDocument { { "$nin", arrayExerciciosExistentesProva } });
            }



            var match = new BsonDocument { { "$match", new BsonDocument { elements } } };

            return match;
        }

        /// <summary>
        /// Criação de Pipeline de Agrupamento
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        private AggregateArgs GetPipelineGroup(MontaProvaFiltroPost filtro)
        {

            AggregateArgs pipeline = new AggregateArgs();
            var group = new BsonDocument();
            var group2 = new BsonDocument();
            var unwind = new BsonDocument { { "$unwind", "$Especialidades" } };
            var especFiltrada = filtro.Especialidades.Any();

            var cond = GetCriteria(filtro);
            var match = GetMatchPermitidos(filtro);

            string jsonCond = "{ '$sum' : { '$cond': [{ '$gte': [ '$count', 1]}, 1, 0 ]} }";
            var distCond = new BsonDocument(BsonDocument.Parse(jsonCond));



            switch (filtro.FiltroModulo)
            {
                case EModuloFiltro.Home:
                    group = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroTextId", "1" }, {"QuestaoId" , "$QuestaoId" } } },
                                                                        { "count", new BsonDocument("$sum", cond) } } } };


                    group2 = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroTextId", "$_id.FiltroTextId" }} },
                                                                        { "count" , new BsonDocument("$sum", 1)  },
                                                                        { "dcount", new BsonDocument("$sum", distCond) } } } };


                    pipeline.Pipeline = especFiltrada ? new[] { match, unwind, group, group2 } : new[] { match, group, group2 };

                    break;
                case EModuloFiltro.Especialidades:
                    group = new BsonDocument {{ "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "EspecialidadeId", "$Especialidades.EspecialidadeId" }, { "EspecialidadeSigla", "$Especialidades.EspecialidadeSigla" }, { "EspecialidadeNome", "$Especialidades.EspecialidadeNome" }, { "EspecialidadeParentId", "$Especialidades.EspecialidadeParentId" } } },
                                                                        { "count", new BsonDocument("$sum", cond) } } } };
                    pipeline.Pipeline = new[] { match, unwind, group };


                    break;
                case EModuloFiltro.Concursos:
                    group = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "ConcursoEntidadeId", "$ConcursoEntidadeId" }, { "ConcursoNome", "$ConcursoNome" }, { "ConcursoSigla", "$ConcursoSigla" }, {"QuestaoId" , "$QuestaoId" } } },
                                                                        { "count", new BsonDocument("$sum", cond) } } } };

                    group2 = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "ConcursoEntidadeId", "$_id.ConcursoEntidadeId" }, { "ConcursoNome", "$_id.ConcursoNome" }, { "ConcursoSigla", "$_id.ConcursoSigla" } } },
                                                                        { "count" , new BsonDocument("$sum", 1)  },
                                                                        { "dcount", new BsonDocument("$sum", distCond) } } } };

                    pipeline.Pipeline = especFiltrada ? new[] { match, unwind, group, group2 } : new[] { match, group, group2 };

                    break;
                case EModuloFiltro.UltimosAnos:
                    group = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "QuestaoAno", "$QuestaoAno" } , {"QuestaoId" , "$QuestaoId" } } },
                                                                        { "count", new BsonDocument("$sum", cond) } } } };


                    group2 = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "QuestaoAno", "$_id.QuestaoAno" } } },
                                                                        { "count" , new BsonDocument("$sum", 1)  },
                                                                        { "dcount", new BsonDocument("$sum", distCond) } } } };


                    pipeline.Pipeline = especFiltrada ? new[] { match, unwind, group, group2 } : new[] { match, group, group2 };


                    break;
                case EModuloFiltro.FiltrosEspeciais:
                    group = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroEspecialId", "1" }, {"FiltroEspecial", "Somente as que eu Errei" } ,  {"QuestaoId" , "$QuestaoId" } } },
                                                                        { "count", new BsonDocument("$sum", cond) } } } };


                    group2 = new BsonDocument { { "$group", new BsonDocument { { "_id", new BsonDocument
                                                                        { { "FiltroEspecialId", "$_id.FiltroEspecialId" }, { "FiltroEspecial", "$_id.FiltroEspecial" } } },
                                                                        { "count" , new BsonDocument("$sum", 1)  },
                                                                        { "dcount", new BsonDocument("$sum", "$count") } } } };


                    pipeline.Pipeline = especFiltrada ? new[] { match, unwind, group, group2 } : new[] { match, group, group2 };

                    break;
                default:
                    break;

            }
            return pipeline;

        }

        /// <summary>
        /// Criação de Pipeline de Matching
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        private AggregateArgs GetPipelineMatch(MontaProvaFiltroPost filtro, int? limit = null)
        {

            AggregateArgs pipeline = new AggregateArgs();

            var match = GetMatchCriteria(filtro);

            var project = BsonDocument.Parse("{ '$project': { 'QuestaoId': '$QuestaoId' , 'OrigemQuestao' : '$OrigemQuestao', 'ConcursoId' : '$ConcursoId', 'Especialidades' : '$Especialidades'  }}");


            if (limit.HasValue)
            {
                var sampleAgg = BsonDocument.Parse("{ '$sample': { size : " + limit.Value + " } }");
                pipeline.Pipeline = new[] { match, project, sampleAgg };
            }
            else
            {
                pipeline.Pipeline = new[] { match, project };
            }

            return pipeline;
        }

        private AggregateArgs GetPipelineMatchCounter(MontaProvaFiltroPost filtro, int? limit = null)
        {

            AggregateArgs pipeline = new AggregateArgs();

            var match = GetMatchCriteria(filtro);



            //var project = BsonDocument.Parse("{ '$project': { 'QuestaoId': '$QuestaoId' , 'OrigemQuestao' : '$OrigemQuestao'  }}");

            var counter = BsonDocument.Parse("{ '$count': 'pipelinecounter'}");


            pipeline.Pipeline = new[] { match, counter };

            return pipeline;
        }


        #endregion

        #region Calculos

        private int CalcularQuantidadeQuestoesDistintas(List<MontaProvaModuloFiltroItem> especs)
        {
            //array completo
            var ac = especs.SelectMany(x => x.Multidisciplinares).ToArray();

            //array distinto
            var ad = ac.Distinct().ToArray();

            //quantidade distinta = total - (array completo lenght - array distinto lenght)
            var qtde = especs.Sum(x => x.QuantidadeQuestoes) - (ac.Length - ad.Length);

            return qtde;

        }

        private int CalcularTotalQuestoes(MontaProvaFiltroPost filtro)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();
            IRepository<Entidades.MongoDbCollections.Especialidade> espRep = new MongoRepository<Entidades.MongoDbCollections.Especialidade>();

            if (!filtro.Especialidades.Any())
                filtro.Especialidades = espRep.Select(x => (int)x.EspecialidadeId).ToArray(); //.Where(x => x.EspecialidadeId != 32)

            //var pipeline = GetPipelineMatch(filtro);
            //var result = questaoRep.Collection.Aggregate(pipeline).ToList();
            //List<Entidades.Questao> ids = result.Select(item => new Entidades.Questao { Id = item["QuestaoId"].AsInt32, ExercicioTipoID = item["OrigemQuestao"].AsInt32 }).ToList();
            //return ids.Count();

            var pipeline2 = GetPipelineMatchCounter(filtro);
            var result2 = questaoRep.Collection.Aggregate(pipeline2).ToList();
            int counter;
            if (result2.Any())
            {
                counter = result2.FirstOrDefault()["pipelinecounter"].AsInt32;
            }
            else
            {
                counter = result2.Count();
            }
            return counter;
        }

        private int CalcularQuantidadeQuestoesOriginais(MontaProvaFiltroPost filtro)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            var especiais = filtro.FiltrosEspeciais;
            var todosConcursos = filtro.TodosConcursos;
            filtro.TodosConcursos = false;
            if (filtro.FiltrosEspeciais.Any() && !filtro.FiltrosEspeciais.Contains(2))
            {
                filtro.FiltrosEspeciais = new int[] { 1, 2 };

            }
            else
            {
                filtro.FiltrosEspeciais = new int[] { 2 };

            }

            var pipeline = GetPipelineMatch(filtro);
            var result = questaoRep.Collection.Aggregate(pipeline).ToList();

            filtro.FiltrosEspeciais = especiais;
            filtro.TodosConcursos = todosConcursos;


            return result.Count();
        }

        public int GetTotalFiltro(MontaProvaFiltroPost filtro, MontaProvaModuloFiltroItem itemFiltroEspecial, EFiltroEspecialTipo tipoFiltro)
        {
            if (itemFiltroEspecial != null && itemFiltroEspecial.Id == (int)tipoFiltro)
                return itemFiltroEspecial.QuantidadeQuestoes;

            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            var especiais = filtro.FiltrosEspeciais;

            if (tipoFiltro == EFiltroEspecialTipo.Todos)
            {
                filtro.FiltrosEspeciais = new int[] { (int)EFiltroEspecialTipo.Erradas, (int)EFiltroEspecialTipo.Impressas, (int)EFiltroEspecialTipo.Originais };
            }
            else
            {
                filtro.FiltrosEspeciais = new int[] { itemFiltroEspecial.Id, (int)tipoFiltro };
            }

            var pipeline = GetPipelineMatch(filtro);
            var result = questaoRep.Collection.Aggregate(pipeline).ToList();

            filtro.FiltrosEspeciais = especiais;

            return result.Count();
        }

        private EFiltroMontaProvaStatus GetStatusItemPai(List<MontaProvaModuloFiltroItem> especs)
        {
            var statusArray = especs.Select(x => x.Status).ToList();

            if (statusArray.TrueForAll(x => x == EFiltroMontaProvaStatus.Completo))
                return EFiltroMontaProvaStatus.Completo;
            if (statusArray.TrueForAll(x => x == EFiltroMontaProvaStatus.Inativo))
                return EFiltroMontaProvaStatus.Inativo;
            if (statusArray.Any(x => x == EFiltroMontaProvaStatus.Completo))
                return EFiltroMontaProvaStatus.Habilitado;
            else
                return EFiltroMontaProvaStatus.Desabilitado;

        }

        private string GetNomesSelecionados(IList<MontaProvaModuloFiltroItem> itens)
        {
            string str = string.Empty;

            var selecionados = itens.Where(x => x.Status == EFiltroMontaProvaStatus.Habilitado || x.Status == EFiltroMontaProvaStatus.Completo).ToList();
            var selecionadosSub = selecionados.Where(x => x.SubItens != null && x.SubItens.Any()).ToList().SelectMany(x => x.SubItens).ToList();

            if (selecionadosSub.Any())
                str = GetNomesSelecionados(selecionadosSub);

            var str2 = string.Join(", ", selecionados.Select(x => x.Titulo).ToArray());


            return !string.IsNullOrEmpty(str) ? (str2 + ", " + str) : str2;


        }

        private int CalcularAno(int ano)
        {
            return (DateTime.Now.Year - (ano - 1));
        }

        #endregion

        #region Setup

        private int[] GetMultidisciplinar(int EspecialidadeId, MontaProvaFiltroPost filtro)
        {
            try
            {
                IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

                filtro.MultiDisciplinar = true;
                filtro.EspecialidadeId = EspecialidadeId;

                AggregateArgs pipeline = new AggregateArgs();

                var match = GetMatchCriteria(filtro);

                filtro.MultiDisciplinar = false;
                filtro.EspecialidadeId = 0;

                var project = BsonDocument.Parse("{ '$project': { '_id': 0, 'QuestaoId': 1 }}");

                pipeline.Pipeline = new[] { match, project };
                var result = questaoRep.Collection.Aggregate(pipeline).ToList();
                List<int> ids = new List<int>();

                foreach (var item in result)
                {
                    var id = item["QuestaoId"].AsInt32;
                    if (id > 0) ids.Add(id);
                }


                return ids.ToArray();
            }
            catch
            {

                throw;
            }



        }

        private MontaProvaFiltroPost CheckHistoricoPermissao(MontaProvaFiltroPost filtro)
        {
            if (filtro.FiltroModulo == EModuloFiltro.Home
                && string.IsNullOrEmpty(filtro.FiltroTexto)
                && !filtro.Especialidades.Any()
                && !filtro.Concursos.Any()
                && !filtro.Anos.Any())
            {

                if (filtro.ExercicioPermissaoAluno != null && filtro.HistoricoQuestaoErradaAluno != null)
                {
                    var historico = GetHistoricoErradas(filtro.Matricula);
                    var permissao = GetExerciciosPermitidosAluno(filtro.Matricula);


                    filtro.HistoricoQuestaoErradaAluno = historico.GroupBy(x => x.OrigemQuestao).Select(y => new ExercicioQuestoesFiltroPost { Tipo = y.Key, Ids = y.Select(z => z.QuestaoId.Value).ToArray() }).ToList();
                    filtro.ExercicioPermissaoAluno = permissao.GroupBy(x => x.TipoExercicioId).Select(y => new ExercicioQuestoesFiltroPost { Tipo = y.Key, Ids = y.Select(z => z.ExercicioId).ToArray() }).ToList();

                }
                else //Legado
                {

                    SyncExerciciosPermitidos(filtro.Matricula);
                    SyncHistoricoExercicio(filtro.Matricula);
                }




            }


            return filtro;

        }


        public List<HistoricoQuestaoErradaAluno> GetHistoricoErradas(int matricula)
        {
            using (var ctx = new AcademicoContext())
            {
				var RespostasErradasSimuladoMatricula = (from cro in ctx.tblCartaoResposta_objetiva
												where cro.intClientID == matricula
												  && cro.intExercicioTipoId == 1
												select new
												{
													intQuestaoID = cro.intQuestaoID,
													txtLetraAlternativa = cro.txtLetraAlternativa
												}).ToList();

				var RespostasErradasSimulado = (from cro in RespostasErradasSimuladoMatricula
												join q in ctx.tblQuestoes
												  on cro.intQuestaoID equals q.intQuestaoID
												join qa in ctx.tblQuestaoAlternativas
												  on cro.intQuestaoID equals qa.intQuestaoID
												where qa.txtLetraAlternativa == cro.txtLetraAlternativa
												  && qa.bitCorreta == false
												  && !q.bitAnulada
												select new {
													QuestaoId = cro.intQuestaoID,
													OrigemQuestao = 1,
													Matricula = matricula
												}).ToList();

				using (var ctxMatDir = new DesenvContext())
                {
                    var QuestoesMarcadasErradasConcurso = (from cro in ctx.tblCartaoResposta_objetiva
                                                           where cro.intClientID == matricula
                                                             && cro.intExercicioTipoId == 2
														   select new
														   {
                                                                intQuestaoID = cro.intQuestaoID,
                                                                txtLetraAlternativa = cro.txtLetraAlternativa
														   }).ToList();

                    List<int> ConcursointQuestaoId = QuestoesMarcadasErradasConcurso.Select(x => x.intQuestaoID).ToList();

                    var QuestoesErradasConcurso = (from cq in ctxMatDir.tblConcursoQuestoes
                                                   join qa in ctxMatDir.tblConcursoQuestoes_Alternativas
                                                     on cq.intQuestaoID equals qa.intQuestaoID
                                                   where (cq.bitCasoClinico == null || cq.bitCasoClinico != "1")
                                                     && (qa.bitCorreta == false && qa.bitCorretaPreliminar == false)
                                                     && (ConcursointQuestaoId.Contains(cq.intQuestaoID))
                                                   select new
                                                   {
                                                       cq.intQuestaoID,
                                                       qa.txtLetraAlternativa
                                                   }).ToList();

                    var RespostasErradasConcurso = (from qmec in QuestoesMarcadasErradasConcurso
                                                    join qec in QuestoesErradasConcurso
                                                      on qmec.intQuestaoID equals qec.intQuestaoID
                                                    where qmec.txtLetraAlternativa == qec.txtLetraAlternativa
                                                    select new
                                                    {
                                                        QuestaoId = qec.intQuestaoID,
                                                        OrigemQuestao = 2,
                                                        Matricula = matricula
                                                    }).ToList();

                    return (from sim in RespostasErradasSimulado
                            select new HistoricoQuestaoErradaAluno
                            {
                                QuestaoId = sim.QuestaoId,
                                OrigemQuestao = 1,
                                Matricula = matricula
                            }).ToList().Union(
                            from conc in RespostasErradasConcurso
                            select new HistoricoQuestaoErradaAluno
                            {
                                QuestaoId = conc.QuestaoId,
                                OrigemQuestao = 2,
                                Matricula = matricula
                            }).ToList();
                }
            }
        }

        private List<ExercicioPermissaoAluno> GetExerciciosPermitidosAluno(int matricula)
        {

            int[] tipos = new int[] { 1, 2 };

            using (var ctx = new DesenvContext())
            {
                
                var exerciciosPermitidos = ctx.Set<msp_Medsoft_SelectPermissaoExercicios_Result>().FromSqlRaw("msp_Medsoft_SelectPermissaoExercicios @bitVisitanteExpirado = {0}, @bitAlunoVisitante = {1}, @intClientID = {2}", false, false, matricula).ToList()
                     .Where(c => tipos.ToList().Contains(c.intExercicioTipo.Value))
                     .Select(x => new ExercicioPermissaoAluno { Matricula = matricula, ExercicioId = x.intExercicioID.Value, TipoExercicioId = x.intExercicioTipo.Value })
                     .ToList();

                return exerciciosPermitidos;

            }
        }

        private List<ExercicioPermissaoAluno> GetExerciciosPermitidos(int matricula)
        {
            IRepository<Entidades.MongoDbCollections.ExercicioPermissaoAluno> permissaoRep = new MongoRepository<Entidades.MongoDbCollections.ExercicioPermissaoAluno>();

            var permitidos = permissaoRep.Where(x => x.Matricula == matricula).ToList();

            return permitidos;

        }

        private List<Entidades.MongoDbCollections.Especialidade> EspecialidadesOrdenadas()
        {

            IRepository<Entidades.MongoDbCollections.Especialidade> espRep = new MongoRepository<Entidades.MongoDbCollections.Especialidade>();

            return espRep.Where(x => x.EspecialidadeId > 0).ToList();

        }

        #endregion

        #region Sincronização

        public void SyncHistoricoExercicio(int matricula)
        {
            IRepository<HistoricoQuestaoErradaAluno> historicoRep = new MongoRepository<HistoricoQuestaoErradaAluno>();

            var questoes = GetHistoricoErradas(matricula);

            var query = Query<HistoricoQuestaoErradaAluno>.Where(x => x.Matricula == matricula);
            historicoRep.Delete(query);

            if (questoes.Any())
                historicoRep.Add(questoes);

        }

        private void SyncExerciciosPermitidos(int matricula)
        {
            IRepository<ExercicioPermissaoAluno> permissaoRep = new MongoRepository<ExercicioPermissaoAluno>();

            var exercicios = GetExerciciosPermitidosAluno(matricula);

            try
            {
                var query = Query<ExercicioPermissaoAluno>.Where(x => x.Matricula == matricula);
                permissaoRep.Delete(query);

                if (exercicios.Any())
                    permissaoRep.Add(exercicios);
            }
            catch
            {
                throw;
            }
        }

        [Obsolete("Usar PopulateQuestoesAlternativas")]
        public void SyncAlternativas()
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            var idsQuestoesMongo = questaoRep.Select(x => x.QuestaoId).ToList();

            using (var ctx = new DesenvContext())
            {
                foreach (var id in idsQuestoesMongo)
                {
                    var alternativas = (from a in ctx.tblConcursoQuestoes_Alternativas
                                        where a.intQuestaoID == id
                                        select new Entidades.MongoDbCollections.Alternativa()
                                        {
                                            AlternativaId = a.intAlternativaID,
                                            Descricao = a.txtAlternativa
                                        }).ToList();

                    var filterDefinition = Query<Entidades.MongoDbCollections.Questao>.Where(x => x.QuestaoId == id);
                    var updateDefinition = Update<Entidades.MongoDbCollections.Questao>.Set(x => x.Alternativas, alternativas);

                    questaoRep.UpdateWithQuery(filterDefinition, updateDefinition);
                }
            }
        }

        #endregion

        #region Sincronização Novo

        public List<Entidades.MongoDbCollections.Questao> GetQuestoesJsonSimulado(int ano)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            var questoesJsonArray = new List<Entidades.MongoDbCollections.Questao>();

            using (var ctx = new DesenvContext())
            {
                var simulados = GetSimuladosAno(ano);

                foreach (var sim in simulados)
                {
                    if (sim.Ano > DateTime.Now.Year) continue;

                    var questoesSimulado = GetQuestoesMongoSimulado(sim);

                    var questaoEspecialidades = GetQuestaoEspecialidadeSimulado(questoesSimulado);

                    foreach (var item in questoesSimulado)
                    {

                        item.Especialidades.AddRange(questaoEspecialidades.Where(x => x.QuestaoId == item.QuestaoId).Select(c => c.Especialidades).FirstOrDefault());
                        item.Multidisciplinar = item.Especialidades.Count() > 1;
                        questoesJsonArray.AddRange(questoesSimulado);
                    }
                }

                questoesJsonArray = PopulateQuestoesAlternativas(questoesJsonArray, (int)EOrigemQuestao.Original);

                return questoesJsonArray;
            }
        }

        public List<Entidades.MongoDbCollections.Questao> GetQuestoesJsonConcurso(int ano)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            var questoesJsonArray = new List<Entidades.MongoDbCollections.Questao>();

            var sgConcursos = GetSiglasConcursos();

            using (var ctx = new DesenvContext())
            {

                var questoesConcurso = GetQuestoesConcursoVendaLiberada(ano);

                var questoesConcursoIds = questoesConcurso.Select(x => x.QuestaoId).ToList();

                var especialidadesXClassificacao = ctx.tblMedsoft_Especialidade_Classificacao.Select(x => new { x.intClassificacaoID, x.intEspecialidadeID }).ToList();

                var questaoXEspecialidade = GetQuestaoEspecialidadeConcurso(questoesConcursoIds);

                foreach (var questao in questoesConcurso.OrderBy(x => x.ConcursoSigla))
                {
                    questao.Especialidades.AddRange(questaoXEspecialidade.Where(x => x.QuestaoId == questao.QuestaoId)
                        .Select(x => new Entidades.MongoDbCollections.Especialidade
                        {
                            EspecialidadeId = x.EspecialidadeId,
                            EspecialidadeNome = x.EspecialidadeNome,
                            EspecialidadeSigla = x.EspecialidadeSigla,
                            EspecialidadeParentId = (especialidadesXClassificacao.FirstOrDefault(z => z.intClassificacaoID == x.ClassificacaoParentId) != null) ? especialidadesXClassificacao.FirstOrDefault(z => z.intClassificacaoID == x.ClassificacaoParentId).intEspecialidadeID : 0
                        }).ToList());

                    questao.ConcursoEntidadeId = sgConcursos.FindIndex(x => x == questao.ConcursoSigla.Trim()) + 1;
                }

                questoesJsonArray.AddRange(questoesConcurso);
            }

            questoesJsonArray = PopulateQuestoesAlternativas(questoesJsonArray, (int)EOrigemQuestao.Concurso);

            return questoesJsonArray;
        }

        public List<Entidades.MongoDbCollections.Questao> GetQuestoesConcursoVendaLiberada(int ano)
        {
            List<Entidades.MongoDbCollections.Questao> questoesConcurso;

            using (var ctx = new DesenvContext())
            {
                questoesConcurso =
                 (from q in ctx.tblConcursoQuestoes
                  join cp in ctx.tblConcurso_Provas
                  on q.intProvaID equals cp.intProvaID
                  join cpt in ctx.tblConcurso_Provas_Tipos
                  on cp.intProvaTipoID equals cpt.intProvaTipoID
                  join c in ctx.tblConcurso
                  on cp.ID_CONCURSO equals c.ID_CONCURSO
                  where c.VL_ANO_CONCURSO == ano &&
                  cp.bitVendaLiberada == true

                  select new Entidades.MongoDbCollections.Questao
                  {
                      QuestaoId = q.intQuestaoID,
                      ConcursoId = c.ID_CONCURSO,
                      Enunciado = q.txtEnunciado,
                      ConcursoNome = c.NM_CONCURSO,
                      ConcursoSigla = c.SG_CONCURSO,
                      QuestaoAno = c.VL_ANO_CONCURSO,
                      OrigemQuestao = 2,
                      Comentario = q.txtComentario,
                      Recurso = q.txtRecurso,
                      ProvaId = q.intProvaID.Value,
                      TipoProva = new ProvaTipo { intProvaTipoID = cpt.intProvaTipoID, txtDescription = cpt.txtDescription }
                  }).ToList();
            }

            return questoesConcurso;
        }

        public List<Entidades.MongoDbCollections.Questao> PopulateQuestoesAlternativas(List<Entidades.MongoDbCollections.Questao> questoes, int OrigemQuestao)
        {

            var idsQuestoesMongo = questoes.Select(x => x.QuestaoId).ToList();
            List<QuestaoAlternativaDTO> alternativasXId;

            using (var ctx = new DesenvContext())
            {

                alternativasXId = GetAlternativasConcursoQuestoes(idsQuestoesMongo, OrigemQuestao);

                foreach (var qstao in questoes)
                {
                    var alters = alternativasXId.FindAll(x => x.QuestaoId == qstao.QuestaoId).ToList();

                    List<Entidades.MongoDbCollections.Alternativa> alternativas = new List<Entidades.MongoDbCollections.Alternativa>();

                    foreach (var alter in alters)
                    {
                        Entidades.MongoDbCollections.Alternativa alternativa =
                       new Entidades.MongoDbCollections.Alternativa()
                       {
                           AlternativaId = alter.AlternativaId,
                           Descricao = alter.Descricao
                       };

                        alternativas.Add(alternativa);
                    }

                    qstao.Alternativas = alternativas;
                }
            }

            return questoes;
        }

        public void SeedOrdenacaoEspecialidades()
        {
            IRepository<Entidades.MongoDbCollections.Especialidade> espRep = new MongoRepository<Entidades.MongoDbCollections.Especialidade>();
            IRepositoryManager<Entidades.MongoDbCollections.Especialidade> espMan = new MongoRepositoryManager<Entidades.MongoDbCollections.Especialidade>();

            List<Entidades.MongoDbCollections.Especialidade> espOrderm = new List<Entidades.MongoDbCollections.Especialidade>();

            var esp1 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 30, Ordem = 1 };
            var esp2 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 32, Ordem = 2 };
            var esp3 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 110, Ordem = 3 };
            var esp4 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 124, Ordem = 4 };
            var esp5 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 253, Ordem = 5 };
            var esp6 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 42, Ordem = 6 };
            var esp7 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 109, Ordem = 7 };
            var esp8 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 116, Ordem = 8 };
            var esp9 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 136, Ordem = 9 };

            var sesp1 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 17, Ordem = 1, EspecialidadeParentId = 32 };
            var sesp2 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 51, Ordem = 2, EspecialidadeParentId = 32 };
            var sesp3 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 57, Ordem = 3, EspecialidadeParentId = 32 };
            var sesp4 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 65, Ordem = 4, EspecialidadeParentId = 32 };
            var sesp5 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 252, Ordem = 5, EspecialidadeParentId = 32 };
            var sesp6 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 71, Ordem = 6, EspecialidadeParentId = 32 };
            var sesp7 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 93, Ordem = 7, EspecialidadeParentId = 32 };
            var sesp8 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 91, Ordem = 8, EspecialidadeParentId = 32 };
            var sesp9 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 128, Ordem = 9, EspecialidadeParentId = 32 };
            var sesp10 = new Entidades.MongoDbCollections.Especialidade { EspecialidadeId = 145, Ordem = 10, EspecialidadeParentId = 32 };

            espOrderm.Add(esp1);
            espOrderm.Add(esp2);
            espOrderm.Add(esp3);
            espOrderm.Add(esp4);
            espOrderm.Add(esp5);
            espOrderm.Add(esp6);
            espOrderm.Add(esp7);
            espOrderm.Add(esp8);
            espOrderm.Add(esp9);
            espOrderm.Add(sesp1);
            espOrderm.Add(sesp2);
            espOrderm.Add(sesp3);
            espOrderm.Add(sesp4);
            espOrderm.Add(sesp5);
            espOrderm.Add(sesp6);
            espOrderm.Add(sesp7);
            espOrderm.Add(sesp8);
            espOrderm.Add(sesp9);
            espOrderm.Add(sesp10);

            espMan.Drop();
            espRep.Add(espOrderm);


        }

        public void SeedQuestoesImpressas()
        {
            var idsQuestoesImpressas = GetQuestoesImpressas();
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            var filterDefinition = Query<Entidades.MongoDbCollections.Questao>.Where(x => x.QuestaoId.In(idsQuestoesImpressas));
            var updateDefinition = Update<Entidades.MongoDbCollections.Questao>.Set(x => x.Impressa, true);

            questaoRep.UpdateWithQuery(filterDefinition, updateDefinition, true);
        }

        #endregion

        public List<Entidades.MongoDbCollections.Questao> GetQuestoesElegiveisToMongo(int ano)
        {
            var questoesElegiveisConcurso = GetQuestoesJsonConcurso(ano);
            var questoesElegiveisSimulado = GetQuestoesJsonSimulado(ano);

            var questoesElegiveis = new List<Entidades.MongoDbCollections.Questao>();

            questoesElegiveis.AddRange(questoesElegiveisConcurso);


            var questoesImpressas = GetQuestoesImpressas().ToList();

            foreach (var questao in questoesElegiveis)
            {
                if (questoesImpressas.Exists(x => x == questao.QuestaoId))
                {
                    questao.Impressa = true;
                }
            }


            questoesElegiveis.AddRange(questoesElegiveisSimulado);

            questoesElegiveis = questoesElegiveis.Distinct().ToList();

            return questoesElegiveis;
        }

        public int RemoverQuestoesMongo(List<Entidades.MongoDbCollections.Questao> questoes)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRepo = new MongoRepository<Entidades.MongoDbCollections.Questao>();

            List<int?> questoesToRemove = new List<int?>();
            questoesToRemove.AddRange(questoes.Select(x => x.QuestaoId));
            var retorno = Convert.ToInt32(questaoRepo.Collection.Remove(Query<Entidades.MongoDbCollections.Questao>.In(p => p.QuestaoId, questoesToRemove)).DocumentsAffected);
            //questaoRepo.Collection.DropAllIndexes();
            return retorno;

        }

        public bool InsertQuestoesBatchMongo(List<Entidades.MongoDbCollections.Questao> questoes)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRepo = new MongoRepository<Entidades.MongoDbCollections.Questao>();
            var writeResults = questaoRepo.Collection.InsertBatch(questoes).ToList();
            //IMongoIndexKeys enunciado_questoes_alternativas = new IndexKeysBuilder().Text("Enunciado").Text("Alternativas.Descricao");
            //questaoRepo.Collection.EnsureIndex(enunciado_questoes_alternativas);
            if (writeResults.Any(x => x.HasLastErrorMessage))
                return false;
            return true;
        }

        public List<QuestaoAlternativaDTO> GetAlternativasConcursoQuestoes(List<int?> idsQuestoes, int OrigemQuestao)
        {
            List<QuestaoAlternativaDTO> alternativaXQuestao = null;

            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    switch (OrigemQuestao)
                    {
                        case (int)EOrigemQuestao.Concurso:
                            alternativaXQuestao = (from a in ctxMatDir.tblConcursoQuestoes_Alternativas
                                                   where idsQuestoes.Contains(a.intQuestaoID)
                                                   select new QuestaoAlternativaDTO()
                                                   {
                                                       QuestaoId = a.intQuestaoID,
                                                       AlternativaId = a.intAlternativaID,
                                                       Descricao = a.txtAlternativa
                                                   }).ToList();
                            break;
                        case (int)EOrigemQuestao.Original:
                            alternativaXQuestao = (from a in ctx.tblQuestaoAlternativas
                                                   where idsQuestoes.Contains(a.intQuestaoID)
                                                   select new QuestaoAlternativaDTO()
                                                   {
                                                       QuestaoId = a.intQuestaoID,
                                                       AlternativaId = a.intAlternativaID,
                                                       Descricao = a.txtAlternativa
                                                   }).ToList();
                            break;
                    }

                }
            }
            return alternativaXQuestao;
        }

        public List<QuestaoEspecialidadeDTO> GetQuestaoEspecialidadeSimulado(List<Entidades.MongoDbCollections.Questao> questoesSimulado)
        {
            List<QuestaoEspecialidadeDTO> questaoEspecialidades;

            using (var ctx = new DesenvContext())
            {
                questaoEspecialidades = (from q in questoesSimulado
                                         group new Entidades.MongoDbCollections.Especialidade
                                         {
                                             EspecialidadeId = q.EspecialidadeId,
                                             EspecialidadeSigla = q.EspecialidadeSigla,
                                             EspecialidadeNome = q.EspecialidadeNome,
                                             EspecialidadeParentId = 0
                                         }
                                         by q.QuestaoId into g
                                         select new QuestaoEspecialidadeDTO
                                         {
                                             QuestaoId = g.Key,
                                             Especialidades = g.ToList()
                                         }).ToList();
            }
            return questaoEspecialidades;
        }

        public List<QuestaoEspecialidadeDTO> GetQuestaoEspecialidadeConcurso(List<int?> questoesConcursoIds)
        {
            List<QuestaoEspecialidadeDTO> questaoXEspecialidade;

            using (var ctx = new DesenvContext())
            {
                using (var ctxAcad = new AcademicoContext())
                {
                    var questao = (from cqc in ctx.tblConcursoQuestao_Classificacao
                                   join cqcc in ctx.tblConcursoQuestaoCatologoDeClassificacoes on cqc.intClassificacaoID equals cqcc.intClassificacaoID
                                   join es in ctx.tblMedsoft_Especialidade_Classificacao on cqc.intClassificacaoID equals es.intClassificacaoID
                                   where (new[] { 2, 3, 4, 5, 8 }).Contains(cqc.intTipoDeClassificacao) && questoesConcursoIds.Contains(cqc.intQuestaoID)
                                   select new
                                   {
                                       QuestaoId = cqc.intQuestaoID,
                                       intEspecialidadeId = es.intEspecialidadeID,
                                       ClassificacaoParentId = cqcc.intParent
                                   }).ToList();

                    List<int> listaEspecialidadeId = questao.Select(x => x.intEspecialidadeId).ToList();

                    var especialidade = (from e in ctxAcad.tblEspecialidades
                                         where listaEspecialidadeId.Contains(e.intEspecialidadeID)
                                         select new
                                         {
                                             e.intEspecialidadeID,
                                             e.CD_ESPECIALIDADE,
                                             e.DE_ESPECIALIDADE,
                                         }).ToList();

                    questaoXEspecialidade = (from c in questao
                                             join e in especialidade on c.intEspecialidadeId equals e.intEspecialidadeID
                                             select new QuestaoEspecialidadeDTO
                                             {
                                                 QuestaoId = c.QuestaoId,
                                                 EspecialidadeId = e.intEspecialidadeID,
                                                 EspecialidadeSigla = e.CD_ESPECIALIDADE,
                                                 EspecialidadeNome = e.DE_ESPECIALIDADE,
                                                 ClassificacaoParentId = c.ClassificacaoParentId
                                             }).ToList();
                }
            }
            return questaoXEspecialidade;
        }

        public List<Entidades.MongoDbCollections.Questao> GetQuestoesMongoSimulado(SimuladoDTO sim)
        {
            List<Entidades.MongoDbCollections.Questao> questoesSimulado;

            using (var ctx = new AcademicoContext())
            {
                questoesSimulado = (
                   from q in ctx.tblQuestoes
                   join v in ctx.tblSimuladoVersao on q.intQuestaoID equals v.intQuestaoID
                   join s in ctx.tblSimulado on v.intSimuladoID equals s.intSimuladoID
                   join e in ctx.tblEspecialidades
                   on q.intEspecialidadeID equals e.intEspecialidadeID
                   where v.intSimuladoID == sim.ID
                   && v.intVersaoID == 1

                   select new Entidades.MongoDbCollections.Questao
                   {
                       QuestaoId = q.intQuestaoID,
                       EspecialidadeId = q.intEspecialidadeID,
                       Enunciado = q.txtEnunciado,
                       EspecialidadeSigla = e.CD_ESPECIALIDADE,
                       EspecialidadeNome = e.DE_ESPECIALIDADE,
                       QuestaoAno = s.intAno,
                       OrigemQuestao = 1,
                       SimuladoNome = s.txtSimuladoDescription,
                       SimuladoId = s.intSimuladoID,
                       SimuladoSigla = s.txtSimuladoName,
                       Comentario = q.txtComentario,
                       Recurso = q.txtRecurso,
                       ProvaId = sim.ID
                   }).ToList();
            }
            return questoesSimulado;
        }

        public List<Entidades.MongoDbCollections.Questao> GetQuestoesMongoConcurso(int concursoID)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();
            return questaoRep.Where(x => x.ConcursoEntidadeId == concursoID).ToList();
        }

        public int GetConcursoEntidadeIdBySigla(string sigla)
        {
            IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();
            return (int)questaoRep.Where(x => x.ConcursoSigla == sigla).FirstOrDefault().ConcursoEntidadeId;
        }

        public List<SimuladoDTO> GetSimuladosAno(int ano)
        {
            List<SimuladoDTO> simulados;

            using (var ctx = new DesenvContext())
            {
                var anoComDteReleaseGabaritoNulo = 2017;

                using (var ctxAcad = new AcademicoContext())
                {
                    var simulado = (from s in ctxAcad.tblSimulado
                                    where ((s.dteReleaseGabarito <= DateTime.Now) || (s.intAno < anoComDteReleaseGabaritoNulo))  //Antes de 2017 os campos dteReleaseGabarito são todos nulos
                                       && s.intAno == ano
                                    select new
                                    {
                                        s.intSimuladoID,
                                        s.txtSimuladoName,
                                        s.txtSimuladoDescription,
                                        s.dteDataHoraInicioWEB,
                                        s.dteDataHoraTerminoWEB,
                                        s.intTipoSimuladoID,
                                        s.bitOnline,
                                        s.intDuracaoSimulado,
                                        s.dteInicioConsultaRanking,
                                        s.intBookID
                                    }).ToList();

                    var books = (from p in ctx.tblProducts
                                 join book in ctx.tblBooks on p.intProductID equals book.intBookID
                                 //join s in ctx.tblSimuladoes on p.intProductID equals s.intBookID
                                 where book.intYear != null
                                 //&& ((s.dteReleaseGabarito <= DateTime.Now) || (s.intAno < anoComDteReleaseGabaritoNulo))  //Antes de 2017 os campos dteReleaseGabarito são todos nulos
                                 //&& s.intAno == ano
                                 select new
                                 {
                                     //ID = s.intSimuladoID,
                                     //ExercicioName = s.txtSimuladoName,
                                     //Descricao = s.txtSimuladoDescription,
                                     intBookID = book.intBookID,
                                     Ano = book.intYear ?? 0,
                                     //DataInicio = s.dteDataHoraInicioWEB,
                                     //DataFim = s.dteDataHoraTerminoWEB,
                                     //TipoId = s.intTipoSimuladoID,
                                     //Online = s.bitOnline,
                                     //Duracao = s.intDuracaoSimulado,
                                     //DtLiberacaoRanking = s.dteInicioConsultaRanking
                                 }).ToList();

                    simulados = (from b in books
                                 join s in simulado on b.intBookID equals s.intBookID
                                 orderby s.intSimuladoID
                                 select new SimuladoDTO
                                 {
                                     ID = s.intSimuladoID,
                                     ExercicioName = s.txtSimuladoName,
                                     Descricao = s.txtSimuladoDescription,
                                     Ano = b.Ano,
                                     DataInicio = s.dteDataHoraInicioWEB,
                                     DataFim = s.dteDataHoraTerminoWEB,
                                     TipoId = s.intTipoSimuladoID,
                                     Online = s.bitOnline,
                                     Duracao = s.intDuracaoSimulado,
                                     DtLiberacaoRanking = s.dteInicioConsultaRanking
                                 }).Distinct().ToList();
                }
            }
            return simulados;
        }

        public IEnumerable<int?> GetQuestoesImpressas()
        {
            using (var ctx = new DesenvContext())
            {
                var questoesImpressas = (from a in ctx.tblConcursoQuestoes
                                         join b in ctx.tblConcursoQuestao_Classificacao_Autorizacao on a.intQuestaoID equals b.intQuestaoID
                                         join c in ctx.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada on b.intMaterialID equals c.intProductID
                                         join d in ctx.tblBooks on c.intProductID equals d.intBookID
                                         where (bool)b.bitAutorizacao && c.bitActive
                                         select (int?)a.intQuestaoID).ToList();

                return questoesImpressas;
            }
        }

        public List<String> GetSiglasConcursos()
        {
            List<String> siglas;

            using (var ctx = new DesenvContext())
            {
                siglas = ctx.tblConcurso.Select(x => x.SG_CONCURSO.Trim()).Distinct().ToList();
            }

            return siglas;
        }

        public void Update_ConcursoEntidadeId_QuestoesMongo()
        {
            var sgConcursos = GetSiglasConcursos();

            foreach (var sigla in sgConcursos)
            {
                var concursoEntidadeId = sgConcursos.FindIndex(x => x == sigla) + 1;

                IRepository<Entidades.MongoDbCollections.Questao> questaoRep = new MongoRepository<Entidades.MongoDbCollections.Questao>();

                var filterDefinition = Query<Entidades.MongoDbCollections.Questao>.Where(x => x.ConcursoSigla.Equals(sigla) && x.ConcursoEntidadeId != concursoEntidadeId);
                var updateDefinition = Update<Entidades.MongoDbCollections.Questao>.Set(x => x.ConcursoEntidadeId, concursoEntidadeId);

                questaoRep.UpdateWithQuery(filterDefinition, updateDefinition, true);
            }
        }

        public List<int?> GetListaAnosElegiveisSincronizar()
        {
            using(MiniProfiler.Current.Step("listando anos elegíveis sincronizados"))
            {
                using (var ctx = new DesenvContext())
                {
                    IEnumerable<int?> anoMatDir = ((from c in ctx.tblConcurso select c.VL_ANO_CONCURSO == null ? 0 : c.VL_ANO_CONCURSO).Distinct()
                        .Union(from c2 in ctx.tblConcursos select c2.intAno == null ? 0 : c2.intAno).Distinct()
                        .Union(from b in ctx.tblBooks select b.intYear == null ? 0 : b.intYear).Distinct()).AsEnumerable();

                    using (AcademicoContext ctxAcad = new AcademicoContext())
                    {
                        IEnumerable<int?> anoAcad = (from s in ctxAcad.tblSimulado select s.intAno == null ? 0 : s.intAno).Distinct().AsEnumerable();

                        return anoMatDir.Union(anoAcad).Where(x => x > 0).Distinct().ToList();
                    }

                    //return ((from c in ctx.tblConcurso select c.VL_ANO_CONCURSO == null ? 0 : c.VL_ANO_CONCURSO).Distinct()
                    //    .Union(from c2 in ctx.tblConcursos select c2.intAno == null ? 0 : c2.intAno).Distinct()
                    //    .Union(from b in ctx.tblBooks select b.intYear == null ? 0 : b.intYear).Distinct()
                    //    .Union(from s in ctx.tblSimuladoes select s.intAno == null ? 0 : s.intAno).Distinct())
                    //    .Where(x => x > 0).Distinct().ToList();
                }
            }
        }
    }
}