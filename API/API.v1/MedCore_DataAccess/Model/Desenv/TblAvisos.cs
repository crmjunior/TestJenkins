using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAvisos
    {
        public tblAvisos()
        {
            tblAvisos_Chamados = new HashSet<tblAvisos_Chamados>();
        }

        public int intAvisoID { get; set; }
        public string txtAviso { get; set; }
        public int intApplicationID { get; set; }
        public string txtDescricao { get; set; }
        public string txtTitulo { get; set; }

        public virtual ICollection<tblAvisos_Chamados> tblAvisos_Chamados { get; set; }
    }
}
