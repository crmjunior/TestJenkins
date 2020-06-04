using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_API.Academico;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using StackExchange.Profiling;
using MedCore_DataAccess.Util;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO.Base;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;

namespace MedCore_DataAccess.Repository
{
    public class AlunoEntity : IAlunoEntity
    {
        public int? GetAnoProva(int idProva)
        {
            using(MiniProfiler.Current.Step("Obtendo ano de prova"))
            {
                using (var ctx = new DesenvContext())
                {
                    return (from cp in ctx.tblConcurso_Provas
                            join c in ctx.tblConcursos on cp.ID_CONCURSO equals c.intConcursoID
                            where cp.intProvaID == idProva
                            select c.intAno).FirstOrDefault();
                }
            }
        }

        public int? GetConcursoIDByProvaId(int provaId)
        {
            using(MiniProfiler.Current.Step("Obtendo ID de concurso por ID prova"))
            {
                using (var ctx = new DesenvContext())
                {
                    return Convert.ToInt32(ctx.tblConcurso_Provas.Where(c => c.intProvaID == provaId)
                        .FirstOrDefault()
                        .ID_CONCURSO);
                }
            }
        }

        public int SetAutorizacaoTrocaDispositivo(SegurancaDevice device)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    ctx.tblSeguranca.Add(new tblSeguranca
                    {
                        intApplicationId = device.IdAplicacao,
                        dteCadastro = DateTime.Now,
                        intClientId = device.Matricula,
                        txtDeviceToken = device.Token,
                        intDeviceId = device.IdDevice
                    });
                    ctx.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool SetMedsoftScreenshotReport(SegurancaDevice seguranca)
        {
            try
            {
                //lancamento fim de ano 
                //por enquanto somente medsoft ionic
                var medsoftMobileId = 17;
                var deviceId = string.Empty;

                try
                {
                    deviceId = seguranca.IdDevice.ToString();
                }
                catch (Exception) { }

                using (var ctx = new DesenvContext())
                {
                    ctx.tblMedsoftScreenshotReport.Add(new tblMedsoftScreenshotReport
                    {
                        dteCriacao = DateTime.Now,
                        intApplicationId = medsoftMobileId,
                        intClientID = seguranca.Matricula,
                        txtDeviceID = deviceId,
                        intScreenshotCounter = seguranca.ScreenshotCounter
                    });
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public Generica GetEspecialidadeAluno(int matricula)
        {
            using(MiniProfiler.Current.Step("Obtendo especialidade aluno"))
            {
                using (var ctx = new DesenvContext())
                {
                    using (var ctxAcad = new AcademicoContext())
                    {
                        int? especialidadeID;
                        try
                        {
                            especialidadeID = ctx.tblClients.Where(a => a.intClientID == matricula).FirstOrDefault().intEspecialidadeID;
                        }
                        catch 
                        {
                            return new Generica();
                        }

                        return new Generica
                        {
                            Valor = especialidadeID,
                            Descricao = ctxAcad.tblEspecialidades.Where(e => e.intEspecialidadeID == especialidadeID)
                                    .FirstOrDefault()
                                    .DE_ESPECIALIDADE
                        };
                    }
                }
            }
        }

        public List<AlunoEspecialidadeDTO> GetEspecialiddesByConcursoIdAnoProva(int? concursoID, int? anoProva)
        {
            using (var ctx = new DesenvContext())
            {
                using (var ctxAcad = new AcademicoContext())
                {
                    using(MiniProfiler.Current.Step("Obtendo especialidades por ID de concurso e ano da prova"))
                    {           
                        var concursos = (from c in ctx.tblConcursos
                                        join cv in ctx.tblConcursosVagas on c.intConcursoID equals cv.intConcursoID
                                        where c.intConcursoID == concursoID
                                            && c.intAno == anoProva
                                        select new
                                        {
                                            intConcursoID = c.intConcursoID,
                                            intAno = c.intAno,
                                            intEspecialidadeID = cv.intEspecialidadeID,
                                            txtDescription = c.txtDescription
                                        }).Distinct().ToList();

                        List<int?> listaEspecialidadeId = concursos.Select(x => x.intEspecialidadeID).ToList();

                        var especialidade = (from e in ctxAcad.tblEspecialidades
                                            where listaEspecialidadeId.Contains(e.intEspecialidadeID)
                                            select new
                                            {
                                                e.intEspecialidadeID,
                                                e.DE_ESPECIALIDADE
                                            }).ToList();


                        return (from c in concursos
                                join e in especialidade on c.intEspecialidadeID equals e.intEspecialidadeID
                                where c.intConcursoID == concursoID
                                    && c.intAno == anoProva
                                select new AlunoEspecialidadeDTO
                                {
                                    intEspecialidadeID = c.intEspecialidadeID,
                                    DE_ESPECIALIDADE = e.DE_ESPECIALIDADE,
                                    txtDescription = c.txtDescription
                                }).Distinct().ToList();
                    }   
                }
            }
        }

        public double? GetNotadeCorteAnoPosteriorByNmConcursoAnoProvaEspecialideID(string txtDescription, int? anoProva, int? intEspecialidadeID)
        {
            using (var ctx = new DesenvContext())
            {
                using(MiniProfiler.Current.Step("Obtendo Nota de corte ano posterior por nome do concurso, ano da prova e ID de especialidade"))
                {
                    return (from c in ctx.tblConcursos
                            join cv in ctx.tblConcursosVagas on c.intConcursoID equals cv.intConcursoID
                            where
                                c.intAno == (anoProva + 1)
                                && cv.intEspecialidadeID == intEspecialidadeID
                                && !string.IsNullOrEmpty(c.txtDescription)
                                && !string.IsNullOrEmpty(txtDescription)
                                && c.txtDescription.Trim() == txtDescription.Trim()
                            select cv.dblNotaCorte).FirstOrDefault();
                }
            }
        }

        public int GetTotalQuestoesByProvaId(int provaId)
        {
            using (var ctx = new DesenvContext())
            {
                using(MiniProfiler.Current.Step("Obtendo total de questões pelo ID da prova"))
                {
                    return ctx.tblConcursoQuestoes.Where(a => a.intProvaID == provaId).Select(b => new {enunciado = b.txtEnunciado}).Distinct().Count();
                }
            }
        }

        public bool IsExAlunoTodosProdutos(int matricula)
        {
            var anoAtual = MedCore_DataAccess.Util.Utilidades.GetYear();
            var alunoMeioDeAno = Utilidades.IsCicloCompletoNoMeioDoAno(matricula);

            using (var ctx = new DesenvContext())
            {
                var alunoAnoAtual = (from so in ctx.tblSellOrders
                                     join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                     join prod in ctx.tblProducts on sod.intProductID equals prod.intProductID
                                     join c in ctx.tblCourses on prod.intProductID equals c.intCourseID
                                     where so.intClientID == matricula
                                         && anoAtual == c.intYear
                                         && (so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                             || (so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                                 && alunoMeioDeAno
                                             )
                                         )
                                     select so).Distinct().Count() > 0;


                return !alunoAnoAtual;
            }
        }

        public ResponseDTO<AlunoMedsoft> GetAlunoMsCross(string register, int idaplicacao)
        {
            var response = new ResponseDTO<AlunoMedsoft>();
            response.Retorno = new AlunoMedsoft();

            try
            {
                var clienteEntity = new ClienteEntity();
                var funcionarioEntity = new FuncionarioEntity();
                var pessoaEntity = new PessoaEntity();

                var cliente = new Cliente();
                using(MiniProfiler.Current.Step("Obtendo dados do usuario"))
                {
                    cliente = clienteEntity.GetByFilters(new Cliente { Register = register },
                            aplicacao: (Aplicacoes)Convert.ToInt32(idaplicacao))
                        .FirstOrDefault();
                }


                if (cliente == null)
                {
                    response.Sucesso = false;
                    response.Mensagem = "Usuário não cadastrado.";
                    response.TipoErro = "CadastroInexistente";
                    return response;
                }

                if (cliente.RetornoStatus == Cliente.StatusRetorno.SemAcesso || cliente.RetornoStatus == Cliente.StatusRetorno.Cancelado)
                {
                    response.Sucesso = false;
                    response.Mensagem = cliente.MensagemRetorno;
                    response.TipoErro = cliente.TipoErro;
                    return response;
                }
                else if (!string.IsNullOrEmpty(cliente.MensagemRetorno)) //Le mensagem inadimplente.
                {
                    response.Sucesso = true;
                    response.Mensagem = cliente.MensagemRetorno;
                    response.TipoErro = cliente.TipoErro;
                }

                var produtosPermitidos = new List<Produto.Produtos>
                {
                    Produto.Produtos.MEDCURSO,
                    Produto.Produtos.MEDEAD,
                    Produto.Produtos.MED,
                    Produto.Produtos.MEDCURSOEAD,
                    Produto.Produtos.MEDELETRO,
                    Produto.Produtos.INTENSIVAO,
                    Produto.Produtos.CPMED,
                    Produto.Produtos.ADAPTAMED,
                    Produto.Produtos.RAC_IMED,
                    Produto.Produtos.RACIPE_IMED,
                };

                if (idaplicacao == (int)Aplicacoes.MsProDesktop)
                {
                    produtosPermitidos = new List<Produto.Produtos>
                    {
                        Produto.Produtos.MEDCURSO,
                        Produto.Produtos.MEDEAD,
                        Produto.Produtos.MED,
                        Produto.Produtos.MEDCURSOEAD,
                        Produto.Produtos.INTENSIVAO,
                        Produto.Produtos.RAC,
                        Produto.Produtos.RACIPE
                    };
                }

                var produtosContradados = new List<Produto>();
                using(MiniProfiler.Current.Step("Obtendo produtos contratados"))
                {
                    produtosContradados = ProdutoEntity.GetProdutosContratadosPorAno(cliente.ID, aplicacaoId: idaplicacao);
                }
                var anoLetivoAtual = Utilidades.GetYear();
                var anoSeguinte = anoLetivoAtual + 1;
                var anoAnterior = anoLetivoAtual - 1;
                var anoAnteriorAntesDataLimite = Utilidades.IsAntesDatalimite(anoAnterior, 17);

                //Alunos com OV a partir do ano configurado, tem direito a acessar o Medsoft
                var anoDireitoVitalicio = Convert.ToInt32(ConfigurationProvider.Get("Settings:anoComDireitoVitalicio"));
                var anosPermitidos = new List<int>();

                for (var ano = anoDireitoVitalicio; ano <= anoSeguinte; ano++)
                {
                    anosPermitidos.Add(ano);
                }

                if (anoAnteriorAntesDataLimite) anosPermitidos.Add(anoAnterior);

                var hasPermitidos = produtosContradados
                    .Any(p => produtosPermitidos.Contains((Produto.Produtos)p.IDProduto) && anosPermitidos.Contains(p.Ano.Value));

                if (!hasPermitidos)
                {
                    response.Sucesso = false;
                    response.Mensagem = "Aluno sem produtos contratados.";
                    response.TipoErro = "SemProdutosContratados";
                    return response;
                }

                using (var ctx = new DesenvContext())
                {
                    var catalogModulosValidos = new List<ModuloMedsoft>();
                    catalogModulosValidos.Add(new ModuloMedsoft { Id = 1, Descricao = "Simulado" });
                    catalogModulosValidos.Add(new ModuloMedsoft { Id = 5, Descricao = "Monta Provas" });
                    catalogModulosValidos.Add(new ModuloMedsoft { Id = 9, Descricao = "Concursos na Íntegra" });
                    catalogModulosValidos.Add(new ModuloMedsoft { Id = 10, Descricao = "Questões de Apostila" });
                    catalogModulosValidos.Add(new ModuloMedsoft { Id = 11, Descricao = "Aula de Revisão" });
                    var modulos = ctx.Set<msp_Medsoft_SelectModulosPermitidos_Result>().FromSqlRaw("msp_Medsoft_SelectModulosPermitidos @intClientId = {0}, @intReleaseID = {1}", cliente.ID, 90).ToList()
                        .Where(m => m.intAtivo == 1)
                        .Where(m => catalogModulosValidos.Select(c => c.Id).Contains(m.intModuloID))
                        .Select(m => new ModuloMedsoft
                        {
                            Id = m.intModuloID,
                            Descricao = catalogModulosValidos.FirstOrDefault(o => o.Id == m.intModuloID).Descricao
                        })
                        .ToList();

                    var golden = clienteEntity.UserGolden(cliente.Register, Aplicacoes.MsProMobile);


                    var aluno = new AlunoMedsoft
                    {
                        ID = cliente.ID,
                        Nome = cliente.Nome,
                        NickName = cliente.NickName,
                        Register = cliente.Register,
                        Senha = cliente.Senha,
                        Modulos = modulos,
                        Foto = cliente.Foto,
                        FotoPerfil = cliente.FotoPerfil,
                        IsGolden = golden > 0
                    };

                    if (aluno.Senha == string.Empty)
                    {
                        response.Sucesso = false;
                        response.Mensagem = "Para acessar o MEDSOFT, você deverá informar uma senha, além do seu CPF/Passaporte." +
                                        " Por favor, cadastre uma senha agora, confirmando-a em seguida.<br><br>\n\r" +
                                        "IMPORTANTE:<br>\n\r" +
                                        "Seu login é único e intransferível e só pode ser realizado através do seu CPF e SENHA.\n\r<br><br>" +
                                        "Acessos simultâneos (2 ou mais usuários ao mesmo tempo e com o mesmo login) serão identificados pelo sistema e " +
                                        "interpretados como fraude. Neste caso, o acesso ao sistema será automaticamente cancelado.<br><br>\n\r" +
                                        "Atenciosamente,<br>\n\r" +
                                        "MEDGRUPO.";
                        response.TipoErro = "SemSenhaCadastrada";
                    }
                    else
                    {
                        response.Sucesso = true;
                        response.Retorno = aluno;
                    }

                    return response;
                }
            }
            catch
            {
                response.Sucesso = false;
                response.Mensagem = "Este serviço está indisponível no momento. Tente novamente, em breve.";
                response.TipoErro = "Erro500";
                return response;
            }
        }

        public int SetAceiteTermosPermissaoInadimplencia(PermissaoInadimplencia aceiteTermo)
        {
            try
            {
                SetChamadoInadimplencia(aceiteTermo);
                return 1;

            }
            catch
            {

                throw;
            }
        }

        public int SetChamadoInadimplencia(PermissaoInadimplencia aceiteTermo)
        {
            using(MiniProfiler.Current.Step("Criando chamado de inadimplencia"))
            {
                try
                {
                    if (aceiteTermo.LstIdOrdemDeVenda == null)
                        aceiteTermo.LstIdOrdemDeVenda = new List<int> { aceiteTermo.IdOrdemDeVenda };

                    aceiteTermo.LstIdOrdemDeVenda.RemoveAll(x => x <= 0);

                    foreach (var id in aceiteTermo.LstIdOrdemDeVenda)
                    {
                        ChamadoCallCenterEntity chamadosCallCenter = new ChamadoCallCenterEntity();
                        var haschamado = chamadosCallCenter.ExisteChamadoInadimplenciaTermoAceiteAberto(aceiteTermo.Matricula, aceiteTermo.IdOrdemDeVenda.ToString());

                        string cursosContratados;
                        using (var ctx = new DesenvContext())
                        {
                            cursosContratados = ctx.tblSellOrders.Where(x => x.intOrderID == id).Select(y => y.txtComment).FirstOrDefault();

                            if (haschamado) return 0;

                            var registro = new ChamadoCallCenter
                            {
                                DataAbertura = DateTime.Now,
                                IdCliente = aceiteTermo.Matricula,
                                Status = 1,
                                IdGrupoChamado = Constants.INADIMPLENCIA_CHAMADO_GRUPO,
                                IdCategoria = Constants.INADIMPLENCIA_CHAMADO_CATEGORIA,
                                Assunto = String.Concat("Aviso Eletrônico Visualizado OV: ", id),
                                Detalhe = String.Concat("Registro de Informação. Produto(s): ", cursosContratados),
                                Arquivo = null,
                                Gravidade = 2,
                                Notificar = false,
                                DiaSolucao = null,
                                IdSetor = -1,
                                IdStatusInterno = Constants.INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA,
                                AbertoPorIdFuncionario = Constants.IdEmployeeChamado,
                                DataPrevista1 = Utilidades.GetServerDate(-1),
                                DataPrevista2 = Utilidades.GetServerDate(-1).AddDays(6),
                                IdCurso = -1,
                                IdDepartamentoOrigem = 4,

                            };
                            var idChamado = chamadosCallCenter.InsertGenerico(registro);

                            chamadosCallCenter.SetOvInadimplencia(idChamado, id, aceiteTermo.IdAplicacao);

                            ctx.SaveChanges();
                        }

                    }

                    return 1;
                }

                catch
                {
                    throw;
                }
            }
        }

        public int SetDeviceToken(DeviceToken deviceToken)
        {
            using(MiniProfiler.Current.Step("Criando ou atualizando registro de device"))
            {
                if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeySetDeviceToken))
                    return RedisCacheManager.GetItemObject<int>(RedisCacheConstants.DadosFakes.KeySetDeviceToken);

                using (var ctx = new DesenvContext())
                {
                    var obj = new tblDeviceToken()
                    {
                        dteDataCriacao = DateTime.Now,
                        intClientID = deviceToken.Register,
                        txtOneSignalToken = deviceToken.Token,
                        bitIsTablet = deviceToken.bitIsTablet,
                        bitAtivo = true,
                        intApplicationId = deviceToken.AplicacaoId == default(int)
                            ? (int)Aplicacoes.MsProMobile : deviceToken.AplicacaoId
                    };

                    var existingObj = ctx.tblDeviceToken.OrderByDescending(x => x.intID)
                                                        .FirstOrDefault(x => x.intClientID == deviceToken.Register
                                                        && ((x.intApplicationId == null && obj.intApplicationId == (int)Aplicacoes.MsProMobile)
                                                            || (x.intApplicationId.HasValue && x.intApplicationId.Value == obj.intApplicationId.Value))
                                                        && (x.bitIsTablet == deviceToken.bitIsTablet || x.bitIsTablet == null));

                    if (existingObj == null || existingObj.txtOneSignalToken != deviceToken.Token)
                    {
                        ctx.tblDeviceToken.Add(obj);
                        if (existingObj != null)
                            existingObj.bitAtivo = false;
                    }
                    else
                    {
                        existingObj.bitAtivo = obj.bitAtivo;
                        existingObj.bitIsTablet = obj.bitIsTablet;
                    }

                    return ctx.SaveChanges();
                }
            }
        }

        public bool AlunoPossuiDireitoVitalicio(int matricula)
        {
            var ctx = new DesenvContext();
            var anoAtual = Utilidades.GetYear();
            var alunoMeioDeAno = Utilidades.IsCicloCompletoNoMeioDoAno(matricula);
            var alunoMeioDeAnoAnosAnteriores = Utilidades.CicloCompletoAnosAnterioresNoMeioDoAno(matricula);
            var anoDireitoVitalicio = Convert.ToInt32(ConfigurationProvider.Get("Settings:anoComDireitoVitalicio"));
            var alunoR3 = new PerfilAlunoEntity().IsAlunoR3(matricula);
            var aluonImed = new PerfilAlunoEntity().IsAlunoMedEletroIMed(matricula);

            //Se a chave no Web.config não existir, considerar o ano vitalício a partir de 2018
            if (anoDireitoVitalicio == 0) anoDireitoVitalicio = 2018;

            var query = (from so in ctx.tblSellOrders
                         join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                         join pro in ctx.tblProducts on sod.intProductID equals pro.intProductID
                         join cl in ctx.tblCourses on pro.intProductID equals cl.intCourseID
                         where so.intClientID == matricula
                             && cl.intYear >= anoDireitoVitalicio
                             && (so.intStatus == (int)Utilidades.ESellOrderStatus.Ativa
                                 || (anoAtual == cl.intYear
                                     && so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                     && alunoMeioDeAno
                                 )
                                 || (so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                     && alunoMeioDeAnoAnosAnteriores.Contains(cl.intYear.Value)
                                 )
                                 || (so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                     && alunoR3
                                 )
                                 || (so.intStatus == (int)Utilidades.ESellOrderStatus.Cancelada
                                     && aluonImed
                                 )
                                 || (so.intStatus == (int)Utilidades.ESellOrderStatus.Pendente 
                                    && (pro.intProductGroup1 == (int)Produto.Produtos.MED_MASTER || pro.intProductGroup1 == (int)Produto.Produtos.CPMED_EXTENSIVO)
                                 )
                             )
                         select so).Distinct();

            return query.Count() > 0;
        }

        public PermissaoInadimplencia RemoveOvInadimplenteBloqueado(PermissaoInadimplencia permissoes)
        {
            if (permissoes.LstOrdemVendaMsg.Any(x => x.PermiteAcesso == (int)Utilidades.TipoMensagemInadimplente.PermiteAcesso))
            {
                permissoes.LstOrdemVendaMsg = permissoes.LstOrdemVendaMsg.Where(x => x.PermiteAcesso == (int)Utilidades.TipoMensagemInadimplente.PermiteAcesso).ToList();

                if (!permissoes.LstOrdemVendaMsg.Any(x => x.IdOrdemDeVenda == permissoes.IdOrdemDeVenda && x.PermiteAcesso == (int)Utilidades.TipoMensagemInadimplente.PermiteAcesso))
                {
                    permissoes.IdOrdemDeVenda = 0;
                    permissoes.Mensagem = "";
                }
            }

            return permissoes;
        }        

        public PermissaoInadimplencia GetPermissao(string registro, int idAplicacao, bool geraChamado = true,
            int ClientId = 0)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetPermissao))
                return RedisCacheManager.GetItemObject<PermissaoInadimplencia>(RedisCacheConstants.DadosFakes.KeyGetPermissao);

