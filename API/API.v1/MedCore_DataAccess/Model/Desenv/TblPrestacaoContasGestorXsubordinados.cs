using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblPrestacaoContasGestorXsubordinados
    {
        public int intGestorID { get; set; }
        public int intSubordinadoID { get; set; }
        public int intID { get; set; }

        public virtual tblEmployees intGestor { get; set; }
        public virtual tblEmployees intSubordinado { get; set; }
    }
}
