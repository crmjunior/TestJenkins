using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoQuestoesGravacaoProtocolo_Catalogo
    {
        public tblConcursoQuestoesGravacaoProtocolo_Catalogo()
        {
            tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT = new HashSet<tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT>();
            tblConcursoQuestoesGravacaoProtocolo_Questoes = new HashSet<tblConcursoQuestoesGravacaoProtocolo_Questoes>();
        }

        public int intProtocoloID { get; set; }
        public string txtNome { get; set; }
        public string intTypeID { get; set; }
        public int intEmployeeID { get; set; }
        public bool? bitExport { get; set; }
        public byte[] timestamp { get; set; }
        public DateTime? dteDataProtocolo { get; set; }
        public int? intEntityID { get; set; }

        public virtual ICollection<tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT> tblConcursoQuestoesGravacaoProtocolo_CatalogoPPT { get; set; }
        public virtual ICollection<tblConcursoQuestoesGravacaoProtocolo_Questoes> tblConcursoQuestoesGravacaoProtocolo_Questoes { get; set; }
    }
}
