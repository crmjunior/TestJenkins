using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Template", Namespace = "a")]
    public class Template
    {
        [DataMember(Name = "PaymentTemplateID", EmitDefaultValue = false)]
        public int? PaymentTemplateID { get; set; }

        [DataMember(Name = "DescricaoComplementar", EmitDefaultValue = false)]
        public string DescricaoComplementar { get; set; }

        [DataMember(Name = "OrdemVendaID", EmitDefaultValue = false)]
        public int OrdemVendaID { get; set; }

        [DataMember(Name = "Ano", EmitDefaultValue = false)]
        public int? Ano { get; set; }

        [DataMember(Name = "TurmaID", EmitDefaultValue = false)]
        public int TurmaID { get; set; }

        [DataMember(Name = "TemplateId", EmitDefaultValue = false)]
        public int TemplateId { get; set; }

        [DataMember(Name = "TipoCondicaoPagamento", EmitDefaultValue = false)]
        public int TipoCondicaoPagamento { get; set; }

    }
}