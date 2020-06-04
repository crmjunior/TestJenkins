using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_YearType
    {
        public tblAccess_YearType()
        {
            tblAccess = new HashSet<tblAccess>();
            tblAccess_Condition = new HashSet<tblAccess_Condition>();
            tblAccess_PersonalException = new HashSet<tblAccess_PersonalException>();
        }

        public int intYearTypeID { get; set; }
        public int intYear { get; set; }
        public string txtValue { get; set; }
        public DateTime? dteStart { get; set; }
        public DateTime? dteEnd { get; set; }

        public virtual ICollection<tblAccess> tblAccess { get; set; }
        public virtual ICollection<tblAccess_Condition> tblAccess_Condition { get; set; }
        public virtual ICollection<tblAccess_PersonalException> tblAccess_PersonalException { get; set; }
    }
}
