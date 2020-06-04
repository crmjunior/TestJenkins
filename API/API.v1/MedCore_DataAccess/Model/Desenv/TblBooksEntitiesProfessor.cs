using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBooksEntitiesProfessor
    {
        public int intBooksEntitiesProfessor { get; set; }
        public int? intContactId { get; set; }
        public long? intBookEntitiesId { get; set; }
        public bool? bitControlado { get; set; }

        public virtual tblPersons intContact { get; set; }
    }
}
