using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using System.Linq;
using System;
using MedCore_DataAccess.Contracts.Enums;
using System.Data.SqlClient;

namespace MedCore_DataAccessTests.EntitiesDataTests
{
    public class PerfilAlunoEntityTestData
    {
        private static int filialMEDREADER = 187;
        private static int filialEAD = 97;

        public Aluno GetAlunoExtensivoAnoAtualAtivo()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             join senha in ctx.tblPersons_Passwords on p.intContactID equals senha.intContactID
                             where (pr.intProductGroup1 == (int)Produto.Produtos.MED || pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                    && c.intYear == ano
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister,
                                 Senha = senha.txtPassword
                             }).FirstOrDefault();
                return aluno;
            }

       }
        public Produto GetProdutoPorTurma(int courseId)
        {          
            using (var ctx = new DesenvContext())
            {
                var produto = (from p in ctx.tblProducts
                               where (p.intProductID == courseId)
                               select new Produto { ID = p.intProductID, GrupoProduto1 = p.intProductGroup1 ?? 0 }).FirstOrDefault();

                if (produto.GrupoProduto1 == (int)Produto.Produtos.MED || produto.GrupoProduto1 == (int)Produto.Produtos.MEDEAD)
                    produto.GrupoProduto2 = (int)Produto.Cursos.MED;
                else if (produto.GrupoProduto1 == (int)Produto.Produtos.MEDCURSO || produto.GrupoProduto1 == (int)Produto.Produtos.MEDCURSOEAD)
                    produto.GrupoProduto2 = (int)Produto.Cursos.MEDCURSO;

                return produto;
            }
        }




        public Aluno GetAlunoMedcursoAnoAtualAtivo()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             join senha in ctx.tblPersons_Passwords on p.intContactID equals senha.intContactID
                             where pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                    && c.intYear == ano
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).FirstOrDefault();
                return aluno;
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

        public Aluno GetAlunoExtensivoAnoAtualAtivoSelecionouEspecialidadeRevalidaSeInscreveuEm2018()
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
                                            && so.dteDate.Value.Year == (ano - 1)
                                      orderby so.dteDate descending
                                      select new Aluno
                                      {
                                          ID = so.intClientID,
                                          Register = p.txtRegister
                                      });

                var aluno = alunosAnoAtual.FirstOrDefault();

                return aluno;
            }
        }


        public int GetMatriculaAlunoExtensivoAnoAtualAtivo_SomenteUmaOV()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join so2 in ctx.tblSellOrders on so.intClientID equals so2.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             where (pr.intProductGroup1 == (int)Produto.Produtos.MED || pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                    && c.intYear == ano
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID
                             }).ToList()
                             .GroupBy(x => x.ID)
                             .Select(x => new { ID = x.Key, Qtd = x.ToList() })
                             .Where(x => x.Qtd.Count() == 1)
                             .FirstOrDefault();
                return aluno.ID;
            }

        }

        public int GetMatriculaAluno_SomenteUmaOV(int ano, int productGroup1, int ovStatus1, int ovStatus2)
        {
            
            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join so2 in ctx.tblSellOrders on so.intClientID equals so2.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join sod2 in ctx.tblSellOrderDetails on so.intOrderID equals sod2.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             where pr.intProductGroup1 == productGroup1
                                    && so.intStatus == ovStatus1
                                    && so.intStatus2 == ovStatus2
                                    && c.intYear == ano
                             orderby so.dteDate descending
                             select new
                             {
                                 ID = so.intClientID
                             }).ToList()
                             .GroupBy(x => x.ID)
                             .Select(x => new { ID = x.Key, Qtd = x.ToList() })
                             .Where(x => x.Qtd.Count() == 1)
                             .FirstOrDefault();

                return aluno.ID;
            }

        }

        public int GetMatriculaAlunoCanceladoSemPagamento_SomenteUmaOV(int ano, int productGroup1)
        {

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join so2 in ctx.tblSellOrders on so.intClientID equals so2.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join sod2 in ctx.tblSellOrderDetails on so.intOrderID equals sod2.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join ad in ctx.tblAccountData on so.intOrderID equals ad.intOrderID
                             where pr.intProductGroup1 == productGroup1
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Cancelada
                                    && so.intStatus2 == (int)OrdemVenda.StatusOv.Cancelada
                                    && c.intYear == ano
                                    && ad.dblValue <= 0
                             orderby so.dteDate descending
                             select new
                             {
                                 ID = so.intClientID
                             }).FirstOrDefault();

                return aluno.ID;
            }

        }

        public int GetMatriculaAluno(int ano, int productGroup1, int ovStatus1, int ovStatus2)
        {
            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join so2 in ctx.tblSellOrders on so.intClientID equals so2.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join sod2 in ctx.tblSellOrderDetails on so.intOrderID equals sod2.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             where pr.intProductGroup1 == productGroup1
                                    && so.intStatus == ovStatus1
                                    && so.intStatus2 == ovStatus2
                                    && c.intYear == ano
                             orderby so.dteDate descending
                             select new
                             {
                                 ID = so.intClientID
                             }).FirstOrDefault();

                return aluno == null ? 0 : aluno.ID;
            }
        }
        public Aluno GetAluno(int ano, int productGroup1, int ovStatus1, int ovStatus2)
        {

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join so2 in ctx.tblSellOrders on so.intClientID equals so2.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join sod2 in ctx.tblSellOrderDetails on so.intOrderID equals sod2.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join ps in ctx.tblPersons on so.intClientID equals ps.intContactID
                             where pr.intProductGroup1 == productGroup1
                                    && so.intStatus == ovStatus1
                                    && so.intStatus2 == ovStatus2
                                    && c.intYear == ano
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = ps.txtRegister
                             }).ToList()
                             .FirstOrDefault();

                return aluno;
            }

        }

        public Aluno GetAlunoSomenteUmaOV(int ano, int productGroup1, int ovStatus1, int ovStatus2, bool todas = false, bool excluiMeioDeAno = false)
        {

               using (var ctx = new DesenvContext())
                {
                    var alunos = (from so in ctx.tblSellOrders
                             join so2 in ctx.tblSellOrders on so.intClientID equals so2.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join sod2 in ctx.tblSellOrderDetails on so.intOrderID equals sod2.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             join pass in ctx.tblPersons_Passwords on p.intContactID equals pass.intContactID
                             where pr.intProductGroup1 == productGroup1
                                    && so.intStatus == ovStatus1
                                    && so.intStatus2 == ovStatus2
                                    && c.intYear == ano
                                    && (todas || so2.intStatus == ovStatus1)
                             orderby so.dteDate descending
                             select new
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).ToList()
                             .GroupBy(x => new { x.ID, x.Register })
                             .Select(x => new { x.Key.ID, x.Key.Register, Qtd = x.ToList() })
                             .Where(x => x.Qtd.Count() == 1)
                             .Select(a => new Aluno { ID = a.ID, Register = a.Register });



                    if (!excluiMeioDeAno)
                    return alunos.FirstOrDefault();
                    else
                    {
                        var alunosMeioAno = ctx.tblAlunosAnoAtualMaisAnterior.Select(x => x.intClientID).ToList();
                        return alunos.Where(x => !alunosMeioAno.Contains(x.ID)).FirstOrDefault();
                    }
               }
        }


       

        public Aluno GetAlunoAdaptaAnoAtualAtivo()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where pr.intProductGroup1 == (int)Produto.Produtos.ADAPTAMED
                                   && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                   && c.intYear == ano
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).FirstOrDefault();
                return aluno;
            }
        }

        public Aluno GetAlunoAdaptaAnoAnteriorAtivo()
        {
            var ano = Utilidades.GetYear() -1;

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where pr.intProductGroup1 == (int)Produto.Produtos.ADAPTAMED
                                   && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                   && c.intYear == ano
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).FirstOrDefault();
                return aluno;
            }
        }

        public Aluno GetAlunoSomenteMedEletroAnoAtual()
        {
            var ano = Utilidades.GetYear();
            var aluno = new Aluno();

            using (var ctx = new DesenvContext())
            {
                aluno = (from so in ctx.tblSellOrders
                         join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                         join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                         join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                         join p in ctx.tblPersons on so.intClientID equals p.intContactID
                         join pw in ctx.tblPersons_Passwords on p.intContactID equals pw.intContactID
                         where pr.intProductGroup1 == (int)Produto.Produtos.MEDELETRO
                               && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                               && c.intYear == ano
                               select new Aluno
                         {
                             ID = so.intClientID,
                             Register = p.txtRegister,
                             Senha = pw.txtPassword
                         }).FirstOrDefault();

            }

            return aluno;
        }

        public Aluno GetAlunoCpMedAnoAtualAtivo()
        {

            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where pr.intProductGroup1 == (int)Produto.Produtos.CPMED
                                   && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                   && c.intYear == ano
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).FirstOrDefault();
                return aluno;
            }
        }


        public Aluno GetAlunoCpMedAnoAnterior()
        {

            var ano = Utilidades.GetYear() - 1;

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where pr.intProductGroup1 == (int)Produto.Produtos.CPMED
                                   && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                   && c.intYear == ano
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).FirstOrDefault();
                return aluno;
            }
        }


        public Aluno GetAlunoIntensivoAnoAnteriorAtivoSelecionouEspecialidadeRevalida()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {

                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             join cl in ctx.tblClients on p.intContactID equals cl.intClientID
                             where pr.intProductGroup1 == (int)Produto.Produtos.INTENSIVAO && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                   && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                   && c.intYear == (ano - 1) && cl.intEspecialidadeID == Constants.ID_ESPECIALIDADE_REVALIDA
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).FirstOrDefault();
                return aluno;
            }
        }


        public Aluno GetAlunoExtensivoAtivoSelecionouEspecialidadeRevalida(int ano)
        {

            using (var ctx = new DesenvContext())
            {

                var aluno = (from so in ctx.tblSellOrders
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
                             }).FirstOrDefault();
                return aluno;
            }
        }

        public Aluno GetAlunoExtensivoAnoAtualAtivoSelecionouAreaRevalida()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {

                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             join cl in ctx.tblClients on p.intContactID equals cl.intClientID
                             where (pr.intProductGroup1 == (int)Produto.Produtos.MED || pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                   && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                   && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                   && c.intYear == ano && cl.txtArea.Contains(Constants.TEXT_AREA_REVALIDA)
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).FirstOrDefault();
                return aluno;
            }
        }


        public Aluno GetAlunoExtensivoEMedEletroAnoAtualAtivo()
        {
            var lstMed = GetAlunosMedAnoAtualAtivo();
            var lstMedEletro = GetAlunosMedEletroAnoAtualAtivo();

            var lstAluno = new List<Aluno>();

            foreach (var item in lstMed)
            {
                var aluno = lstMedEletro.Find(l => l.ID == item.ID);
                if (aluno != null)
                {
                    lstAluno.Add(item);
                }
            }

            var alunoAtivo = lstAluno.FirstOrDefault();

            return alunoAtivo;
        }

        public List<Aluno> GetAlunosMedAnoAtualAtivo()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from so in ctx.tblSellOrders
                                join sode in ctx.tblSellOrderDetails on so.intOrderID equals sode.intOrderID
                                join prod in ctx.tblProducts on sode.intProductID equals prod.intProductID
                                join cour in ctx.tblCourses on prod.intProductID equals cour.intCourseID
                                join pe in ctx.tblPersons on so.intClientID equals pe.intContactID
                                where
                                prod.intProductGroup1 == (int)Produto.Produtos.MED
                                && cour.intYear == ano
                                && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                orderby so.intOrderID descending
                                select new Aluno
                                {
                                    ID = so.intClientID,
                                    Register = pe.txtRegister
                                }).ToList();
                return lstaluno;
            }
        }

        public List<Aluno> GetAlunosMedAtivo(int ano)
        {
            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from so in ctx.tblSellOrders
                                join sode in ctx.tblSellOrderDetails on so.intOrderID equals sode.intOrderID
                                join prod in ctx.tblProducts on sode.intProductID equals prod.intProductID
                                join cour in ctx.tblCourses on prod.intProductID equals cour.intCourseID
                                join pe in ctx.tblPersons on so.intClientID equals pe.intContactID
                                where
                                prod.intProductGroup1 == (int)Produto.Produtos.MED
                                && cour.intYear == ano
                                && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                orderby so.intOrderID descending
                                select new Aluno
                                {
                                    ID = so.intClientID,
                                    Register = pe.txtRegister
                                }).ToList();
                return lstaluno;
            }
        }

        public List<Aluno> GetAlunosMedEletroAnoAtualAtivo()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from sel in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on sel.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                join pe in ctx.tblPersons on sel.intClientID equals pe.intContactID
                                where pr.intProductGroup1 == (int)Produto.Produtos.MEDELETRO
                                && c.intYear == ano
                                && sel.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && sel.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                select new Aluno
                                {
                                    ID = sel.intClientID,
                                    Register = pe.txtRegister
                                }).ToList();

                return lstaluno;
            }
        }

        public List<Aluno> GetAlunosMedEletroAnoAnteriorAtivo()
        {
            var ano = Utilidades.GetYear() -1;

            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from sel in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on sel.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                join pe in ctx.tblPersons on sel.intClientID equals pe.intContactID
                                where pr.intProductGroup1 == (int)Produto.Produtos.MEDELETRO
                                && c.intYear == ano
                                && sel.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && sel.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                select new Aluno
                                {
                                    ID = sel.intClientID,
                                    Register = pe.txtRegister
                                }).ToList();

                return lstaluno;
            }
        }


        public Aluno GetAlunoExtensivoAnoAtualInadimplente()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where (pr.intProductGroup1 == (int)Produto.Produtos.MED || pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente
                                    && c.intYear == ano
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).FirstOrDefault();
                return aluno;

            }
        }


        public Aluno GetAlunoPlanejamentoDoisAnos(int anoMed, int anoMedCurso)
        {
            using (var ctx = new DesenvContext())
            {
                var txtAnoMed = anoMed.ToString();
                var txtAnoMedCurso = anoMedCurso.ToString();

                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             where so.txtComment.Contains(txtAnoMed + " med ") && so.txtComment.Contains(txtAnoMedCurso + " medcurso")
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                    && so.intStoreID != filialMEDREADER
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                             }).FirstOrDefault();

                return aluno;
            }
        }

        public int? GetAlunoSomentePlanejamentoDoisAnos(int anoMed, int anoMedCurso)
        {
            using (var ctx = new DesenvContext())
            {
                var txtAnoMed = anoMed.ToString();
                var txtAnoMedCurso = anoMedCurso.ToString();

                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             where so.txtComment.Contains(txtAnoMed + " med ") && so.txtComment.Contains(txtAnoMedCurso + " medcurso")
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                    && so.intStoreID != filialMEDREADER
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                             }).ToList()
                             .GroupBy(x => x.ID)
                             .Select(x => new { ID = x.Key, Qtd = x.ToList() })
                             .Where(x => x.Qtd.Count() == 1)
                             .FirstOrDefault();

                return aluno != null ? (int?)aluno.ID : null;
            }
        }

        public Aluno GetAlunoMedMedCursoAdaptaMed(int anoMed, int anoMedCurso)
        {
            using (var ctx = new DesenvContext())
            {
                var txtAnoMed = anoMed.ToString();
                var txtAnoMedCurso = anoMedCurso.ToString();

                var alunoconsulta = (from so in ctx.tblSellOrders
                                     join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                     join pdt in ctx.tblProducts on sod.intProductID equals pdt.intProductID
                                     join c in ctx.tblCourses on pdt.intProductID equals c.intCourseID
                                     where so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                            && so.intStoreID != filialMEDREADER
                                            && c.intYear == anoMed
                                            && (pdt.intProductGroup1 == (int)Produto.Produtos.MEDCURSO || pdt.intProductGroup1 == (int)Produto.Produtos.MED || pdt.intProductGroup1 == (int)Produto.Produtos.ADAPTAMED)
                                     group so by new { so.intClientID } into g
                                     where g.Count() > 3
                                     select new { g.Key });

                var retorno = alunoconsulta.FirstOrDefault();
                Aluno aluno = new Aluno
                {
                    ID = Convert.ToInt32(retorno.Key.intClientID)
                };


                return aluno;
            }
        }

        public Aluno GetAlunoMedMedCursoAdaptaMedRevalida(int anoMed, int anoMedCurso)
        {
            using (var ctx = new DesenvContext())
            {
                var txtAnoMed = anoMed.ToString();
                var txtAnoMedCurso = anoMedCurso.ToString();

                var alunoconsulta = (from so in ctx.tblSellOrders
                                     join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                     join pdt in ctx.tblProducts on sod.intProductID equals pdt.intProductID
                                     join c in ctx.tblCourses on pdt.intProductID equals c.intCourseID
                                     where so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                            && so.intStoreID != filialMEDREADER
                                            && c.intYear == anoMed
                                            && (pdt.intProductGroup1 == (int)Produto.Produtos.MEDCURSO || pdt.intProductGroup1 == (int)Produto.Produtos.MED || pdt.intProductGroup1 == (int)Produto.Produtos.ADAPTAMED || pdt.intProductGroup1 == (int)Produto.Produtos.REVALIDA)
                                     group so by new { so.intClientID } into g
                                     where g.Count() > 3
                                     select new { g.Key });

                var retorno = alunoconsulta.FirstOrDefault();
                Aluno aluno = new Aluno
                {
                    ID = Convert.ToInt32(retorno.Key.intClientID)
                };


                return aluno;
            }
        }

        public Aluno GetAlunoPlanejamentoDoisAnosEAD(int anoMed, int anoMedCurso)
        {
            using (var ctx = new DesenvContext())
            {
                var txtAnoMed = anoMed.ToString();
                var txtAnoMedCurso = anoMedCurso.ToString();

                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             where
                             so.txtComment.Contains(txtAnoMed + " ead med ") && so.txtComment.Contains(txtAnoMedCurso + " ead medcurso")
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                    && so.intStoreID != filialMEDREADER
                                    && so.intStoreID == filialEAD
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                             }).FirstOrDefault();

                return aluno;
            }
        }

        public Aluno GetAlunoComCPMEDRSemCPMED(int ano)
        {
            using (var ctx = new DesenvContext())
            {
                var alunosQueTemCPMEDR = (from so in ctx.tblSellOrders
                                          join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                          join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                          join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                          where
                                          pr.intProductGroup1 == (int)Produto.Produtos.CPMED && pr.intProductGroup2 != (int)Produto.Produtos.APOSTILA_CPMED
                                          && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && c.intYear == ano
                                          orderby so.dteDate descending
                                          select new Aluno
                                          {
                                              ID = so.intClientID,
                                          }).ToList();

                var alunosQueTemCPMED = (from so in ctx.tblSellOrders
                                         join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                         join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                         join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                         where
                                         pr.intProductGroup1 == (int)Produto.Produtos.CPMED && pr.intProductGroup2 == (int)Produto.Produtos.APOSTILA_CPMED
                                         && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && c.intYear == ano
                                         orderby so.dteDate descending
                                         select new Aluno
                                         {
                                             ID = so.intClientID,
                                         }).ToList();

                var alunoComCPMEDRSemCPMED = alunosQueTemCPMEDR.Where(a => !alunosQueTemCPMED.Contains(new Aluno { ID = a.ID })).FirstOrDefault();

                return alunoComCPMEDRSemCPMED;
            }
        }

        public Aluno GetAlunoComCPMED(int ano)
        {
            using (var ctx = new DesenvContext())
            {
                var alunosQueTemCPMED = (from so in ctx.tblSellOrders
                                         join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                         join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                         join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                         where
                                         pr.intProductGroup1 == (int)Produto.Produtos.CPMED && pr.intProductGroup2 == (int)Produto.Produtos.APOSTILA_CPMED
                                         && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && c.intYear == ano
                                         orderby so.dteDate descending
                                         select new Aluno
                                         {
                                             ID = so.intClientID,
                                         }).FirstOrDefault();

                return alunosQueTemCPMED;
            }
        }

        public Aluno GetAlunoComOvPeriodoInscricoesExtensivo2019()
        {
            var dataInicioPeriodoInscricoesExtensivo2019 = new DateTime(2018, 10, 15);
            using (var ctx = new DesenvContext())
            {
                var produtosExtensivo = new List<int> { (int)Produto.Produtos.MEDEAD, (int)Produto.Produtos.MEDCURSOEAD, (int)Produto.Produtos.MEDCURSO, (int)Produto.Produtos.MED };
                var alunoComOvDurantePeriodoInscricoesExtensivo =
                    (from so in ctx.tblSellOrders
                     join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                     join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                     join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                     join person in ctx.tblPersons on so.intClientID equals person.intContactID
                     where
                     produtosExtensivo.Contains(p.intProductGroup1 ?? 0)
                     && so.dteDate >= dataInicioPeriodoInscricoesExtensivo2019
                     select new Aluno
                     {
                         ID = person.intContactID
                     }).FirstOrDefault();

                return alunoComOvDurantePeriodoInscricoesExtensivo;
            }
        }

        public Aluno GetAlunoAcademico()
        {
            using (var ctx = new DesenvContext())
            {
                var alunoAcademico = (from so in ctx.tblSellOrders
                                      join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                      where so.txtComment.Contains("Medreader")
                                      orderby so.dteDate //descending
                                      select new Aluno
                                      {
                                          ID = so.intClientID,
                                      }).FirstOrDefault();

                return alunoAcademico;
            }
        }

        public Aluno GetAlunoR3(int ano = 0)
        {
            using (var ctx = new DesenvContext())
            {
                var anoAluno = ano == 0 ? Utilidades.GetYear() : ano;

                var alunoR3 = (from so in ctx.tblSellOrders
                               join p in ctx.tblPersons on so.intClientID equals p.intContactID
                               join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                               join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                               join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                               where so.txtComment.Contains("R3")
                               && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                               && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                               && (ano == 0 || c.intYear == anoAluno)
                               && so.dteDate <= new DateTime(anoAluno, 02, 01)
                               orderby so.dteDate descending
                               select new Aluno
                               {
                                   ID = so.intClientID,
                                   Register = p.txtRegister
                               }).FirstOrDefault();

                return alunoR3;
            }
        }

        public Aluno GetAlunoR3ComCarencia()
        {
            using (var ctx = new DesenvContext())
            {
                var alunoR3 = (from so in ctx.tblSellOrders
                               join p in ctx.tblPersons on so.intClientID equals p.intContactID
                               where so.txtComment.Contains("R3")
                               && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                               && so.intStatus2 == (int)Utilidades.ESellOrderStatus.Carencia
                               orderby so.dteDate descending
                               select new Aluno
                               {
                                   ID = so.intClientID,
                                   Register = p.txtRegister
                               }).FirstOrDefault();

                return alunoR3;
            }
        }




        public List<Aluno> GetAlunosAcademico()
        {
            using (var ctx = new DesenvContext())
            {
                var alunosAcademicos = (from so in ctx.tblSellOrders
                                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                        where so.txtComment.Contains("Medreader")
                                        orderby so.dteDate descending
                                        select new Aluno
                                        {
                                            ID = so.intClientID,
                                        }).ToList();

                return alunosAcademicos;
            }
        }

        public int GetMatriculoAlunoAcademicoVimeo()
        {
            using (var ctx = new DesenvContext())
            {
                var alunosAcademicosSomente2019 = (from so in ctx.tblSellOrders
                                                   join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                                   where (so.txtComment.Contains("2019") && so.txtComment.Contains("Medreader"))
                                                   orderby so.dteDate descending
                                                   select new Aluno
                                                   {
                                                       ID = so.intClientID,
                                                   });

                var idsAlunos = alunosAcademicosSomente2019.Select(X => X.ID);

                var alunosAcademicos = (from so in ctx.tblSellOrders
                                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                        where (so.txtComment.Contains("2018") && so.txtComment.Contains("Medreader"))
                                        && idsAlunos.Contains(so.intClientID)
                                        orderby so.dteDate descending
                                        select new Aluno
                                        {
                                            ID = so.intClientID,
                                        });

                foreach (var aluno in alunosAcademicos)
                {
                    var permitidos = (from p in ctx.msp_Medsoft_SelectPermissaoExercicios(false, false, aluno.ID)
                                      join b in ctx.tblBooks on p.intExercicioID equals b.intBookEntityID
                                      where b.intBookEntityID != null && p.intExercicioTipo == 3
                                      select b.intBookEntityID.Value).Distinct().ToList();
                    if (permitidos.Count > 0)
                        return aluno.ID;
                }
            }
            return 0;
        }



        public List<Aluno> GetAlunosTurmaDevicesAtivos(int courseId)
        {
            using (var ctx = new DesenvContext())
            {
                var alunosTurma = (from so in ctx.tblSellOrders
                                   join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                   join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                   join dt in ctx.tblDeviceToken on p.intContactID equals dt.intClientID
                                   where sod.intProductID == courseId
                                    && dt.bitAtivo == true
                                    && (dt.intApplicationId == null
                                        || (dt.intApplicationId.HasValue && dt.intApplicationId.Value == (int)Aplicacoes.MsProMobile)
                                    )
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                    && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                   orderby so.dteDate descending
                                   select new Aluno
                                   {
                                       ID = so.intClientID,
                                   }).ToList();

                return alunosTurma;
            }
        }

        public int GetAlunoComRegraExcecaoSlideAulas()
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblAlunoExcecaoSlideAulas.Any() ? ctx.tblAlunoExcecaoSlideAulas.FirstOrDefault().intClientID : 0;
            }
        }

        public int GetAlunoAnoAtualComAnosAnteriores()
        {
            string anoAtual = DateTime.Now.ToString("yyyy");
            string anoLancamento = Utilidades.AnoLancamentoMedsoftPro.ToString();
            using (var ctx = new DesenvContext())
            {

                var alunosAcademicosSomente2019 = (from so in ctx.tblSellOrders
                                                   join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                                   where so.txtComment.Contains(anoAtual)
                                                   && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                                   && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                                   && !p.txtName.ToLower().Contains("academico")
                                                   orderby so.dteDate descending
                                                   select new Aluno
                                                   {
                                                       ID = so.intClientID,
                                                   });

                var idsAlunos = alunosAcademicosSomente2019.Select(X => X.ID);

                var alunoAcademico = (from so in ctx.tblSellOrders
                                      join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                      where so.txtComment.Contains(anoLancamento)
                                      && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                      && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                      && idsAlunos.Contains(so.intClientID)
                                      orderby so.dteDate descending
                                      select new Aluno
                                      {
                                          ID = so.intClientID

                                      }).FirstOrDefault();

                return alunoAcademico.ID;
            }

        }



        public tblSeguranca GetAcessoAplicacao(Aplicacoes app, Utilidades.TipoDevice device)
        {
            using (var ctx = new DesenvContext())
            {
                var alunoDesktop = ctx.tblSeguranca
                    .Where(x => x.intDeviceId.Equals((int)device) && x.intApplicationId.Equals((int)app))
                    .ToList()
                    .OrderByDescending(d => d.dteCadastro)
                    .FirstOrDefault();

                return alunoDesktop;
            }
        }

        public List<Aluno> GetAlunosStatus2(Produto.Produtos produto = Produto.Produtos.NAO_DEFINIDO, OrdemVenda.StatusOv status2 = OrdemVenda.StatusOv.Adimplente, int courseId = 0)
        {
            using (var ctx = new DesenvContext())
            {
                var alunosTurma = (from so in ctx.tblSellOrders
                                   join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                   join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                   join pd in ctx.tblProducts on sod.intProductID equals pd.intProductID
                                   where ((int)produto == (int)Produto.Produtos.NAO_DEFINIDO || pd.intProductGroup1 == (int)produto)
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                    && so.intStatus2 == (int)status2
                                    && (courseId == 0 || courseId == pd.intProductID)
                                   orderby so.dteDate descending
                                   select new Aluno
                                   {
                                       ID = so.intClientID,
                                       Register = p.txtRegister
                                   }).ToList();

                return alunosTurma;
            }
        }

        public Aluno GetAlunoStatus2ComTurma(int anoMatricula, Produto.Produtos produto = Produto.Produtos.NAO_DEFINIDO, OrdemVenda.StatusOv status2 = OrdemVenda.StatusOv.Adimplente, int courseId = 0)
        {
            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             join pd in ctx.tblProducts on sod.intProductID equals pd.intProductID
                             join t in ctx.tblCourses on pd.intProductID equals t.intCourseID
                             where ((int)produto == (int)Produto.Produtos.NAO_DEFINIDO || pd.intProductGroup1 == (int)produto)
                              && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                              && so.intStatus2 == (int)status2
                              && (courseId == 0 || courseId == pd.intProductID)
                              && t.intYear == anoMatricula
                              && t.dteStartDateTime != null
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister,
                                 Turma = new Turma
                                 {
                                     ID = t.intCourseID,
                                     Inicio = t.dteStartDateTime ?? DateTime.Now,
                                     Fim = t.dteStartDateTime ?? DateTime.Now
                                 }
                             }).ToList().FirstOrDefault();

                return aluno;
            }
        }

        public int GetAlunoMedApenasAnoAtualAtivo()
        {
            var ano = Utilidades.GetYear();
            string anoAtual = DateTime.Now.ToString("yyyy");

            using (var ctx = new DesenvContext())
            {
                var alunosRemover = (from so in ctx.tblSellOrders
                                            join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                            join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                            join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                            join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                            where
                                                   so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                                    && ((c.intYear < ano && pr.intProductGroup1 == (int)Produto.Produtos.MED)
                                                        || pr.intProductGroup1 == (int)Produto.Produtos.CPMED)
                                            orderby so.dteDate descending
                                            select new Aluno
                                            {
                                                ID = so.intClientID,
                                                Register = p.txtRegister
                                            }).ToList();

                var alunosAnoAtual = (from so in ctx.tblSellOrders
                                      join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                      join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                      join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                      join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                      where

                                          pr.intProductGroup1 == (int)Produto.Produtos.MED
                                          && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                          && so.txtComment.Contains(anoAtual)


                                      orderby so.dteDate descending
                                      select new Aluno
                                      {
                                          ID = so.intClientID,
                                          Register = p.txtRegister
                                      }).ToList();


                foreach (Aluno a in alunosRemover)
                {
                    alunosAnoAtual.RemoveAll(x => x.ID == a.ID);
                }
                return alunosAnoAtual.FirstOrDefault().ID;
            }
        }

        public List<Aluno> GetAlunoAnoAtualStatus2SemSenha(Produto.Produtos produto, OrdemVenda.StatusOv status2)
        {
            using (var ctx = new DesenvContext())
            {
                var anoAtual = Utilidades.GetYear();

                var alunos = (from so in ctx.tblSellOrders
                              join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                              join p in ctx.tblPersons on so.intClientID equals p.intContactID
                              join pd in ctx.tblProducts on sod.intProductID equals pd.intProductID
                              join c in ctx.tblCourses on pd.intProductID equals c.intCourseID
                              join ps in ctx.tblPersons_Passwords on p.intContactID equals ps.intContactID into _ps
                              from jps in _ps.DefaultIfEmpty()
                              where jps == null && c.intYear == anoAtual && pd.intProductGroup1 == (int)produto
                                && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && so.intStatus2 == (int)status2
                              orderby so.dteDate descending
                              select new Aluno
                              {
                                  ID = so.intClientID,
                                  Register = p.txtRegister
                              }).ToList();

                return alunos;
            }
        }

        public int GetAlunoMedApenasAno2018()
        {
            var ano = 2018;

            using (var ctx = new DesenvContext())
            {
                var alunosComOutrosAnos = (from so in ctx.tblSellOrders
                                           join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                           join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                           join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                           join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                           where
                                           c.intYear != ano
                                           && pr.intProductGroup1 == (int)Produto.Produtos.MED
                                           && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                           orderby so.dteDate descending
                                           select p).Select(x => x.intContactID);

                var alunoAno2018 = (from so in ctx.tblSellOrders
                                    join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                    join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                    join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                    join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                    where
                                    !alunosComOutrosAnos.Contains(p.intContactID)
                                    && c.intYear == ano
                                    && pr.intProductGroup1 == (int)Produto.Produtos.MED
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                    orderby so.dteDate descending
                                    select p).Select(x => x.intContactID).ToList().FirstOrDefault();



                return alunoAno2018;
            }
        }

        public int GetAlunoMedApenasAno2019()
        {
            var ano = 2019;

            using (var ctx = new DesenvContext())
            {

                var alunosAno2019 = (from so in ctx.tblSellOrders
                                     join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                     join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                     join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                     join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                     where
                                     c.intYear == ano
                                     && pr.intProductGroup1 == (int)Produto.Produtos.MED
                                     && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                     orderby so.dteDate descending
                                     select p).Select(x=> x.intContactID).ToList();

                var alunosComOutrosAnos = (from so in ctx.tblSellOrders
                                           join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                           join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                           join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                           join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                           where
                                           c.intYear != ano
                                           && pr.intProductGroup1 == (int)Produto.Produtos.MED
                                           && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                           orderby so.dteDate descending
                                           select p).Select(x => x.intContactID).ToList();



                alunosAno2019.RemoveAll(x => alunosComOutrosAnos.Contains(x));

                return alunosAno2019.FirstOrDefault();
            }
        }


        public int GetAlunoMedAnoAtualComAnosAnteriores()
        {
            string anoAtual = DateTime.Now.ToString("yyyy");
            string anoLancamento = Utilidades.AnoLancamentoMedsoftPro.ToString();
            using (var ctx = new DesenvContext())
            {

                var alunosAcademicosSomente2019 = (from so in ctx.tblSellOrders
                                                   join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                                   join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                                   join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                                   where so.txtComment.Contains(anoAtual)
                                                   && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                                   && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                                   && pr.intProductGroup1 == (int)Produto.Produtos.MED
                                                   orderby so.dteDate descending
                                                   select new Aluno
                                                   {
                                                       ID = so.intClientID,
                                                   });

                var idsAlunos = alunosAcademicosSomente2019.Select(X => X.ID);

                var alunoAcademico = (from so in ctx.tblSellOrders
                                      join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                      join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                      join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                      where so.txtComment.Contains(anoLancamento)
                                      && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                      && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                      && idsAlunos.Contains(so.intClientID)
                                      && pr.intProductGroup1 == (int)Produto.Produtos.MED
                                      orderby so.dteDate descending
                                      select new Aluno
                                      {
                                          ID = so.intClientID

                                      }).FirstOrDefault();

                return alunoAcademico.ID;
            }

        }
        public int GetAlunoMedAnoAnterior()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {

                var alunosAnteriores = (from so in ctx.tblSellOrders
                                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                        join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                        join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                        where
                                        c.intYear == (ano - 1)
                                        && pr.intProductGroup1 == (int)Produto.Produtos.MED
                                        && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                        orderby so.dteDate descending
                                        select p).Select(x => x.intContactID).ToList();

                var alunosAnoAtual = (from so in ctx.tblSellOrders
                                      join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                      join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                      join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                      join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                      where
                                      c.intYear == ano
                                      && pr.intProductGroup1 == (int)Produto.Produtos.MED
                                      && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                      orderby so.dteDate descending
                                      select p).Select(x => x.intContactID).ToList();



                alunosAnteriores.RemoveAll(x => alunosAnoAtual.Contains(x));

                return alunosAnteriores.FirstOrDefault();
            }
        }
        public int GetAlunoAnoAtualComAnosAnteriores(Produto.Produtos produto)
        {
            int anoAtual = Utilidades.GetYear();
            int anoLancamento = Utilidades.AnoLancamentoMedsoftPro;

            using (var ctx = new DesenvContext())
            {
                var alunos2019 = (from so in ctx.tblSellOrders
                                  join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                  join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                  join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                  join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                  where c.intYear == anoAtual
                                  && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                  && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                  && pr.intProductGroup1 == (int)produto
                                  orderby so.dteDate descending
                                  select new Aluno
                                  {
                                      ID = so.intClientID,
                                  });

                var idsAlunos = alunos2019.Select(X => X.ID);

                var produtosGrupo = Utilidades.GrupoProduto((int)produto);

                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where c.intYear >= anoLancamento && c.intYear < anoAtual
                             && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                             && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                             && idsAlunos.Contains(so.intClientID)
                             && produtosGrupo.Contains(pr.intProductGroup1)
                             orderby so.dteDate descending
                             select new Aluno
                             {
                                 ID = so.intClientID

                             }).FirstOrDefault();

                return aluno.ID;
            }
        }

        public Aluno GetAlunoSomenteR1()
        {
            using (var ctx = new DesenvContext())
            {
                var alunosR3 = (from so in ctx.tblSellOrders
                                join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                where (so.txtComment.Contains(Constants.R3) || so.txtComment.Contains(Constants.R4) || so.txtComment.Contains(Constants.R4Tego))
                               && so.intStatus == 2
                               && so.intStatus2 == 6
                                orderby so.dteDate descending
                                select new Aluno
                                {
                                    ID = so.intClientID,
                                    Register = p.txtRegister
                                }).Select(x => x.ID);

                var alunosR1 = (from so in ctx.tblSellOrders
                                join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                where (!so.txtComment.Contains(Constants.R3) && !so.txtComment.Contains(Constants.R4) && !so.txtComment.Contains(Constants.R4Tego))
                                && so.intStatus == 2
                                && so.intStatus2 == 6
                                orderby so.dteDate descending
                                select new Aluno
                                {
                                    ID = so.intClientID,
                                    Register = p.txtRegister
                                });


                return alunosR1.Where(x => !alunosR3.Contains(x.ID)).FirstOrDefault();
            }
        }
        public Aluno GetAlunoR3Inadimplente()
        {
            var anoAtual = Utilidades.GetYear();
            using (var ctx = new DesenvContext())
            {
                var produtosR3 = Utilidades.ProdutosR3();
                var alunoR3 = (from so in ctx.tblSellOrders
                               join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                               join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                               join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                               join p in ctx.tblPersons on so.intClientID equals p.intContactID
                               where produtosR3.Contains(pr.intProductGroup2 ?? 0)
                               && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                               && so.intStatus2 == (int)Utilidades.ESellOrderStatus.Inadimplente
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

        public List<Aluno> GetAlunoExtensivoComInteresseR3()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from so in ctx.tblSellOrders
                                join sode in ctx.tblSellOrderDetails on so.intOrderID equals sode.intOrderID
                                join prod in ctx.tblProducts on sode.intProductID equals prod.intProductID
                                join cour in ctx.tblCourses on prod.intProductID equals cour.intCourseID
                                join pe in ctx.tblPersons on so.intClientID equals pe.intContactID
                                join c in ctx.tblClients on so.intClientID equals c.intClientID
                                where
                                (prod.intProductGroup1 == (int)Produto.Produtos.MED
                                || prod.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                && cour.intYear == ano
                                && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                && (c.txtArea == Constants.R3 || c.txtArea == Constants.R4 || c.txtArea == Constants.R4Tego)
                                orderby so.intOrderID descending
                                select new Aluno
                                {
                                    ID = so.intClientID,
                                    Register = pe.txtRegister
                                }).ToList();
                return lstaluno;
            }
        }

        public List<Aluno> GetAlunoExtensivoSemInteresseR3()
        {
            var produtosR3 = Utilidades.ProdutosR3();
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var lstAlunosInteresseR3 = GetAlunoExtensivoComInteresseR3().Select(x=> x.ID).ToList();

                var lstaluno = (from so in ctx.tblSellOrders
                                join sode in ctx.tblSellOrderDetails on so.intOrderID equals sode.intOrderID
                                join prod in ctx.tblProducts on sode.intProductID equals prod.intProductID
                                join cour in ctx.tblCourses on prod.intProductID equals cour.intCourseID
                                join pe in ctx.tblPersons on so.intClientID equals pe.intContactID
                                where
                                (prod.intProductGroup1 == (int)Produto.Produtos.MED
                                || prod.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                && cour.intYear == ano
                                && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                && !lstAlunosInteresseR3.Contains(so.intClientID)
                                orderby so.intOrderID descending
                                select new 
                                {
                                    ID = so.intClientID,
                                    Register = pe.txtRegister,
                                    GrupoProduto = prod.intProductGroup1
                                }).ToList();

                var alunosR3 = GetAlunosR3(ano).Select(a => a.ID);

                return lstaluno.Where(a => !alunosR3.Contains(a.ID))
                .Select(a => new Aluno
                {
                    ID = a.ID,
                    Register = a.Register
                }).ToList();
            }
        }

        public List<Aluno> GetAlunosR3(int ano = 0)
        {
            using (var ctx = new DesenvContext())
            {
                var alunosR3 = (from so in ctx.tblSellOrders
                                join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                where so.txtComment.Contains("R3")
                                && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                && (ano == 0 || c.intYear == ano)
                                orderby so.dteDate descending
                                select new Aluno
                                {
                                    ID = so.intClientID,
                                    Register = p.txtRegister
                                }).ToList();

                return alunosR3;
            }
        }


        public List<Aluno> GetAlunosR3InadimplenteSemTermoAceite()
        {
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            List<Aluno> result = new List<Aluno>();
            using (var ctx = new DesenvContext())
            {
                var produtosR3 = Utilidades.ProdutosR3();
                var alunosR3 = (from so in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                where produtosR3.Contains(pr.intProductGroup2 ?? 0)
                                && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                && (so.intStatus2 == (int)Utilidades.ESellOrderStatus.Inadimplente)// ||so.intStatus2 == (int)Utilidades.ESellOrderStatus.InadimplenteMesesAnteriores)
                                orderby so.dteDate descending
                                select new Aluno
                                {
                                    ID = so.intClientID,
                                    Register = p.txtRegister,
                                    Complemento = so.intOrderID.ToString().Trim()
                                }).Distinct().ToList();

                foreach (var aluno in alunosR3)
                {
                    var proc = new DBQuery().ExecuteStoredProcedure(
                   "msp_OperacoesControleFluxo_Carga_MensagemInadimplenciaAplicativo_desenvProducao",
                   new SqlParameter[]
                   {
                        new SqlParameter("@intClientID", aluno.ID),
                        new SqlParameter("@intAplicativoID", idAplicacao)
                   });

                    if (proc.Tables.Count > 0)
                    {
                        for (int i = 0; i < proc.Tables[0].Rows.Count; i++)
                        {
                            var IdOrdemDeVenda = proc.Tables[0].Rows[i]["IntOrderID"].ToString();
                            var intMensagemID = Convert.ToInt32(proc.Tables[0].Rows[i]["intMensagemID"]);

                            if(aluno.Complemento == IdOrdemDeVenda &&
                               intMensagemID == (int)Utilidades.TipoMensagemInadimplente.PermiteAcessoComTermoDeInadimplencia)
                            {
                                result.Add(aluno);
                            }

                        }
                    }
                }

                return result;
            }
        }


        public List<Aluno> GetAlunosR3InadimplenteBloqueado()
        {
            var idChamadoInternoAberto = 1;
            using (var ctx = new DesenvContext())
            {

                var produtosR3 = Utilidades.ProdutosR3();
                var dataAnterior = DateTime.Now.AddDays(-1);
                ; var alunosR3 = (from so in ctx.tblSellOrders
                                  join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                  join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                  join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                  join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                  join cc in ctx.tblCallCenterCalls on so.intClientID equals cc.intClientID
                                  where produtosR3.Contains(pr.intProductGroup2 ?? 0)
                                  && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                  && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente)// || so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                  && cc.dteDataPrevisao2 < dataAnterior
                                    && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                  && cc.txtSubject.Contains(so.intOrderID.ToString().Trim())
                                  && cc.intStatusID == idChamadoInternoAberto
                                  orderby so.dteDate descending
                                  select new Aluno
                                  {
                                      ID = so.intClientID,
                                      Register = p.txtRegister
                                  }).Distinct().ToList();

                var idsAlunos = alunosR3.Select(x => x.ID).ToList();

                var idsNaoBloqueados = (from so in ctx.tblSellOrders
                                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                        join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                        join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                        join cc in ctx.tblCallCenterCalls on so.intClientID equals cc.intClientID
                                        where produtosR3.Contains(pr.intProductGroup2 ?? 0)
                                        && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                        && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente)
                                        && idsAlunos.Contains(cc.intClientID)
                                        && cc.dteDataPrevisao2 >= dataAnterior
                                        && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                        && cc.intStatusID == idChamadoInternoAberto
                                        select cc.intClientID).ToList();


                alunosR3.RemoveAll(x => idsNaoBloqueados.Contains(x.ID));

                return alunosR3;
            }
        }

        public List<Aluno> GetAlunosMedMasterInadimplenteBloqueado()
        {
            var idChamadoInternoAberto = 1;
            using (var ctx = new DesenvContext())
            {

                var medmaster = (int)Produto.Produtos.MED_MASTER;
                var dataAnterior = DateTime.Now.AddDays(-1);
                var alunosMedMaster = (from so in ctx.tblSellOrders
                                  join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                  join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                  join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                  join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                  join cc in ctx.tblCallCenterCalls on so.intClientID equals cc.intClientID
                                  where pr.intProductGroup1 == medmaster
                                  && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                  && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                  && cc.dteDataPrevisao2 < dataAnterior
                                  && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                  && cc.txtSubject.Contains(so.intOrderID.ToString().Trim())
                                  && cc.intStatusID == idChamadoInternoAberto
                                  orderby so.dteDate descending
                                  select new Aluno
                                  {
                                      ID = so.intClientID,
                                      Register = p.txtRegister
                                  }).Distinct().ToList();

                var idsAlunos = alunosMedMaster.Select(x => x.ID).ToList();

                var idsNaoBloqueados = (from so in ctx.tblSellOrders
                                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                        join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                        join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                        join cc in ctx.tblCallCenterCalls on so.intClientID equals cc.intClientID
                                        where pr.intProductGroup1 == medmaster
                                        && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                        && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                        && idsAlunos.Contains(cc.intClientID)
                                        && cc.dteDataPrevisao2 >= dataAnterior
                                        && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                        && cc.intStatusID == idChamadoInternoAberto
                                        select cc.intClientID).ToList();


                alunosMedMaster.RemoveAll(x => idsNaoBloqueados.Contains(x.ID));

                return alunosMedMaster;
            }



        }

        public List<Aluno> GetAlunosInadimplenteBloqueado(int intProductGroup1)
        {
            var idChamadoInternoAberto = 1;
            using (var ctx = new DesenvContext())
            {

                var dataAnterior = DateTime.Now.AddDays(-1);
                var alunosMedMaster = (from so in ctx.tblSellOrders
                                       join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                       join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                       join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                       join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                       join cc in ctx.tblCallCenterCalls on so.intClientID equals cc.intClientID
                                       where pr.intProductGroup1 == intProductGroup1
                                       && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                       && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                       && cc.dteDataPrevisao2 < dataAnterior
                                       && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                       && cc.txtSubject.Contains(so.intOrderID.ToString().Trim())
                                       && cc.intStatusID == idChamadoInternoAberto
                                       orderby so.dteDate descending
                                       select new Aluno
                                       {
                                           ID = so.intClientID,
                                           Register = p.txtRegister
                                       }).Distinct().ToList();

                var idsAlunos = alunosMedMaster.Select(x => x.ID).ToList();

                var idsNaoBloqueados = (from so in ctx.tblSellOrders
                                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                        join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                        join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                        join cc in ctx.tblCallCenterCalls on so.intClientID equals cc.intClientID
                                        where pr.intProductGroup1 == intProductGroup1
                                        && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                        && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                        && idsAlunos.Contains(cc.intClientID)
                                        && cc.dteDataPrevisao2 >= dataAnterior
                                        && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                        && cc.intStatusID == idChamadoInternoAberto
                                        select cc.intClientID).ToList();


                alunosMedMaster.RemoveAll(x => idsNaoBloqueados.Contains(x.ID));

                return alunosMedMaster;
            }



        }



        public Aluno GetAlunoMedEletroIMedCancelado()
        {
            using (var ctx = new DesenvContext())
            {
                var produtoMedEletroIMed = Produto.Produtos.MEDELETRO_IMED.GetHashCode();
                var alunoMedEletroIMed = (from so in ctx.tblSellOrders
                                          join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                          join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                          join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                          join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                          where pr.intProductGroup1 == produtoMedEletroIMed
                                          && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                          orderby so.dteDate descending
                                          select new Aluno
                                          {
                                              ID = so.intClientID,
                                              Register = p.txtRegister
                                          }).FirstOrDefault();

                return alunoMedEletroIMed;
            }
        }

        public List<Aluno> GetAlunosR3InadimplenteCarencia()
        {
            var idChamadoInternoAberto = 1;
            using (var ctx = new DesenvContext())
            {

                var produtosR3 = Utilidades.ProdutosR3();
                var alunosR3 = (from so in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                join cc in ctx.tblCallCenterCalls on so.intClientID equals cc.intClientID
                                where produtosR3.Contains(pr.intProductGroup2 ?? 0)
                                && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente ||
                                     so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                && cc.dteDataPrevisao2 > DateTime.Now
                                  && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                && cc.txtSubject.Contains(so.intOrderID.ToString().Trim())
                                && cc.intStatusID == idChamadoInternoAberto
                                orderby so.dteDate descending
                                select new Aluno
                                {
                                    ID = so.intClientID,
                                    Register = p.txtRegister
                                }).Distinct().ToList();

                return alunosR3;
            }
        }

        public Aluno GetAlunoMedEletroIMed()
        {
            using (var ctx = new DesenvContext())
            {
                var produtoMedEletroIMed = Produto.Produtos.MEDELETRO_IMED.GetHashCode();
                var alunoMedEletroIMed = (from so in ctx.tblSellOrders
                                          join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                          join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                          join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                          join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                          where pr.intProductGroup1 == produtoMedEletroIMed
                                          && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                          && so.intStatus2 == (int)Utilidades.ESellOrderStatus.Adimplente
                                          orderby so.dteDate descending
                                          select new Aluno
                                          {
                                              ID = so.intClientID,
                                              Register = p.txtRegister
                                          }).FirstOrDefault();

                return alunoMedEletroIMed;
            }
        }

        public List<Aluno> GetAlunosMedEletroIMedAnoAtualAtivo()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from sel in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on sel.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                join pe in ctx.tblPersons on sel.intClientID equals pe.intContactID
                                where pr.intProductGroup1 == (int)Produto.Produtos.MEDELETRO_IMED
                                && c.intYear == ano
                                && sel.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && sel.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                select new Aluno
                                {
                                    ID = sel.intClientID,
                                    Register = pe.txtRegister
                                }).ToList();

                return lstaluno;
            }
        }

        public List<Aluno> GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueado()
        {
            var ano = Utilidades.GetYear();
            var idChamadoInternoAberto = 1;

            var dataLimitePrevisao2 = DateTime.Now.AddDays(-1);

            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from sel in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on sel.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                join pe in ctx.tblPersons on sel.intClientID equals pe.intContactID
                                join cc in ctx.tblCallCenterCalls on sel.intClientID equals cc.intClientID
                                where pr.intProductGroup1 == (int)Produto.Produtos.MEDELETRO_IMED
                                && c.intYear == ano
                                && sel.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && sel.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente
                                && cc.dteDataPrevisao2 < dataLimitePrevisao2
                                && cc.txtSubject.Contains(sel.intOrderID.ToString().Trim())
                                && cc.intStatusID == idChamadoInternoAberto
                                select new Aluno
                                {
                                    ID = sel.intClientID,
                                    Register = pe.txtRegister
                                }).Distinct().ToList();

                return lstaluno;
            }
        }

        public List<Aluno> GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueadoMesesAnteriores()
        {
            var ano = Utilidades.GetYear();
            var idChamadoInternoAberto = 1;

            var dataLimitePrevisao2 = DateTime.Now.AddDays(-1);

            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from sel in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on sel.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                join pe in ctx.tblPersons on sel.intClientID equals pe.intContactID
                                join cc in ctx.tblCallCenterCalls on sel.intClientID equals cc.intClientID
                                where pr.intProductGroup1 == (int)Produto.Produtos.MEDELETRO_IMED
                                && c.intYear == ano
                                && sel.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && sel.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES
                                && cc.dteDataPrevisao2 < dataLimitePrevisao2
                                && cc.txtSubject.Contains(sel.intOrderID.ToString().Trim())
                                && cc.intStatusID == idChamadoInternoAberto
                                select new Aluno
                                {
                                    ID = sel.intClientID,
                                    Register = pe.txtRegister
                                }).Distinct().ToList();

                return lstaluno;
            }
        }



        public List<Aluno> GetAlunosMedEletroIMedAnoAtualInadimplenteSemTermoBloqueado()
        {
            var ano = Utilidades.GetYear();
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            List<Aluno> result = new List<Aluno>();

            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from sel in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on sel.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                join pe in ctx.tblPersons on sel.intClientID equals pe.intContactID

                                where pr.intProductGroup1 == (int)Produto.Produtos.MEDELETRO_IMED
                                && c.intYear == ano
                                && sel.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && (sel.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente ||
                                     sel.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)

                                select new Aluno
                                {
                                    ID = sel.intClientID,
                                    Register = pe.txtRegister,
                                    Complemento = sel.intOrderID.ToString().Trim()
                                }).Distinct().ToList();

                foreach (var aluno in lstaluno)
                {
                    var proc = new DBQuery().ExecuteStoredProcedure(
                   "msp_OperacoesControleFluxo_Carga_MensagemInadimplenciaAplicativo_desenvProducao",
                   new SqlParameter[]
                   {
                        new SqlParameter("@intClientID", aluno.ID),
                        new SqlParameter("@intAplicativoID", idAplicacao)
                   });

                    if (proc.Tables.Count > 0)
                    {
                        for (int i = 0; i < proc.Tables[0].Rows.Count; i++)
                        {
                            var IdOrdemDeVenda = proc.Tables[0].Rows[i]["IntOrderID"].ToString();
                            var intMensagemID = Convert.ToInt32(proc.Tables[0].Rows[i]["intMensagemID"]);

                            if (aluno.Complemento == IdOrdemDeVenda &&
                               intMensagemID == (int)Utilidades.TipoMensagemInadimplente.PermiteAcessoComTermoDeInadimplencia)
                            {
                                result.Add(aluno);
                            }

                        }
                    }
                }
                return result;
            }
        }

        public List<Aluno> GetAlunosExtensivoAnoAtualAtivo()
        {
            var ano = Utilidades.GetYear();

            using (var ctx = new DesenvContext())
            {
                var alunos = (from so in ctx.tblSellOrders
                              join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                              join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                              join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                              join p in ctx.tblPersons on so.intClientID equals p.intContactID
                              join senha in ctx.tblPersons_Passwords on p.intContactID equals senha.intContactID
                              where (pr.intProductGroup1 == (int)Produto.Produtos.MED || pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                     && so.intStatus == (int)OrdemVenda.StatusOv.Ativa && so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente
                                     && c.intYear == ano
                              orderby so.dteDate descending
                              select new Aluno
                              {
                                  ID = so.intClientID,
                                  Register = p.txtRegister
                              }).ToList();
                return alunos;
            }

        }

        public List<Aluno> GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresSemTermosBloqueado()
        {
            var ano = Utilidades.GetYear();
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            List<Aluno> result = new List<Aluno>();

            using (var ctx = new DesenvContext())
            {
                var lstaluno = (from sel in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on sel.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                join pe in ctx.tblPersons on sel.intClientID equals pe.intContactID

                                where (pr.intProductGroup1 == (int)Produto.Produtos.MED || pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                && c.intYear == ano
                                && sel.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && (sel.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)

                                select new Aluno
                                {
                                    ID = sel.intClientID,
                                    Register = pe.txtRegister,
                                    Complemento = sel.intOrderID.ToString().Trim()
                                }).Distinct().ToList();

                foreach (var aluno in lstaluno)
                {
                    var proc = new DBQuery().ExecuteStoredProcedure(
                   "msp_OperacoesControleFluxo_Carga_MensagemInadimplenciaAplicativo_desenvProducao",
                   new SqlParameter[]
                   {
                        new SqlParameter("@intClientID", aluno.ID),
                        new SqlParameter("@intAplicativoID", idAplicacao)
                   });

                    if (proc.Tables.Count > 0)
                    {
                        for (int i = 0; i < proc.Tables[0].Rows.Count; i++)
                        {
                            var IdOrdemDeVenda = proc.Tables[0].Rows[i]["IntOrderID"].ToString();
                            var intMensagemID = Convert.ToInt32(proc.Tables[0].Rows[i]["intMensagemID"]);

                            if (aluno.Complemento == IdOrdemDeVenda &&
                               intMensagemID == (int)Utilidades.TipoMensagemInadimplente.PermiteAcessoComTermoDeInadimplencia)
                            {
                                result.Add(aluno);
                            }

                        }
                    }
                }
                return result;
            }
        }


        public List<Aluno> GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresBloqueado()
        {
            var ano = Utilidades.GetYear();
            var statusChamadoAberto = 1;

            var dataLimitePrevisao2 = DateTime.Now.AddDays(-1);

            using (var ctx = new DesenvContext())
            {

                var lstaluno = (from sel in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on sel.intOrderID equals sod.intOrderID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                join c in ctx.tblCourses on pr.intProductID equals c.intCourseID
                                join pe in ctx.tblPersons on sel.intClientID equals pe.intContactID
                                join cc in ctx.tblCallCenterCalls on sel.intClientID equals cc.intClientID
                                where (pr.intProductGroup1 == (int)Produto.Produtos.MED || pr.intProductGroup1 == (int)Produto.Produtos.MEDCURSO)
                                && c.intYear == ano
                                && sel.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                && (sel.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                && cc.dteDataPrevisao2 < dataLimitePrevisao2
                                && cc.txtSubject.Contains(sel.intOrderID.ToString().Trim())
                                && cc.intStatusID == statusChamadoAberto
                                && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                select new Aluno
                                {
                                    ID = sel.intClientID,
                                    Register = pe.txtRegister
                                }).Distinct().ToList();

                return lstaluno;
            }
        }

        public List<Aluno> GetAlunosMedMasterInadimplente()
        {
            var idChamadoInternoAberto = 1;
            using (var ctx = new DesenvContext())
            {
                var medmaster = (int)Produto.Produtos.MED_MASTER;
                var dataAnterior = DateTime.Now.AddDays(-1);
                var alunosInadimplentes = (from so in ctx.tblSellOrders
                                  join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                  join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                  join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                  join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                  where pr.intProductGroup1 == medmaster
                                  && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                  && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                  select new Aluno
                                  {
                                      ID = so.intClientID,
                                      Register = p.txtRegister
                                  }).Distinct().ToList();

                var idsAlunos = alunosInadimplentes.Select(x => x.ID).ToList();

                var idsChamadoAberto = (from so in ctx.tblSellOrders
                                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                        join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                        join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                        join cc in ctx.tblCallCenterCalls on so.intClientID equals cc.intClientID
                                        where pr.intProductGroup1 == medmaster
                                        && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                        && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                        && idsAlunos.Contains(cc.intClientID)
                                        && cc.dteDataPrevisao2 >= dataAnterior
                                        && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                        && cc.intStatusID == idChamadoInternoAberto
                                        select cc.intClientID).ToList();


                alunosInadimplentes.RemoveAll(x => idsChamadoAberto.Contains(x.ID));

                return alunosInadimplentes;
            }
        }

        public List<Aluno> GetAlunosMedMasterInadimplenteVisualizouTermo()
        {
            var idChamadoInternoAberto = 1;
            using (var ctx = new DesenvContext())
            {
                var medmaster = (int)Produto.Produtos.MED_MASTER;
                var dataAnterior = DateTime.Now.AddDays(-1);
                var alunosInadimplentes = (from so in ctx.tblSellOrders
                                           join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                           join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                           join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                           join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                           where pr.intProductGroup1 == medmaster
                                           && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                           && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                           select new Aluno
                                           {
                                               ID = so.intClientID,
                                               Register = p.txtRegister
                                           }).Distinct().ToList();

                var idsAlunos = alunosInadimplentes.Select(x => x.ID).ToList();

                var idsChamadoAberto = (from so in ctx.tblSellOrders
                                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                        join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                        join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                        join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                        join cc in ctx.tblCallCenterCalls on so.intClientID equals cc.intClientID
                                        where pr.intProductGroup1 == medmaster
                                        && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                        && (so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                                        && idsAlunos.Contains(cc.intClientID)
                                        && cc.dteDataPrevisao2 >= dataAnterior
                                        && cc.intStatusInternoID == Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA
                                        && cc.intStatusID == idChamadoInternoAberto
                                        select cc.intClientID).ToList();


                alunosInadimplentes.RemoveAll(x => !idsChamadoAberto.Contains(x.ID));

                return alunosInadimplentes;
            }
        }


        public List<Aluno> GetAlunosMedEletroIMedAnoAtualExtensivoAnoAtual()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualAtivo();
            var alunosExtensivo = GetAlunosExtensivoAnoAtualAtivo();
            var lstAlunos = alunosExtensivo.Select(x => x.ID);

            var result = alunosExtensivo.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }


        public List<Aluno> GetAlunosMedEletroIMedAnoAtualExtensivoAnoAtualInadimplente()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualAtivo();
            var alunosExtensivo = GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresSemTermosBloqueado();
            var lstAlunos = alunosExtensivo.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosMedEletroIMedAnoAtualExtensivoAnoAtualInadimplenteBloqueado()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualAtivo();
            var alunosExtensivo = GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresBloqueado();
            var lstAlunos = alunosExtensivo.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }


        public List<Aluno> GetAlunosMedEletroIMedAnoAtualInadimplenteExtensivoAnoAtualAtivo()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteSemTermoBloqueado();
            var alunosExtensivo = GetAlunosExtensivoAnoAtualAtivo();
            var lstAlunos = alunosExtensivo.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }


        public List<Aluno> GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueadoExtensivoAnoAtualAtivo()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueado();
            var alunosExtensivo = GetAlunosExtensivoAnoAtualAtivo();
            var lstAlunos = alunosExtensivo.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosMedEletroIMedAnoAtualInadimplenteExtensivoAnoAtualInadimplente()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteSemTermoBloqueado();
            var alunosExtensivo = GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresSemTermosBloqueado();
            var lstAlunos = alunosExtensivo.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

		public List<Aluno> GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueadoExtensivoAnoAtualInadimplente()
		{
			var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueado();
			var alunosExtensivo = GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresSemTermosBloqueado();
			var lstAlunos = alunosExtensivo.Select(x => x.ID);

			var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
			return result;
		}

		public List<Aluno> GetAlunosMedEletroIMedAnoAtualInadimplenteExtensivoAnoAtualInadimplenteBloqueado()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteSemTermoBloqueado();
            var alunosExtensivo = GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresBloqueado();
            var lstAlunos = alunosExtensivo.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }


        public List<Aluno> GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueadoExtensivoAnoAtualInadimplenteBloqueado()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueado();
            var alunosExtensivo = GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresBloqueado();
            var lstAlunos = alunosExtensivo.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisAdimplenteIMedAdimplente()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualAtivo();
            var alunosRMais = GetAlunosR3();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisAdimplenteIMedInadimplente()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteSemTermoBloqueado();
            var alunosRMais = GetAlunosR3();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisAdimplenteIMedInadimplenteBloqueado()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueado();
            var alunosRMais = GetAlunosR3();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisInadimplenteIMedAdimplente()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualAtivo();
            var alunosRMais = GetAlunosR3InadimplenteSemTermoAceite();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisInadimplenteBloqueadoIMedAdimplente()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualAtivo();
            var alunosRMais = GetAlunosR3InadimplenteBloqueado();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisInadimplenteIMedInadimplente()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteSemTermoBloqueado();
            var alunosRMais = GetAlunosR3InadimplenteSemTermoAceite();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisInadimplenteBloqueadoIMedInadimplenteBloqueado()
        {
            var alunosImed = GetAlunosMedEletroIMedAnoAtualInadimplenteBloqueado();
            var alunosRMais = GetAlunosR3InadimplenteBloqueado();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosImed.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisAdimplenteExtensivoAnoAtualAdimplente()
        {
            var alunosExtensivo = GetAlunosStatus2(Produto.Produtos.MED, OrdemVenda.StatusOv.Adimplente);
            var alunosRMais = GetAlunosStatus2(Produto.Produtos.R3CIRURGIA, OrdemVenda.StatusOv.Adimplente);
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosExtensivo.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisAdimplenteExtensivoAnoAtualInadimplente()
        {
            var alunosExtensivo = GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresSemTermosBloqueado();
            var alunosRMais = GetAlunosR3();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosExtensivo.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisAdimplenteExtensivoAnoAtualInadimplenteBloqueado()
        {
            var alunosExtensivo = GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresBloqueado();
            var alunosRMais = GetAlunosR3();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosExtensivo.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisInadimplenteExtensivoAnoAtualAdimplente()
        {
            var alunosExtensivo = GetAlunosExtensivoAnoAtualAtivo();
            var alunosRMais = GetAlunosR3InadimplenteSemTermoAceite();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosExtensivo.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public List<Aluno> GetAlunosRMaisInadimplenteBloqueadoExtensivoAnoAtualAdimplente()
        {
            var alunosExtensivo = GetAlunosExtensivoAnoAtualAtivo();
            var alunosRMais = GetAlunosR3InadimplenteBloqueado();
            var alunosRMaisCarencia = GetAlunosR3InadimplenteCarencia();

            var lstAlunosRMaisCarencia = alunosRMaisCarencia.Select(x => x.ID);

            var lstAlunosRMais = alunosRMais.Where(x => !lstAlunosRMaisCarencia.Contains(x.ID)).Select(y => y.ID);

            var result = alunosExtensivo.Where(x => lstAlunosRMais.Contains(x.ID)).ToList();
            return result;
        }


        public List<Aluno> GetAlunosRMaisInadimplenteBloqueadoExtensivoAnoAtualInadimplenteMesesAnterioresBloqueado()
        {
            var alunosExtensivo = GetAlunosExtensivoAnoAtualInadimplenteMesesAnterioresBloqueado();
            var alunosRMais = GetAlunosR3InadimplenteBloqueado();
            var lstAlunos = alunosRMais.Select(x => x.ID);

            var result = alunosExtensivo.Where(x => lstAlunos.Contains(x.ID)).ToList();
            return result;
        }

        public Aluno GetAlunoBlacklist()
        {
            using (var ctx = new DesenvContext())
            {
                var aluno = (from cbl in ctx.tblClients_BlackList
                             join p in ctx.tblPersons on cbl.intClientID equals p.intContactID
                             join so in ctx.tblSellOrders on p.intContactID equals so.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join ad in ctx.tblEmed_AccessDenied on p.intContactID equals ad.intClientID
                             join cb in ctx.tblClients_BlackList on p.txtRegister equals cb.txtRegister
                             where c.intYear > Utilidades.AnoLancamentoMedsoftPro
                             select new Aluno
                             {
                                 ID = so.intClientID,
                                 Register = p.txtRegister
                             }).FirstOrDefault();

                return aluno;
            }
        }

       
        public Aluno GetAlunoExtensivoAnoSeguinte()
        {

            int ano = DateTime.Now.Year + 1;
            var produtosExtensivo = Utilidades.ProdutosExtensivo();
            using (var ctx = new DesenvContext())
            {
                var anosanteriores = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             where c.intYear < ano
                             select so.intClientID);

                var aluno = (from p in ctx.tblPersons
                             join so in ctx.tblSellOrders on p.intContactID equals so.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join pp in ctx.tblPersons_Passwords on p.intContactID equals pp.intContactID
                             where c.intYear == ano
                             && produtosExtensivo.Contains(pr.intProductGroup1 ?? 0)
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                             && so.intStatus2 == (int)Utilidades.ESellOrderStatus.Adimplente
                             && !anosanteriores.Contains(p.intContactID)
                             select new Aluno
                             {
                                 ID = p.intContactID,
                                 Senha = pp.txtPassword,
                                 Register = p.txtRegister.Trim()
                             }).FirstOrDefault();

                return aluno;

            }
        }

        public Aluno GetAlunoExtensivoAnoAnterior()
        {

            int ano = DateTime.Now.Year - 1;
            var produtosExtensivo = Utilidades.ProdutosExtensivo();
            using (var ctx = new DesenvContext())
            {

                var aluno = (from p in ctx.tblPersons
                             join so in ctx.tblSellOrders on p.intContactID equals so.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join pp in ctx.tblPersons_Passwords on p.intContactID equals pp.intContactID
                             where c.intYear == ano
                             && produtosExtensivo.Contains(pr.intProductGroup1 ?? 0)
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                             && so.intStatus2 == (int)Utilidades.ESellOrderStatus.Adimplente
                             select new Aluno
                             {
                                 ID = p.intContactID,
                                 Senha = pp.txtPassword,
                                 Register = p.txtRegister.Trim()
                             }).FirstOrDefault();

                return aluno;

            }
        }


        public Aluno GetAlunoExtensivoAnoAtualBloqueado_AnoSeguinte()
        {

            int anoSeguinte = DateTime.Now.Year + 1;
            var produtosExtensivo = Utilidades.ProdutosExtensivo();
            using (var ctx = new DesenvContext())
            {
                var anoatual = (from so in ctx.tblSellOrders
                                join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                where c.intYear == DateTime.Now.Year
                                && produtosExtensivo.Contains(pr.intProductGroup1 ?? 0)
                                && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                && so.intStatus2 == (int)Utilidades.ESellOrderStatus.InadimplenteMesesAnteriores
                                select so.intClientID);

                var aluno = (from p in ctx.tblPersons
                             join so in ctx.tblSellOrders on p.intContactID equals so.intClientID
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join pp in ctx.tblPersons_Passwords on p.intContactID equals pp.intContactID
                             where c.intYear == anoSeguinte
                             && produtosExtensivo.Contains(pr.intProductGroup1 ?? 0)
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                             && so.intStatus2 == (int)Utilidades.ESellOrderStatus.Adimplente
                             && anoatual.Contains(p.intContactID)
                             select new Aluno
                             {
                                 ID = p.intContactID,
                                 Senha = pp.txtPassword,
                                 Register = p.txtRegister.Trim()
                             }).FirstOrDefault();

                return aluno;

            }

        }

        public Aluno GetAlunoMedMasterAnoAtualAtivo()
        {
            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where c.intYear == DateTime.Now.Year
                             && pr.intProductGroup1 == (int)Produto.Produtos.MED_MASTER
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                             && so.intStatus2 == (int)Utilidades.ESellOrderStatus.Adimplente
                             select new Aluno { ID = so.intClientID, Register = p.txtRegister }).FirstOrDefault();

                return aluno;
            }
        }

        public Aluno GetAlunoMedMasterAnoAtualPendente()
        {
            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where c.intYear == DateTime.Now.Year
                             && pr.intProductGroup1 == (int)Produto.Produtos.MED_MASTER
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Pendente
                             select new Aluno { ID = so.intClientID, Register = p.txtRegister }).FirstOrDefault();

                return aluno;
            }
        }

        public Aluno GetAlunoNaoMedMasterCPMEDExtensivoPendente()
        {
            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where c.intYear != DateTime.Now.Year
                             && pr.intProductGroup1 != (int)Produto.Produtos.MED_MASTER
                             && pr.intProductGroup1 != (int)Produto.Produtos.CPMED_EXTENSIVO
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Pendente
                             select new Aluno { ID = so.intClientID, Register = p.txtRegister }).FirstOrDefault();

                return aluno;
            }
        }

        public Aluno GetAlunoCPMEDExtensivoPendente()
        {
            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where pr.intProductGroup1 == (int)Produto.Produtos.CPMED_EXTENSIVO
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Pendente
                             select new Aluno { ID = so.intClientID, Register = p.txtRegister }).FirstOrDefault();

                return aluno;
            }
        }

        public Aluno GetAlunoMedMasterAnoAtualCancelado()
        {
            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where c.intYear == DateTime.Now.Year
                             && pr.intProductGroup1 == (int)Produto.Produtos.MED_MASTER
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                             select new Aluno { ID = so.intClientID, Register = p.txtRegister }).FirstOrDefault();

                return aluno;
            }
        }

        public int GetAlunoMedMasterAnoAtualCanceladoSemAnoAnterior()
        {

            using (var ctx = new DesenvContext())
            {
                var alunos = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             join p in ctx.tblPersons on so.intClientID equals p.intContactID
                             where c.intYear == DateTime.Now.Year
                             && pr.intProductGroup1 == (int)Produto.Produtos.MED_MASTER
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                             select so.intClientID).ToList();

                var idsExtensivos = new List<int> { (int)Utilidades.ProductGroups.MED, (int)Utilidades.ProductGroups.MEDEAD, (int)Utilidades.ProductGroups.MedMaster, (int)Utilidades.ProductGroups.MEDCURSO, (int)Utilidades.ProductGroups.MEDCURSOEAD };

                var alunosExtensivo2019 = (from so in ctx.tblSellOrders
                                          join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                          join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                          join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                          join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                          where c.intYear == DateTime.Now.Year
                                          && idsExtensivos.Contains((int)pr.intProductGroup1)
                                          && alunos.Contains(so.intClientID)
                                          select new Aluno { ID = so.intClientID, Register = p.txtRegister }).ToList();

                return alunos.Where(x => !alunosExtensivo2019.Any(y => x == y.ID)).FirstOrDefault();
            }

        }

        public int GetAlunoMedMasterAnoAtualInadimplente()
        {
            using (var ctx = new DesenvContext())
            {
                var aluno = (from so in ctx.tblSellOrders
                             join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                             join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                             join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                             where c.intYear == DateTime.Now.Year
                             && pr.intProductGroup1 == (int)Produto.Produtos.MED_MASTER
                             && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                             && so.intStatus2 == (int)Utilidades.ESellOrderStatus.InadimplenteMesesAnteriores
                             select so.intClientID).FirstOrDefault();

                return aluno;
            }
        }

        public Aluno GetAlunoMedMasterAnoAtualAtivoComAnoAtualCancelado(int produtoId)
        {
            using (var ctx = new DesenvContext())
            {
                var medmasters = (from so in ctx.tblSellOrders
                                  join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                  join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                  join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                  join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                  where c.intYear == DateTime.Now.Year
                                  && pr.intProductGroup1 == (int)Produto.Produtos.MED_MASTER
                                  && so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                  && so.intStatus2 == (int)Utilidades.ESellOrderStatus.Adimplente
                                  select new Aluno { ID = so.intClientID, Register = p.txtRegister }).ToList();

                var extensivoCancelado = (from so in ctx.tblSellOrders
                                          join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                          join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                          join pr in ctx.tblProducts on sod.intProductID equals pr.intProductID
                                          join p in ctx.tblPersons on so.intClientID equals p.intContactID
                                          where c.intYear == DateTime.Now.Year
                                          && pr.intProductGroup1 == produtoId
                                          && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                          select new Aluno { ID = so.intClientID, Register = p.txtRegister }).ToList();

                var migrados = (from m in medmasters
                                join e in extensivoCancelado
                                on m.ID equals e.ID
                                select m
                                ).FirstOrDefault();

                return migrados;
            }
        }

        public List<int> GetVideosProvaComAnexo(int temaId)
        {
            using (var ctx = new DesenvContext())
            {
                var ids = (from pvi in ctx.tblProvaVideoIndice
                              join pv in ctx.tblProvaVideo on pvi.intProvaVideoIndiceId equals pv.intProvaVideoIndiceId
                              where pvi.intLessonTitleId == temaId
                              && pv.bitPossuiAnexo
                              && pv.bitAtivo
                              select pv.intVideoId).ToList();
                return ids;
            }
        }


        public Cliente GetAlunoAcademicoMock()
        {
            Cliente clienteMock = new Cliente
            {
                AcessoGolden = false,
                Avatar = "http://static.medgrupo.com.br/static/Imagens/Recursos/ClientPictures/bWGjFmjrwUiMJ9vplOgI_.jpg",
                Filial = "São Paulo (SP)",
                Foto = "http://static.medgrupo.com.br/static/Imagens/Recursos/ClientPictures/bWGjFmjrwUiMJ9vplOgI_.jpg",
                FotoPerfil = "http://static.medgrupo.com.br/static/Imagens/Recursos/ClientPictures/bWGjFmjrwUiMJ9vplOgI_.jpg",
                ID = 173010,
                IdFilial = 31,
                Nome = "ACADEMICO 2",
                NickName = "Acad1234",
                Register = "21724944967",
                RetornoStatus = Cliente.StatusRetorno.Sucesso,
                Senha = "1Vqs05YnT4I+uKY5miR13M2HEII=",
                Sexo = 0,
                SituacaoAluno = -1,
                TipoPessoa = Pessoa.EnumTipoPessoa.Funcionario,
                TipoPessoaDescricao = "Funcionario",
                AnosPermitidos = new List<int> { 2019, 2018, 2017, 2016 }

            };

            return clienteMock;
        }
    }
}