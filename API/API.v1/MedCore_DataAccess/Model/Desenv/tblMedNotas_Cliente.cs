using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedNotas_Cliente
    {
        public tblMedNotas_Cliente()
        {
            tblMedNotas_Nota = new HashSet<tblMedNotas_Nota>();
        }

        public int intClienteID { get; set; }
        public int intEmpresaID { get; set; }

        public virtual tblMedNotas_Empresa intEmpresa { get; set; }
        public virtual ICollection<tblMedNotas_Nota> tblMedNotas_Nota { get; set; }
    }
}
