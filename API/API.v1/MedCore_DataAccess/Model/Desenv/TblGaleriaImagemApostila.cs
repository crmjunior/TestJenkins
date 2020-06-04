using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblGaleriaImagemApostila
    {
        public int intGaleriaImagemApostilaId { get; set; }
        public int intGaleriaImagemId { get; set; }
        public int intEmployeeId { get; set; }
        public int intBookId { get; set; }
        public int intPagina { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAtivo { get; set; }

        public virtual tblBooks intBook { get; set; }
        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblGaleriaImagem intGaleriaImagem { get; set; }
    }
}
