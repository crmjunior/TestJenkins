using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedcode_DataMatrix
    {
        public int intDataMatrixID { get; set; }
        public int intBookID { get; set; }
        public int intProductGroup { get; set; }
        public int intMediaID { get; set; }
        public int intMediaTipo { get; set; }
        public DateTime dteDateTime { get; set; }
        public string txtDescription { get; set; }
        public int? intIndex { get; set; }
        public string txtMediaCode { get; set; }
        public int? intEmployeeId { get; set; }
        public string txtArquivo { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblMedcode_DataMatrix_Tipo intMediaTipoNavigation { get; set; }
    }
}
