using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class DAEmailItemDTO
    {
        public string Professor { get; set; }

        public bool TemEntidade { get; set; }

        public int Menos2Dias { get; set; }

        public int Entre2e7Dias { get; set; }

        public int Mais7Dias { get; set; }

        public int Total { get; set; }

        public int Questoes { get; set; }

        public int Apostilas { get; set; }

        public int SemVinculo { get; set; }

        public int Encaminhadas { get; set; }

        public int TotalOrigem { get; set; }

        public int TotalGeralDias { get; set; }

        public int TotalGeralOrigem { get; set; }

        public int PrimeirasMenos2Dias { get; set; }

        public int PrimeirasEntre2e7Dias { get; set; }

        public int PrimeirasMais7Dias { get; set; }

        public int PrimeirasTotal { get; set; }

        public int PrimeirasQuestoes { get; set; }

        public int PrimeirasApostilas { get; set; }

        public int PrimeirasSemVinculo { get; set; }

        public int PrimeirasEncaminhadas { get; set; }

        public int PrimeirasTotalOrigem { get; set; }

        public int TotaisRespondidas { get; set; }

        public Dictionary<int, int> DuvidasResolvidas { get; set; }
    }
}