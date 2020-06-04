using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_Recurso_AccessDenied
    {
        public int intClientID { get; set; }
        public DateTime? dteDateTimeStart { get; set; }
        public DateTime? dteDateTimeEnd { get; set; }
        public string txtReason { get; set; }
        public bool? bitActive { get; set; }
        public string txtLoggedUser { get; set; }
        public string txtSolicitador { get; set; }
        public string txtAutorizador { get; set; }
        public string txtMotivoDesbloqueio { get; set; }
        public int? intSolicitadorId { get; set; }
        public int? intAutorizadorId { get; set; }
        public int intConcursoRecursoId { get; set; }
        public int? intBlackListCategoriaID { get; set; }
    }
}
