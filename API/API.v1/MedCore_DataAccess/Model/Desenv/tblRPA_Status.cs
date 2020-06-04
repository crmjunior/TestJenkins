using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPA_Status
    {
        public tblRPA_Status()
        {
            tblRPA = new HashSet<tblRPA>();
        }

        public int intStatusID { get; set; }
        public string txtStatus { get; set; }

        public virtual ICollection<tblRPA> tblRPA { get; set; }
    }
}