            try
            {
                //TODO: MedReader ano anterior. Nao verificar Inativo quando o MedReader for lançado no medsoft pro(2018).
                //var mobile = new int[] { 5, 6, 7, 8, 9 };
                var retorno = new PermissaoInadimplencia { PermiteAcesso = 1, Mensagem = string.Empty };
                retorno = GetPermissaoInativo(idAplicacao, registro);
                var isInativo = (retorno.PermiteAcesso == (int)PermissaoInadimplencia.StatusAcesso.Negado);
                var exAlunoMedeletro = idAplicacao == (int)Aplicacoes.MEDELETRO && IsExAlunoMedeletro(registro);
                if (isInativo && !exAlunoMedeletro)
                    return retorno;

                return (Aplicacoes)idAplicacao == Aplicacoes.AreaRestrita
                    ? GetPermissaoInadimplenciaRestrita(idAplicacao, geraChamado, registro)
                    : GetPermissaoInadimplencia(idAplicacao, geraChamado, registro);
            }
            catch (Exception ex)
            {
                return new PermissaoInadimplencia { Mensagem = ex.Message };
            }
        }

        public PermissaoInadimplencia GetPermissaoInadimplencia(int idAplicacao, bool geraChamado, string registro)
        {
            var aplicacoesServico = Utilidades.AplicacoesServico();
            var isAplicacaoServico = aplicacoesServico.Contains(idAplicacao);

            var _idOrdemDeVendaInvalidaParaInadimplencia = -1;

            //var mobile = new int[] { 5, 6, 7, 8, 9 };
            var retorno = new PermissaoInadimplencia { PermiteAcesso = 1, Mensagem = string.Empty };
            retorno.LstOrdemVendaMsg = new List<PermissaoAcessoItem>();
            if (!Utilidades.IsActiveFunction(Utilidades.Funcionalidade.AlunoInadimplente)) return retorno;

            if (idAplicacao == (int)Aplicacoes.MEDELETRO && IsExAlunoMedeletro(registro))
            {
                retorno.PermiteAcesso = (int)PermissaoInadimplencia.StatusAcesso.ExAluno;
            }

            using (var ctx = new DesenvContext())
            {

                var aluno = ctx.tblPersons.Where(a => a.txtRegister == registro).FirstOrDefault();
                PermissaoAcessoItem permissaoItem;
                var proc = new DBQuery().ExecuteStoredProcedure(
                    "msp_OperacoesControleFluxo_Carga_MensagemInadimplenciaAplicativo_desenvProducao",
                    new SqlParameter[]
                    {
                        new SqlParameter("@intClientID", aluno.intContactID),
                        new SqlParameter("@intAplicativoID", idAplicacao)
                    });


                /*
				    --1 - O alunoDetalhes poderá acessar, sem mostrar mensagem alguma (casos: pendente, adimplente, carencia, possui chamado dentro do prazo, possui chamado com prazo extra dentro do prazo) 
				    -- segundo Pedro se não tiver ov tem que retornar 1 porque ele vai jogar para a tela de login.  
				    --2 - O alunoDetalhes poderá acessar somente se clicar em ok para gerar o chamado (casos:  inadimplente ou Inadimplente meses anteriores sem chamado)  
				    --3 - O alunoDetalhes não poderá acessar de forma alguma (casos:  O alunoDetalhes está inadimplente, e já tem chamado, com prazo normal ou extra vencidos)  
				*/

                if (proc.Tables.Count > 0)
                {
                    for (int i = 0; i < proc.Tables[0].Rows.Count; i++)
                    {
                        var IdOrdemDeVenda = Convert.ToInt32(proc.Tables[0].Rows[i]["IntOrderID"].ToString());
                        var intMensagemID = Convert.ToInt32(proc.Tables[0].Rows[i]["intMensagemID"]);
                        int tipoMsg = RetornaTipoMensagem(IdOrdemDeVenda);

                        var mensagem = "";
                        if (intMensagemID == (int)Utilidades.TipoMensagemInadimplente.PermiteAcessoComTermoDeInadimplencia)
                        {
                            mensagem = RetornarMensagemAcordo(tipoMsg, aluno.txtName.Trim());
                        }
                        else if (intMensagemID == (int)Utilidades.TipoMensagemInadimplente.BloqueadoPorInadimplencia)
                        {
                            mensagem = RetornarMensagemBloqueio(tipoMsg, aluno.txtName.Trim());
                        }

                        switch (intMensagemID)
                        {
                            case (int)Utilidades.TipoMensagemInadimplente.PermiteAcesso:
                            case (int)Utilidades.TipoMensagemInadimplente.PermiteAcessoInadimplencia:
                                permissaoItem = new PermissaoAcessoItem();
                                permissaoItem.IdOrdemDeVenda = 0;
                                permissaoItem.Mensagem = "";
                                permissaoItem.PermiteAcesso = (int)Utilidades.TipoMensagemInadimplente.PermiteAcesso;

                                retorno.LstOrdemVendaMsg.Add(permissaoItem);
                                break;
                            // LE MENSAGEM DE INADIMPLENCIA                   
                            case (int)Utilidades.TipoMensagemInadimplente.PermiteAcessoComTermoDeInadimplencia:
                                retorno.Mensagem = GetMensagemInadimplenciaTratada(mensagem, idAplicacao);
                                retorno.IdOrdemDeVenda = IdOrdemDeVenda;

                                permissaoItem = new PermissaoAcessoItem();
                                permissaoItem.IdOrdemDeVenda = IdOrdemDeVenda;
                                permissaoItem.Mensagem = mensagem;
                                permissaoItem.PermiteAcesso = (int)Utilidades.TipoMensagemInadimplente.PermiteAcesso;

                                retorno.LstOrdemVendaMsg.Add(permissaoItem);

                                if (geraChamado)
                                {
                                    ctx.tblPermissaoInadimplenciaLogAlerta.Add(new tblPermissaoInadimplenciaLogAlerta
                                    {
                                        intClientId = aluno.intContactID,
                                        intApplicationId = idAplicacao,
                                        dteCadastro = DateTime.Now
                                    });
                                    ctx.SaveChanges();
                                }
                                break;

                            // BLOQUEADO
                            case (int)Utilidades.TipoMensagemInadimplente.BloqueadoPorInadimplencia:
                                var alunoPossueProdutoR3 = AlunoPossuiProdutoR3(IdOrdemDeVenda);
                                var alunoPossueProdutoMedEletroIMed = AlunoPossuiProdutoMedEletroIMed(IdOrdemDeVenda);
                                var alunoPossueProdutoMedMaster = AlunoPossuiProdutoMedMaster(IdOrdemDeVenda);

                                retorno.Mensagem = GetMensagemInadimplenciaTratada(mensagem, idAplicacao);
                                retorno.IdOrdemDeVenda = IdOrdemDeVenda;
                                retorno.PermiteAcesso = (alunoPossueProdutoR3 || alunoPossueProdutoMedEletroIMed || alunoPossueProdutoMedMaster) && !isAplicacaoServico ? (int)Utilidades.TipoMensagemInadimplente.PermiteAcesso : (int)Utilidades.TipoMensagemInadimplente.SemAcesso;

                                permissaoItem = new PermissaoAcessoItem();
                                permissaoItem.IdOrdemDeVenda = IdOrdemDeVenda;
                                permissaoItem.Mensagem = mensagem;

                                if ((tipoMsg == (int)Utilidades.MensagemInadimplente.R3 ||
                                   tipoMsg == (int)Utilidades.MensagemInadimplente.IMed ||
                                   tipoMsg == (int)Utilidades.MensagemInadimplente.MedMaster ) && !isAplicacaoServico)
                                {
                                    var isChamadoAberto = new ChamadoCallCenterEntity().ExisteChamadoInadimplenciaPrimeiroAvisoAberto(aluno.intContactID, IdOrdemDeVenda);
                                    if (isChamadoAberto)
                                    {
                                        permissaoItem.IdOrdemDeVenda = _idOrdemDeVendaInvalidaParaInadimplencia;
                                    }

                                    permissaoItem.PermiteAcesso = (int)Utilidades.TipoMensagemInadimplente.PermiteAcesso;
                                }
                                else
                                {
                                    permissaoItem.PermiteAcesso = (int)Utilidades.TipoMensagemInadimplente.SemAcesso;
                                }

                                retorno.LstOrdemVendaMsg.Add(permissaoItem);
                                break;

                            default:
                                break;
                        }
                        retorno = TrataInadimplenciaMedeletro(aluno, idAplicacao, retorno);

                    }
                }

                if (retorno.LstOrdemVendaMsg != null)
                {
                    if (retorno.Mensagem == string.Empty &&
                        retorno.LstOrdemVendaMsg.Where(x => x.Mensagem != "").Any())
                    {
                        retorno.Mensagem = retorno.LstOrdemVendaMsg.Where(x => x.Mensagem != "").FirstOrDefault().Mensagem;
                    }

                    retorno.PermiteAcesso = retorno.LstOrdemVendaMsg.Where(x => x.PermiteAcesso == 1).Any() == true ? 1 : 0;
                }

                return retorno;
            }
        }

        private PermissaoInadimplencia TrataInadimplenciaMedeletro(tblPersons aluno, int idAplicacao,
            PermissaoInadimplencia retorno)
        {
            var permissao = retorno;
            var isAppMEDELETRO = (int)Aplicacoes.MEDELETRO == idAplicacao;
            var isMedreader = (int)Aplicacoes.LeitordeApostilas == idAplicacao;
            var visualizaMensagemMedeletro = new[]
                                             {
                                                 (int) Aplicacoes.LeitordeApostilas,
                                                 (int) Aplicacoes.MEDELETRO
                                             };
            var OrdensDeVendaDoAluno = new OrdemVendaEntity()
                .GetOrdensVenda(aluno.intContactID,
                    new[] { Utilidades.GetAnoInscricao(Aplicacoes.INSCRICAO_MEDELETRO), Utilidades.GetYear() }.ToList(), 0, 0)
                .Where(o => o.Status == OrdemVenda.StatusOv.Ativa);

            bool isAdimplenteExtensivo = OrdensDeVendaDoAluno.Where(
                    o => (o.GroupID == 1 || o.GroupID == 5 || o.GroupID == 8 || o.GroupID == 9)
                         && (o.Status2 == OrdemVenda.StatusOv.Adimplente || o.Status2 == OrdemVenda.StatusOv.Inadimplente ||
                             o.Status2 == OrdemVenda.StatusOv.Carencia)
                         && o.Year == Utilidades.GetYear())
                .Any();
            bool isInadimplenteExtensivo = OrdensDeVendaDoAluno
                .Where(o => (o.GroupID == 1 || o.GroupID == 5 || o.GroupID == 8 || o.GroupID == 9)
                            && (o.Status2 == OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
                            && (o.Year == Utilidades.GetYear() || Utilidades.IsAntesDatalimite(o.Year, idAplicacao)))
                .Any();

            bool IsInadimplenteMedEletro = OrdensDeVendaDoAluno.Where(o => (o.GroupID == 57)
                                                                           &&
                                                                           (o.Status2 == OrdemVenda.StatusOv.Inadimplente ||
                                                                            o.Status2 == OrdemVenda.StatusOv
                                                                                .Inadimplente_MESES_ANTERIORES))
                .Any();



            string mensagemMedeletro = string.Empty;
            using (var ctx1 = new DesenvContext())
            {
                var idmensagem = permissao.PermiteAcesso == 0 ? 21 : 18;
                mensagemMedeletro = GetMensagemInadimplenciaTratada(
                    ctx1.tblAvisos.Where(x => x.intAvisoID == idmensagem)
                        .FirstOrDefault()
                        .txtAviso.Replace("{0}", aluno.txtName.Trim()), idAplicacao);
            }

            if (isAdimplenteExtensivo && !isAppMEDELETRO)
            {
                permissao.PermiteAcesso = 1;
                if (!isMedreader)
                {
                    permissao.Mensagem = string.Empty;
                    return permissao;
                }

            }


            if (IsInadimplenteMedEletro)
            {

                if ((!isInadimplenteExtensivo || isAppMEDELETRO) && !string.IsNullOrEmpty(permissao.Mensagem))
                {
                    permissao.Mensagem = mensagemMedeletro;
                }

                return permissao;
            }
            return permissao;
        }

        public bool AlunoPossuiProdutoR3(int idOV)
        {
            var produtosR3 = Utilidades.ProdutosR3();
            var anoAtual = Utilidades.GetYear();
            var ctx = new DesenvContext();

            var matricula = (from so in ctx.tblSellOrders
                             where so.intOrderID == idOV
                             select so.intClientID
                             ).FirstOrDefault();

            bool isR3 = (from so in ctx.tblSellOrders
                         join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                         join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                         join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                         join ps in ctx.tblPersons on so.intClientID equals ps.intContactID
                         where produtosR3.Contains(p.intProductGroup2 ?? 0)
                          && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                          && ((so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente)
                          || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Carencia)
                          || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente)
                          || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Cancelada)
                          || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES))
                          && so.intClientID == matricula
                         select
                             p.intProductID
                         ).Any();

            return isR3;
        }

        public bool AlunoPossuiProdutoMedEletroIMed(int idOV)
        {
            var produtosMedEletroImed = Produto.Produtos.MEDELETRO_IMED.GetHashCode();
            var anoAtual = Utilidades.GetYear();
            var ctx = new DesenvContext();

            var matricula = (from so in ctx.tblSellOrders
                             where so.intOrderID == idOV
                             select so.intClientID
                             ).FirstOrDefault();

            bool isMedEletroIMed = (from so in ctx.tblSellOrders
                                    join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                    join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                                    join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                    join ps in ctx.tblPersons on so.intClientID equals ps.intContactID
                                    where (p.intProductGroup2 ?? 0) == produtosMedEletroImed
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                    && ((so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente)
                                    || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Carencia)
                                    || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente)
                                    || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Cancelada)
                                    || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES))
                                    && so.intClientID == matricula
                                    select
                                        p.intProductID
                                    ).Any();

            return isMedEletroIMed;
        }

        public bool AlunoPossuiProdutoMedMaster(int idOV)
        {
            var produtosMedMaster = Produto.Produtos.MED_MASTER.GetHashCode();
            var anoAtual = Utilidades.GetYear();
            using (var ctx = new DesenvContext())
            {
                var matricula = (from so in ctx.tblSellOrders
                                 where so.intOrderID == idOV
                                 select so.intClientID
                              ).FirstOrDefault();

                bool isMedMaster = (from so in ctx.tblSellOrders
                                    join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                    join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                                    join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
                                    join ps in ctx.tblPersons on so.intClientID equals ps.intContactID
                                    where (p.intProductGroup2 ?? 0) == produtosMedMaster
                                    && so.intStatus == (int)OrdemVenda.StatusOv.Ativa
                                    && ((so.intStatus2 == (int)OrdemVenda.StatusOv.Adimplente)
                                    || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Pendente)
                                    || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Carencia)
                                    || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente)
                                    || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Cancelada)
                                    || (c.intYear == anoAtual && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES))
                                    && so.intClientID == matricula
                                    select
                                        p.intProductID
                                        ).Any();

                return isMedMaster;
            }             
        }

        private string GetMensagemInadimplenciaTratada(string msg, int idAplicacao)
        {
            try
            {
                var msgTratada = msg;
                switch ((Aplicacoes)idAplicacao)
                {
                    case Aplicacoes.Recursos_Android:
                    case Aplicacoes.Recursos_iPad:
                    case Aplicacoes.Recursos_iPhone:
                    case Aplicacoes.LeitordeApostilas:
                    case Aplicacoes.MEDCODE:
                    case Aplicacoes.MEDELETRO:
                    case Aplicacoes.MEDSOFT_Android:
                    case Aplicacoes.MEDSOFTiPad:
                        msgTratada = Utilidades.RemoveHtml(msg).Replace("<br>", string.Empty).Replace("<br />", string.Empty);
                        break;

                    default:
                        break;
                }
                return msgTratada;
            }
            catch
            {
                throw;
            }

        }

        public string RetornarMensagemBloqueio(int tipoMsg, string nomeAluno)
        {
            var ctx = new DesenvContext();
            var msgDeBloqueio = (from pic in ctx.tblPermissaoInadimplenciaConfiguracao
                                 where pic.intId == tipoMsg
                                 select
                                      pic.txtMensagemBloqueio
                                      ).FirstOrDefault().Replace("{0}", nomeAluno.Trim());
            return msgDeBloqueio;
        }

        public string RetornarMensagemAcordo(int tipoMsg, string nomeAluno)
        {
            var ctx = new DesenvContext();
            var msgDeAcordo = (from pic in ctx.tblPermissaoInadimplenciaConfiguracao
                               where pic.intId == tipoMsg
                               select
                                    pic.txtMensagemDeAcordo
                                      ).FirstOrDefault().Replace("{0}", nomeAluno.Trim());
            return msgDeAcordo;
        }

        public bool IsOvR3(int idOV)
        {
            var produtosR3 = Utilidades.ProdutosR3();
            var ctx = new DesenvContext();
            var isR3 = (from sod in ctx.tblSellOrderDetails
                        join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                        where sod.intOrderID == idOV && produtosR3.Contains(p.intProductGroup1 ?? 0)
                        select
                           sod.intOrderID
                     ).Any();
            return isR3;
        }

        public bool IsOvIMed(int idOV)
        {
            var produtoIMED = (int)Utilidades.ProductGroups.MEDELETRO_IMED;
            var ctx = new DesenvContext();
            var isIMED = (from sod in ctx.tblSellOrderDetails
                          join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                          where sod.intOrderID == idOV && p.intProductGroup1 == produtoIMED
                          select
                             sod.intOrderID
                     ).Any();
            return isIMED;
        }

        public bool IsOvMedMaster(int idOV)
        {
            var produtoMedMaster = (int)Utilidades.ProductGroups.MedMaster;
            using (var ctx = new DesenvContext())
            {
                var isMedMaster = (from sod in ctx.tblSellOrderDetails
                                   join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                                   where sod.intOrderID == idOV && p.intProductGroup1 == produtoMedMaster
                                   select
                                      sod.intOrderID
                         ).Any();
                return isMedMaster;
            }   
        }

        public int RetornaTipoMensagem(int IdOrdemDeVenda)
        {
            var ovR3 = IsOvR3(IdOrdemDeVenda);
            var ovIMED = IsOvIMed(IdOrdemDeVenda);
            var ovMedMaster = IsOvMedMaster(IdOrdemDeVenda);
            int tipoMsg;

 
            if (ovR3)
            {
                tipoMsg = (int)Utilidades.MensagemInadimplente.R3;
            }
            else if (ovIMED)
            {
                tipoMsg = (int)Utilidades.MensagemInadimplente.IMed;
            }
            else if (ovMedMaster)
            {
                tipoMsg = (int)Utilidades.MensagemInadimplente.MedMaster;
            }
            else 
            {
                tipoMsg = (int)Utilidades.MensagemInadimplente.Extensivo;
            }
            return tipoMsg;
        }

        public PermissaoInadimplencia GetPermissaoInadimplenciaRestrita(int idAplicacao, bool geraChamado, string registro)
        {
            //var mobile = new int[] { 5, 6, 7, 8, 9 };
            var lstidsOV = new List<int>();
            var retorno = new PermissaoInadimplencia { PermiteAcesso = 1, Mensagem = string.Empty };
            if (!Utilidades.IsActiveFunction(Utilidades.Funcionalidade.AlunoInadimplente)) return retorno;


            using (var ctx = new DesenvContext())
            {

                var aluno = ctx.tblPersons.Where(a => a.txtRegister == registro).FirstOrDefault();
                var proc = new DBQuery().ExecuteStoredProcedure(
                    "msp_OperacoesControleFluxo_Carga_MensagemInadimplenciaAplicativo",
                    new SqlParameter[]
                    {
                        new SqlParameter("@intClientID", aluno.intContactID),
                        new SqlParameter("@intAplicativoID", idAplicacao)
                    });


                /*
				    --1 - O alunoDetalhes poderá acessar, sem mostrar mensagem alguma (casos: pendente, adimplente, carencia, possui chamado dentro do prazo, possui chamado com prazo extra dentro do prazo) 
				    -- segundo Pedro se não tiver ov tem que retornar 1 porque ele vai jogar para a tela de login.  
				    --2 - O alunoDetalhes poderá acessar somente se clicar em ok para gerar o chamado (casos:  inadimplente ou Inadimplente meses anteriores sem chamado)  
				    --3 - O alunoDetalhes não poderá acessar de forma alguma (casos:  O alunoDetalhes está inadimplente, e já tem chamado, com prazo normal ou extra vencidos)  
				*/
                switch (Convert.ToInt32(proc.Tables[0].Rows[0]["intMensagemID"]))
                {
                    // LE MENSAGEM DE INADIMPLENCIA
                    case 2:
                        retorno.Mensagem = GetMensagemInadimplenciaTratada(
                            ctx.tblPermissaoInadimplenciaConfiguracao.FirstOrDefault()
                                .txtMensagemDeAcordo.Replace("{0}", aluno.txtName.Trim()), idAplicacao);
                        lstidsOV.Add(Convert.ToInt32(proc.Tables[0].Rows[0]["IntOrderID"].ToString()));
                        retorno.LstIdOrdemDeVenda = lstidsOV;

                        if (geraChamado)
                        {
                            ctx.tblPermissaoInadimplenciaLogAlerta.Add(new tblPermissaoInadimplenciaLogAlerta
                            {
                                intClientId = aluno.intContactID,
                                intApplicationId = idAplicacao,
                                dteCadastro = DateTime.Now
                            });
                            ctx.SaveChanges();
                        }
                        break;

                    // BLOQUEADO
                    case 3:
                        retorno.Mensagem = GetMensagemInadimplenciaTratada(
                            ctx.tblPermissaoInadimplenciaConfiguracao.FirstOrDefault()
                                .txtMensagemBloqueio.Replace("{0}", aluno.txtName.Trim()), idAplicacao);
                        lstidsOV.Add(Convert.ToInt32(proc.Tables[0].Rows[0]["IntOrderID"].ToString()));
                        retorno.LstIdOrdemDeVenda = lstidsOV;
                        retorno.PermiteAcesso = 0;
                        break;

                    default:
                        break;
                }

                retorno = TrataInadimplenciaMedeletro(aluno, idAplicacao, retorno);


                return retorno;
            }
        }

        public bool IsExAlunoMedeletro(string registro)
        {
            using (var ctx = new DesenvContext())
            {
                var retorno = false;
                var ordens = new OrdemVendaEntity().GetOrdensVenda(registro.Trim(), 57);
                var anoAtual = DateTime.Now.Year;
                var isAlunoAtual = ordens.Where(a => a.Year == anoAtual).Any();
                var isExAluno = ordens.Where(a => a.Year != anoAtual).Any();

                if (!isAlunoAtual && isExAluno)
                {
                    retorno = true;
                }

                return retorno;
            }
        }

        private PermissaoInadimplencia GetPermissaoInativo(int idAplicacao, string registro)
        {
            //var mobile = new int[] { 5, 6, 7, 8, 9 };
            var retorno = new PermissaoInadimplencia { PermiteAcesso = 1, Mensagem = string.Empty };
            // ##########################################################################################################
            // ##########################################################################################################
            // ##########################################################################################################
            // RECURSO EMERGENCIAL (A ideia é reaproveitar esse método sem ter que republicar todos os apps. Temporário.)
            var ctxAluno = new DesenvContext();
            var aluno = ctxAluno.tblPersons.Where(p => p.txtRegister == registro);
            var matricula = aluno.Select(x => x.intContactID).FirstOrDefault();

            //Alunos com OV a partir de 2018, tem direito a acessar o Medsoft
            var alunoPossuiDireitoVitalicio = AlunoPossuiDireitoVitalicio(matricula);


            string[] lMaterialImpresso = ConfigurationProvider.Get("Settings:AppMaterialImpresso").Split(',');
            var isMaterialImpresso = lMaterialImpresso.Contains((idAplicacao).ToString());
            if (!isMaterialImpresso && idAplicacao != (int)Aplicacoes.AreaRestrita)
            {
                // Aluno Próximo Ano
                if ((Aplicacoes)idAplicacao != Aplicacoes.MsProMobile && (Aplicacoes)idAplicacao != Aplicacoes.MEDSOFT_PRO_ELECTRON && new ClienteEntity().IsAlunoProximoAnoInativoAnoAtual(matricula, (Aplicacoes)idAplicacao))
                {
                    var idAviso = 12; // tblAvisos
                    retorno.PermiteAcesso = (int)PermissaoInadimplencia.StatusAcesso.Negado;
                    retorno.Mensagem = MensagemEntity.GetAviso(idAviso, 0); // 0 = Todas as aplicações                    

                    MensagemEntity.SetLogAviso(matricula, idAviso, true);

                    return retorno;
                }

                // Aluno Cancelado

                if ((idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON || idAplicacao == (int)Aplicacoes.MsProMobile) && IsAlunoAtivoOuCancelado(matricula, idAplicacao) && !alunoPossuiDireitoVitalicio)
                {
                    return retorno;
                }
                else if (IsAlunoCancelado(matricula, idAplicacao) && !alunoPossuiDireitoVitalicio)
                {
                    var idAviso = 2; // tblAvisos
                    retorno.PermiteAcesso = (int)PermissaoInadimplencia.StatusAcesso.Negado;
                    retorno.Mensagem = MensagemEntity.GetAviso(idAviso, 0); // 0 = Todas as aplicações                    

                    MensagemEntity.SetLogAviso(matricula, idAviso, true);

                    return retorno;
                }
                // ##########################################################################################################
                // ##########################################################################################################
                // ##########################################################################################################

            }
            ctxAluno.Dispose();

            return retorno;
        }

        private bool IsAlunoAtivoOuCancelado(int idCliente, int aplicacaoId)
        {
            var ano = Utilidades.GetYear();
            var ctx = new DesenvContext();
            var dataLimite = ctx.Set<msp_GetDataLimite_ByApplication_Result>().FromSqlRaw("msp_GetDataLimite_ByApplication @intYear = {0}, @intAplicationID = {1}", ano - 1, aplicacaoId).ToList().FirstOrDefault().dteDataLimite ?? DateTime.MinValue;
            var isAntesDataLimite = dataLimite >= DateTime.Now;
            var anos = isAntesDataLimite ? new[] { ano, ano - 1 } : new[] { ano };
            var ordens = new OrdemVendaEntity().GetResumed(idCliente, anos.ToList());
            List<OrdemVenda> ordensValidas = (from o in ordens
                                              where (int)o.Status == (int)(int)Utilidades.ESellOrderStatus.Ativa
                                                  || (int)o.Status == (int)(int)Utilidades.ESellOrderStatus.Cancelada
                                              select o).ToList();

            return ordensValidas.Any();
        }

        private bool IsAlunoCancelado(int matricula, int idAplicacao)
        {
            var isCancelado = false;

            if (!Utilidades.IsActiveFunction(Utilidades.Funcionalidade.ValidaAlunoCancelado))
                return false;

            var isAtivo = new ClienteEntity().IsAlunoAtivoAnoAtual(matricula, (Aplicacoes)idAplicacao);
            isCancelado = !isAtivo;

            return isCancelado;
        }

        public string GetMensagensLogin(int idAplicacao, int intTipoMensagem)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.Aluno.KeyLoginGetMensagemLoginCache))
            {
                var Key = string.Format("{0}:{1}:{2}", RedisCacheConstants.Aluno.KeyLoginGetMensagemLoginCache, idAplicacao, intTipoMensagem);
                var ret = RedisCacheManager.GetItemObject<string>(Key);
                if (ret != null)
                    return ret;

                var mensagem = GetMensagensLoginDB(idAplicacao, intTipoMensagem);

                RedisCacheManager.SetItemObject(Key, mensagem, TimeSpan.FromDays(1));

                return mensagem;                
            }
            else
            {
                return GetMensagensLoginDB(idAplicacao, intTipoMensagem);
            }
        }

        public bool IsExAlunoTodosProdutosCache(int matricula)
        {
            var key = String.Format("{0}:{1}", RedisCacheConstants.Produtos.KeyIsExAlunoTodosProdutos, matricula);

            if (!RedisCacheManager.CannotCache(RedisCacheConstants.Produtos.KeyIsExAlunoTodosProdutos))
            {
                if (!RedisCacheManager.HasAny(key))
                {
                    var isExAlunoTodosProdutos = IsExAlunoTodosProdutos(matricula);
                    RedisCacheManager.SetItemObject(key, isExAlunoTodosProdutos, TimeSpan.FromMinutes(5));
                    return isExAlunoTodosProdutos;
                }
                else
                {
                    return RedisCacheManager.GetItemObject<bool>(key);
                }
            }
            else
            {
                return IsExAlunoTodosProdutos(matricula);
            }
        }

        public bool IsAlunoPendentePagamento(int matricula)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.Aluno.KeyIsAlunoPendentePagamento))
            {
                var Key = string.Format("{0}:{1}", RedisCacheConstants.Aluno.KeyIsAlunoPendentePagamento, matricula);

                if (RedisCacheManager.HasAny(Key))
                {
                    return RedisCacheManager.GetItemObject<bool>(Key);
                }


                var isAlunoPendente = IsAlunoPendentePagamentoDB(matricula);

                RedisCacheManager.SetItemObject(Key, isAlunoPendente, TimeSpan.FromHours(6));

                return isAlunoPendente;
            }
            else
            {
                return IsAlunoPendentePagamentoDB(matricula);
            }
        }

        public bool IsAlunoPendentePagamentoDB(int matricula)
        {
            var produtosPermitidosPendente = new int[] { Produto.Produtos.MED_MASTER.GetHashCode(), Produto.Produtos.CPMED_EXTENSIVO.GetHashCode() };

            using (var ctx = new DesenvContext())
            {
                return (from so in ctx.tblSellOrders
                        join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                        join p in ctx.tblProducts on sod.intProductID equals p.intProductID
                        where so.intClientID == matricula
                            && produtosPermitidosPendente.Contains(p.intProductGroup1 ?? 0)
                            && so.intStatus2 == (int)Utilidades.ESellOrderStatus.Pendente
                        select so).Any();

            }


        }

        public string GetMensagensLoginDB(int idAplicacao, int idTipoMensagem)
        {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblMensagensLogin.Where(o => o.intAplicacaoId == idAplicacao && o.intTipoMensagemId == idTipoMensagem).FirstOrDefault().txtMensagem;
            }
        }

        public List<Produto.Produtos> GetProdutosPermitidosLogin(int idAplicacao)
        {
            var produtosPermitidos = new List<Produto.Produtos>();

            if (idAplicacao == (int)Aplicacoes.MsProDesktop)
            {
                produtosPermitidos = new List<Produto.Produtos>
                    {
                        Produto.Produtos.MEDCURSO,
                        Produto.Produtos.MEDEAD,
                        Produto.Produtos.MED,
                        Produto.Produtos.MEDCURSOEAD,
                        Produto.Produtos.INTENSIVAO,
                        Produto.Produtos.RAC,
                        Produto.Produtos.RACIPE,
                        Produto.Produtos.MED_MASTER
                    };
            }
            else
            {
                produtosPermitidos = new List<Produto.Produtos>
                {
                    Produto.Produtos.MEDCURSO,
                    Produto.Produtos.MEDEAD,
                    Produto.Produtos.MED,
                    Produto.Produtos.MEDCURSOEAD,
                    Produto.Produtos.MEDELETRO,
                    Produto.Produtos.INTENSIVAO,
                    Produto.Produtos.CPMED,
                    Produto.Produtos.ADAPTAMED,
                    Produto.Produtos.RAC,
                    Produto.Produtos.RACIPE,
                    Produto.Produtos.RAC_IMED,
                    Produto.Produtos.RACIPE_IMED,
                    Produto.Produtos.R3CIRURGIA,
                    Produto.Produtos.R3CLINICA,
                    Produto.Produtos.R3PEDIATRIA,
                    Produto.Produtos.R4GO,
                    Produto.Produtos.MEDELETRO_IMED,
                    Produto.Produtos.MASTO,
                    Produto.Produtos.MED_MASTER,
                    Produto.Produtos.TEGO,
                    Produto.Produtos.CPMED_EXTENSIVO
                };
            }

            return produtosPermitidos;
        }

        public PermissaoDevice GetPermissaoAcesso(int idAplicacao, int matricula, string token, Utilidades.TipoDevice idDevice)
        {
            try
            {
                switch (idAplicacao)
                {
                    case (int)Aplicacoes.MsProDesktop:
                    case (int)Aplicacoes.MEDSOFT_PRO_ELECTRON:
                        return GetPermissaoAcessoDesktop(idAplicacao, matricula, token, idDevice);
                    default:
                        return Utilidades.IsMobile(idDevice)
                            ? GetPermissaoAcessoMovel(idAplicacao, matricula, token, idDevice)
                            : GetPermissaoAcessoTablet(idAplicacao, matricula, token, idDevice);
                }
            }
            catch
            {
                throw;
            }
        }

        public PermissaoDevice GetPermissaoAcessoDesktop(int idAplicacao, int matricula, string token, Utilidades.TipoDevice idDevice)
        {
            try
            {
                var retorno = new PermissaoDevice { PermiteAcesso = 1 };
                using (var ctx = new DesenvContext())
                {
                    var idsDesktop = new[]
                                    {
                                        Convert.ToInt32(Utilidades.TipoDevice.windows),
                                        Convert.ToInt32(Utilidades.TipoDevice.mac)
                                    };

                    var registrosSeguranca = (from s in ctx.tblSeguranca
                                              join d in ctx.tblDeviceMovel on s.intDeviceId equals d.intDeviceId
                                              where s.intClientId == matricula && s.intApplicationId == idAplicacao
                                                  && idsDesktop.Contains(d.intDeviceId)
                                              select new
                                              {
                                                  idSeguranca = s.intMsMovelSegurancaId,
                                                  token = s.txtDeviceToken,
                                                  deviceId = s.intDeviceId,
                                                  dataCadastro = s.dteCadastro,
                                                  tipoDevice = d.txtDescricao
                                              }).ToList();

                    if (registrosSeguranca.Count == 0)
                    {
                        ctx.tblSeguranca.Add(new tblSeguranca
                        {
                            intApplicationId = idAplicacao,
                            intClientId = matricula,
                            intDeviceId = Convert.ToInt32(idDevice),
                            txtDeviceToken = token,
                            dteCadastro = DateTime.Now
                        });
                        ctx.SaveChanges();
                        retorno.PermiteTroca = 0;
                    }
                    else
                    {
                        if (registrosSeguranca.OrderByDescending(d => d.dataCadastro).FirstOrDefault().token == token)
                            retorno.PermiteTroca = 0;

                        else if (!registrosSeguranca.Any(s => s.token == token))
                            retorno.PermiteTroca = 1;

                        else
                        {
                            retorno.PermiteAcesso = 0;
                            retorno.PermiteTroca = 0;
                        }
                    }

                    return retorno;
                }
            }
            catch
            {
                throw;
            }
        }

        public PermissaoDevice GetPermissaoAcessoMovel(int idAplicacao, int matricula, string token,
            Utilidades.TipoDevice idDevice)
        {
            try
            {
                var retorno = new PermissaoDevice { PermiteAcesso = 1 };
                using (var ctx = new DesenvContext())
                {
                    var idsMobile = new[]
                                    {
                                        Convert.ToInt32(Utilidades.TipoDevice.androidMobile),
                                        Convert.ToInt32(Utilidades.TipoDevice.iosMobile)
                                    };

                    var registrosSeguranca = (from s in ctx.tblSeguranca
                                              join d in ctx.tblDeviceMovel on s.intDeviceId equals d.intDeviceId
                                              where s.intClientId == matricula && s.intApplicationId == idAplicacao
                                              select new RegistrosSeguranca()
                                              {
                                                  IdSeguranca = s.intMsMovelSegurancaId,
                                                  Token = s.txtDeviceToken,
                                                  DeviceId = s.intDeviceId,
                                                  DataCadastro = s.dteCadastro,
                                                  TipoDevice = d.txtDescricao
                                              }).ToList();


                    var registrosSegurancaMobile = registrosSeguranca.Where(x => idsMobile.Contains(x.DeviceId)).ToList();

                    if (registrosSegurancaMobile.Count == 0)
                    {
                        ctx.tblSeguranca.Add(new tblSeguranca
                        {
                            intApplicationId = idAplicacao,
                            intClientId = matricula,
                            intDeviceId = Convert.ToInt32(idDevice),
                            txtDeviceToken = token,
                            dteCadastro = DateTime.Now
                        });
                        ctx.SaveChanges();

                        retorno.PermiteTroca = 0;
                    }
                    else
                    {
                        if (registrosSegurancaMobile.OrderByDescending(d => d.DataCadastro).FirstOrDefault().Token == token)
                            retorno.PermiteTroca = 0;

                        else if (!registrosSegurancaMobile.Any(s => s.Token == token))
                            retorno.PermiteTroca = 1;

                        else
                        {
                            var whiteList = ctx.tblAlunoCrossPlataformaWhiteList.Any(x => x.intClientId == matricula);

                            retorno.PermiteAcesso = 0;
                            retorno.PermiteTroca = 0;

                            if (AlunoExcecaoPodeAcessarPorTablet(matricula, token, registrosSeguranca) || whiteList)
                                retorno.PermiteAcesso = 1;
                        }
                    }

                    return retorno;
                }
            }
            catch
            {
                throw;
            }
        }

        public PermissaoDevice GetPermissaoAcessoTablet(int idAplicacao, int matricula, string token, Utilidades.TipoDevice idDevice)
        {
            try
            {
                var retorno = new PermissaoDevice { PermiteAcesso = 1 };
                using (var ctx = new DesenvContext())
                {
                    var idsTablet = new[]
                                    {
                                        Convert.ToInt32(Utilidades.TipoDevice.androidTablet),
                                        Convert.ToInt32(Utilidades.TipoDevice.iosTablet)
                                    };

                    var registrosSeguranca = (from s in ctx.tblSeguranca
                                              join d in ctx.tblDeviceMovel on s.intDeviceId equals d.intDeviceId
                                              where s.intClientId == matricula && s.intApplicationId == idAplicacao
                                                  && idsTablet.Contains(d.intDeviceId)
                                              select new RegistrosSeguranca()
                                              {
                                                  IdSeguranca = s.intMsMovelSegurancaId,
                                                  Token = s.txtDeviceToken,
                                                  DeviceId = s.intDeviceId,
                                                  DataCadastro = s.dteCadastro,
                                                  TipoDevice = d.txtDescricao
                                              }).ToList();

                    if (registrosSeguranca.Count == 0)
                    {
                        ctx.tblSeguranca.Add(new tblSeguranca
                        {
                            intApplicationId = idAplicacao,
                            intClientId = matricula,
                            intDeviceId = Convert.ToInt32(idDevice),
                            txtDeviceToken = token,
                            dteCadastro = DateTime.Now
                        });
                        ctx.SaveChanges();
                        retorno.PermiteTroca = 0;
                    }
                    else
                    {
                        if (registrosSeguranca.OrderByDescending(d => d.DataCadastro).FirstOrDefault().Token == token)
                            retorno.PermiteTroca = 0;

                        else if (!registrosSeguranca.Any(s => s.Token == token))
                            retorno.PermiteTroca = 1;

                        else
                        {
                            retorno.PermiteAcesso = ctx.tblAlunoCrossPlataformaWhiteList.Any(x => x.intClientId == matricula) ? 1 : 0;
                            retorno.PermiteTroca = 0;
                        }
                    }

                    return retorno;
                }
            }
            catch
            {
                throw;
            }
        }

        public string GetAlunoEstado(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return (from p in ctx.tblPersons
                        join c in ctx.tblCities on p.intCityID equals c.intCityID
                        join s in ctx.tblStates on c.intState equals s.intStateID
                        where p.intContactID == matricula
                        select s.txtCaption).FirstOrDefault();
            }
        }

        private bool AlunoExcecaoPodeAcessarPorTablet(int matricula, string token, List<RegistrosSeguranca> registrosSeguranca)
        {
            int[] matriculasPermitidas = GetMatriculasAlunoExcecaoAcessoTablet().ToArray(); //Matriculas Permitidas, https://trello.com/c/dRuKRVta/59-aluna-n-consegue-acessar-2-dispositivos-simult%C3%A2neo
            if (!matriculasPermitidas.Contains(matricula))
                return false;

            registrosSeguranca = registrosSeguranca.OrderByDescending(d => d.DataCadastro).Take(2).ToList();

            return registrosSeguranca.Select(x => x.Token).ToList().Contains(token);
        }

        public List<int> GetMatriculasAlunoExcecaoAcessoTablet()
        {
            List<int> alunosExcessaoTablet;

            using (var ctx = new DesenvContext())
            {
                var alunosExcessaoTabletQuery = from a in ctx.tblAlunoExcecaoAcessoTablet
                                                select a.intClientID;

                alunosExcessaoTablet = alunosExcessaoTabletQuery.ToList();
            }
            return alunosExcessaoTablet;
        }

        public Aluno GetAlunosDevice(int matricula, Aplicacoes aplicacao)
        {
            throw new NotImplementedException();
        }

        public int SetNickName(Aluno aluno)
        {
            try
            {
                var nomesImprorios = new DBQuery().ExecuteStoredProcedure("emed_Load_Nick_Names_improprios")
                    .Tables[0]
                    .Rows
                    .OfType<DataRow>()
                    .Select(dr => dr.Field<string>("txtName")
                                .Trim()
                                .ToUpper())
                    .ToList();

                var palavras = aluno.NickName.Split(' ');
                foreach (var palavra in palavras)
                    if (nomesImprorios.Contains(palavra.ToUpper()))
                        return 0;

                var nickName = aluno.NickName.Trim().ToUpper();
                if (NicknameMatch4LettersAnd4Numbers(nickName) == false)
                {
                    return 0;
                }

                using (var ctx = new DesenvContext())
                {

                    var jaTemCadastrado = ctx.tblPersons.Any(p => p.intContactID != aluno.ID &&
                                                                  p.txtNickName.Trim().ToUpper() == nickName);
                    if (jaTemCadastrado)
                        return 0;

                    var al = ctx.tblPersons.FirstOrDefault(p => p.intContactID == aluno.ID);
                    al.txtNickName = aluno.NickName.Trim();
                    ctx.SaveChanges();

                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool NicknameMatch4LettersAnd4Numbers(string nickname)
        {
            const int limitBySentence = 4;
            const int limitTotal = 8;

            if (String.IsNullOrEmpty(nickname) || nickname.Length != limitTotal)
                return false;


            if (nickname.All(Char.IsLetterOrDigit) == false)
                return false;

            if (nickname.Any(Char.IsLetter) && nickname.Count(Char.IsLetter) > limitBySentence)
                return false;

            if (nickname.Any(Char.IsNumber) && nickname.Count(Char.IsNumber) > limitBySentence)
                return false;

            return true;
        }

         public bool SetMedsoftClipboardReport(SegurancaDevice seguranca)
        {
            try
            {
                //lancamento fim de ano
                //por enquanto somente medsoft ionic
                var medsoftMobileId = 17;
                var deviceId = string.Empty;

                //lancamento fim de ano
                //por enquanto sem device id obrigatório
                try
                {
                    deviceId = seguranca.IdDevice.ToString();
                }
                catch (Exception) { }

                using (var ctx = new DesenvContext())
                {
                    ctx.tblMedsoftClipboardReport.Add(new tblMedsoftClipboardReport
                    {
                        dteCriacao = DateTime.Now,
                        intApplicationId = medsoftMobileId,
                        intClientID = seguranca.Matricula,
                        txtDeviceID = deviceId
                    });
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}