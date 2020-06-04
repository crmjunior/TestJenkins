using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using Medgrupo.DataAccessEntity;
using Microsoft.EntityFrameworkCore.Internal;


namespace MedCore_DataAccess.Business
{    public class ComboBusiness : IComboBusiness
    {
        private readonly IComboData _comboRepository;
        private readonly IAccessData _accessRepository;

        public ComboBusiness(IComboData comboRepository, IAccessData accessRepository)
        {
            _comboRepository = comboRepository; 
            _accessRepository = accessRepository;
        }

      
        public List<Combo> GetComboAposLancamentoMsProCache(int idClient, int applicationId, string versaoApp)
        {

            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetComboAposLancamentoMsPro))
                return RedisCacheManager.GetItemObject<List<Combo>>(RedisCacheConstants.DadosFakes.KeyGetComboAposLancamentoMsPro);

            try
            {
                if (RedisCacheManager.CannotCache(RedisCacheConstants.Combo.KeyGetComboAposLancamentoMsPro))
                    return GetComboAposLancamentoMsPro(idClient, applicationId, versaoApp);

                var key = String.Format("{0}:{1}:{2}:{3}", RedisCacheConstants.Combo.KeyGetComboAposLancamentoMsPro, idClient, applicationId, versaoApp);
                var combo = RedisCacheManager.GetItemObject<List<Combo>>(key);

                if (combo == null)
                {
                    combo = GetComboAposLancamentoMsPro(idClient, applicationId, versaoApp);
                    if (combo != null)
                        RedisCacheManager.SetItemObject(key, combo);
                }
                return combo;

            }
            catch (Exception)
            {
                return GetComboAposLancamentoMsPro(idClient, applicationId, versaoApp);
            }

        }

        public List<Combo> GetComboAposLancamentoMsPro(int idClient, int applicationId, string versaoApp)
        {
             
            List<Combo> comboMsPro = GetCombo(idClient, applicationId, versaoApp);

            var idsExtensivos = new int[] { (int)Produto.Cursos.MED, (int)Produto.Cursos.MEDCURSO };
            var idsIntensivos = new int[] { (int)Produto.Cursos.INTENSIVAO };
            var idsRMais = new int[] { (int)Produto.Cursos.R3Cirurgia, (int)Produto.Cursos.R3Clinica, (int)Produto.Cursos.R3Pediatria, (int)Produto.Cursos.R4GO, (int)Produto.Cursos.MASTO, (int)Produto.Cursos.TEGO };

            var anoAtual = Utilidades.GetYear();
            var anoAnterior = anoAtual - 1;
            var isAntesDataLimite = _comboRepository.IsBeforeDataLimite(applicationId, anoAtual);

            if (comboMsPro.Any(x => (idsExtensivos.Contains(x.ComboId) || idsIntensivos.Contains(x.ComboId)) && x.Anos.Contains(anoAtual)))
                comboMsPro.Where(x => idsExtensivos.Contains(x.ComboId) && !x.Anos.Contains(anoAtual) && x.Anos.Any(a => a < anoAtual)).ToList().ForEach(z => z.Anos.Add(anoAtual));
            else if (comboMsPro.Any(x => (idsExtensivos.Contains(x.ComboId) || idsIntensivos.Contains(x.ComboId)) && x.Anos.Contains(anoAnterior)) && isAntesDataLimite)
                comboMsPro.Where(x => idsExtensivos.Contains(x.ComboId) && !x.Anos.Contains(anoAnterior) && x.Anos.Any(a => a < anoAnterior)).ToList().ForEach(z => z.Anos.Add(anoAnterior));

            comboMsPro.ForEach(x => x.Anos = x.Anos.OrderByDescending(o => o).ToList());

            foreach (Combo combo in comboMsPro)
            {
                if (combo.Anos.Count > 0)
                {
                    var anoMaximoProduto = combo.Anos.Max();

                    if (anoMaximoProduto <= Utilidades.AnoLancamentoMedsoftPro && anoMaximoProduto == anoAnterior && isAntesDataLimite)
                        combo.Anos.RemoveAll(c => c < anoMaximoProduto);
                    else if (anoMaximoProduto <= Utilidades.AnoLancamentoMedsoftPro)
                        combo.Anos.RemoveAll(c => c <= anoMaximoProduto);
                    else if (combo.ComboId == (int)Produto.Cursos.MEDELETRO)
                        combo.Anos.RemoveAll(c => c < Utilidades.AnoLancamentoMaterialMedEletro);
                    else if (combo.ComboId == (int)Produto.Cursos.INTENSIVAO)
                        combo.Anos.RemoveAll(c => c < Utilidades.AnoLancamentoMaterialIntensivao);
                    else if (combo.ComboId == (int)Produto.Cursos.CPMED)
                        combo.Anos.RemoveAll(c => c < Utilidades.AnoLancamentoMaterialCPMed);
                    else
                        combo.Anos.RemoveAll(c => c <= Utilidades.AnoLancamentoMedsoftPro);
                }
            }

            comboMsPro = ValidaSubstituicaoCombo(comboMsPro, idClient);

            comboMsPro.RemoveAll(x => x.Anos.Count == 0);

            return comboMsPro;
        }

        public List<Combo> ValidaSubstituicaoCombo(List<Combo> comboMsPro, int idClient)
        {
            var comboLiberacao = _comboRepository.GetProdutoComboLiberado(idClient);

            if (comboLiberacao != null && comboLiberacao.Any())
            {
                if (comboLiberacao.Count == 1 && comboLiberacao.Any(x => x.ProdutoFake) ) //possui somente fake
                {
                    comboMsPro = AdicionaProdutosLiberados(comboMsPro, comboLiberacao);
                }
                else if (!comboLiberacao.Any(x => x.ProdutoFake))   //nao possui fake
                {
                    comboMsPro = RemoveProdutosNaoLiberados(comboMsPro, comboLiberacao);
                }
                else //possui fake e outros
                {
                    comboMsPro = RemoveProdutosNaoLiberados(comboMsPro, comboLiberacao);
                    comboMsPro = AdicionaProdutosLiberados(comboMsPro, comboLiberacao);
                    
                }
            }
            return comboMsPro;
        }

        public List<Combo> RemoveProdutosNaoLiberados(List<Combo> comboMsPro, List<DTO.ProdutoComboLiberadoDTO> comboLiberacao)
        {
            comboMsPro.RemoveAll(c => !comboLiberacao.Select(cl => cl.intCurso).Contains(c.ComboId));
            foreach (Combo combo in comboMsPro)
            {
                combo.Anos.RemoveAll(c => !comboLiberacao.Where(x => x.intCurso == combo.ComboId).Select(cl => cl.intYear).Contains(c));
            }
            return comboMsPro;
        }

        public List<Combo> AdicionaProdutosLiberados(List<Combo> comboMsPro, List<DTO.ProdutoComboLiberadoDTO> comboLiberacao)
        {
            foreach (var item in comboLiberacao)
            {
                if (!comboMsPro.Any(x => x.ComboId == item.intCurso))
                {
                    var produtoLiberado = comboLiberacao.Where(x => x.intCurso == item.intCurso)
                                                        .GroupBy(g => new { g.intCurso, g.Id, g.Nome, g.Ordem })
                                                        .Select(x => new Combo
                                                        {
                                                            Id = x.Key.Id,
                                                            ComboId = x.Key.intCurso,
                                                            Nome = x.Key.Nome,
                                                            tipoLayoutMain = (int)Utilidades.GetTipoLayoutMain((Produto.Cursos)x.Key.intCurso),
                                                            IntOrdem = x.Key.Ordem,
                                                            Anos = x.Select(y => y.intYear).ToList()
                                                        }).FirstOrDefault();
                    comboMsPro.Add(produtoLiberado);
                }
            }
            return comboMsPro;
        }

        public List<Combo> GetCombo(int idClient, int applicationId, string versaoApp)
        {
            const string VERSAO_APP_TROCA_LAYOUT_MASTOLOGIA = "5.2.0";
            versaoApp = string.IsNullOrEmpty(versaoApp) ? "0.0.0" : versaoApp;
            List<int> idProdutoAulao = new List<int>() { (int)Produto.Cursos.R3Cirurgia, (int)Produto.Cursos.R3Pediatria , (int)Produto.Cursos.R3Clinica
                , (int)Produto.Cursos.R4GO };
            List<int> idProdutoAulaoMastoTEGO = new List<int>() { (int)Produto.Cursos.TEGO, (int)Produto.Cursos.MASTO };
            var permissoes = GetPermissoes(idClient, applicationId);
            var comboPermitido = _comboRepository.GetCombosPermitidos(permissoes, applicationId, versaoApp);
                      
            var lstAnoPorProduto = _comboRepository.GetAnosPorProduto(idClient);

            if (comboPermitido.Any(x => x.ComboId == (int)Produto.Cursos.MEDCURSO_AULAS_ESPECIAIS))
                lstAnoPorProduto.Add((int)Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS, new int[] { Utilidades.GetYear() });

            if (comboPermitido.Any(x => x.ComboId == (int)Produto.Cursos.MED_AULAS_ESPECIAIS))
                lstAnoPorProduto.Add((int)Produto.Produtos.MED_AULAS_ESPECIAIS, new int[] { Utilidades.GetYear() });

            var produtosAnoAgrupados = AgrupaProdutos(lstAnoPorProduto);

            foreach (var combo in comboPermitido)
            {
                var curso = (Produto.Cursos)combo.ComboId;
                int produtoId = (int)curso.GetProductByCourse();

                combo.tipoLayoutMain = (int)Utilidades.GetTipoLayoutMain(curso);
           
                if(applicationId == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON && Utilidades.CursosAulasEspeciais().Contains(produtoId))
                    combo.tipoLayoutMain = (int)TipoLayoutMainMSPro.WEEK_DOUBLE;

                if ((curso == Produto.Cursos.MASTO || curso == Produto.Cursos.TEGO) && Utilidades.VersaoMenorOuIgual(versaoApp, VERSAO_APP_TROCA_LAYOUT_MASTOLOGIA))
                    combo.tipoLayoutMain = (int)TipoLayoutMainMSPro.WEEK_SINGLE;

                if (idProdutoAulao.Contains((int)curso)  && (
                     ((applicationId == (int)Aplicacoes.MsProMobile && Utilidades.VersaoMenorOuIgual(versaoApp,
                 ConfigurationProvider.Get("Settings:VersaoAppTrocaLayoutAuloesSR3R4"))) || !Convert.ToBoolean(ConfigurationProvider.Get("Settings:AtivaAuloesSR3R4")))
                     || applicationId == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON))
                    combo.tipoLayoutMain = (int)TipoLayoutMainMSPro.WEEK_SINGLE;

                if ((idProdutoAulaoMastoTEGO.Contains((int)curso)) 
                    && (((applicationId == (int)Aplicacoes.MsProMobile 
                    && Utilidades.VersaoMenorOuIgual(versaoApp,ConfigurationProvider.Get("Settings:VersaoAppTrocaLayoutAuloesSR3R4"))) 
                    || !Convert.ToBoolean(ConfigurationProvider.Get("Settings:AtivaAuloesSR3R4")))))
                    combo.tipoLayoutMain = (int)TipoLayoutMainMSPro.WEEK;
                
                if (idProdutoAulaoMastoTEGO.Contains((int)curso) && applicationId == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON)
                    combo.tipoLayoutMain = (int)TipoLayoutMainMSPro.WEEK_SINGLE;

                combo.Anos = produtosAnoAgrupados.Where(x => x.Key == produtoId).SelectMany(y => y.Value).ToList();
            }

            comboPermitido.RemoveAll(x => x.Anos.Count == 0);

            return comboPermitido;

        }


        public Dictionary<int, int[]> AgrupaProdutos(Dictionary<int, int[]> dicProdutoAnos)
        {

            var idsMed = new int[] { (int)Utilidades.ProductGroups.MED, (int)Utilidades.ProductGroups.MEDEAD, (int)Utilidades.ProductGroups.MedMaster };
            var idsMedcurso = new int[] { (int)Utilidades.ProductGroups.MEDCURSO, (int)Utilidades.ProductGroups.MEDCURSOEAD , (int)Utilidades.ProductGroups.MedMaster };

            if(dicProdutoAnos.Any(x => x.Key == (int)Utilidades.ProductGroups.MedMaster))
            {
                var anosMedmaster = dicProdutoAnos[(int)Utilidades.ProductGroups.MedMaster].ToList();
                anosMedmaster.Add(anosMedmaster.Min() - 1);
                dicProdutoAnos[(int)Utilidades.ProductGroups.MedMaster] = anosMedmaster.ToArray();
            }

            var med = dicProdutoAnos.Where(x => idsMed.Contains(x.Key))
                                    .Select(z => new { id = (int)Utilidades.ProductGroups.MED, ano = z.Value })
                                    .GroupBy(y => y.id)
                                    .Select(a => new { id = a.Key, anos = a.SelectMany(k => k.ano).Distinct().ToArray()})
                                    .ToList();

            var medcurso = dicProdutoAnos.Where(x => idsMedcurso.Contains(x.Key))
                                         .Select(z => new { id = (int)Utilidades.ProductGroups.MEDCURSO, ano = z.Value })
                                         .GroupBy(y => y.id)
                                         .Select(a => new { id = a.Key, anos = a.SelectMany(k => k.ano).Distinct().ToArray() })
                                         .ToList();

            var outros = dicProdutoAnos.Where(x => !idsMed.Contains(x.Key) && !idsMedcurso.Contains(x.Key))
                                       .Select(z => new { id = z.Key, ano = z.Value })
                                       .GroupBy(y => y.id)
                                       .Select(a => new { id = a.Key, anos = a.SelectMany(k => k.ano).Distinct().ToArray() })
                                       .ToList();

            var  dicProdutosAgrupadosAnos = med.Concat(medcurso)
                                                     .Concat(outros)
                                                     .ToDictionary(x => x.id, x => x.anos);

            return dicProdutosAgrupadosAnos;
        }


        public int GetProdutoExtensivoMaisRecente(Dictionary<int, int[]> dicProdutoAnos)
        {
            var anoAtual = Utilidades.GetYear();
            var idsMed = new int[] { (int)Utilidades.ProductGroups.MED, (int)Utilidades.ProductGroups.MEDEAD };
            var idsMedcurso = new int[] { (int)Utilidades.ProductGroups.MEDCURSO, (int)Utilidades.ProductGroups.MEDCURSOEAD };

            var extensivo = dicProdutoAnos.Where(x => (idsMed.Contains(x.Key) || idsMedcurso.Contains(x.Key)))
                                          .Select(y => new { prod = y.Key, ano = y.Value.Max() })
                                          .Distinct()
                                          .ToList();

            var produtoMaisRecente = extensivo.Where(a => a.ano < anoAtual).OrderByDescending(x => x.ano).FirstOrDefault().prod;

            return produtoMaisRecente;
        }

        public int GetProdutoExtensivoComplemento(int produtoId)
        {
            if (produtoId == (int)Utilidades.ProductGroups.MED || produtoId == (int)Utilidades.ProductGroups.MEDEAD)
                return (int)Utilidades.ProductGroups.MEDCURSO;
            else if (produtoId == (int)Utilidades.ProductGroups.MEDCURSO || produtoId == (int)Utilidades.ProductGroups.MEDCURSOEAD)
                return (int)Utilidades.ProductGroups.MED;
            else return 0;
        }



        public List<AccessObject> GetPermissoes(int idClient, int applicationId)
        {
            var lstCombo = _accessRepository.GetAll(applicationId, (int)Utilidades.AccessObjectType.Combo);
            var lstPermissoes = _accessRepository.GetAlunoPermissoes(lstCombo, idClient, applicationId);

            var permissoes = (from a in lstCombo
                              join b in lstPermissoes on a.Id equals b.ObjetoId
                              where b.AcessoId != (int)Utilidades.PermissaoStatus.SemAcesso
                              select a).ToList();

            return permissoes;
        }

        public List<Combo> GetBonusComboProdutos(int matricula, string versaoApp)
        {
           var anoMinimoComConteudo = 2017;
           var combo =  new ComboBusiness(new ComboEntity(), new AccessEntity()).GetComboAposLancamentoMsProCache(matricula, (int)Aplicacoes.MsProMobile, versaoApp);

           var filtroCombo = combo.Where(x => x.ComboId == (int)Produto.Cursos.MED || x.ComboId == (int)Produto.Cursos.MEDCURSO).ToList();
           foreach (var item in filtroCombo)
           {
               item.ComboId = item.ComboId == (int)Produto.Cursos.MEDCURSO ? (int)Produto.Produtos.MEDCURSO : (int)Produto.Produtos.MED;   
               item.Anos.RemoveAll(x => x <= anoMinimoComConteudo);
           }

           return filtroCombo;

        }

        public List<ComboDeclaracao> GetDeclaracaoMatriculaComboProdutos(int matricula)
        {
            var ovsAluno = new OrdemVendaEntity().GetOvsAtivaAluno(matricula);

            var lstProdutos = new List<int>() {
                (int)Produto.Produtos.MEDCURSO, (int)Produto.Produtos.MED,
                (int)Produto.Produtos.MEDCURSOEAD, (int)Produto.Produtos.MEDEAD,
                (int)Produto.Produtos.INTENSIVAO, (int)Produto.Produtos.CPMED,
                (int)Produto.Produtos.MEDELETRO, (int)Produto.Produtos.RAC,
                (int)Produto.Produtos.RACIPE
            };
            ovsAluno.RemoveAll(h => !lstProdutos.Contains(h.GroupID));
            var anos = ovsAluno.GroupBy(x => x.Year).ToList();

            var lstCombo = new List<ComboDeclaracao>();
            foreach (var item in anos)
            {
                var combo = new ComboDeclaracao();
                combo.Ano = item.Key;

                var produtosDoAno = ovsAluno.Where(x => x.Year == item.Key).ToList();

                combo.ListaProdutos = new List<ListaProdutosDTO>();
                foreach (var item2 in produtosDoAno)
                {
                    var produtoDTO = new ListaProdutosDTO()
                    {
                        ID = new List<int>() { Convert.ToInt32(item2.GroupID) },
                        Descricao = Convert.ToInt32(item2.GroupID) != (int)Produto.Produtos.CPMED ? item2.Descricao.ToLower() : "cpmed"
                    };
                    combo.ListaProdutos.Add(produtoDTO);
                }
                lstCombo.Add(combo);
            }

            var lstComboDeclaracaoFinal = UnirExtensivo(lstCombo);

            var aluno = new ClienteEntity().GetClient(matricula);
            lstComboDeclaracaoFinal[0].EmailAluno = aluno.Email;

            return lstComboDeclaracaoFinal;
        }
        public static List<ComboDeclaracao> UnirExtensivo(List<ComboDeclaracao> combo)
        {
            var QtdProdutoMedcursoMed = 2;
            List<int> IDprodutoExtensivo = new List<int>() { (int)Produto.Produtos.MEDCURSO, (int)Produto.Produtos.MED };
            foreach (var item in combo)
            {
                List<int> IDproduto = new List<int>();
                foreach (var produto in item.ListaProdutos)
                {
                    var res = produto.ID.Where(x => IDprodutoExtensivo.Contains(x)).FirstOrDefault();
                    switch (res)
                    {
                        case (int)Produto.Produtos.MEDCURSO:
                        case (int)Produto.Produtos.MED:
                            produto.Descricao = "extensivo";
                            IDproduto.Add(res);
                            break;
                    }

                    if (IDproduto.Count == QtdProdutoMedcursoMed)
                    {
                        var resultado = item.ListaProdutos.Join(
                            IDprodutoExtensivo,
                            listaprodutos => listaprodutos.ID.FirstOrDefault(),
                            idProduto => idProduto,
                            (listaprodutos, idProduto) => listaprodutos
                        ).ToList();
                        foreach (var produtoExtensivo in resultado)
                        {
                            item.ListaProdutos.Remove(produtoExtensivo);
                        }
                        item.ListaProdutos.Add(new ListaProdutosDTO()
                        {
                            Descricao = "extensivo",
                            Ordenacao = 1,
                            ID = IDprodutoExtensivo

                        });
                        break;
                    }
                }

            }
            return combo;
        }
    }
}