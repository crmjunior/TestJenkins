using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Application
    {
        public tblAccess_Application()
        {
            tblAccess_Menu_Apagar = new HashSet<tblAccess_Menu_Apagar>();
            tblAccess_Screen = new HashSet<tblAccess_Screen>();
            tblAnoInscricao = new HashSet<tblAnoInscricao>();
            tblCallCenterCallsInadimplenciaLog = new HashSet<tblCallCenterCallsInadimplenciaLog>();
            tblDocumento = new HashSet<tblDocumento>();
            tblLessonsEvaluation = new HashSet<tblLessonsEvaluation>();
        }

        public int intApplicationID { get; set; }
        public string txtApplication { get; set; }

        public virtual ICollection<tblAccess_Menu_Apagar> tblAccess_Menu_Apagar { get; set; }
        public virtual ICollection<tblAccess_Screen> tblAccess_Screen { get; set; }
        public virtual ICollection<tblAnoInscricao> tblAnoInscricao { get; set; }
        public virtual ICollection<tblCallCenterCallsInadimplenciaLog> tblCallCenterCallsInadimplenciaLog { get; set; }
        public virtual ICollection<tblDocumento> tblDocumento { get; set; }
        public virtual ICollection<tblLessonsEvaluation> tblLessonsEvaluation { get; set; }
    }
}
