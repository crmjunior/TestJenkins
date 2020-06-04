using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPaymentDocuments
    {
        public tblPaymentDocuments()
        {
            tblLogMesesBlocoMaterialAnteriorAvulso = new HashSet<tblLogMesesBlocoMaterialAnteriorAvulso>();
        }

        public int intPaymentDocumentID { get; set; }
        public int intPaymentMethodID { get; set; }
        public int intPaymentStatusID { get; set; }
        public int intCounterpartyID { get; set; }
        public string txtDescription { get; set; }
        public DateTime dteDate { get; set; }
        public double dblValue { get; set; }
        public string txtDocumentNumber { get; set; }
        public int intCreditAccountID { get; set; }
        public int intDebitAccountID { get; set; }
        public int? intTranche { get; set; }
        public bool? bitPreview { get; set; }
        public int? intDocumentTypeID { get; set; }
        public string txtComplement { get; set; }
        public int? intStoreID { get; set; }
        public string txtBancoPagador { get; set; }
        public string txtAgenciaPagadora { get; set; }
        public string txtContaCorrentePagadora { get; set; }
        public int intSellOrderID { get; set; }
        public double? dblPaidValue { get; set; }
        public DateTime? dtePaymentDate { get; set; }

        public virtual tblPersons intCounterparty { get; set; }
        public virtual tblStores intStore { get; set; }
        public virtual ICollection<tblLogMesesBlocoMaterialAnteriorAvulso> tblLogMesesBlocoMaterialAnteriorAvulso { get; set; }
    }
}
