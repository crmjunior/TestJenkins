using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblTurmaExcecaoCpfBase
    {
        public int intTurmaExcecaoCpfBaseId { get; set; }
        public int intCourseId { get; set; }
        public int intYear { get; set; }
        public int intMatriculaBaseId { get; set; }
        public DateTime dteCadastro { get; set; }
        public int intDiasLimite { get; set; }
        public int intProdutoID { get; set; }
    }
}
