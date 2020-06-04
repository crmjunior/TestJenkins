using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMedCode_MarcasCompartilhadas
    {
        public int intMarcasCompartilhadasId { get; set; }
        public int intBookOriginal { get; set; }
        public int intBookCompartilhadaId { get; set; }
    }
}
