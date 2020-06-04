using System;

namespace MedCore_DataAccess.DTO
{
    public class CartaoRespostaObjetivaDTO
    {
        public int intID { get; set; }
        public int intQuestaoID {get; set;}
        public int intHistoricoExercicioID {get; set;}
        public string txtLetraAlternativa {get; set;}
        public Guid guidQuestao {get; set;}
        public int intExercicioTipoId {get; set;}
        public Nullable<DateTime> dteCadastro { get; set; }

        public override bool Equals(object obj)
        {
            var cartao = obj as CartaoRespostaObjetivaDTO;
            return intID == cartao.intID
                && intQuestaoID == cartao.intQuestaoID
                && intHistoricoExercicioID == cartao.intHistoricoExercicioID
                && txtLetraAlternativa == cartao.txtLetraAlternativa
                && guidQuestao == cartao.guidQuestao
                && intExercicioTipoId == cartao.intExercicioTipoId
                && dteCadastro == cartao.dteCadastro;
        }

        public override int GetHashCode()
        {
            return intID.GetHashCode() * 17 + intQuestaoID.GetHashCode();
        }
    }
}