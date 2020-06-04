using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_Provas
    {
        public tblConcurso_Provas()
        {
            tblBloqueioConcurso = new HashSet<tblBloqueioConcurso>();
            tblConcurso_Provas_ArquivosAluno = new HashSet<tblConcurso_Provas_ArquivosAluno>();
        }

        public int intProvaID { get; set; }
        public int? ID_CONCURSO { get; set; }
        public string txtName { get; set; }
        public string txtDescription { get; set; }
        public int ID_CONCURSO_RECURSO_STATUS { get; set; }
        public DateTime? dteDate { get; set; }
        public DateTime? dteLightboxExpirationDate { get; set; }
        public bool? bitAtivo { get; set; }
        public string txtGabaritoPreliminar { get; set; }
        public string txtGabaritoFinal { get; set; }
        public string txtProva { get; set; }
        public int? intDVDID { get; set; }
        public bool? bitVendaLiberada { get; set; }
        public bool bitAtiva { get; set; }
        public string txtBibliografia { get; set; }
        public string txtLightBox { get; set; }
        public bool? bitActiveLightBox { get; set; }
        public bool bitRecursoForumAcertosLiberado { get; set; }
        public int? intRecursoForumAcertosQtdQuestoes { get; set; }
        public DateTime? dteExpiracao { get; set; }
        public DateTime? dteDateTimeForumComentBlocked { get; set; }
        public Guid guidProvaID { get; set; }
        public DateTime dteDateTimeLastUpdate { get; set; }
        public int intProvaTipoID { get; set; }
        public DateTime? dteProvaInicioDateTime { get; set; }
        public DateTime? dteProvaFimDateTime { get; set; }
        public bool? bitPrevisaoLiberacaoProva { get; set; }
        public string txtEditaLiberacaoProva { get; set; }
        public DateTime? dteLibProvaDateTime { get; set; }
        public string txtLocalLibGabarito { get; set; }
        public string txtLocalLibGabaritoPos { get; set; }
        public string txtObs { get; set; }
        public string txtEditaRecurso { get; set; }
        public DateTime? dteLibGabaritoDateTime { get; set; }
        public DateTime? dteLibGabaritoPosDateTime { get; set; }
        public DateTime? dtePrevLibGabaritoDateTime { get; set; }
        public string txtEndereco { get; set; }
        public string txtLocalLiberacao { get; set; }
        public bool? bitDataProvaConfirmada { get; set; }
        public bool? bitProvaFisica { get; set; }
        public int? intStatusID { get; set; }
        public string txtTipoProva { get; set; }
        public bool? bitEditalPreveLiberacao { get; set; }
        public DateTime? dtePrevLibGabaritoPosDateTime { get; set; }
        public bool? bitProvaLiberada { get; set; }
        public bool bitSobDemanda { get; set; }
        public bool? bitUploadAluno { get; set; }
        public bool bitProvaSemGabarito { get; set; }
        public string txtTituloPainelAvisos { get; set; }
        public string txtDestaquePainelAvisos { get; set; }

        public virtual ICollection<tblBloqueioConcurso> tblBloqueioConcurso { get; set; }
        public virtual ICollection<tblConcurso_Provas_ArquivosAluno> tblConcurso_Provas_ArquivosAluno { get; set; }
    }
}
