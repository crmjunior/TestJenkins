using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblStores_Complement
    {
        public int intStoreID { get; set; }
        public string txtLat { get; set; }
        public string txtLng { get; set; }
        public string txtGoogleMapLink { get; set; }
        public string txtSateliteAviso { get; set; }
        public string txtInfoTitle { get; set; }
        public string txtInfoAviso { get; set; }
        public string txtRevisoesTitle { get; set; }
        public string txtRevisoesAviso { get; set; }
    }
}
