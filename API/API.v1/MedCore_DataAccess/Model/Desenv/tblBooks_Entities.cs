using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBooks_Entities
    {
        public tblBooks_Entities()
        {
            tblBooks = new HashSet<tblBooks>();
        }

        public long intID { get; set; }
        public string txtName { get; set; }
        public string txtDescription { get; set; }
        public int? intCursoGrandeAreaId { get; set; }

        public virtual ICollection<tblBooks> tblBooks { get; set; }
    }
}
