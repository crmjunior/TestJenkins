using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class FiltroAluno
    {
        [DataMember(Name = "Id")]
        public Int32 Id { get; set; }

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public Int32 Matricula { get; set; }

        [DataMember(Name = "DataCriacao")]
        public double DataCriacao { get; set; }

        public DateTime Criacao { get; set; }
        
        [DataMember(Name = "Especialidades")]
        public string Especialidades { get; set; }

        [DataMember(Name = "Concursos")]
        public string Concursos { get; set; }

        [DataMember(Name = "Anos")]
        public string Anos { get; set; }

        [DataMember(Name = "FiltrosEspeciais")]
        public string FiltrosEspeciais { get; set; }

        [DataMember(Name = "JsonFiltro")]
        public string JsonFiltro { get; set; }

        [DataMember(Name = "PalavraChave")]
        public string PalavraChave { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "ProvasAluno")]
        public ProvasAluno ProvasAluno { get; set; }

        [DataMember(Name = "QuantidadeQuestoesAssociadas")]
        public int QuantidadeQuestoesAssociadas { get; set; }

        [DataMember(Name = "QuantidadeQuestoesNaoAssociadas")]
        public int QuantidadeQuestoesNaoAssociadas { get; set; }

        [DataMember(Name = "QuantidadeQuestoes")]
        public int QuantidadeQuestoes { get; set; }

        [DataMember(Name = "QuantidadeQuestoesOrNull")]
        public int? QuantidadeQuestoesOrNull { get; set; }
    }

    public class FiltrosAluno : List<FiltroAluno>
    {
    }
}