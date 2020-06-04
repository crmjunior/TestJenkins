using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_DuvidaApostila
    {
        public int intDuvidaApostilaId { get; set; }
        public int intDuvidaId { get; set; }
        public int intMaterialApostilaId { get; set; }
        public int? intTipoCategoria { get; set; }
        public int? intNumCapitulo { get; set; }
        public string txtTrecho { get; set; }
        public string txtCodigoMarcacao { get; set; }

        public virtual tblDuvidasAcademicas_Duvidas tblDuvidasAcademicas_Duvidas { get; set; }
        public virtual tblMaterialApostila intMaterialApostila { get; set; }
    }
}
