using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "RegraAcesso", Namespace = "a")]
    public class RegraAcesso
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Alias")]
        public string Alias { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "Categoria")]
        public string Categoria { get; set; }
    }
}