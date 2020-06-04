using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "PermissaoRegra", Namespace = "a")]
    public class PermissaoRegra
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Regra")]
        public Regra Regra { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "ObjetoId", EmitDefaultValue = false)]
        public int ObjetoId { get; set; }

        [DataMember(Name = "AcessoId", EmitDefaultValue = false)]
        public int AcessoId { get; set; }

        [DataMember(Name = "Ordem", EmitDefaultValue = false)]
        public int Ordem { get; set; }

        [DataMember(Name = "DataLimite", EmitDefaultValue = false)]
        public bool IsDataLimite { get; set; }

        [DataMember(Name = "DataUltimaAlteracao", EmitDefaultValue = false)]
        public DateTime DataUltimaAlteracao { get; set; }

        [DataMember(Name = "EmployeeId", EmitDefaultValue = false)]
        public int EmployeeId { get; set; }

        [DataMember(Name = "MensagemId", EmitDefaultValue = false)]
        public int? MensagemId { get; set; }

        [DataMember(Name = "DescricaoMensagem", EmitDefaultValue = false)]
        public string DescricaoMensagem { get; set; }

        [DataMember(Name = "InterruptorId", EmitDefaultValue = false)]
        public int InterruptorId { get; set; }

        [DataMember(Name = "Ativo")]
        public bool Ativo { get; set; }
    }

    /// <summary>
    /// Enum referente a tabela tblAccess_Permission_Status
    /// </summary>
    public enum ETipoPermissaoRegra
    {
        SemAcesso = 1,
        SomenteLeitura = 2,
        AcessoPermitido = 3
    }
}