using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEmployeeCargos
    {
        public tblEmployeeCargos()
        {
            tblEmployees = new HashSet<tblEmployees>();
            tblRequisicoes_PerfilItem = new HashSet<tblRequisicoes_PerfilItem>();
        }

        public int intCargoID { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblEmployees> tblEmployees { get; set; }
        public virtual ICollection<tblRequisicoes_PerfilItem> tblRequisicoes_PerfilItem { get; set; }
    }
}
