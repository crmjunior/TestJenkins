using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Screen_Action
    {
        public tblAccess_Screen_Action()
        {
            tblAccess_PersonalException = new HashSet<tblAccess_PersonalException>();
        }

        public int intScreenActionID { get; set; }
        public int intScreenID { get; set; }
        public int intActionID { get; set; }

        public virtual tblAccess_Action intAction { get; set; }
        public virtual tblAccess_Screen intScreen { get; set; }
        public virtual ICollection<tblAccess_PersonalException> tblAccess_PersonalException { get; set; }
    }
}
