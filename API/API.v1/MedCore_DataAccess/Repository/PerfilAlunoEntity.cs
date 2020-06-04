using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Entidades;
using System.Linq;
using System.Collections.Generic;
using MedCore_API.Academico;
using System.Threading.Tasks;

namespace MedCore_DataAccess.Repository
{
    public class PerfilAlunoEntity : IPerfilAlunoData
    {
        public bool IsAlunoR3(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var produtosR3 = Utilidades.ProdutosR3();
                var anoAtual = Utilidades.GetYear();

                bool IsR3 = (from so in ctx.tblSellOrders
                            join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                            join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                            join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                            where produtosR3.Contains(p.intProductGroup2 ?? 0)
                            && ((so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && ((so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente)
                                || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Carencia )
                                || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente)))
                                || (so.intStatus == (int)OrdemVenda.StatusOv.Cancelada))
                            && so.intClientID == matricula
                            select new Aluno
                            {
                                ID = so.intClientID
                            }).Any();                    
                    
            return IsR3;
            }
        }

        public bool IsAlunoMedEletroIMed(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var produtosIMed = Produto.Produtos.MEDELETRO_IMED.GetHashCode();
                var anoAtual = Utilidades.GetYear();

                bool IsMedEletroIMed = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             where p.intProductGroup1 == produtosIMed
                              && ((so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                  && ((so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente)
                                  || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Carencia )
                                  || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente)))
                                || (so.intStatus == (int)OrdemVenda.StatusOv.Cancelada))
                              && so.intClientID == matricula
                             select new Aluno
                             {
                                 ID = so.intClientID
                             }).Any();

                return IsMedEletroIMed;
            }
        }

        public Task<bool> IsAlunoExtensivoAsync(string register)
        {
            return Task.Factory.StartNew(() => IsAlunoExtensivo(register));
        }

        public bool IsAlunoExtensivo(string register)
        {
            var produtosExtensivo = new int[]
            {
                (int)Produto.Produtos.MED,
                (int)Produto.Produtos.MEDCURSO,
                (int)Produto.Produtos.MEDEAD,
                (int)Produto.Produtos.MEDCURSOEAD,
                (int)Produto.Produtos.INTENSIVAO,
                (int)Produto.Produtos.CPMED
            };

            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                return (from so in ctx.tblSellOrders
                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                        join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                        join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                        where p.txtRegister == register && c.intYear == ano
                            && produtosExtensivo.Contains(pr.intProductGroup1 ?? 0)
                            && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                            && (so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                || (c.intYear == ano && so.intStatus2 == (int)OrdemVenda.StatusOv.Carencia)
                                || (c.intYear == ano && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente))
                        select new Aluno
                        {
                            ID = so.intClientID,
                        }).Any();
            }
        }

        public bool AlunoTemInteresseRMais(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                string R3 = "R3";
                string R4 = "R4";
                var clienteTemItenresseR3 = (from c in ctx.tblClients
                                            where (c.txtArea == R3 || c.txtArea == R4) && c.intClientID == matricula
                                            select 
                                            c.intClientID
                                            ).Any();                
                return clienteTemItenresseR3;
            }
        }        

        public KeyValuePair<int, int> GetAlunoComQuestaoFavoritada(int tipoExercicio)
        {
            using (var ctx = new AcademicoContext())
            {
                var questao = (from q in ctx.tblQuestao_Marcacao
                               where q.intTipoExercicioID == tipoExercicio && q.bitFlagFavorita
                               orderby q.intID descending
                               select new
                               {
                                   q.intClientID,
                                   q.intQuestaoID
                               }).FirstOrDefault();

                var alunoQuestao = new KeyValuePair<int, int>(questao.intClientID, questao.intQuestaoID);
                return alunoQuestao;
            }

        }

        public KeyValuePair<int, int> GetAlunoComQuestaoAnotada(int tipoExercicio)
        {
            using (var ctx = new AcademicoContext())
            {
                var questao = (from q in ctx.tblQuestao_Marcacao
                               where q.intTipoExercicioID == tipoExercicio && q.txtAnotacao != null
                               orderby q.intID descending
                               select new
                               {
                                   q.intClientID,
                                   q.intQuestaoID
                               }).FirstOrDefault();

                var alunoQuestao = new KeyValuePair<int, int>(0, 0);
                if (questao != null) alunoQuestao = new KeyValuePair<int, int>(questao.intClientID, questao.intQuestaoID);
                return alunoQuestao;
            }
        }

        public Aluno GetAlunoExtensivoSomenteAnoAtualAtivoSelecionouEspecialidadeRevalida()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {

                var alunosAnoAtual = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             join cl in ctx.tblClients on p.intContactID equals cl.intClientID
                             where (pr.intProductGroup1 == (int)Produto.Produtos.MED || pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                   && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                   && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                   && c.intYear == ano && cl.intEspecialidadeID == Constants.ID_ESPECIALIDADE_REVALIDA
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             });

                var alunosAnosAnteriores = (from so in ctx.tblSellOrders
                                      join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                      join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                      join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                      join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                      join cl in ctx.tblClients on p.intContactID equals cl.intClientID
                                      where (pr.intProductGroup1 == (int)Produto.Produtos.MED || pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                            && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                            && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                            && c.intYear != ano && cl.intEspecialidadeID == Constants.ID_ESPECIALIDADE_REVALIDA
                                      orderby so.dteDate descending
                                      select new Aluno
                                      {
                                          ID = so.intClientID,
                                          Register = p.txtRegister
                                      }).Select(x => x.ID);

                var aluno = alunosAnoAtual.Where(x => !alunosAnosAnteriores.Contains(x.ID)).FirstOrDefault();

                return aluno;
            }
        }


        public Aluno GetAlunoR3Cancelado()
        {
            using (var ctx = new DesenvContext())
            {
                var anoAtual = Utilidades.GetYear();
                var produtosR3 = Utilidades.ProdutosR3();
                var alunoR3 = (from so in ctx.tblSellOrders
                               join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                               join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                               join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                               join p in ctx.tblPersons on so.intClientID equals p.intContactID
                               where produtosR3.Contains(pr.intProductGroup2 ?? 0)
                               && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                               && c.intYear == anoAtual
                               orderby so.dteDate descending
                               select new Aluno
                               {
                                   ID = so.intClientID,
                                   Register = p.txtRegister
                               }).FirstOrDefault();

                return alunoR3;
            }
        }
    }
}