using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using Medgrupo.DataAccessEntity;

namespace MedCore_DataAccess.Business
{
    public class AulaBusiness
    {
        private IAulaEntityData _aulaRepository;

        public AulaBusiness(IAulaEntityData repository)
        {
            _aulaRepository = repository;
        }

        public List<Semana> GetPermissaoSemanas(int ano, int matricula, int cursoId, int anoMaterial = 0)
        {
            var cronogramaMaterialDireito = new List<Semana>();
            var anoVigente = ano;
            if (anoVigente == 0) anoVigente = Utilidades.GetYear();
            if (anoMaterial == 0) anoMaterial = anoVigente;

            //ctx.SetCommandTimeOut(90);

            var curso = (Produto.Cursos)cursoId;
            int produtoId = (int)curso.GetProductByCourse();

            var acessoAntecipado = _aulaRepository.AlunoTemAcessoAntecipado(matricula);

            var semanasCronograma = _aulaRepository.ObterCronograma(cursoId, anoMaterial);

            var materialDireito = _aulaRepository.ObterMaterialDireitoAluno(matricula, produtoId, cursoId, anoVigente, acessoAntecipado, anoMaterial);

            if (anoMaterial != anoVigente)
                materialDireito = _aulaRepository.AcertarMaterialDireitoByCronograma(materialDireito, semanasCronograma);

            materialDireito = AtivaMateriaisAnosAnteriores(materialDireito, anoMaterial, matricula, cursoId);

            cronogramaMaterialDireito = MontarCronograma(semanasCronograma, materialDireito, ano);

            return cronogramaMaterialDireito;
        }

        private List<Semana> MontarCronograma(List<IGrouping<int?, msp_API_ListaEntidades_Result>> semanasCronograma, List<MaterialDireitoDTO> materialDireito, int ano)
        {
            var cronogramaMaterialDireito = new List<Semana>();
            var semanaAtual = Utilidades.GetNumeroSemanaAtual(DateTime.Now);
            
            foreach (var itSemana in semanasCronograma)
            {
                if (itSemana.Count() <= 2)
                {
                    var semana = new Semana()
                    {

                        Ativa = materialDireito.Any(x => itSemana.Any(y => y.intID == x.Id && x.Ativa)) ? 1 : 0,
                        Numero = itSemana.Key ?? 0,

                        DataFim = materialDireito.Any(x => x.IntSemana == itSemana.Key)
                        ? materialDireito.Where(x => x.IntSemana == itSemana.Key).Select(x => x.DataFim).FirstOrDefault()
                        : itSemana.First().datafim,

                        DataInicio = materialDireito.Any(x => x.IntSemana == itSemana.Key)
                        ? materialDireito.Where(x => x.IntSemana == itSemana.Key).Select(x => x.DataInicio).FirstOrDefault()
                        : itSemana.First().dataInicio,

                        SemanaAtiva = semanaAtual == itSemana.Key ? 1 : 0,

                        Ano = itSemana.FirstOrDefault().intYear.Value

                    };

                    var apostilasAprovadas = new List<int?>();

                    var apostilas = materialDireito
                    .Where(x => x.IntSemana == itSemana.Key && x.ApostilasAprovadas != null && x.ApostilasAprovadas.Count > 0 && (ano == 0 || x.IntYear == ano))
                    .ToList();
                    
                    if (apostilas.Count > 0)
                    {
                        apostilas.ForEach(x => apostilasAprovadas.AddRange(x.ApostilasAprovadas));
                        semana.ApostilasAprovadas = apostilasAprovadas.Distinct().ToList();
                    }
                    GetQuestoesAprovadas(materialDireito, itSemana.Key, ano, semana);
                    semana.SemanaAtiva = SemanaAtualAtiva(semana.DataInicio, semana.DataFim, semana.Ano);

                    cronogramaMaterialDireito.Add(semana);
                }
                else
                {
                    var index = 0;
                    var groupedByEntity = itSemana.GroupBy(x => index++ / 2).ToList();

                    foreach (var it in groupedByEntity)
                    {
                        var semana = new Semana()
                        {
                            Ativa = materialDireito.Any(x => itSemana.Any(y => y.intID == x.Id && x.Ativa)) ? 1 : 0,

                            Numero = itSemana.Key ?? 0,

                            DataFim = materialDireito.Any(x => x.IntSemana == itSemana.Key)
                            ? materialDireito.Where(x => x.IntSemana == itSemana.Key).Select(x => x.DataFim).FirstOrDefault()
                            : itSemana.First().datafim,

                            DataInicio = materialDireito.Any(x => x.IntSemana == itSemana.Key)
                            ? materialDireito.Where(x => x.IntSemana == itSemana.Key).Select(x => x.DataInicio).FirstOrDefault()
                            : itSemana.First().dataInicio,

                            SemanaAtiva = semanaAtual == itSemana.Key ? 1 : 0,

                            Ano = itSemana.FirstOrDefault().intYear.Value

                        };

                        var apostilasAprovadas = new List<int?>();

                        var apostilas = materialDireito
                        .Where(x => x.IntSemana == itSemana.Key && x.ApostilasAprovadas != null && x.ApostilasAprovadas.Count > 0 && (ano == 0 || x.IntYear == ano))
                        .ToList();

                        if (apostilas.Count > 0)
                        {
                            apostilas.ForEach(x => apostilasAprovadas.AddRange(x.ApostilasAprovadas));
                            semana.ApostilasAprovadas = apostilasAprovadas.Distinct().ToList();
                        }
                        GetQuestoesAprovadas(materialDireito, itSemana.Key, ano, semana);
                        semana.SemanaAtiva = SemanaAtualAtiva(semana.DataInicio, semana.DataFim, semana.Ano);

                        cronogramaMaterialDireito.Add(semana);
                    }
                }

            }

            if (cronogramaMaterialDireito.Any() && cronogramaMaterialDireito.All(x => x.SemanaAtiva != 1))
                cronogramaMaterialDireito.Last().SemanaAtiva = 1;

            return cronogramaMaterialDireito;
        }

