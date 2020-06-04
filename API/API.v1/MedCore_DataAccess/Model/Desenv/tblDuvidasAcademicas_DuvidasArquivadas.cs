using System;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_DuvidasArquivadas
    {
        public int intDuvidaArquivadaID { get; set; }
        public Nullable<int> intDuvidaID { get; set; }
        public Nullable<int> intClientID { get; set; }
        public Nullable<bool> bitRespMaisTarde { get; set; }
        public Nullable<DateTime> dteDataCriacao { get; set; }
    
        public virtual tblPersons tblPersons { get; set; }
        public virtual tblDuvidasAcademicas_Duvidas tblDuvidasAcademicas_Duvidas { get; set; }
    }
}