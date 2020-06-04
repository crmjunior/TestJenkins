using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLabels
    {
        public tblLabels()
        {
            tblLabelDetails = new HashSet<tblLabelDetails>();
        }

        public int intLabelID { get; set; }
        public string txtDescription { get; set; }
        public string txtColor { get; set; }
        public bool bitReadOnly { get; set; }
        public bool bitPublico { get; set; }
        public int intContactID { get; set; }
        public int intLabelGroupID { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblPersons intContact { get; set; }
        public virtual tblLabelGroups intLabelGroup { get; set; }
        public virtual ICollection<tblLabelDetails> tblLabelDetails { get; set; }
    }
}
