using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_ObjectType
    {
        public tblAccess_ObjectType()
        {
            tblAccess_Object = new HashSet<tblAccess_Object>();
            tblDocumento = new HashSet<tblDocumento>();
        }

        public int intObjectTypeId { get; set; }
        public string txtType { get; set; }

        public virtual ICollection<tblAccess_Object> tblAccess_Object { get; set; }
        public virtual ICollection<tblDocumento> tblDocumento { get; set; }
    }
}
