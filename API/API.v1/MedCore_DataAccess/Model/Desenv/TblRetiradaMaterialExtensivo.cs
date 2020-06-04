using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRetiradaMaterialExtensivo
    {
        public int intRetiradaMaterialExtensivoId { get; set; }
        public int intAtributo { get; set; }
        public string txtDescricao { get; set; }
        public int intCursoId { get; set; }
        public int intAno { get; set; }
        public int intClassificacaoId { get; set; }
        public int? intWareHouseId { get; set; }
    }
}
