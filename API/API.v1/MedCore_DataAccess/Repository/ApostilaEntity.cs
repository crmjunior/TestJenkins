using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedCore_DataAccess.Repository
{
    public class ApostilaEntity
    {
        public bool IsQuestaoInApostila(int idApostila, int idQuestao)
        {
            var ctx = new DesenvContext();
            var consulta = (from cq in ctx.tblConcursoQuestao_Classificacao
                            where cq.intClassificacaoID == idApostila
                            && cq.intQuestaoID == idQuestao
                            select cq.intID).Any();
            var consulta2 = (
                from cq in ctx.tblConcursoQuestao_Classificacao_Autorizacao
                where cq.intMaterialID == idApostila
                && cq.intQuestaoID == idQuestao && cq.bitAutorizacao == true
                select cq.intQuestaoID).Any();


            return (consulta || consulta2);

        }

        public List<Apostila> GetApostila(int idApostila)
        {
            using (var ctx = new DesenvContext())
            {
                var liberadasRevisao = ctx.tblConcursoQuestao_Classificacao_Autorizacao_ApostilaLiberada.ToList();
                var consulta = from apostila in ctx.Set<msp_API_ListaApostilas_Result>().FromSqlRaw("msp_API_ListaApostilas @intYear = {0}", 0).ToList()
                               where
                                (idApostila == apostila.intBookID)
                               select new Apostila()
                               {
                                   ID = apostila.intBookID,
                                   Capa = !String.IsNullOrEmpty(apostila.Capa) ? Convert.ToString(apostila.Capa).Trim() : "",
                                   Codigo = !String.IsNullOrEmpty(apostila.txtCode) ? Convert.ToString(apostila.txtCode).Trim() : "",
                                   Titulo = !String.IsNullOrEmpty(apostila.txtTitle) ? Convert.ToString(apostila.txtTitle).Trim() : "",
                                   Ano = Convert.ToInt32(apostila.intYear),
                                   IdProduto = (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDMEDCURSO)) ?
                                    (int)Produto.Cursos.MEDCURSO : (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDCPMED)) ?
                                    (int)Produto.Cursos.MED : Convert.ToInt32(apostila.intProductGroup2),
                                   IdProdutoGrupo = apostila.intProductGroup2.Value,
                                   IdGrandeArea = Convert.ToInt32(apostila.intClassificacaoID),
                                   IdSubEspecialidade = Convert.ToInt32(apostila.intProductGroup3),
                                   IdEntidade = Convert.ToInt32(apostila.intBookEntityID),
                                   ProdutosAdicionais = (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDMEDCURSO)) ?
                                    Convert.ToInt32(Produto.Cursos.MED).ToString() : (apostila.intProductGroup2.Equals((int)Produto.Cursos.MEDCPMED)) ?
                                    Convert.ToInt32(Produto.Cursos.CPMED).ToString() : (apostila.intProductGroup2.Equals((int)Produto.Cursos.RAC) || apostila.intProductGroup2.Equals((int)Produto.Cursos.RACIPE)) ? Convert.ToInt32((int)Produto.Cursos.RA).ToString() : "",
                                   NomeCompleto = string.Format("{0} -{1}", apostila.txtCode, (apostila.txtName ?? "").Trim().Replace(apostila.intYear.ToString(), "")),
                                   LiberadaRevisao = liberadasRevisao.Where(a => a.intProductID == apostila.intBookID).Select(al => al.bitRevisar ?? false).FirstOrDefault()
                               };

                return consulta.ToList();
            }
        }
    }
}