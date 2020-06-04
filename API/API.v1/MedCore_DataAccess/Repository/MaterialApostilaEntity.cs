
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using System;
using System.Collections.Generic;
using MedCore_DataAccess.Util;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class MaterialApostilaEntity : IMaterialApostilaData
    {
        public MaterialApostilaAluno GetApostilaAtiva(int idMaterial, int matricula, int idVersao)
        {
            try
            {
                var apostila = new MaterialApostilaAluno();
                using(MiniProfiler.Current.Step("Obter apostila do aluno atraves de matricula e versão"))
                {
                    apostila = GetUltimaApostila(idMaterial, matricula, idVersao);
                }

                if (apostila == null)
                {
                    using(MiniProfiler.Current.Step("Registrando nova apostila"))
                    {
                        if (!RegistraNovaApostila(idMaterial, matricula))
                            return new MaterialApostilaAluno();
                    }

                    using(MiniProfiler.Current.Step("Registrado progresso"))
                    {
                        RegistraProgresso(idMaterial, matricula);
                    }

                    using(MiniProfiler.Current.Step("Obtendo ultima apostila salva"))
                    {
                        return GetUltimaApostila(idMaterial, matricula, idVersao);
                    }
                }
                
                return apostila;
            }
            catch
            {
                throw;
            }
        }

        public MaterialApostilaDTO GetMioloApostilaOriginal(int idMaterial)
        {
            using(MiniProfiler.Current.Step("Obtendo miolo da apostila original"))
            {
                try
                {
                    using (var ctx = new DesenvContext())
                    {
                        var apostila = (from o in ctx.tblMaterialApostila
                                        where o.intProductId == idMaterial
                                        select new MaterialApostilaDTO()
                                        {
                                            ID = o.intID,
                                            ProductId = (int)o.intProductId,
                                            Conteudo = o.txtConteudo,
                                            DataCriacao = o.dteDataCriacao
                                        }).FirstOrDefault();
                        return apostila;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public int PostModificacaoApostila(int matricula, int idApostila, string conteudo)
        {
            try
            {
                return InsereNovaVersao(matricula, idApostila, conteudo);
            }
            catch
            {
                throw;
            }
        }

        public List<int> GetIdsVersoesApostila(int apostilaId, int matricula)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {

                    var ids = ctx.tblMaterialApostilaAluno
                                    .Where(x => x.bitAtiva == 1
                                                && x.intClientID == matricula
                                                && x.intMaterialApostilaID == apostilaId)
                                    .Select(y => y.intID)
                                    .ToList();

                    return ids;
                }
            }
            catch 
            {
                throw;
            }
        }

        public MaterialApostilaAluno LimpaVersoes(int idMaterial, int matricula)
        {
            try
            {
                if (ApagaApostilas(idMaterial, matricula))
                {
                    /**
                     * Quando não há apostila ativa, o método GetApostilaAtiva pega uma
                     * apostila original, insere ela na tabela referente à apostila do
                     * aluno e retorna essa apostila
                     */
                    return GetApostilaAtiva(idMaterial, matricula, 0);
                }
                return new MaterialApostilaAluno();
            }
            catch
            {

                throw;
            }
        }

        public decimal GetProgresso(int idMaterial, int matricula)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var apostilaId = ctx.tblMaterialApostila.Where(x => x.intProductId == idMaterial).Select(y => y.intID).FirstOrDefault();

                    return ctx.tblMaterialApostilaProgresso
                        .Where(x => x.intClientID == matricula && x.intApostilaID == apostilaId)
                        .Select(x => x.dblPercentProgresso)
                        .FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public int GetProgressoMaterial(long idMaterial, int matricula)
        {
            return Convert.ToInt32(GetProgresso((int)idMaterial, matricula));
        }

        public int PostProgresso(MaterialApostilaProgresso progresso)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(progresso);
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MinValue;
            int? timeout = null;

            try
            {
                using(var ctx = new DesenvContext())
                {
                    timeout = ctx.Database.GetCommandTimeout();
                    var result = ctx.tblMaterialApostilaProgresso.FirstOrDefault(x => x.intClientID == progresso.ClientId && x.intApostilaID == progresso.ApostilaId);

                    if(result == null)
                    {
                        result = new tblMaterialApostilaProgresso()
                        {
                            dteDataAlteracao = DateTime.Now,
                            intApostilaID = progresso.ApostilaId,
                            intClientID = progresso.ClientId,
                            dblPercentProgresso = 0
                        };
                        ctx.tblMaterialApostilaProgresso.Add(result);

                    }

                    if (progresso.Progresso <= result.dblPercentProgresso)
                    {
                        return 0;
                    }                        

                    result.dblPercentProgresso = progresso.Progresso;

                    startDate = DateTime.Now;
                    var retorno = ctx.SaveChanges();
                    endDate = DateTime.Now;

                    return retorno;
                }
            }
            catch (Exception ex)
            {
                endDate = DateTime.Now;
                Console.WriteLine($@"
###################################################################################

PAYLOAD: {json} - {startDate} - {endDate} - {timeout}
Exception: {ex.Message}
Stack Trace: {ex.StackTrace}
Inner Exception: {(ex.InnerException != null ? ex.InnerException.Message : String.Empty)}
Inner Stack Trace: {(ex.InnerException != null ? ex.InnerException.StackTrace : String.Empty)}

###################################################################################");
                return 0;
            }
        }

        public List<MaterialApostilaInteracao> GetInteracoesAluno(int idApostila, int matricula)
        {
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var idVersao = (from ap in ctx.tblMaterialApostila
                                    join apl in ctx.tblMaterialApostilaAluno
                                    on ap.intID equals apl.intMaterialApostilaID
                                    where ap.intID == idApostila && apl.intClientID == matricula && apl.bitAtiva == 1
                                    orderby apl.intID descending
                                    select apl.intID
                                    ).FirstOrDefault();

                    return ctx.tblMaterialApostilaInteracao
                            .Where(x => x.intClientID == matricula
                                    && x.intApostilaID == idApostila
                                    && x.intVersaoMinima <= idVersao
                                    && (x.intVersaoMaxima == 0 || x.intVersaoMaxima > idVersao))
                            .Select(y => new MaterialApostilaInteracao()
                            {
                                Id = y.intID,
                                ApostilaId = idApostila,
                                ClientId = matricula,
                                AnotacaoId = y.txtInteracaoID,
                                Comentario = y.txtComentario,
                                VersaoMinima = y.intVersaoMinima,
                                VersaoMaxima = y.intVersaoMaxima ?? 0,
                                Conteudo = y.txtConteudo,
                                TipoInteracao = y.intTipoInteracao
                            })
                            .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public int PostInteracoes(MaterialApostilaInteracoes interacoes)
        {
            try
            {
                var novosInteracoes = AdicionaNovos(interacoes);
                var interacoesAtualizadas = AtualizaModificacoes(interacoes);

                if (novosInteracoes + interacoesAtualizadas < 1)
                    return 0;

                return 1;
            }
            catch
            {
                throw;
            }
        }

        public long? GetEntidadeId(int idApostila)
        {
            using(MiniProfiler.Current.Step("Obtendo entidade pelo id da apostila"))
            {
                using (var ctx = new DesenvContext())
                {
                    var entidadeId = (from book in ctx.tblBooks
                                    where book.intBookID == idApostila
                                    select book.intBookEntityID)
                                    .FirstOrDefault();
                    return entidadeId;
                }
            }
        }

        public List<AddOnApostila> GetAddonApostila(int idApostila)
        {           
            try
            {
                using (var ctx = new DesenvContext())
                {
                    var lstApostilaAddOn = ctx.tblApostilaAddOn
                         .Where(x => x.intApostilaID == idApostila)
                         .Select(y => new AddOnApostila()
                          {
                              Id = y.intApostilaAddOnID,
                              Posicao = y.txtPosicao,
                              IdApostila = idApostila,
                              Conteudo = y.txtConteudo,
                          }).ToList();

                    return lstApostilaAddOn;
                }
            }
            catch
            {

                throw;
            }

        }

        public int RegistraPrintApostila(LogPrintApostila registro)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    ctx.tblLog_PrintApostilaMedsoftPro.Add(new tblLog_PrintApostilaMedsoftPro
                    {
                        cpf = registro.CPF,
                        data = registro.Date,
                        apostila = registro.Apostila,
                        pagina = (int)registro.Pagina,
                        numPorcentagem = registro.Pagina
                    });

                    ctx.SaveChanges();

                    return 1;
                }
                catch
                {
                    return 0;
                }

            }
        }

        public tblMaterialApostilaAluno GetMaterialApostilaAluno(int matricula, int materialApostila)
        {
            using (var ctx = new DesenvContext())
            {
                try
                {
                    return ctx.tblMaterialApostilaAluno
                        .Where(x => x.intClientID == matricula && x.intMaterialApostilaID == materialApostila && x.bitAtiva == 1)
                        .FirstOrDefault();
                }
                catch
                {
                    throw;
                }
            }
        }

        public MaterialApostilaAluno GetUltimaApostila(int MaterialId, int matricula, int IdVersao)
        {
            var  apostilaAluno = new MaterialApostilaAluno();

            var materialApostilaAlunoManager = new MaterialApostilaAlunoManager();

            using (var ctx = new DesenvContext())
            {

                var query = (from a in ctx.tblMaterialApostilaAluno
                             join b in ctx.tblMaterialApostila on a.intMaterialApostilaID equals b.intID
                             where a.bitAtiva == 1 && a.intClientID == matricula && b.intProductId == MaterialId
                             select new MaterialApostilaAluno()
                             {
                                 DataAtualizacao = a.dteDataCriacao,
                                 Id = a.intID,
                                 PdfId = b.intProductId.Value,
                                 ClientId = a.intClientID.Value,
                                 ApostilaId = a.intMaterialApostilaID.Value,
                                 Ativa = a.bitAtiva == 1,
                                 EntidadeId = b.intProductId.Value
                             });


                 apostilaAluno = query.OrderByDescending(x => x.Id).FirstOrDefault();



                if (apostilaAluno != null)
                {
                    apostilaAluno.DataRelease = GetDataReleaseApostila(MaterialId, matricula);

                    var anoRelease = GetAnoReleaseApostila(MaterialId);

                    apostilaAluno.Configs = ctx.tblMaterialApostilaConfig
                                               .Select(x => new MaterialApostilaConfig()
                                               {
                                                   Descricao = x.txtDescricao,
                                                   Ativa = x.bitAtiva
                                               })
                                               .ToList();

                    //carrega o link do css da apostila
                    var cssName = GetNameCss(MaterialId, anoRelease);
                    apostilaAluno.LinkCss = GetApostilaCss(anoRelease, cssName);
                                     
                    //carregar o material do aluno para obter o nome da apostila que está no S3
                    var materialApostilaAluno = ctx.tblMaterialApostilaAluno
                                .Where(x => x.intClientID == matricula && x.intMaterialApostilaID == apostilaAluno.ApostilaId && x.bitAtiva == 1)
                                .FirstOrDefault();

                    var apostilaVersao = Utilidades.GetDetalhesApostila(materialApostilaAluno);

                    //carregar o conteudo da apostila do aluno
                    string chave = Utilidades.CriarNomeApostila(matricula, apostilaAluno.ApostilaId, apostilaVersao.Versao);
                    apostilaAluno.Conteudo = materialApostilaAlunoManager.ObterArquivo(chave);

                    //retorna se o aluno tem permissão de edição do material
                    var alunoEntity = new AlunoEntity();
                    if (alunoEntity.IsAlunoPendentePagamento(matricula))
                    {
                        apostilaAluno.Bloqueado = false;
                    }                    
                    else
                    {
                        apostilaAluno.Bloqueado = alunoEntity.IsExAlunoTodosProdutos(matricula);
                    }

                    var anoAtual = Utilidades.GetYear();
                    var anoApostila = Utilidades.UnixTimeStampToDateTime(apostilaAluno.DataRelease).Year;
                    apostilaAluno.NaoInterageDuvidas = apostilaAluno.Bloqueado ? apostilaAluno.Bloqueado : anoApostila < anoAtual;
                }
            }

            return apostilaAluno;
        }

        private string GetApostilaCss(int anoRelease, string cssName)
        {
            using (var ctx = new DesenvContext())
            {
                var link = ctx.tblMaterialApostilaAssets.Where(x => cssName == null ? x.intAno == anoRelease : x.intAno == -1).FirstOrDefault();
                var url = link.txtUrl + cssName + "?v=" + Utilidades.ToUnixTimespan(DateTime.Now);
                return url;                
            }
        }

        public string GetAssetApostila(int anoRelease)
        {
            using (var ctx = new DesenvContext())
            {
                var link = ctx.tblMaterialApostilaAssets.Where(x => x.intAno == anoRelease).FirstOrDefault();
                return link.txtUrl;
            }
        }

        public string GetNameCss(int materialId, int anoRelease)
        {
            if(anoRelease >= Utilidades.AnoMinimoParaCssFatiado)
            {
                using (var ctx = new DesenvContext())
                {
                    var anoAtual = DateTime.Now.Year;
                    var materials = (from cqc in ctx.tblConcursoQuestaoCatologoDeClassificacoes
                               join ccpv in ctx.tblConcursoQuestao_Classificacao_ProductGroup_ValidacaoApostila on cqc.intClassificacaoID equals ccpv.intClassificacaoID
                               join p in ctx.tblProducts on ccpv.intProductGroupID equals p.intProductGroup3
                               join b in ctx.tblBooks on p.intProductID equals b.intBookID
                               where p.intProductID == materialId
                               orderby cqc.txtTipoDeClassificacao
                               select new CssApostila
                               {
                                   TipoClassificacao = cqc.txtTipoDeClassificacao,
                                   NomeCss = Convert.ToString((double)b.intYear).Trim() + "_" +
                                    Convert.ToString((double)p.intProductGroup2).Trim() + "_" +
                                    Convert.ToString((double)ccpv.intProductGroupID).Trim() +
                                    ".min.css"
                               }).ToList();

                    var material = materials.FirstOrDefault();

                    if(material != null && material.NomeCss != null)
                    {
                        return material.NomeCss;
                    }   
                }
            }
            return null;           
        }


        private long GetDataReleaseApostila(int idMaterial, int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var query = (from crono in ctx.mview_Cronograma
                    join lm in ctx.tblLesson_Material on crono.intLessonID equals lm.intLessonID
                    join c in ctx.tblCourses on crono.intCourseID equals c.intCourseID
                    join b in ctx.tblBooks on lm.intMaterialID equals b.intBookID
                    join p in ctx.tblProducts on b.intBookID equals p.intProductID
                    join p2 in ctx.tblProducts on c.intCourseID equals p2.intProductID
                    join sod in ctx.tblSellOrderDetails on p2.intProductID equals sod.intProductID
                    join so in ctx.tblSellOrders on sod.intOrderID equals so.intOrderID
                    where so.intClientID == matricula && b.intBookID == idMaterial
                    orderby crono.dteDateTime
                    select crono.dteDateTime
                ).FirstOrDefault();

                query = query.AddHours(2);

                return Utilidades.ToUnixTimespan(query);
            }

        }

        private int GetAnoReleaseApostila(int idMaterial)
        {
            using (var ctx = new DesenvContext())
            {
                var query = (from crono in ctx.mview_Cronograma
                             join lm in ctx.tblLesson_Material on crono.intLessonID equals lm.intLessonID
                             join b in ctx.tblBooks on lm.intMaterialID equals b.intBookID                          
                             where b.intBookID == idMaterial
                             orderby b.intYear
                             select b.intYear
                ).FirstOrDefault();

                return query ?? 0;
            }
        }

        public bool RegistraNovaApostila(int idMaterial, int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var original = (from o in ctx.tblMaterialApostila
                                where o.intProductId == idMaterial
                                select new MaterialApostila()
                                {
                                    Id = o.intID,
                                    PdfId = (int)o.intProductId,
                                    Conteudo = o.txtConteudo,
                                    DataAtualizacao = o.dteDataCriacao
                                }).FirstOrDefault();

                
                //criar chave do arquivo que vai ser salvo no S3
                var chaveArquivo = Utilidades.CriarNomeApostila(matricula, original.Id, 1);

                ctx.tblMaterialApostilaAluno.Add(new tblMaterialApostilaAluno()
                {
                    intClientID = matricula,
                    bitAtiva = 1,
                    dteDataCriacao = DateTime.Now,
                    intMaterialApostilaID = original.Id,
                    txtApostilaNameId = chaveArquivo,
                    txtConteudo = ""

                });

                var materialApostilaAlunoManager = new MaterialApostilaAlunoManager();
                using(MiniProfiler.Current.Step("Salvando nova apostila"))
                {
                    if(materialApostilaAlunoManager.SalvarArquivo(chaveArquivo, original.Conteudo))
                    {
                        return (ctx.SaveChanges() == 1);
                    }
                }

                return false;
            }
        }

        public List<ProgressoSemana> GetProgressoMaterial(int matricula, int ano, int produto)
        {
            using(MiniProfiler.Current.Step("Obtendo progresso do material"))
            {
                using (var ctx = new DesenvContext())
                {
                    var list = (from a in ctx.tblMaterialApostilaProgresso
                                join b in ctx.tblMaterialApostila on a.intApostilaID equals b.intID
                                join c in ctx.tblProducts on b.intProductId equals c.intProductID
                                join bo in ctx.tblBooks on c.intProductID equals bo.intBookID
                                where a.intClientID == matricula 
                                    && a.dblPercentProgresso >= 1 
                                    && (c.intProductGroup2 == produto || (produto == (int)Produto.Cursos.CPMED && c.intProductGroup2 == (int)Produto.Cursos.MED))
                                    && bo.intYear == ano
                                select new
                                {
                                    IdEntidade = b.intProductId,
                                    PercentLido = a.dblPercentProgresso
                                })
                        .GroupBy(x => x.IdEntidade)
                        .Select(x => new ProgressoSemana()
                        {
                            IdEntidade = x.Key.Value,
                            PercentLido = (int)x.Max(y => y.PercentLido)
                        })
                        .ToList();

                    return list;
                }
            }
        }

        private tblMaterialApostilaAluno ZeraBitAtiva(int matricula, int idApostila)
        {
            using(MiniProfiler.Current.Step("Atualizando versão ativa de apostila"))
            {
                using (var ctx = new DesenvContext())
                {
                    var apostila = ctx.tblMaterialApostilaAluno
                        .Where(x => x.intClientID == matricula && x.intMaterialApostilaID == idApostila && x.bitAtiva == 1)
                        .FirstOrDefault();

                    if (apostila != null)
                    {
                        apostila.bitAtiva = 0;

                        ctx.SaveChanges();

                        return apostila;
                    }
                    return null;
                }
            }
        }

        private int InsereNovaVersao(int matricula, int idApostila, string conteudo)
        {
            using (var ctx = new DesenvContext())
            {
                var ultimaAtiva = ctx.tblMaterialApostilaAluno
                                        .Where(x => x.bitAtiva == 1
                                                    && x.intClientID == matricula
                                                    && x.intMaterialApostilaID == idApostila)
                                        .OrderBy(y => y.dteDataCriacao)
                                        .FirstOrDefault();

             
                if (ultimaAtiva != null)
                {
                    long versao = 1;

                    if (ultimaAtiva.txtApostilaNameId != "")
                    {
                        var ultimaVersao = Utilidades.GetDetalhesApostila(ultimaAtiva);
                        versao = ultimaVersao.Versao + 1;
                    }

                    //chave do arquivo que vai ser salvo no S3                
                    var chaveArquivo = Utilidades.CriarNomeApostila(matricula, idApostila, versao);

                    //atualiza a chave para a nova versao
                    ultimaAtiva.txtApostilaNameId = chaveArquivo;

                    //após salvar o na tabela MaterialApostilaAluno, salvar o arquivo no bucket com o nome que foi criado
                    var materialApostilaAlunoManager = new MaterialApostilaAlunoManager();
                    using(MiniProfiler.Current.Step("Salvando nova versão de apostila no S3"))
                    {
                        if (materialApostilaAlunoManager.SalvarArquivo(chaveArquivo, conteudo))                        
                        {
                            ctx.SaveChanges();
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    var apostila = ctx.tblMaterialApostila.
                                    Where(x => x.intID == idApostila).
                                    FirstOrDefault();

                    if (!RegistraNovaApostila(apostila.intProductId.Value, matricula))
                        return 0;
                }
                

                int count = ctx.tblMaterialApostilaAluno
                    .Count(x => x.intClientID == matricula
                            && x.intMaterialApostilaID == idApostila
                            && x.bitAtiva == 1);
                if (count > 10)
                    DeletaUltimaVersao(matricula, idApostila);

                return 1;
            }
        }

        private int DeletaUltimaVersao(int matricula, int idApostila)
        {
            using(MiniProfiler.Current.Step("Deleta ultima versão da apostila"))
            {
                using (var ctx = new DesenvContext())
                {
                    var obj = ctx.tblMaterialApostilaAluno
                        .Where(x => x.intClientID == matricula && x.intMaterialApostilaID == idApostila)
                        .OrderBy(y => y.intClientID)
                        .FirstOrDefault();

                    ctx.tblMaterialApostilaAluno.Remove(obj);

                    ctx.SaveChanges();

                    return 1;
                }
            }
        }

        private bool ApagaApostilas(int idMaterial, int matricula)
        {
            using(MiniProfiler.Current.Step("Apaga as versões de apostila do aluno"))
            {
                using (var ctx = new DesenvContext())
                {
                    var idApostila = ctx.tblMaterialApostila
                        .Where(x => x.intProductId == idMaterial)
                        .Select(y => y.intID)
                        .FirstOrDefault();

                    if (idApostila == 0)
                        return false;

                    var query = ctx.tblMaterialApostilaAluno
                        .Where(x => x.intClientID == matricula && x.intMaterialApostilaID == idApostila);

                    query.ToList().ForEach(x => ctx.Entry(x).State = EntityState.Deleted);

                    var a = ctx.SaveChanges();

                    return true;
                }
            }
        }

        private bool IniciaProgressoApostila(int idMaterial, int matricula, DesenvContext ctx)
        {
            using(MiniProfiler.Current.Step("Registra progresso de apostila"))
            {
                var apostila = ctx.tblMaterialApostila.Any(x => x.intID == idMaterial);

                if (!apostila)
                    return false;

                var obj = new tblMaterialApostilaProgresso()
                {
                    dteDataAlteracao = DateTime.Now,
                    intApostilaID = idMaterial,
                    intClientID = matricula,
                    dblPercentProgresso = 0
                };

                ctx.tblMaterialApostilaProgresso.Add(obj);

                return ctx.SaveChanges() == 1;
            }
        }

        public bool RegistraProgresso(int idMaterial, int matricula)
        {
            using(MiniProfiler.Current.Step("Registra progresso de apostila"))
            {
                using (var ctx = new DesenvContext())
                {
                    var idOriginal = ctx.tblMaterialApostila
                        .Where(x => x.intProductId == idMaterial)
                        .Select(y => y.intID)
                        .FirstOrDefault();

                    if (idOriginal == 0)
                        return false;

                    ctx.tblMaterialApostilaProgresso.Add(new tblMaterialApostilaProgresso()
                    {
                        dteDataAlteracao = DateTime.Now,
                        dblPercentProgresso = 0,
                        intApostilaID = idOriginal,
                        intClientID = matricula
                    });

                    return ctx.SaveChanges() == 1;
                }
            }
        }

        private int AdicionaNovos(MaterialApostilaInteracoes comentarios)
        {
                        using(MiniProfiler.Current.Step("Adiciona novas interações de apostila"))
            {
                using (var ctx = new DesenvContext())
                {
                    var novos = comentarios
                                .Where(x => x.Id == 0)
                                .Select(x => new tblMaterialApostilaInteracao()
                                {
                                    intID = 0,
                                    intClientID = x.ClientId,
                                    txtInteracaoID = x.AnotacaoId,
                                    intApostilaID = x.ApostilaId,
                                    txtComentario = x.Comentario,
                                    txtConteudo = x.Conteudo,
                                    intVersaoMinima = x.VersaoMinima,
                                    intVersaoMaxima = 0,
                                    intTipoInteracao = x.TipoInteracao

                                })
                                .ToList();

                    novos.ForEach(x => ctx.tblMaterialApostilaInteracao.Add(x));

                    return ctx.SaveChanges();
                }
            }
        }

        private int AtualizaModificacoes(MaterialApostilaInteracoes interacoes)
        {
            using(MiniProfiler.Current.Step("Atualiza interações de apostila"))
            {
                using (var ctx = new DesenvContext())
                {
                    interacoes
                        .Where(x => x.Id != 0)
                        .ToList()
                        .ForEach(y =>
                        {
                            var anotacao = ctx.tblMaterialApostilaInteracao.FirstOrDefault(z => z.intID == y.Id);
                            anotacao.intVersaoMaxima = y.VersaoMaxima;
                            anotacao.txtComentario = y.Comentario;
                            anotacao.txtConteudo = y.Conteudo;
                            ctx.Entry(anotacao).State = EntityState.Modified;
                        });

                    return ctx.SaveChanges();
                }
            }
        }

        private void RestringeComentarios(int idApostila, int matricula, int versaoMinima)
        {
            using(MiniProfiler.Current.Step("Restringindo comentarios de apostila"))
            {
                using (var ctx = new DesenvContext())
                {
                    int versaoMaxima = ctx.tblMaterialApostilaAluno
                        .Where(x => x.intMaterialApostilaID == idApostila && x.intClientID == matricula)
                        .OrderByDescending(y => y.intID)
                        .Select(z => z.intID)
                        .FirstOrDefault();

                    ctx.tblMaterialApostilaInteracao
                        .Where(x => x.intApostilaID == idApostila && x.intClientID == matricula && x.intVersaoMinima > versaoMinima && x.intVersaoMaxima == 0)
                        .ToList()
                        .ForEach(y =>
                        {
                            y.intVersaoMaxima = versaoMaxima;
                            ctx.Entry(y).State = EntityState.Modified;
                        });

                    ctx.SaveChanges();
                }
            }
        }

        public List<int> GetChecklistsPratico(int matricula)
        {
            using(MiniProfiler.Current.Step("Obtendo checklists práticos"))
            {
                using (var ctx = new DesenvContext())
                {
                    var checklists = (from mat in ctx.tblMaterialApostila
                                    join maAluno in ctx.tblMaterialApostilaAluno
                                    on mat.intID equals maAluno.intMaterialApostilaID
                                    join pr in ctx.tblProducts 
                                    on mat.intProductId equals pr.intProductID
                                    where maAluno.intClientID == matricula
                                    && pr.intProductGroup2 == (int)Produto.Cursos.CPMED
                                    && pr.intProductGroup3 == (int)Utilidades.EProductsGroup1.CursoPratico
                                    select pr.intProductID )
                                    .ToList();

                    return checklists;
                }
            } 
        }

        public List<int> GetChecklistsExtrasLiberado()
        {
            using(MiniProfiler.Current.Step("Obtendo checklists extras liberados"))
            {
                using (var ctx = new DesenvContext())
                {
                    var checklists = (from mat in ctx.tblMaterialApostila
                                    join pr in ctx.tblProducts
                                    on mat.intProductId equals pr.intProductID
                                    join ml in ctx.tblLiberacaoApostila
                                    on pr.intProductID equals ml.intBookId
                                    where pr.intProductGroup2 == (int)Produto.Cursos.CPMED
                                    && ml.bitLiberado == true
                                    && pr.intProductGroup3 == (int)Utilidades.EProductsGroup1.ApostilaCpmed
                                    select pr.intProductID)
                                    .ToList();

                    return checklists;
                }
            }
        }

        public List<int> GetChecklistsExtrasLiberado(int ano)
        {
            using (var ctx = new DesenvContext())
            {
                var checklists = (from mat in ctx.tblMaterialApostila
                                  join pr in ctx.tblProducts
                                  on mat.intProductId equals pr.intProductID
                                  join ml in ctx.tblLiberacaoApostila
                                  on pr.intProductID equals ml.intBookId
                                  join b in ctx.tblBooks
                                  on pr.intProductID equals b.intBookID
                                  where pr.intProductGroup2 == (int)Produto.Cursos.CPMED
                                  && ml.bitLiberado == true
                                  && pr.intProductGroup3 == (int)Utilidades.EProductsGroup1.ApostilaCpmed
                                  && b.intYear == ano
                                  select pr.intProductID)
                                .ToList();

                return checklists;
            }
        }
     }
}
