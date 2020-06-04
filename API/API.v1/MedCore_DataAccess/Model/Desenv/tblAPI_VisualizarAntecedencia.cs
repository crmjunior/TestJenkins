using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAPI_VisualizarAntecedencia
    {
        public int intContactID { get; set; }
        public bool? bitActive { get; set; }
        public DateTime dteDateTime { get; set; }
    }
}
