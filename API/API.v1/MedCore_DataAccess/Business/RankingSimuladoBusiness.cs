using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business
{
    public class RankingSimuladoBusiness
    {
        private IRankingSimuladoData _rankingSimuladoRepository;
        private IEspecialidadeData _especialidadeRepository;
        private IFilialData _filialRepository;

        public RankingSimuladoBusiness(IRankingSimuladoData rankingSimuladoRepository, IEspecialidadeData especialidadeRepository, IFilialData filialRepository)
        {
            _rankingSimuladoRepository = rankingSimuladoRepository;
            _especialidadeRepository = especialidadeRepository;
            _filialRepository = filialRepository;
        }
        public RankingSimuladoAluno GetResultadoRankingAluno(int matricula, int idSimulado, int idAplicacao, string especialidade, string unidades, string localidade = "")
        {
            try
            {
                using(MiniProfiler.Current.Step("Obtendo resultado ranking aluno"))
                {
                    var simuladoConsolidado = _rankingSimuladoRepository.GetSimuladoConsolidado(matricula, idSimulado);
                    var result = new RankingSimuladoAluno();

                    if (simuladoConsolidado == null)
                    {
                        result.EstatisticasAlunoRankingOnline = _rankingSimuladoRepository.GetEstatisticaAlunoSimulado(matricula, idSimulado, true);

                        _rankingSimuladoRepository.InsertSimuladoConsolidado(matricula, idSimulado, result.EstatisticasAlunoRankingOnline);
                    }
                    else
                        result.EstatisticasAlunoRankingOnline = _rankingSimuladoRepository.GetEstatisticaAlunoSimulado(matricula, idSimulado, true);

                    result.EstatisticasAlunoRankingEstudo = _rankingSimuladoRepository.GetEstatisticaAlunoSimulado(matricula, idSimulado, false);

                    var local = string.IsNullOrEmpty(localidade) ? string.Empty : (localidade.IndexOf('(') > 0 ? localidade.Substring(0, localidade.IndexOf('(')).Trim() : localidade);

                    var rank = new RankingSimuladoAluno();

                    try
                    {
                        if (RedisCacheManager.CannotCache(RedisCacheConstants.Simulado.KeyRankingSimulado))
                            rank = _rankingSimuladoRepository.GetRankingObjetiva(matricula, idSimulado, especialidade, unidades, local);
                        else
                            rank = _rankingSimuladoRepository.GetRankingObjetivaCache(matricula, idSimulado, especialidade, unidades, local);
                    }
                    catch (Exception)
                    {
                        rank = _rankingSimuladoRepository.GetRankingObjetiva(matricula, idSimulado, especialidade, unidades, local);
                    }
                    

                    if (rank != null)
                    {
                        result.Nota = rank.Nota;
                        result.Posicao = rank.Posicao;
                        result.DataRealizacao = rank.Simulado.DtHoraInicio;
                        result.QuantidadeParticipantes = (int)Math.Ceiling(Convert.ToDecimal(getQuantidadeParticipantesRanking(rank.QuantidadeParticipantes)) / 2);
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int getQuantidadeParticipantesRanking(int quantidadeParticipantes)
        {
            if (quantidadeParticipantes < 200)
            {
                return (quantidadeParticipantes * (200 / quantidadeParticipantes)) + quantidadeParticipantes;
            }
            else
            {
                return quantidadeParticipantes;
            }
        }

        public FiltroRanking GetFiltroRankingSimulado(int idSimulado)
        {
            try
            {
                using(MiniProfiler.Current.Step("Obter modal simulado online"))
                {
                    var filtroRanking = new FiltroRanking();
                    var especialidades = new Especialidades();
                    var estados = new Estados();
                    var filiais = new Filiais();
                    var ranking = _rankingSimuladoRepository.GetRankingParcial(idSimulado);
                    foreach (var item in ranking)
                    {
                        especialidades.Add(new Especialidade { Descricao = item.txtEspecialidade });

                        var hasSigla = item.txtUnidade.Any(x => x.Equals('('));
                        estados.Add(new Estado
                        {
                            Sigla = hasSigla ? item.txtUnidade.Substring(item.txtUnidade.IndexOf('(')).Replace("(", "").Replace(")", "") : item.txtUnidade,
                            ID = (int)item.intStateID
                        });

                        filiais.Add(new Filial
                        {

                            Nome = item.txtUnidade == "MEDREADER" ? "MEDREADER" : item.txtUnidade,
                            EstadoID = (int)item.intStateID
                        });
                    }

                    var estadoEAD = -1;

                    filtroRanking.Especialidades.AddRange(especialidades.GroupBy(e => new { e.Descricao }).Select(g => g.First()).ToList());
                    filtroRanking.Estados.AddRange(estados.GroupBy(e => new { e.Sigla, e.ID }).Select(g => g.First()).ToList());
                    filtroRanking.Unidades.AddRange(filiais.Where(e => e.EstadoID != estadoEAD).GroupBy(e => new { e.Nome, e.ID, e.EstadoID }).Select(g => g.First()).ToList());

                    return filtroRanking;
                    //}
                }
            }
            catch
            {
                throw;
            }
        }

        public List<SimuladoRankingFase01DTO> GeraRankingSimuladoNacional(int intSimuladoID)
        {
            var simuladoRankingFase01 = new List<SimuladoRankingFase01DTO>();
            var productGroup = new int[] { 
                (int)Utilidades.ProductGroups.MEDCURSO,
                (int)Utilidades.ProductGroups.MED,
                (int)Utilidades.ProductGroups.MEDEAD,
                (int)Utilidades.ProductGroups.MEDCURSOEAD,
                (int)Utilidades.ProductGroups.INTENSIVO,
                (int)Utilidades.ProductGroups.RAC,
                (int)Utilidades.ProductGroups.RACIPE
            };
            
            var simulado = _rankingSimuladoRepository.GetSimulado(intSimuladoID);
            if (simulado != null)
            {
                var listaSimulados = new List<SimuladoDTO>(){simulado}; 
                var intNumQuestoes = simulado.QuantidadeQuestoes ?? 0;
                var peso = simulado.PesoProvaObjetiva ?? 1;
                var intYear = simulado.Ano ?? 0;

                var listaOrdensVenda = _rankingSimuladoRepository.GetOrdemVendaTodosClientes(intYear);

                var listaLogSimuladoAlunoTurma = _rankingSimuladoRepository.GetLogSimuladoAlunoTurma(intSimuladoID);

                var listaEspecialidades = _especialidadeRepository.GetAll();

                var especialidadeFixa = BuscarEspecialidadeFixa(simulado, listaEspecialidades);
                var txtUnidadeFixo = BuscarUnidadeFixo(simulado);
                var txtLocalFixo = BuscarLocalFixo(simulado);
                var intStateFixo = BuscarIntStateFixo(simulado); 

                var dadosCliente = (
                    from ordens in listaOrdensVenda
                    join sim in listaSimulados on intSimuladoID equals sim.ID
                    join e in listaEspecialidades on ordens.intEspecialidadeID equals e.Id into leftEsp
                    from esp in leftEsp.DefaultIfEmpty()
                    join l in listaLogSimuladoAlunoTurma on ordens.intClientID equals l.intClientID into leftLog
                    from log in leftLog.DefaultIfEmpty()
                    where
                        (sim.TipoId == (int)Constants.TipoSimulado.Extensivo && productGroup.Contains(ordens.intProductGroup1ID))
                        || (sim.TipoId == (int)Constants.TipoSimulado.CPMED && ordens.intProductGroup1ID == (int)Utilidades.ProductGroups.CPMED)
                        || (sim.TipoId == (int)Constants.TipoSimulado.Intensivo && ordens.intProductGroup1ID == (int)Utilidades.ProductGroups.INTENSIVO)
                        || (sim.TipoId == (int)Constants.TipoSimulado.R3_Pediatria && ordens.intProductGroup1ID == (int)Utilidades.ProductGroups.R3_PEDIATRIA)
                        || (sim.TipoId == (int)Constants.TipoSimulado.R3_Clinica && ordens.intProductGroup1ID == (int)Utilidades.ProductGroups.R3_CLINICA)
                        || (sim.TipoId == (int)Constants.TipoSimulado.R3_Cirurgia && ordens.intProductGroup1ID == (int)Utilidades.ProductGroups.R3_CIRURGIA)
                        || (sim.TipoId == (int)Constants.TipoSimulado.R4_GO && ordens.intProductGroup1ID == (int)Utilidades.ProductGroups.R4_GO)
                    group ordens by new
                    {
                        ordens.intOrderID,
                        ordens.intClientID,
                        ordens.personName,
                        txtTurma = log != null ? log.txtTurma : null,
                        ordens.txtDescription,
                        txtUnidade = log != null ? log.txtUnidade : null,
                        ordens.txtStoreName,
                        intState = log != null ? log.intState : 0,
                        ordens.cityIntState,
                        DescricaoEspecialidade = esp != null ? esp.Descricao : string.Empty
                    } into g
                    select new
                    {
                        g.Key.intOrderID,
                        g.Key.intClientID,
                        g.Key.personName,
                        txtTurma = txtLocalFixo ?? ((g.Key.txtTurma ?? "") != "" ? g.Key.txtTurma : g.Min(x => x.txtName)),
                        g.Key.txtDescription,
                        txtUnidade = txtUnidadeFixo ?? ((g.Key.txtUnidade ?? "") != "" ? g.Key.txtUnidade : g.Key.txtStoreName),
                        intState = intStateFixo ?? ((g.Key.intState ?? 0) != 0 ? g.Key.intState : g.Key.cityIntState),
                        txtEspecialidade = especialidadeFixa ?? g.Key.DescricaoEspecialidade
                    }
                ).ToList();

                var listaResultados = _rankingSimuladoRepository.ListResultado(intSimuladoID);

                var resultados = (
                    from res in listaResultados
                    join dados in dadosCliente on res.intClientID equals dados.intClientID
                    where
                        res.intSimuladoID == intSimuladoID
                    orderby res.intAcertos descending, res.intClientID
                    select new
                    {
                        res.intSimuladoID,
                        res.intAcertos,
                        res.intClientID,
                        dados.personName,
                        txtLocal = dados.txtTurma,
                        dados.txtUnidade,
                        dados.intState,
                        dados.txtEspecialidade,
                        res.intArquivoID
                    }
                ).Distinct().ToList();

                var resultadosTratados = (
                    from resDifEadMed in resultados
                    from resEadMed in resultados
                        .Where(x => x.txtUnidade == "EAD MED" && x.intClientID == resDifEadMed.intClientID)
                        .DefaultIfEmpty()
                    where
                        resDifEadMed.txtUnidade != "EAD MED"
                    select new
                    {
                        resDifEadMed.intSimuladoID,
                        resDifEadMed.intAcertos,
                        resDifEadMed.intClientID,
                        resDifEadMed.personName,
                        txtLocal = resEadMed != null && resEadMed.txtLocal != null ?
                            resDifEadMed.txtLocal + '/' + resEadMed.txtLocal :
                            resDifEadMed.txtLocal,
                        resDifEadMed.txtUnidade,
                        resDifEadMed.intState,
                        resDifEadMed.txtEspecialidade,
                        resDifEadMed.intArquivoID
                    }
                ).ToList();

                int i = 1;
                var listaNotas = (
                    from res in resultadosTratados
                    group res by new
                    {
                        res.intAcertos,
                        res.intClientID,
                        res.txtUnidade,
                        res.personName,
                        res.txtEspecialidade,
                        res.intState,
                        res.intArquivoID
                    } into g
                    select new {
                        idNum = i++,
                        intAcertos = g.Key.intAcertos,
                        dblNotaFinal = Math.Round(Convert.ToDouble(g.Key.intAcertos) / intNumQuestoes * peso, 2),
                        intClientID = g.Key.intClientID,
                        personName = g.Key.personName,
                        txtLocal = g.Max(x => x.txtLocal),
                        txtUnidade = g.Key.txtUnidade,
                        intState = g.Key.intState,
                        txtEspecialidade = g.Key.txtEspecialidade,
                        intArquivoID = g.Key.intArquivoID
                    }
                ).OrderByDescending(x => x.dblNotaFinal)
                .ToList();

                var notasAgrupadas = listaNotas
                    .GroupBy(x => x.dblNotaFinal)
                    .Select(x => new { dblNotaFinal = x.Key, minIdNum = x.Min(n => n.idNum) })
                    .ToList();

                var ranking = (
                    from notas in listaNotas
                    join agrupadas in notasAgrupadas on notas.dblNotaFinal equals agrupadas.dblNotaFinal
                    select new 
                    {
                        idNum = notas.idNum,
                        idNum2 = agrupadas.minIdNum,
                        intAcertos = notas.intAcertos,
                        dblNotaFinal = notas.dblNotaFinal,
                        intClientID = notas.intClientID,
                        personName = notas.personName,
                        txtLocal = notas.txtLocal,
                        txtUnidade = notas.txtUnidade,
                        intState = notas.intState,
                        txtEspecialidade = notas.txtEspecialidade,
                        intArquivoID = notas.intArquivoID
                    }
                ).ToList();

                simuladoRankingFase01.AddRange((
                    from r in ranking 
                    select new SimuladoRankingFase01DTO()
                    {
                        intSimuladoID = intSimuladoID,
                        txtPosicao = r.idNum2.ToString() + "º",
                        intAcertos = r.intAcertos,
                        dblNotaFinal = r.dblNotaFinal,
                        intClientID = r.intClientID,
                        txtUnidade = r.txtUnidade,
                        txtLocal = r.txtLocal,
                        txtName = r.personName,
                        txtEspecialidade = r.txtEspecialidade,
                        intStateID = r.intState,
                        intArquivoID = r.intArquivoID
                    }
                    
                ).ToList());

                _rankingSimuladoRepository.RemoverSimuladoRankingFase01(intSimuladoID);
                _rankingSimuladoRepository.InserirSimuladoRankingFase01(simuladoRankingFase01);
            }
            return simuladoRankingFase01;
        }

        private string BuscarEspecialidadeFixa(SimuladoDTO simulado, List<Especialidade> listaEspecialidades)
        {
            string especialidade = null;
            var mapaRankingIdEspecialidade = Utilidades.GetMapaRankingIdEspecialidade();
            if (mapaRankingIdEspecialidade.ContainsKey(simulado.TipoId))
            {
                especialidade = listaEspecialidades
                    .Where(x => x.Id == mapaRankingIdEspecialidade[simulado.TipoId])
                    .Select(x => x.Descricao)
                    .FirstOrDefault();
            }

            return especialidade;
        }

        private String BuscarUnidadeFixo(SimuladoDTO simulado)
        {
            Filial unidadeFixo = null;
            switch ((Constants.TipoSimulado)simulado.TipoId)
            {
                case Constants.TipoSimulado.R3_Pediatria:
                    unidadeFixo = _filialRepository.GetFilial((int)Constants.Stores.R3_MEDERI);
                    break;
                case Constants.TipoSimulado.R3_Clinica:
                    unidadeFixo = _filialRepository.GetFilial((int)Constants.Stores.R3_MEDWRITERS);
                    break;
                case Constants.TipoSimulado.R3_Cirurgia:
                    unidadeFixo = _filialRepository.GetFilial((int)Constants.Stores.R3_MEDERI);
                    break;
                case Constants.TipoSimulado.R4_GO:
                    unidadeFixo = _filialRepository.GetFilial((int)Constants.Stores.R3_MEDERI);
                    break;
            }

            return unidadeFixo != null ? unidadeFixo.Nome : null;
        }

        private String BuscarLocalFixo(SimuladoDTO simulado)
        {
            string localFixo = null;

            switch ((Constants.TipoSimulado)simulado.TipoId)
            {
                case Constants.TipoSimulado.R3_Pediatria:
                    localFixo = string.Format("{0} R3 PEDIATRIA", simulado.Ano);
                    break;
                case Constants.TipoSimulado.R3_Clinica:
                    localFixo = string.Format("{0} R3 CLÍNICA MÉDICA", simulado.Ano);
                    break;
                case Constants.TipoSimulado.R3_Cirurgia:
                    localFixo = string.Format("{0} R3 CIRURGIA", simulado.Ano);
                    break;
                case Constants.TipoSimulado.R4_GO:
                    localFixo = string.Format("{0} R4 GO", simulado.Ano);
                    break;
            }

            return localFixo;
        }

        private int? BuscarIntStateFixo(SimuladoDTO simulado)
        {
            var isRMais = simulado.TipoId == (int)Constants.TipoSimulado.R3_Cirurgia
                || simulado.TipoId == (int)Constants.TipoSimulado.R3_Clinica
                || simulado.TipoId == (int)Constants.TipoSimulado.R3_Pediatria
                || simulado.TipoId == (int)Constants.TipoSimulado.R4_GO;

            return isRMais ? -1 : (int?)null;
        }

        public string ValidaRankingSimulado(int intSimuladoId)
        {
            string Resultado = "";
            var ranking = _rankingSimuladoRepository.GetRankingSimulado(intSimuladoId);
            var simulado = _rankingSimuladoRepository.GetSimulado(intSimuladoId);
            int QuantidadeQuestaoSimulado = (int)simulado.QuantidadeQuestoes;

            var AlunosGabaritaram = ranking.Where(x => x.intAcertos == QuantidadeQuestaoSimulado).Count();

            if (ranking == null || ranking.Count == 0)
                Resultado += string.Format("Sem registro de Ranking para o simulado ID: {0} \n\n", intSimuladoId);

            if (AlunosGabaritaram >= (ranking.Count / 10))
                Resultado += "Quantidade de alunos que gabaritaram é superior a 10% do total de registros do Ranking \n\n";

            return Resultado;
        }
    }
}