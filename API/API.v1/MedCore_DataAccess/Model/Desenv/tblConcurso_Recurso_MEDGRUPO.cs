using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_Recurso_MEDGRUPO
    {
        public int ID { get; set; }
        public int intOrdem { get; set; }
        public int? intContactID { get; set; }
        public int? intQuestaoID { get; set; }
        public int? intProvaID { get; set; }
        public string txtEditor { get; set; }
        public string txtRecurso_Comentario { get; set; }
        public DateTime? dteCadastro { get; set; }
        public bool? bitActive { get; set; }
        public bool? bitOpiniao { get; set; }
        public int? intTipo { get; set; }
        public string txtForum { get; set; }
        public int? intStateID { get; set; }
        public int? intEspecialidadeID { get; set; }
        public string txtPicturePath { get; set; }
        public bool bitSelarForum { get; set; }
        public int? ID_CONCURSO_RECURSO_ALUNO { get; set; }
        public string LoggedUser { get; set; }
        public int? intEmployeeID { get; set; }
        public int? IdCoordenador { get; set; }
    }
}
