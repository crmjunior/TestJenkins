using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAvisos_Chamados
    {
        public int intAvisoChamado { get; set; }
        public int intAvisoId { get; set; }
        public int? intChamadoCategoriaId { get; set; }
        public int? intStatusInternoId { get; set; }
        public string txtNome { get; set; }
        public int intOrdem { get; set; }
        public bool bitAtivo { get; set; }

        public virtual tblAvisos intAviso { get; set; }
    }
}
