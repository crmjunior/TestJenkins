using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Grupo", Namespace = "a")]
    public class Grupo
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public Int32 Id { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public String Descricao { get; set; }

        [DataMember(Name = "Duracao", EmitDefaultValue = false)]
        public Int32 Duracao { get; set; }

        [DataMember(Name = "Ordem", EmitDefaultValue = false)]
        public Int32 Ordem { get; set; }

        [DataMember(Name = "HoraInicio", EmitDefaultValue = false)]
        public string DataHoraInicio { get; set; }

        [DataMember(Name = "Questoes", EmitDefaultValue = false)]
        public List<Questao> Questoes { get; set; }

        [DataMember(Name = "GrupoDatas", EmitDefaultValue = false)]
        public List<GrupoData> GrupoDatas { get; set; }

        public Grupo()
        {
            Questoes = new List<Questao>();
            GrupoDatas = new List<GrupoData>();
        }
    }
}