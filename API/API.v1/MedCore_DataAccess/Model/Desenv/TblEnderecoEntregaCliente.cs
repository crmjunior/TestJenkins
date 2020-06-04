using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblEnderecoEntregaCliente
    {
        public int IdEnderecoEntrega { get; set; }
        public int IdCliente { get; set; }
        public string txtEndereco { get; set; }
        public string txtComplementoEndereco { get; set; }
        public string txtBairro { get; set; }
        public string txtZipCode { get; set; }
        public int? intCityID { get; set; }
        public int? intStateID { get; set; }

        public virtual tblPersons IdClienteNavigation { get; set; }
        public virtual tblCities intCity { get; set; }
    }
}
