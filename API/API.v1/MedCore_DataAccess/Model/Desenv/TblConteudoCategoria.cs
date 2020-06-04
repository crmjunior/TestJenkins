using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConteudoCategoria
    {
        public int intID { get; set; }
        public string txtTitulo { get; set; }
        public string txtAlias { get; set; }
        public DateTime dteData { get; set; }
        public int intEmployeeId { get; set; }
        public bool bitAtivo { get; set; }
        public int intTipo { get; set; }
    }
}
