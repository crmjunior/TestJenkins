using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedmeAreas
    {
        public int intAreaId { get; set; }
        public string txtTitulo { get; set; }
        public int? intIconID { get; set; }
        public bool bitAtivo { get; set; }
    }
}
