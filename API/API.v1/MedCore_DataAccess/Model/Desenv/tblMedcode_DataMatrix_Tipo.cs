using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedcode_DataMatrix_Tipo
    {
        public tblMedcode_DataMatrix_Tipo()
        {
            tblMedcode_DataMatrix = new HashSet<tblMedcode_DataMatrix>();
        }

        public int intMediaTipoID { get; set; }
        public string txtMediaDescription { get; set; }

        public virtual ICollection<tblMedcode_DataMatrix> tblMedcode_DataMatrix { get; set; }
    }
}
