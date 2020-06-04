using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblExercicio_MontaProva
    {
        public tblExercicio_MontaProva()
        {
            tblQuestao_MontaProva = new HashSet<tblQuestao_MontaProva>();
        }

        public int intID { get; set; }
        public int intClientId { get; set; }
        public DateTime dteDataCriacao { get; set; }
        public string txtNome { get; set; }
        public bool? bitAtivo { get; set; }
        public int? intFiltroId { get; set; }

        public virtual ICollection<tblQuestao_MontaProva> tblQuestao_MontaProva { get; set; }
    }
}
