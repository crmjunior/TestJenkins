using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "MensagemRecurso", Namespace = "a")]
    public class MensagemRecurso
    {
        [DataMember(Name = "Texto", EmitDefaultValue = false)]
        public string Texto { get; set; }

        public enum TipoMensagemLogin
        {
            SenhaIncorreta = 1,
            SenhaInexistente = 2,
            SenhaRecadastro = 3
        }
    }
}