using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblInscricao_MedCadastro
    {
        public int intMedEmailId { get; set; }
        public string txtEmail { get; set; }
        public string guidSession { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
