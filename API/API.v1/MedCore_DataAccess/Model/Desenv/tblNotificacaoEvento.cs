using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblNotificacaoEvento
    {
        public int intNotificacaoEvento { get; set; }
        public int? intNotificacaoId { get; set; }
        public string Metadados { get; set; }
        public int intContactId { get; set; }
        public string txtTitulo { get; set; }
        public string txtDescricao { get; set; }
        public DateTime? dteCadastro { get; set; }
        public int? intStatus { get; set; }
        public int? intStatusLeitura { get; set; }
        public bool bitAtivo { get; set; }

        public virtual tblNotificacao intNotificacao { get; set; }
    }
}
