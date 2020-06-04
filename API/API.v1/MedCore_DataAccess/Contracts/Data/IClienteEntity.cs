using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IClienteEntity
    {
         Clientes GetByFilters(Cliente registro, int ano = 0, Aplicacoes aplicacao = Aplicacoes.LeitordeApostilas);

         int UserGolden(string register, Aplicacoes aplicacao = Aplicacoes.LeitordeApostilas);
         Clientes GetPreByFilters(Cliente registro, Aplicacoes aplicacao = Aplicacoes.LeitordeApostilas);
         AlunoDTO GetAlunoPorFiltros(AlunoDTO filtro);
         Pessoa.EnumTipoPessoa GetPersonType(string register);
        Cliente GetDadosBasicos(int matricula);
        Cliente GetDadosBasicos(string registro);

        string ObterSenhaGolden();
        Pessoa GetByClientLogin(string clientLogin);

        AlunoSenhaDTO GetAlunoSenha(int clientId);

        int InserirAlunoSenha(AlunoSenhaDTO alunoSenha);
        int AlterarAlunoSenha(AlunoSenhaDTO alunoSenha);    

        Cliente UpdateEsqueciSenha(Cliente cliente, Aplicacoes aplicacao = Aplicacoes.AreaRestrita, bool isAdaptaMed = false);
    }
}