using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursos
    {
        public int intConcursoID { get; set; }
        public string txtDescription { get; set; }
        public string txtSigla { get; set; }
        public int? intAno { get; set; }
        public string txtImagem { get; set; }
        public int? intCity { get; set; }
        public int? intStateID { get; set; }
        public int? intEmployeeID { get; set; }
        public string dblInscricaoTaxa { get; set; }
        public string strInscricaoObs { get; set; }
        public string strInscricaoPeriodo { get; set; }
        public string strAvaliacaoMedgrupo { get; set; }
        public string dteDataProva { get; set; }
        public bool? bitCadernoLiberado { get; set; }
        public int? intNroQuestoes { get; set; }
        public string strBibliografia { get; set; }
        public string dtePrazoRecurso { get; set; }
        public string dblValorRecurso { get; set; }
        public string strListaClassificados { get; set; }
        public string strForumProvaAnoPassado { get; set; }
        public int? intNroQuestoesRecurso { get; set; }
        public int? intNroQuestoesAlteradas { get; set; }
        public string strResumo { get; set; }
        public string strRecursoPrazoTexto { get; set; }
        public string dteRecursoPrazoReal { get; set; }
        public string strRecursoObs { get; set; }
        public string strProvasObs { get; set; }
        public string strResultadosObs { get; set; }
        public string strGabaritosObs { get; set; }
        public int? intConcursoIDOrigem { get; set; }
        public int? bitProducao { get; set; }
        public bool? bitValidadoFull { get; set; }
        public int? intConcursoIDDestino { get; set; }
        public bool? bitEnviarEmail { get; set; }
        public bool? bitEditalDivulgado { get; set; }
        public string dteEditalDivulgado { get; set; }
        public string dtePrevisaoEditalDivulgado { get; set; }
        public string dblValorInscricao { get; set; }
        public string strBibliografiaTexto { get; set; }
        public bool bitExibirSiteRecursos { get; set; }
    }
}
