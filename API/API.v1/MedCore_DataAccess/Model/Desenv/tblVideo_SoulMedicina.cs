using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideo_SoulMedicina
    {
        public int intSoulMedicinaId { get; set; }
        public int intSoulMedicina_TipoId { get; set; }
        public int intVideoId { get; set; }

        public virtual tblSoulMedicina_Tipo intSoulMedicina_Tipo { get; set; }
    }
}
