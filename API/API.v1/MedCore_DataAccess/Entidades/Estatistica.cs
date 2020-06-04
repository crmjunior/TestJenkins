using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Estatistica", Namespace = "a")]
    public class Estatistica
    {
        [DataMember(Name = "Letra")]
        public String Letra { get; set; }

        [DataMember(Name = "Valor")]
        public Double Valor { get; set; }

        [DataMember(Name = "Correta")]
        public Boolean Correta { get; set; }
    }
}