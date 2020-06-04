using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideoMedme
    {
        public int intVideoMedmeID { get; set; }
        public int? intVideoMedmeIndiceID { get; set; }
        public int? intProfessorID { get; set; }
        public string txtDescricao { get; set; }
        public int? intOrdem { get; set; }
        public int? intVideoID { get; set; }
        public DateTime? dteCadastro { get; set; }

        public virtual tblVideoMedmeIndice intVideoMedmeIndice { get; set; }
    }
}
