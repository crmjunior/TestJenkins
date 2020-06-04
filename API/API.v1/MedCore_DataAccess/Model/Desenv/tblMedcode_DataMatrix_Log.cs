using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedcode_DataMatrix_Log
    {
        public int intDataMatrixLogID { get; set; }
        public int? intEmployeeIdAlteracao { get; set; }
        public DateTime? dteDataAlteracao { get; set; }
        public int intDataMatrixID { get; set; }
        public int intBookID { get; set; }
        public int intProductGroup { get; set; }
        public int intMediaID { get; set; }
        public int intMediaTipo { get; set; }
        public DateTime dteDateTime { get; set; }
        public string txtDescription { get; set; }
        public int? intIndex { get; set; }
        public string txtMediaCode { get; set; }
        public int intEmployeeId { get; set; }
        public string txtArquivo { get; set; }
    }
}
