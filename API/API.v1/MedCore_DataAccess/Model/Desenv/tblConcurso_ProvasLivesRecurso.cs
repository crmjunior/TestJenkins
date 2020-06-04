using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_ProvasLivesRecurso
    {
        public int intConcursoProvasLivesRecursoID { get; set; }
        public int intProvaID { get; set; }
        public string txtUrlProvaGabaritada { get; set; }
        public string txtUrlOtimizadaProvaGabaritada { get; set; }
        public bool bitLiberarProvaSemRevisao { get; set; }
        public bool bitExibirFacebookLivePortalRecursos { get; set; }
        public string txtUrl { get; set; }
        public DateTime dteData { get; set; }
        public string txtAliasUrlProvaGabaritada { get; set; }
    }
}
