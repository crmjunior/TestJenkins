using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class ViewDireitoIntensivao_Chamados2018
    {
        public int? Cliente { get; set; }
        public string txtName { get; set; }
        public string Email { get; set; }
        public string statusAluno { get; set; }
        public int? Ordem_de_Venda { get; set; }
        public DateTime? Data_da_Ordem_de_Venda { get; set; }
        public double? SALDO_ALUNO_NO_ANO { get; set; }
        public string STATUS_DA_ORDEM_DE_VENDA_ATUAL { get; set; }
        public string Direito_Intensivão_Regra { get; set; }
        public string Direito_Intensivão_Final { get; set; }
        public string Intensivão_Cortesia_Anos_Anteriores_ { get; set; }
        public string Categoria { get; set; }
        public string Produto_Atual { get; set; }
        public string Filial { get; set; }
        public string Proutos_OVS_Anteriores { get; set; }
        public string Observação { get; set; }
        public string Já_esteve_como_sim_no_passado_ { get; set; }
        public string Possui_chamado_de_TERMO_CONFORME_ { get; set; }
        public string Possui_OV_INTENSIVÃO_2018_ { get; set; }
        public double? Valor_Pago_INTENSIVÃO_2018 { get; set; }
        public string Já_analisado_ { get; set; }
        public string Possui_venda_de_material_paga_no_ano_ { get; set; }
        public double? SALDO_ALUNO_ANOS_ANTERIORES { get; set; }
        public DateTime? Data_chamado { get; set; }
        public string Chamado_criado_por { get; set; }
        public string Acesso_ou_Retirada_no_Intensivão_2018 { get; set; }
        public string Status_do_Chamado_de_Termo { get; set; }
    }
}
