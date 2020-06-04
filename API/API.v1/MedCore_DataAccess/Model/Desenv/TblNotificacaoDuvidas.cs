using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblNotificacaoDuvidas
    {
        public int intNotificacaoDuvidaId { get; set; }
        public int? intNotificacaoId { get; set; }
        public int? intDuvidaId { get; set; }
        public int? intContactId { get; set; }
        public string txtDescricao { get; set; }
        public DateTime? dteCadastro { get; set; }
        public int? intStatus { get; set; }
        public int? intTipoCategoria { get; set; }

        public virtual tblPersons intContact { get; set; }
        public virtual tblDuvidasAcademicas_Duvidas intDuvida { get; set; }
        public virtual tblNotificacao intNotificacao { get; set; }
    }
}
