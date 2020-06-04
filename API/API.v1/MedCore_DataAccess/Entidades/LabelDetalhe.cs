using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "LabelDetalhe", Namespace = "a")]
    public class LabelDetalhe
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "IdLabel", EmitDefaultValue = false)]
        public int IdLabel { get; set; }

        [DataMember(Name = "IdFuncionario", EmitDefaultValue = false)]
        public int IdFuncionario { get; set; }

        [DataMember(Name = "IdItemMarcado", EmitDefaultValue = false)]
        public int IdItemMarcado { get; set; }

        [DataMember(Name = "Padrao", EmitDefaultValue = false)]
        public bool? Padrao { get; set; }

        [DataMember(Name = "Ativo", EmitDefaultValue = false)]
        public bool Ativo { get; set; }
    }
}