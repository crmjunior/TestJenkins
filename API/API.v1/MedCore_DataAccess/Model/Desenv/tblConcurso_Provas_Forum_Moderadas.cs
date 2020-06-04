using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso_Provas_Forum_Moderadas
    {
        public int intProvaForumId { get; set; }
        public bool? bitModerado { get; set; }
        public int intProvaForumModeradaID { get; set; }
    }
}
