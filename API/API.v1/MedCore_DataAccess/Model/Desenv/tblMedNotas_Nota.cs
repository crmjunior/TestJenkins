using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedNotas_Nota
    {
        public tblMedNotas_Nota()
        {
            tblMedNotas_Guia = new HashSet<tblMedNotas_Guia>();
        }

        public int intNotaID { get; set; }
        public int intFornecedorID { get; set; }
        public int intClienteID { get; set; }
        public DateTime? dteDataPagamento { get; set; }
        public string txtNumero { get; set; }
        public double dblValorBruto { get; set; }
        public double dblValorLiquido { get; set; }
        public string txtObservacao { get; set; }
        public DateTime dteDataAlteracao { get; set; }
        public int intUsuarioID { get; set; }
        public DateTime dteEmissao { get; set; }

        public virtual tblMedNotas_Cliente intCliente { get; set; }
        public virtual tblMedNotas_Fornecedor intFornecedor { get; set; }
        public virtual ICollection<tblMedNotas_Guia> tblMedNotas_Guia { get; set; }
    }
}
