using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblInscricao_EadCadastro
    {
        public int intEadEmailId { get; set; }
        public string txtEmail { get; set; }
        public string guidSession { get; set; }
        public DateTime dteCadastro { get; set; }
        public int? intOrderID { get; set; }
    }
}
