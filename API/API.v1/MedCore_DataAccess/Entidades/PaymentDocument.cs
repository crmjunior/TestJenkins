using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "PaymentDocument", Namespace = "a")]
    public class PaymentDocument
    {
        [DataMember(Name = "IsRAC")]
        public bool IsRAC { get; set; }

        [DataMember(Name = "SellOrderInfoID")]
        public int SellOrderInfoID { get; set; }

        [DataMember(Name = "PaymentDocumentID")]
        public int PaymentDocumentID { get; set; }

        [DataMember(Name = "PaymentDocumentValue")]
        public double PaymentDocumentValue { get; set; }

        [DataMember(Name = "PaymentDocumentDescription")]
        public string PaymentDocumentDescription { get; set; }
    }
}