using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCountries
    {
        public tblCountries()
        {
            tblProspectsAdaptamed = new HashSet<tblProspectsAdaptamed>();
            tblStates = new HashSet<tblStates>();
        }

        public int intCountryID { get; set; }
        public string txtDescription { get; set; }
        public int? intAreaCode { get; set; }
        public string txtCodigoBacen { get; set; }

        public virtual ICollection<tblProspectsAdaptamed> tblProspectsAdaptamed { get; set; }
        public virtual ICollection<tblStates> tblStates { get; set; }
    }
}
