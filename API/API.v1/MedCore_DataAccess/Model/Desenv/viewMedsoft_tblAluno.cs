using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class viewMedsoft_tblAluno
    {
        public int intContactID { get; set; }
        public string txtName { get; set; }
        public string txtRegister { get; set; }
        public int? intSex { get; set; }
        public DateTime? dteBirthday { get; set; }
        public int? intAddressType { get; set; }
        public string txtAddress1 { get; set; }
        public string txtAddress2 { get; set; }
        public string txtNeighbourhood { get; set; }
        public int? intCityID { get; set; }
        public string txtZipCode { get; set; }
        public string txtPhone1 { get; set; }
        public string txtPhone2 { get; set; }
        public string txtCel { get; set; }
        public string txtFax { get; set; }
        public string txtEmail1 { get; set; }
        public string txtEmail2 { get; set; }
        public string txtEmail3 { get; set; }
        public string txtPassport { get; set; }
        public string txtIDDocument { get; set; }
        public DateTime? dteDate { get; set; }
        public byte[] imgFingerPrint { get; set; }
        public bool bitActive { get; set; }
        public string txtNickName { get; set; }
        public int intClientID { get; set; }
        public string txtSubscriptionCode { get; set; }
        public int intAccountID { get; set; }
        public int intClientStatusID { get; set; }
        public int intEspecialidadeID { get; set; }
        public int intExpectedGraduationTermID { get; set; }
        public int intSchoolID { get; set; }
        public string txtArea { get; set; }
        public string txtEspecialidade { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
