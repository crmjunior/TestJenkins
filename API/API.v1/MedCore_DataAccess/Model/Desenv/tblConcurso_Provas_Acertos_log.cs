using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_Provas_Acertos_log
    {
        public int intProvaAcertosID { get; set; }
        public int intContactID { get; set; }
        public string txtComentario { get; set; }
        public DateTime dteCadastro { get; set; }
        public string txtIP_Usuario { get; set; }
        public bool? bitActive { get; set; }
        public int? intProvaID { get; set; }
        public string txtNickname { get; set; }
        public int? intAcertos { get; set; }
        public int? intEspecialidadeID { get; set; }
    }
}
