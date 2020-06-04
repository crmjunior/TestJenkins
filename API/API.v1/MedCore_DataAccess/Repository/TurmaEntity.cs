using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MedCore_DataAccess.Repository
{
    public class TurmaEntity : ITurmaData, IDataAccess<Turma>
    {
        public Turma TurmaConvidadaAluno(int matricula, int anoInscricao = 0)
        {

            using (var ctx = new DesenvContext())
            {
                if (anoInscricao == 0) anoInscricao = Utilidades.GetAnoInscricao(Aplicacoes.INSCRICAO_CPMED);
                int idClassificacao;
                switch (anoInscricao)
                {
                    case 2015:
                        idClassificacao = (int)Classificacao.TipoClassificacao.TurmaConvidada2015;
                        break;
                    case 2016:
                        idClassificacao = (int)Classificacao.TipoClassificacao.TurmaConvidada2016;
                        break;
                    case 2017:
                        idClassificacao = (int)Classificacao.TipoClassificacao.TurmaConvidada2017;
                        break;
                    case 2018:
                        idClassificacao = (int)Classificacao.TipoClassificacao.TurmaConvidada2018;
                        break;
                    case 2019:
                        idClassificacao = (int)Classificacao.TipoClassificacao.TurmaConvidada2019;
                        break;

                    default:
                        idClassificacao = 0;
                        break;
                }


                var descricao = (from cc in ctx.tblClientClassifications
                                 join ca in ctx.tblClassificationAttributes on cc.intAttributeID equals ca.intAttributeID
                                 where cc.intPersonID == matricula && cc.intClassificationID == idClassificacao
                                 select ca.txtDescription).FirstOrDefault();

                if (descricao == null)
                {
                    return new Turma();
                }

                var idTurma = Convert.ToInt32(descricao.Split('-')[0].ToString());

                var turma = GetDiasAula(idTurma, true);
                turma.ID = idTurma;
                return turma;
            }
        }

        public Turma TurmaMedAluno(int matricula, int anoInscricao)
        {
            using (var ctx = new DesenvContext())
            {
                var idTurma = (from so in ctx.tblSellOrders 
                                join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                join pro in ctx.tblProducts on sod.intProductID equals pro.intProductID
                                join cur in ctx.tblCourses on pro.intProductID equals cur.intCourseID
                                where so.intClientID == matricula
                                && cur.intYear == anoInscricao
                                && (pro.intProductGroup1 == (int)Produto.Produtos.MED
                                    || pro.intProductGroup1 == (int)Produto.Produtos.MEDEAD)
                                select cur.intCourseID).FirstOrDefault();

                var turma = GetDiasAula(idTurma, true);
                turma.ID = idTurma;
                return turma;
            }
        }

        public Turma GetDiasAula(int idTurma, bool turmaConvidada = false)
        {
            using (var ctx = new DesenvContext())
            {
                var turma = new Turma();
                var diasAula = new Turma().DiasAula = new List<DiasAula>();
                var intLessonType = new[] { (int)TipoLicaoEnum.EAD_MEDCURSO, (int)TipoLicaoEnum.EAD_MED };

                CultureInfo culture = new CultureInfo("pt-BR");
                DateTimeFormatInfo dtfi = culture.DateTimeFormat;

                var ret = new List<DiasAula>();

                if (!turmaConvidada)
                {
                    ret = (from mv in ctx.mview_Cronograma
                           join lt in ctx.tblLessonTitles on mv.intLessonTitleID equals lt.intLessonTitleID
                           where mv.intCourseID == idTurma
                           select new DiasAula
                           {
                               DataAula = mv.dteDateTime,
                               HoraInicioAula = mv.dteDateTime,
                               Duracao = mv.intDuration,
                               Tema = lt.txtLessonTitleName,
                               Tipo = mv.intLessonType
                           }).OrderBy(c => c.DataAula).Distinct().ToList();
                }
                else
                {
                    ret = (from l in ctx.tblLessons
                           join lt in ctx.tblLessonTitles on l.intLessonTitleID equals lt.intLessonTitleID
                           join lm in ctx.tblLesson_Material on l.intLessonID equals lm.intLessonID
                           join mv in ctx.mview_Cronograma on lm.intLessonID equals mv.intLessonID
                           join b in ctx.tblBooks on lm.intMaterialID equals b.intBookID
                           where mv.intCourseID == idTurma
                           && l.intCourseID == idTurma
                           && !intLessonType.Contains(mv.intLessonType)
                           && b.intBookEntityID == Constants.APOSTILATREINAMENTOTEORICOPRATICO
                           select new DiasAula
                           {
                               DataAula = mv.dteDateTime,
                               HoraInicioAula = mv.dteDateTime,
                               Duracao = mv.intDuration,
                               Tema = lt.txtLessonTitleName,
                               Tipo = mv.intLessonType
                           }).OrderBy(c => c.DataAula).Distinct().ToList();
                }

                if (ret.Count() == 0)
                {
                    return new Turma();
                }

                foreach (var item in ret)
                {
                    item.DiaSemana = Utilidades.FirstCharToUpper(dtfi.GetDayName(item.DataAula.DayOfWeek));
                    item.HoraFimAula = AulaHoraInicioFim(item.DataAula, item.Duracao);
                    item.DataAulaStr = item.DataAula.ToString("dd/MM");
                    item.HoraInicioAulaStr = item.HoraInicioAula.ToString("HH:mm");
                    item.HoraFimAulaStr = item.HoraFimAula.ToString("HH:mm");

                    diasAula.Add(item);
                }

                turma.DiasAula = diasAula;

                return turma;
            }
        }

        public DateTime AulaHoraInicioFim(DateTime inicio, int duracao)
        {
            var horaFim = inicio.AddMinutes(duracao);
            return horaFim;
        }

        public List<Turma> GetTurmasContratadas(int intClientID, int[] anos, int produto = 0, int adimplentes = 0)
        {
            anos = (anos == null) ? new[] { DateTime.Now.Year } : anos;
            var lanos = anos.ToList();
            //var pgAdimplente = (PagamentosClienteEntity.GetPagamentosCliente(intClientID, ano)).Where(pg =>pg.DblBalance == 0 && pg.Status == PagamentosCliente.StatusPagamento.OK).Select(p => p.intOrderID);
            //var pgPendente = (PagamentosClienteEntity.GetPagamentosCliente(intClientID, ano)).Where(pg => pg.DblSumOfDebits < 0 && pg.Status == PagamentosCliente.StatusPagamento.Pendente).Select(p => p.intOrderID);
            var pgCliente = (PagamentosClienteEntity.GetPagamentosCliente(intClientID, anos)).Any(pg => pg.DblSumOfDebits >= 0 || pg.Status == PagamentosCliente.StatusPagamento.OK);
            var enumFiliais = Enum.GetValues(typeof(Utilidades.Filiais));
            var filiais = new List<int>();

            foreach (var value in enumFiliais)
            {
                filiais.Add((int)value);
            }
            var ctx = new DesenvContext();
            var result = (from pessoas in ctx.tblClients
                          join ordemvenda in ctx.tblSellOrders on pessoas.intClientID equals ordemvenda.intClientID
                          join detalhesvenda in ctx.tblSellOrderDetails on ordemvenda.intOrderID equals detalhesvenda.intOrderID
                          join cursos in ctx.tblCourses on detalhesvenda.intProductID equals cursos.intCourseID
                          join produtos in ctx.tblProducts on cursos.intCourseID equals produtos.intProductID
                          join filialprodutospg in
                              (

                                  from cbprod in ctx.tblStore_CombosPaymentTemplate
                                  join prodcomb in ctx.tblProductCombos_Products on cbprod.intComboID equals prodcomb.intComboID
                                  select new { intProductId = prodcomb.intProductID, bitActive = cbprod.bitActive, bitInternet = cbprod.bitInternet }).Union(

                                  from stprod in ctx.tblStore_Product_PaymentTemplate
                                  select new { intProductId = stprod.intProductID, bitActive = stprod.bitActive, bitInternet = stprod.bitInternet }
                                  ).Distinct()
                              on produtos.intProductID equals filialprodutospg.intProductId


                          //join filialprodutospg in ctx.tblStore_Product_PaymentTemplate on produtos.intProductID equals filialprodutospg.intProductID
                          //join cr in ctx.mview_Cronograma on cursos.intCourseID equals cr.intCourseID into dates
                          where pessoas.intClientID == intClientID
                          && (produto == 0 || produto == produtos.intProductGroup1)
                          && (lanos.Contains(cursos.intYear ?? 0))
                          && ((bool)filialprodutospg.bitActive
                          || (bool)filialprodutospg.bitInternet)
                          && (
                                 ((adimplentes == 0) && (
                                 ordemvenda.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                 || ordemvenda.intStatus == (int)OrdemVenda.StatusOv.Carencia
                                 || (
                                     ordemvenda.intStatus == (int)OrdemVenda.StatusOv.Cancelada
                                     && pgCliente
                                 )
                                 )) || ((ordemvenda.intStatus == (int)OrdemVenda.StatusOv.Ativa) && ordemvenda.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente)

                             )
                          //&& filiais.Contains(ordemvenda.intStoreID)

                          select new Turma { 
                              IdProduto = produtos.intProductGroup1 ?? 0,
                              ID = produtos.intProductID, 
                              Nome = produtos.txtName, 
                              Inicio = ctx.mview_Cronograma.Where(x => x.intCourseID == cursos.intCourseID).Min(y => y.dteDateTime),  //(dates.Count() > 0) ? dates.Min(d => d.dteDateTime) : new DateTime(), 
                              Fim = ctx.mview_Cronograma.Where(x => x.intCourseID == cursos.intCourseID).Max(y => y.dteDateTime)
                            });
            var lst = new Turmas();
            lst.AddRange(result.Distinct().ToList());
            return lst;
        }

        public List<TurmaDTO> GetTurmasCronograma(int idFilial, int anoLetivoAtual)
        {
                       var turmaEmAnalise = 7;
            var turmaX = 14;
            var lstLessonTypes = new List<int>() { turmaEmAnalise, turmaX };
            var lstProdutosID = new List<int>() { 
                (int)Utilidades.ProductGroups.MEDCURSO,
                (int)Utilidades.ProductGroups.MED,
                (int)Utilidades.ProductGroups.INTENSIVO,
                (int)Utilidades.ProductGroups.CPMED,
                (int)Utilidades.ProductGroups.MEDELETRO,
                (int)Utilidades.ProductGroups.RAC };

            using (var ctx = new DesenvContext())
            {
                var lstTurmas = (from c in ctx.tblCourses
                                 join cr in ctx.tblClassRooms on c.intPrincipalClassRoomID equals cr.intClassRoomID
                                 join pr in ctx.tblProducts on c.intCourseID equals pr.intProductID
                                 join mv in ctx.mview_ProdutosPorFilial on pr.intProductID equals mv.intProductID
                                 join le in ctx.tblLessons on pr.intProductID equals le.intCourseID
                                 where c.intYear == anoLetivoAtual
                                       && mv.intStoreID == idFilial
                                     //  && lstLessonTypes.Contains(le.intLessonType)
                                       && lstProdutosID.Contains(pr.intProductGroup1 ?? 0)
                                 select new TurmaDTO()
                                 {
                                     Id = cr.intClassRoomID,
                                     Nome = cr.txtDescription
                                 }
                               ).Distinct().ToList();

                lstTurmas.ForEach(x => x.Nome = x.Nome.Replace("Local " + anoLetivoAtual, "").Trim());

                return lstTurmas;
        }
    }

        public List<Turma> GetByFilters(Turma registro)
        {
            throw new NotImplementedException();
        }

        public List<Turma> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Insert(Turma registro)
        {
            throw new NotImplementedException();
        }

        public int Update(Turma registro)
        {
            throw new NotImplementedException();
        }

        public int Delete(Turma registro)
        {
            throw new NotImplementedException();
        }
    }
}