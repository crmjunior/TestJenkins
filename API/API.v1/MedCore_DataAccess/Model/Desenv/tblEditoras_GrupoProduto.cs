using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEditoras_GrupoProduto
    {
        public int ID { get; set; }
        public int? EditoraId { get; set; }
        public int? GrupoProdutoId { get; set; }
    }
}
