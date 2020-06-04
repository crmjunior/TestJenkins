using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.DTO
{
    public class ValidaLoginDTO
    {
        public PerfilDTO Perfil { get; set; }
        public ValidacaoLogin Validacao { get; set; }
        public VersaoDTO Versao { get; set; }
        public string Mensagem { get; set; }
        public int IdOrdemVenda { get; set; }

        public static ValidaLoginDTO AlunoInexistente
        {
            get
            {
                return new ValidaLoginDTO
                {
                    Validacao = ValidacaoLogin.ErroAviso,
                    Mensagem = Constants.Messages.Acesso.AlunoInexistente.GetDescription()
                };
            }
        }

        public static ValidaLoginDTO AlunoInvalido
        {
            get
            {
                return new ValidaLoginDTO
                {
                    Validacao = ValidacaoLogin.ErroMensagem,
                    Mensagem = Constants.AVISO_ALUNO_INVALIDO
                };
            }
        }

        public static ValidaLoginDTO SenhaInvalida
        {
            get
            {
                return new ValidaLoginDTO
                {
                    Validacao = ValidacaoLogin.ErroAviso,
                    Mensagem = Constants.Messages.Acesso.SenhaIncorreta.GetDescription()
                };
            }
        }
    }

    public enum ValidacaoLogin
    {
        Sucesso = 1,
        InadimplenteTermos = 2,
        SucessoAviso = 3,
        ErroMensagem = 4,
        ErroAviso = 5
    }
}