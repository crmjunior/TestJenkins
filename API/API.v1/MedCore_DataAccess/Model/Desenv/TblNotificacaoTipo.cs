using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblNotificacaoTipo
    {
        public int intNotificacaoTipoId { get; set; }
        public string txtDescricao { get; set; }
        public int intOrdem { get; set; }
        public string txtAlias { get; set; }
    }
}
