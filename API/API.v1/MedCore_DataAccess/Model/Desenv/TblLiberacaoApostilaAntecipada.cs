using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLiberacaoApostilaAntecipada
    {
        public int intLiberacaoApostilaAntecipadaID { get; set; }
        public int? intContactID { get; set; }
        public DateTime? dteDataCadastro { get; set; }

        public virtual tblPersons intContact { get; set; }
    }
}
