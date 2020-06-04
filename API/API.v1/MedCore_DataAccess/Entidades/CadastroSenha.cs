using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "CadastroSenha", Namespace = "a")]
    public class CadastroSenha
    {
        [DataMember(Name = "register")]
        public string register { get; set; }

        [DataMember(Name = "senha")]
        public string senha { get; set; }

        [DataMember(Name = "idAplicacao")]
        public int  idAplicacao { get; set; }

        [DataMember(Name = "senhaAnterior")]
        public string senhaAnterior { get; set; }
    }
}