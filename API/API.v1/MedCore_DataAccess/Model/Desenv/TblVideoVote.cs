using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblVideoVote
    {
        public int intVideoVoteID { get; set; }
        public int? intVideoID { get; set; }
        public int? intBookID { get; set; }
        public DateTime? dteDataCriacao { get; set; }
        public int? intContactID { get; set; }
        public int? intTipoInteracaoId { get; set; }

        public virtual tblBooks intBook { get; set; }
        public virtual tblPersons intContact { get; set; }
    }
}
