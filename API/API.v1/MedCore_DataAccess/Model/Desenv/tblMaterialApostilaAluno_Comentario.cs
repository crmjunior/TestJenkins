using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterialApostilaAluno_Comentario
    {
        public int intID { get; set; }
        public int intApostilaID { get; set; }
        public int intClientID { get; set; }
        public int intComentarioID { get; set; }
        public string txtComentario { get; set; }

        public virtual tblMaterialApostilaAluno intApostila { get; set; }
        public virtual tblPersons intClient { get; set; }
    }
}
