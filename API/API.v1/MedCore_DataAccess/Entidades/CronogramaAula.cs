using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class CronogramaAula
    {

        public CronogramaAula()
        {
            tema = new List<TemaAula>();
        }

        [DataMember(Name = "NomeTurma", EmitDefaultValue = false)]
        public string NomeTurma { get; set; }

        [DataMember(Name = "IdTurma", EmitDefaultValue = false)]
        public int IdTurma { get; set; }

        [DataMember(Name = "Dia", EmitDefaultValue = false)]
        public int Dia { get; set; }

        [DataMember(Name = "Mes", EmitDefaultValue = false)]
        public int Mes { get; set; }

        [DataMember(Name = "tema", EmitDefaultValue = false)]
        public List<TemaAula> tema { get; set; }

        [DataMember(Name = "IdProduto", EmitDefaultValue = false)]
        public int IdProduto { get; set; }

        [DataMember(Name = "NomeProduto", EmitDefaultValue = false)]
        public string NomeProduto { get; set; }

        [DataMember(Name = "Tempo", EmitDefaultValue = false)]
        public string Tempo { get; set; }
    }
}