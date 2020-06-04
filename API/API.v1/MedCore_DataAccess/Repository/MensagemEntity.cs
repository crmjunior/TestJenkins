using System;
using System.Collections.Generic;
using MedCore_DataAccess.Model;
using System.Linq;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class MensagemEntity
    {
        public static List<tblAvisos> GetAvisos(int applicationID)
        {
            using (var ctx = new DesenvContext())
            {
                var query = (from a in ctx.tblAvisos
                             where a.intApplicationID == applicationID
                             select a).ToList();

                return query;
            }
        }

        public static string GetAviso(int avisoID, int applicationID)
        {
            using(MiniProfiler.Current.Step("Obtendo aviso"))
            {
                using (var ctx = new DesenvContext())
                {
                    var aviso = string.Empty;

                    if (applicationID == 0)                             // Todos as aplica��es
                        aviso = (from a in ctx.tblAvisos
                                where a.intAvisoID == avisoID
                                select a.txtAviso).FirstOrDefault();
                    else
                        aviso = (from a in ctx.tblAvisos
                                where a.intApplicationID == applicationID && a.intAvisoID == avisoID
                                select a.txtAviso).FirstOrDefault();


                    return aviso;
                }
            }
            
        }

        public static bool SetLogAviso(int clientID, int avisoID, bool confirmaVisualizacao = false)
        {
            try
            {
                using(MiniProfiler.Current.Step("Inserindo log de aviso"))
                {
                    using (var ctx = new DesenvContext())
                    {
                        ctx.tblLogAvisos.Add(new tblLogAvisos()
                        {
                            intAvisoID = avisoID,
                            intClientID = clientID,
                            bitConfirmaVisualizacao = confirmaVisualizacao,
                            dteVisualizacao = DateTime.Now
                        });
                        ctx.SaveChanges();

                        return true;
                    }    
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<tblLogAvisos> GetLogAvisos(int clientID)
        {
            using (var ctx = new DesenvContext())
            {
                var avisos = (from a in ctx.tblLogAvisos
                              where a.intClientID == clientID
                              select a).ToList();

                return avisos;
            }
        }

        public static bool IsAvisoVisto(int clientID, int avisoID)
        {
            using(MiniProfiler.Current.Step("Consultando se um aviso foi visualizado"))
            {
                using (var ctx = new DesenvContext())
                {
                    var avisoVisto = (from a in ctx.tblLogAvisos
                                    where a.intClientID == clientID && a.intAvisoID == avisoID
                                    select a.bitConfirmaVisualizacao).FirstOrDefault();

                    return avisoVisto;
                }
            }
        }

        // public int MensagemPermissaoConcursoNaIntegra(int idMenu, int matricula, int idAplicacao)
        // {
        //     var lstMenu = new List<Menu>();
        //     int mensagemId = 0;

        //     lstMenu.Add(new Menu() { Id = 40 });
        //     lstMenu.Add(new Menu() { Id = 51 });
        //     lstMenu.Add(new Menu() { Id = idMenu });

        //     var lstPermissaoRegra = new MenuEntity().GetAlunoPermissoesMenu(lstMenu, matricula, idAplicacao);

        //     var menuExtSimulado = lstPermissaoRegra.Where(y => y.ObjetoId == 40 && y.MensagemId == 17).FirstOrDefault();
        //     var menuEletroEcg = lstPermissaoRegra.Where(y => y.ObjetoId == 51 && y.MensagemId == 21).FirstOrDefault();

        //     var permissaoConcursoNaIntegra = lstPermissaoRegra.Where(x => x.ObjetoId == idMenu).FirstOrDefault();

        //     if ((menuExtSimulado == null && menuEletroEcg == null) || (menuExtSimulado != null || menuEletroEcg != null))
        //     {
        //         mensagemId = permissaoConcursoNaIntegra.MensagemId ?? -1;
        //     }


        //     return mensagemId;
        // }

        // public Mensagem Get(Mensagem Msg, int matricula, int idMenu, int idAplicacao)
        // {
        //     using (var ctx = new DesenvContext())
        //     {

        //         //temporario revisaoEstudos
        //         if (idMenu == 79)
        //         {
        //             return new Mensagem();
        //         }

        //         var menuRetorno = new Menu();
        //         var lstMenu = new List<Menu>();
        //         int mensagemId = 0;

        //         lstMenu.Add(new Menu()
        //         {
        //             Id = idMenu
        //         });

        //         if (idMenu == 35)
        //         {
        //             lstMenu.Add(new Menu() { Id = 40 });
        //             lstMenu.Add(new Menu() { Id = 51 });
        //         }

        //         mensagemId = (int)new MenuEntity().GetAlunoPermissoesMenu(lstMenu, matricula, idAplicacao).Find(p => p.ObjetoId == idMenu).MensagemId;

        //         //var menuPermissao = new MenuEntity().GetAlunoPermissoesMenu(lstMenu, matricula).Find(p => p.ObjetoId == idMenu);

        //         if (mensagemId == -1)
        //             return new Mensagem();


        //         var mensagem = (from av in ctx.tblAvisos
        //                         where av.intAvisoID == mensagemId
        //                         select new Mensagem()
        //                         {
        //                             Id = av.intAvisoID,
        //                             Titulo = av.txtTitulo,
        //                             Texto = av.txtAviso,
        //                             IdAplicacao = Msg.IdAplicacao

        //                         }).FirstOrDefault();

        //         if (mensagem != null)
        //         {
        //             mensagem.Acoes = new List<Acoes>();
        //             mensagem.Acoes.AddRange(from ac in ctx.tblAvisos_Chamados
        //                                     where ac.intAvisoId == mensagemId
        //                                     select new Acoes()
        //                                     {
        //                                         Nome = ac.txtNome,
        //                                         IdChamadoCategoria = ac.intChamadoCategoriaId ?? 0,
        //                                         IdStatusInterno = ac.intStatusInternoId ?? 0,
        //                                         Ordem = ac.intOrdem
        //                                     });

        //             //TODO:Virar Organizar
        //             if (mensagemId == 17 || mensagemId == 16 || mensagemId == 18 || mensagemId == 21)
        //             {
        //                 //IncluiOVs relacionadas ao chamado
        //                 var idsProductGroup = (from mp in ctx.tblAccess_Menu_ProductGroup
        //                                        join m in ctx.tblAccess_Menu on mp.intMenuId equals m.intMenuId
        //                                        where m.intObjectId == idMenu
        //                                        select mp.intProductGroup).Distinct().ToList();
        //                 var idsExtensivo = idsProductGroup.Where(i => i != (int)Produto.Produtos.MEDELETRO).ToList();
        //                 var idsMEDELETRO = idsProductGroup.Where(i => i == (int)(Produto.Produtos.MEDELETRO)).ToList();
        //                 var anoAtual = Utilidades.GetYear();

        //                 int[] idsProduto = { (int)Produto.Produtos.MED, (int)Produto.Produtos.MEDCURSO, (int)Produto.Produtos.MEDEAD, (int)Produto.Produtos.MEDCURSOEAD, (int)Produto.Produtos.INTENSIVAO };
        //                 int[] idsOvStatus = { (int)OrdemVenda.StatusOv.Inadimplente, (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES };

        //                 mensagem.IdsRelacionados = new List<int>();
        //                 List<int> lstOvInadimplentes = (from so in ctx.tblSellOrders
        //                                                 join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
        //                                                 join prod in ctx.tblProducts on sod.intProductID equals prod.intProductID
        //                                                 join c in ctx.tblCourses on sod.intProductID equals c.intCourseID
        //                                                 where so.intStatus == (int)OrdemVenda.StatusOv.Ativa &&
        //                                                 ((idsExtensivo.Count() > 0 && idsProduto.Contains(prod.intProductGroup1 ?? 0) && so.intStatus2 == (int)OrdemVenda.StatusOv.Inadimplente_MESES_ANTERIORES)
        //                                                 || (idsMEDELETRO.Count() > 0 && prod.intProductGroup1 == (int)Produto.Produtos.MEDELETRO && idsOvStatus.Contains(so.intStatus2 ?? 0))
        //                                                 )
        //                                                 && so.intClientID == matricula //&& c.intYear == anoAtual
        //                                                 select so.intOrderID).Distinct().ToList();
        //                 mensagem.IdsRelacionados = lstOvInadimplentes;



        //                 if (mensagemId == 17 || mensagemId == 21)
        //                 {

        //                     mensagem = TrataBloqueioInadimplencia(matricula, mensagem);
        //                 }

        //                 if (mensagemId == 16 || mensagemId == 18)
        //                 {
        //                     foreach (var idOv in lstOvInadimplentes)
        //                     {
        //                         new ChamadoCallCenterEntity().SetVisualizacaoMensagemInadimplencia(matricula, idOv, idAplicacao);
        //                     }
        //                     mensagem = TrataAvisoInadimplencia(matricula, mensagem);
        //                 }

        //             }
        //         }

        //         return mensagem;
        //     }
        // }

        // private Mensagem TrataAvisoInadimplencia(int matricula, Mensagem mensagem)
        // {
        //     using (var ctx = new DesenvContext())
        //     {
        //         //Inclui log
        //         ctx.tblPermissaoInadimplenciaLogAlertas.Add(new tblPermissaoInadimplenciaLogAlerta
        //         {
        //             intClientId = matricula,
        //             intApplicationId = mensagem.IdAplicacao,
        //             dteCadastro = DateTime.Now
        //         });
        //         ctx.SaveChanges();

        //         //IncluirOvs iD relacionados estava aqui
        //     }

        //     mensagem.Texto = FormataMsgInadinplente(mensagem.Texto, matricula);
        //     return mensagem;
        // }

        // private Mensagem TrataBloqueioInadimplencia(int matricula, Mensagem mensagem)
        // {
        //     //abre chamdado bloqueio 
        //     using (var ctx = new DesenvContext())
        //     {
        //         foreach (var id in mensagem.IdsRelacionados)
        //         {
        //             ctx.msp_GeraChamadoBloqueioInadimplencia(id, Constants.IdEmployeeChamado, matricula);
        //         }
        //     }
        //     mensagem.Texto = FormataMsgInadinplente(mensagem.Texto, matricula);

        //     return mensagem;
        // }

        // public string FormataMsgInadinplente(string msg, int matricula)
        // {
        //     using (var ctx = new DesenvContext())
        //     {
        //         var nomeAluno = (from p in ctx.tblPersons
        //                          where p.intContactID == matricula
        //                          select p).FirstOrDefault().txtName.Trim();

        //         var msgFormatada = string.Format(msg, nomeAluno);
        //         return msgFormatada;
        //     }
        // }

        // public List<Mensagem> GetMensagensAplicacao(int applicationID)
        // {
        //     if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetMensagensAplicacao))
        //         return RedisCacheManager.GetItemObject<List<Mensagem>>(RedisCacheConstants.DadosFakes.KeyGetMensagensAplicacao);

        //     using (var ctx = new DesenvContext())
        //     {
        //         var mensagens = ctx.tblMensagens.Where(x => x.intAplication == applicationID)
        //             .Select(y => new Mensagem
        //         {
        //             Id = y.intMensagemId,
        //             Titulo = y.txtDescricao,
        //             Texto = y.txtMensagem,
        //             IdAplicacao = y.intAplication
        //         }).ToList();

        //         return mensagens;
        //     }
        // }
    }
}