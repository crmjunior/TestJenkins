using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Combo
    {
        public int intComboId { get; set; }
        public int intObjectId { get; set; }
        public string txtNome { get; set; }
        public int? intProductGroup2Id { get; set; }
        public int? intOrdem { get; set; }
    }
}
