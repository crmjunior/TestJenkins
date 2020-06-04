using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPerfil
    {
        public tblPerfil()
        {
            tblPerfil_Employees = new HashSet<tblPerfil_Employees>();
            tblPerfil_Funcionalidade = new HashSet<tblPerfil_Funcionalidade>();
        }

        public int intPerfilID { get; set; }
        public string txtNome { get; set; }
        public string txtDescricao { get; set; }

        public virtual ICollection<tblPerfil_Employees> tblPerfil_Employees { get; set; }
        public virtual ICollection<tblPerfil_Funcionalidade> tblPerfil_Funcionalidade { get; set; }
    }
}
