namespace MedCore_DataAccess.DTO
{
    public class RecuperaSenhaDTO
    {
        public ValidaRecuperaSenha Validacao { get; set; }
    }

    public enum ValidaRecuperaSenha
    {
        EmailEnviado = 1,
        Inexistente = 2
    }
}