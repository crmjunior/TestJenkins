using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProspects
    {
        public int intProspectID { get; set; }
        public string txtName { get; set; }
        public int intSex { get; set; }
        public string txtZipCode { get; set; }
        public string txtAddress { get; set; }
        public string txtAddressComplement { get; set; }
        public string txtNeighbourhood { get; set; }
        public string txtCity { get; set; }
        public int intStateId { get; set; }
        public string txtCel { get; set; }
        public string txtEmail { get; set; }
        public string txtInstitution { get; set; }
        public DateTime dteCadastro { get; set; }
        public string txtNumero { get; set; }
        public string txtEnderecoReferencia { get; set; }
    }
}
