using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Curso
    {
        public tblRequisicoes_Curso()
        {
            tblRequisicoes_Requisicao = new HashSet<tblRequisicoes_Requisicao>();
        }

        public int intRequisicaoCursoId { get; set; }
        public string txtDescricao { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual ICollection<tblRequisicoes_Requisicao> tblRequisicoes_Requisicao { get; set; }
    }
}
