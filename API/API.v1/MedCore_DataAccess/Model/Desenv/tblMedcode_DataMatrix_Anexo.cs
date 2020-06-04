using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedcode_DataMatrix_Anexo
    {
        public int intAnexoID { get; set; }
        public int intDataMatrixID { get; set; }
        public string txtDescricaoAnexo { get; set; }
        public DateTime dteDataInclusao { get; set; }
        public bool? bitExcluido { get; set; }
    }
}
