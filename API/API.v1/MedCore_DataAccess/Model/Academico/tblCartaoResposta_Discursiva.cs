using System;
using System.Collections.Generic;

namespace MedCore_API.Academico
{
    public partial class tblCartaoResposta_Discursiva
    {
        public int intID { get; set; }
        public int intQuestaoDiscursivaID { get; set; }
        public int intHistoricoExercicioID { get; set; }
        public string txtResposta { get; set; }
        public int intExercicioTipoId { get; set; }
        public int intDicursivaId { get; set; }
        public DateTime? dteCadastro { get; set; }
        public double? dblNota { get; set; }

        public virtual tblExercicio_Historico intHistoricoExercicio { get; set; }
    }
}
