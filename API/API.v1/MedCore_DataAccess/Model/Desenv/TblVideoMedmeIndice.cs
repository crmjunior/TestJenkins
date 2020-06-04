using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideoMedmeIndice
    {
        public tblVideoMedmeIndice()
        {
            tblVideoMedme = new HashSet<tblVideoMedme>();
        }

        public int intVideoIndiceID { get; set; }
        public int intEnfermidadeID { get; set; }
        public int? intOrdem { get; set; }
        public DateTime? dteCadastro { get; set; }

        public virtual ICollection<tblVideoMedme> tblVideoMedme { get; set; }
    }
}
