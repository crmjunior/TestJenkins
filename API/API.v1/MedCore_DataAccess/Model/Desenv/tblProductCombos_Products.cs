using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProductCombos_Products
    {
        public int intComboID { get; set; }
        public int intProductID { get; set; }

        public virtual tblProducts intProduct { get; set; }
    }
}
