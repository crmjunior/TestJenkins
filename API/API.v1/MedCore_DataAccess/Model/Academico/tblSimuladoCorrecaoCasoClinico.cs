using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblSimuladoCorrecaoCasoClinico
    {
        public long intId { get; set; }
        public short? intQuestaoOrderId { get; set; }
        public long? intQuestaoId { get; set; }
        public long intSimuladoId { get; set; }
        public long intContactId { get; set; }
        public long? intStoreId { get; set; }
        public long? intProfessorId { get; set; }
        public int intStatus { get; set; }
        public double? dblNota { get; set; }
        public string txtCoordCorrecao { get; set; }
        public DateTime? dteDistribuicao { get; set; }
        public string txtObsRevisao { get; set; }
        public DateTime? dtePrazo { get; set; }
    }
}
