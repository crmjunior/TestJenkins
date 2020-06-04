using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAvatar_Category
    {
        public tblAvatar_Category()
        {
            tblAvatar = new HashSet<tblAvatar>();
        }

        public int intAvatarCategoryID { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblAvatar> tblAvatar { get; set; }
    }
}
