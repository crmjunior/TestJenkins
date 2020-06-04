using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestao_Duvida_Encaminhamento
    {
        public int intQuestaoDuvidaEncaminhamentoID { get; set; }
        public int intQuestaoDuvidaID { get; set; }
        public int? intRemetenteID { get; set; }
        public int intDestinatarioID { get; set; }
        public DateTime dteEncaminhamento { get; set; }
        public bool bitAtivo { get; set; }

        public virtual tblPersons intDestinatario { get; set; }
        public virtual tblQuestao_Duvida intQuestaoDuvida { get; set; }
        public virtual tblPersons intRemetente { get; set; }
    }
}
