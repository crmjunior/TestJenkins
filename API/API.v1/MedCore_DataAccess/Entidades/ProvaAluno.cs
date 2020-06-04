using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ProvaAluno", Namespace = "a")]
    public class ProvaAluno : Prova
    {
        [DataMember(Name = "DataCriacao")]
        public double DataCriacao { get; set; }
        
        public DateTime Criacao { get; set; }

        [DataMember(Name = "QuantidadeQuestoes")]
        public Int32 QuantidadeQuestoes { get; set; }

        [DataMember(Name = "Acertos")]
        public Int32 Acertos { get; set; }

        [DataMember(Name = "Erros")]
        public Int32 Erros { get; set; }

        [DataMember(Name = "NaoRealizadas")]
        public Int32 NaoRealizadas { get; set; }

    }

    [CollectionDataContract(Name = "ProvasAluno", Namespace = "a")]
    public class ProvasAluno : List<ProvaAluno>
    {

    }
}