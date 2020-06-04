using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblImagemSemana
    {
        public int intImagemSemanaID { get; set; }
        public DateTime dteImagemSemana { get; set; }
        public string txtDescricao { get; set; }
        public string txtImagemGrande { get; set; }
        public string txtImagemPequena { get; set; }
        public string txtImagemResposta { get; set; }
        public string txtResposta { get; set; }
        public bool bitLiberarImagemSemana { get; set; }
        public bool bitLiberarResposta { get; set; }
        public string txtThumb { get; set; }
        public bool? Ativo { get; set; }
    }
}
