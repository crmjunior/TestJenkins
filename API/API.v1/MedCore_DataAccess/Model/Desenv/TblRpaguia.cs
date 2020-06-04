using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPAGuia
    {
        public tblRPAGuia()
        {
            tblRPAGuia_Status_Historico = new HashSet<tblRPAGuia_Status_Historico>();
        }

        public int intID { get; set; }
        public int intRPAID { get; set; }
        public DateTime dteInclusao { get; set; }
        public int intStatusID { get; set; }
        public string txtOBS { get; set; }
        public int intCourseID { get; set; }
        public float Valor { get; set; }
        public int intUsuarioInclusao { get; set; }
        public int intEntregueID { get; set; }
        public DateTime? dteOriginalRecebido { get; set; }
        public DateTime? dtePagamento { get; set; }
        public DateTime dteGuia { get; set; }
        public int? intQtdDependentes { get; set; }

        public virtual tblRPAGuia_Status intStatus { get; set; }
        public virtual ICollection<tblRPAGuia_Status_Historico> tblRPAGuia_Status_Historico { get; set; }
    }
}
