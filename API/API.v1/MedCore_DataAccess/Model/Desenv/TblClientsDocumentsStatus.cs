using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClientsDocumentsStatus
    {
        public tblClientsDocumentsStatus()
        {
            tblClientsDocuments = new HashSet<tblClientsDocuments>();
        }

        public int intStatusID { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblClientsDocuments> tblClientsDocuments { get; set; }
    }
}
