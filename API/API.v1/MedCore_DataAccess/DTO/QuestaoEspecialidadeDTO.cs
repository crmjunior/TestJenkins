using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class QuestaoEspecialidadeDTO
    {
        public int? QuestaoId { get; set; }

        public int EspecialidadeId { get; set; }

        public string EspecialidadeSigla { get; set; }

        public string EspecialidadeNome { get; set; }

        public int? ClassificacaoParentId { get; set; }

        public List<Entidades.MongoDbCollections.Especialidade> Especialidades { get; set; }
    }
}