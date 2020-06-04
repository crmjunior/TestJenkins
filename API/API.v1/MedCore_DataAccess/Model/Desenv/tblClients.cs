using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblClients
    {
        public tblClients()
        {
            tblDuvidasAcademicas_Lidas = new HashSet<tblDuvidasAcademicas_Lidas>();
            tblSellOrders = new HashSet<tblSellOrders>();
        }

        public int intClientID { get; set; }
        public string txtSubscriptionCode { get; set; }
        public int? intAccountID { get; set; }
        public int? intClientStatusID { get; set; }
        public int? intEspecialidadeID { get; set; }
        public int? intExpectedGraduationTermID { get; set; }
        public int? intSchoolID { get; set; }
        public string txtArea { get; set; }

        public virtual tblPersons intClient { get; set; }
        public virtual tblEspecialidades intEspecialidade { get; set; }
        public virtual tblExpectedGraduationTermCatalog intExpectedGraduationTerm { get; set; }
        public virtual tblSchools intSchool { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_Lidas> tblDuvidasAcademicas_Lidas { get; set; }
        public virtual ICollection<tblSellOrders> tblSellOrders { get; set; }
    }
}
