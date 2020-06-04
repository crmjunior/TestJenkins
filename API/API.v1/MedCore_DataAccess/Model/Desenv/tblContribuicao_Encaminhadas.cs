using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContribuicao_Encaminhadas
    {
        public int intContribuicaoEncaminhadaID { get; set; }
        public int intContribuicaoID { get; set; }
        public int intClientID { get; set; }
        public int intEmployeeID { get; set; }
        public DateTime dteDataEncaminhamento { get; set; }

        public virtual tblEmployees intClient { get; set; }
        public virtual tblContribuicao intContribuicao { get; set; }
        public virtual tblEmployees intEmployee { get; set; }
    }
}
