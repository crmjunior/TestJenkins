using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MedCoreAPI.ViewModel.Base
{
    public enum TipoErroAcesso
    {
        [Description("Versão Desatualizada.")]
        VersaoAppDesatualizada,
        [Description("Cadastro inexistente.")]
        CadastroInexistente,
        [Description("Não há produtos contratados.")]
        SemProdutosContratados,
        [Description("Você ainda não tem uma senha cadastrada.")]
        SemSenhaCadastrada,
        [Description("Senha incorreta.")]
        SenhaIncorreta,
        [Description("Dispositivo bloqueado.")]
        DeviceBloqueado,
        [Description("Acesso bloqueado.")]
        BloqueadoInadimplencia,
        [Description("Serviço Indisponível.")]
        Erro500,
        [Description("")]
        ProximoAnoCanceladoOuInadimplente,
        [Description("")]
        LeMensagemInadimplencia

    }
}
