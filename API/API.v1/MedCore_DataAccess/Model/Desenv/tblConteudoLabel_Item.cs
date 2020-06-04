using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConteudoLabel_Item
    {
        public int intConteudoLabelItemId { get; set; }
        public int intConteudoId { get; set; }
        public int intConteudoLabelId { get; set; }
        public bool bitAtivo { get; set; }
    }
}
