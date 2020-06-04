using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRPA
    {
        public tblRPA()
        {
            tblRPA_Ativo_Historico = new HashSet<tblRPA_Ativo_Historico>();
            tblRPA_DadosBancarios = new HashSet<tblRPA_DadosBancarios>();
        }

        public int intID { get; set; }
        public string txtName { get; set; }
        public string txtLogin { get; set; }
        public int intWarehouseID { get; set; }
        public int intCargoID { get; set; }
        public string txtCelular { get; set; }
        public string txtResidencial { get; set; }
        public string txtEndereco { get; set; }
        public string txtComplemento { get; set; }
        public string txtBairro { get; set; }
        public string txtCEP { get; set; }
        public string txtEmail { get; set; }
        public string txtRG { get; set; }
        public string txtCPF { get; set; }
        public string txtCTPS { get; set; }
        public string txtINSS { get; set; }
        public string txtDadosBancarios { get; set; }
        public DateTime? dteInicio { get; set; }
        public DateTime? dteDesligamento { get; set; }
        public int? intStatus { get; set; }
        public string txtObservacoes { get; set; }
        public bool? bitConferencia { get; set; }
        public string txtSenha { get; set; }
        public int? intCityID { get; set; }
        public bool bitAtivo { get; set; }
        public DateTime? dteDataCriacao { get; set; }
        public int? intUsuarioInclusao { get; set; }
        public int? intQtdDependentes { get; set; }
        public string txtSerieCTPS { get; set; }
        public DateTime? dteDataCTPS { get; set; }
        public DateTime? dteDataCadastramentoPis { get; set; }
        public DateTime? dteDataEmissaoRG { get; set; }
        public DateTime? dteDataNascimento { get; set; }
        public int? intEstadoCivil { get; set; }
        public string txtFuncao { get; set; }
        public string txtGrauInstrucao { get; set; }
        public string txtNacionalidade { get; set; }
        public string txtRaca { get; set; }
        public string txtSexo { get; set; }
        public string txtTituloEleitor { get; set; }
        public string txtSecaoTituloEleitor { get; set; }
        public string txtZonaTituloEleitor { get; set; }
        public string txtCertificadoReservista { get; set; }

        public virtual tblRPA_Cargos intCargo { get; set; }
        public virtual tblCities intCity { get; set; }
        public virtual tblRPA_Status intStatusNavigation { get; set; }
        public virtual ICollection<tblRPA_Ativo_Historico> tblRPA_Ativo_Historico { get; set; }
        public virtual ICollection<tblRPA_DadosBancarios> tblRPA_DadosBancarios { get; set; }
    }
}
