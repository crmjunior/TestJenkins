using System;

namespace MedCore_DataAccess.DTO
{
    public class QuestaoMarcacaoDTO
    {
        public int intID { get; set; }

        public int intClientID { get; set; }

        public int intQuestaoID { get; set; }

        public int intTipoExercicioID { get; set; }

        public string txtAnotacao { get; set; }

        public DateTime dtAnotacao { get; set; }

        public bool bitFlagEmDuvida { get; set; }

        public bool bitFlagFavorita { get; set; }
    }
}