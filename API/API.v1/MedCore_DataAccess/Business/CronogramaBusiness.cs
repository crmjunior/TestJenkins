using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Business.Enums;
using System.Data;
using static MedCore_DataAccess.Repository.CronogramaEntity;
using MedCore_DataAccess.DTO.Base;
using StackExchange.Profiling;
using Medgrupo.DataAccessEntity;

namespace MedCore_DataAccess.Business
{
    public class CronogramaBusiness
    {
        private readonly IAulaEntityData _aulaRepository;
        private readonly IMednetData _mednetRepository;
        private readonly IMaterialApostilaData _materialApostilaRepository;
        private readonly IRevalidaData _revalidaRepository;
        private readonly ICronogramaData _cronogramaRepository;
        private readonly ITurmaData _turmaRepository;
        public IFilialData _filialRepository;

        public CronogramaBusiness(IAulaEntityData aulaRepository, IMednetData mednetRepository, IMaterialApostilaData materialApostilaRepository, IRevalidaData revalidaRepository, ICronogramaData cronogramaRepository)
        {
            _aulaRepository = aulaRepository;
            _mednetRepository = mednetRepository;
            _materialApostilaRepository = materialApostilaRepository;
            _revalidaRepository = revalidaRepository;
            _cronogramaRepository = cronogramaRepository;
            _turmaRepository = new TurmaEntity();
        }

        #region Progressos