        public void GetQuestoesAprovadas(List<MaterialDireitoDTO> materialDireito, int? numeroSemana, int ano,Semana semana)
        {
            var questoesAprovadas = new List<int?>();

            var questoes = materialDireito
           .Where(x => x.IntSemana == numeroSemana && x.QuestoesAprovadas != null && x.QuestoesAprovadas.Count > 0 && (ano == 0 || x.IntYear == ano))
           .ToList();

           
            if (questoes.Count() > 0)
            {
                questoes.ForEach(x => questoesAprovadas.AddRange(x.QuestoesAprovadas));
                semana.QuestoesAprovadas = questoesAprovadas.Distinct().ToList();
            }
        }

        public int SemanaAtualAtiva(string inicio, string fim, int ano)
        {
            var hoje = DateTime.Now;
            return (hoje > GetDateFromStringCronograma(inicio, ano) && hoje < GetDateFromStringCronograma(fim, ano)) ? 1 : 0;
        }

        private DateTime GetDateFromStringCronograma(string data, int ano)
        {
            var cultureInfo = new CultureInfo("pt-BR");
            var dataString = string.Concat(data, "/", (ano != 0 ? ano : DateTime.Now.Year));

            return DateTime.Parse(dataString, cultureInfo);
        }

        private List<MaterialDireitoDTO> AtivaMateriaisAnosAnteriores(List<MaterialDireitoDTO> materialDireito, int ano, int matricula, int cursoId)
        {
            var idProductGroup1 = ProdutoEntity.GetProductByCourse(cursoId);
            var anoAtual = Utilidades.GetYear();

            var idsExtensivos = new int[] { (int)Produto.Cursos.MED, (int)Produto.Cursos.MEDCURSO };
            var alunoCancelado_UltimaSemanaPaga = -1;
            if (idsExtensivos.Contains(cursoId))
            {
                alunoCancelado_UltimaSemanaPaga = _aulaRepository.BuscarSemanaPagaAlunoCancelado(ano, matricula, anoAtual, cursoId);
            }

            List<int> anosOV = _aulaRepository.GetAnosOVAluno(matricula, idProductGroup1);
            var ovAnoAnterior = anosOV.Where(x => x < ano).Any();

            if (ano == anoAtual && ovAnoAnterior)
            {
                if (alunoCancelado_UltimaSemanaPaga >= 0 && materialDireito.Where(x => x.IntSemana > alunoCancelado_UltimaSemanaPaga).Count() > 0)
                {
                    var temp = new List<MaterialDireitoDTO>();
                    foreach (var item in materialDireito)
                    {
                        if (item.IntSemana <= alunoCancelado_UltimaSemanaPaga && item.IntYear == anoAtual)
                        {
                            item.Ativa = true;
                        }
                    }
                } else
                {
                    materialDireito = materialDireito.Select(c => { c.Ativa = true; return c; }).ToList();
                }
            }
            return materialDireito;
        }

        public List<Semana> GetPermissaoSemanasAulasEspeciais(int ano, int matricula, int cursoId)
        {

            var curso = (Produto.Cursos)cursoId;
            int produtoId = (int)curso.GetProductByCourse();

            var acessoAntecipado = _aulaRepository.AlunoTemAcessoAntecipado(matricula);

            var semanasCronograma = _aulaRepository.ObterCronograma(cursoId, ano, matricula);

            var materialDireito = _aulaRepository.ObterMaterialDireitoAluno(matricula, produtoId, cursoId, ano, acessoAntecipado, ano);

            var cronogramaMaterialDireito = MontarCronograma(semanasCronograma, materialDireito, ano);

            cronogramaMaterialDireito.ForEach(x => x.Ativa = 1);

            return cronogramaMaterialDireito;
        }
    }
}