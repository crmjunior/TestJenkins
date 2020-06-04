using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace MedCore_DataAccess.Repository
{
    public class GrandeAreaEntity
    {
        public List<GrandeArea> GetAll()
        {
            using (var ctx = new DesenvContext())
            {
                var grandeArea = ctx.tblConcursoQuestaoCatologoDeClassificacoes
                                        .Where(area => area.intTipoDeClassificacao == 2)
                                        .Select(area => new GrandeArea()
                                        {
                                            ID = area.intClassificacaoID,
                                            Nome = area.txtSubTipoDeClassificacao
                                        })
                                        .ToList();

                grandeArea.ForEach(ga =>
                {
                    ga.SubEspecialidades = new List<SubEspecialidade>();
                    ga.SubEspecialidades.AddRange(new SubEspecialidadeEntity().GetChildren(ga.ID));
                });

                return grandeArea;
            }
        }

        public List<GrandeArea> GetAll(int intProductGroupId)
        {

            var clinicaMedica = (int)GrandeArea.Especiais.ClinicaMedica;
            var outras = (int)GrandeArea.Especiais.Outras;
            var lst = new List<GrandeArea>();
            var ctx = new DesenvContext();
            var esp = (new SubEspecialidadeEntity()).GetAll();
            if (intProductGroupId == (int)Produto.Cursos.MED || intProductGroupId == (int)Produto.Cursos.MEDCURSO || intProductGroupId == (int)Produto.Cursos.RA || intProductGroupId == (int)Produto.Cursos.ADAPTAMED)
            {
                var idgrupo = (intProductGroupId == (int)Produto.Cursos.RA || intProductGroupId == (int)Produto.Cursos.ADAPTAMED) ? intProductGroupId : 0;
                
                

                var consulta2 = ctx.Set<msp_API_LoadGrandeArea_Result>().FromSqlRaw("msp_API_LoadGrandeArea @intProductGroup2 = {0}", idgrupo).ToList();

                foreach (var it in consulta2)
                {
                    var ga = new GrandeArea
                    {
                        ID = it.ID,
                        Nome = it.Nome
                    };

                    if (intProductGroupId.Equals((int)Produto.Cursos.MEDCURSO) && (ga.ID.Equals(clinicaMedica) || ga.ID.Equals(outras)))
                    {
                        ga.SubEspecialidades = esp.Where(i => i.GrandeArea == ga.ID).ToList();
                    }

                    lst.Add(ga);
                }

            }
            if (intProductGroupId.Equals((int)Produto.Cursos.MEDCURSO))
            {
                lst = lst.Where(i => !esp.Where(j => j.GrandeArea == outras).Any(x => x.ID == i.ID && i.ID != clinicaMedica)).ToList();
            }
            if (intProductGroupId.Equals((int)Produto.Cursos.MED))
            {
                lst = lst.Where(i => i.ID != outras).ToList();
            }
            return lst;
        }
    }
}