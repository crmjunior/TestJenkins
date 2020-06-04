using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Estado", Namespace = "a")]
    public class Estado
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "Sigla")]
        public string Sigla { get; set; }

        [DataMember(Name = "IdRegiao")]
        public int IdRegiao { get; set; }

        [DataMember(Name = "Regiao")]
        public string Regiao { get; set; }

        [DataMember(Name = "IdPais")]
        public int IdPais { get; set; }
    }
}