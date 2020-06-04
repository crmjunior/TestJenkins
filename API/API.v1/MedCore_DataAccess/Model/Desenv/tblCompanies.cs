using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblCompanies
    {
        public tblCompanies()
        {
            tblBankAccounts = new HashSet<tblBankAccounts>();
            tblClassRooms = new HashSet<tblClassRooms>();
            tblEmployees = new HashSet<tblEmployees>();
            tblMaterials = new HashSet<tblMaterials>();
            tblRequisicoes_AtivointCompany = new HashSet<tblRequisicoes_Ativo>();
            tblRequisicoes_AtivointCompanyIdOriginalNavigation = new HashSet<tblRequisicoes_Ativo>();
            tblRequisicoes_RequisicaointCompany = new HashSet<tblRequisicoes_Requisicao>();
            tblRequisicoes_RequisicaointCompanyIdOriginalNavigation = new HashSet<tblRequisicoes_Requisicao>();
        }

        public int intCompanyID { get; set; }
        public string txtRegistrationName { get; set; }
        public string txtFantasyName { get; set; }
        public string txtRegisterCode { get; set; }
        public bool? bitVendor { get; set; }
        public bool? bitBranch { get; set; }
        public int? intHoldingCompanyID { get; set; }
        public bool? bitStore { get; set; }
        public int? intAddressType { get; set; }
        public string txtAddress1 { get; set; }
        public string txtAddress2 { get; set; }
        public string txtZipCode { get; set; }
        public string txtNeighbourhood { get; set; }
        public int? intCityID { get; set; }
        public string txtContract { get; set; }
        public string txtDirectory { get; set; }
        public string txtLicencaCobreBem { get; set; }
        public string txtCodigoCustodiaCheques { get; set; }
        public bool? bitWebta { get; set; }
        public string txtSenha { get; set; }
        public string txtNomeChave { get; set; }
        public string txtCertificado { get; set; }
        public int? intAmbienteBradesco { get; set; }
        public string txtCNPJ { get; set; }

        public virtual tblAddressTypes intAddressTypeNavigation { get; set; }
        public virtual tblCities intCity { get; set; }
        public virtual ICollection<tblBankAccounts> tblBankAccounts { get; set; }
        public virtual ICollection<tblClassRooms> tblClassRooms { get; set; }
        public virtual ICollection<tblEmployees> tblEmployees { get; set; }
        public virtual ICollection<tblMaterials> tblMaterials { get; set; }
        public virtual ICollection<tblRequisicoes_Ativo> tblRequisicoes_AtivointCompany { get; set; }
        public virtual ICollection<tblRequisicoes_Ativo> tblRequisicoes_AtivointCompanyIdOriginalNavigation { get; set; }
        public virtual ICollection<tblRequisicoes_Requisicao> tblRequisicoes_RequisicaointCompany { get; set; }
        public virtual ICollection<tblRequisicoes_Requisicao> tblRequisicoes_RequisicaointCompanyIdOriginalNavigation { get; set; }
    }
}
