using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLabel_SmartInfo
    {
        public int intLabelSmartInfoId { get; set; }
        public string txtDescricao { get; set; }
        public string txtRegraEntrada { get; set; }
        public string txtRegraSaida { get; set; }
        public int? intLabelId { get; set; }
        public int intOrdem { get; set; }
        public bool? bitAtivo { get; set; }
    }
}
