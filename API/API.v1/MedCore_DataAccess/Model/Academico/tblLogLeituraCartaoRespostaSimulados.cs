using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblLogLeituraCartaoRespostaSimulados
    {
        public tblLogLeituraCartaoRespostaSimulados()
        {
            //tblSimuladoImportacaoCartaoResposta = new HashSet<tblSimuladoImportacaoCartaoResposta>();
            //tblSimuladoRanking_Fase01 = new HashSet<tblSimuladoRanking_Fase01>();
            tblSimuladoRespostas = new HashSet<tblSimuladoRespostas>();
            tblSimuladoResultados = new HashSet<tblSimuladoResultados>();
        }

        public int intLogID { get; set; }
        public int? intEmployeeID { get; set; }
        public DateTime? dteDate { get; set; }
        public string txtSimuladoID { get; set; }
        public string txtNomeArquivo { get; set; }
        public bool? bitInicioImp { get; set; }
        public bool? bitFimImp { get; set; }
        public int? intQtdLidos { get; set; }
        public int? intClassRoomID { get; set; }
        public DateTime? dteFimImportacao { get; set; }

        //public virtual ICollection<tblSimuladoImportacaoCartaoResposta> tblSimuladoImportacaoCartaoResposta { get; set; }
        //public virtual ICollection<tblSimuladoRanking_Fase01> tblSimuladoRanking_Fase01 { get; set; }
        public virtual ICollection<tblSimuladoRespostas> tblSimuladoRespostas { get; set; }
        public virtual ICollection<tblSimuladoResultados> tblSimuladoResultados { get; set; }
    }
}
