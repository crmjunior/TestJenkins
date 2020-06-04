using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestao_Duvida
    {
        public tblQuestao_Duvida()
        {
            tblQuestao_Duvida_Encaminhamento = new HashSet<tblQuestao_Duvida_Encaminhamento>();
        }

        public int intQuestaoDuvidaId { get; set; }
        public int intQuestaoId { get; set; }
        public int intTipoExercicioId { get; set; }
        public int intClientId { get; set; }
        public int intApplicationId { get; set; }
        public string txtPergunta { get; set; }
        public DateTime? dtePergunta { get; set; }
        public string txtResposta { get; set; }
        public DateTime? dteResposta { get; set; }
        public bool bitActive { get; set; }

        public virtual ICollection<tblQuestao_Duvida_Encaminhamento> tblQuestao_Duvida_Encaminhamento { get; set; }
    }
}
