using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblFiltroAluno_MontaProva
    {
        public int intID { get; set; }
        public int intClientId { get; set; }
        public DateTime dteDataCriacao { get; set; }
        public string txtNome { get; set; }
        public string txtJsonFiltro { get; set; }
        public string txtConcursos { get; set; }
        public string txtAnos { get; set; }
        public string txtPalavraChave { get; set; }
        public bool? bitAtivo { get; set; }
        public string txtEspecialidades { get; set; }
        public string txtFiltrosEspeciais { get; set; }
        public int? intQtdQuestoes { get; set; }
    }
}
