using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterialApostilaConfig
    {
        public int intID { get; set; }
        public string txtDescricao { get; set; }
        public DateTime dteDataAtualizacao { get; set; }
        public bool bitAtiva { get; set; }
    }
}
