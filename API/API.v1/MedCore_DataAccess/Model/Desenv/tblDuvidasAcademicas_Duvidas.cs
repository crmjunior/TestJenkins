using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_Duvidas
    {
        public tblDuvidasAcademicas_Duvidas()
        {
            tblDuvidasAcademicas_Denuncia = new HashSet<tblDuvidasAcademicas_Denuncia>();
            tblDuvidasAcademicas_DuvidaApostila = new HashSet<tblDuvidasAcademicas_DuvidaApostila>();
            tblDuvidasAcademicas_DuvidaQuestao = new HashSet<tblDuvidasAcademicas_DuvidaQuestao>();
            tblDuvidasAcademicas_DuvidasHistorico = new HashSet<tblDuvidasAcademicas_DuvidasHistorico>();
            tblDuvidasAcademicas_Lidas = new HashSet<tblDuvidasAcademicas_Lidas>();
            tblDuvidasAcademicas_Log = new HashSet<tblDuvidasAcademicas_Log>();
            tblDuvidasAcademicas_Notificacao = new HashSet<tblDuvidasAcademicas_Notificacao>();
            tblDuvidasAcademicas_Resposta = new HashSet<tblDuvidasAcademicas_Resposta>();
            tblNotificacaoDuvidas = new HashSet<tblNotificacaoDuvidas>();
        }

        public int intDuvidaID { get; set; }
        public int intClientID { get; set; }
        public DateTime dteDataCriacao { get; set; }
        public string txtDescricao { get; set; }
        public bool? bitModerado { get; set; }
        public bool bitAtiva { get; set; }
        public DateTime dteAtualizacao { get; set; }
        public string txtOrigem { get; set; }
        public string txtOrigemSubnivel { get; set; }
        public bool? bitEditado { get; set; }
        public string txtCurso { get; set; }
        public string txtCidadeFilial { get; set; }
        public string txtEstadoFilial { get; set; }
        public string txtNomeFake { get; set; }
        public string txtEstadoFake { get; set; }
        public bool? bitAtivaDesenv { get; set; }
        public int? intAcademicoID { get; set; }
        public string txtOrigemProduto { get; set; }

        public virtual tblPersons intAcademico { get; set; }
        public virtual tblPersons intClient { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Denuncia> tblDuvidasAcademicas_Denuncia { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_DuvidaApostila> tblDuvidasAcademicas_DuvidaApostila { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_DuvidaQuestao> tblDuvidasAcademicas_DuvidaQuestao { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_DuvidasHistorico> tblDuvidasAcademicas_DuvidasHistorico { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Lidas> tblDuvidasAcademicas_Lidas { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Log> tblDuvidasAcademicas_Log { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Notificacao> tblDuvidasAcademicas_Notificacao { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Resposta> tblDuvidasAcademicas_Resposta { get; set; }
        public virtual ICollection<tblNotificacaoDuvidas> tblNotificacaoDuvidas { get; set; }
    }
}
