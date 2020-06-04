using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLabelGroups
    {
        public tblLabelGroups()
        {
            tblLabels = new HashSet<tblLabels>();
        }

        public int intLabelGroupID { get; set; }
        public string txtName { get; set; }

        public virtual ICollection<tblLabels> tblLabels { get; set; }
    }
}
