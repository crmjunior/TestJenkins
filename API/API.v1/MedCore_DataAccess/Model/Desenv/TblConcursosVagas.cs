using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursosVagas
    {
        public int intVagaID { get; set; }
        public int? intConcursoID { get; set; }
        public int? intInstituicaoID { get; set; }
        public int? intEspecialidadeID { get; set; }
        public int? intQuantidade { get; set; }
        public int? intOrigemVagaID { get; set; }
        public int? intSituacaoID { get; set; }
        public int? intOrigemSituacaoID { get; set; }
        public double? dblNotaCorte { get; set; }
        public int? intOrigemNotaCorteID { get; set; }
        public string txtR1R3 { get; set; }
        public int? intCandidatos { get; set; }
        public DateTime? dteUltimaAtualizacao { get; set; }
        public double? dblVagasCandidato { get; set; }
        public bool? bitExibirInstSite { get; set; }
        public int? intOrigemCandVaga { get; set; }
        public double? dblNotaCorte1 { get; set; }
        public int? intOrigemNotaCorte1ID { get; set; }
    }
}
