using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMednet_AlunoComentario
    {
        public int intMnComentarioId { get; set; }
        public int intClientId { get; set; }
        public int intRevisaoAulaId { get; set; }
        public int intTempo { get; set; }
        public string txtTitulo { get; set; }
        public string txtComentario { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
