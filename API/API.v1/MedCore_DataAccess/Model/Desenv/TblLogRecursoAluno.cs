using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogRecursoAluno
    {
        public int intID { get; set; }
        public int intClientID { get; set; }
        public int intQuestaoID { get; set; }
        public DateTime dteDataCriacao { get; set; }
        public bool bitVisualizadoBanca { get; set; }
        public bool bitVisualizadoMedgrupo { get; set; }
    }
}
