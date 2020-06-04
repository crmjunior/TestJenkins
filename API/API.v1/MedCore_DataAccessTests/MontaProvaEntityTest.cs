using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;
using MedCore_DataAccessTests.EntitiesDataTests;
using MedCore_DataAccessTests.EntitiesMockData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NSubstitute;

namespace MedCore_DataAccessTests
{
    [TestClass]
    public class MontaProvaEntityTest
    {
        public const int matriculaUsuarioAcademicoTeste = 241718;

        [TestMethod]
        public void MontaProvaFiltroStopWordsTeste() {
            var man = new MontaProvaManager();
            var post = new MontaProvaFiltroPost();
            post.Concursos = new int[] { };
            post.Anos = new int[] { };
            post.Especialidades = new int[] { };
            post.FiltrosEspeciais = new int[] { };
            post.Matricula = 111222;
            post.FiltroTexto = "câncer de pulmão";

            var result = man.Filtrar(post);

            var post2 = new MontaProvaFiltroPost();
            post2.Concursos = new int[] { };
            post2.Anos = new int[] { };
            post2.Especialidades = new int[] { };
            post2.FiltrosEspeciais = new int[] { };
            post2.Matricula = 111222;
            post2.FiltroTexto = "câncer pulmão";

            var result2 = man.Filtrar(post2);

            Assert.AreEqual(result.TotalQuestoes, result2.TotalQuestoes);
        }

        [TestMethod]
        public void MontaProva3QuestoesCorretas7ErradasTeste()
        {
            var montaProvaMock = Substitute.For<IMontaProvaData>();

            var provasAluno = MontaProvaEntityTestData.GetProvasAluno();

            var questoes = MontaProvaEntityTestData.GetQuestoes();
            var questoesSimulado = questoes.Where(x => x.Value == 1).ToList();
            var questoesConcurso = questoes.Where(x => x.Value == 2).ToList();

            var respostasSimulado = MontaProvaEntityTestData.GetRespostasSimulado(questoesSimulado);
            var respostasConcurso = MontaProvaEntityTestData.GetRespostasConcurso(questoesConcurso);

            respostasSimulado[0].Alternativa = "A";
            respostasSimulado[0].AlternativaRespondida = "A";
            respostasSimulado[0].Correta = true;

            respostasConcurso[0].Alternativa = "A";
            respostasConcurso[0].AlternativaRespondida = "A";
            respostasConcurso[0].Correta = true;

            respostasConcurso[1].Alternativa = "A";
            respostasConcurso[1].AlternativaRespondida = "A";
            respostasConcurso[1].Correta = true;
            
            montaProvaMock.ObterProvasAluno(16401).Returns(provasAluno);
            montaProvaMock.GetQuestoesProva(provasAluno.First()).Returns(questoes);
            montaProvaMock.ObterRespostasSimulado(227181, questoesSimulado.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasSimulado);
            montaProvaMock.ObterRespostasConcurso(227181, questoesConcurso.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasConcurso);

            var provas = new MontaProvaBusiness(montaProvaMock).GetProvasFiltro(227181, 16401);
            Assert.AreEqual(3, provas.First().Acertos);
            Assert.AreEqual(7, provas.First().Erros);
            Assert.AreEqual(0, provas.First().NaoRealizadas);
            Assert.AreEqual(10, provas.First().QuantidadeQuestoes);
        }

