using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterialApostilaAluno
    {
        public tblMaterialApostilaAluno()
        {
            tblMaterialApostilaAluno_Comentario = new HashSet<tblMaterialApostilaAluno_Comentario>();
        }

        public int intID { get; set; }
        public int? intMaterialApostilaID { get; set; }
        public int? intClientID { get; set; }
        public string txtConteudo { get; set; }
        public DateTime dteDataCriacao { get; set; }
        public int? bitAtiva { get; set; }
        public string txtApostilaNameId { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblMaterialApostila intMaterialApostila { get; set; }
        public virtual ICollection<tblMaterialApostilaAluno_Comentario> tblMaterialApostilaAluno_Comentario { get; set; }
    }
}
