using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Regra", Namespace = "a")]
    public class Regra
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "DataCriacao")]
        public DateTime DataCriacao { get; set; }

        [DataMember(Name = "DataUltimaAlteracao")]
        public DateTime DataUltimaAlteracao { get; set; }

        [DataMember(Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [DataMember(Name = "Ativo")]
        public bool Ativo { get; set; }

        [DataMember(Name = "RegraDetalhes")]
        public List<RegraCondicao> RegraDetalhes { get; set; }
 
    }
}