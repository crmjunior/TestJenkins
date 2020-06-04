using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRequisicoes_Anexo
    {
        public int intRequisicaoAnexoId { get; set; }
        public int intEntidadeId { get; set; }
        public int intAnexoTipoId { get; set; }
        public string txtAnexoLink { get; set; }
        public int intEmployeeId { get; set; }
        public DateTime dteAnexoData { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
    }
}
