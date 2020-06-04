using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Namespace = "a")]
    public class Simulador
    {
        [DataMember(Name = "ItensSimuladores")]
        public List<ItemSimulador> ItensSimuladores { get; set; }

        [DataMember(Name = "DiaSemana")]
        public DayOfWeek DiaSemana { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "IDLocal")]
        public int IDLocal { get; set; }

        [DataMember(Name = "Endereco")]
        public string Endereco { get; set; }

        [DataMember(Name = "Obs")]
        public string Obs { get; set; }

        [DataMember(Name = "FormatoModulo")]
        public string FormatoModulo { get; set; }

        [DataMember(Name = "DataInicio")]
        public DateTime DataInicio { get; set; }

        [DataMember(Name = "DataFim")]
        public DateTime DataFim { get; set; }

        [DataMember(Name = "Quorum")]
        public bool Quorum { get; set; }

        [DataMember(Name = "DiaRevisao")]
        public string DiaRevisao { get; set; }

        [DataMember(Name = "LocalRevisao")]
        public string LocalRevisao { get; set; }


    }
}