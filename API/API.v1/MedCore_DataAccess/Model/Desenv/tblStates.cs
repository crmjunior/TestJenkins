using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblStates
    {
        public tblStates()
        {
            tblCities = new HashSet<tblCities>();
        }

        public int intStateID { get; set; }
        public string txtDescription { get; set; }
        public int intCountryID { get; set; }
        public string txtCaption { get; set; }
        public int? intRegionID { get; set; }
        public string txtCodigoIBGE { get; set; }

        public virtual tblCountries intCountry { get; set; }
        public virtual tblRegions intRegion { get; set; }
        public virtual ICollection<tblCities> tblCities { get; set; }
    }
}
