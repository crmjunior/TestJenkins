using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblDuvidasAcademicas_Resposta
    {
        public tblDuvidasAcademicas_Resposta()
        {
            InverseintParentResposta = new HashSet<tblDuvidasAcademicas_Resposta>();
            tblDuvidasAcademicas_Denuncia = new HashSet<tblDuvidasAcademicas_Denuncia>();
            tblDuvidasAcademicas_Log = new HashSet<tblDuvidasAcademicas_Log>();
            tblDuvidasAcademicas_RespostaHistorico = new HashSet<tblDuvidasAcademicas_RespostaHistorico>();
        }

        public int intRespostaID { get; set; }
        public int intDuvidaID { get; set; }
        public int intClientID { get; set; }
        public int? bitAprovacaoMedgrupo { get; set; }
        public DateTime dteDataCriacao { get; set; }
        public bool? bitRespostaMed { get; set; }
        public bool? bitModerado { get; set; }
        public string txtDescricao { get; set; }
        public bool bitAtiva { get; set; }
        public DateTime dteAtualizacao { get; set; }
        public bool? bitEditado { get; set; }
        public int? intParentRespostaID { get; set; }
        public string txtCurso { get; set; }
        public string txtObservacao { get; set; }
        public string intMedGrupoID { get; set; }
        public string txtCidadeFilial { get; set; }
        public string txtEstadoFilial { get; set; }
        public string txtNomeFake { get; set; }
        public string txtEstadoFake { get; set; }
        public bool? bitAtivaDesenv { get; set; }
        public int? intAcademicoID { get; set; }

        public virtual tblPersons intAcademico { get; set; }
        public virtual tblPersons intClient { get; set; }
        public virtual tblDuvidasAcademicas_Duvidas intDuvida { get; set; }
        public virtual tblDuvidasAcademicas_Resposta intParentResposta { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Resposta> InverseintParentResposta { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Denuncia> tblDuvidasAcademicas_Denuncia { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Log> tblDuvidasAcademicas_Log { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_RespostaHistorico> tblDuvidasAcademicas_RespostaHistorico { get; set; }
    }
}
