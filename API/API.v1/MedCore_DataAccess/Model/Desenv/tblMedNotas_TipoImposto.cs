using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedNotas_TipoImposto
    {
        public tblMedNotas_TipoImposto()
        {
            tblMedNotas_Guia = new HashSet<tblMedNotas_Guia>();
        }

        public int intTipoImpostoID { get; set; }
        public string txtTipoImpostoNome { get; set; }

        public virtual ICollection<tblMedNotas_Guia> tblMedNotas_Guia { get; set; }
    }
}
