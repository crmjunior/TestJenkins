using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class RespostaConcursoQuestoesDTO
    {
        public int intQuestaoID { get; set; }

        public string txtLetraAlternativa { get; set; }

        public bool ?bitCorreta { get; set; }

        public bool ?bitCorretaPreliminar { get; set; }

        public bool bitAnulada { get; set; }

        public List<RespostaConcursoQuestoesDTO> QuestoesObjecto { get; set; }
    }
}