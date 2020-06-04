using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAcess_Object_Validity
    {
        public int intValidityId { get; set; }
        public int intObjectId { get; set; }
        public DateTime dteInicio { get; set; }
        public DateTime dteFim { get; set; }
        public bool bitAtivo { get; set; }

        public virtual tblAccess_Object intObject { get; set; }
    }
}
