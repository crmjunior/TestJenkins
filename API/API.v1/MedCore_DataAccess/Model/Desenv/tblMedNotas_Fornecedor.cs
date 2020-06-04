using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedNotas_Fornecedor
    {
        public tblMedNotas_Fornecedor()
        {
            tblMedNotas_Nota = new HashSet<tblMedNotas_Nota>();
        }

        public int intFornecedorID { get; set; }
        public int intEmpresaID { get; set; }

        public virtual tblMedNotas_Empresa intEmpresa { get; set; }
        public virtual ICollection<tblMedNotas_Nota> tblMedNotas_Nota { get; set; }
    }
}
