using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedNotas_Empresa
    {
        public tblMedNotas_Empresa()
        {
            tblMedNotas_Cliente = new HashSet<tblMedNotas_Cliente>();
            tblMedNotas_Fornecedor = new HashSet<tblMedNotas_Fornecedor>();
        }

        public int intEmpresaID { get; set; }
        public string txtNome { get; set; }
        public string txtCNPJ { get; set; }

        public virtual ICollection<tblMedNotas_Cliente> tblMedNotas_Cliente { get; set; }
        public virtual ICollection<tblMedNotas_Fornecedor> tblMedNotas_Fornecedor { get; set; }
    }
}
