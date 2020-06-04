using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblInscricoesRessalvas
    {
        public int intMsgID { get; set; }
        public string txtMsgText { get; set; }
        public int intMsgYear { get; set; }
        public bool bitMsgProducao { get; set; }
        public int intStoreID { get; set; }
        public int intProductGroupID { get; set; }
        public int? intCourseID { get; set; }
        public bool bitInscricaoSomenteCentral { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblProductGroups1 intProductGroup { get; set; }
        public virtual tblStores intStore { get; set; }
    }
}
