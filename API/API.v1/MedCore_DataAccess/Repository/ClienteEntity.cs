using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_API.Academico;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class ClienteEntity : PessoaEntity, IClienteEntity
    {
        public Clientes GetByFilters(Cliente registro, int ano = 0, Aplicacoes aplicacao = Aplicacoes.LeitordeApostilas)
        {
            using(MiniProfiler.Current.Step("Obtendo dados do aluno"))
            {
                var isMedSoftPro = (aplicacao == Aplicacoes.MsProMobile);
                var isMedSoftProDesktop = ((int)aplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON);
                var isMedEletro = (aplicacao == Aplicacoes.MEDELETRO);
                var isConcursos = (aplicacao == Aplicacoes.Concursos);
                var isRecursos = (aplicacao == Aplicacoes.Recursos || aplicacao == Aplicacoes.Recursos_iPad || aplicacao == Aplicacoes.Recursos_iPhone || aplicacao == Aplicacoes.Recursos_Android);
                var isVisitanteMedcode = (aplicacao == Aplicacoes.MEDCODE && registro.Register.Trim().Equals("99999999999"));
                var aplicacoesServico = Utilidades.AplicacoesServico();
                ano = (ano == 0) ? DateTime.Now.Year : ano;
                var ctx = new DesenvContext();
                int clientId;

                if (isMedSoftPro || isMedSoftProDesktop || isMedEletro)
                {
                    var isEmail = Utilidades.IsValidEmail(registro.Register);
                    if (isEmail)
                    {
                        var email = registro.Register.ToLower().Trim();
                        var aluno = (from l in ctx.tblPersons
                                    where l.txtClientLogin == email
                                    select new Cliente
                                    {
                                        ID = l.intContactID,
                                        Register = l.txtRegister
                                    }).FirstOrDefault();

                        if (aluno != null)
                        {
                            clientId = aluno.ID;
                            registro.Register = aluno.Register.Trim();
                        }
                        else
                        {
                            clientId = 0;
                        }

                    }
                    else
                    {
                        clientId = (from l in ctx.tblPersons
                                    where (String.IsNullOrEmpty(registro.Register) || registro.Register.Equals(l.txtRegister))
                                    select l.intContactID).FirstOrDefault();
                    }
                }
                else
                {
                    clientId = (from l in ctx.tblPersons
                                where (String.IsNullOrEmpty(registro.Register) || registro.Register.Equals(l.txtRegister))
                                select l.intContactID).FirstOrDefault();
                }



                var blacklistService = new BlackListEntity();
                var isClienteNaoNulo = clientId > 0;
                bool isOnlyMedReaderDenied = false;
                bool isBloqueadoPorAplicacoesOuAplicativos = false;

                if (isClienteNaoNulo)
                {
                    using(MiniProfiler.Current.Step("Obtendo dados de blacklist"))
                    {
                        var blacklistMember = blacklistService.GetAll().Where(x => x.ID == clientId).FirstOrDefault();

                        if (blacklistMember != null)
                        {
                            isBloqueadoPorAplicacoesOuAplicativos = (blacklistService.IsBloqueado(blacklistMember, Bloqueio.TipoBloqueio.Aplicacao) || blacklistService.IsBloqueado(blacklistMember, Bloqueio.TipoBloqueio.Aplicativos));
                            isOnlyMedReaderDenied = blacklistMember.Bloqueios.Count == 1 && (blacklistMember.Bloqueios[0].AplicacaoId == (int)Aplicacoes.LeitordeApostilas);

                            if (isOnlyMedReaderDenied && aplicacao != Aplicacoes.LeitordeApostilas)
                                isBloqueadoPorAplicacoesOuAplicativos = false;
                        }
                    }
                }
                using(MiniProfiler.Current.Step("Construindo dados do cliente como permissoes e tipo de usuario"))
                {
                    List<Cliente> clientes = new List<Cliente>();

                    var client = (from l in ctx.tblPersons
                                join c in ctx.tblClients on l.intContactID equals c.intClientID
                                join pp in ctx.tblPersons_Passwords on l.intContactID equals pp.intContactID into ppp

                                from j in ppp.DefaultIfEmpty()
                                where (String.IsNullOrEmpty(registro.Register) || registro.Register.Equals(l.txtRegister))
                                && (!isBloqueadoPorAplicacoesOuAplicativos || isRecursos || (isOnlyMedReaderDenied && aplicacao == Aplicacoes.MsProMobile))
                                select new
                                {
                                    ID = l.intContactID,
                                    Nome = l.txtName,
                                    Register = l.txtRegister,
                                    Senha = j.txtPassword,
                                    Login = l.txtClientLogin,
                                    intEspecialidadeID = c.intEspecialidadeID
                                }).ToList();


                    clientes.AddRange(client.Select(a => new Cliente() { ID = a.ID, Nome = a.Nome, Register = a.Register, Senha = a.Senha, IdEspecialidade = a.intEspecialidadeID ?? 0, Login = a.Login }));

                    List<Cliente> alunos = new List<Cliente>();

                    if (isRecursos || aplicacao == Aplicacoes.MEDSOFT)
                    {
                        using (var ctxAcad = new AcademicoContext())
                        {
                            List<int?> ListEspecialidadeID = client.Select(x => x.intEspecialidadeID).ToList();

                            var Especialidade = (from e in ctxAcad.tblEspecialidades
                                                where ListEspecialidadeID.Contains(e.intEspecialidadeID)
                                                select new
                                                {
                                                    e.intEspecialidadeID,
                                                    e.DE_ESPECIALIDADE
                                                }).ToList();

                            var alu = (from c in client
                                    join e in Especialidade on c.intEspecialidadeID equals e.intEspecialidadeID into ee
                                    from eee in ee.DefaultIfEmpty()
                                    where !ctx.tblConcurso_Recurso_AccessDenied.Where(b => b.bitActive.Value).Select(b => b.intClientID).Contains(c.ID)
                                    select new
                                    {
                                        ID = c.ID,
                                        Nome = c.Nome,
                                        Register = c.Register,
                                        Senha = c.Senha,
                                        intEspecialidadeID = eee.intEspecialidadeID,
                                        Especialidade = eee.DE_ESPECIALIDADE
                                    }).ToList();

                            alunos.AddRange(alu.Select(a => new Cliente { ID = a.ID, Nome = a.Nome, Register = a.Register, Senha = a.Senha, IdEspecialidade = a.intEspecialidadeID, Especialidade = a.Especialidade })); 
                        }
                    }
                    var usuarios = isRecursos || aplicacao == Aplicacoes.MEDSOFT
                        ? alunos
                        : clientes;

                    var lCli = new Clientes();
                    var prod = (Produtos)(new ProdutoEntity()).GetAll();

                    var tipoPessoa = new PessoaEntity().GetPersonType(registro.Register);

                    foreach (var p in usuarios)
                    {
                        var filial = new FilialEntity().GetClientFilial(p.ID);
                        var anosContratados = GetAnosContratados(p.ID, aplicacao);
                        Cliente c = new Cliente
                        {
                            ID = p.ID,
                            Nome = p.Nome.Trim(),
                            Register = (p.Register.Trim()),
                            Senha = p.Senha ?? "",
                            Produtos = prod,
                            ProdutosContratados = ProdutoEntity.GetProdutosContratados(p.ID, anosContratados),
                            RetornoStatus = Cliente.StatusRetorno.SemAcesso,
                            AnosPermitidos = anosContratados.ToList(),
                            IdFilial = filial == null ? 0 : filial.ID,
                            Filial = filial == null ? string.Empty : filial.Nome,
                            TipoPessoa = tipoPessoa,
                            TipoPessoaDescricao = tipoPessoa.ToString(),
                            Login = p.Login
                        };

                        //Alunos com OV a partir de 2018, tem direito a acessar o Medsoft
                        //var alunoPossuiDireitoVitalicio = false;//new AlunoEntity().AlunoPossuiDireitoVitalicio(c.ID);

                        




                        var alunoPossuiDireitoVitalicio = false;
                        string[] appPermiteAlunosInativos = ConfigurationProvider.Get("Settings:AppPermiteAlunosInativos").Split(',');
                        var isAppPermiteInativos = appPermiteAlunosInativos.Contains(((int)aplicacao).ToString());

                        
                        string[] validaInadimplenciaPosLogin = ConfigurationProvider.Get("Settings:AppValidaInadimplenciaPosLogin").Split(',');
                        var appValidaAcessoPosLogin = validaInadimplenciaPosLogin.Contains(((int)aplicacao).ToString());

                        string[] appMsgBloqueioInativoOk = ConfigurationProvider.Get("Settings:AppMsgBloqueioInativoOk").Split(',');
                        var isMsgBloqueioNoLogin = appMsgBloqueioInativoOk.Contains(((int)aplicacao).ToString());

                        
                        string[] appImpedeSomenteAdapta = ConfigurationProvider.Get("Settings:AppImpedeSomenteAdapta").Split(',');
                        var isAppImpedeSomenteAdapta = appImpedeSomenteAdapta.Contains(((int)aplicacao).ToString());

                        string[] appSomenteAdapta = ConfigurationProvider.Get("Settings:AppSomenteAdapta").Split(',');
                        var isAppSomenteAdapta = appSomenteAdapta.Contains(((int)aplicacao).ToString());

                        var permissaoAluno = new AlunoEntity().GetPermissao(registro.Register, Convert.ToInt32(aplicacao), false);

                        var isAlunoAtivoAtual = IsAlunoAtivoAnoAtual(c.ID, aplicacao);
                        var isAlunoAtivoProximoAno = IsAlunoProximoAno(c.ID, aplicacao);
                        var ProximoAnoCanceladoOuInadimplente = permissaoAluno.PermiteAcesso == (int)PermissaoInadimplencia.StatusAcesso.Negado;
                        var AlunoSemAcessoNoLogin = !isVisitanteMedcode && !appValidaAcessoPosLogin && ProximoAnoCanceladoOuInadimplente;
                        var AlunoInativoComMensagemNoLogin = (isMsgBloqueioNoLogin && !isAppPermiteInativos && !isAlunoAtivoAtual && !alunoPossuiDireitoVitalicio);
                        var AlunoInativoNoLogin = (!isAppPermiteInativos && !isAlunoAtivoAtual && !alunoPossuiDireitoVitalicio);

                        if (ProximoAnoCanceladoOuInadimplente && !isAlunoAtivoProximoAno)
                        {
                            c.MensagemRetorno = permissaoAluno.Mensagem;
                            c.TipoErro = "ProximoAnoCanceladoOuInadimplente";
                            c.ETipoErro = TipoErroAcesso.ProximoAnoCanceladoOuInadimplente;
                        }
                        else if (isAlunoAtivoAtual && permissaoAluno.Mensagem != string.Empty)
                        {
                            c.MensagemRetorno = permissaoAluno.Mensagem;
                            c.TipoErro = "LeMensagemInadimplencia";
                            c.ETipoErro = TipoErroAcesso.LeMensagemInadimplencia;
                        }

                        //Separação das Restritas
                        var alunoSomenteAdapta = AlunoSomenteAdapta(c.ID);
                        var alunoSemAdapta = AlunoSemAdapta(c.ID);
                        var AlunoImpedidoSomenteAdapta = (isAppImpedeSomenteAdapta && alunoSomenteAdapta);
                        var AlunoImpedidoSemAdapta = (isAppSomenteAdapta && alunoSemAdapta);
                        if (AlunoImpedidoSemAdapta || AlunoImpedidoSomenteAdapta)
                            return new Clientes { new Cliente { RetornoStatus = Cliente.StatusRetorno.SemAcesso } };


                        c.RetornoStatus = isVisitanteMedcode ? Cliente.StatusRetorno.Sucesso : Cliente.StatusRetorno.SemAcesso;
                        if (c.ProdutosContratados.Any())
                            c.RetornoStatus = AlunoSemAcessoNoLogin ? Cliente.StatusRetorno.SemAcesso :
                                AlunoImpedidoSemAdapta ? Cliente.StatusRetorno.SemAcesso :
                                AlunoImpedidoSomenteAdapta ? Cliente.StatusRetorno.SemAcesso :
                                AlunoInativoComMensagemNoLogin ? Cliente.StatusRetorno.Cancelado :
                                AlunoInativoNoLogin ? Cliente.StatusRetorno.SemAcesso :
                                Cliente.StatusRetorno.Sucesso;

                        // _____________________________________________ NICKNAME (Refatorar!)
                        c.NickName = Utilidades.GetNickName(c.Nome);

                        if (aplicacao == Aplicacoes.MEDSOFT && !string.IsNullOrEmpty(ctx.tblPersons.Where(a => a.intContactID == c.ID).FirstOrDefault().txtNickName))
                            c.NickName = ctx.tblPersons.Where(a => a.intContactID == c.ID).FirstOrDefault().txtNickName.Trim().Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\'", "");

                        if (aplicacao == Aplicacoes.MsProMobile || aplicacao == Aplicacoes.MsProDesktop)
                        {
                            var nick = ctx.tblPersons.Where(a => a.intContactID == c.ID).FirstOrDefault().txtNickName;
                            c.NickName = nick == null
                                ? string.Empty
                                : nick.Trim().Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\'", "");
                        }


                        // _____________________________________________ INADIMPLENCIA
                        var permissaoInadimplencia = new AlunoEntity().GetPermissaoInadimplencia(Convert.ToInt32(aplicacao), false, registro.Register);

                        c.LstOrdemVendaMsg = permissaoInadimplencia.LstOrdemVendaMsg;
                        c.SituacaoAluno = -1;
                        var isInadimplente = permissaoInadimplencia.PermiteAcesso == (int)PermissaoInadimplencia.StatusAcesso.Negado;

                        if (isInadimplente)
                        {
                            c.MensagemRetorno = permissaoInadimplencia.Mensagem;
                            c.TituloMensagemRetorno = permissaoInadimplencia.PermiteAcesso == 1 ? TipoErroAcesso.LeMensagemInadimplencia.GetDescription() : TipoErroAcesso.BloqueadoInadimplencia.GetDescription();
                            c.TipoErro = permissaoInadimplencia.PermiteAcesso == 1 ? "LeMensagemInadimplencia" : "BloqueadoInadimplencia";
                            c.ETipoErro = permissaoInadimplencia.PermiteAcesso == 1 ? TipoErroAcesso.LeMensagemInadimplencia : TipoErroAcesso.BloqueadoInadimplencia;
                            c.RetornoStatus = permissaoInadimplencia.PermiteAcesso == 1 ? Cliente.StatusRetorno.Sucesso : Cliente.StatusRetorno.SemAcesso;
                        }

                        if (aplicacao == Aplicacoes.MEDELETRO && !isInadimplente)
                        {
                            var anoNovoAplicativoMedeletro = 2017;
                            var isMedeletroAtivo = Convert.ToBoolean(ConfigurationProvider.Get("Settings:isMedeletroAnoAtualLiberado"));
                            var anoMedeletro = Utilidades.GetAnoInscricao(Aplicacoes.INSCRICAO_MEDELETRO);
                            //var anoAtual = isMedeletroAtivo ? anoMedeletro : anoMedeletro - 1;
                            var LiberacaoPrimeiraAula = DateTime.Now.AddHours(2);
                            var anosInscritoMedeletro = (from so in ctx.tblSellOrders
                                                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                                        join curso in ctx.tblCourses on sod.intProductID equals curso.intCourseID
                                                        join produto in ctx.tblProducts on curso.intCourseID equals produto.intProductID
                                                        join mc in ctx.mview_Cronograma on produto.intProductID equals mc.intCourseID
                                                        where ((produto.intProductGroup1 ?? 0) == (int)Produto.Produtos.MEDELETRO || (produto.intProductGroup1 ?? 0) == (int)Produto.Produtos.MEDELETRO_IMED) && new[] { 2, 5 }.Contains(so.intStatus ?? 0)
                                                                && mc.dteDateTime < LiberacaoPrimeiraAula && curso.intYear <= anoMedeletro && so.intClientID == c.ID
                                                        select curso.intYear);
                            var permissaoMedeletro = anosInscritoMedeletro.Any();
                            if (permissaoMedeletro)
                            {
                                var anoMaximoMedEletro = anosInscritoMedeletro.Max();
                                c.SituacaoAluno = anoMaximoMedEletro < anoNovoAplicativoMedeletro ? 0 : anoMaximoMedEletro == anoMedeletro && isMedeletroAtivo ? 2 : 1;
                            }

                            c.RetornoStatus = permissaoMedeletro ? Cliente.StatusRetorno.Sucesso : Cliente.StatusRetorno.SemAcesso;
                        }

                        c.Especialidade = p.Especialidade;
                        c.AcessoGolden = UserGolden(p.Register) > 0;
                        c.Foto = GetClienteFoto(p.ID);
                        c.FotoPerfil = GetClienteFotoPerfil(p.ID);
                        c.Avatar = GetClienteAvatar(p.ID).Caminho;

                        var possuiMedeletro = c.ProdutosContratados.Contains((int)Produto.Cursos.MEDELETRO);

                        if (possuiMedeletro && (isConcursos || isRecursos))
                        {
                            if (GetOnlyMedeletro(p.ID, aplicacao))
                            {
                                c.RetornoStatus = Cliente.StatusRetorno.SemAcesso;
                            }
                        }

                        var possuiMedeletroIMed = c.ProdutosContratados.Contains((int)Produto.Cursos.MEDELETRO_IMED);
                        if (possuiMedeletroIMed && aplicacoesServico.Contains((int)aplicacao))
                        {
                            if (GetOnlyMedeletroIMed(p.ID, aplicacao))
                            {
                                c.RetornoStatus = Cliente.StatusRetorno.SemAcesso;
                            }
                        }

                        var possuiCpMed = c.ProdutosContratados.Contains((int)Produto.Cursos.CPMED);
                        if (isClientCPMedR(p.ID))
                        {
                            if (!HasAlunoCpMedOriginal(p.ID, aplicacao))
                            {
                                c.ProdutosContratados.Remove((int)Produto.Cursos.CPMED);
                            }

                            if (IsOnlyCpMedR(p.ID, aplicacao))
                            {
                                if (aplicacao == Aplicacoes.AreaRestrita)
                                    c.RetornoStatus = Cliente.StatusRetorno.SemAcessoCpMedR;
                                else
                                    c.RetornoStatus = Cliente.StatusRetorno.SemAcesso;
                            }
                        }

                        if (!PermiteAcessoMobile((int)aplicacao, c.ID))
                            c.RetornoStatus = Cliente.StatusRetorno.SemAcesso;



                        if (isRecursos)
                        {
                            
                            var anos = ConfigurationProvider.Get("Settings:anosRecursos").Split(',').Select(Int32.Parse).ToList();
                            var ativo = tipoPessoa == Pessoa.EnumTipoPessoa.Funcionario ? true : anos.Any(x => c.AnosPermitidos.Contains(x));
                            c.IdEspecialidade = p.IdEspecialidade;

                            if (ativo) lCli.Add(c);
                        }
                        else if (aplicacao == Aplicacoes.LeitordeApostilas)
                        {
                            c.IdEspecialidade = p.IdEspecialidade;
                            lCli.Add(c);
                        }
                        else
                        {
                            lCli.Add(c);
                        }

                    }
                    return lCli.Count > 0 ? lCli : new Clientes { new Cliente { RetornoStatus = Cliente.StatusRetorno.SemAcesso, Produtos = prod, TipoErro = "ErroGenerico", MensagemRetorno = "Usuário não cadastrado." } };
                }
            }
        }

        public bool PermiteAcessoMobile(int idAplicacao, int matricula)
        {
            try
            {
                
                string[] appsMoveis = ConfigurationProvider.Get("Settings:AppsMoveis").Split(',');

                var idChamdadoSemMobile = 2375;
                var isAppMovel = appsMoveis.Contains(idAplicacao.ToString());
                if (!isAppMovel) return true;
                var isChamadoSemMobileAberto = new ChamadoCallCenterEntity().ExisteChamadoAberto(Convert.ToInt32(idChamdadoSemMobile), (Convert.ToInt32(matricula)));
                return !isChamadoSemMobileAberto;
            }
            catch
            {

                throw;
            }
        }

        public bool IsOnlyCpMedR(int idCliente, Aplicacoes aplicacao)
        {
            var idsTurmasCPMED_R = new List<int>();
            var anos = new List<int>();

            anos = Utilidades.GetAnosValidosDaAplicacao(idCliente, aplicacao);

            var ordens = new OrdemVendaEntity().GetResumed(idCliente, anos.ToList(), 0, 0);

            using (var ctx = new DesenvContext())
            {
                idsTurmasCPMED_R = (from cpmR in ctx.tblTurmasCPMED_R
                                    select cpmR.ProductID).ToList();
            }

            List<OrdemVenda> ordensValidas = (from o in ordens
                                              where (int)o.Status == (int)OrdemVenda.StatusOv.Ativa
                                              && !idsTurmasCPMED_R.Contains(o.IdProduto)
                                              select o).ToList();

            var valido = ordensValidas.Count > 0;

            var testeMeiodeAno = IsCicloCompletoNoMeioDoAno(idCliente);

            if (valido || testeMeiodeAno)
            {
                return false;
            }
            else
            {
                List<OrdemVenda> ordensTurmasCPMED_R = (from o in ordens
                                                        where (int)o.Status == (int)OrdemVenda.StatusOv.Ativa
                                                        && idsTurmasCPMED_R.Contains(o.IdProduto)
                                                        select o).ToList();

                if (ordensTurmasCPMED_R.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool HasAlunoCpMedOriginal(int idCliente, Aplicacoes aplicacao)
        {
            var ano = Utilidades.GetYear();
            var idsTurmasCPMED_R = new List<int>();
            var isAntesDataLimite = Utilidades.IsBeforeDataLimite((int)aplicacao, ano);
            var anos = isAntesDataLimite ? new[] { ano, ano - 1 } : new[] { ano };
            var ordens = new OrdemVendaEntity().GetResumed(idCliente, anos.ToList(), 0, 0);

            using (var ctx = new DesenvContext())
            {
                idsTurmasCPMED_R = (from cpmR in ctx.tblTurmasCPMED_R
                                    select cpmR.ProductID).ToList();
            }

            List<OrdemVenda> ordensVendaCPMEDOriginal = (from o in ordens
                                                         where (int)o.Status == (int)OrdemVenda.StatusOv.Ativa
                                                         && o.GroupID == (int)Produto.Produtos.CPMED
                                                         && !idsTurmasCPMED_R.Contains(o.IdProduto)
                                                         select o).ToList();

            var valido = ordensVendaCPMEDOriginal.Count > 0;

            if (valido)
                return true;
            else
                return false;
        }

        public bool isClientCPMedR(int idCliente)
        {
            using (var ctx = new DesenvContext())
            {
                var ret = (from so in ctx.tblSellOrders
                           join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                           join cpmR in ctx.tblTurmasCPMED_R on sod.intProductID equals cpmR.ProductID
                           where so.intClientID == idCliente
                           select new Cliente
                           {
                               ID = so.intClientID
                           }).Any();
                return ret;
            }
        }

        private bool GetOnlyMedeletroIMed(int idCliente, Aplicacoes aplicacao)
        {
            var ano = Utilidades.GetYear();
            var isAntesDataLimite = Utilidades.IsBeforeDataLimite((int)aplicacao, ano);
            var anos = isAntesDataLimite ? new[] { ano, ano - 1 } : new[] { ano };
            var ordens = new OrdemVendaEntity().GetResumed(idCliente, anos.ToList(), 0, 0);
            List<OrdemVenda> ordensValidas = (from o in ordens
                                              where (int)o.Status == (int)OrdemVenda.StatusOv.Ativa
                                              && o.GroupID != (int)Produto.Produtos.MEDELETRO_IMED
                                              select o).ToList();
            return !(ordensValidas.Count > 0 || IsCicloCompletoNoMeioDoAno(idCliente));
        }

        public bool GetOnlyMedeletro(int idCliente, Aplicacoes aplicacao)
        {
            var ano = Utilidades.GetYear();
            var isAntesDataLimite = Utilidades.IsBeforeDataLimite((int)aplicacao, ano);
            var anos = isAntesDataLimite ? new[] { ano, ano - 1 } : new[] { ano };
            var ordens = new OrdemVendaEntity().GetResumed(idCliente, anos.ToList(), 0, 0);
            List<OrdemVenda> ordensValidas = (from o in ordens
                                              where (int)o.Status == (int)OrdemVenda.StatusOv.Ativa
                                              && o.GroupID != (int)Produto.Produtos.MEDELETRO
                                              select o).ToList();

            var valido = ordensValidas.Count > 0;
            var testeMeiodeAno = IsCicloCompletoNoMeioDoAno(idCliente);

            if (valido || testeMeiodeAno)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsCicloCompletoNoMeioDoAno(int matricula)
        {
            var ctx = new DesenvContext();
            var meioDeAno = ctx.tblAlunosAnoAtualMaisAnterior.Where(x => x.intClientID == matricula).Any();
            return meioDeAno;
        }

        public string GetClienteFoto(int intContactID)
        {
            var ctx = new DesenvContext();
            var foto = (from pp in ctx.tblPersonsPicture
                        where pp.intContactID == intContactID
                        select pp.txtPicturePath.Trim()).FirstOrDefault();
            return string.Concat(Constants.LINK_STATIC_FOTOS, foto);
        }

        public Avatar GetClienteAvatar(int matricula)
        {
            if (!string.IsNullOrEmpty(matricula.ToString()))
            {
                using (var ctx = new DesenvContext())
                {
                    var foto = ctx.tblPersonsPicture.Where(a => a.intContactID == matricula && a.bitActive == true).FirstOrDefault();
                    if (foto != null)
                        return new Avatar { Caminho = string.Format("{0}{1}", Constants.LINK_STATIC_FOTOS, foto.txtPicturePath.Trim()), Matricula = matricula };

                    var avatar = ctx.tblPersonsAvatar.Where(a => a.intContactID == matricula && a.bitActive == true).FirstOrDefault();
                    if (avatar != null)
                    {
                        var caminhoAvatar = ctx.tblAvatar.Where(a => a.intAvatarID == avatar.intAvatarID).FirstOrDefault();
                        return new Avatar { Caminho = string.Format("{0}{1}", Constants.LINK_STATIC_AVATARES, caminhoAvatar.txtAvatarPath.Trim()), Matricula = matricula };
                    }
                }
            }
            return new Avatar { CaminhoImagemPadrao = Constants.LINK_STATIC_AVATAR_PADRAO, Matricula = matricula };
        }

        public int[] GetAnosContratados(int idCliente, Aplicacoes aplicacao = Aplicacoes.LeitordeApostilas)
        {
            var idAplicacao = (int)aplicacao == 0 ? (int)Aplicacoes.LeitordeApostilas : (int)aplicacao;
            var apps2012 = new[] { 5, 6 };
            var statusPermitidos = new[] { 0, 2, 5, 8 }.ToList();
            var ctx = new DesenvContext();
            return (from e in ctx.Set<emed_CursosAnosStatus_Result>().FromSqlRaw("emed_CursosAnosStatus @intClientID = {0}, @intYear = {1}, @intProductGroup1 = {2}, @intOrderStatus = {3}", idCliente, null, null, null).ToList()
                    where statusPermitidos.Contains((int)e.intStatus)
                    && (e.intYear > 2012 || !apps2012.Contains(idAplicacao))
                    select Convert.ToInt32(e.intYear)).Distinct().ToArray();
        }

        public MensagemRecurso GetMensagemLogin(int aplicacaoId, int tipoMensagem)
        {
            using(MiniProfiler.Current.Step("Obtendo mensagem de login"))
            {
                using (var ctx = new DesenvContext())
                {
                    try
                    {
                        var mensagem = ctx.tblMensagensLogin.Where(o => o.intAplicacaoId == aplicacaoId && o.intTipoMensagemId == tipoMensagem);
                        return new MensagemRecurso { Texto = mensagem.FirstOrDefault().txtMensagem.Trim() };
                    }
                    catch
                    {
                        return new MensagemRecurso();
                    }
                }
            }
        }

        public Cliente UpdateEsqueciSenha(Cliente cliente, Aplicacoes aplicacao = Aplicacoes.AreaRestrita, bool isAdaptaMed = false)
        {
            var person = new tblPersons_Passwords();
            var emailEnvio = string.Empty;
            var emailFrom = "";
            var ctx = new DesenvContext();
            var r = new Random();
            var chave = r.Next(100000, 2000000);
            var resultado = new tblPersons();
            var c = new Cliente();

            using(MiniProfiler.Current.Step("Atualizando informações de senha"))
            {
                var isMedSoftPro = (aplicacao == Aplicacoes.MsProMobile);
                var isMedSoftProDesktop = ((int)aplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON);
                var isMedEletro = (aplicacao == Aplicacoes.MEDELETRO);

                if (isMedSoftPro || isMedSoftProDesktop || isMedEletro)
                {
                    var isEmail = Utilidades.IsValidEmail(cliente.Register);
                    if (isEmail)
                    {
                        var email = cliente.Register.ToLower().Trim();
                        var aluno = (from l in ctx.tblPersons
                                    where l.txtClientLogin == email
                                    select l).FirstOrDefault();

                        if (aluno != null)
                        {
                            resultado = aluno;
                        }
                        else
                        {
                            resultado.intContactID = 0;
                        }
                    }
                    else
                    {
                        resultado = (from l in ctx.tblPersons
                                    where (String.IsNullOrEmpty(cliente.Register) || cliente.Register.Equals(l.txtRegister))
                                    select l).FirstOrDefault();
                    }
                }
                else
                {
                    resultado = (from l in ctx.tblPersons
                                where (String.IsNullOrEmpty(cliente.Register) || cliente.Register.Equals(l.txtRegister))
                                select l).FirstOrDefault();
                }

                person = ctx.tblPersons_Passwords.SingleOrDefault(a => a.intContactID == resultado.intContactID);
                


                if (string.IsNullOrEmpty(resultado.txtEmail1))
                {
                    if (string.IsNullOrEmpty(resultado.txtEmail2))
                    {
                        if (!string.IsNullOrEmpty(resultado.txtEmail3))
                            emailEnvio = resultado.txtEmail3;
                    }
                    else
                        emailEnvio = resultado.txtEmail2;
                }
                else
                    emailEnvio = resultado.txtEmail1;
            }

            if (person != null && !string.IsNullOrEmpty(emailEnvio))
            {
                person.intChave = chave;
                person.dteDataLimite = DateTime.Now.AddDays(1);
                ctx.SaveChanges();
                c.ID = resultado.intContactID;
                c.Key = (int)person.intChave;
;
                var link = ConfigurationProvider.Get("ConnectionStrings:DesenvConnection");
                var site = string.Empty;
                if (link.Contains("ordomederi"))
                {
                    if (aplicacao == Aplicacoes.Recursos)
                        site = "desenv.ordomederi.com/Recursos/Home.aspx";
                    else if (isAdaptaMed)
                        site = "desenv.ordomederi.com/Restrita_Adapta/EsqueciSenha.aspx";
                    else
                        site = "desenv.ordomederi.com/Restrita/EsqueciSenha.aspx";
                }
                else
                {
                    if (aplicacao == Aplicacoes.Recursos)
                        site = "recursos.medgrupo.com.br/Home.aspx";
                    else if (isAdaptaMed)
                        site = "arearestrita.adaptamed.com.br/EsqueciSenha.aspx";
                    else
                        site = "arearestrita.medgrupo.com.br/EsqueciSenha.aspx";
                }

                var arquivoRemoto = isAdaptaMed ? "http://static.medgrupo.com.br/static/AreaRestrita/EsqueciSenha/AdaptaMed.html" : "http://static.medgrupo.com.br/static/AreaRestrita/EsqueciSenha/EsqueciSenha.html";
                var mensagem = Utilidades.GetConteudoArquivoRemoto(arquivoRemoto);
                mensagem = mensagem.Replace("@site", site).Replace("@id", c.ID.ToString()).Replace("@key", c.Key.ToString());
                var assunto = "Recadastramento de Senha";

                var e = new Email();
                e.mailBody = mensagem;
                e.mailSubject = assunto;


                if (ConfigurationProvider.Get("Settings:enviaEmailParaAluno") == "SIM")
                    e.mailTo = emailEnvio;
                else
                    e.mailTo = ConfigurationProvider.Get("Settings:emailDesenv");

                if (isAdaptaMed)
                {
                    emailFrom = "admaster@adaptamed.com.br";
                }
                
                using(MiniProfiler.Current.Step("Enviando email"))
                {
                    var retorno = Utilidades.SendMailProfile("Extensivo", e.mailTo, e.mailSubject, e.mailBody, e.copyRecipients, e.BlindCopyRecipients, mailFrom: emailFrom);
                }

            }
            else
            {
                c.ID = resultado.intContactID;
                c.Key = 0;
            }
            return c;
        }

        public int CadastrarSenha(string register, string senha, int id = 0, Aplicacoes aplicacao = Aplicacoes.MEDSOFT)
        {
            using(MiniProfiler.Current.Step("Cadastrando senha do usuario"))
            {
                var idAplicacao = (int)aplicacao;
                var ctx = new DesenvContext();
                var resultado = string.IsNullOrEmpty(register) ? ctx.tblPersons.SingleOrDefault(a => a.intContactID == id) : ctx.tblPersons.Where(c => c.txtRegister == register).OrderBy(o => o.intContactID).FirstOrDefault();
                var person = ctx.tblPersons_Passwords.SingleOrDefault(a => a.intContactID == resultado.intContactID);
                if (person == null)
                {
                    person = new tblPersons_Passwords();
                    person.intContactID = resultado.intContactID;
                    person.txtPassword = senha;
                    person.dteDatePassword = DateTime.Now;
                    person.intChave = null;
                    person.dteDataLimite = null;
                    person.intAplicacaoId = idAplicacao;
                    ctx.tblPersons_Passwords.Add(person);
                }
                else
                {
                    person.txtPassword = senha;
                    person.dteDatePassword = DateTime.Now;
                    person.intChave = null;
                    person.dteDataLimite = null;
                    person.intAplicacaoId = idAplicacao;
                }
                ctx.SaveChanges();
                return 1;
            }
        }

        public string GetLinkEsqueciEmail(int idAplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo link de esqueci minha senha"))
            {
                try
                {
                    var isMedSoftPro = (idAplicacao == Convert.ToInt32(Aplicacoes.MsProMobile));
                    var isMedSoftProDesktop = (idAplicacao == Convert.ToInt32(Aplicacoes.MEDSOFT_PRO_ELECTRON));

                    using (var ctx = new DesenvContext())
                    {
                        if (isMedSoftPro || isMedSoftProDesktop)
                        {
                            var link = ctx.tblLinkEsqueciSenha.FirstOrDefault(a => a.intApplicationId == idAplicacao);
                                return link.txtLink;
                        }

                    }

                    return string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public bool IsAlunoAtivoAnoAtual(int idCliente, Aplicacoes aplicacao, int group1 = 0, int group2 = 0, int ano = 0)
        {
            ano = (ano == 0) ? Utilidades.GetYear() : ano;
            var ctx = new DesenvContext();
            var idRevalida = (int)Produto.Produtos.REVALIDA;
            
            var dataLimite = ctx.Set<msp_GetDataLimite_ByApplication_Result>().FromSqlRaw("msp_GetDataLimite_ByApplication @intYear = {0}, @intAplicationID = {1}", ano - 1, (int)aplicacao).ToList().FirstOrDefault().dteDataLimite ?? DateTime.MinValue;
            var isAntesDataLimite = dataLimite >= DateTime.Now;
            var anos = isAntesDataLimite ? new[] { ano, ano - 1 } : new[] { ano };
            var ordens = new OrdemVendaEntity().GetResumed(idCliente, anos.ToList(), group1, group2);
            List<OrdemVenda> ordensvalidas = (from o in ordens where (int)o.Status != (int)(int)Utilidades.ESellOrderStatus.Pendente && (int)o.Status != (int)(int)Utilidades.ESellOrderStatus.Suspensa && o.GroupID != idRevalida select o).ToList();

            var meioDeAno = ctx.tblAlunosAnoAtualMaisAnterior.Where(x => x.intClientID == idCliente).Any();
            return ordensvalidas.Count() > 0 || meioDeAno;
        }

        public bool IsAlunoProximoAno(int idCliente, Aplicacoes aplicacao)
        {
            var ctx = new DesenvContext();
            var ano = Utilidades.GetYear() + 1;
            var anos = new[] { ano };
            var ordens = new OrdemVendaEntity().GetResumed(idCliente, anos.ToList(), 0, 0);
            List<OrdemVenda> ordensvalidas = (from o in ordens where (int)o.Status == (int)Utilidades.ESellOrderStatus.Ativa || (int)o.Status == (int)Utilidades.ESellOrderStatus.Cancelada select o).ToList();
            return ordensvalidas.Count() > 0;
        }

        public bool AlunoSemAdapta(int matricula)
        {
            var statusPermitidos = new[] { 0, 2, 5 }.ToList();
            var ctx = new DesenvContext();
            
            return !(from e in ctx.Set<emed_CursosAnosStatus_Result>().FromSqlRaw("emed_CursosAnosStatus @intClientID = {0}, @intYear = {1}, @intProductGroup1 = {2}, @intOrderStatus = {3}", matricula, null, null, null).ToList()
                     where statusPermitidos.Contains((int)e.intStatus)
                     && (int)Produto.Produtos.ADAPTAMED == e.intProductGroup1
                     select Convert.ToInt32(e.intYear)).Any();

        }

        public bool AlunoSomenteAdapta(int matricula)
        {
            var statusPermitidos = new[] { 0, 2, 5 }.ToList();
            var ctx = new DesenvContext();
            

            var cursos = ctx.Set<emed_CursosAnosStatus_Result>().FromSqlRaw("emed_CursosAnosStatus @intClientID = {0}, @intYear = {1}, @intProductGroup1 = {2}, @intOrderStatus = {3}", matricula, null, null, null).ToList();
            var possuiadapta = (from e in cursos
                                where statusPermitidos.Contains((int)e.intStatus)
                                && (int)Produto.Produtos.ADAPTAMED == e.intProductGroup1
                                select Convert.ToInt32(e.intYear)).Any();

            var possuiOutroCurso = (from e in cursos
                                    where statusPermitidos.Contains((int)e.intStatus)
                                    && (int)Produto.Produtos.ADAPTAMED != e.intProductGroup1
                                    select Convert.ToInt32(e.intYear)).Any();

            return possuiadapta && !possuiOutroCurso;
        }

        public int UserGolden(string register, Aplicacoes aplicacao = Aplicacoes.LeitordeApostilas)
        {
            var module = "E-MED";
            var name = "intTempoExpiracaoAcessoGolden";
            using (var ctx = new DesenvContext())
            {
                var retorno = 0;
                var resul = ctx.tblParametrosGenericos.Where(a => a.txtModule == module && a.txtName == name).FirstOrDefault().txtValue;
                var limite = Convert.ToInt32(resul);
                var atual = DateTime.Now.AddMinutes(-limite);
                var ret = ctx.tblEmed_AccessGolden.Where(b => b.txtCPF == register && (b.bitEterno || atual < b.dteDateTime)).Any();
                if (ret)
                    retorno = 1;
                return retorno;
            }
        }

        public Cliente GetDadosBasicos(string registro)
        {
            using (var ctx = new DesenvContext())
            {
                var pessoa = ctx.tblPersons.Where(p => p.txtRegister == registro).FirstOrDefault();

                if (pessoa != null)
                {
                    return new Cliente
                    {
                        Nome = pessoa.txtName,
                        Register = pessoa.txtRegister,
                        ID = pessoa.intContactID
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public string ObterSenhaGolden()
        {
            return Utilidades.ParametroGenericoValue("Relacionamento", "txtAcessoGolden");
        }

        public Cliente GetDadosBasicos(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var pessoa = ctx.tblPersons.Where(p => p.intContactID == matricula).FirstOrDefault();

                if (pessoa != null)
                {
                    return new Cliente
                    {
                        Nome = pessoa.txtName,
                        Register = pessoa.txtRegister,
                        ID = pessoa.intContactID
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public Clientes GetPreByFilters(Cliente registro, Aplicacoes aplicacao = Aplicacoes.LeitordeApostilas)
        {
            Clientes clientes = new Clientes();

            var isEmail = Utilidades.IsValidEmail(registro.Register);
            var email = registro.Register.ToLower().Trim();

            using (var ctx = new DesenvContext())
            {
                var client = (from l in ctx.tblPersons
                              join pp in ctx.tblPersons_Passwords on l.intContactID equals pp.intContactID into ppp
                              from j in ppp.DefaultIfEmpty()
                              where (isEmail && email.Equals(l.txtClientLogin))
                              || (!isEmail && registro.Register.Equals(l.txtRegister))
                              select new
                              {
                                  ID = l.intContactID,
                                  Nome = l.txtName,
                                  Register = l.txtRegister,
                                  Senha = j.txtPassword,
                                  Login = l.txtClientLogin
                              }).ToList();

                clientes.AddRange(client.Select(a => new Cliente() { ID = a.ID, Nome = a.Nome, Register = a.Register, Senha = a.Senha ?? "", Login = a.Login }));
            }

            var blacklistService = new BlackListEntity();
            var isBlacklistMember = false;
            if (clientes.Any()) isBlacklistMember = blacklistService.IsAcessoBloqueado(clientes.FirstOrDefault().ID, aplicacao);
            if (isBlacklistMember) clientes = new Clientes();

            clientes.ForEach(c =>
            {
                c.NickName = GetNickName(c, aplicacao);
                c.FotoPerfil = GetClienteFotoPerfil(c.ID);
            });

            return clientes.Count > 0 ? clientes : new Clientes { new Cliente { RetornoStatus = (clientes.Any()) ? Cliente.StatusRetorno.SemAcesso : Cliente.StatusRetorno.Inexistente, TipoErro = "ErroGenerico", MensagemRetorno = "Usuário não cadastrado." } };
        }

        public string GetNickName(Cliente _cliente, Aplicacoes aplicacao)
        {
            try
            {
                var NickName = Utilidades.GetNickName(_cliente.Nome);
                using (var ctx = new DesenvContext())
                {
                    if (aplicacao == Aplicacoes.MEDSOFT && !string.IsNullOrEmpty(ctx.tblPersons.Where(a => a.intContactID == _cliente.ID).FirstOrDefault().txtNickName))
                        NickName = ctx.tblPersons.Where(a => a.intContactID == _cliente.ID).FirstOrDefault().txtNickName.Trim().Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\'", "");

                    if (aplicacao == Aplicacoes.MsProMobile || aplicacao == Aplicacoes.MsProDesktop)
                    {
                        var nick = ctx.tblPersons.Where(a => a.intContactID == _cliente.ID).FirstOrDefault().txtNickName;
                        NickName = nick == null
                            ? string.Empty
                            : nick.Trim().Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\'", "");
                    }
                }

                return NickName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public AlunoDTO GetAlunoPorFiltros(AlunoDTO filtro)
        {
            using (var ctx = new DesenvContext())
            {

                var aluno = (from l in ctx.tblPersons
                             where (filtro.Email != null && l.txtClientLogin == filtro.Email)
                                || (filtro.Register != null && l.txtRegister != null && l.txtRegister.Trim() == filtro.Register.Trim())
                                || (filtro.Id != 0 && l.intContactID == filtro.Id)
                             select new AlunoDTO
                             {
                                 Id = l.intContactID,
                                 Register = l.txtRegister,
                                 Email = l.txtClientLogin
                             }).FirstOrDefault();
                return aluno;
            }
        }

        public string GetClienteFotoPerfil(int matricula)
        {
            try
            {
                var query = string.Format("select dbo.medfn_GetParametroGenerico('Sistema','strImageUrl')+'?intContactID={0}'", matricula);
                var ds = new DBQuery().ExecuteQuery(query);

                return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0][0].ToString() : string.Empty;
            }
            catch
            {
                throw;
            }
        }

        public bool IsAlunoProximoAnoInativoAnoAtual(int idCliente, Aplicacoes aplicacao)
        {
            if (!Utilidades.IsActiveFunction(Utilidades.Funcionalidade.AlunoProximoAnoCanceladoAnoAtual)) return false;

            var retorno = IsAlunoProximoAno(idCliente, aplicacao) && !IsAlunoAtivoAnoAtual(idCliente, aplicacao);
            return retorno;
        }

        public AlunoSenhaDTO GetAlunoSenha(int clientId)
        {
            using (var ctx = new DesenvContext())
            {
                var alunoSenha = ctx.tblPersons_Passwords
                    .Where(a => a.intContactID == clientId)
                    .Select(x => new AlunoSenhaDTO
                    {
                        Id = x.intID,
                        AplicacaoId = x.intAplicacaoId ?? 0,
                        Senha = x.txtPassword,
                        ClientId = x.intContactID,
                        Data = x.dteDatePassword
                    }).FirstOrDefault();

                return alunoSenha;
            }
        }

        public int InserirAlunoSenha(AlunoSenhaDTO alunoSenha)
        {
            using (var ctx = new DesenvContext())
            {
                var person = new tblPersons_Passwords();
                person.intContactID = alunoSenha.ClientId;
                person.txtPassword = alunoSenha.Senha;
                person.dteDatePassword = DateTime.Now;
                person.intChave = null;
                person.dteDataLimite = null;
                person.intAplicacaoId = alunoSenha.AplicacaoId;
                ctx.tblPersons_Passwords.Add(person);
                return ctx.SaveChanges();
            }
        }

        public int AlterarAlunoSenha(AlunoSenhaDTO alunoSenha)
        {
            using (var ctx = new DesenvContext())
            {
                var personPassword = ctx.tblPersons_Passwords.FirstOrDefault(x => x.intID == alunoSenha.Id);
                personPassword.txtPassword = alunoSenha.Senha;
                personPassword.dteDatePassword = DateTime.Now;
                personPassword.intAplicacaoId = alunoSenha.AplicacaoId;
                return ctx.SaveChanges();
            }
        }     

        public Cliente GetClient(Int32 ClientID)
        {
            var ctx = new DesenvContext();
            var query = (from p in ctx.tblPersons
                         join c in ctx.tblClients
                             on p.intContactID equals c.intClientID
                         where p.intContactID == ClientID
                         select new
                         {
                             p.txtName,
                             p.txtEmail1,
                             p.txtRegister
                         }).FirstOrDefault();
            Cliente cliente = new Cliente();
            cliente.ID = ClientID;
            if (query != null)
            {
                cliente.Nome = query.txtName;
                cliente.Email = query.txtEmail1;
                cliente.Register = query.txtRegister;
            }
            return cliente;
        }
    }
}