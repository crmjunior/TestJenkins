using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestoesGravacaoProtocolo_Questoes
    {
        public int intProtocoloID { get; set; }
        public int intQuestaoID { get; set; }
        public string txtCodeCopiar { get; set; }
        public string txtCodeDeletar { get; set; }
        public int? intEmployeeID { get; set; }
        public bool? bitValida { get; set; }
        public bool? bitReutilizar { get; set; }
        public byte[] timestamp { get; set; }
        public DateTime? dteDistribuitionQuest { get; set; }
        public DateTime? dteValidationQuest { get; set; }

        public virtual tblConcursoQuestoesGravacaoProtocolo_Catalogo intProtocolo { get; set; }
    }
}
