using System;
using System.Collections.Generic;
using MedCore_DataAccess.Repository.MongoRepository;
using MongoDB.Bson.Serialization.Attributes;

namespace MedCore_DataAccess.Entidades.MongoDbCollections
{
    [CollectionName("Questao")]
    public class Questao : Entity
    {
        public Questao()
        {
            Especialidades = new List<Especialidade>();
        }

        public Int32? QuestaoId { get; set; }

        public Int32? ConcursoId { get; set; }

        public Int32? ConcursoEntidadeId { get; set; }

        public string ConcursoSigla { get; set; }

        public string ConcursoNome { get; set; }

        public string Enunciado { get; set; }

        public Int32? QuestaoAno { get; set; }

        public Int32? ConcursoAno { get; set; }

        [BsonIgnore]
        public Int32? EspecialidadeId { get; set; }
        [BsonIgnore]
        public string EspecialidadeSigla { get; set; }
        [BsonIgnore]
        public string EspecialidadeNome { get; set; }

        public List<Especialidade> Especialidades { get; set; }

        public int OrigemQuestao { get; set; }

        public string SimuladoNome { get; set; }

        public string SimuladoSigla { get; set; }

        public string Comentario { get; set; }

        public string Recurso { get; set; }

        public bool Multidisciplinar { get; set; }

        public int ProvaId { get; set; }
 
        public bool? Impressa { get; set; }

        public List<Alternativa> Alternativas { get; set; }

        public int SimuladoId { get; set; }

        public ProvaTipo TipoProva { get; set; }
    }

    public class Alternativa
    {
        public Int32? AlternativaId { get; set; }
        public string Descricao { get; set; }
    }

    [CollectionName("Especialidade")]
    public class Especialidade : Entity
    {
        public Int32? EspecialidadeId { get; set; }

        public string EspecialidadeSigla { get; set; }

        public string EspecialidadeNome { get; set; }

        public Int32? EspecialidadeParentId { get; set; }

        public Int32 Ordem { get; set; }
    }

    [CollectionName("HistoricoQuestaoErradaAluno")]
    public class HistoricoQuestaoErradaAluno : Entity
    {
        public int Matricula { get; set; }

        public Int32? QuestaoId { get; set; }

        public int OrigemQuestao { get; set; }

         
    }

    [CollectionName("ExercicioPermissaoAluno")]
    public class ExercicioPermissaoAluno : Entity
    {
        public int Matricula { get; set; }

        public int ExercicioId { get; set; }

        public int TipoExercicioId { get; set; }
    }

    [CollectionName("ProvaTipo")]
    public class ProvaTipo : Entity
    {
        public int intProvaTipoID { get; set; } 
        public string txtDescription { get; set; }
    }
}