using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Business
{
    public class SimuladoBusiness : ISimuladoBusiness
    {
        private ISimuladoData _repository;
        private IExercicioData _exercicioRepository;
        private IBannerData _bannerRepository;
        private IProdutoData _produtoRepository;

        public SimuladoBusiness(ISimuladoData repository, IExercicioData exercicioRepository, IBannerData bannerRepository)
        {
            _repository = repository;
            _exercicioRepository = exercicioRepository;
            _bannerRepository = bannerRepository;
        }

        public SimuladoBusiness(ISimuladoData repository, IExercicioData exercicioRepository, IBannerData bannerRepository, IProdutoData produtoRepository)
        {
            _repository = repository;
            _exercicioRepository = exercicioRepository;
            _bannerRepository = bannerRepository;
            _produtoRepository = produtoRepository;
        }

        public List<SimuladoCronogramaDTO> GetCronogramaSimulados(int ano, int matricula)
        {
            var ret = _exercicioRepository.GetCronogramaSimulados(ano, matricula);
            return ret;
        }

        public int GetIDSimuladoCPMED(int ano) 
        {
            var ret = _exercicioRepository.GetIDSimuladoCPMED(ano);
            return ret;
        }

        public List<int> GetAnosSimuladoExtensivo(int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo anos simulado extensivo"))
            {
                if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetAnosExerciciosPermitidos))
                    return RedisCacheManager.GetItemObject<List<int>>(RedisCacheConstants.DadosFakes.KeyGetAnosExerciciosPermitidos);

                return _exercicioRepository.GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, matricula, true, idaplicacao);
            }
        }

        public List<Exercicio> GetSimuladosExtensivo(int ano, int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo simulado extensivo"))
            {
                return GetSimuladoEspecialidadesAgrupadas(ano, matricula, idaplicacao, (int)Constants.TipoSimulado.Extensivo);
            }
        }

        public List<Exercicio> GetSimuladoEspecialidadesAgrupadas(int ano, int matricula, int idAplicacao, int idTipoSimulado = (int) Constants.TipoSimulado.Extensivo)
        {
            try
            {
                var retorno = new List<Exercicio>();

                var ids = _exercicioRepository.GetIdsExerciciosRealizadosAluno(matricula, idTipoSimulado);

                var simulados = _exercicioRepository.GetSimuladosByFilters(ano, matricula, idAplicacao, true, idTipoSimulado);

                if (simulados.Count == 0)
                    return new List<Exercicio>();

                var simuladosAgrupados = this.GetSimuladosDistintosAgrupados(simulados);

                foreach (var sim in simuladosAgrupados)
                {        

                    var simulado = new Exercicio
                    {
                        Ano = sim.Ano,
                        ExercicioName = sim.Descricao.Substring(5).Split('-')[0].Trim(),
                        Descricao = sim.Descricao.Substring(5).Split('-')[1].Trim(),
                        ID = sim.ID,
                        DataInicio = sim.DataInicio,
                        DataFim = sim.DataFim,  
                        Online = sim.Online,
                        Ativo = true,
                        IdTipoRealizacao = 1,
                        Duracao = sim.Duracao,
                        Realizado = Convert.ToInt32(ChecaAlgumExercicioDoSimuladoFoiRealizado(ids, sim)),
                        DtLiberacaoRanking = sim.DtLiberacaoRanking,
                        DtUnixLiberacaoRanking = Utilidades.ToUnixTimespan(sim.DtLiberacaoRanking),
                        TipoId = sim.TipoId,
                        BitAndamento = sim.bitAndamento
                    };

                    simulado.Especialidades = _repository.GetEspecialidadesSimulado(simulados, sim);

                    simulado.Ativo = ChecaSimuladoOnlineDeveEstarHabilitado(sim, simulado, ids, matricula);

                    simulado.Label = GetLabelSimulado(simulado.Ativo,
                                                    Convert.ToBoolean(simulado.Realizado),
                                                    Convert.ToBoolean(simulado.Online),
                                                    simulado.DataInicio,
                                                    simulado.DataFim);

                    if (!simulado.Ativo && Convert.ToBoolean(simulado.Realizado) && (idAplicacao == (int)Aplicacoes.MsProMobile || idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON))
                    {
                        simulado.Descricao = string.Format("SIMULADO REALIZADO RANKING EM {0}/{1}", simulado.DtLiberacaoRanking.Day.ToString("00"), simulado.DtLiberacaoRanking.Month.ToString("00"));
                    }

                    retorno.Add(simulado);
                }

                SetarSimuladoAtual(retorno, ano);

                return retorno;
            }
            catch
            {
                throw;
            }
        }

        private void SetarSimuladoAtual(List<Exercicio> retorno, int ano)
        {
            if (ano == DateTime.Now.Year && !retorno.Any(e => e.Atual == 1))
            {
                var idProximo = _repository.GetIdProximoSimulado(retorno);
                retorno.FirstOrDefault(x => x.ID == idProximo).Atual = 1;
            }
        }

        public string GetLabelSimulado(bool ativo, bool realizado, bool online, double dataInicio, double dataFim)
        {
            DateTime dtAtual;
            try
            {
                dtAtual = Utilidades.GetServerDate();
            }
            catch
            {
                dtAtual = DateTime.Now;
            }

            var dtInicio = Utilidades.ConvertFromUnixTimestamp(dataInicio);
            var dtFim = Utilidades.ConvertFromUnixTimestamp(dataFim);
            if (ativo)
            {
                if (online) return "realize";
                if (dtFim < dtAtual) return "estude";
            }
            else
            {
                if (realizado) return "processando";
                if (dtInicio > dtAtual) return "bloqueado";
            }

            return string.Empty;
        }

        public bool ChecaSimuladoOnlineDeveEstarHabilitado(ExercicioDTO sim, Exercicio simulado, Dictionary<bool, List<int>> idsExerciciosRealizados, int matricula)
        {
            
            if (Convert.ToBoolean(sim.Online))
            {
                if (DateTime.Now > sim.DtLiberacaoRanking)
                {
                    simulado.Ativo = true;
                    simulado.Online = 0;
                }
                else
                {
                    if (ChecaAlgumExercicioDoSimuladoFoiRealizado(idsExerciciosRealizados, sim))
                        simulado.Ativo = false;
                    else
                    {
                        if (Convert.ToBoolean(simulado.Realizado))
                            simulado.Ativo = ChecaTempoHabilUsuarioRealizacaoDeSimulado(matricula, simulado);
                        else
                            simulado.Ativo = ChecaJaneladeTempoRealizacaoDeSimulado(simulado);
                    }
                }
                return simulado.Ativo;
            }
            else
            {
                return false;
            }
        }

        private bool ChecaJaneladeTempoRealizacaoDeSimulado(Exercicio sim)
        {
            return (DateTime.Now >= Convert.ToDateTime(Utilidades.UnixTimeStampToDateTime(sim.DataInicio))
               && DateTime.Now <= Convert.ToDateTime(Utilidades.UnixTimeStampToDateTime(sim.DataFim)));
        }

        public bool ChecaTempoHabilUsuarioRealizacaoDeSimulado(int matricula, Exercicio simulado)
        {
            return DateTime.Now > _repository.GetDtInicioUltimaRealizacao(matricula, simulado.ID).AddMinutes(simulado.Duracao) ? false : true;
        }

        public bool ChecaAlgumExercicioDoSimuladoFoiRealizado(Dictionary<bool, List<int>> idsExerciciosRealizados, ExercicioDTO simulado)
        {
            var realizouSim = 0;
            if (idsExerciciosRealizados.ContainsKey(true))
            {
                bool contem = idsExerciciosRealizados.FirstOrDefault(x => x.Key).Value.Contains(simulado.ID);
                realizouSim = Convert.ToInt32(contem);
            }
            return Convert.ToBoolean(realizouSim);
        }

        public IEnumerable<ExercicioDTO> GetSimuladosDistintosAgrupados(IEnumerable<Exercicio> simulados)
        {
            var simuladosAgrupados = simulados.Select(s => new ExercicioDTO
            {
                Ano = s.Ano,
                Descricao = s.Descricao,
                ID = s.ID,
                ExercicioName = s.ExercicioName,
                DataInicio = s.DataInicio,
                DataFim = s.DataFim,
                IdTipoRealizacao = 1,
                Online = s.Online,
                Ativo = s.Ativo,
                Duracao = s.Duracao,
                DtLiberacaoRanking = s.DtLiberacaoRanking,
                TipoId = s.TipoId,
                bitAndamento = s.BitAndamento
            }).Distinct(new ExercicioDTO()).ToList();

            return simuladosAgrupados;
        }

        public List<int> GetAnosSimuladoR3Cir(int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo anos simulado R3Cir"))
            {
                return GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, matricula, (int)Constants.TipoSimulado.R3_Cirurgia, true, idaplicacao);
            }
        }

        public List<int> GetAnosSimuladoR3Cli(int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo anos simulado R3Cir"))
            {
                return GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, matricula, (int)Constants.TipoSimulado.R3_Clinica, true, idaplicacao);
            }
        }

        public List<int> GetAnosExerciciosPermitidos(Exercicio.tipoExercicio tipoExercicio, int matricula, int idTipoSimulado, bool getOnline = false, int idAplicacao = 1)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetAnosExerciciosPermitidos))
                return RedisCacheManager.GetItemObject<List<int>>(RedisCacheConstants.DadosFakes.KeyGetAnosExerciciosPermitidos);

            var anos = _exercicioRepository.GetAnosSimulados(matricula, getOnline, idAplicacao, idTipoSimulado);

            var produtosContratados = _produtoRepository.GetProdutosContratadosPorAnoMatricula(matricula);

            int ano;
            try
            {
                ano = Utilidades.GetYear();
            }
            catch
            {
                ano = DateTime.Today.Year;
            }
            var produtosAnoCorrente = produtosContratados.FindAll(x => x.Ano == ano);

            return anos;
        }

        public List<int> GetAnosSimuladoR3Ped(int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo anos simulado R3Ped"))
            {
                return GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, matricula, (int)Constants.TipoSimulado.R3_Pediatria, true, idaplicacao);
            }
        }

        public List<int> GetAnosSimuladoR4GO(int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo anos simulado R4GO"))
            {
                return GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, matricula, (int)Constants.TipoSimulado.R4_GO, true, idaplicacao);
            }
        }

        public List<int> GetAnosSimuladoCPMED(int matricula, int idaplicacao)
		{
            using(MiniProfiler.Current.Step("Obtendo anos simulado CPMED"))
            {
			    return GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, matricula, (int)Constants.TipoSimulado.CPMED, true, idaplicacao);
            }
		}

        public List<Exercicio> GetSimuladosR3Cir(int ano, int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo simulado R3Cir"))
            {
                return GetSimuladoEspecialidadesAgrupadas(ano, matricula, idaplicacao, (int)Constants.TipoSimulado.R3_Cirurgia);
            }
        }

        public List<Exercicio> GetSimuladosR3Cli(int ano, int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo anos simulado R3Cli"))
            {
                return GetSimuladoEspecialidadesAgrupadas(ano, matricula, idaplicacao, (int)Constants.TipoSimulado.R3_Clinica);
            }
        }

        public List<Exercicio> GetSimuladosR3Ped(int ano, int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo simulado R3Ped"))
            {
                return GetSimuladoEspecialidadesAgrupadas(ano, matricula, idaplicacao, (int)Constants.TipoSimulado.R3_Pediatria);
            }
        }

        public List<Exercicio> GetSimuladosR4GO(int ano, int matricula, int idaplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo simulado R4GO"))
            {
                return GetSimuladoEspecialidadesAgrupadas(ano, matricula, idaplicacao, (int)Constants.TipoSimulado.R4_GO);
            }
        }

        public List<Exercicio> GetSimuladosCPMED(int ano, int matricula, int idaplicacao)
		{
            using(MiniProfiler.Current.Step("Obtendo simulado CPMED"))
            {
			    return GetSimuladoEspecialidadesAgrupadas(ano, matricula, idaplicacao, (int)Constants.TipoSimulado.CPMED);
            }
		}

        public String GetLinkSimuladoImpresso(int idSimulado, int matricula, int ApplicationID, string environmentRootPath)
        {
            using(MiniProfiler.Current.Step("Obtendo simulado impresso"))
            {
                var log = new LogSimuladoImpresso
                {
                    AplicacaoId = ApplicationID,
                    Matricula = matricula,
                    SimuladoId = idSimulado,
                    AcaoId = 1

                };

                var anoSimulado = _repository.GetSimulado(idSimulado).Ano;                       

                var ret = string.Format(ConfigurationProvider.Get("Settings:SimuladoPDFLinkFormat"), anoSimulado, idSimulado);

                if (!SimuladoExisteS3(idSimulado, anoSimulado.Value))
                {
                    salvarSimuladoImpresso(idSimulado, anoSimulado.Value, environmentRootPath);
                }

                return ret;
            }
        }

        private String salvarSimuladoImpresso(int idSimulado, int ano, string environmentRootPath)
        {
            var arquivo = _exercicioRepository.GetPdfSimuladoImpressa(idSimulado, ano, environmentRootPath);
            var bucketSimuladoPDF = ConfigurationProvider.Get("Settings: BucketSimuladoPDF");
            var diretorio = ConfigurationProvider.Get("Settings: DiretorioSimuladoPDF");

            var arquivoBytes = File.ReadAllBytes(arquivo);

            var amazon = new AmazonManager(ConfigurationProvider.Get("Settings: SimuladoPDFKey"), ConfigurationProvider.Get("Settings: SimuladoPDFSecret"));
            
            var arquivoNome = string.Format("SIMULADO-{0}-{1}.pdf", ano, idSimulado);
            return amazon.UploadFile(bucketSimuladoPDF, diretorio, arquivoBytes, arquivoNome);
        }

        private bool SimuladoExisteS3(int idSimulado, int anoSimulado)
        {
            var bucketSimuladoPDF = ConfigurationProvider.Get("Settings:BucketSimuladoPDF");
            
            var arquivoNome = string.Format("pdf/SIMULADO-{0}-{1}.pdf", anoSimulado, idSimulado);
            return new AmazonManager(ConfigurationProvider.Get("Settings:SimuladoPDFKey") , ConfigurationProvider.Get("Settings:SimuladoPDFSecret"))
                                    .CheckFileExist(bucketSimuladoPDF, arquivoNome);
        }

        public List<SimuladoDTO> GetSimuladosPorAno(int ano)
		{
			return _repository.GetSimuladosPorAno(ano);
		}
        
		public SimuladoDTO GetSimuladoPorId(int id)
		{
			return _repository.GetSimuladoPorId(id);
		}

        public int Alterar(SimuladoDTO modelo)
		{
			if (modelo == null)
			{
				return 0;
			}

			modelo.DataUltimaAtualizacao = DateTime.Now;
			modelo.Online = modelo.DataInicioConsultaRanking.HasValue;

			return _repository.Alterar(modelo);
		}

        public List<TemaSimuladoDTO> GetTemasSimuladoPorAno(int ano)
		{
			var dados = _repository
				.GetTemasSimuladoPorAno(ano);

			if (dados == null || dados.Count == 0)
			{
				return new List<TemaSimuladoDTO>();
			}

			return dados
				.Select(x => new TemaSimuladoDTO()
				{
					ID = x.ID,
					Nome = x.Nome,
					Ano = x.Ano
				})
				.ToList();
		}

		public List<TipoSimuladoDTO> GetTiposSimulado()
		{
			var dados = _repository
				.GetTiposSimulado();

			if (dados == null || dados.Count == 0)
			{
				return new List<TipoSimuladoDTO>();
			}

			return dados
				.Select(x => new TipoSimuladoDTO()
				{
					ID = x.ID,
					Nome = x.Nome
				})
				.ToList();
		}

		public List<EspecialidadeDTO> GetEspecialidadesSimulado()
		{
			var dados = new EspecialidadeEntity()
				.GetAll();

			if (dados == null || dados.Count == 0)
			{
				return new List<EspecialidadeDTO>();
			}

			return dados
				.Select(x => new EspecialidadeDTO()
				{
					ID = x.Id,
					Nome = x.Descricao
				})
				.ToList();
		}

        public Exercicio TryGetInformacaoSimuladoParaHotsite()
        {
            try
            {
                return GetInformacaoSimuladoParaHotsite();
            }
            catch (Exception)
            {
                return new Exercicio();
            }
        }

        public Exercicio GetInformacaoSimuladoParaHotsite()
        {
            List<Banner> bannersAtuaisSendoExibidosRestrita = _bannerRepository.GetBanners(Aplicacoes.AreaRestrita);
            var bannerDeSimuladoExibido = FiltraSomenteBannerDeSimulado(bannersAtuaisSendoExibidosRestrita);

            var simuladoAtual = _repository.GetInformacoesBasicasSimulado(bannerDeSimuladoExibido);

            SetSimuladoAtivoCasoDataInicioPassou(simuladoAtual);

            return simuladoAtual;
        }  

        public Banner FiltraSomenteBannerDeSimulado(List<Banner> banners)
        {
            var bannerDeSimuladoExibido = banners != null ? banners.Where(b => b.IdSimulado != null && b.IdSimulado > 0).FirstOrDefault() : new Banner();            
            return bannerDeSimuladoExibido;
        }

        public Exercicio SetSimuladoAtivoCasoDataInicioPassou(Exercicio simulado)
        {
            var dataAtual = DateTime.Now;
            var isSimuladoAgendadoEmAndamento = Utilidades.ConvertFromUnixTimestamp(simulado.DataInicio) <= dataAtual;
            simulado.Ativo = isSimuladoAgendadoEmAndamento;
            return simulado;
        }

        public List<Exercicio> GetSimuladosCPMedExtensivo(int ano, int matricula, int idaplicacao)
        {
            return GetSimuladoEspecialidadesAgrupadas(ano, matricula, idaplicacao, (int)Constants.TipoSimulado.CPMedExtensivo);
        }

        public List<int> GetAnosSimuladoCPMedExtensivo(int matricula, int idaplicacao)
        {
            return GetAnosExerciciosPermitidos(Exercicio.tipoExercicio.SIMULADO, matricula, (int)Constants.TipoSimulado.CPMedExtensivo, true, idaplicacao);
        }

    }
}