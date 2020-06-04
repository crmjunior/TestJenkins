using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblQuestao_Duvida_Resposta
    {
        public int intQuestaoDuvidaRespostaId { get; set; }
        public int intEncaminhamentoId { get; set; }
        public string txtResposta { get; set; }
        public DateTime dteResposta { get; set; }
        public bool bitActive { get; set; }
    }
}
