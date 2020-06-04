using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDeviceToken
    {
        public int intID { get; set; }
        public int intClientID { get; set; }
        public string txtOneSignalToken { get; set; }
        public DateTime? dteDataCriacao { get; set; }
        public bool? bitIsTablet { get; set; }
        public bool? bitAtivo { get; set; }
        public int? intApplicationId { get; set; }

        public virtual tblPersons intClient { get; set; }
    }
}
