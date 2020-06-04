using System;

namespace MedCore_DataAccess.Entidades
{
    public partial class csp_loadMesesCursados_Result
    {
        public Nullable<int> intYear { get; set; }
        public Nullable<int> intProductGroup { get; set; }
        public Nullable<int> intMonth { get; set; }
        public string txtType { get; set; }
    }
}