using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConteudoLog
    {
        public int intLogID { get; set; }
        public int intID { get; set; }
        public int intPaiID { get; set; }
        public int intPosicao { get; set; }
        public string txtTitulo { get; set; }
        public string txtDescricao { get; set; }
        public int intTipoVersao { get; set; }
        public DateTime dteData { get; set; }
        public int intEmployeeId { get; set; }
        public bool bitAtivo { get; set; }
        public string txtHierarquia { get; set; }
        public int intCategoria { get; set; }
        public bool bitPublicado { get; set; }
        public bool bitRevertido { get; set; }
        public int intNivel { get; set; }
    }
}
