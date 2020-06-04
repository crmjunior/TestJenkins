using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Concurso", Namespace = "a")]
    public class Concurso
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "Sigla", EmitDefaultValue = false)]
        public string Sigla { get; set; }

        [DataMember(Name = "UF", EmitDefaultValue = false)]
        public string UF { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "Ano", EmitDefaultValue = false)]
        public int Ano { get; set; }

        [DataMember(Name = "Data", EmitDefaultValue = false)]
        public DateTime Data { get; set; }

        [DataMember(Name = "BitDiscursiva", EmitDefaultValue = false)]
        public bool BitDiscursiva { get; set; }

        [DataMember(Name = "QtdQuestoes", EmitDefaultValue = false)]
        public int QtdQuestoes { get; set; }

        [DataMember(Name = "QtdQuestoesDiscursivas", EmitDefaultValue = false)]
        public int QtdQuestoesDiscursivas { get; set; }

        [DataMember(Name = "Duracao", EmitDefaultValue = false)]
        public int Duracao { get; set; }

        [DataMember(Name = "CartoesResposta")]
        public CartoesResposta CartoesResposta { get; set; }

        [DataMember(Name = "TipoProva")]
        public string TipoProva { get; set; }

        [DataMember(Name = "PermissaoProva")]
        public PermissaoProva PermissaoProva { get; set; }

        [DataMember(Name = "IdExercicio", EmitDefaultValue = false)]
        public int IdExercicio { get; set; }
    }

    public enum BloqueioConcursoArea 
    { 
      PortalProfessor = 1,
      AreaRestrita = 2
    }
}