        public ResponseDTO<List<SemanaProgressoPermissao>> GetProgressos(int idProduto, int matricula, int ano = 0)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetProgressos))
                return RedisCacheManager.GetItemObject<ResponseDTO<List<SemanaProgressoPermissao>>>(RedisCacheConstants.DadosFakes.KeyGetProgressos);

            try
            {
                var semanaProgressoPermissao = new List<SemanaProgressoPermissao>();
                var menus = new Dictionary<Semana.TipoAba, ESubMenus>
                {
                    { Semana.TipoAba.Aulas, ESubMenus.Aulas },
                    { Semana.TipoAba.Materiais, ESubMenus.Materiais },
                    { Semana.TipoAba.Questoes, ESubMenus.Questoes },
                    { Semana.TipoAba.Revalida, ESubMenus.Revalida }
                };

                foreach (var item in menus)
                {
                    semanaProgressoPermissao.Add(new SemanaProgressoPermissao
                    {
                        MenuId = (int)item.Value,
                        ProgressoSemanas = GetPercentSemanas(ano, matricula, idProduto, item.Key)
                    });
                }
                return new ResponseDTO<List<SemanaProgressoPermissao>> { Retorno = semanaProgressoPermissao, Sucesso = true };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<SemanaProgressoPermissao>> { Sucesso = false, Mensagem = ex.Message };
            }
        }

        public List<ProgressoSemana> GetPercentSemanas(int ano, int matricula, int produto, Semana.TipoAba aba)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetPercentSemanas))
                return RedisCacheManager.GetItemObject<List<ProgressoSemana>>(RedisCacheConstants.DadosFakes.KeyGetPercentSemanas);

            if (ano == 0)
                ano = Utilidades.GetYear();

            switch (aba)
            {
                case Semana.TipoAba.Aulas:

                    if (Utilidades.CursosAulasEspeciais().Contains(produto))
                        produto = Utilidades.GetCursoOrigemCursoAulaEspecial(produto);

                    List<ProgressoSemana> progressoAulas = _mednetRepository.GetProgressoAulas(matricula, ano, produto);

                    if (produto == (int)Produto.Cursos.CPMED)
                    {
                        progressoAulas.AddRange(_mednetRepository.GetProgressoAulas(matricula, ano, (int)Produto.Cursos.MED));
                    }

                    return progressoAulas;
                case Semana.TipoAba.Materiais:
                case Semana.TipoAba.Checklists:
                    if (produto == (int)Produto.Cursos.MEDELETRO_IMED)
                    {
                        return _materialApostilaRepository.GetProgressoMaterial(matricula, ano, (int)Produto.Cursos.MEDELETRO);
                    }
                    else
                    {
                        return _materialApostilaRepository.GetProgressoMaterial(matricula, ano, produto);
                    }
                case Semana.TipoAba.Questoes:
                    return GetProgressoQuestoes(matricula, ano, produto);
                case Semana.TipoAba.Revalida:
                    return _revalidaRepository.GetProgressoRevalida(matricula, produto);
                default:
                    return null;
            }
        }

        public List<ProgressoSemana> GetProgressoQuestoes(int matricula, int ano, int produto)
        {
            var lstProgressoSemana = GetListaQuestoesProgresso(matricula, ano, produto);

            return lstProgressoSemana;
        }

        public List<ProgressoSemana> GetListaQuestoesProgresso(int matricula, int ano, int produto)
        {
            var listaProgressoSemana = new List<ProgressoSemana>();

            var produtosNaoEntram = new int[] { (int)Utilidades.ProductGroups.INTENSIVO, (int)Utilidades.ProductGroups.APOSTILA_MEDELETRON };

            var listaQuestoesApostila = _aulaRepository.GetQuestoesApostila_PorAnoProduto(ano, produto);

            var respostas = _aulaRepository.GetRespostas_PorMatricula(matricula);

            var groupListQuestoes = (from resp in respostas
                                     join lq in listaQuestoesApostila on resp equals lq.QuestaoID
                                     select new
                                     {
                                         intQuestao = lq.QuestaoID,
                                         intQuestaoResposta = resp,
                                         lq.ExercicioID
                                     }).GroupBy(x => x.ExercicioID).ToList();


            foreach (var q in groupListQuestoes)
            {
                var qtdQuestoes = listaQuestoesApostila.Where(x => q.Key == x.ExercicioID).Count();

                decimal percentual = CalcularPercentual_QuestoesRealizadas(qtdQuestoes, q.Count());
                ProgressoSemana p = new ProgressoSemana
                {
                    IdEntidade = q.Key,
                    PercentLido = (int)Math.Ceiling(percentual)
                };
                listaProgressoSemana.Add(p);
            }

            return listaProgressoSemana;
        }

        public decimal CalcularPercentual_QuestoesRealizadas(int totalQuestoes, int totalQuestoesRealizadas)
        {
            return ((100 * totalQuestoesRealizadas) / totalQuestoes);
        }

        #endregion

        #region Cronograma
        public CronogramaSemana GetCronogramaAluno(int idProduto, int ano, int menuId, int matricula = 0, string versaoApp = "", int idAplicacao = (int)Aplicacoes.MsProMobile)
        {
            const string VERSAO_APP_TROCA_LAYOUT_MASTOLOGIA = "5.2.0";
            versaoApp = string.IsNullOrEmpty(versaoApp) ? "0.0.0" : versaoApp;
            List<int> idProdutoAulao = new List<int>() { (int)Produto.Cursos.R3Cirurgia, (int)Produto.Cursos.R3Pediatria , (int)Produto.Cursos.R3Clinica
                , (int)Produto.Cursos.R4GO };
            List<int> idProdutoAulaoMastoTEGO = new List<int>() { (int)Produto.Cursos.TEGO, (int)Produto.Cursos.MASTO };


            var cronograma = new CronogramaSemana();
            var tipoLayout = GetTipoLayout(idProduto, menuId);

            if ((idProduto == (int)Produto.Cursos.MASTO || idProduto == (int)Produto.Cursos.TEGO) && Utilidades.VersaoMenorOuIgual(versaoApp, VERSAO_APP_TROCA_LAYOUT_MASTOLOGIA))
                tipoLayout = Enums.TipoLayoutMainMSPro.WEEK_SINGLE;

            if (idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON && Utilidades.CursosAulasEspeciais().Contains(idProduto))
                tipoLayout = Enums.TipoLayoutMainMSPro.WEEK_DOUBLE;

            if (matricula != 0 && ano == 0)
                ano = _cronogramaRepository.UltimoAnoCursadoAluno(matricula, idProduto);

            if (idProdutoAulao.Contains(idProduto) &&
                    (
                        (
                            (idAplicacao == (int)Aplicacoes.MsProMobile
                            && Utilidades.VersaoMenorOuIgual(versaoApp, ConfigurationProvider.Get("Settings:VersaoAppTrocaLayoutAuloesSR3R4"))
                            )
                            || !Convert.ToBoolean(ConfigurationProvider.Get("Settings:AtivaAuloesSR3R4"))
                        )
                        || idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON
                    )
               )
                tipoLayout = Enums.TipoLayoutMainMSPro.WEEK_SINGLE;


            if (idProdutoAulaoMastoTEGO.Contains(idProduto) && (
                    ((idAplicacao == (int)Aplicacoes.MsProMobile && Utilidades.VersaoMenorOuIgual(versaoApp,
                     ConfigurationProvider.Get("Settings:VersaoAppTrocaLayoutAuloesSR3R4"))) || !Convert.ToBoolean(ConfigurationProvider.Get("Settings:AtivaAuloesSR3R4")))
                    || idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON))
                tipoLayout = Enums.TipoLayoutMainMSPro.WEEK;

            if (idProdutoAulaoMastoTEGO.Contains(idProduto) && idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON)
                tipoLayout = Enums.TipoLayoutMainMSPro.WEEK_SINGLE;


            switch (tipoLayout)
            {
                case Enums.TipoLayoutMainMSPro.WEEK_DOUBLE:
                case Enums.TipoLayoutMainMSPro.WEEK:
                    cronograma.Semanas = GetSemanaDuplaCronograma(idProduto, ano, matricula, versaoApp, idAplicacao);
                    break;
                case Enums.TipoLayoutMainMSPro.WEEK_SINGLE:
                    cronograma.Semanas = GetSemanaUnicaCronograma(idProduto, ano, matricula);
                    break;
                case Enums.TipoLayoutMainMSPro.SHELF:
                    cronograma.Prateleiras = GetCronogramaPrateleiras(idProduto, ano, matricula, menuId);
                    break;
                case Enums.TipoLayoutMainMSPro.REVALIDA:
                    cronograma.Revalida = _cronogramaRepository.RevalidaCronogramaPermissao(_cronogramaRepository.GetRevalidaCronograma(idProduto, ano), matricula);
                    break;
                case Enums.TipoLayoutMainMSPro.MIXED:
                    cronograma.Dinamico = GetCronogramaDinamico(idProduto, ano, matricula, menuId);
                    break;
                default:
                    break;
            }
            cronograma.Tipo = GetTipoAba(menuId);

            return cronograma;
        }
        public List<CronogramaPrateleira> GetCronogramaPrateleiras(int idProduto, int ano, int matricula, int menuId)
        {
            try
            {
                var cronograma = _cronogramaRepository.GetCronogramaPrateleirasAsync(idProduto, ano, matricula, menuId);

                var apostilasLiberadas = GetConteudoLiberado(matricula, menuId, idProduto);

                var progressos = GetProgressoConteudo(idProduto, ano, matricula, menuId);

                var entidadesBloqueadas = _cronogramaRepository.GetBookEntitiesBloqueadosNoCronograma();

                var codigosApostilas = _cronogramaRepository.GetCodigosAmigaveisApostilas();

                var acessoAntecipado = Utilidades.PossuiMaterialAntecipado(matricula);

                cronograma.Wait();

                if (idProduto == (int)Produto.Cursos.CPMED)
                {
                    var cronogramaTurmaConvidadaMed = GetCronogramaPrateleirasCPMEDTurmaConvidada(ano, menuId, matricula);
                    if (cronogramaTurmaConvidadaMed != null) cronograma.Result.AddRange(cronogramaTurmaConvidadaMed);
                }

                cronograma.Result.RemoveAll(x => entidadesBloqueadas.Contains(x.EntidadeID));

                var prateleiras = cronograma.Result.Where(p => !p.ExibeConformeCronograma || (p.ExibeConformeCronograma && p.Data <= DateTime.Now))
                    .GroupBy(x => new { x.ID, x.Descricao, x.Ordem, x.ExibeEspecialidade, x.ExibeConformeCronograma })
                                                            .Select(s => new CronogramaPrateleira
                                                            {
                                                                Id = s.Key.ID,
                                                                Titulo = s.Key.Descricao,
                                                                Numero = s.Key.Ordem,
                                                                DataInicio = string.Empty,
                                                                DataFim = string.Empty,
                                                                Apostilas = s.GroupBy(apostila => new { apostila.MaterialId, apostila.LessonTitleID }).Select(grp => grp.FirstOrDefault()).Select(y => new Apostila
                                                                {
                                                                    IdEntidade = (int)y.EntidadeID,
                                                                    MaterialId = y.MaterialId,
                                                                    Nome = GetCodigoAmigavelApostila(y.MaterialId, y.LessonTitleID, y.EntidadeCodigo, codigosApostilas),
                                                                    EspecialidadeCodigo = Utilidades.GetEspecialidadePorProductGroup1(y.EspecialidadeId),
                                                                    Ano = y.Ano,
                                                                    Aprovada = GetAprovacaoApostilas(y.Data, apostilasLiberadas, y.MaterialId, menuId, acessoAntecipado),
                                                                    ExibeEspecialidade = s.Key.ExibeEspecialidade,
                                                                    PercentLido = CalculaPercentualProgresso(progressos, y.MaterialId),
                                                                    Temas = new List<AulaTema>
                                                                    {
                                                                        new AulaTema
                                                                        {
                                                                            TemaID = y.LessonTitleID,
                                                                            PodeAvaliar = false,
                                                                            Data = y.Data.ToString()
                                                                        }
                                                                    }
                                                                }).OrderBy(a => a.MaterialId).Distinct().ToList()
                                                            }).OrderBy(o => o.Numero).ToList();

                return prateleiras;
            }
            catch (Exception ex)
            {
                return new List<CronogramaPrateleira>();

            }

        }

        public bool GetAprovacaoApostilas(DateTime dataCronograma, List<int> apostilasAprovadas, int materialId, int menuId, bool acessoAntecipado)
        {
            switch (menuId)
            {
                case (int)ESubMenus.Aulas:
                    return (acessoAntecipado || DateTime.Now > dataCronograma);
                case (int)ESubMenus.Materiais:
                    return ((DateTime.Now > dataCronograma || acessoAntecipado) && apostilasAprovadas.Any(x => x == materialId));
                case (int)ESubMenus.Checklists:
                    return (apostilasAprovadas.Any(x => x == materialId));
                default:
                    return false;
            }
        }

        public List<CronogramaPrateleiraDTO> GetCronogramaPrateleirasCPMEDTurmaConvidada(int ano, int menuId, int matricula)
        {
            var turmaConvidada = GetTurmaConvidada(matricula, ano);
            var cronograma = _cronogramaRepository.GetCronogramaPrateleirasCPMEDTurmaConvidada(ano, menuId, turmaConvidada);

            if (menuId == (int)ESubMenus.Aulas)
            {
                var temas = _cronogramaRepository.GetTemasAnosAnteriores(cronograma.Select(x => x.Nome).ToList());

                foreach (var item in cronograma)
                {
                    var tema = temas.FirstOrDefault(x => x.Nome.Trim() == item.Nome.Trim());
                    if (tema != null)
                        item.LessonTitleID = tema.TemaID;
                }
            }

            return cronograma;
        }

        public string GetCodigoAmigavelApostila(int MaterialId, int LessonTitleId, string entidadeCodigo, List<ApostilaCodigoDTO> codigos, int numero = 0)
        {
            const int SIZE = 9;
            const int START = 4;
            var codigoAmigavel = codigos.FirstOrDefault(x => (x.ProdutoId == MaterialId && x.TemaId == LessonTitleId)
                                                          || (x.ProdutoId == MaterialId && x.TemaId == -1)
                                                          || (x.TemaId == LessonTitleId && x.ProdutoId == -1)
                                                          );

            if (codigoAmigavel != null)
            {
                codigoAmigavel.Nome = codigoAmigavel.Nome.Replace("#NUMERO", numero.ToString());
                return codigoAmigavel.Nome;
            }
            else if (entidadeCodigo.Length >= START + SIZE)
                return entidadeCodigo.Substring(START, SIZE).Trim().ToUpper();
            else
                return entidadeCodigo.Trim().ToUpper();
        }

        public bool GetAprovacaoApostilas(DateTime dataCronograma, List<MaterialLiberacaoDTO> apostilasAprovadas, int materialId, int menuId, bool acessoAntecipado)
        {
            switch (menuId)
            {
                case (int)ESubMenus.Aulas:
                    return (acessoAntecipado || DateTime.Now > dataCronograma);
                case (int)ESubMenus.Materiais:
                    return ((DateTime.Now > dataCronograma || acessoAntecipado) && apostilasAprovadas.Any(x => x.ProductId == materialId));
                case (int)ESubMenus.Checklists:
                    return ((apostilasAprovadas.Any(x => x.ProductId == materialId && x.LiberacaoAutomatica))
                            || ((acessoAntecipado || DateTime.Now > dataCronograma) && apostilasAprovadas.Any(x => x.ProductId == materialId && !x.LiberacaoAutomatica)));
                default:
                    return false;
            }
        }

        public bool GetAprovacaoApostilasPorLayout(DateTime dataCronograma, List<MaterialLiberacaoDTO> apostilasAprovadas, int materialId, bool acessoAntecipado, Enums.TipoLayoutMainMSPro layout)
        {
            switch (layout)
            {
                case TipoLayoutMainMSPro.PLAYLIST:
                    return (acessoAntecipado || DateTime.Now > dataCronograma);
                case TipoLayoutMainMSPro.WEEK:
                case TipoLayoutMainMSPro.LIST:
                case TipoLayoutMainMSPro.WEEK_SINGLE:
                case TipoLayoutMainMSPro.WEEK_DOUBLE:
                    return ((DateTime.Now > dataCronograma || acessoAntecipado) && apostilasAprovadas.Any(x => x.ProductId == materialId));
                case TipoLayoutMainMSPro.SHELF:
                case TipoLayoutMainMSPro.MIXED:
                    return ((apostilasAprovadas.Any(x => x.ProductId == materialId && x.LiberacaoAutomatica))
                            || ((acessoAntecipado || DateTime.Now > dataCronograma) && apostilasAprovadas.Any(x => x.ProductId == materialId && !x.LiberacaoAutomatica)));
                default:
                    return false;
            }
        }

        public int GetTurmaConvidada(int matricula, int ano)
        {
            var turmaRepository = new TurmaEntity();

            int turmaConvidada = turmaRepository.TurmaConvidadaAluno(matricula, ano).ID;

            if (turmaConvidada == 0)
            {
                turmaConvidada = turmaRepository.TurmaMedAluno(matricula, ano).ID;
            }

            return turmaConvidada;
        }

        public List<MaterialLiberacaoDTO> GetConteudoLiberado(int matricula, int menuId, int idProduto)
        {
            var menu = (ESubMenus)menuId;

            switch (menu)
            {
                case ESubMenus.Materiais:
                case ESubMenus.Aulas:
                    return GetMateriaisApostilaLiberados(matricula);
                case ESubMenus.Checklists:
                    return GetChecklistsLiberados(matricula, idProduto);
                default:
                    return new List<MaterialLiberacaoDTO>();
            }
        }

        public List<MaterialLiberacaoDTO> GetMateriaisApostilaLiberados(int matricula)
        {
            var apostilasLiberacao = _aulaRepository.RetornaApostilasDeAcordoComMatricula(matricula);
            return apostilasLiberacao.Where(w => w.intBookId.HasValue)
                                    .Select(x => new MaterialLiberacaoDTO
                                    {
                                        ProductId = x.intBookId.Value,
                                        LiberacaoAutomatica = false
                                    }).ToList();
        }

        public List<MaterialLiberacaoDTO> GetChecklistsLiberados(int matricula, int idProduto)
        {
            List<MaterialChecklistDTO> checklists = new List<MaterialChecklistDTO>();
            var checklistsExtras = _cronogramaRepository.GetChecklistsExtrasLiberados(matricula, idProduto);
            var checklistsPraticos = _cronogramaRepository.GetChecklistsPraticosLiberados(matricula, idProduto);

            if (checklistsExtras != null)
            {
                checklists.AddRange(checklistsExtras);
            }

            if (checklistsPraticos != null)
            {
                checklists.AddRange(checklistsPraticos);
            }

            var apostilasLiberacao = _aulaRepository.RetornaApostilasDeAcordoComMatricula(matricula).Select(x => x.intBookId ?? 0).ToList();
            var checklistsLiberados = checklists.Where(x => x.ProductId.HasValue
                                                        && (x.ProductGroup3Id == (int)Utilidades.EProductsGroup1.CursoPratico
                                                            || (x.ProductGroup3Id != (int)Utilidades.EProductsGroup1.CursoPratico && apostilasLiberacao.Contains(x.ProductId ?? 0))
                                                           )).Select(m => new MaterialLiberacaoDTO
                                                           {
                                                               ProductId = m.ProductId.Value,
                                                               LiberacaoAutomatica = m.ProductGroup3Id == (int)Utilidades.EProductsGroup1.CursoPratico
                                                           }).ToList();

            return checklistsLiberados;
        }

        public List<ProgressoApostilaDTO> GetProgressoConteudo(int idProduto, int ano, int matricula, int menuId)
        {
            var percentuais = GetPercentSemanas(ano, matricula, idProduto, GetTipoAba(menuId));

            var progressos = percentuais.Select(x => new ProgressoApostilaDTO { IdEntidade = x.IdEntidade, PercentLido = x.PercentLido }).ToList();

            return progressos;
        }

        public int CalculaPercentualProgresso(List<ProgressoApostilaDTO> progressos, int entidadeId)
        {
            var percentual = progressos.FirstOrDefault(x => x.IdEntidade == entidadeId);
            return percentual != null ? percentual.PercentLido : 0;
        }

        public List<Semana> GetSemanaDuplaCronograma(int idProduto, int ano, int matricula, string versaoApp = "", int idAplicacao = (int)Aplicacoes.MsProMobile)
        {
            try
            {
                var lSemanas = new List<Semana>();
                var semanas = new List<msp_API_ListaEntidades_Result>();
                var semanasLiberadas = new List<msp_API_ListaEntidades_Result>();
                List<int> idProdutoAulao = new List<int>() { (int)Produto.Cursos.R3Cirurgia, (int)Produto.Cursos.R3Pediatria , (int)Produto.Cursos.R3Clinica
                , (int)Produto.Cursos.R4GO, (int)Produto.Cursos.TEGO, (int)Produto.Cursos.MASTO };
                if (Utilidades.CursosAulasEspeciais().Contains(idProduto))
                {
                    const int LIMITE_DIAS_PADRAO = 7;
                    semanas = GetListaEntidadesPorMatriculaBase(idProduto, ano, matricula);
                    var diasLimite = GetDiasLimiteTurmaExcecao(idProduto, ano, matricula);
                    var cronoProd = _cronogramaRepository.GetListaEntidades(Utilidades.GetCursoOrigemCursoAulaEspecial(idProduto), ano, matricula);
                    var dataCronogramaMateriais = GetDateCronogramaLessonTitle(cronoProd);

                    foreach (var itemSemana in semanas)
                    {
                        if (dataCronogramaMateriais.Any(x => x.Key == itemSemana.intLessonTitleID))
                        {
                            var materialCronograma = dataCronogramaMateriais.FirstOrDefault(x => x.Key == itemSemana.intLessonTitleID);
                            var dataInicio = materialCronograma.Value;
                            var dataFim = (diasLimite == 0 ? dataInicio.AddDays(LIMITE_DIAS_PADRAO) : dataInicio.AddDays(diasLimite));

                            if (dataInicio <= DateTime.Now && (diasLimite == 0 || (diasLimite != 0 && dataFim >= DateTime.Now)))
                            {
                                itemSemana.dataInicio = dataInicio.ToString("dd/MM");
                                itemSemana.datafim = dataFim.ToString("dd/MM");
                                semanasLiberadas.Add(itemSemana);
                            }
                        }
                    }
                    semanas = semanasLiberadas;
                }
                else
                {
                    if (versaoApp == "")
                        versaoApp = "5.5.0";

                    if (idAplicacao == (int)Aplicacoes.MsProMobile && Utilidades.VersaoMaiorOuIgual(versaoApp, "5.5.0"))
                        semanas = _cronogramaRepository.GetListaEntidades(idProduto, ano, Constants.CONTACTID_ACADEMICO);
                    else
                        semanas = GetListaEntidadesPorMatriculaBase(idProduto, ano, matricula);

                    if (idProduto == (int)Produto.Cursos.MASTO || idProduto == (int)Produto.Cursos.TEGO)
                    {
                        semanas = semanas.OrderBy(x => x.intSemana).ThenBy(a => a.entidade).ToList();
                    }
                }
                if (idProdutoAulao.Contains(idProduto))
                {
                    semanas = semanas.Where(x => x.intLessonSubjectID != Constants.intSubjetctId_AulaoRMAIS).ToList();
                }
                lSemanas = BuildSemanaDuplaCronograma(semanas);
                return lSemanas;
            }
            catch (Exception ex)
            {
                return new List<Semana>();
            }

        }

        public List<Semana> GetSemanaUnicaCronograma(int idProduto, int ano, int matricula)
        {
            try
            {
                var lSemanas = new List<Semana>();
                List<int> idProdutoAulao = new List<int>() { (int)Produto.Cursos.R3Cirurgia, (int)Produto.Cursos.R3Pediatria , (int)Produto.Cursos.R3Clinica
                , (int)Produto.Cursos.R4GO, (int)Produto.Cursos.TEGO, (int)Produto.Cursos.MASTO };

                var matriculaCronograma = Utilidades.IsIntensivo(idProduto) ? matricula : Constants.CONTACTID_ACADEMICO;

                var semanas = _cronogramaRepository.GetListaEntidades(idProduto, ano, matriculaCronograma);
                if (idProdutoAulao.Contains(idProduto))
                {
                    semanas = semanas.Where(x => x.intLessonSubjectID != Constants.intSubjetctId_AulaoRMAIS).ToList();
                }
                lSemanas = BuildSemanaUnicaCronograma(semanas);

                return lSemanas;
            }
            catch (Exception ex)
            {
                return new List<Semana>();
            }
        }

        public List<CronogramaDinamicoDTO> GetCronogramaDinamico(int idProduto, int ano, int matricula, int menuId)
        {
            var cronogramaMateriais = new List<msp_API_ListaEntidades_Result>();
            List<int> idProdutoAulao = new List<int>() { (int)Produto.Cursos.R3Cirurgia, (int)Produto.Cursos.R3Pediatria , (int)Produto.Cursos.R3Clinica
                , (int)Produto.Cursos.R4GO };
            List<int> idProdutoAulaoMastoTEGO = new List<int>() { (int)Produto.Cursos.TEGO, (int)Produto.Cursos.MASTO };

            if (Utilidades.CursosAulasEspeciais().Contains(idProduto))
                cronogramaMateriais = GetListaEntidadesPorMatriculaBase(idProduto, ano, matricula);
            else
                cronogramaMateriais = _cronogramaRepository.GetListaEntidades(idProduto, ano, matricula);

            var configMateriais = _cronogramaRepository.GetConfiguracaoMateriaisEntidades(menuId, idProduto);
            var cronogramaDinamico = new List<CronogramaDinamicoDTO>();

            var cronogramaDinamicoGroup = cronogramaMateriais.Join(configMateriais, mat => mat.intLessonTitleID, config => config.LessonTitleID, (mat, config) => new { Material = mat, Tipo = config.TipoLayout })
                                                                   .GroupBy(x => x.Tipo)
                                                                   .ToDictionary(x => x.Key, x => x.Select(z => z.Material).ToList());
            var dataCronogramaMateriais = new Dictionary<int, DateTime>();
            if (Utilidades.CursosAulasEspeciais().Contains(idProduto))
            {
                var cronoProd = _cronogramaRepository.GetListaEntidades(Utilidades.GetCursoOrigemCursoAulaEspecial(idProduto), ano, matricula);
                dataCronogramaMateriais = GetDateCronogramaMateriais(cronoProd);
            }
            else
                dataCronogramaMateriais = GetDateCronogramaMateriais(cronogramaMateriais);

            foreach (var item in cronogramaDinamicoGroup)
            {
                switch (item.Key)
                {
                    case Enums.TipoLayoutMainMSPro.WEEK:
                    case Enums.TipoLayoutMainMSPro.WEEK_DOUBLE:
                        var cronograma = Utilidades.MapListToOtherList<List<Semana>, CronogramaMixed>(BuildSemanaDuplaCronograma(item.Value));
                        cronogramaDinamico.Add(new CronogramaDinamicoDTO { TipoLayout = item.Key, Itens = BuildPermissaoProgresso(cronograma, matricula, menuId, idProduto, dataCronogramaMateriais, item.Key, ano) });
                        break;
                    case Enums.TipoLayoutMainMSPro.LIST:
                        var cronogramaList = Utilidades.MapListToOtherList<List<Semana>, CronogramaMixed>(BuildSemanaUnicaCronograma(item.Value));
                        cronogramaDinamico.Add(new CronogramaDinamicoDTO { TipoLayout = item.Key, Itens = BuildList(cronogramaList, matricula, menuId, idProduto, dataCronogramaMateriais, item.Key, ano) });
                        break;
                    case Enums.TipoLayoutMainMSPro.WEEK_SINGLE:
                        var cronogramaSingle = Utilidades.MapListToOtherList<List<Semana>, CronogramaMixed>(BuildSemanaUnicaCronograma(item.Value));
                        cronogramaDinamico.Add(new CronogramaDinamicoDTO { TipoLayout = item.Key, Itens = BuildPermissaoProgresso(cronogramaSingle, matricula, menuId, idProduto, dataCronogramaMateriais, item.Key, ano) });
                        break;
                    case Enums.TipoLayoutMainMSPro.SHELF:
                        cronogramaDinamico.Add(new CronogramaDinamicoDTO { TipoLayout = item.Key, Itens = Utilidades.MapListToOtherList<List<CronogramaPrateleira>, CronogramaMixed>(GetCronogramaPrateleiras(idProduto, ano, matricula, menuId)) });
                        break;
                    case Enums.TipoLayoutMainMSPro.PLAYLIST:
                        cronogramaDinamico.Add(new CronogramaDinamicoDTO { TipoLayout = item.Key, Itens = Utilidades.MapListToOtherList<List<CronogramaPlaylistDTO>, CronogramaMixed>(BuildPlaylist(idProduto, matricula, item.Value, menuId, dataCronogramaMateriais, item.Key, ano)) });
                        break;
                    default:
                        break;
                }
            }
            if (idProdutoAulao.Contains(idProduto) && (menuId == (int)ESubMenus.Questoes || menuId == (int)ESubMenus.Materiais))
            {

                cronogramaMateriais = cronogramaMateriais.Where(x => x.intLessonSubjectID != Constants.intSubjetctId_AulaoRMAIS).ToList();
                var cronograma = Utilidades.MapListToOtherList<List<Semana>, CronogramaMixed>(BuildSemanaDuplaCronograma(cronogramaMateriais));
                cronogramaDinamico.Add(new CronogramaDinamicoDTO { TipoLayout = Enums.TipoLayoutMainMSPro.WEEK_SINGLE, Itens = BuildPermissaoProgresso(cronograma, matricula, menuId, idProduto, dataCronogramaMateriais, Enums.TipoLayoutMainMSPro.WEEK_SINGLE, ano) });

            }
            if (idProdutoAulaoMastoTEGO.Contains(idProduto) && (menuId == (int)ESubMenus.Questoes || menuId == (int)ESubMenus.Materiais))
            {
                cronogramaMateriais = cronogramaMateriais.Where(x => x.intLessonSubjectID != Constants.intSubjetctId_AulaoRMAIS).ToList();
                var cronograma = Utilidades.MapListToOtherList<List<Semana>, CronogramaMixed>(BuildSemanaDuplaCronograma(cronogramaMateriais));
                cronogramaDinamico.Add(new CronogramaDinamicoDTO { TipoLayout = Enums.TipoLayoutMainMSPro.WEEK, Itens = BuildPermissaoProgresso(cronograma, matricula, menuId, idProduto, dataCronogramaMateriais, Enums.TipoLayoutMainMSPro.WEEK, ano) });

            }
            return cronogramaDinamico;
        }

        private List<CronogramaMixed> BuildPermissaoProgresso(List<CronogramaMixed> cronogramaMixed, int matricula, int menuId, int idProduto, IDictionary<int, DateTime> dataCronogramaMateriais, Enums.TipoLayoutMainMSPro layout, int ano)
        {
            var apostilasLiberadas = GetConteudoLiberado(matricula, menuId, idProduto);
            var acessoAntecipado = Utilidades.PossuiMaterialAntecipado(matricula);
            var progressos = GetProgressoConteudo(idProduto, ano, matricula, menuId);

            foreach (var crono in cronogramaMixed)
            {
                foreach (var apostila in crono.Apostilas)
                {
                    apostila.PercentLido = CalculaPercentualProgresso(progressos, apostila.MaterialId);
                    apostila.Aprovada = GetAprovacaoApostilasPorLayout(dataCronogramaMateriais[apostila.MaterialId], apostilasLiberadas, apostila.MaterialId, acessoAntecipado, layout);
                }
            }

            return cronogramaMixed;
        }

        private List<CronogramaMixed> BuildList(List<CronogramaMixed> cronogramaMixed, int matricula, int menuId, int idProduto, IDictionary<int, DateTime> dataCronogramaMateriais, Enums.TipoLayoutMainMSPro layout, int ano)
        {
            var apostilasLiberadas = GetConteudoLiberado(matricula, menuId, idProduto);
            var acessoAntecipado = Utilidades.PossuiMaterialAntecipado(matricula);
            var progressos = GetProgressoConteudo(idProduto, ano, matricula, menuId);
            var codigosApostilas = _cronogramaRepository.GetCodigosAmigaveisApostilas();

            foreach (var crono in cronogramaMixed)
            {
                foreach (var apostila in crono.Apostilas)
                {
                    apostila.PercentLido = CalculaPercentualProgresso(progressos, apostila.MaterialId);
                    apostila.Aprovada = GetAprovacaoApostilasPorLayout(dataCronogramaMateriais[apostila.MaterialId], apostilasLiberadas, apostila.MaterialId, acessoAntecipado, layout);

                    var nomeAmigavel = GetCodigoAmigavelApostila(apostila.MaterialId, apostila.Temas.FirstOrDefault().TemaID, apostila.Nome, codigosApostilas);
                    apostila.FiltroConteudo.TituloApostila = nomeAmigavel;
                    apostila.Nome = nomeAmigavel;
                }
            }

            return cronogramaMixed;
        }

        private List<CronogramaPlaylistDTO> BuildPlaylist(int idProduto, int matricula, List<msp_API_ListaEntidades_Result> cronograma, int menuId, IDictionary<int, DateTime> dataCronogramaMateriais, Enums.TipoLayoutMainMSPro layout, int ano)
        {
            List<CronogramaPlaylistDTO> playlist = new List<CronogramaPlaylistDTO>();

            var apostilasLiberadas = new List<MaterialLiberacaoDTO>();
            var diasLimite = 0;
            var cronogramaLessonTitles = new Dictionary<int, DateTime>();

            if (Utilidades.CursosAulasEspeciais().Contains(idProduto))
            {
                var produtoOrigem = Utilidades.GetCursoOrigemCursoAulaEspecial(idProduto);
                apostilasLiberadas = GetConteudoLiberado(matricula, menuId, produtoOrigem);
                var cronoProd = _cronogramaRepository.GetListaEntidades(produtoOrigem, ano, matricula);
                diasLimite = GetDiasLimiteTurmaExcecao(idProduto, ano, matricula);
                cronogramaLessonTitles = GetDateCronogramaLessonTitle(cronoProd);
            }
            else
                apostilasLiberadas = GetConteudoLiberado(matricula, menuId, idProduto);

            var acessoAntecipado = Utilidades.PossuiMaterialAntecipado(matricula);

            foreach (var item in cronograma)
            {
                if (diasLimite != 0
                    && DateTime.Now >= cronogramaLessonTitles[item.intLessonTitleID].AddDays(diasLimite))
                {
                    continue;
                }

                if (Utilidades.CursosAulasEspeciais().Contains(idProduto) && (!cronogramaLessonTitles.Any(k => k.Key == item.intLessonTitleID)
                    || DateTime.Now < cronogramaLessonTitles[item.intLessonTitleID]))
                {
                    continue;
                }

                var codigosApostilas = _cronogramaRepository.GetCodigosAmigaveisApostilas();
                var nomeAmigavel = GetCodigoAmigavelApostila(item.intMaterialID, item.intLessonTitleID, item.txtCode, codigosApostilas, item.intSemana ?? 0);

                var videos = GetVideosPorTipo(idProduto, matricula, item.intLessonTitleID, (int)Aplicacoes.MsProMobile, menuId);

                List<CronogramaPlaylistVideosDTO> playlistVideos = new List<CronogramaPlaylistVideosDTO>();

                if (videos.Videos != null)
                {
                    var aprovada = GetAprovacaoApostilasPorLayout(dataCronogramaMateriais[item.intMaterialID], apostilasLiberadas, item.intMaterialID, acessoAntecipado, layout);

                    foreach (var video in videos.Videos)
                    {
                        playlistVideos.Add(new CronogramaPlaylistVideosDTO
                        {
                            Ativo = aprovada && video.Ativo,
                            DuracaoFormatada = video.DuracaoFormatada,
                            ID = video.ID,
                            Assistido = video.Assistido,
                            PalavrasChaves = string.Join(" , ", item.txtCode, nomeAmigavel, video.Descricao),
                            Progresso = video.Progresso,
                            Thumb = video.Thumb,
                            Descricao = video.Descricao,
                            IdOrigemVideo = menuId == (int)ESubMenus.Provas ? video.IdProvaVideo : video.IdRevisaoAula
                        });
                    }

                    var percentMedia = playlistVideos.Count > 0 ? playlistVideos.Sum(x => x.Progresso) / playlistVideos.Count : 0;

                    playlist.Add(new CronogramaPlaylistDTO
                    {
                        Ativa = aprovada ? 1 : 0,
                        IdTema = item.intLessonTitleID,
                        Nome = nomeAmigavel,
                        PercentMedia = percentMedia,
                        Videos = playlistVideos
                    });
                }
            }

            if (playlist.Count == 0 && Utilidades.CursosAulasEspeciais().Contains(idProduto))
            {
                playlist.Add(new CronogramaPlaylistDTO
                {
                    Ativa = 1,
                    IdTema = 0,
                    Nome = "Videos em produção",
                    PercentMedia = 0,
                    Videos = new List<CronogramaPlaylistVideosDTO>()
                });
            }

            return playlist;
        }

        public TemaApostila GetVideosPorTipo(int produtoId, int matricula, int temaId, int aplicacaoId, int menuId)
        {

            if (menuId == (int)ESubMenus.Provas)
                return GetVideos(produtoId, matricula, temaId, aplicacaoId, ETipoVideo.ProvaVideo);
            else
            {
                var videos = GetVideos(produtoId, matricula, temaId, aplicacaoId, ETipoVideo.Revisao);
                videos.Videos = videos.VideosRevisao;
                return videos;
            }


        }

        private Dictionary<int, DateTime> GetDateCronogramaMateriais(List<msp_API_ListaEntidades_Result> cronogramaMateriais)
        {
            var dataCronogramaMateriais = new Dictionary<int, DateTime>();
            foreach (var item in cronogramaMateriais)
            {
                if (!dataCronogramaMateriais.ContainsKey(item.intMaterialID))
                {
                    var data = DateTime.ParseExact(string.Format("{0}/{1}", item.dataInicio, (int)item.intYear), "dd/MM/yyyy", null);
                    dataCronogramaMateriais.Add(item.intMaterialID, data);
                }
            }

            return dataCronogramaMateriais;
        }

        private Dictionary<int, DateTime> GetDateCronogramaLessonTitle(List<msp_API_ListaEntidades_Result> cronogramaMateriais)
        {
            var dataCronogramaMateriais = new Dictionary<int, DateTime>();
            foreach (var item in cronogramaMateriais)
            {
                if (!dataCronogramaMateriais.ContainsKey(item.intLessonTitleID))
                {
                    var data = DateTime.ParseExact(string.Format("{0}/{1}", item.dataInicio, (int)item.intYear), "dd/MM/yyyy", null);
                    dataCronogramaMateriais.Add(item.intLessonTitleID, data);
                }
            }

            return dataCronogramaMateriais;
        }

        private List<Semana> BuildSemanaDuplaCronograma(List<msp_API_ListaEntidades_Result> semanas)
        {
            try
            {
                var lSemanas = new List<Semana>();

                var semanasAgrupadas = semanas.GroupBy(x => new { Semana = x.intSemana }).ToList();

                foreach (var itSemana in semanasAgrupadas)
                {
                    var key = new GroupSemana()
                    {
                        Semana = itSemana.Key.Semana
                    };

                    if (itSemana.Count() <= 2)
                    {
                        var semana = BuildSemana(key, itSemana.ToList());

                        lSemanas.Add(semana);
                    }
                    else
                    {
                        var index = 0;
                        var groupedByEntity = itSemana.GroupBy(x => index++ / 2).ToList();

                        foreach (var it in groupedByEntity)
                        {
                            var semana = BuildSemana(key, it.ToList());

                            lSemanas.Add(semana);
                        }
                    }
                }

                return lSemanas;
            }
            catch (Exception ex)
            {
                return new List<Semana>();
            }

        }

        private List<Semana> BuildSemanaUnicaCronograma(List<msp_API_ListaEntidades_Result> semanas)
        {
            try
            {
                var lSemanas = new List<Semana>();
                var semanasAgrupadas = semanas.GroupBy(x => new { Semana = x.intSemana, ID = x.intID }).ToList();

                foreach (var itSemana in semanasAgrupadas)
                {
                    var key = new GroupSemana()
                    {
                        Semana = itSemana.Key.Semana,
                        ID = itSemana.Key.ID
                    };
                    var semana = BuildSemana(key, itSemana.ToList());
                    lSemanas.Add(semana);
                }
                return lSemanas;
            }
            catch (Exception ex)
            {
                return new List<Semana>();
            }
        }

        private Semana BuildSemana(GroupSemana key, IEnumerable<msp_API_ListaEntidades_Result> agrupamento)
        {
            var semana = new Semana()
            {
                Ativa = 0,
                Numero = key.Semana ?? 0,
                DataFim = "",
                DataInicio = "",
                Apostilas = new List<Apostila>()
            };

            foreach (var tema in agrupamento)
            {
                var apostila = new Apostila
                {
                    IdEntidade = (int)tema.intID,
                    Nome = tema.entidade,
                    PercentLido = 0,
                    Temas = new List<AulaTema>(),
                    MaterialId = tema.intMaterialID,
                    FiltroConteudo = new FiltroConteudoDTO
                    {
                        Codigo = tema.txtCode,
                        TituloApostila = tema.txtLessonTitleName,
                        TituloVideos = string.Empty,
                        PalavrasChaves = GetPalavrasChavesTema(tema)
                    }
                };

                apostila.Temas.Add(new AulaTema { TemaID = tema.intLessonTitleID });
                semana.Apostilas.Add(apostila);
            }

            if (semana.Apostilas.Count == 1)
                semana.Apostilas.Add(semana.Apostilas.FirstOrDefault());
            else //Caso tenha uma semana que a mesma entidade apareça mais de 2x mudar para não perder tema
                semana.Apostilas = semana.Apostilas.Take(2).ToList();

            return semana;
        }

        public string GetPalavrasChavesTema(msp_API_ListaEntidades_Result temaEntidade)
        {
            try
            {
                const string stringRemover = "apostila de";
                const string stringClm = "(clm)";

                return string.Join(" , ", temaEntidade.txtDescription == null ? null : temaEntidade.txtDescription.ToLowerInvariant().Replace(stringClm, string.Empty).Replace(stringRemover, string.Empty),
                                        temaEntidade.txtLessonTitleName == null ? null : temaEntidade.txtLessonTitleName.ToLowerInvariant(),
                                        temaEntidade.txtCode == null ? null : temaEntidade.txtCode.ToLowerInvariant());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public Enums.TipoLayoutMainMSPro GetTipoLayout(int idProduto, int menuId)
        {
            Business.Enums.TipoLayoutMainMSPro tipoLayoutMainMSPro;

            if (menuId == (int)ESubMenus.Revalida)
            {
                tipoLayoutMainMSPro = Enums.TipoLayoutMainMSPro.REVALIDA;
            }
            else
            {
                tipoLayoutMainMSPro = Utilidades.GetTipoLayoutMain((Produto.Cursos)idProduto);
            }


            return tipoLayoutMainMSPro;
        }

        public Semana.TipoAba GetTipoAba(int menuId)
        {
            switch ((ESubMenus)menuId)
            {
                case ESubMenus.Aulas:
                    return Semana.TipoAba.Aulas;
                case ESubMenus.Materiais:
                case ESubMenus.MaterialFake:
                    return Semana.TipoAba.Materiais;
                case ESubMenus.Questoes:
                    return Semana.TipoAba.Questoes;
                case ESubMenus.Revalida:
                    return Semana.TipoAba.Revalida;
                case ESubMenus.Checklists:
                    return Semana.TipoAba.Checklists;
                default:
                    return Semana.TipoAba.Nenhum;
            }
        }



        #endregion


        public List<CronogramaAula> GetCronogramaAula(int matricula, int ano)
        {
            DataTable dtCronograma = _aulaRepository.GetCronograma(matricula, ano);
            var mesFimCronograma = _aulaRepository.GetMesFimCronograma();

            var lstMeses = (from row in dtCronograma.AsEnumerable()
                            orderby row["intMonth"]
                            where row.Field<Int32>("intMonth") <= mesFimCronograma
                            select row.Field<Int32>("intMonth")
                           ).ToList().Distinct();

            var lstCronogramaAula = new List<CronogramaAula>();

            foreach (var mes in lstMeses)
            {

                var aulasMes = (from row in dtCronograma.AsEnumerable()
                                orderby row["intMonth"]
                                where Convert.ToInt32(row["intMonth"]) == mes
                                select new CronogramaAula
                                {
                                    NomeTurma = row["TxtName"].ToString(),
                                    IdTurma = Convert.ToInt32(row["intProductId"]),
                                    Dia = Convert.ToInt32(row["intDay"]),
                                    Mes = Convert.ToInt32(row["intMonth"]),
                                    IdProduto = Convert.ToInt32(row["intProductGroup1"]),
                                    NomeProduto = row["DescProd"].ToString(),
                                    Tempo = row["txtTempo"].ToString(),
                                    tema = new List<TemaAula>
                                       {
                                        new TemaAula {
                                                       NomeTema = row["txtLessonSubject"].ToString(),
                                                       Titulo = row["txtLessonTitle"].ToString() ,
                                                       Hora = Convert.ToDateTime(row["dteDateTime"]).TimeOfDay.ToString(),
                                                       IdProduto =  Convert.ToInt32(row["intProductGroup1"]),
                                                       Tempo = row["txtTempo"].ToString()

                                                   }
                                       }
                                }

                             ).ToList();

                var MesAgrupadasPorTurma = aulasMes.GroupBy(p => p.Dia + p.IdTurma,
                         (key, g) => new { Dia = key, TurmasDetalhadas = g }).ToList();


                foreach (var item in MesAgrupadasPorTurma)
                {
                    var crono = new CronogramaAula();

                    foreach (var item2 in item.TurmasDetalhadas)
                    {
                        crono.NomeTurma = item2.NomeTurma;
                        crono.IdTurma = item2.IdTurma;
                        crono.Mes = item2.Mes;
                        crono.Dia = item2.Dia;
                        crono.IdProduto = item2.IdProduto;
                        crono.NomeProduto = item2.NomeProduto;
                        crono.Tempo = item2.Tempo;

                        var tema = new TemaAula();

                        foreach (var item3 in item2.tema)
                        {
                            item2.tema = new List<TemaAula>();
                            tema.NomeTema = item3.NomeTema;
                            tema.Titulo = item3.Titulo;
                            tema.Hora = item3.Hora;
                            tema.IdProduto = item3.IdProduto;
                            tema.Tempo = item3.Tempo;

                            crono.tema.Add(tema);
                        }
                    }
                    lstCronogramaAula.Add(crono);
                }

            }

            return lstCronogramaAula;
        }

        public List<CronogramaAula> GetCronogramaAulaAluno(int matricula, int ano)
        {
            var dtCronograma = _aulaRepository.GetCronograma(matricula, ano);
            var lstCronogramaAulaAluno = FormataCronograma(dtCronograma);

            return lstCronogramaAulaAluno;
        }

        public List<CronogramaAula> GetCronogramaAulaTurma(int idTurma, int ano)
        {
            var dtCronograma = _aulaRepository.GetCronogramaTurma(idTurma, ano);
            var lstCronogramaAulaTurma = FormataCronograma(dtCronograma);

            return lstCronogramaAulaTurma;
        }

        public List<CronogramaAula> FormataCronograma(DataTable dtCronograma)
        {
            var mesFimCronograma = _aulaRepository.GetMesFimCronograma();
            var lstMeses = (from row in dtCronograma.AsEnumerable()
                            orderby row["intMonth"]
                            where row.Field<Int32>("intMonth") <= mesFimCronograma
                            select row.Field<Int32>("intMonth")
                           ).ToList().Distinct();

            var lstCronogramaAula = new List<CronogramaAula>();

            foreach (var mes in lstMeses)
            {

                var aulasMes = (from row in dtCronograma.AsEnumerable()
                                orderby row["intMonth"]
                                where Convert.ToInt32(row["intMonth"]) == mes
                                select new CronogramaAula
                                {
                                    NomeTurma = row["TxtName"].ToString(),
                                    IdTurma = Convert.ToInt32(row["intProductId"]),
                                    Dia = Convert.ToInt32(row["intDay"]),
                                    Mes = Convert.ToInt32(row["intMonth"]),
                                    IdProduto = Convert.ToInt32(row["intProductGroup1"]),
                                    NomeProduto = row["txtDescription"].ToString(),
                                    tema = new List<TemaAula>
                                       {
                                        new TemaAula {
                                                       NomeTema = row["txtLessonSubject"].ToString(),
                                                       Titulo = row["txtLessonTitle"].ToString() ,
                                                       Hora = Convert.ToDateTime(row["dteDateTime"]).TimeOfDay.ToString(),
                                                       Tempo = row["txtTempo"].ToString()

                                                   }
                                       }
                                }

                             ).ToList();

                var MesAgrupadasPorTurma = aulasMes.GroupBy(p => p.Dia + p.IdTurma,
                         (key, g) => new { Dia = key, TurmasDetalhadas = g }).ToList();


                foreach (var item in MesAgrupadasPorTurma)
                {
                    var crono = new CronogramaAula();

                    foreach (var item2 in item.TurmasDetalhadas)
                    {
                        crono.NomeTurma = item2.NomeTurma;
                        crono.IdTurma = item2.IdTurma;
                        crono.Mes = item2.Mes;
                        crono.Dia = item2.Dia;
                        crono.IdProduto = item2.IdProduto;
                        crono.NomeProduto = item2.NomeProduto;

                        var tema = new TemaAula();

                        foreach (var item3 in item2.tema)
                        {
                            item2.tema = new List<TemaAula>();
                            tema.NomeTema = item3.NomeTema;
                            tema.Titulo = item3.Titulo;
                            tema.Hora = item3.Hora;
                            tema.Tempo = item3.Tempo;

                            crono.tema.Add(tema);
                        }
                    }
                    lstCronogramaAula.Add(crono);
                }
            }

            return lstCronogramaAula;
        }

        public TemaApostila GetVideos(int idProduto, int matricula, int idTema, int idAplicacao, ETipoVideo tipoVideo = ETipoVideo.Revisao)
        {
            try
            {
                var todosVideos = _mednetRepository.GetTemasVideos(idProduto, matricula, 0, 0, false, idTema, tipoVideo: tipoVideo);
                var videoTema = todosVideos.Where(v => v.IdTema == idTema).FirstOrDefault();

                if (videoTema != null)
                {
                    if (tipoVideo == ETipoVideo.ProvaVideo)
                        videoTema = _mednetRepository.CalculaProgressosVideosTemaProva(videoTema, matricula);
                    else
                        videoTema = _mednetRepository.CalculaProgressosVideosTemaRevisao(videoTema, matricula);
                }
                else
                {
                    videoTema = new TemaApostila();
                }

                return videoTema;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<msp_API_ListaEntidades_Result> GetListaEntidadesPorMatriculaBase(int idProduto, int ano, int matricula)
        {

            var idProdutoOrigem = Utilidades.GetCursoOrigemCursoAulaEspecial(idProduto);

            var curso = (Produto.Cursos)idProdutoOrigem;
            int idProductGroup1 = (int)curso.GetProductByCourse();

            int courseId = -1;
            if (!_aulaRepository.AlunoPossuiMedMaster(matricula, ano) || _aulaRepository.AlunoPossuiMedOuMedcursoAnoAtualAtivo(matricula))
            {
                var turmas = _turmaRepository.GetTurmasContratadas(matricula, new int[] { ano }, idProductGroup1);

                if (turmas != null && turmas.Count > 0)
                {
                    if (turmas.Count > 1)
                    {
                        courseId = turmas.Where(a => a.IdStatusOrdemVenda == (int)OrdemVenda.StatusOv.Ativa).Select(b => b.ID).FirstOrDefault();
                        if (courseId == 0)
                        {
                            courseId = turmas.FirstOrDefault().ID;
                        }
                    }
                    else
                    {
                        courseId = turmas.FirstOrDefault().ID;
                    }
                }
            }

            int matriculaBase = GetMatriculaBaseCronogramaTurma(courseId, ano, idProduto);

            var listaEntidades = _cronogramaRepository.GetListaEntidades(idProdutoOrigem, ano, matriculaBase);

            return listaEntidades;

        }

        public int GetMatriculaBaseCronogramaTurma(int courseId, int ano, int produtoId)
        {
            var turmasMatriculasBase = _cronogramaRepository.GetTurmaMatriculasBase(new TurmaMatriculaBaseDTO { CourseId = courseId, Ano = ano, ProdutoId = produtoId });

            if (turmasMatriculasBase == null || !turmasMatriculasBase.Any())
                return Constants.CONTACTID_ACADEMICO;

            var baseGeral = turmasMatriculasBase.Where(x => x.Ano == -1 && x.CourseId == -1).FirstOrDefault();
            var baseTurma = turmasMatriculasBase.Where(x => (x.Ano == ano || x.Ano == -1) && x.CourseId == courseId).FirstOrDefault();

            if (baseTurma == null)
                return baseGeral.MatriculaBase;
            else
                return baseTurma.MatriculaBase;

        }
        public int GetDiasLimiteTurmaExcecao(int idProduto, int ano, int matricula)
        {
            var curso = (Produto.Cursos)idProduto;
            int idProductGroup1 = (int)curso.GetProductByCourse();
            int courseId = -1;

            idProductGroup1 = Utilidades.GetProdutoOrigemProdutoAulaEspecial(idProduto);

            var turmas = _turmaRepository.GetTurmasContratadas(matricula, new int[] { ano }, idProductGroup1);

            if (turmas != null && turmas.Count > 0)
                courseId = turmas.FirstOrDefault().ID;

            int diasLimite = GetDiasLimiteCronogramaTurma(courseId, ano, idProduto);

            return diasLimite;
        }

        public ResponseDTO<List<SemanaProgressoPermissao>> GetPermissoes(int idProduto, int matricula, int anoMaterial = 0)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetPermissoes))
                return RedisCacheManager.GetItemObject<ResponseDTO<List<SemanaProgressoPermissao>>>(RedisCacheConstants.DadosFakes.KeyGetPermissoes);

            try
            {

                var semanaProgressoPermissao = new List<SemanaProgressoPermissao>();
                var aulaBusiness = new AulaBusiness(new AulaEntity());

                if (Utilidades.CursosAulasEspeciais().Contains(idProduto))
                {
                    var idProdutoOrigem = Utilidades.GetCursoOrigemCursoAulaEspecial(idProduto);
                    var curso = (Produto.Cursos)idProdutoOrigem;

                    var courseId = -1;
                    var turmas = _turmaRepository.GetTurmasContratadas(matricula, new int[] { anoMaterial }, (int)curso.GetProductByCourse());
                    if (turmas != null && turmas.Count > 0)
                        courseId = turmas.FirstOrDefault().ID;

                    matricula = GetMatriculaBaseCronogramaTurma(courseId, anoMaterial, idProduto);

                    semanaProgressoPermissao.Add(new SemanaProgressoPermissao
                    {
                        PermissaoSemanas = aulaBusiness.GetPermissaoSemanasAulasEspeciais(anoMaterial, matricula, idProdutoOrigem)
                    });

                }
                else
                {
                    var anosProdutoAluno = _cronogramaRepository.AnosProdutoAluno(matricula, idProduto, anoMaterial);
                    var maiorAnoAluno = anosProdutoAluno.OrderByDescending(c => c).FirstOrDefault().GetValueOrDefault();

                    if (anoMaterial == 0) anoMaterial = maiorAnoAluno;
                    if (anoMaterial < maiorAnoAluno && anosProdutoAluno.Contains(anoMaterial)) maiorAnoAluno = anoMaterial;


                    semanaProgressoPermissao.Add(new SemanaProgressoPermissao
                    {
                        PermissaoSemanas = aulaBusiness.GetPermissaoSemanas(maiorAnoAluno, matricula, idProduto, anoMaterial)
                    });
                }
                return new ResponseDTO<List<SemanaProgressoPermissao>> { Retorno = semanaProgressoPermissao, Sucesso = true };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<SemanaProgressoPermissao>> { Sucesso = false, Mensagem = ex.Message };
            }

        }


        public int GetDiasLimiteCronogramaTurma(int courseId, int ano, int idProduto)
        {
            var turmasMatriculasBase = _cronogramaRepository.GetTurmaMatriculasBase(new TurmaMatriculaBaseDTO { CourseId = courseId, Ano = ano, ProdutoId = idProduto });

            if (turmasMatriculasBase == null)
                return Constants.CONTACTID_ACADEMICO;

            var baseGeral = turmasMatriculasBase.Where(x => x.Ano == -1 && x.CourseId == -1).FirstOrDefault();
            var baseTurma = turmasMatriculasBase.Where(x => (x.Ano == ano || x.Ano == -1) && x.CourseId == courseId).FirstOrDefault();

            if (baseTurma == null)
                return baseGeral.DiasLimite;
            else
                return baseTurma.DiasLimite;

        }
    }
}