        [TestMethod]
        public void MontaProva2QuestoesCorretas1Anulada7ErradasTeste()
        {
            var montaProvaMock = Substitute.For<IMontaProvaData>();
            var contextMock = Substitute.For<DesenvContext>();

            var provasAluno = MontaProvaEntityTestData.GetProvasAluno();

            var questoes = MontaProvaEntityTestData.GetQuestoes();
            var questoesSimulado = questoes.Where(x => x.Value == 1).ToList();
            var questoesConcurso = questoes.Where(x => x.Value == 2).ToList();

            var respostasSimulado = MontaProvaEntityTestData.GetRespostasSimulado(questoesSimulado);
            var respostasConcurso = MontaProvaEntityTestData.GetRespostasConcurso(questoesConcurso);

            respostasSimulado[0].Alternativa = "A";
            respostasSimulado[0].AlternativaRespondida = "A";
            respostasSimulado[0].Correta = true;

            respostasConcurso[0].Alternativa = "A";
            respostasConcurso[0].AlternativaRespondida = "A";
            respostasConcurso[0].Correta = true;

            respostasConcurso[1].Alternativa = "A";
            respostasConcurso[1].AlternativaRespondida = "A";
            respostasConcurso[1].Correta = true;
            respostasConcurso[1].Anulada = true;

            montaProvaMock.ObterProvasAluno(16401).Returns(provasAluno);
            montaProvaMock.GetQuestoesProva(provasAluno.First()).Returns(questoes);
            montaProvaMock.ObterRespostasSimulado(227181, questoesSimulado.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasSimulado);
            montaProvaMock.ObterRespostasConcurso(227181, questoesConcurso.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasConcurso);

            var provas = new MontaProvaBusiness(montaProvaMock).GetProvasFiltro(227181, 16401);
            Assert.AreEqual(2, provas.First().Acertos);
            Assert.AreEqual(8, provas.First().Erros);
            Assert.AreEqual(0, provas.First().NaoRealizadas);
            Assert.AreEqual(10, provas.First().QuantidadeQuestoes);
        }
        [TestMethod]
        public void MontaProva_ObterRespostasConcurso_Com50000Questoes()
        {
            var montaProvaMock = Substitute.For<IMontaProvaData>();
            var contextMock = Substitute.For<DesenvContext>();

           var provasAluno = MontaProvaEntityTestData.GetProvasAluno();

            var questoes = MontaProvaEntityTestData.GetQuestoes(50000,2000);
            var questoesSimulado = questoes.Where(x => x.Value == 1).ToList();
            var questoesConcurso = questoes.Where(x => x.Value == 2).ToList();

            var respostasSimulado = MontaProvaEntityTestData.GetRespostasSimulado(questoesSimulado);
            var respostasConcurso = MontaProvaEntityTestData.GetRespostasConcurso(questoesConcurso);

          

            montaProvaMock.ObterProvasAluno(16401).Returns(provasAluno);
            montaProvaMock.GetQuestoesProva(provasAluno.First()).Returns(questoes);
            montaProvaMock.ObterRespostasSimulado(227181, questoesSimulado.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasSimulado);
            montaProvaMock.ObterRespostasConcurso(227181, questoesConcurso.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasConcurso);

            var provas = new MontaProvaBusiness(montaProvaMock).GetProvasFiltro(227181, 16401);
            Assert.AreEqual(50000, provas.First().QuantidadeQuestoes);
        }
        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetHistoricoQuestoesErradas()
        {
            var manager = new MontaProvaManager();
            var historico = manager.GetHistoricoErradas(241740);

            Assert.IsNotNull(historico);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanGetProvasAluno()
        {
            var business = new MontaProvaBusiness(new MontaProvaEntity());
            var historico = business.GetProvasAluno(227163, 17);

            Assert.IsNotNull(historico);

            historico.ForEach(x =>
            {
                Assert.IsNotNull(x.ProvasAluno);
            });
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void CanCreateProvaNovo2()
        {
            // Esse testa o cenário que produzia provas com menos de 100 questões.

            string jsonStringPost = @"{""Anos"":[],""Concursos"":[],""Especialidades"":[],""FiltroModulo"":0,""FiltroTexto"":"""",""FiltrosEspeciais"":[],""Matricula"":241682,""Nome"":""Woop"",""TodasEspecialidades"":false,""TodosConcursos"":false,""ExercicioPermissaoAluno"":[{""Ids"":[10,11,12,13,14,15,16,17,18,517,518,520,521,522,523,524,525,527,528,529,532,534,535,536,537,538,539,540,541,546,547,548,549,550,551,552,553,554,555,556,557,558,559,560,561,562,563,564,565,566,567,568,569,570,571,572,573,574,575,576,577,578,579,580,581,582,583,584,585,586,587,588,589,590,591,592,593,594,595,596,597,598,599,600,601,602,603,604,605,606,607,608,609,610,611,612,614,615,616,617,618,619,620,621,622,624,628,629,630,631,632,633,634,635,636],""Tipo"":1},{""Ids"":[43,44,46,47,49,52,53,54,58,59,60,61,62,64,66,67,68,70,71,72,73,74,76,78,81,82,83,84,87,88,89,96,97,98,101,102,104,108,112,113,115,118,122,123,125,127,135,136,137,139,143,149,153,157,158,165,170,171,172,180,186,194,196,197,208,213,216,218,219,220,224,225,227,228,230,231,232,290,291,292,293,294,295,296,298,299,300,301,302,303,304,306,308,309,310,311,313,314,315,316,317,321,322,324,325,326,327,328,329,330,331,332,335,336,337,338,339,340,341,342,343,344,345,346,347,348,349,350,351,352,353,354,360,364,365,368,390,391,392,393,402,403,404,405,407,408,410,411,413,416,417,419,420,421,422,423,424,425,426,427,429,430,432,435,436,441,442,443,447,449,450,451,452,453,455,456,457,459,467,470,471,472,473,479,481,486,490,491,492,495,497,499,501,503,505,506,513,515,516,519,525,526,527,530,534,536,539,542,543,550,551,557,567,571,572,576,581,588,591,600,602,609,630,647,652,655,660,661,647552,793842,793845,793846,793852,793855,793861,793864,793865,793866,793867,793869,793882,793890,793897,793911,793917,793921,793924,793926,793927,793928,793931,793942,793949,793956,793958,793959,793970,793975,793982,793985,793987,794005,794011,794016,794031,794033,794041,794044,794046,794048,794050,794051,794054,794055,794063,794064,794066,794070,794081,794094,794099,794104,794110,794134,794136,794140,794143,794146,794147,794148,794149,794157,794161,794163,794167,794168,794170,794172,794270,794428,794443,868850,868857,868943,872978,872981,872982,872989,872992,872998,873002,873004,873005,873007,873008,873021,873029,873031,877869,878557,878571,878572,878573,878577,878581,878584,878585,878586,878587,878588,878591,878601,878602,878619,878630,878632,878635,878642,878646,878656,878665,878666,878672,878677,878689,878693,878694,878695,878703,878706,878707,878708,878710,878713,878714,878717,878718,878719,878725,878727,878728,878730,878734,878745,878750,878759,878769,878775,878779,878784,878793,878797,878802,878804,878811,878814,878815,878817,878827,878840,878841,878842,878845,878848,878859,878868,878891,878927,878934,879027,879057,879078,879095,879125,879137,879152,879161,879166,883714,883837,884028,891970,891971,891973,891976,891982,891992,892066,892191,892203,892248,892517,892653,892922,893057,893147,893152,893197,893287,893422,893826,894187,894277,894400,894502,895132,895177,895402,895583,895717,895763,895809,895853,895899,896034,896484,897249,897293,897519,897789,898013,898329,898374,898407,898465,898588,898915,899365,899769,900086,900311,900491,900581,900940,901076,901120,901166,901255,901346,901391,901571,901616,901661,901784,901886,901930,901975,901976,902067,902249,902372,902743,902744,902969,903419,903869,904094,904095,904411,904545,904636,904816,904905,904951,905175,905176,905253,905266,905544,905593,905729,905774,905775,905820,905865,906135,906225,906314,906415,906451,906586,906721,906766,906810,906865,907071,907128,907701,907790,907850,907853,907854,907855,907870,907873,912552,914100,914102,914103,914105,914107,914125,914130,914158,914168,914169,914170,919981,919986,919991,919997,920000,920006,920010,920016,920022,920024,920025,920037,920045,920047,920051,920055,920069,920070,920075,920079,920082,920083,920085,920090,920091,920099,920110,920120,920122,920126,920127,920132,920138,920143,920152,920154,920155,920158,920159,920162,920180,920182,920186,920191,920203,920206,920207,920209,920218,920220,920221,920229,920232,920234,920236,920237,920241,920243,920244,920248,920249,920252,920259,920275,920280,920283,920287,920292,920294,920296,920303,920310,920324,920333,920336,920340,920344,920361,920369,920375,920381,920393,920413,920425,920426,920430,920433,920441,920445,920447,920450,920453,920454,920459,920469,920486,920489,920492,920493,920499,920501,920503,920505,920506,920509,920512,920513,920514,920516,920521,920538,920551,920554,920566,920569,920573,920592,920599,920604,920605,920608,920610,920615,920617,920618,920620,920622,920626,920629,920637,920639,920646,920647,920649,920651,920654,920661,920744,920762,920763,920766,921429,921430,921431,921433,921439,921444,921447,921453,921457,921459,921465,921468,921473,921481,921482,921483,921486,921487,921493,921499,921502,921504,921505,921508,921511,921520,921524,921530,921534,921539,921545,921553,921554,921555,921570,921571,921576,921585,921593,921596,921598,921602,921605,921609,921611,921614,921622,921626,921630,921671,921682,921686,921701,921703,921714,921721,921744,921749,921750,921756,921759,921762,921778,921780,921792,921800,921803,921804,921809,921821,921823,921824,921834,921843,921845,921854,921861,921867,921875,921877,921890,921892,921897,921901,921904,921906,921910,921915,921916,921917,921919,921922,921936,921939,921940,921944,921949,921955,921956,921958,921960,921965,921968,921976,921991,922002,922019,922027,922030,922032,922035,922040,922052,922053,922055,922060,922065,922066,922068,922069,922077,922078,922093,922094,922098,922100,922102,922103,922106,922111,922112,922121,922122,922127,922130,922139,922141,922146,922256,922268,922269,922272,922273,922275,922280,922287,922289,922290,922291,922293,922294,922296,922297,922298,922299,922300,922302,922303,922304,922305,922308,922310,922311,922317,922318,922323,922325,922326,922327,922331,922336,922339,922345,922346,922350,922354,922355,922359,922368,922369,922371,922373,922375,922378,922380,922382,922383,922385,922386,922391,922408,922415,922423,922425,922436,922440,922446,922451,922473,922478,922480,922484,922497,922499,922501,922510,922518,922520,922523,922525,922527,922536,922545,922548,922550,922554,922564,922566,922568,922574,922576,922579,922582,922584,922588,922590,922593,922600,922603,922604,922608,922611,922613,922615,922616,922618,922620,922621,922625,922628,922635,922645,922647,922659,922660,922662,922665,922671,922673,922675,922677,922681,922684,922686,922688,922690,922691,922692,922696,922697,922706,922720,922722,922727,922732,922734,922737,922738,922745,922746,922747,922748,922751,922763,922764,922772,922780,922784,922785,922787,922789,922790,922791,922792,922795,922796,922797,922798,922799,922800,922801,922802,922803,922804,922805,922807,922811,922814,922815,922822,922823,922825,922826,922827,922837,922839,922871,922872,922873,922889,922890,922891,922892,922893,922895,922896,922897,922899,922900,922901,922902,922904,922905,922906,922908,922909,922910,922911,922912,922913,922914,922915,922916,922917,922918,922919,922965,922971,922975,922976,922981,922983,922984,922985,922990,922992,922994,922995,922996,922998,923001,923004,923005,923008,923012,923013,923015,923016,923023,923029,923030,923035,923037,923039,923048,923060,923074,923076,923077,923079,923080,923082,923085,923090,923092,923094,923096,923098,923099,923111,923114,923123,923134,923136,923138,923142,923145,923146,923153,923162,923166,923179,923186,923188,923193,923195,923198,923201,923206,923208,923211,923218,923227,923235,923238,923242,923250,923254,923264,923268,923270,923272,923273,923275,923277,923278,923279,923280,923282,923288,923289,923292,923297,923300,923303,923306,923312,923317,923322,923324,923325,923333,923337,923340,923343,923347,923354,923359,923362,923379,923384,923386,923387,923389,923396,923400,923403,923407,923414,923415,923423,923425,923426,923427,923431,923433,923436,923440,923441,923446,923447,923448,923451,923454,923459,923460,923461,923468,923469,923470,923477,923481,923482,923484,923486,923490,923491,923494,923500,923503,923504,923517,923520,923524,923525,923526,923528,923533,923535,923536,923538,923539,923540,923541,923542,923545,923546,923565,923569,923576,923585,923593,923596,923605,923607,923608,923609,923610,923611,923612,923613,923614,923615,923616,923617,923618,923620,923622,923623,923624,923625,923626,923627,923628,923629,923630,923631,923633,923634,923635,923637,923638,923639,923640,923641,923642,923643,923644,923645,923647,923649,923650,923651,923652,923653,923654,923655,923656,923657,923658,923660,923661,923662,923663,923664,923665,923666,923667,923668,923669,923670,923673,923674,923675,923677,923678,923686,923687,923696,923699,923702,923706,923707,923710,923714,923715,923716,923717,923721,923725,923726,923728,923732,923735,923736,923739,923742,923745,923747,923748,923749,923756,923762,923764,923769,923772,923774,923782,923783,923795,923798,923809,923811,923814,923815,923816,923819,923824,923825,923827,923829,923831,923833,923834,923835,923847,923850,923859,923871,923884,923887,923888,923895,923904,923908,923915,923923,923930,923932,923939,923941,923944,923947,923952,923954,923957,923961,923965,923984,923987,923999,924003,924014,924021,924022,924024,924027,924029,924030,924031,924032,924034,924040,924042,924045,924054,924057,924061,924068,924075,924078,924080,924096,924097,924100,924107,924114,924116,924119,924122,924128,924137,924139,924145,924147,924149,924151,924152,924159,924168,924172,924177,924181,924185,924189,924191,924192,924197,924199,924202,924208,924214,924216,924219,924222,924227,924228,924229,924236,924238,924239,924250,924251,924253,924254,924255,924257,924262,924263,924266,924275,924276,924283,924288,924292,924297,924298,924303,924307,924312,924314,924315,924326,924338,924342,924343,924344,924345,924352,924353,924355,924356,924357,924359,924360,924361,924362,924365,924366,924382,924383,924384,924385,924387,924388,924389,924390,924392,924394,924398,924400,924402,924403,924404,924405,924406,924407,924408,924409,924411,924412,924413,924414,924417,924420,924421,924422,924424,924425,924426,924427,924429,924431,924434,924435,924439,924440,924441,924443,924445,924450,924451,924452,924453,924454,924456,924458,924459,924460,924461,924463,924464,924465],""Tipo"":2}],""HistoricoQuestaoErradaAluno"":[{""Ids"":[6781,6895,6941,6968],""Tipo"":2}]}";

            MontaProvaFiltroPost filtro = JsonConvert.DeserializeObject<MontaProvaFiltroPost>(jsonStringPost);

            
            var qtdMax = Convert.ToInt32(ConfigurationProvider.Get("Settings:quantidadeQuestaoProva"));

            //retorna o ID da prova
            var retorno = new MontaProvaManager().MontarProva(filtro, qtdMax);

            //Verificar ser a prova tem somente qtdMax questões
            var page = 0;
            var limit = 0;
            var filtros = new MontaProvaBusiness(new MontaProvaEntity()).GetProvasAlunoNovo(filtro.Matricula, (int)Aplicacoes.MsProMobile, page, limit);
            var provas = new List<ProvaAluno>();

            //Recuperando todas as provas de todos os Filtros
            foreach (var filtroItem in filtros)
                provas.AddRange(filtroItem.ProvasAluno.FindAll(x => true));

            var prova = provas.Find(x => x.ID == retorno);

            //Buscar Prova -> Not Null
            Assert.IsNotNull(prova);

            //Qtd questões == qtdMax
            Assert.AreEqual(qtdMax, prova.QuantidadeQuestoes);
        }

        [TestMethod]
        [TestCategory("Basico")]
        public void CanCreateProvaNovo()
        {

            string jsonStringPost = @"{""FiltroModulo"":4,""Matricula"":241718,""FiltroTexto"":"""",""Especialidades"":[57,65,93,145],""Concursos"":[24,293,112,168,605,180,194,316,574],""Anos"":[],""FiltrosEspeciais"":[],""Nome"":""TesteAPIEndpointNovo"",""HistoricoQuestaoErradaAluno"":[{""Ids"":[24125,24128,24131,24134,24135,24138,24140,24141,24159,24161],""Tipo"":1},{""Ids"":[21479,21654,21702,22539,23497,23544,24787,25295,26063,26920,27344,27698,27705,27905,27916,32525,34334,35229,44359,44381,45172,46086,49195,52118,52439,60314,64135,78495,126762,136272,136277,137279,148982,148995,150504,150527,150535,152503,160093,161025,161269,165072,165077,165100,167107,167172,167178,167181],""Tipo"":2}],""ExercicioPermissaoAluno"":[{""Ids"":[10,11,12,13,14,15,16,17,18,517,518,520,521,522,523,524,525,527,528,529,532,534,535,536,537,538,539,540,541,546,547,548,549,550,551,552,553,554,555,556,557,558,559,560,561,562,563,564,565,566,567,568,569,570,571,572,573,574,575,576,577,578,579,580,581,582,583,584,585,586,587,588,589,590,591,592,593,594,595,596,597,598,599,600,601,602,603,604,605,606,607,608,609,610,611,612,614,615,616,617,618,619,620,621,622,624,628,629,630],""Tipo"":1},{""Ids"":[43,44,46,47,49,52,53,54,58,59,60,61,62,64,66,67,68,70,71,72,73,74,76,78,81,82,83,84,87,88,89,96,97,98,101,102,104,108,112,113,115,118,122,123,125,127,135,136,137,139,143,149,153,157,158,165,170,171,172,180,186,194,196,197,208,213,216,218,219,220,224,225,227,228,230,231,232,290,291,292,293,294,295,296,298,299,300,301,393,402,403,404,405,407,408,410,411,413,416,417,419,420,421,422,423,424,425,426,427,429,430,432,435,436,441,442,443,447,449,450,451,452,453,455,456,457,459,467,470,471,472,473,479,481,486,490,491,492,495,497,499,501,503,505,506,513,515,516,519,525,526,527,530,534,536,539,542,543,550,551,557,567,571,572,576,581,588,591,600,602,609,630,647,652,655,660,661,647552,793842,793845,793846,793852,793855,793861,793864,793865,793866,793867,793869,793882,793890,793897,793911,793917,793921,793924,793926,793927,793928,793931,793942,793949,793956,793958,793959,793970,793975,793982,793985,793987,794005,794011,794016,794031,794033,794041,794044,794046,794048,794050,794051,794054,794055,794063,794064,794066,794070,794081,794094,794099,794104,794110,794134,794136,794140,794143,794146,794147,794148,794149,794157,794161,794163,794167,794168,794170,794172,794270,794428,794443,868850,868857,868943,872978,872981,872982,872989,872992,872998,873002,873004,873005,873007,873008,873021,873029,873031,877869,878557,878571,878572,878573,878577,878581,878584,878585,878586,878587,878588,878591,878601,878602,878619,878630,878632,878635,878642,878646,878656,878665,878666,878672,878677,878689,878693,878694,878695,878703,878706,878707,878708,878710,878713,878714,878717,878718,878719,878725,878727,878728,878730,878734,878745,878750,878759,878769,878775,878779,878784,878793,878797,878802,878804,878811,878814,878815,878817,878827,878840,878841,878842,878845,878848,878859,878868,878891,878927,878934,879027,879057,879078,879095,879125,879137,879152,879161,879166,883714,883837,884028,891970,891971,891973,891976,891982,891992,892066,892191,892203,892248,892517,892653,892922,893057,893147,893152,893197,893287,893422,893826,894187,894277,894400,894502,895132,895177,895402,895583,895717,895763,895809,895853,895899,896034,896484,897249,897293,897519,897789,898013,898329,898374,898407,898465,898588,898915,899365,899769,900086,900311,900491,900581,900940,901076,901120,901166,901255,901346,901391,901571,901616,901661,901784,901886,901930,901975,901976,902067,902249,902372,902743,902744,902969,903419,903869,904094,904095,904411,904545,904636,904816,904905,904951,905175,905176,905253,905266,905544,905593,905729,905774,905775,905820,905865,906135,906225,906314,906415,906451,906586,906721,906766,906810,906865,907071,907128,907701,907790,907850,907853,907854,907855,907870,907873,912552,914100,914102,914103,914105,914107,914125,914130,914158,914168,914169,914170,919981,919986,919991,919997,920000,920006,920010,920016,920022,920024,920025,920037,920045,920047,920051,920055,920069,920070,920075,920079,920082,920083,920085,920090,920091,920099,920110,920120,920122,920126,920127,920132,920138,920143,920152,920154,920155,920158,920159,920162,920180,920182,920186,920191,920203,920206,920207,920209,920218,920220,920221,920229,920232,920234,920236,920237,920241,920243,920244,920248,920249,920252,920259,920275,920280,920283,920287,920292,920294,920296,920303,920310,920324,920333,920336,920340,920344,920361,920369,920375,920381,920393,920413,920425,920426,920430,920433,920441,920445,920447,920450,920453,920454,920459,920469,920486,920489,920492,920493,920499,920501,920503,920505,920506,920509,920512,920513,920514,920516,920521,920538,920551,920554,920566,920569,920573,920592,920599,920604,920605,920608,920610,920615,920617,920618,920620,920622,920626,920629,920637,920639,920646,920647,920649,920651,920654,920661,920762,920763,920766,921429,921430,921431,921433,921439,921444,921447,921453,921457,921459,921465,921468,921473,921481,921482,921483,921486,921487,921493,921499,921502,921504,921505,921508,921511,921520,921524,921530,921534,921539,921545,921553,921554,921555,921570,921571,921576,921585,921593,921596,921598,921602,921605,921609,921611,921614,921622,921626,921630,921671,921682,921686,921701,921703,921714,921721,921744,921749,921750,921756,921759,921762,921778,921780,921792,921800,921803,921804,921809,921821,921823,921824,921834,921843,921845,921854,921861,921867,921875,921877,921890,921892,921897,921901,921904,921906,921910,921915,921916,921917,921919,921922,921936,921939,921940,921944,921949,921955,921956,921958,921960,921965,921968,921976,921991,922002,922019,922027,922030,922032,922035,922040,922052,922053,922055,922060,922065,922066,922068,922069,922077,922078,922093,922094,922098,922100,922102,922103,922106,922111,922112,922121,922122,922127,922130,922139,922141,922256,922268,922269,922272,922273,922275,922280,922287,922289,922290,922291,922293,922294,922296,922297,922298,922299,922300,922302,922303,922304,922305,922308,922310,922311,922317,922318,922323,922325,922326,922327,922331,922336,922339,922345,922346,922350,922354,922355,922359,922368,922369,922371,922373,922375,922378,922380,922382,922383,922385,922386,922391,922408,922415,922423,922425,922436,922440,922446,922451,922473,922478,922484,922497,922499,922501,922510,922518,922520,922523,922525,922527,922536,922545,922548,922550,922554,922564,922566,922568,922574,922576,922579,922582,922584,922588,922590,922593,922600,922603,922604,922608,922611,922613,922615,922616,922618,922620,922621,922625,922628,922635,922645,922647,922659,922660,922662,922665,922671,922673,922675,922677,922681,922684,922686,922688,922690,922691,922692,922696,922697,922706,922720,922722,922727,922732,922734,922737,922738,922745,922746,922747,922748,922751,922763,922764,922772,922780,922784,922785,922787,922790,922791,922792,922795,922796,922797,922798,922799,922801,922802,922803,922805,922807,922811,922814,922815,922822,922823,922825,922826,922827,922837,922839,922872,922889,922890,922891,922892,922893,922896,922902,922905,922906,922909,922910,922913,922915,922917,922918,922965,922971,922975,922976,922981,922983,922984,922985,922990,922992,922994,922995,922996,922998,923001,923004,923005,923008,923012,923013,923015,923016,923023,923029,923030,923035,923037,923039,923048,923060,923074,923076,923077,923079,923080,923082,923085,923090,923092,923094,923096,923098,923099,923111,923114,923123,923134,923136,923138,923142,923145,923146,923153,923162,923166,923179,923186,923188,923193,923195,923198,923201,923206,923208,923211,923218,923227,923235,923238,923242,923250,923254,923264,923268,923270,923272,923273,923275,923277,923278,923279,923280,923282,923288,923289,923292,923297,923300,923303,923306,923312,923317,923322,923324,923325,923333,923337,923340,923343,923347,923354,923359,923362,923379,923384,923386,923387,923389,923396,923400,923403,923407,923415,923423,923425,923426,923427,923431,923433,923436,923440,923441,923446,923447,923448,923451,923454,923459,923460,923461,923468,923469,923470,923477,923481,923482,923484,923486,923490,923491,923494,923500,923503,923504,923517,923520,923524,923525,923526,923528,923533,923535,923536,923538,923539,923540,923541,923542,923546,923565,923569,923576,923585,923593,923596,923605,923607,923608,923609,923610,923612,923613,923614,923615,923616,923617,923620,923622,923623,923624,923625,923626,923628,923635,923637,923638,923639,923647,923649,923654,923655,923656,923657,923658,923663,923665,923666,923667,923668,923669,923670,923673,923675,923696,923699,923702,923706,923707,923710,923714,923715,923716,923717,923721,923725,923726,923728,923732,923735,923736,923739,923742,923745,923747,923748,923749,923756,923762,923764,923769,923772,923774,923782,923783,923795,923798,923809,923811,923814,923815,923816,923819,923824,923825,923827,923829,923831,923833,923834,923835,923847,923850,923859,923871,923884,923887,923888,923895,923904,923908,923915,923923,923930,923932,923939,923941,923944,923947,923952,923954,923957,923961,923965,923984,923987,923999,924003,924014,924021,924022,924024,924027,924029,924030,924031,924032,924034,924040,924042,924045,924054,924057,924068,924075,924078,924080,924096,924097,924100,924107,924114,924116,924119,924122,924128,924137,924139,924145,924147,924149,924151,924152,924159,924168,924172,924177,924181,924185,924189,924191,924192,924197,924199,924202,924208,924214,924216,924219,924222,924227,924228,924229,924236,924238,924239,924250,924251,924253,924254,924255,924257,924262,924263,924266,924275,924276,924283,924288,924292,924297,924298,924303,924307,924312,924314,924315,924326,924338,924342,924343,924344,924345,924362,924365,924366],""Tipo"":2}]}";

            MontaProvaFiltroPost filtro = JsonConvert.DeserializeObject<MontaProvaFiltroPost>(jsonStringPost);

            var qtdMax = Convert.ToInt32(ConfigurationProvider.Get("Settings:quantidadeQuestaoProva"));

            //retorna o ID da prova
            var retorno = new MontaProvaManager().MontarProva(filtro, qtdMax);

            //Verificar ser a prova tem somente qtdMax questões
            var idFiltro = GetIntIdFiltroAlunoMontaProva(filtro.Matricula, retorno);
            var provas = new MontaProvaBusiness(new MontaProvaEntity()).GetProvasFiltro(filtro.Matricula, idFiltro);

            var prova = provas.Find(x => x.ID == retorno);
            var questoes = new MontaProvaEntity().GetQuestoesProva(prova);

            //Buscar Prova -> Not Null
            Assert.IsNotNull(prova);

            //Qtd questões == qtdMax
            Assert.AreEqual(questoes.Count, prova.QuantidadeQuestoes);
        }


        [TestMethod]
        [TestCategory("Basico")]
        public void MontaProva_GetProvasAlunoNovo_RetornaProvas()
        {
            var matricula = 241779;
            var idAplicacao = (int)Aplicacoes.MsProMobile;
            var page = 1;
            var limit = 4;

            var montaProvaMock = Substitute.For<IMontaProvaData>();
            var provasAluno = MontaProvaEntityTestData.GetProvasAluno();
            var filtrosAluno = MontaProvaEntityTestData.GetFiltrosAluno();
            var questoes = MontaProvaEntityTestData.GetQuestoes(50000, 2000);
            var questoesSimulado = questoes.Where(x => x.Value == 1).ToList();
            var questoesConcurso = questoes.Where(x => x.Value == 2).ToList();

            var respostasSimulado = MontaProvaEntityTestData.GetRespostasSimulado(questoesSimulado);
            var respostasConcurso = MontaProvaEntityTestData.GetRespostasConcurso(questoesConcurso);
			var contadorQuestoes = MontaProvaEntityTestData.GetProvasAluno();
            montaProvaMock.GetFiltrosAluno(matricula, page, limit).Returns(filtrosAluno);
            montaProvaMock.ObterProvasAluno(1).Returns(provasAluno);
            montaProvaMock.GetQuestoesProva(provasAluno.First()).Returns(questoes);
            montaProvaMock.ObterRespostasSimulado(241779, questoesSimulado.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasSimulado);
			montaProvaMock.ObterContadorDeQuestoes(241779).Returns(contadorQuestoes);
            montaProvaMock.ObterRespostasConcurso(241779, questoesConcurso.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasConcurso);
            foreach (var filtro in filtrosAluno)
            {
                montaProvaMock.GetQuantidadeQuestoesNaoAssociadas(filtro.Id).Returns(100);
                montaProvaMock.GetQuantidadeQuestoesFiltro(filtro.Id).Returns(20000);
            }            
            var business = new MontaProvaBusiness(montaProvaMock);
            var retorno = business.GetProvasAlunoNovo(matricula, idAplicacao, page, limit);           

            Assert.AreEqual(retorno.Count(), 4);
            Assert.IsNotNull(retorno);
        }

        [TestMethod]
        [TestCategory("Monta Prova")]
        public void MontaProva_AlterarQuestoesProvaNovo_AlterarQuantidade()
        {
            QuestoesMontaProvaPost questoesPost = new QuestoesMontaProvaPost();
            questoesPost.Quantidade = 100;

            var montaProvaMock = Substitute.For<IMontaProvaData>();
            var provasAluno = MontaProvaEntityTestData.GetProvasAluno();

            var questoes = MontaProvaEntityTestData.GetQuestoes(50000, 2000);
            var questoesSimulado = questoes.Where(x => x.Value == 1).ToList();
            var questoesConcurso = questoes.Where(x => x.Value == 2).ToList();

            var respostasSimulado = MontaProvaEntityTestData.GetRespostasSimulado(questoesSimulado);
            var respostasConcurso = MontaProvaEntityTestData.GetRespostasConcurso(questoesConcurso);

            montaProvaMock.ObterProvasAluno(16401).Returns(provasAluno);
            montaProvaMock.GetQuestoesProva(provasAluno.First()).Returns(questoes);
            montaProvaMock.ObterRespostasSimulado(227181, questoesSimulado.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasSimulado);
            montaProvaMock.ObterRespostasConcurso(227181, questoesConcurso.Select(y => y.Key).ToArray()).ReturnsForAnyArgs(respostasConcurso);
            montaProvaMock.AlterarQuestoesProvaNovo(227181, 1, Convert.ToInt32(questoesPost.Quantidade)).Returns(1);
            var business = new MontaProvaBusiness(montaProvaMock);
            var prova = business.GetProvasFiltro(227181, 16401);
            var retorno = business.AlterarQuestoesProvaNovo(227181, 1, Convert.ToInt32(questoesPost.Quantidade));

            //Returns 1
            Assert.AreEqual(1, retorno);

             //Quantidade de Questões da Prova Novo
            var qtdQuestoesProvaNovo = prova.First().QuantidadeQuestoes + questoesPost.Quantidade;

            //Prova com + ou  - x questões
            Assert.AreEqual(prova.First().QuantidadeQuestoes + questoesPost.Quantidade, qtdQuestoesProvaNovo);
        }

        [TestMethod]
        [TestCategory("Monta Prova")]
        public void MontaProva_DeleteNovo_DeletarProva()
        {

            var montaProvaMock = Substitute.For<IMontaProvaData>();
            var provasAluno = MontaProvaEntityTestData.GetProvasAluno();
            montaProvaMock.ObterProvasAluno(16401).Returns(provasAluno);
            montaProvaMock.DeleteNovo(provasAluno.First()).Returns(1);
            var retornoDeleteProva = new MontaProvaBusiness(montaProvaMock).DeleteNovo(provasAluno.First());
            Assert.AreEqual(1, retornoDeleteProva);
        }


        [TestMethod]
        [TestCategory("Integrado")]
        public void MontaProva_ProvaDeClincaMedicaSemSubespecialidade_DeveExistirQuestoes()
        {
            //TODO: Remover data tolerancia
            var dataTolerancia = Utilidades.DataToleranciaTestes();
            if (DateTime.Now <= dataTolerancia)
            {
                Assert.Inconclusive();
            }
            var manager = new MontaProvaManager();

            var filtro = MontaProvaEntityTestData.GetFiltroPostMontaProvaComQuestoesComEspecialidadeCLM(matriculaUsuarioAcademicoTeste);

            var idProva = manager.MontarProva(filtro, 100);
            var idFiltro = GetIntIdFiltroAlunoMontaProva(matriculaUsuarioAcademicoTeste, idProva);

            var qtdQuestoes = new MontaProvaEntity().GetQuantidadeQuestoesFiltro(idProva);

            Assert.IsTrue(qtdQuestoes > 0);
        }

        [TestMethod]
        [TestCategory("Integrado")]
        public void MontaProva_QuantidadeDeQuestoesCorretasEErrdas_DeveVirCerta()
        {
            int matriculaDeTeste = 96409;
            var dataTest = new MontaProvaEntity().GetContadorQuestoes_MontaProva_TestData(matriculaDeTeste);

            if (dataTest == null)
                Assert.Inconclusive("Não há cenário para teste");

            var provaId = dataTest.intProvaId;
            
            var manager = new MontaProvaManager();
            var page = 0;
            var limit = 0;
            var provas = new MontaProvaBusiness(new MontaProvaEntity()).GetProvasAlunoNovo(matriculaDeTeste, (int)Aplicacoes.MsProMobile, page, limit);

            var prova = provas.FirstOrDefault(x => x.ProvasAluno.Any(y => y.ID == provaId));

            Assert.IsTrue(prova.ProvasAluno[0].Acertos == dataTest.intAcertos);
            Assert.IsTrue(prova.ProvasAluno[0].Erros == dataTest.intErros);
        }

        [TestMethod]
        public void FiltrarMontaProvaManager_AlunoR1_NaoRetornarQuestoesR3()
        {
            var man = new MontaProvaManager();
            var ConcursoABC = man.GetConcursoEntidadeIdBySigla("ABC");
            
            var post = new MontaProvaFiltroPost();
            post.Concursos = new int[] { };
            post.Anos = new int[] { };
            post.Especialidades = new int[] { };
            post.FiltrosEspeciais = new int[] { };
            post.Concursos = new int[] { ConcursoABC };
            post.Matricula = new PerfilAlunoEntityTestData().GetAlunoSomenteR1().ID;

            var result = man.Filtrar(post);

            var resultQntdQuestoesConcursoABC = result.Filtros[1].Concursos.Where(x => x.Id == ConcursoABC).FirstOrDefault().QuantidadeQuestoes;
            var qntdQuestoesConcursoABC = man.GetQuestoesMongoConcurso(ConcursoABC).Count();

            Assert.IsTrue(resultQntdQuestoesConcursoABC <= qntdQuestoesConcursoABC);
        }

        [TestMethod]
        public void FiltrarMontaProvaManager_AlunoR3_RetornarQuestoesR3()
        {
            var man = new MontaProvaManager();
            var ConcursoABC = man.GetConcursoEntidadeIdBySigla("ABC");

            var post = new MontaProvaFiltroPost();
            post.Concursos = new int[] { };
            post.Anos = new int[] { };
            post.Especialidades = new int[] { };
            post.FiltrosEspeciais = new int[] { };
            post.Concursos = new int[] { ConcursoABC };
            post.Matricula = new PerfilAlunoEntityTestData().GetAlunoR3(Utilidades.GetYear() -1).ID;

            var result = man.Filtrar(post);

            var resultQntdQuestoesConcursoABC = result.Filtros[1].Concursos.Where(x => x.Id == ConcursoABC).FirstOrDefault().QuantidadeQuestoes;
            var qntdQuestoesConcursoABC = man.GetQuestoesMongoConcurso(ConcursoABC).Count();

            Assert.IsTrue(resultQntdQuestoesConcursoABC == qntdQuestoesConcursoABC);
        }

        private List<int> GetIntProvaIdPorMatricula(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                return (from f in ctx.tblFiltroAluno_MontaProva
                        join p in ctx.tblExercicio_MontaProva on f.intID equals p.intFiltroId
                        where f.intClientId == matricula
                            && f.bitAtivo == true
                            && p.bitAtivo == true
                        select p.intID)
                        .Distinct()
                        .ToList(); 
            }
        }

        private int GetIntIdFiltroAlunoMontaProva(int matricula, int intProvaId)
        {
            using (var ctx = new DesenvContext())
            {
                return (from f in ctx.tblFiltroAluno_MontaProva
                        join p in ctx.tblExercicio_MontaProva on f.intID equals p.intFiltroId
                        where p.intID == intProvaId 
                            && f.intClientId == matricula
                        select f.intID
                        ).FirstOrDefault();
            }
        }

        public int GetMatriculaFiltroAlunoMontaProva()
        {
            using (var ctx = new DesenvContext())
            {
                var filtro = ctx.tblFiltroAluno_MontaProva
                    .GroupBy(x => x.intClientId)
                    .Where(grp => grp.Count() < 10)
                    .Select(g => new { intClientId = g.Key, qtd = g.Count(), dteDataCriacao = g.Max(x => x.dteDataCriacao) })
                    .OrderByDescending(x => x.dteDataCriacao)
                    .FirstOrDefault();

                return filtro.intClientId;
            }

        }
    }
}