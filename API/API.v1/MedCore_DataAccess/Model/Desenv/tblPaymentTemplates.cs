using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPaymentTemplates
    {
        public tblPaymentTemplates()
        {
            this.tblStore_CombosPaymentTemplate = new HashSet<tblStore_CombosPaymentTemplate>();
            this.tblSellOrdersTemplate = new HashSet<tblSellOrdersTemplate>();
            this.tblTemplateDescontoTurmaCPMED = new HashSet<tblTemplateDescontoTurmaCPMED>();
        }
    
        public int intPaymentTemplateID { get; set; }
        public string txtPaymentTemplateName { get; set; }
        public string txtShortName { get; set; }
        public Nullable<int> intPaymentTemplateTypeID { get; set; }
        public int intConditionTypeID { get; set; }
    
        public virtual ICollection<tblStore_CombosPaymentTemplate> tblStore_CombosPaymentTemplate { get; set; }
        public virtual ICollection<tblSellOrdersTemplate> tblSellOrdersTemplate { get; set; }
        public virtual ICollection<tblTemplateDescontoTurmaCPMED> tblTemplateDescontoTurmaCPMED { get; set; }
    }
}