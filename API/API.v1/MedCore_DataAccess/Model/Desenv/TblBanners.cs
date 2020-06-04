using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBanners
    {
        public int intBannerId { get; set; }
        public int intObjectId { get; set; }
        public string txtDescricao { get; set; }
        public string txtImagem { get; set; }
        public string txtLink { get; set; }
        public bool bitLinkExterno { get; set; }
        public string txtClickAqui { get; set; }
        public int? intSimuladoID { get; set; }

        public virtual tblAccess_Object intObject { get; set; }
    }
}
