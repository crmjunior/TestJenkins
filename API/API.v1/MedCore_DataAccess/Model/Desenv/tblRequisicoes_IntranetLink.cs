using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_IntranetLink
    {
        public int intIntranetLinkId { get; set; }
        public string txtIntranetId { get; set; }
        public string txtIntranetLink { get; set; }
        public int? intRequisicaoId { get; set; }
        public bool? bAtivo { get; set; }

        public virtual tblRequisicoes_Requisicao intRequisicao { get; set; }
    }
}
