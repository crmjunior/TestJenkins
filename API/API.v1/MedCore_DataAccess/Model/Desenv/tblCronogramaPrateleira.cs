using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCronogramaPrateleira
    {
        public tblCronogramaPrateleira()
        {
            tblCronogramaPrateleira_LessonTitles = new HashSet<tblCronogramaPrateleira_LessonTitles>();
        }

        public int intID { get; set; }
        public int intProductGroup1 { get; set; }
        public string txtDescricao { get; set; }
        public int intOrdem { get; set; }
        public int intMenuId { get; set; }
        public int intEmployeeID { get; set; }
        public DateTime dteDataInclusao { get; set; }
        public bool bitExibeEspecialidade { get; set; }
        public bool bitExibeConformeCronograma { get; set; }
        public int? intTipoLayout { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual tblAccess_Object intMenu { get; set; }
        public virtual ICollection<tblCronogramaPrateleira_LessonTitles> tblCronogramaPrateleira_LessonTitles { get; set; }
    }
}
