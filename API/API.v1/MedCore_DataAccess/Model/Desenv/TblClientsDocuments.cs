using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClientsDocuments
    {
        public int intDocumentID { get; set; }
        public int intClientID { get; set; }
        public string txtDocument { get; set; }
        public int intStatusID { get; set; }
        public int intEmployeeID { get; set; }
        public DateTime dteDate { get; set; }
        public bool bitExibirPainel { get; set; }
        public int intTypeID { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblClientsDocumentsStatus intStatus { get; set; }
        public virtual tblClientsDocumentsTypes intType { get; set; }
    }
}
