using System;

namespace MedCore_DataAccess.Entidades
{
    public partial class csp_CustomClient_PagamentosProdutosGeral_Result
    {
        public Nullable<int> intClientID { get; set; }
        public Nullable<int> intOrderID { get; set; }
        public string txtName { get; set; }
        public string txtRegister { get; set; }
        public Nullable<int> intYear { get; set; }
        public Nullable<int> intMonth { get; set; }
        public string txtComment { get; set; }
        public string txtStatus { get; set; }
        public Nullable<double> dblValue { get; set; }
        public Nullable<double> dblSumOfPaymt { get; set; }
        public Nullable<double> dblBalance { get; set; }
    }
}