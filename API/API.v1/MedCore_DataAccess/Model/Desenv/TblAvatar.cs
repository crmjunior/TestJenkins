using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAvatar
    {
        public int intAvatarTypeID { get; set; }
        public string txtAvatarPath { get; set; }
        public bool? bitActive { get; set; }
        public DateTime dteDateTime { get; set; }
        public int intCategoryID { get; set; }
        public int intAvatarID { get; set; }

        public virtual tblAvatar_Types intAvatarType { get; set; }
        public virtual tblAvatar_Category intCategory { get; set; }
    }
}
