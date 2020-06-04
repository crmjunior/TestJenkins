using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCtrlPanel_Link
    {
        public int intLinkID { get; set; }
        public string txtLink { get; set; }
        public int? intPosicao { get; set; }
        public string txtUrl { get; set; }
        public int? intReference { get; set; }
        public bool? BitVisible { get; set; }
        public int? intLinkPai { get; set; }
    }
}
