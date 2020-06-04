using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestaoAnotacao", Namespace = "a")]
    public class QuestaoAnotacao
    {
        [DataMember(Name = "Favorita", EmitDefaultValue = false)]
        public Boolean Favorita {get; set;}

        [DataMember(Name = "Duvida", EmitDefaultValue = false)]
        public Boolean Duvida {get; set;}

        [DataMember(Name = "DataHora", EmitDefaultValue = false)]
        public string DataHora {get; set;}

        [DataMember(Name = "Anotacao", EmitDefaultValue = false)]
        public String Anotacao { get; set; }

        public QuestaoAnotacao() { 
        
        }
    }
}