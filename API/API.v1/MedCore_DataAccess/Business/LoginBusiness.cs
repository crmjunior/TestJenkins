using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Business
{
    public class LoginBusiness
    {
        private readonly IFuncionarioData _funcionarioRepository;
        private readonly IClienteEntity _clienteRepository;
        private readonly IAlunoEntity _alunoRepository;
        private readonly IVersaoAppPermissaoEntityData _versaoAppPermissaoRepository;
        private readonly IPerfilAlunoData _perfilRepository;
        readonly IMenuBusiness _menuBusiness;

        public LoginBusiness(IClienteEntity clienteRepository, IAlunoEntity alunoRepository,
            IFuncionarioData funcionarioRepository, IPerfilAlunoData perfilRepository, IVersaoAppPermissaoEntityData versaoAppPermissaoRepository,
            IMenuData menuRepository, IDataAccess<Pessoa> pessoaRepository, IBlackListData blackListRepository)
        {
            _alunoRepository = alunoRepository;
            _clienteRepository = clienteRepository;
            _funcionarioRepository = funcionarioRepository;
            _perfilRepository = perfilRepository;
            _versaoAppPermissaoRepository = versaoAppPermissaoRepository;
            _menuBusiness = new MenuBusiness(menuRepository, pessoaRepository, blackListRepository);
        }     

        public ValidaLoginDTO Login(string identificador, string senha, Aplicacoes aplicacao, string versaoApp)
        {
            return Login(identificador, senha, true, aplicacao, versaoApp);
        }

        private ValidaLoginDTO Login(string identificador, string senha, bool validarSenha, Aplicacoes aplicacao, string versaoApp)
        {
            senha = validarSenha ? Utilidades.EncryptionSHA1(senha) : senha;
            Task<PermissaoInadimplencia> tarefaInadimplencia = null;
            Task<List<Menu>> menusPermitidos = null;
            Task<IDictionary<Utilidades.EMenuAccessObject, bool>> tarefaPermissoesRMais = null;
            IDictionary<Utilidades.EMenuAccessObject, bool> permissoesRMais = new Dictionary<Utilidades.EMenuAccessObject, bool>();

            var tarefaVersaoLoja = Utilidades.ObterUltimaVersaoLojaAsync(aplicacao);

            Cliente pessoa = new Cliente();
            var isGolden = false;
            var alunoMedMedcurso = true;

            Cliente cliente = new Cliente { Register = identificador };
            var dadosBasicos = GetRegisterParaLogin(identificador);

            if (dadosBasicos != null)
            {
                cliente.Register = dadosBasicos.Register;
                menusPermitidos = ObterPermissoesRecursosAsync(dadosBasicos.ID, versaoApp, aplicacao);              
            }

            var tipoPessoa = _clienteRepository.GetPersonType(cliente.Register);
            var isProfessor = (tipoPessoa == Pessoa.EnumTipoPessoa.Professor || tipoPessoa == Pessoa.EnumTipoPessoa.Funcionario);
            if (tipoPessoa == Pessoa.EnumTipoPessoa.Cliente)
            {

                tarefaInadimplencia = VerificaInadimplenciaAsync(cliente.Register, aplicacao);                

                pessoa = _clienteRepository.GetByFilters(cliente, 0, aplicacao).FirstOrDefault();
                pessoa.Nome = Utilidades.GetNomeResumido(pessoa.Nome);
                pessoa.Avatar = pessoa.Avatar;
                pessoa.Estado = _alunoRepository.GetAlunoEstado(pessoa.ID);
            }
            else if (isProfessor)
            {
                var funcionario = _funcionarioRepository.GetFuncionariosRecursos(cliente.Register).FirstOrDefault();
                pessoa.ID = funcionario.ID;
                pessoa.Nome = Utilidades.GetNomeResumido(funcionario.Nome);
                pessoa.Senha = funcionario.Senha;
                pessoa.RetornoStatus = Cliente.StatusRetorno.Inexistente;

                var avatarProfessor = string.Concat(
                        Constants.URLDIRETORIOAVATARPROFESSOR, pessoa.ID, ".jpg"
                        );

                pessoa.Avatar = Utilidades.VerificaImagemExiste(avatarProfessor) ?
                    avatarProfessor : Constants.LINK_STATIC_AVATAR_PADRAO;
            }
            else if (tipoPessoa == Pessoa.EnumTipoPessoa.NaoExiste)
            {
                return ValidaLoginDTO.AlunoInexistente;
            }

            if (IsLoginSucesso(tipoPessoa, pessoa.RetornoStatus))
            {
                isGolden = _clienteRepository.UserGolden(cliente.Register, aplicacao) == 1;
            }

            if (tipoPessoa == Pessoa.EnumTipoPessoa.Cliente)
            {
                tarefaPermissoesRMais = ObterPermissoesRecursosRMaisAsync(menusPermitidos.Result);
                alunoMedMedcurso = ObterPermissoesRecursosRUm(menusPermitidos.Result);

                var result = ValidaResultadoLoginCliente(pessoa, senha, validarSenha, isGolden, tarefaInadimplencia);

                if (result != null)
                {
                    if (StatusLoginPermiteAcesso(result.Validacao))
                    {
                        var acessoRmais = tarefaPermissoesRMais.Result;
                        var acessoR1 = alunoMedMedcurso;
                        var acessoRmaisPorInteresse = GanhaPermissaoRMaisPorInteresse(pessoa.ID, acessoR1, acessoRmais);

                        if (acessoRmaisPorInteresse)
                        {
                            acessoRmais = LiberarPermissoes(tarefaPermissoesRMais.Result);
                        }

                        var permissao = ObterHashPermissao(
                            new KeyValuePair<string, object>("matricula", pessoa.ID),
                            new KeyValuePair<string, object>("RMais", acessoRmais),
                            new KeyValuePair<string, object>("RUm", acessoR1)
                            );

                        result.Versao = ObterDadosVersao(versaoApp, tarefaVersaoLoja.Result, aplicacao);
                        result.Perfil.RUm = acessoR1;
                        result.Perfil.RMais = PossuiPermissaoRecursoRMais(pessoa.ID, acessoR1, acessoRmais);
                        result.Perfil.ProdutoPrincipal = GetNomeProdutoPermissao(acessoRmais);
                        result.Perfil.Hash = Criptografia.CryptAES(permissao);
                    }
                    return result;
                }
            }

            var senhaGolden = string.Empty;
            if (isGolden)
            {
                senhaGolden = _clienteRepository.ObterSenhaGolden();
                senhaGolden = Utilidades.EncryptionSHA1(senhaGolden);
            }

            if (validarSenha && senha != pessoa.Senha && (!isGolden || senha != senhaGolden))
            {
                return ValidaLoginDTO.SenhaInvalida;
            }

            var versao = ObterDadosVersao(versaoApp, tarefaVersaoLoja.Result, aplicacao);
            
            var permissaoR1 = alunoMedMedcurso;

            if (isProfessor)
            {
                permissoesRMais = LiberarPermissoesRecursosRMaisProfessor();
                permissaoR1 = true;
            }
            else
            {
                permissoesRMais = tarefaPermissoesRMais.Result;
            }

            var permiteRMaisPorInteresse = GanhaPermissaoRMaisPorInteresse(pessoa.ID, permissaoR1, permissoesRMais);

            if(permiteRMaisPorInteresse)
            {
                permissoesRMais = LiberarPermissoes(tarefaPermissoesRMais.Result);
            }

            var hash = ObterHashPermissao(
                            new KeyValuePair<string, object>("matricula", pessoa.ID),
                            new KeyValuePair<string, object>("RMais", permissoesRMais),
                            new KeyValuePair<string, object>("RUm", permissaoR1)
                            );

            return new ValidaLoginDTO
            {
                Validacao = ValidacaoLogin.Sucesso,
                Versao = versao,
                Perfil = new PerfilDTO
                {
                    Matricula = pessoa.ID,
                    Email = pessoa.Email ?? pessoa.Email2 ?? pessoa.Email3,
                    Especialidade = pessoa.Especialidade,
                    Nome = pessoa.Nome,
                    UrlAvatar = pessoa.Avatar,
                    RUm = permissaoR1,
                    RMais = PossuiPermissaoRecursoRMais(pessoa.ID, permissaoR1, permissoesRMais),
                    ProdutoPrincipal = GetNomeProdutoPermissao(permissoesRMais),
                    Estado = pessoa.Estado,
                    Hash = Criptografia.CryptAES(hash)
                }
            };
        }

        private IDictionary<Utilidades.EMenuAccessObject, bool> LiberarPermissoes(IDictionary<Utilidades.EMenuAccessObject, bool> permissoes)
        {
            var novasPermissoes = new Dictionary<Utilidades.EMenuAccessObject, bool>();
            foreach (var p in permissoes)
            {
                novasPermissoes.Add(p.Key, true);
            }
            return novasPermissoes;
        }

        private string GetNomeProdutoPermissao(IDictionary<Utilidades.EMenuAccessObject, bool> result)
        {
            var nome = string.Empty;
            if (result.Count(p => p.Value) == 1)
            {
                var permissao = result.First(p => p.Value);
                nome = permissao.Key.GetDescription();
            }
            return nome;
        }

        private Task<PermissaoInadimplencia> VerificaInadimplenciaAsync(string register, Aplicacoes aplicacao)
        {
            return Task.Factory.StartNew(
                    () => _alunoRepository.GetPermissao(register, (int)aplicacao)
                );
        }

        private ValidaLoginDTO ValidaResultadoLoginCliente(Cliente pessoa, string senha, bool validarSenha, bool isGolden, Task<PermissaoInadimplencia> tarefaInadimplencia)
        {
            PermissaoInadimplencia permissao = null;

            if (pessoa.RetornoStatus == Cliente.StatusRetorno.Cancelado)
            {
                return ValidaLoginDTO.AlunoInvalido;
            }

            permissao = tarefaInadimplencia.Result;
            var resultadoInadimplencia = ValidaResultadoInadimplencia(permissao, pessoa);

            if (resultadoInadimplencia != null && !string.IsNullOrEmpty(pessoa.Senha))
            {
                var senhaGolden = string.Empty;
                if (isGolden)
                {
                    senhaGolden = _clienteRepository.ObterSenhaGolden();
                    senhaGolden = Utilidades.EncryptionSHA1(senhaGolden);
                }

                if (validarSenha && senha != pessoa.Senha && (!isGolden || senha != senhaGolden))
                {
                    return ValidaLoginDTO.SenhaInvalida;
                }
                return resultadoInadimplencia;
            }

            if (pessoa.RetornoStatus == Cliente.StatusRetorno.SemAcesso)
            {
                return ValidaLoginDTO.AlunoInvalido;
            }
            return null;
        }

        private bool IsLoginSucesso(Pessoa.EnumTipoPessoa tipoPessoa, Cliente.StatusRetorno statusLogin)
        {
            return (tipoPessoa == Pessoa.EnumTipoPessoa.Cliente && statusLogin == Cliente.StatusRetorno.Sucesso)
                    || (tipoPessoa == Pessoa.EnumTipoPessoa.Professor || tipoPessoa == Pessoa.EnumTipoPessoa.Funcionario);
        }

        private ValidaLoginDTO ValidaResultadoInadimplencia(PermissaoInadimplencia permissao, Cliente pessoa)
        {
            if (pessoa.RetornoStatus == Cliente.StatusRetorno.Sucesso)
            {
                if (permissao.PermiteAcesso == (int)PermissaoInadimplencia.StatusAcesso.Permitido && !string.IsNullOrEmpty(permissao.Mensagem))
                {
                    return new ValidaLoginDTO
                    {
                        Validacao = ValidacaoLogin.InadimplenteTermos,
                        IdOrdemVenda = permissao.IdOrdemDeVenda,
                        Mensagem = permissao.Mensagem,
                        Perfil = new PerfilDTO
                        {
                            Matricula = pessoa.ID,
                            Especialidade = pessoa.Especialidade,
                            Nome = pessoa.Nome,
                            UrlAvatar = pessoa.Avatar,
                            Estado = pessoa.Estado
                        }
                    };
                }
            }
            else if (pessoa.RetornoStatus == Cliente.StatusRetorno.SemAcesso)
            {
                if (permissao.PermiteAcesso == (int)PermissaoInadimplencia.StatusAcesso.Negado && !string.IsNullOrEmpty(permissao.Mensagem))
                {
                    return new ValidaLoginDTO
                    {
                        Validacao = ValidacaoLogin.ErroMensagem,
                        Mensagem = permissao.Mensagem
                    };
                }
            }
            return null;
        }

        private Pessoa GetRegisterParaLogin(string identificador)
        {
            Pessoa pessoa = null;
            if (Utilidades.IsValidEmail(identificador))
            {
                pessoa = _clienteRepository.GetByClientLogin(identificador);
            }
            else
            {
                pessoa = _clienteRepository.GetDadosBasicos(identificador);
            }
            return pessoa;
        }

        public int AceitarTermoInadimplencia(int idOrdemVenda, int matricula, Aplicacoes aplicacao)
        {
            return _alunoRepository.SetChamadoInadimplencia(new PermissaoInadimplencia
            {
                IdOrdemDeVenda = idOrdemVenda,
                Matricula = matricula,
                IdAplicacao = (int)aplicacao
            });
        }

        private bool StatusLoginPermiteAcesso(ValidacaoLogin status)
        {
            var statusPermiteAcesso = new ValidacaoLogin[]
            {
                ValidacaoLogin.Sucesso,
                ValidacaoLogin.SucessoAviso,
                ValidacaoLogin.InadimplenteTermos
            };
            return statusPermiteAcesso.Contains(status);
        }

        public int RegistrarTokenNotificacaoDevice(int matricula, string token, Utilidades.TipoDevice idDevice, Aplicacoes aplicacao)
        {
            var result = 0;
            if (!string.IsNullOrEmpty(token) && (Utilidades.IsTablet(idDevice) || Utilidades.IsMobile(idDevice)))
            {
                result = _alunoRepository.SetDeviceToken(new DeviceToken
                {
                    Register = matricula,
                    Token = token,
                    bitIsTablet = Utilidades.IsTablet(idDevice),
                    AplicacaoId = (int)aplicacao
                });
            }
            return result;
        }

        public VersaoDTO ObterDadosVersao(string versaoApp, string ultimaVersao, Aplicacoes aplicacao)
        {
            var versaoInexistente = "0.0.0";
            versaoApp = string.IsNullOrEmpty(versaoApp) ? versaoInexistente : versaoApp;

            var versaoBloqueada = _versaoAppPermissaoRepository.GetUltimaVersaoBloqueada((int)aplicacao);

            if (string.IsNullOrEmpty(versaoBloqueada))
            {
                versaoBloqueada = versaoInexistente;
            }

            return new VersaoDTO
            {
                VersaoValida = !Utilidades.VersaoMenorOuIgual(versaoApp, versaoBloqueada),
                VersaoAtualizada = string.IsNullOrEmpty(ultimaVersao) || Utilidades.VersaoMaiorOuIgual(versaoApp, ultimaVersao),
                NumeroUltimaVersao = ultimaVersao
            };
        }

        private Dictionary<Produto.Cursos, Utilidades.EMenuAccessObject> GetListaPermissoesRecursosRMais()
        {
            return new Dictionary<Produto.Cursos, Utilidades.EMenuAccessObject>
            {
                { Produto.Cursos.R3Cirurgia, Utilidades.EMenuAccessObject.RecursosRMaisCirurgia },
                { Produto.Cursos.R3Clinica, Utilidades.EMenuAccessObject.RecursosRMaisClinica },
                { Produto.Cursos.R3Pediatria, Utilidades.EMenuAccessObject.RecursosRMaisPediatria },
                { Produto.Cursos.R4GO, Utilidades.EMenuAccessObject.RecursosRMaisGO }
            };
        }

        private IDictionary<Utilidades.EMenuAccessObject, bool> ObterPermissoesRecursosRMais(List<Menu> menus)
        {
            var result = new Dictionary<Utilidades.EMenuAccessObject, bool>();
            var tarefas = new Dictionary<Utilidades.EMenuAccessObject, Task<bool>>();

            var listaRMaisPermissao = GetListaPermissoesRecursosRMais();

            foreach (var rmais in listaRMaisPermissao)
            {
                tarefas.Add(rmais.Value, Task.Factory.StartNew(() =>
                {                 
                    var permissao = Utilidades.MenuPermitido(menus, (int)rmais.Value);
                    return permissao;
                }));
            }

            Task.WaitAll(tarefas.Select(t => t.Value).ToArray());
            foreach(var t in tarefas)
            {
                result.Add(t.Key, t.Value.Result);
            }
            return result;
        }

        private IDictionary<Utilidades.EMenuAccessObject, bool> LiberarPermissoesRecursosRMaisProfessor()
        {
            var result = new Dictionary<Utilidades.EMenuAccessObject, bool>();

            var listaRMaisPermissao = GetListaPermissoesRecursosRMais();

            foreach (var rmais in listaRMaisPermissao)
            {
                result.Add(rmais.Value, true);
            }

            return result;
        }

        private Task<IDictionary<Utilidades.EMenuAccessObject, bool>> ObterPermissoesRecursosRMaisAsync(List<Menu> menus)
        {
            return Task.Factory.StartNew(() => ObterPermissoesRecursosRMais(menus));
        }

        private bool ObterPermissoesRecursosRUm(List<Menu> menus)
        {
            return menus.Where(p=>p.Id == (int)Utilidades.EMenuAccessObject.RecursosRUm).Any();
        }

        private Task<List<Menu>> ObterPermissoesRecursosAsync(int matricula, string versao, Aplicacoes aplicacao)
        {
            return Task.Factory.StartNew(() => _menuBusiness.GetPermitidos((int)aplicacao, matricula, 0, 0, versao));
        }

        private bool PossuiPermissaoRecursoRMais(int matricula, bool possuiAcessoR1, IDictionary<Utilidades.EMenuAccessObject, bool> acessoRMais)
        {
            var permissaoPorOv = acessoRMais.Any(a => a.Value);
            return permissaoPorOv || GanhaPermissaoRMaisPorInteresse(matricula, possuiAcessoR1, acessoRMais);
        }

        public bool GanhaPermissaoRMaisPorInteresse(int matricula, bool possuiAcessoR1, IDictionary<Utilidades.EMenuAccessObject, bool> acessoRMais)
        {
            var permissaoPorInteresseRMais = false;
            var permissaoPorOv = acessoRMais.Any(a => a.Value);

            if (!permissaoPorOv && possuiAcessoR1)
            {
                permissaoPorInteresseRMais = _perfilRepository.AlunoTemInteresseRMais(matricula);
            }
            return permissaoPorInteresseRMais;
        }

        private HashPermissaoDTO ObterHashPermissao(params KeyValuePair<string, object>[] objetos)
        {
            var hash = new HashPermissaoDTO { Permissoes = new Dictionary<string, object>() };
            foreach(var item in objetos)
            {
                hash.Permissoes.Add(item.Key, item.Value);
            }
            return hash;
        }      

        public ValidaLoginDTO ValidaAcesso(int matricula, Aplicacoes aplicacao, string versaoApp)
        {
            if(matricula == default(int))
            {
                var tarefaVersaoLoja = Utilidades.ObterUltimaVersaoLojaAsync(aplicacao);
                var versao = ObterDadosVersao(versaoApp, tarefaVersaoLoja.Result, aplicacao);
                return new ValidaLoginDTO
                {
                    Validacao = ValidacaoLogin.Sucesso,
                    Versao = versao
                };
            }

            var cliente = _clienteRepository.GetDadosBasicos(matricula);
            return Login(cliente.Register, string.Empty, false, aplicacao, versaoApp);
        }
    }
}