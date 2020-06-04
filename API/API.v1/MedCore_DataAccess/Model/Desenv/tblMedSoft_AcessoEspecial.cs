using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedSoft_AcessoEspecial
    {
        public int intAcessoEspecialID { get; set; }
        public int intObjectId { get; set; }
        public int intClientID { get; set; }
        public DateTime? dteDataHoraAlteracao { get; set; }
    }
}
