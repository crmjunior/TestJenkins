using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAvaliacaoConteudoQuestaoAlternativas
    {
        public tblAvaliacaoConteudoQuestaoAlternativas()
        {
            tblAvaliacaoConteudoQuestao = new HashSet<tblAvaliacaoConteudoQuestao>();
        }

        public int intID { get; set; }
        public string txtDescricao { get; set; }
        public bool bitAtiva { get; set; }

        public virtual ICollection<tblAvaliacaoConteudoQuestao> tblAvaliacaoConteudoQuestao { get; set; }
    }
}
