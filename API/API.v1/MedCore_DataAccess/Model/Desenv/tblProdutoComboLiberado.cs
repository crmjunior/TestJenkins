using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProdutoComboLiberado
    {
        public int intID { get; set; }
        public int intClientId { get; set; }
        public int intCurso { get; set; }
        public int intYear { get; set; }
        public bool bitFake { get; set; }
    }
}
