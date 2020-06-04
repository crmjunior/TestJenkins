using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
     [DataContract(Name = "ParametrosPrimeiraAula", Namespace = "")]
    public class ParametrosPrimeiraAula
    {
        [DataMember(Name = "TurmaId")]
        public int TurmaId { get; set; }

        [DataMember(Name = "DataString")]
        public string DataString { get; set; }

        public DateTime Data { get; set; }

        [DataMember(Name = "DiasAntecedencia")]
		public int DiasAntecedencia { get; set; }

    }
}