using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConteudoMaterialMed
    {
        public int intConteudoMaterialMedID { get; set; }
        public DateTime data { get; set; }
        public string imgPequena { get; set; }
        public string imgGrande { get; set; }
        public string titulo { get; set; }
        public string conteudo { get; set; }
        public int intEmployeeID { get; set; }
        public DateTime dataInclusao { get; set; }
        public int intConteudoMaterialMedTipoID { get; set; }
        public bool? Ativo { get; set; }

        public virtual tblConteudoMaterialMedTipo intConteudoMaterialMedTipo { get; set; }
    }
}
