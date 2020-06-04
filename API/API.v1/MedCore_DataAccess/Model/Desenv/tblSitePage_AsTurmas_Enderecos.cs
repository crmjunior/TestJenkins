using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSitePage_AsTurmas_Enderecos
    {
        public int intEnderecoID { get; set; }
        public int? intStoreID { get; set; }
        public string txtLocal { get; set; }
        public string txtLat { get; set; }
        public string txtLng { get; set; }
        public string txtGoogleMapLink { get; set; }
        public int? intCourseID { get; set; }
        public string txtDetalhe { get; set; }
    }
}
