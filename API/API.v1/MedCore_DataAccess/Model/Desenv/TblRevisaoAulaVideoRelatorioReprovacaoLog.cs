using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevisaoAulaVideoRelatorioReprovacaoLog
    {
        public int intId { get; set; }
        public int intCursoId { get; set; }
        public string txtApostilaSigla { get; set; }
        public string txtTema { get; set; }
        public int? intProfessorId { get; set; }
        public string txtVideoTitulo { get; set; }
        public int intVideoId { get; set; }
        public int? intEmployeeId { get; set; }
        public DateTime dteReprovacao { get; set; }
        public string txtJustificativa { get; set; }
        public int? intTipoAprovador { get; set; }
    }
}
