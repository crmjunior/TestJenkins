using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblIntensivao_Cronograma
    {
        public int codIntensivao { get; set; }
        public string dia_semana { get; set; }
        public string horario { get; set; }
        public string periodo { get; set; }
        public string formato { get; set; }
        public string localprova { get; set; }
        public string endereco { get; set; }
        public string intLocal { get; set; }
        public string txtCidade { get; set; }
        public int intStoreID { get; set; }
        public string txtObs { get; set; }
        public bool? bitAtivo { get; set; }
    }
}
