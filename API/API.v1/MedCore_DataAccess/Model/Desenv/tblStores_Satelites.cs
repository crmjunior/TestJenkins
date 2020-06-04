using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblStores_Satelites
    {
        public int intStoreID { get; set; }
        public string txtState { get; set; }
        public bool bitPrincipal { get; set; }

        public virtual tblStores intStore { get; set; }
    }
}
