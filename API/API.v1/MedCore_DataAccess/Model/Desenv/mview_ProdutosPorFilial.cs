using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class mview_ProdutosPorFilial
    {
        public int intStoreID { get; set; }
        public int intProductID { get; set; }
        public bool? bitActive { get; set; }
    }
}
