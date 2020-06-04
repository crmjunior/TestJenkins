using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
      [DataContract(Name = "ParametrosAvaliacaoAula", Namespace = "")]
    public class ParametrosAvaliacaoAula
    {
        [DataMember(Name = "TurmaId")]
        public int TurmaId { get; set; }

        [DataMember(Name = "DataString")]
        public string DataString { get; set; }

        public DateTime Data { get; set; }

    }
}