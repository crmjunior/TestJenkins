using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblInscricoesBloqueiosTipos
    {
        public tblInscricoesBloqueiosTipos()
        {
            tblInscricoesBloqueios = new HashSet<tblInscricoesBloqueios>();
        }

        public int intTypeID { get; set; }
        public string txtDescription { get; set; }
        public string txtDetails { get; set; }
        public string txtMessageMGE { get; set; }

        public virtual ICollection<tblInscricoesBloqueios> tblInscricoesBloqueios { get; set; }
    }
}
