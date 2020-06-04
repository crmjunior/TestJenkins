using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConteudoMaterialMedTipo
    {
        public tblConteudoMaterialMedTipo()
        {
            tblConteudoMaterialMed = new HashSet<tblConteudoMaterialMed>();
        }

        public int intConteudoMaterialMedTipoID { get; set; }
        public string ConteudoMaterialMedTipo { get; set; }

        public virtual ICollection<tblConteudoMaterialMed> tblConteudoMaterialMed { get; set; }
    }
}
