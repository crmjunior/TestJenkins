using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDocumento
    {
        public tblDocumento()
        {
            tblContrato = new HashSet<tblContrato>();
        }

        public int intDocumentoId { get; set; }
        public int intObjectTypeId { get; set; }
        public int intApplicationID { get; set; }
        public int intAno { get; set; }
        public string txtDescricao { get; set; }

        public virtual tblAccess_Application intApplication { get; set; }
        public virtual tblAccess_ObjectType intObjectType { get; set; }
        public virtual ICollection<tblContrato> tblContrato { get; set; }
    }
}
