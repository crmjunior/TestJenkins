using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContadorQuestoes_MontaProva
    {
        public int intId { get; set; }
        public int intProvaId { get; set; }
        public int intClientId { get; set; }
        public int intQuantidadeQuestoes { get; set; }
        public int intNaoRealizadas { get; set; }
        public int intAcertos { get; set; }
        public int intErros { get; set; }
        public DateTime dteDataCriacao { get; set; }
    }
}
