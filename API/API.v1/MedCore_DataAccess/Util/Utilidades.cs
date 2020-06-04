using System;
using System.Linq;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.DTO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Mail;
using MedCore_DataAccess.Contracts.Enums;
using System.Threading;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Configuration;
using MedCore_API.MedMail;
using StackExchange.Profiling;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using MedCore_DataAccess.Repository;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections;
using System.Reflection;

namespace MedCore_DataAccess.Util
{
    public static class Utilidades
    {

        static int TempoDecorridoAno = 0;
        static int TempoDecorridoMeses = 4;
        static int TempoDecorridoSemanas = 1;
        static int TempoDecorridoDias = 1;
        static int TempoDecorridoHoras = 1;

        public const int NovasInteracoesDuvidasAcademicas = 60;
        public enum TipoAlunoInscricoes { Cortesia, ExAlunoOuAluno, NovoAluno, SugereRac, JaInscrito, SomenteRac, Cancelado, Nenhum, Inadimplente }

        public enum TipoSimulador { Cortesia, Combo, RAC, Normal }

        public enum TipoTermoInscricaoIntensivao { TermoRac, TermoIntensivao, ContratoIntensivao }

        public enum TipoPagtos { Boleto = 6, Cheque = 2 }

        public static string GetTempoDecorridoPorExtenso(DateTime b)
        {
            var date = SetTempoDecorrido(b);

            if (date.years > TempoDecorridoAno)
                return date.years > 1 ? date.years + " anos atrás" : "1 ano atrás";
            else if (date.months >= TempoDecorridoMeses)
                return date.months > 1 ? date.months + " meses atrás" : "1 mês atrás";
            else if (date.weeks >= TempoDecorridoSemanas)
                return date.weeks > 1 ? date.weeks + " semanas atrás" : "1 semana atrás";
            else if (date.days >= TempoDecorridoDias)
                return date.days > 1 ? date.days + " dias atrás" : "1 dia atrás";
            else if (date.hours >= TempoDecorridoHoras)
                return date.hours > 1 ? date.hours + " horas atrás" : "1 hora atrás";
            else if (date.mins == 0 || date.mins < 0)
                return "1 minuto atrás";
            else return date.mins == 0 ? "1 minuto atrás" : date.mins + " minutos atrás";
        }

        public static List<int> CursosAulasEspeciais()
        {
            return new List<int> {
                Produto.Cursos.MEDCURSO_AULAS_ESPECIAIS.GetHashCode(),
                Produto.Cursos.MED_AULAS_ESPECIAIS.GetHashCode()
            };
        }

        public static int GetCursoOrigemCursoAulaEspecial(int cursoId)
        {
            if (cursoId == Produto.Cursos.MEDCURSO_AULAS_ESPECIAIS.GetHashCode())
                return Produto.Cursos.MEDCURSO.GetHashCode();
            else if (cursoId == Produto.Cursos.MED_AULAS_ESPECIAIS.GetHashCode())
                return Produto.Cursos.MED.GetHashCode();
            else
                return cursoId;
        }

        public static int GetProdutoOrigemProdutoAulaEspecial(int produtoId)
        {
            if (produtoId == Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS.GetHashCode())
                return Produto.Produtos.MEDCURSO.GetHashCode();
            else if (produtoId == Produto.Produtos.MED_AULAS_ESPECIAIS.GetHashCode())
                return Produto.Produtos.MED.GetHashCode();
            else
                return produtoId;

        }

