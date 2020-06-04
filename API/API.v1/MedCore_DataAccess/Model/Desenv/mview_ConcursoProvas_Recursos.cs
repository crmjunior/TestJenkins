using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class mview_ConcursoProvas_Recursos
    {
        public string SG_CONCURSO { get; set; }
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
        public string NM_CONCURSO { get; set; }
        public int? VL_ANO_CONCURSO { get; set; }
        public string CD_UF { get; set; }
        public int intOrderTipoProva { get; set; }
    }
}
