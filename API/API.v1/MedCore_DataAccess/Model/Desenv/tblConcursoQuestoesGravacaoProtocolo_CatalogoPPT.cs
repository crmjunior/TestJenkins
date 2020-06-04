using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT
    {
        public int intPPTControleID { get; set; }
        public int intProtocoloID { get; set; }
        public string txtNamePPT { get; set; }
        public DateTime dteCriacao { get; set; }
        public int? intEmployeeID { get; set; }
        public int? intTipoQuestaoID { get; set; }

        public virtual tblConcursoQuestoesGravacaoProtocolo_Catalogo intProtocolo { get; set; }
    }
}
