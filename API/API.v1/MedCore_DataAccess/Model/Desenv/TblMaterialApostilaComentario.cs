using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterialApostilaComentario
    {
        public int intID { get; set; }
        public int intApostilaID { get; set; }
        public int intClientID { get; set; }
        public string txtComentarioID { get; set; }
        public string txtComentario { get; set; }
        public int intVersaoMinima { get; set; }
        public int? intVersaoMaxima { get; set; }

        public virtual tblMaterialApostila intApostila { get; set; }
        public virtual tblPersons intClient { get; set; }
    }
}
