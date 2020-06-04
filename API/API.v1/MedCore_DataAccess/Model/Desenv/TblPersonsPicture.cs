using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPersonsPicture
    {
        public int intContactID { get; set; }
        public int intPictureTypeID { get; set; }
        public string txtPicturePath { get; set; }
        public bool? bitActive { get; set; }
        public DateTime dteDateTime { get; set; }

        public virtual tblPersons intContact { get; set; }
    }
}
