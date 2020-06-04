using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPerfil1
    {
        public tblPerfil1()
        {
            tblPerfil_Employees1 = new HashSet<tblPerfil_Employees1>();
            tblPerfil_Funcionalidade1 = new HashSet<tblPerfil_Funcionalidade1>();
        }

        public int intPerfilID { get; set; }
        public string txtNome { get; set; }
        public string txtDescricao { get; set; }

        public virtual ICollection<tblPerfil_Employees1> tblPerfil_Employees1 { get; set; }
        public virtual ICollection<tblPerfil_Funcionalidade1> tblPerfil_Funcionalidade1 { get; set; }
    }
}
