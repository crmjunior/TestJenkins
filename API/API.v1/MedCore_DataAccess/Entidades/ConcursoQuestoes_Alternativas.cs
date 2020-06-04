using System;

namespace MedCore_DataAccess.Entidades
{
    public class ConcursoQuestoes_Alternativas
    {
        public int intAlternativaID { get; set; }
        public int intQuestaoID { get; set; }
        public Nullable<int> intQuestaoIDOld { get; set; }
        public string txtLetraAlternativa { get; set; }
        public string txtAlternativa { get; set; }
        public Nullable<bool> bitCorreta { get; set; }
        public Nullable<bool> bitCorretaPreliminar { get; set; }
        public string txtResposta { get; set; }
        public string txtImagem { get; set; }
        public string txtImagemOtimizada { get; set; }
    }
}