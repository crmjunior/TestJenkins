using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClientsDocumentsTypes
    {
        public tblClientsDocumentsTypes()
        {
            tblClientsDocuments = new HashSet<tblClientsDocuments>();
        }

        public int intTypeID { get; set; }
        public string txtDescription { get; set; }
        public bool bitFinanceiro { get; set; }
        public int intTamanhoMaximo { get; set; }
        public bool bitPermiteDuplicidade { get; set; }

        public virtual ICollection<tblClientsDocuments> tblClientsDocuments { get; set; }
    }
}
