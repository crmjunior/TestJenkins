using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class QuestaoAvaliacaoComentarioOpcoes
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int ID { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }
    }
}