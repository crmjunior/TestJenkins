using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProspectsAdaptamed
    {
        public int intProspectID { get; set; }
        public string txtNome { get; set; }
        public string txtEmail { get; set; }
        public string txtTelefone { get; set; }
        public string txtEndereco { get; set; }
        public string txtEnderecoComplemento { get; set; }
        public string txtCep { get; set; }
        public string txtBairro { get; set; }
        public string txtCidade { get; set; }
        public int intPaisID { get; set; }
        public string txtFaculdade { get; set; }
        public DateTime dteCadastro { get; set; }
        public int intAnoFormatura { get; set; }

        public virtual tblCountries intPais { get; set; }
    }
}
