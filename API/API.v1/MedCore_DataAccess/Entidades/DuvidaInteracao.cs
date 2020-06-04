using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "DuvidaInteracao", Namespace = "a")]
    public class DuvidaInteracao
    {
        [DataMember(Name = "ID")]
        public long ID { get; set; }

        public DateTime dteDataResposta { get; set; }

        [DataMember(Name = "DataResposta")]
        public string DataResposta { get; set; }

        [DataMember(Name = "RespostaPesquisa")]
        public string RespostaPesquisa { get; set; }
    }
}