using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using System.Linq;
using MedCore_API.Academico;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Repository
{
    public class EspecialidadeEntity : IEspecialidadeData
    {
        public List<Especialidade> GetByFilters(int QuestaoID)
        {
            using (var ctx = new DesenvContext())
            {
                List<Especialidade> ListaEspecialidadeClassificacao = (from d in ctx.tblMedsoft_Especialidade_Classificacao
                                                                       join cq in ctx.tblConcursoQuestaoCatologoDeClassificacoes on d.intClassificacaoID equals cq.intClassificacaoID
                                                                       join cqc in ctx.tblConcursoQuestao_Classificacao on cq.intClassificacaoID equals cqc.intClassificacaoID
                                                                       where cqc.intQuestaoID == QuestaoID
                                                                       select new Especialidade { Id = d.intEspecialidadeID, Descricao = cq.txtSubTipoDeClassificacao }).ToList();

                using (var ctxAcad = new AcademicoContext())
                {
                    var ListaQuestaoEspecialidade = (from q in ctxAcad.tblQuestoes
                                                     join es in ctxAcad.tblEspecialidades on q.intEspecialidadeID equals es.intEspecialidadeID
                                                     where q.intQuestaoID == QuestaoID
                                                     select new Especialidade { Id = es.intEspecialidadeID, Descricao = es.DE_ESPECIALIDADE }).ToList();

                    var subEspecialidades = (from es in ctxAcad.tblEspecialidades.AsEnumerable()
                                             join lista in ListaEspecialidadeClassificacao on es.intEspecialidadeID equals lista.Id
                                             select new Especialidade { Id = lista.Id, Descricao = lista.Descricao }
                                            ).Union(
                                                from lista in ListaQuestaoEspecialidade
                                                join d in ctx.tblMedsoft_Especialidade_Classificacao.AsEnumerable() on lista.Id equals d.intEspecialidadeID
                                                select new Especialidade { Id = lista.Id, Descricao = lista.Descricao }
                                            , new ComparerCaseInsensitive<Especialidade>()).ToList();

                    return subEspecialidades;
                }
            }
        }

        public List<Especialidade> GetByQuestaoSimulado(int questaoID, int simuladoID)
        {
            using (var ctx = new AcademicoContext())
            {
                List<Especialidade> subEspecialidades =
                    (from qs in ctx.tblQuestao_Simulado
                     join q in ctx.tblQuestoes on qs.intQuestaoID equals q.intQuestaoID
                     join e in ctx.tblEspecialidades on q.intEspecialidadeID equals e.intEspecialidadeID
                     where qs.intQuestaoID == questaoID && qs.intSimuladoID == simuladoID
                     select new Especialidade
                     {
                         Id = e.intEspecialidadeID,
                         Descricao = e.DE_ESPECIALIDADE
                     }).ToList();

                return subEspecialidades;
            }
        }

        public List<Especialidade> GetAll()
        {
            List<Especialidade> registros = new List<Especialidade>();

            using (var ctx = new AcademicoContext())
            {
                registros =
                (
                    from a in ctx.tblEspecialidades
                    select new Especialidade()
                    {
                        Id = a.intEspecialidadeID,
                        Descricao = a.DE_ESPECIALIDADE,
                        CodigoArea = a.CD_AREA,
                        CodigoEspecialidade = a.CD_ESPECIALIDADE
                    }
                )
                .ToList();
            }

            return registros;
        }
    }
}