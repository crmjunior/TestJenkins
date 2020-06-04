using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblGaleriaRelacaoImagem
    {
        public int intRelacaoId { get; set; }
        public int intGaleriaImagemId { get; set; }
        public int intImagemId { get; set; }
        public int intEmployeeId { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblGaleriaImagem intGaleriaImagem { get; set; }
        public virtual tblImagemGaleria intImagem { get; set; }
    }
}
