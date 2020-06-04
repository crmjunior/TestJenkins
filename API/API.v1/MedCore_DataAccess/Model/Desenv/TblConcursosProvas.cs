using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursosProvas
    {
        public int intProvaID { get; set; }
        public int? intTipoProvaID { get; set; }
        public int? intPeso { get; set; }
        public string dteDataInicio { get; set; }
        public string dteDataTermino { get; set; }
        public int? intConcursoID { get; set; }
        public string strPeriodo { get; set; }
        public int? intSituacaoID { get; set; }
        public DateTime? dteUltimaAtualizacao { get; set; }
        public string strObs { get; set; }
        public bool? bitBuscarProva { get; set; }
        public string strDiaSemana { get; set; }
        public bool? bitALiberar { get; set; }
        public int? intNumQuestoes { get; set; }
        public string txtConcursoR1 { get; set; }
        public int? intEditalID { get; set; }
    }
}
