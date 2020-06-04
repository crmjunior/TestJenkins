using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Repository
{
    public class LabelEntity
    {
        public List<int> ListarItemMarcados(int aIdLabel, int aIdContact) 
        {
            var label = GetByFilters(new Label() { Id = aIdLabel, IdContato = aIdContact }).FirstOrDefault(l => l.Id == aIdLabel);
            if (label != null)
                return label.Detalhes.Select(d => d.IdItemMarcado).ToList();
            else
                return new List<int>() { -1 };
        }    

        public List<Label> GetByFilters(Label registro) 
        {
            using (var ctx = new DesenvContext()) 
            {
                var labels = (from l in ctx.tblLabels where l.bitAtivo.Value select l);
                var grupos = (from g in ctx.tblLabelGroups select g);

                if (registro.Id > 0)
                labels = labels.Where(l => l.intLabelID == registro.Id);
                else 
                {
                grupos = registro.IdGrupoLabel > 0 ?
                        grupos.Where(g => g.intLabelGroupID == registro.IdGrupoLabel)
                        :
                        grupos.Where(g => g.txtName.ToUpper() == registro.GrupoLabel.ToUpper());

                labels = registro.IdContato > 0 ?
                        labels.Where(l => l.intContactID == registro.IdContato || l.bitPublico)
                        :
                        labels.Where(l => l.bitPublico);
                }

                var lista = (from g in grupos
                            join l in labels on g.intLabelGroupID equals l.intLabelGroupID
                            join p in ctx.tblPersons on l.intContactID equals p.intContactID
                            select new Label 
                            {
                            Id = l.intLabelID,
                            GrupoLabel = g.txtName,
                            IdGrupoLabel = g.intLabelGroupID,
                            Descricao = l.txtDescription,
                            Cor = l.txtColor,
                            ReadOnly = l.bitReadOnly,
                            Publico = l.bitPublico,
                            IdContato = l.intContactID,
                            NomeContato = p.txtName.TrimEnd(),
                            Detalhes = (from d in ctx.tblLabelDetails
                                        where d.intLabelID == l.intLabelID && d.bitAtivo.Value
                                        select new LabelDetalhe 
                                        {
                                            Id = d.intLabelDetailID,
                                            IdLabel = d.intLabelID,
                                            IdItemMarcado = d.intObjetoID,
                                            Padrao = d.bitPadrao
                                        })
                            }).ToList();
                return lista;
            }
        }    
    }
}