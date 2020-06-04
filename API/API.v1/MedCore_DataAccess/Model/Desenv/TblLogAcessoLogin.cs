using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLogAcessoLogin
    {
        public int intLogAcessoLoginID { get; set; }
        public int intClientID { get; set; }
        public int intAplicacaoID { get; set; }
        public DateTime dteDate { get; set; }
        public int intAcessoID { get; set; }
    }
}