        public static string GetTempoDecorrido(DateTime b)
        {
            var date = SetTempoDecorrido(b);

            if (date.years > TempoDecorridoAno)
                return date.years + " a";
            else if (date.months >= TempoDecorridoMeses)
                return date.months + " m";
            else if (date.weeks >= TempoDecorridoSemanas)
                return date.weeks + " sem";
            else if (date.days >= TempoDecorridoDias)
                return date.days + " d";
            else if (date.hours >= TempoDecorridoHoras)
                return date.hours + " h";
            else if (date.mins == 0 || date.mins < 0)
                return "1 min";
            else return date.mins == 0 ? "1 min" : date.mins + " min";
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        private static TempoDecorrido SetTempoDecorrido(DateTime b)
        {
            var now = DateTime.Now;
            var date = new TempoDecorrido
            {
                now = now,
                secs = (now - b).TotalSeconds,
                mins = Math.Floor((now - b).TotalMinutes),
                hours = Math.Floor((now - b).TotalHours),
                days = Math.Floor((now - b).TotalDays),
                weeks = Math.Floor((now - b).TotalDays / 7),
                months = ((now.Year - b.Year) * 12) + now.Month - b.Month,
                years = (((now.Year - b.Year) * 12) + now.Month - b.Month) > 12 ? now.Year - b.Year : 0

            };
            return date;
        }

        public static string GetNickName(string txtName)
        {
            var resultado = txtName.Replace("teste-firefox", "")
                .Replace("teste-ie", "")
                .Replace("teste", "")
                .Replace("-teste", "")
                .Replace("- teste", "")
                .Replace("teste", "")
                .Replace("-", "");

            var fullName = resultado.Trim();
            string[] name = fullName.Split(' ');
            string primeiro = name[0];
            string ultimo = name[name.Length - 1];
            string nick = String.Concat(primeiro.Trim(), ultimo.Trim());
            return nick;
        }

        public const int AnoMinimoParaCssFatiado = 2019;
        public const int NotificacaoPrimeiraAulaDiasAntecedenciaPadrao = 1;
        public const string TituloEmailDuvidasAcademicas = "MEDSOFT PRO - Dúvidas Acadêmicas";
        public const int AnoLancamentoMedsoftPro = 2017;
        public const string DefaultEmailProfile = "ses";
        public const int AnoLancamentoMaterialMedEletro = 2019;
        public const int AnoLancamentoMaterialIntensivao = 2019;
        public const int AnoLancamentoMaterialCPMed = 2019;
        public const string UltimaVersaoIonic1 = "4.1.18";

        public static int UsuarioSistema = 131220;//EmployeeId Sistema

        public static List<int> ProdutosExtensivo()
        {
            return new List<int>() {
            Produto.Produtos.MEDCURSO.GetHashCode(),
            Produto.Produtos.MEDCURSOEAD.GetHashCode(),
            Produto.Produtos.MED.GetHashCode(),
            Produto.Produtos.MEDEAD.GetHashCode(),
            Produto.Produtos.MED_MASTER.GetHashCode()
            };
        }            

        public static string GetEstadoCursoAluno(int matricula)
        {
            try
            {
                var ano = DateTime.Now.Year;
                using (var ctx = new DesenvContext())
                {
                    var order = (from a in ctx.tblSellOrders
                                 join p in ctx.tblPersons on a.intClientID equals p.intContactID
                                 join cp in ctx.tblCities on p.intCityID equals cp.intCityID
                                 join gp in ctx.tblStates on cp.intState equals gp.intStateID
                                 join b in ctx.tblSellOrderDetails on a.intOrderID equals b.intOrderID
                                 join c in ctx.tblProducts on b.intProductID equals c.intProductID
                                 join d in ctx.tblCourses on c.intProductID equals d.intCourseID
                                 join e in ctx.tblStores on a.intStoreID equals e.intStoreID
                                 join f in ctx.tblCities on e.intCityID equals f.intCityID
                                 join g in ctx.tblStates on f.intState equals g.intStateID
                                 where a.intClientID == matricula && d.intYear == ano
                                 select new EstadoOrigemAlunoDTO
                                 {
                                     DataOrdem = a.dteDate.Value,
                                     EstadoCurso = g.txtCaption,
                                     EstadoUsuario = gp.txtCaption
                                 }).OrderByDescending(x => x.DataOrdem).FirstOrDefault();


                    if (order == null)
                    {
                        return Constants.OrigemOutras;
                    }
                    if (order.EstadoCurso != null && order.EstadoCurso != Constants.OrigemOutras)
                    {
                        return order.EstadoCurso;
                    }
                    return order.EstadoUsuario == Constants.OrigemOutras || order.EstadoUsuario == null ? Constants.OrigemOutras : order.EstadoUsuario;
                }
            }
            catch
            {
                return Constants.OrigemOutras;
            }
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {

            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return origin.AddSeconds(timestamp);

        }

        public static DateTime DataToleranciaTestes()
        {
            return new DateTime(2020, 11, 10);
        }

        public static long ToUnixTimespan(DateTime date)
        {
            TimeSpan tspan = date.Subtract(
               new DateTime(1970, 1, 1, 0, 0, 0));

            return (long)Math.Truncate(tspan.TotalSeconds);
        }

        public static bool IsExAlunoTodosProdutos(int matricula)
        {
            var ctx = new DesenvContext();
            var anoAtual = GetYear();
            var alunoMeioDeAno = IsCicloCompletoNoMeioDoAno(matricula);
            var alunoAnoAtual = (from so in ctx.tblSellOrders
                                 join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                                 join prod in ctx.tblProducts on sod.intProductID equals prod.intProductID
                                 join c in ctx.tblCourses on prod.intProductID equals c.intCourseID
                                 where so.intClientID == matricula
                                     && anoAtual == c.intYear
                                     && (so.intStatus == (int)ESellOrderStatus.Ativa
                                         || (so.intStatus == (int)ESellOrderStatus.Cancelada
                                             && alunoMeioDeAno
                                         )
                                     )
                                 select so).Distinct().Count() > 0;


            return !alunoAnoAtual;
        }

        public static bool IsSiteRecursosInativo()
        {
            return (Utilidades.IsActiveFunction(Utilidades.Funcionalidade.RecursosIphone_BloqueiaPosts));
        }

        public static bool IsSimuladoRMais(int idTipoSimualdo)
        {
            var TipoR = new[] {
                    (int) Constants.TipoSimulado.R3_Clinica,
                    (int) Constants.TipoSimulado.R3_Cirurgia,
                    (int) Constants.TipoSimulado.R3_Pediatria,
                    (int) Constants.TipoSimulado.R4_GO
                }.ToList();

            return TipoR.Contains(idTipoSimualdo);
        }

        public static bool IsAWord(this string text)
        {
            var regex = new Regex(@"\b[\w']+\b");
            var match = regex.Match(text);
            return match.Value.Equals(text);
        }
        public static Int32 GetYear()
        {
            int? result = null;
            bool cacheEnabled = !RedisCacheManager.CannotCache(RedisCacheConstants.Utilidades.KeyGetYear);

            if (cacheEnabled)
                result = RedisCacheManager.GetItemObject<int?>(RedisCacheConstants.Utilidades.KeyGetYear);

            if (!result.HasValue || result.Value == 0)
            {
                using (var ctx = new DesenvContext())
                {
                    var anoLetivo = ctx.tblAccess_Year_Type.Where(x => x.intTipoAnoId == 2).FirstOrDefault();
                    result = anoLetivo.intAno;
                    RedisCacheManager.SetItemObject(RedisCacheConstants.Utilidades.KeyGetYear, result);
                }
            }

            return (result.HasValue) ? result.Value : 0;
        }

        public static bool LimpaRegistroPraTesteUnitario(string nomeTabela, string parametros, bool isAcademicoRDS = false)
        {
            try
            {
                var query = String.Format("delete from {0} where {1}", nomeTabela, parametros);
                var retorno = new DBQuery().ExecuteQuery(query, isAcademicoRDS);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public static bool IsCicloCompletoNoMeioDoAno(int matricula)
        {
            var ctx = new DesenvContext();
            var meioDeAno = ctx.tblAlunosAnoAtualMaisAnterior.Where(x => x.intClientID == matricula).Any();
            return meioDeAno;
        }

        public static DateTime GetServerDate(int intState = -1)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetServerDate))
                return DateTime.Now;

            var ctx = new DesenvContext();
            
            var data = (ctx.Set<csp_getServerDate_Result>().FromSqlRaw("csp_getServerDate @intStateID = {0}", intState).ToList().FirstOrDefault() ?? ctx.Set<csp_getServerDate_Result>().FromSqlRaw("csp_getServerDate @intStateID = {0}", -1).ToList().FirstOrDefault());
            return data.serverDate;
        }

        public static bool GetChaveamento()
        {
            string chaveUsarRDS = ConfigurationProvider.Get("Settings:AtivaSimuladoRDS");
            return (!string.IsNullOrEmpty(chaveUsarRDS) && Convert.ToBoolean(chaveUsarRDS));
        }

        public static int SendMailDirect(string to, string body, string subject, string profileName, string mailFrom = "", string nomeMailFrom = "", List<Attachment> anexos = null)
        {
            try
            {
                var listEmail = new List<String>();
                listEmail.Add(to);
                var e = new SendEmail(listEmail, subject, body, mailFrom, nomeMailFrom);

                if(anexos != null && anexos.Any())
                {
                    anexos.ForEach(a => e.AddAttachment(a));
                }

                e.Send();

                MedmailContext ctx = new MedmailContext();
                var mail_queque = new mail_queue_items
                {
                    profile_id = profileName,
                    recipients = to,
                    subject = subject,
                    body = body,
                    body_format = "HTML",
                    status = 1,
                    RetryCount = 1,
                    date_queued = DateTime.Now
                };

                ctx.mail_queue_items.Add(mail_queque);
                ctx.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        public static string CriarNomeApostila(int matricula, int idApostila, long versao)
        {
            var titulo = "MAT{0}_IDAPOST{1}_V{2}";
            return string.Format(titulo, matricula, idApostila, versao);
        }

        public static MaterialApostilaAluno GetDetalhesApostila(tblMaterialApostilaAluno tblMaterial)
        {
            var partes = tblMaterial.txtApostilaNameId.Split(new string[] { "_" }, StringSplitOptions.None);

            return new MaterialApostilaAluno
            {
                ClientId = Convert.ToInt32(GetStringAfter(partes.First(), "MAT")),
                Id = Convert.ToInt32(GetStringAfter(partes[1], "IDAPOST")),
                Versao = Convert.ToInt64(GetStringAfter(partes.Last(), "V"))
            };
        }

        public static string GetStringAfter(this string value, string a)
        {
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }

        public static int GetNumeroSemanaAtual(DateTime dt)
        {
            System.Globalization.CultureInfo ciCurr = System.Globalization.CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public enum ESellOrderStatus
        {
            Pendente = 0,
            Suspensa = 1,
            Ativa = 2,
            Falha = 3,
            Cancelada = 5,
            Adimplente = 6,
            Inadimplente = 7,
            Carencia = 8,
            InadimplenteMesesAnteriores = 9
        }

        public enum EProductsGroup1
        {
            Nenhuma = 0,
            Cardiologia = 21,
            Cirurgia = 22,
            Dermatologia = 23,
            Endocrinologia = 24,
            Gastroenterologia = 25,
            Ginecologia = 26,
            Hematologia = 27,
            Hepatologia = 28,
            Infectologia = 29,
            MedicinaPreventiva = 30,
            Nefrologia = 31,
            Neurologia = 32,
            Obstetrícia = 33,
            Oftalmologia = 34,
            Ortopedia = 35,
            Otorrinolaringologia = 36,
            Pediatria = 37,
            Pneumologia = 38,
            Psiquiatria = 39,
            Reumatologia = 40,
            ApostilaCLM = 54,
            CursoPratico = 51,
            ApostilaCpmed = 53
        }

        public enum ProductGroups
        {
            MATERIALDIDATICO = 4,
            INTENSIVO = 14,
            RAC = 58,
            RACIPE = 61,
            CPMED = 51,
            MED = 5,
            MEDCURSO = 1,
            MEDEAD = 8,
            MEDCURSOEAD = 9,
            R3_CLINICA = 76,
            R3_CIRURGIA = 81,
            R3_PEDIATRIA = 82,
            R4_GO = 83,
            MEDELETRO = 57,
            APOSTILA_CPMED = 53,
            ECOMMERCE_MEDERI = 87,
            ECOMMERCE_MEDWRITERS = 86,
            ECOMMERCE_MEDYKLIN = 84,
            ECOMMERCE_MEDYN = 85,
            REVALIDA = 63,
            ADAPTAMED = 72,
            MEDELETRO_IMED = 88,
            APOSTILA_MEDELETRON = 56,
            RAC_IMED = 91,
            RACIPE_IMED = 92,
            APOSTILA_MED_MEDCURSO = 47,
            APOSTILA_MEDCURSO = 16,
            APOSTILA_MED = 17, 
            R4_MASTOLOGIA = 94,
            TEGO = 93,
            MedMaster = 98
        }

        public enum Filiais
        {
            BeloHorizonte = 17,
            PortoAlegre = 18,
            Campos = 19,
            Curitiba = 20,
            RiodeJaneiro = 21,
            Vitória = 22,
            Salvador = 23,
            RibeirãoPreto = 24,
            Marília = 25,
            PresidentePrudente = 26,
            Florianópolis = 27,
            SãoPaulo = 31,
            Recife = 32,
            JuizdeFora = 35,
            Belém = 36,
            Brasília = 37,
            Goiânia = 38,
            Fortaleza = 39,
            Uberlândia = 40,
            Pelotas = 41,
            JoãoPessoa = 44,
            Maceió = 45,
            CampoGrande = 46,
            Cuiabá = 47,
            Natal = 48,
            Manaus = 49,
            Teresina = 50,
            Uberaba = 52,
            SãoLuis = 56,
            JuazeirodoNorte = 61,
            PousoAlegre = 66,
            SantaMaria = 69,
            SãoJosédoRioPreto = 72,
            Campinas = 75,
            Sorocaba = 77,
            Botucatu = 79,
            Santos = 80,
            VoltaRedonda = 82,
            Niterói = 90,
            Teresópolis = 92,
            Taubaté = 93,
            Petrópolis = 94,
            Aracaju = 95,
            Itaperuna = 96,
            EADAssinaturas = 97,
            Alfenas = 102,
            BragançaPaulista = 105,
            Fernandópolis = 106,
            Jundiaí = 107,
            Londrina = 108,
            Vassouras = 110,
            Camboriú = 111,
            Criciúma = 112,
            CampinaGrande = 114,
            RioBranco = 120,
            Sobral = 121,
            PortoVelho = 122,
            Araguaína = 123,
            RioGrande = 124,
            PassoFundo = 125,
            Blumenau = 126,
            Maringá = 127,
            Itajubá = 128,
            MontesClaros = 129,
            Colatina = 130,
            Itabuna = 131,
            Catanduva = 132,
            Joaçaba = 133,
            Cascavel = 134,
            Lages = 135,
            CaxiasdoSul = 136,
            Joinville = 137,
            Dourados = 138,
            SãoCarlos = 140,
            FeiradeSantana = 141,
            VitóriadaConquista = 142,
            BoaVista = 143,
            Petrolina = 144,
            Valença = 145,
            Paracatu = 148,
            Cacoal = 151,
            Mossoró = 152,
            Chapecó = 153,
            Ipatinga = 156,
            SãoMateus = 157,
            Passos = 158,
            Araguari = 162,
            Palmas = 168,
            SantaCruzdoSul = 169,
            Santarém = 170,
            Tubarão = 171,
            PoçosdeCaldas = 173,
            Araraquara = 174,
            Anápolis = 177,
            PatosdeMinas = 178,
            Divinópolis = 179,
            PontaPorã = 180,
            Limeira = 181
        }

        public static string RemoveMarcacaoApostila(string comp, string conteudo, string selectionId)
        {
            var troca = true;
            var TagDuvidaCompNaoEncontrada = -1;
            var TagFecharDuvidaCompNaoEncontrada = -1;

            var comptag = string.Format(comp, selectionId);
            var closetag = "</comp>";

            while (troca)
            {
                var index = conteudo.IndexOf(comptag);
                var conteudoOriginal = conteudo;
                var indexClosetag = conteudo.IndexOf(closetag, index);

                conteudo = (index < TagDuvidaCompNaoEncontrada || indexClosetag < TagFecharDuvidaCompNaoEncontrada)
                   ? conteudo
                   : conteudo.Remove(indexClosetag, closetag.Length);

                conteudo = (index < TagDuvidaCompNaoEncontrada)
                   ? conteudo
                   : conteudo.Remove(index, comptag.Length);

                troca = conteudo.IndexOf(comptag) > 0;
            }

            return conteudo;
        }

        public static List<int> ProdutosR3()
        {
            return new List<int>() {
            Produto.Produtos.R3CIRURGIA.GetHashCode(),
            Produto.Produtos.R3CLINICA.GetHashCode(),
            Produto.Produtos.R3PEDIATRIA.GetHashCode(),
            Produto.Produtos.R4GO.GetHashCode()
            };
        }

        public static List<int> CursosR3()
        {
            return new List<int>() {
            Produto.Cursos.R3Cirurgia.GetHashCode(),
            Produto.Cursos.R3Clinica.GetHashCode(),
            Produto.Cursos.R3Pediatria.GetHashCode(),
            Produto.Cursos.R4GO.GetHashCode(),
            Produto.Cursos.MASTO.GetHashCode(),
            Produto.Cursos.TEGO.GetHashCode()
            };
        }

        public static TipoLayoutMainMSPro GetTipoLayoutMain(Produto.Cursos curso)
        {
            switch (curso)
            {
                case Produto.Cursos.MEDELETRO:
                case Produto.Cursos.MEDELETRO_IMED:
                case Produto.Cursos.INTENSIVAO:
                case Produto.Cursos.RAC_IMED:
                case Produto.Cursos.RACIPE_IMED:
                    return TipoLayoutMainMSPro.WEEK_SINGLE;
                case Produto.Cursos.REVALIDA:
                    return TipoLayoutMainMSPro.REVALIDA;
                case Produto.Cursos.CPMED:
                    return TipoLayoutMainMSPro.SHELF;
                case Produto.Cursos.CPMED_EXTENSIVO:
                case Produto.Cursos.MED_AULAS_ESPECIAIS:
                case Produto.Cursos.MEDCURSO_AULAS_ESPECIAIS:
                case Produto.Cursos.R3Cirurgia:
                case Produto.Cursos.R3Clinica:
                case Produto.Cursos.R3Pediatria:
                case Produto.Cursos.R4GO:
                case Produto.Cursos.MASTO:
                case Produto.Cursos.TEGO:
                    return TipoLayoutMainMSPro.MIXED;
                default:
                    return TipoLayoutMainMSPro.WEEK_DOUBLE;
            }
        }

        public static List<int?> GrupoProduto(int produtoId)
        {
            var produtos = new List<int?>();
            if (produtoId == (int)Utilidades.ProductGroups.MED || produtoId == (int)Utilidades.ProductGroups.CPMED || produtoId == (int)Utilidades.ProductGroups.MEDEAD)
            {
                produtos.Add((int)Utilidades.ProductGroups.MED);
                produtos.Add((int)Utilidades.ProductGroups.CPMED);
                produtos.Add((int)Utilidades.ProductGroups.MEDEAD);
            }
            else if (produtoId == (int)Utilidades.ProductGroups.MEDCURSO || produtoId == (int)Utilidades.ProductGroups.MEDCURSOEAD)
            {
                produtos.Add((int)Utilidades.ProductGroups.MEDCURSO);
                produtos.Add((int)Utilidades.ProductGroups.MEDCURSOEAD);
            }
            else
            {
                produtos.Add(produtoId);
            }

            return produtos;
        }

        public static List<int> CicloCompletoAnosAnterioresNoMeioDoAno(int matricula)
        {
            var ctx = new DesenvContext();
            var meioDeAno = ctx.tblMeiodeAnoAlunosAnosAnteriores.Where(o => o.intClientID == matricula).Select(s => s.intYear.Value).ToList();
            return meioDeAno;
        }

        public static List<int> AlunosMeioDeAnoAtuais()
        {
            var ctx = new DesenvContext();
            var alunosMeiodeAno = ctx.tblAlunosAnoAtualMaisAnterior.Select(x => x.intClientID).ToList();
            return alunosMeiodeAno;
        }

        public static string GetEspecialidadePorProductGroup1(int ProductGroup1)
        {
            switch ((EProductsGroup1)ProductGroup1)
            {
                case EProductsGroup1.Cardiologia:
                case EProductsGroup1.Endocrinologia:
                case EProductsGroup1.Gastroenterologia:
                case EProductsGroup1.Hematologia:
                case EProductsGroup1.Hepatologia:               
                case EProductsGroup1.Infectologia:
                case EProductsGroup1.Nefrologia:
                case EProductsGroup1.Neurologia:
                case EProductsGroup1.Pneumologia:
                case EProductsGroup1.Reumatologia:
                case EProductsGroup1.ApostilaCLM:
                    return "CLM";
                case EProductsGroup1.Cirurgia:
                    return "CIR";
                case EProductsGroup1.Pediatria:
                    return "PED";
                case EProductsGroup1.Ginecologia:
                case EProductsGroup1.Obstetrícia:
                    return "GO"; ;
                case EProductsGroup1.MedicinaPreventiva:
                    return "PRE";
                default:
                    return string.Empty;
            }
        }

        public static string VersaoMinimaImpressaoSimulado(int idAplicacao)
        {
            using(MiniProfiler.Current.Step("Obtendo Versão mínima impressão simulado"))
            {
                return idAplicacao == (int)Aplicacoes.MEDSOFT_PRO_ELECTRON ?
                                        ConfigurationProvider.Get("Settings:VersaoMinimaMsProDesktopSimuladoImpressao"):
                                        ConfigurationProvider.Get("Settings:VersaoMinimaMsProSimuladoImpressao");
            }
        }

        public static bool VersaoMenorOuIgual(string referencia, string comparacao)
        {
            var arr1 = referencia.Split('.');
            var arr2 = comparacao.Split('.');

            if (Convert.ToInt32(arr1[0]) != Convert.ToInt32(arr2[0])) // change break
                return Convert.ToInt32(arr1[0]) < Convert.ToInt32(arr2[0]);

            if (Convert.ToInt32(arr1[1]) != Convert.ToInt32(arr2[1])) // feature
                return Convert.ToInt32(arr1[1]) < Convert.ToInt32(arr2[1]);

            return Convert.ToInt32(arr1[2]) <= Convert.ToInt32(arr2[2]); // hotfix
        }

        public static int GetAnoInscricao(Aplicacoes aplicacao)
        {
            var anoInscricao = 0;

            switch (aplicacao)
            {
                case Aplicacoes.INSCRICAO_REVALIDA:
                    anoInscricao = Constants.anoInscricaoRevalida;
                    break;

                case Aplicacoes.INSCRICAO_EXTENSIVO:
                    anoInscricao = ObterAnoInscricao((int)Aplicacoes.INSCRICAO_EXTENSIVO);
                    break;

                case Aplicacoes.INSCRICAO_CPMED:
                    anoInscricao = ObterAnoInscricao((int)Aplicacoes.INSCRICAO_CPMED);
                    break;

                case Aplicacoes.INSCRICAO_INTENSIVO:
                    anoInscricao = ObterAnoInscricao((int)Aplicacoes.INSCRICAO_INTENSIVO);
                    break;

                case Aplicacoes.INSCRICAO_ADAPTAMED:
                    anoInscricao = Constants.anoInscricaoAdaptaMed;
                    break;

                case Aplicacoes.INSCRICAO_MEDELETRO:
                case Aplicacoes.MEDELETRO:
                    anoInscricao = ObterAnoInscricao((int)Aplicacoes.MEDELETRO);
                    break;

                case Aplicacoes.Recursos:
                case Aplicacoes.Recursos_Android:
                case Aplicacoes.Recursos_iPad:
                case Aplicacoes.Recursos_iPhone:
                    anoInscricao = Constants.anoRecursos;
                    break;

                default:
                    break;
            }

            return anoInscricao;
        }

        public static int ObterAnoInscricao(int aplicacao)
        {
            using (var ctx = new DesenvContext())
            {
                var anoInscricao = ctx.tblAnoInscricao.Where(x => x.intApplicationID == aplicacao).ToList().FirstOrDefault();
                return anoInscricao.intAno;
            };
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

       public static string GcmKeyNotificacoes(Aplicacoes aplicacao)
        {
            var dados = new Dictionary<Aplicacoes, string>
            {
                { Aplicacoes.MsProMobile, "oneSignalGcmKey" },
                { Aplicacoes.Recursos, "recursosOneSignalGcmKey" }
            };
            return dados[aplicacao];
        }

        public static bool IsIntensivo(int produtoId)
        {
            var idsIntensivo = new int[] {(int)Produto.Produtos.INTENSIVAO,
                                          (int)Produto.Produtos.RAC,
                                          (int)Produto.Produtos.RACIPE,
                                          (int)Produto.Produtos.RAC_IMED,
                                          (int)Produto.Produtos.RACIPE_IMED };

            return idsIntensivo.Contains(produtoId);
        }

        public static string GetDuracaoFormatada(string duracao)
        {
            // http://www.calculatorsoup.com/calculators/time/decimal-to-time-calculator.php
            try
            {
                if (String.IsNullOrEmpty(duracao)) return "00:00:00";
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                var convdec = ConvertToFloat(duracao);

                var horas = convdec / 3600;
                var hSplit = Math.Round(horas, 6).ToString().Split('.');

                var minuto = ConvertToFloat(String.Concat("0.", (hSplit.Count() > 1 ? hSplit[1] : 0.ToString()))) * 60;
                var mSplit = Math.Round(minuto, 6).ToString().Split('.');

                var segundo = ConvertToFloat(String.Concat("0.", (mSplit.Count() > 1 ? mSplit[1] : 0.ToString()))) * 60;
                var sSplit = Math.Round(segundo, 6).ToString().Split('.');

                return String.Concat(
                    String.Format("{0:00}", Convert.ToInt32(hSplit[0])), ":",
                    String.Format("{0:00}", Convert.ToInt32(mSplit[0])), ":",
                    String.Format("{0:00}", Convert.ToInt32(sSplit[0])));
            }
            catch (Exception)
            {
                return "00:00:00";
            }
        }

        public static string RemoveHtml(string txtTexto)
        {
            if (txtTexto == null)
            {
                return "";
            }
            var txtFinal = "";
            var strLimpa = txtTexto;
            strLimpa = strLimpa.Replace("<br>", "evNL").Replace("<br />", "evNL").Trim();
            //strLimpa = Regex.Replace(strLimpa.Trim(), @"(<(?<t>script|object|applet|embbed|frameset|iframe|form|textarea)(\\s+.*?)?>.*?</\\k<t>>)" + "|(<(script|object|applet|embbed|frameset|iframe|form|input|button|textarea)(\\s+.*?)?/?>)"+ "|((?<=<\\w+)((?:\\s+)((?:on\\w+=((\"[^\"]*\")|('[^']*')|(.*?)))|(?<a>(?!on)\\w+=((\"[^\"]*\")|('[^']*')|(.*?)))))*(?=/?>))", "", RegexOptions.Singleline);
            //strLimpa = Regex.Replace(strLimpa.Trim(), @"(<(!?)(--)*(.*?)(--)*>)|.*{(.*?)(})|(\/\*)(.*?)(\*\/)|&lt;!--(.*?)--&gt;", "", RegexOptions.Singleline);
            strLimpa = Regex.Replace(strLimpa.Trim(), @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", "", RegexOptions.Singleline);

            strLimpa = strLimpa.Replace("&lt;!-- /* Font Definitions */ /* List Definitions */ @list l0 {mso-list-id:852769245; mso-list-type:hybrid; mso-list-template-ids:849005656 67698703 67698713 67698715 67698703 67698713 67698715 67698703 67698713 67698715;} @list l0:level1 {mso-level-tab-stop:none; mso-level-number-position:left; text-indent:-18.0pt;} @list l1 {mso-list-id:1237520126; mso-list-type:hybrid; mso-list-template-ids:-1830809156 67698689 67698691 67698693 67698689 67698691 67698693 67698689 67698691 67698693;} @list l1:level1 {mso-level-number-format:bullet; mso-level-text:?; mso-level-tab-stop:none; mso-level-number-position:left; text-indent:-18.0pt; font-family:Symbol;} ol {margin-bottom:0cm;} ul {margin-bottom:0cm;} --&gt;", "");
            strLimpa = strLimpa.Replace("&lt;!-- /* Font Definitions */ --&gt;", "");
            strLimpa = strLimpa.Replace("&lt;!--  /* Font Definitions */  --&gt;", "");
            strLimpa = strLimpa.Replace("/* Style Definitions */", "");
            strLimpa = strLimpa.Replace("ol { margin-bottom: 0cm; }ul { margin-bottom: 0cm; }", "");
            strLimpa = strLimpa.Replace("style=\"margin: 0cm 0cm 0pt -2cm;\"", "");

            strLimpa = Regex.Replace(strLimpa.Trim(), @"^\s*(-->|--&gt;|&nbsp;+|evNL+)", "", RegexOptions.Singleline);
            strLimpa = Regex.Replace(strLimpa, @"<[/]?(font|span|xml|del|ins|[ovwxp]:\w+)[^>]*?>", "", RegexOptions.IgnoreCase);
            strLimpa = Regex.Replace(strLimpa, @"<([^>]*)(?:class|lang|style|size|face|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "", RegexOptions.IgnoreCase);
            strLimpa = Regex.Replace(strLimpa, @"<([^>]*)(?:class|lang|style|size|face|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "", RegexOptions.IgnoreCase);
            strLimpa = strLimpa.Trim().Replace("evNL", "<br />");
            txtFinal = strLimpa.Trim();

            return txtFinal;
        }

        public static string FormatarEnunciadoComentario(string enunciado, string comentario, string id)
        {
            var enunciadoFormatado = enunciado;
            enunciadoFormatado += comentario != null ? "<br/><br/><b>Comentário da Questão (" + id + "):</b><br/><br/>" + comentario : string.Empty;
            return enunciadoFormatado;
        }

        public static bool IsActiveFunction(Enum func)
        {
            try
            {
                var query = String.Format("select count(I.intInterruptorId) as [ativo] from tblInterruptor I where I.intInterruptorCatalogoId = {0} and I.bitAtivo = 1", Convert.ToInt32(func));
                var retorno = new DBQuery().ExecuteQuery(query).Tables[0].Rows[0][0];
                return Convert.ToBoolean(retorno);
            }
            catch
            {
                throw;
            }
        }

        public static string RemoveHtmlEMantemNegritoEPuloDeLinha(string txtTexto)
        {
            var texto = txtTexto.Replace("<b>", "[b]").Replace("</b>", "[/b]").Replace("<strong>", "[b]").Replace("</strong>", "[/b]");
            texto = RemoveHtml(texto);
            texto = texto.Replace("[b]", "<b>").Replace("[/b]", "</b>");

            return texto;
        }

        public static List<int> ConvertStringToListInt(string lista)
        {
            return lista.Split(';').Where(p => p.Trim().ToLower() != String.Empty).Select(Int32.Parse).ToList();
        }

        private static float ConvertToFloat(string valor)
        {
            return float.Parse(valor, CultureInfo.GetCultureInfo("en-US"));
        }

        public static string RetirarCaracteresEspeciais(string texto)
        {
            using (var ctx = new DesenvContext())
            {
                var query = ctx.tblCodigoCaracteres.Select(a => a.txtCodigoCaracter).ToList();

                foreach (string code in query)
                {
                    var caracter = Convert.ToChar(int.Parse(code.Replace("&H", ""), System.Globalization.NumberStyles.HexNumber));

                    texto = texto.Replace(caracter.ToString(), " ");
                }

                return texto;
            }
        }

        private static string RetirarTagsEspecificas(string texto)
        {
            using (var ctx = new DesenvContext())
            {
                var tags = (from c in ctx.tblReplaceHtmlTags
                            select new
                            {
                                tag = c.txtTag,
                                replace = c.txtReplace
                            }).ToList();

                foreach (var t in tags)
                {
                    texto = texto.Replace(t.tag, t.replace);
                }

                return texto;
            }
        }

        public static StringCollection GetTags()
        {
            using (var ctx = new DesenvContext())
            {
                StringCollection sc = new StringCollection();

                //Limpeza de htmls dentro de p, u, ul, li e outros
                var tags = (from c in ctx.tblCleanHtmlTags
                            select new
                            {
                                tagInicio = c.txtTagInicio,
                                tagFinal = c.txtTagFinal
                            }).ToList();

                foreach (var t in tags)
                {
                    sc.Add(String.Format("(?<={0}) .+?(?={1})", t.tagInicio, t.tagFinal));
                }

                return sc;
            }
        }

        public static string CleanHtml(string texto)
        {
            texto = HttpUtility.HtmlDecode((texto ?? String.Empty).ToString());
            texto = RetirarCaracteresEspeciais(texto);
            texto = RetirarTagsEspecificas(texto);

            // Regex para retirar algumas tags e limpar atributos internos delas, mantendo apenas as tags negrito, italico, sublinhado e pulo de linha.
            StringCollection sc = GetTags();

            // get rid of unnecessary tag spans (comments and title)
            sc.Add("<!--(\\w|\\W)+?-->");
            sc.Add("<title>(\\w|\\W)+?</title>");
            sc.Add("<style>(\\w|\\W)+?</style>");

            // Get rid of classes and styles
            sc.Add("\\s?class=\\w+");
            sc.Add("\\s+style='[^']+'");

            // Retirar styles do LibreOffice
            sc.Add("\\s+style=\"[^\"]+\"");

            // Get rid of unnecessary tags
            sc.Add("<(meta|link|/?o:|/?style|/?div|/?st\\d|/?head|/?html|body|/?body|/?span|!\\[)[^>]*?>");

            // Get rid of empty paragraph tags
            sc.Add("(<[^>]+>)+&nbsp;(</\\w+>)+");

            // remove bizarre v: element attached to <img> tag
            sc.Add("\\s+v:\\w+=\"[^\"]+\"");

            // remove extra lines
            sc.Add("(\\n\\r){2,}");

            foreach (string s in sc)
            {
                texto = Regex.Replace(texto, s, "", RegexOptions.IgnoreCase);
            }

            texto = RetirarTagsEspecificas(texto);
            texto = LimparExcessoEspacos(texto);
            texto = TrataMaiorIgualMenorIgual(texto);

            return texto;
        }

        public static int GetEnumDescription<T>(string value)
        {
            Type type = typeof(T);
            var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

            if (String.IsNullOrEmpty(name))
                return 0;

            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ToInt32(customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name);
        }

        public static int ToInt32(object obj)
        {
            string convert = (obj ?? String.Empty).ToString();

            int result = 0;
            int.TryParse(convert, out result);
            return result;
        }
        
        private static string TrataMaiorIgualMenorIgual(string texto)
        {
            texto = texto.Replace("&ge;", "≥");
            texto = texto.Replace("&le;", "≤");
            texto = texto.Replace("<u>></u>", "≥");
            texto = texto.Replace("<u><</u>", "≤");
            texto = texto.Replace(">=", "≥");
            texto = texto.Replace("<=", "≤");
            texto = texto.Replace("\"Maior ou Igual a\"", "≥");
            texto = texto.Replace("\"Maior ou igual a\"", "≥");
            texto = texto.Replace("\"maior ou Igual a\"", "≥");
            texto = texto.Replace("\"maior ou igual a\"", "≥");
            texto = texto.Replace("\"Menor ou Igual a\"", "≤");
            texto = texto.Replace("\"Menor ou igual a\"", "≤");
            texto = texto.Replace("\"menor ou Igual a\"", "≤");
            texto = texto.Replace("\"menor ou igual a\"", "≤");

            return texto;
        }

        public static List<int> GetResponsibilitiesAcademic()
        {
            var ctx = new DesenvContext();
            return ctx.tblSysRoles.Where(a => a.txtDescription.Contains("academico") && a.intResponsabilityID >= 165).Select(b => b.intResponsabilityID).ToList();
        }

        private static string LimparExcessoEspacos(string texto)
        {
            while (texto.IndexOf("  ") >= 0)
            {
                texto = texto.Replace("  ", " ");
            }

            return texto;
        }

        public enum VideoThumbSize
        {
            width_40 = 40,
            width_120 = 120,
            width_320 = 320,
            width_480 = 480,
            width_720 = 720,
            width_1280 = 1280,
            width_1920 = 1920
        }

        public enum CatalogoClassificacao
        {
            MEDCURSO = 4,
            MED = 5,
            CPMED = 11,
            ECG = 8,
            SUBECG = 9,
            RACIPE = 10,
            ADAPTAMED = 12,
            R3CLINICA = 13,
            R3CIRURGIA = 14,
            R3PEDIATRIA = 15,
            R4GO = 16
        }

        public enum Funcionalidade
        {
            AlunoInadimplente = 1,
            DownloadConteudoPosLoginMedsoftDesktop = 2,
            RecursosIphone_BloqueiaPosts = 11,
            ValidaAlunoCancelado = 13,
            AlunoProximoAnoCanceladoAnoAtual = 14,
            MedreaderBloqueio = 15
        }

        public static string FormatDataTime(DateTime data)
        {
            return data.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return string.Empty;
        }

        public static string GetNomeResumido(string nomeCompleto)
        {
            var nomesNaoConsiderados = new List<string>() { "teste", "neto", "junior", "jr", "filho", "de", "da", "desligado" };
            var nomeResumido = string.Empty;

            if (!string.IsNullOrEmpty(nomeCompleto))
            {
                var nomes = nomeCompleto.Split(' ');
                var primeiroNome = nomes[0];
                var ultimoNome = string.Empty;


                if (nomes.Length > 1)
                {
                    foreach (var nome in nomes.Reverse())
                    {
                        if (!string.IsNullOrEmpty(nome) && !nomesNaoConsiderados.Contains(nome.ToLower()))
                        {
                            ultimoNome = nome;
                            break;
                        }
                    }
                }

                nomeResumido = string.IsNullOrEmpty(ultimoNome) ? nomeCompleto : primeiroNome + ' ' + ultimoNome;
            }
            return nomeResumido;
        }

        public static List<Attachment> ImageUrlToAttachment(params string[] urlList)
        {
            var attachment = new List<Attachment>();
            foreach(var url in urlList)
            {
                var fileName = url.Split('/').LastOrDefault();
                var client = new WebClient();
                Stream stream = client.OpenRead(url);
                attachment.Add(new Attachment(stream, fileName));
            }
            return attachment;
        }

        public static string RemoveHtmlETrocaBrPorQuebraDeLinha(string txtTexto)
        {
            var texto = RemoveHtml(txtTexto);
            return texto.Replace("<br />", "\n");
        }

        public static string GerarPdf(string urlOrigem, string nomeArquivoTemp, string environmentRootPath)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            var tmpFileName = String.Concat(environmentRootPath, Constants.FILEOUTPATH, nomeArquivoTemp, "-", Guid.NewGuid(), String.Concat(".pdf"));
            proc.StartInfo.Arguments = String.Format(@" {0} {1} {2} ""A4""", String.Concat(environmentRootPath, Constants.RASTERIZEJSPATH), urlOrigem, tmpFileName);
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = String.Concat(environmentRootPath, Constants.BINPATH, Constants.PHANTOMJSPAHTH);
            proc.Start();
            proc.WaitForExit();
            proc.Close();
            proc.Dispose();

            var retorno = String.Concat(tmpFileName);
            return retorno;

        }

        public enum EMenuAccessObject
        {
            Indefinido = 0,
            DuvidasAcademicas = 88,
            [Description("Cirurgia")]
            RecursosRMaisCirurgia = 308,
            [Description("Pediatria")]
            RecursosRMaisPediatria = 309,
            [Description("Clinica")]
            RecursosRMaisClinica = 310,
            [Description("GO")]
            RecursosRMaisGO = 311,
            [Description("RUm")]
            RecursosRUm = 313
        }

        public enum TipoObjetos
        {
            BANNER = 9
        }

        public static Dictionary<int, int> GetMapaRankingIdEspecialidade()
        {
            var mapa = new Dictionary<int, int>();
            mapa.Add((int)Constants.TipoSimulado.R3_Pediatria, (int)Constants.TipoSimuladoIdEspecialidade.R3_Pediatria);
            mapa.Add((int)Constants.TipoSimulado.R3_Clinica, (int)Constants.TipoSimuladoIdEspecialidade.R3_Clinica);
            mapa.Add((int)Constants.TipoSimulado.R3_Cirurgia, (int)Constants.TipoSimuladoIdEspecialidade.R3_Cirurgia);
            mapa.Add((int)Constants.TipoSimulado.R4_GO, (int)Constants.TipoSimuladoIdEspecialidade.R4_GO);

            return mapa;
        }

        public static List<int> GetEnumValues<T>()
        {
            return new List<int>(Enum.GetValues(typeof(T)).Cast<int>());
        }

        public static bool IsValidEmail(string enderecoEmail)
        {
            string email = @"^([\w\-]+\.)*[\w\- ]+@([\w\- ]+\.)+([\w\-]{2,3})$";
            Match match = Regex.Match(enderecoEmail, email);
            if (match.Success)
                return true;
            else
                return false;
        }

        public static bool IsAntesDatalimiteCache(int ano, int idAplicacao = 0)
        {
            var ctx = new DesenvContext();
            var key = String.Format("{0}:{1}:{2}", RedisCacheConstants.Utilidades.KeyIsAntesDatalimiteCache, ano, idAplicacao);

            if (!RedisCacheManager.CannotCache(RedisCacheConstants.Utilidades.KeyIsAntesDatalimiteCache))
            {
                if (RedisCacheManager.HasAny(key))
                {
                    return RedisCacheManager.GetItemObject<bool>(key);
                }

                var isDataLimite = IsAntesDatalimite(ano, idAplicacao);

                RedisCacheManager.SetItemObject(key, isDataLimite, TimeSpan.FromDays(1));
                return isDataLimite;

            }
            else
            {
                return IsAntesDatalimite(ano, idAplicacao);
            }
        }

        public static int SendMailProfile(String profile_name, String mailTo, String mailSubject, String mailBody, String copyRecipients, String BlindCopyRecipients, bool sendByAPI = true, String mailFrom = "", String nomeMailFrom = "")
        {
            String recipients = mailTo;
            String subject = mailSubject;
            String body = mailBody;
            String body_format = "HTML";
            String copy_recipients = copyRecipients;
            String blind_copy_recipients = BlindCopyRecipients;

            //Caso o tamanho do email seja maior que 8 mil nao podemos usar a proc, devemos enviar o email diretamente

            if (mailBody.Length > 8000 || sendByAPI)
                SendMailDirect(recipients, mailBody, subject, profile_name, mailFrom, nomeMailFrom);
            else
            {
                var ctx = new MedmailContext();
                ctx.Database.ExecuteSqlRaw("exec sp_SendMail_Queue_Mail @profile_name, @recipients, @subject, @body, @body_format, @copy_recipients, @blind_copy_recipients",
                    profile_name, recipients, subject, body, body_format, copy_recipients, blind_copy_recipients);
            }

            return 0;
        }        

        public static string GetConteudoArquivoRemoto(string url)
        {
            WebResponse response = WebRequest.Create(url).GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string conteudoArquivo = reader.ReadToEnd();
            reader.Close();
            response.Close();

            return conteudoArquivo;
        }

        public static List<int> AplicacoesServico()
        {
            return new List<int>
            {
                (int)Aplicacoes.Recursos
            };
        }

        public static bool IsAntesDatalimite(int ano, int idAplicacao = 0)
        {
            var ctx = new DesenvContext();

            var dataLimite = ctx.Set<msp_GetDataLimite_ByApplication_Result>().FromSqlRaw("msp_GetDataLimite_ByApplication @intYear = {0}, @intAplicationID = {1}", ano, idAplicacao).ToList().FirstOrDefault().dteDataLimite ?? DateTime.MinValue;
            return dataLimite >= DateTime.Now;
        }

        public static bool IsBeforeDataLimite(int aplicacao, int anoAtual)
        {
            using (var ctx = new DesenvContext())
            {
                var dataLimite = ctx.Set<msp_GetDataLimite_ByApplication_Result>().FromSqlRaw("msp_GetDataLimite_ByApplication @intYear = {0}, @intAplicationID = {1}", anoAtual - 1, aplicacao).ToList().FirstOrDefault().dteDataLimite ?? DateTime.MinValue;
                var isAntesDataLimite = dataLimite >= DateTime.Now;
                return isAntesDataLimite;
            }
        }

        public static List<int> GetAnosValidosDaAplicacao(int idCliente, Aplicacoes aplicacao)
        {
            var ano = Utilidades.GetYear();
            var anos = new List<int>();
            var isAntesDataLimite = IsBeforeDataLimite((int)aplicacao, ano);

            if (aplicacao == Aplicacoes.LeitordeApostilas)
            {
                var produtosContradados = ProdutoEntity.GetProdutosContratadosPorAno(idCliente);
                foreach (var item in produtosContradados)
                {
                    anos.Add(item.Ano.Value);
                }
            }
            else
            {
                anos.Add(ano);
                if (isAntesDataLimite)
                {
                    anos.Add(ano - 1);
                }
            }

            return anos;
        }

        public enum TipoDevice
        {
            [Description("android mobile")]
            androidMobile = 1,
            [Description("android tablet")]
            androidTablet = 2,
            [Description("ios mobile")]
            iosMobile = 3,
            [Description("ios tablet")]
            iosTablet = 4,
            [Description("ios ipod")]
            iosIpod = 5,
            [Description("Windows")]
            windows = 8,
            [Description("Mac")]
            mac = 9
        }

        public enum MensagemInadimplente
        {
            Extensivo = 1,
            R3 = 2,
            IMed = 3,
            MedMaster = 4

        }

        public enum TipoMensagemInadimplente
        {
            SemAcesso = 0,
            PermiteAcesso = 1,
            PermiteAcessoComTermoDeInadimplencia = 2,
            BloqueadoPorInadimplencia = 3,
            PermiteAcessoInadimplencia = 8
        }        

        public static bool IsMobile(TipoDevice tipo)
        {
            return tipo == TipoDevice.androidMobile || tipo == TipoDevice.iosMobile || tipo == TipoDevice.iosIpod;
        }

        public static bool IsTablet(TipoDevice tipo)
        {
            return tipo == TipoDevice.androidTablet || tipo == TipoDevice.iosTablet;
        }

        public static bool IsDesktop(TipoDevice tipo)
        {
            return tipo == TipoDevice.windows || tipo == TipoDevice.mac;
        }

        public static string EncryptionSHA1(string encryption)
        {
            var sha1CryptoService = new SHA1CryptoServiceProvider();
            sha1CryptoService.ComputeHash(Encoding.UTF8.GetBytes(encryption));
            byte[] encryptionHash = sha1CryptoService.Hash;
            var encryptionToString = Convert.ToBase64String(encryptionHash);
            return encryptionToString;
        }

        public static Task<string> ObterUltimaVersaoLojaAsync(Aplicacoes aplicacao)
        {
            return Task.Factory.StartNew(() => ObterUltimaVersaoLoja(aplicacao));
        }

        public static string ObterUltimaVersaoLoja(Aplicacoes aplicacao)
        {
            var versao = string.Empty;
            using (var client = new HttpClient())
            {
                try
                {
                    var bundleId = ObterAppBundleId(aplicacao);
                    var url = string.Format("lookup?bundleId={0}", bundleId);
                    client.BaseAddress = new Uri(Constants.ITUNES_STORE_URL);
                    var content = client.GetStringAsync(url).Result;
                    var metadados = JsonConvert.DeserializeObject<MetadadosLojaDTO>(content);
                    versao = metadados.Releases.Max(c => Version.Parse(c.Version)).ToString();
                }
                catch { versao = string.Empty; }
                return versao;
            }
        }

        public static string ObterAppBundleId(Aplicacoes aplicacao)
        {
            var appBundleId = string.Empty;
            switch (aplicacao)
            {
                case Aplicacoes.Recursos:
                    appBundleId = Constants.RECURSOS_BUNDLEID;
                    break;
                case Aplicacoes.MsProMobile:
                    appBundleId = Constants.MSPROMOBILE_BUNDLEID;
                    break;
            }
            return appBundleId;
        }


        public static bool VerificaImagemExiste(string urlImagem)
        {
            var request = (HttpWebRequest)WebRequest.Create(urlImagem);
            request.Method = "HEAD";

            bool existe;
            try
            {
                request.GetResponse();
                existe = true;
            }
            catch
            {
                existe = false;
            }
            return existe;
        }

        public static List<K> MapListToOtherList<T, K>(T listOne)
        {
            var otherList = new List<K>();
            foreach (var list in (IEnumerable)listOne)
            {
                var other = (K)Activator.CreateInstance(typeof(K));
                foreach (var prop in list.GetType().GetProperties())
                {
                    PropertyInfo p = other.GetType().GetProperty(prop.Name);
                    if (p != null)
                        p.SetValue(other, prop.GetValue(list, null), null);
                }

                otherList.Add(other);
            }
            return otherList;
        }

        public static bool VersaoMaiorOuIgual(string referencia, string comparacao)
        {
            return new Version(referencia) >= new Version(comparacao);
        }      

        public static bool MenuPermitido(List<Menu> menus, int objectId)
        {
            foreach (var item in menus)
            {
                var menuPaiEncontrado = item.Id == objectId;
                if (menuPaiEncontrado)
                    return true;

                if (item.SubMenu.Any())
                {
                    var menuEncontrado = MenuPermitido(item.SubMenu, objectId);
                    if (menuEncontrado)
                        return true;
                }
            }
            return false;
        }

        public static string ParametroGenericoValue(string txtModule, string txtName)
        {
            var ctx = new DesenvContext();
            var param = ctx.tblParametrosGenericos.Where(a => a.txtModule == txtModule && a.txtName == txtName).FirstOrDefault().txtValue;
            return param;
        }

        public enum PermissaoStatus
        {
            SemAcesso = 1,
            Ler = 2,
            AcessoPermitido = 3
        }      

        public static bool AntesDataLiberacaoTestesMedMaster()
        {
            DateTime DATA_LIBERACAO_TESTES_MEDMASTER = new DateTime(2020, 2, 7);
            return DateTime.Now < DATA_LIBERACAO_TESTES_MEDMASTER;
        }     

        public static string AppIdNotificacoes(Aplicacoes aplicacao)
        {
            var dados = new Dictionary<Aplicacoes, string>
            {
                { Aplicacoes.MsProMobile, "oneSignalAppId" },
                { Aplicacoes.Recursos, "recursosOneSignalAppId" }
            };
            return dados[aplicacao];
        }   

        public static bool PossuiMaterialAntecipado(int matricula)
        {
            if (matricula == Constants.CONTACTID_ACADEMICO)
                return true;
            else
                return false;
        }

        public enum AccessObjectType
        {

            Menu = 1,

            Combo = 2,

            Botao = 3

        }

        public static List<int> ProdutosAulasEspeciais()
        {
            return new List<int> {
                Produto.Produtos.MEDCURSO_AULAS_ESPECIAIS.GetHashCode(),
                Produto.Produtos.MED_AULAS_ESPECIAIS.GetHashCode()
            };
        }
    }
}