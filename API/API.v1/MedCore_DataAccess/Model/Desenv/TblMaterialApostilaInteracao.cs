using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterialApostilaInteracao
    {
        public int intID { get; set; }
        public int intApostilaID { get; set; }
        public int intClientID { get; set; }
        public string txtInteracaoID { get; set; }
        public string txtComentario { get; set; }
        public int intVersaoMinima { get; set; }
        public int? intVersaoMaxima { get; set; }
        public string txtConteudo { get; set; }
        public int intTipoInteracao { get; set; }
        public string txtLinkMedia { get; set; }
        public int? intDuracaoMedia { get; set; }
        public bool? bitTipoComentarioMedia { get; set; }
        public DateTime? dteCriacao { get; set; }

        public virtual tblMaterialApostila intApostila { get; set; }
        public virtual tblPersons intClient { get; set; }
    }
}
