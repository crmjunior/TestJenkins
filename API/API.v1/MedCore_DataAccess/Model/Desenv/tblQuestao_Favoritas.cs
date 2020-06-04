using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestao_Favoritas
    {
        public int intQuestaoFavoritaID { get; set; }
        public int intQuestaoID { get; set; }
        public int intProfessorID { get; set; }
        public DateTime dteDataCadastro { get; set; }
        public bool bitAtivo { get; set; }
        public string txtJustificativa { get; set; }
    }
}
