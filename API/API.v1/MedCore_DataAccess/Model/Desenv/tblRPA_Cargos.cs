using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPA_Cargos
    {
        public tblRPA_Cargos()
        {
            tblRPA = new HashSet<tblRPA>();
        }

        public int intCargoID { get; set; }
        public string txtCargo { get; set; }

        public virtual ICollection<tblRPA> tblRPA { get; set; }
    }
}
