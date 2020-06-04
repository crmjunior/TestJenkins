using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblContribuicao
    {
        public tblContribuicao()
        {
            tblContribuicaoArquivo = new HashSet<tblContribuicaoArquivo>();
            tblContribuicao_Encaminhadas = new HashSet<tblContribuicao_Encaminhadas>();
            tblContribuicoes_Arquivadas = new HashSet<tblContribuicoes_Arquivadas>();
            tblContribuicoes_Interacao = new HashSet<tblContribuicoes_Interacao>();
        }

        public int intContribuicaoID { get; set; }
        public int? intClientID { get; set; }
        public int? intApostilaID { get; set; }
        public DateTime? dteDataCriacao { get; set; }
        public string txtDescricao { get; set; }
        public bool? bitAtiva { get; set; }
        public string txtOrigem { get; set; }
        public bool? bitEditado { get; set; }
        public int? intNumCapitulo { get; set; }
        public string txtTrechoSelecionado { get; set; }
        public string txtCodigoMarcacao { get; set; }
        public bool? bitAprovacaoMedgrupo { get; set; }
        public string txtOrigemSubnivel { get; set; }
        public int? intMedGrupoID { get; set; }
        public int? intTipoCategoria { get; set; }
        public int? intTipoContribuicao { get; set; }
        public string txtEstado { get; set; }
        public int? intOpcaoPrivacidade { get; set; }

        public virtual tblMaterialApostila intApostila { get; set; }
        public virtual tblPersons intClient { get; set; }
        public virtual ICollection<tblContribuicaoArquivo> tblContribuicaoArquivo { get; set; }
        public virtual ICollection<tblContribuicao_Encaminhadas> tblContribuicao_Encaminhadas { get; set; }
        public virtual ICollection<tblContribuicoes_Arquivadas> tblContribuicoes_Arquivadas { get; set; }
        public virtual ICollection<tblContribuicoes_Interacao> tblContribuicoes_Interacao { get; set; }
    }
}
