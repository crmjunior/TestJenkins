using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEmed_Duvidas
    {
        public long idEmed_Duvida { get; set; }
        public string txtTexto_Duvida { get; set; }
        public DateTime dteDuvida_Enviada { get; set; }
        public long intClientID { get; set; }
        public long? intIdEncAprovado { get; set; }
        public long? intStatusDuvidaAluno { get; set; }
        public long? intStatusDuvidaSistema { get; set; }
        public string txtUID { get; set; }
        public int? intProductID { get; set; }
        public int? intApostilaID { get; set; }
        public int? intPagina { get; set; }
        public string txtURLImagem { get; set; }
        public bool? bitAtiva { get; set; }
        public int? intDuvidaPaiID { get; set; }
        public string txtTexto_DuvidaEditado { get; set; }
        public bool? bitFaq { get; set; }
    }
}
