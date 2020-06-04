using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Device
    {
        public tblAccess_Device()
        {
            tblInscricoes_Log = new HashSet<tblInscricoes_Log>();
        }

        public int intDeviceTipoID { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblInscricoes_Log> tblInscricoes_Log { get; set; }
    }
}
