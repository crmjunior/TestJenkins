using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAnexo
    {
        public int intAnexoId { get; set; }
        public int intEntidadeId { get; set; }
        public int intTipoId { get; set; }
        public int intEntidadeTipoId { get; set; }
        public int intProdutoTipoId { get; set; }
        public string txtLink { get; set; }
        public string txtNome { get; set; }
        public int intEmployeeId { get; set; }
        public int? intAno { get; set; }
        public DateTime dteData { get; set; }
        public bool? bitAtivo { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
    }
}
