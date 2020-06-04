using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSoulMedicina_Tipo
    {
        public tblSoulMedicina_Tipo()
        {
            tblVideo_SoulMedicina = new HashSet<tblVideo_SoulMedicina>();
        }

        public int intSoulMedicina_TipoId { get; set; }
        public string txtNome { get; set; }

        public virtual ICollection<tblVideo_SoulMedicina> tblVideo_SoulMedicina { get; set; }
    }
}
