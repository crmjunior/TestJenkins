using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAvatar_Types
    {
        public tblAvatar_Types()
        {
            tblAvatar = new HashSet<tblAvatar>();
        }

        public int intAvatarTypeID { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblAvatar> tblAvatar { get; set; }
    }
}
