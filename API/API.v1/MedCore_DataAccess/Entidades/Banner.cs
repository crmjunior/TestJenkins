using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    //TODO: Aluno vira AlunoInscricao e Herda de Cliente
    [DataContract(Name = "Banner", Namespace = "a")]
    public class Banner
    {
        [DataMember(Name = "intBannerId", EmitDefaultValue = false)]
        public int ID { get; set; }
        [DataMember(Name = "intObjectId", EmitDefaultValue = false)]
        public int ObjectId { get; set; }
        [DataMember(Name = "txtDescricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }
        [DataMember(Name = "txtImagem", EmitDefaultValue = false)]
        public string Imagem { get; set; }
        [DataMember(Name = "txtLink", EmitDefaultValue = false)]
        public string Link { get; set; }
        [DataMember(Name = "bitLinkExterno", EmitDefaultValue = false)]
        public bool IsLinkExterno { get; set; }
        [DataMember(Name = "txtClickAqui", EmitDefaultValue = false)]
        public string ClickAqui { get; set; }
        [DataMember(Name = "IdSimulado", EmitDefaultValue = false)]
        public int? IdSimulado { get; set; }
    }
}