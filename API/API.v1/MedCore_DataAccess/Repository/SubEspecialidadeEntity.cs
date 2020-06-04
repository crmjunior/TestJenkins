using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Repository
{
    public class SubEspecialidadeEntity
    {
        public List<SubEspecialidade> GetChildren(int classificacaoID)
		{
            var ctx = new DesenvContext();
			var subespecialidade = ctx.tblConcursoQuestaoCatologoDeClassificacoes
								.Where(catalogo => catalogo.intParent == classificacaoID)
								.Select(catalogo => new SubEspecialidade()
								{
									ID = catalogo.intClassificacaoID,
									Nome = catalogo.txtTipoDeClassificacao + " - " + catalogo.txtSubTipoDeClassificacao,
									GrandeArea = classificacaoID
								})
								.ToList();

			subespecialidade.ForEach(sub =>
			{
				sub.SubEspecialidades = new List<SubEspecialidade>();
				sub.SubEspecialidades.AddRange(GetChildren(sub.ID));
			});

			return subespecialidade;
		}

        public List<SubEspecialidade> GetAll()
		{
            var ctx = new DesenvContext();
            var consulta = (from s in ctx.tblConcursoQuestaoCatologoDeClassificacoes
                            join sp in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on s.intClassificacaoID equals sp.intClassificacaoID
                            join p in ctx.tblProducts on sp.intProductGroupID equals p.intProductGroup3
							where s.intTipoDeClassificacao == 3
							||
							(
							 p.intProductGroup1 == 4
							 && p.intProductGroup2 == (int)Produto.Cursos.MEDMEDCURSO
							 && p.intProductGroup3 != 22
							)
							select new { id = (s.intTipoDeClassificacao == 3) ? sp.intProductGroupID : sp.intClassificacaoID, s.txtSubTipoDeClassificacao, intGrandeArea = (s.intTipoDeClassificacao == 3) ? 11 : 34 }).Distinct();

			var lst = new List<SubEspecialidade>();
			foreach (var valor in consulta)
			{
				var s = new SubEspecialidade
				{
					ID = valor.id,
					Nome = valor.txtSubTipoDeClassificacao,
					GrandeArea = valor.intGrandeArea
				};
				lst.Add(s);
			}
            //Inclusão do Manual Antib. (Ano Anterior)
            lst.Add(new SubEspecialidade
            {
                ID = 11,
                Nome = "Clínica Médica",
                GrandeArea = 34
            });
			return lst;
		}
    }
}