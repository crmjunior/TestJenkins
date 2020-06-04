using System;

namespace MedCore_DataAccess.DTO
{
    public class CartaoRespostaDiscursivaDTO
    {
        public int intID { get; set; }
        public int intQuestaoDiscursivaID { get; set; }
        public int intHistoricoExercicioID { get; set; }
        public string txtResposta { get; set; }
        public int intExercicioTipoId { get; set; }
        public int intDicursivaId { get; set; }
        public Nullable<DateTime> dteCadastro { get; set; }
        public Nullable<double> dblNota { get; set; }
    }
}