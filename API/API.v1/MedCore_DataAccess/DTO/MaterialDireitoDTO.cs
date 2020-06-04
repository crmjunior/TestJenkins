using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class MaterialDireitoDTO
    {
       

        public long? Id { get; set; }

        public string Entidade { get; set; }

        public int? IntSemana { get; set; }

        public int? IntYear { get; set; }

        public string DataInicio { get; set; }

        public string DataFim { get; set; }

        public int? IntLessonTitleId { get; set; }

        public bool Ativa { get; set; }

        public long? intBookEntityId { get; set; }

        public List<int?> ApostilasAprovadas { get; set; }

        public List<int?> QuestoesAprovadas { get; set; }

        public int? MaterialId { get; set; }

    }
}