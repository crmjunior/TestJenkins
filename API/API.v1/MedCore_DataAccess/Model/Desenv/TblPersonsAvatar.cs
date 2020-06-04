using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPersonsAvatar
    {
        public int intContactID { get; set; }
        public int intAvatarID { get; set; }
        public bool? bitActive { get; set; }
        public DateTime dteDateTime { get; set; }
    }
}
