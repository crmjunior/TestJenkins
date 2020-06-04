using System;
using System.ComponentModel;

namespace MedCore_DataAccess.Util
{
    public static class Constants
    {
        public const string DOMAINS3 = "https://s3-sa-east-1.amazonaws.com/BUCKET/";
        public const string DOMAINS3US = "https://s3.amazonaws.com/BUCKET/";
        public const string DOMAINCLOUDFRONT = "BUCKET.cloudfront.net";
        public const string ECG_REGUA_VIDEO_NAME = "10000-293698E5-99B9-404E-9618-D335D53BD63C";
        public const string URLCOMENTARIOIMAGEM = "https://api.medgrupo.com.br/Media.svc/json/Comentario/Imagem/IDCOMENTARIOIMAGEM/?formato=FORMATO";
        public const string STATIC_MEDGRUPO = "http://static.medgrupo.com.br";
        public const string URLCOMENTARIOIMAGEMHOMOL = "http://desenv.ordomederi.com/API_Homologacao/Media.svc/json/Comentario/Imagem/IDCOMENTARIOIMAGEM/?formato=FORMATO";


        public const string URLEMAILCONFIRMACAOEADEXTENSIVO = "http://static.medgrupo.com.br/_files/Inscricoes/Extensivo/Email_Confirmacao_EAD.txt";
        public const string URLEMAILCONFIRMACAOEXTENSIVO = "http://static.medgrupo.com.br/_files/Inscricoes/Extensivo/Email_Confirmacao.txt";
        public const string URLEMAILCONFIRMACAOMEDELETRO = "http://static.medgrupo.com.br/_files/Inscricoes/Medeletro/Email_Confirmacao.txt";
        public const string URLEMAILCONFIRMACAOREVALIDA = "http://static.medgrupo.com.br/_files/Inscricoes/Revalida/Email_Confirmacao.txt";

        public const string URLIMAGEMQUESTAO = "https://api.medgrupo.com.br/CtrlPanel.svc/json/Questao/Imagem/IDQUESTAOIMAGEM";
        public const string URLIMAGEMQUESTAOHOMOL = "http://desenv.ordomederi.com/API_Homologacao/CtrlPanel.svc/json/Questao/Imagem/IDQUESTAOIMAGEM";
        //public const string URLIMAGEMQUESTAO = "http://desenv.ordomederi.com/API_PortalProfessor2015/CtrlPanel.svc/json/Questao/Imagem/IDQUESTAOIMAGEM";
        public const string URLCOMENTARIOIMAGEMCONCURSO = "https://api.medgrupo.com.br/MateriaisImpressos.svc/json/Comentario/Imagem/IDCOMENTARIOIMAGEM";
        //public const string URLCOMENTARIOIMAGEMCONCURSO = "http://desenv.ordomederi.com/API_PortalProfessor2015/MateriaisImpressos.svc/json/Comentario/Imagem/IDCOMENTARIOIMAGEM";

        public const string URLIMAGEMQUESTAOSIMULADO = "https://api.medgrupo.com.br/CtrlPanel.svc/json/Simulado/Questao/Imagem/IDQUESTAOIMAGEM";
        //public const string URLIMAGEMQUESTAOSIMULADO = "http://desenv.ordomederi.com/API_PortalProfessor2015_QuestaoSimulado/CtrlPanel.svc/json/Simulado/Questao/Imagem/IDQUESTAOIMAGEM";
        public const string URLCOMENTARIOIMAGEMSIMULADO = "https://api.medgrupo.com.br/CtrlPanel.svc/json/Simulado/Questao/Comentario/Imagem/IDCOMENTARIOIMAGEM";
        //public const string URLCOMENTARIOIMAGEMSIMULADO = "http://desenv.ordomederi.com/API_QuestaoSimulado/CtrlPanel.svc/json/Simulado/Questao/Comentario/Imagem/IDCOMENTARIOIMAGEM";

        public const string URLMEDIAIMAGEM = "https://api.medgrupo.com.br/Media.svc/json/ImagemApostila/IDMEDIAIMAGEM/?formato=FORMATO";

        public const string URLDIRETORIOAVATARPROFESSOR = "http://arearestrita.medgrupo.com.br/_static/images/professores/";

        public const string URLATUALIZACAOERRATA = "https://api.medgrupo.com.br/Media.svc/json/Apostila/AtualizacaoErrata/IDATUALIZACAOERRATA/?formato=FORMATO";
        public const string URLTHUMBVIDEO = "https://api.medgrupo.com.br/Media.svc/json/video/thumb/IDVIDEO/?formato=FORMATO";
        public const string URLTHUMBVIDEOVIMEO = "https://api.vimeo.com/videos/VIMEOID/pictures";
        public const string URLAPIVIMEO = "https://api.vimeo.com/videos/";
        public const string FTP_RECURSOS_FOTOS = @"ftp://static.medgrupo.com.br/_static/Imagens/Recursos/ClientPictures/";
        public const string ACCESS_FTP_RECURSOS_USUARIO = "aplweb";
        public const string ACCESS_FTP_RECURSOS_SENHA = "jpk891x5x5";
        public const string LINK_STATIC_FOTOS = @"http://static.medgrupo.com.br/static/Imagens/Recursos/ClientPictures/";
        public const string LINK_STATIC_AVATARES = @"http://static.medgrupo.com.br/static/Imagens/Recursos/images/Avatars/";
        public const string LINK_STATIC_AVATAR_PADRAO = @"http://static.medgrupo.com.br/static/imagens/recursos/images/foto_perfil.gif";
        public const string LINK_STATIC_ANEXO_ANALISERECURSOS = "http://static.medgrupo.com.br/static/imagens/RecursosComentario/";
        public static string FFMPEGPATH = @"apps\ffmpeg\bin\ffmpeg.exe";
        public static string BINPATH = @"apps\phantomjs\bin\";
        public static string PHANTOMJSPAHTH = @"phantomjs.exe";
        public static string URLPROVAIMPRESSA = "https://arearestrita.medgrupo.com.br/provaimpressa.aspx?ExercicioId=IDEXERCICIO&tipo=TIPOEXERCICIO&nome=NOME";
        public static string URLSIMULADOIMPRESSO = "https://arearestrita.medgrupo.com.br/provaimpressa.aspx?ExercicioId=IDEXERCICIO&tipo=TIPOEXERCICIO";
        public static string URLSIMULADOIMPRESSO_COM_ANO = "https://arearestrita.medgrupo.com.br/ProvaimpressaSimulado.aspx?Ano=ANO&ExercicioId=IDEXERCICIO";


        //public static string URLPROVAIMPRESSA = "http://localhost:63965/website/provaimpressa.aspx?ExercicioId=IDEXERCICIO&tipo=TIPOEXERCICIO&nome=NOME";
        //public static string URLPROVAIMPRESSA = "http://desenv.ordomederi.com/restrita/provaimpressa.aspx?ExercicioId=IDEXERCICIO&tipo=2&tipoExercicio=TIPOEXERCICIO&nome=NOME";
        //public static string URLPROVAIMPRESSA = "http://desenv.ordomederi.com/restrita_concurso/provaimpressa.aspx?ExercicioId=IDEXERCICIO&tipo=2&tipoExercicio=TIPOEXERCICIO&nome=NOME";
        public static string RASTERIZEJSPATH = @"apps\phantomjs\js\printheaderfooter.js";
        public static string FILEOUTPATH = @"temp\";
        public static string DASH = " - ";
        public const string ACCESSKEY = "AKIAJGL4PVQWVGIKLTZQ";
        public const string SECRETKEY = "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";
        public const string CLOUDFRONT_PRIVATEKEY = @"-----BEGIN RSA PRIVATE KEY-----
                                                    MIIEowIBAAKCAQEAh02w078QZmbDs0OIypv2V66fXdbVRayf+rfj57u4ybseBQCH
                                                    p8isXcsnDMu3RXGND+jDH6QlXIkKF80CRv4TyUHHH7h8yAUQzJlgbdQvaeKnNFEh
                                                    3d6elwS3M9ohP06isEKr+4IFuPEWiWSsl1ek1eq4G+EbO2DENFqfV7kHeIef9dXH
                                                    GlUHfu73tScZ7YX8jcqEMp9cooT6S9x4B/NEoAW6obp6fyzCdOypbRjBAvrGb/oo
                                                    5RUucd5dE/SkA6KWXawfHsGOpm9Th53Kk0xlyk3wb7rEHXmXtNmjlSb22E7uvqFo
                                                    OA/6t7iGy9jXIkDkyQWGf+8j6Qw+yfOHzCalWQIDAQABAoIBAFBXZo9lXZSgosYY
                                                    bOp6D53jF8h936f+qAQVG2QnyMMos9ueg+6yLv+GuQpVEN9QXMW8ndh0/KZkzTRy
                                                    zE5hYlhYEagZQSb4MxB6ZpLwsrK6HjTnlCeNoDfmWe6VMLbJt7rX7A1tGH7H0W0F
                                                    9aHsLfnD5UzQrv3kKL05rnJFcVU0unEo2gnj0Ak6KgFHve/t5joEfAgN+WktFF2I
                                                    nT0wPt0zDKvA7Jf6oCpBJFWBfAKgmdZKHe8IrVD0Nf3XsYxKP/4yoAqxqQCvR2xc
                                                    H/FCPI8Pn+G4C+6vFV/lZdePm4lZW0zT5JDx1PDTeFd38Xid4LKwC5iA0aWV3UL4
                                                    3ZNN6q0CgYEA4HkRHnTuVY2GPvarmKVcXxeDXl42svlJ1Ib1eux9BMVoDlw546h/
                                                    v3U0EuqiihiuDDGaJ+vAdCtoZ7lVgWxkVOlD2J3IkwldxWQ27L7Bq8PV9rLKNXky
                                                    YfplSAUGted8NzXjN5Nl6hwo3BN85Q1fUNdAMf6WkDZl0bFNt5PBUHMCgYEAmk6I
                                                    cev4FEA+aFiVhXRsYp5Q8wcHp2xj8YaVugqXc1IdJOvfq/S85CCIQ/Iohry/H5/t
                                                    UFXTjsLjdPgDS77hLprDG+L13d7CT4U+9bAOg4Vb5i2W4CgtAECY1Iwfk1D0XjKd
                                                    4XWoo1rs4dxtR7xaUQgIsQOmJh/r1Ge57uapfAMCgYEAwAXxJgvDYn5Zy/z51/AF
                                                    b/dOUujng/0LMnps/sXVQMKafEZa4yxec+sQK+p6NwZbxWSdSe87yGTlwG1j/v45
                                                    agXOGz0Guvm1NMnAzo90X7p8i7hwkIPoHLa184ERN4UZ1qULhZxa/4UtUu581fi1
                                                    hBNPADWYmS9ftJ0nLZaBNY8CgYAzAWiKt6Q+FWpLlZR/E5fMFWbOnnFCP0reCE2v
                                                    vFaJkQf/L1S1A2X4xmQkiYQp3XQToQqRtn0CwJrtDBoSzJqkjBWYPzJkT4DNRk8k
                                                    aTiy8r8I2+L2X3FYlwGtqE+7o1PTW0niv8CplqGcmv8oajM0e8JbuMgP7BdJE6xb
                                                    MrWtPwKBgCRfWO7qxoMVLKFsKvnU+tbKlty6BGpRIkhE/hUK39kqu4mpFGXURVy6
                                                    Ehl6/zKtuvGDT0ytTecinkcWsQ8Qzuk9iJy9yD/a2ukt46aSHAMEA3dEPlQq45dr
                                                    gLWuDs0+IQN8W40ymm/RkHNYLpISeD4EVM0WAFBkRvS16OULstzY
                                                    -----END RSA PRIVATE KEY-----
                                                    ";
        public const string ACCESSKEYKONGROS = "AKIAI3L3QGBUBCH7GN6Q";
        public const string SECRETKEYKONGROS = "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";
        public const string BORACCESSKEY = "rauJoSc8";
        public const string BORSECRETKEY = "ZYr7FAaNj0J8oGrUcjfGabvq";
        public const string FORMATOVIDEO = "mp4";
        public const string FORMATOIMAGEM = "jpg";
        public const string DEFAULTBUCKET = "iosstream";
        public const string DUVIDAQUESTAOBUCKET = "medimagensduvidaquestao";
        public const string CONCURSOSFLVBUCKETBAIXA = "mg-concursos/variable/baixa";
        public const string ACESSO_DIRETO = "ACESSO DIRETO 1";
        public const string CHAMADO_JAENVIOUCHEQUE_ASSUNTO = "Cobrança de Cheques - Afirma enviou";
        public const string CHAMADO_JAENVIOUCHEQUE_DETALHE = "Informou na restrita que enviou";
        public const string CONTRATO_ACCESSKEY = "AKIAI7AZLBIA7GTHJUYQ";
        public const string CONTRATO_SECRETKEY = "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";
        public const string CONTRATO_BUCKET = "medmidiascontrato";


        public const string RECURSOS_BUNDLEID = "com.medgrupo.recursos";
        public const string MSPROMOBILE_BUNDLEID = "com.medgrupo.medsoft.pro";

        public const int anoInscricaoExtensivo = 2020;
        public const string nomeInscricao = "Extensivo";

        public const int anoInscricaoIntensivo = 2018;
        public const int anoInscricaoRevalida = 2016;
        public const int anoInscricaoMedeletro = 2018;
        public const int anoInscricaoAdaptaMed = 2017;


        public const string EmpresaRM = "RM";
        public const string EmpresaRMed = "RMed";
        public const string CNPJ_RM = "08.374.251/0001-48";
        public const string CNPJ_RMed = "17.654.804/0001-07";

        public const int idQuestionarioSobreMaterialDidaticoMed = 15;

        public const decimal valorDescontIRPorDependente = 189.59m;

        //  public const int anoLetivo = 2017;
        public const int anoRecursos = 2018;
        //public const int anoRPA = 2017;
        //public const string URLSLIDES = "http://localhost:17608/Media.svc/json/Aula/IDTEMAAULA/Slide/IDORDEM/";
        public const string URLSLIDES = "https://api.medgrupo.com.br/Media.svc/json/Aula/Slide/IDSLIDE/?formato=FORMATO";
        public const string URLSLIDES_MSCROSS = "https://api.medgrupo.com.br/MsCross.svc/json/Aula/Slide/IDSLIDE/?formato=FORMATO";
        //public const string URLSLIDES_MSCROSS = "http://desenv.ordomederi.com/API_MSCROSS/MsCross.svc/json/Aula/Slide/IDSLIDE/?formato=FORMATO";

        //public const string UrlBucketAmazonSlides = "https://s3-sa-east-1.amazonaws.com/iosstream/slidesaula/";
        public const string UrlBucketAmazonSlides = "http://medmidiasslidesaula.s3.amazonaws.com/";

        public const int MaxErrorRetryAmazon = 5;

        public const int IdEmployeeChamado = 131220;
        public const int CONTACTID_ACADEMICO = 96409;
        public const int ROLE_ACADEMICO = 14;

        public const int ORDEM_SIMULADO_10 = 10;

        public static readonly int[] IDS_ESPECIALIDADES_PRINCIPAIS = { 11, 12, 13, 14, 15 };
        public static readonly int[] IDS_CLASSIFICACAO_ESPECIALIDADES = { 2, 3, 9 };
        public static readonly int[] IDS_CARGO_PROFESSOR = { 112, 113 };
        public static readonly int[] INADIMPLENCIA_CHAMADOS_PRIMEIROAVISO = { 9215, 9377, 9568 };
        public const int INADIMPLENCIA_CHAMADO_VISUALIZACAORESTRITA = 9215;
        public const int INADIMPLENCIA_CHAMADO_FECHADO = 9235;
        public const int INADIMPLENCIA_CHAMADO_AVISOBLOQUEIO = 9226;
        public const int INADIMPLENCIA_CHAMADO_AVISOFISICO = 9568;
        public const int INADIMPLENCIA_CHAMADO_AVISOFINALIZADO = 9223;
        public const int INADIMPLENCIA_CHAMADO_GRUPO = 187;
        public const int INADIMPLENCIA_CHAMADO_CATEGORIA = 2277;
        public const string LINK_STATIC_PPT = "http://static.medgrupo.com.br/Static/pptaviso/NOMEPPT";
        public const int ID_ROLE_ACADEMICO = 14;

        public const string SlidesAulaAmazon_bucket = "medmidiasslidesaula";
        public const string SlidesAulaAmazon_key = "AKIAIB4QU2LPNHM3MD4A";
        public const string SlidesAulaAmazon_secret = "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";


        public const string URLFilesCrowdsource = @"http://static.medgrupo.com.br/csstatic/";
        public const string awsAccessKey = "AKIAI7OKRIGTYI4UQ7XQ";
        public const string awsSecretAccessKey = "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";
        public const string CrowdsourceBucket = "medmidiascrowdsource";
        public const int AULAREVISAO_PRIMEIROANO = 2015;


        public const string BibliotecaAcademica_key = "AKIAI3TL4JN7GL5J3ZLA";
        public const string BibliotecaAcademica_secret = "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";
        public const string BibliotecaAcademica_bucket = "medmidiasimagemdasemana";

        public const string GaleriaImagem_key = "AKIAJ5PIJTLFLN5FFZ2Q";
        public const string GaleriaImagem_secret = "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";
        public const string GaleriaImagem_bucket = "medmidiasgaleriaimagens";

        public const string Avatar_key = "AKIAI35KI66Y4Y57LC6A";
        public const string Avatar_secret = "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";
        public const string Avatar_bucket = "medmidiasavatar";

        public const int AulaCartaMeioDeAnoMed = 3736;
        public const int AulaCartaMeioDeAnoMedcurso = 3636;
        public const int MenuRevisaoDeEstudos = 79;


        public const int MenuRoteiroDeTreinamento = 14;
        public const int AulaRoteirodeTreinamentoMedCurso = 3596;
        public const int AulaRoteirodeTreinamentoMed = 3701;





        public const string RECURSOS_PRAZO_INDEFINIDO = "Não definido";
        public const string EMAIL_RECURSOS = "forum.recursos@medgrupo.com.br";
        public const string EMAIL_DUVIDASACADEMICAS = ",luiz.filho@medgrupo.com.br,academico@medgrupo.com.br,cjunior.eng@gmail.com";

        public const int ANO_LANCAMENTO_READER = 2018;

        public const string AssuntoEmailConvenio = "CP-MED Convênio MEDGRUPO/VIAGENS";
        public const string urlEmailConvenio = "http://static.medgrupo.com.br/_files/Inscricoes/CPMED/email/Email_Convenio_LATAM.txt";
        public const string Link_Static_Foto_Inscricao = @"http://static.medgrupo.com.br/api/img/?intContactID=";

        public const int TemplateConditionTypePlanejamento2AnosEspecial = 19;
        public const int TemplateConditionTypePlanejamento2AnosEspecialEad = 22;
        public const int TemplateConditionTypePlanejamento2AnosAdaptaMed = 33;

        public const int APOSTILATREINAMENTOTEORICOPRATICO = 938;
        public const string URLMEDCDN = "http://d1y36np0qkbzyh.cloudfront.net/";
        public const string PATHATUALIZACAOERRATA = "pdf/atualizacaoerrata/IDAPOSTILA.pdf";

        public const string VideosMedeletro_key = "AKIAI4T5M4SGDZEOPZRA";
        public const string VideosMedeletro_secret = "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";
        public const string VideosMedeletro_bucket = "medmidiasvideosmedeletro";

        public const int TipoAppAtivo = 4;
        public const int TipoFeature = 5;

        public const int IdObjetoDesktopCadastroNickname = 108;

        public const string ChamadoPreBlacklistAssunto = "Inscrição sem sucesso - avaliar com a direção";
        public const string ChamadoPreBlacklistDetalhe = "Inscrição sem sucesso - avaliar com a direção";

        public const int ChamadoPreBlacklistGrupo = 214;
        public const int ChamadoPreBlacklistCategoria = 2474;
        public const int ChamadoPreBlacklistStatusChamadoExtensivo = 10563;
        public const int ChamadoPreBlacklistStatusChamadoIntensivo = 10564;
        public const int ChamadoPreBlacklistStatusChamadoCPMED = 10565;
        public const int ChamadoPreBlacklistStatusChamadoMedEletro = 10566;
        public const int ChamadoPreBlacklistStatusChamadoRacRacipe = 10567;
        public const int ChamadoPreBlacklistStatusChamadoAdaptaMed = 10578;
        public const int ChamadoPreBlacklistStatusChamadoR3 = 11207;

        public const int CHAMADO_NEGA_CORTESIA_STATUSINTERNO = 11017;
        public const int CHAMADO_NEGA_CORTESIA_GROUP = 105;
        public const int CHAMADO_NEGA_CORTESIA_CATEGORY = 1922;


        public const int PreBlacklistInscricaoExtensivo = 109;
        public const int PreBlacklistInscricaoMedeletro = 110;
        public const int PreBlacklistInscricaoIntensivo = 111;
        public const int PreBlacklistInscricaoCPMED = 112;
        public const int PreBlacklistInscricaoRevalida = 113;
        public const int PreBlacklistInscricaoAdaptaMed = 120;
        public const int PreBlacklistInscricaoR3 = 230;

        public const int ChamadoSetorRelacionamento = 1;

        public const string LinkInstrucaoChequeIntensivo = "https://d1y36np0qkbzyh.cloudfront.net/postagem-cheque/intensivo/GuiaPostagemCheque.pdf";

        public const string REQUISICAOANEXO_KEY = "AKIAJ6QDK4DJMUV6EK4Q";
        public const string REQUISICAOANEXO_SECRET = "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";
        public const string REQUISICAOANEXO_BUCKET = "medportalreqanexos/prod";
        //public const string REQUISICAOANEXO_BUCKET = "medportalreqanexos/homol";

        public const int LessonTypeTurmaX = 14;

        public const int ID_ESPECIALIDADE_REVALIDA = 161;
        public const string TEXT_AREA_REVALIDA = "Revalidação de diploma";

        public const int MatriculaDesenv = 131477;
        public const int MatriculaInternet_MGE = 131220;

        public const int WHAREHOUSE_BARRA = 74;
        public const int WHAREHOUSE_TIJUCA = 173;

        public const string URLBOLETOMEDYCORP = "http://boleto.medycorp.com.br/Default.aspx?id1=";
        public const string URLBOLETOMEDGRUPO = "http://boleto.medgrupo.com.br/Default.aspx?id1=";


        public const string EMAIL_TITULO_ECOMMERCE = "RECEBEMOS SUA SOLICITAÇÃO DE COMPRA";
        public const string EMAIL_AUTOMATICO_ECOMMERCE_MEDERI = "automatico@mederieditora.com.br";
        public const string EMAIL_AUTOMATICO_ECOMMERCE_MEDWRITERS = "automatico@medwriters.com.br";
        public const string EMAIL_AUTOMATICO_ECOMMERCE_MEDYKLIN = "automatico@medyklineditora.com.br";
        public const string EMAIL_AUTOMATICO_ECOMMERCE_MEDYN = "automatico@medyneditora.com.br";
        public const string EMAIL_TITULO_MEDELETRO_IMED = "<ANO> MEDELETRO iMED - Solicitação de Contratação";
        public const string EMAIL_TITULO_INTENSIVO = "Inscrição Medgrupo <ANO>";

        public const string AVISO_ALUNO_INVALIDO = "<p>CPF ou Passaporte inválido, caso seja aluno do MEDGRUPO entre em contato com a nossa Central Nacional de Atendimento</p> <br> <p>- 4004-0435 ou 4062-0660 ramal 8008</p> <p>- 0xx21 2131-0031  - medgrupo@medgrupo.com.br​</p>";

        public const int FILIAL_VITRINE = 251;
        public const int FILIAL_EAD_ASSINATURAS = 97;

        public const int IDEMPLOYEEUPLOADVIDEOSOULMEDICINA = 119710;
        public const int IDSTATUSUPLOADVIDEOSOULMEDICINA = 1;
        public const int IDEMPLOYEEUPLOADVIDEOREVISAO = 119710;
        public const int IDSTATUSUPLOADVIDEOREVISAO = 1;
        public const int IDEMPLOYEEUPLOADVIDEOSIMULADO = 119710;
        public const int IDSTATUSUPLOADVIDEOSIMULADO = 1;

        public const int MATRICULA_ACADEMICORECURSOS = 119710;
        public const string AVATAR_ACADEMICORECURSOS = "http://static.medgrupo.com.br/static/Imagens/Recursos/ClientPictures/gXxqd4YEukTl6DAELD_.jpg";

        public const int TLS12 = 3072;

        public const string LINK_ESQUECI_EMAIL = @"https://arearestrita.medgrupo.com.br/";

        public const string DEFAULT_EMAIL_PROFILE = "ses";
        public const string COMP_DUVIDA_APOSTILA = "<comp class=\"CLASSTEMPORARIA_ duvidaComp\" id=\"{0}\">";
        public const int QUANTIDADE_MINIMA_DUVIDASREPLICA = 1;
        public const int PAGINA_INICIAL_CONSULTA = 1;
        public const int DEZ_PRIMEIRAS_REPLICAS = 10;
        public const int ANO_CRONOGRAMA_DEFAULT = 0;
        public const int MINIMODIASOCIOSA = 21;
        public const string OrigemOutras = "O";


        public const int MinimoDiasContribuicoesOciosa = 21;
        public const string COMP_CONTRIBUICAO_APOSTILA = "<comp class=\"CLASSTEMPORARIA_ contribuicaoComp\" id=\"{0}\">";

        //public const string VersaoMinimaVimeoMiolo = "3.0.28";

        public const int StatusEmpregadoAtivo = 1;

        public const int TesteMedreaderPlanejamento2Anos = 260407;

        #region Admin de Conteudo
        // Produção
        public const string ANEXO_KEY = "AKIAJJIR2UZY6T3W6OOA";
        public const string ANEXO_SECRET = "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";
        public const string ANEXO_BUCKET = "med-cdn​​";

        public const string VERSAO_APP_TROCA_LAYOUT_AULOESR3R4 = "5.5.1";
        public const int intSubjetctId_AulaoRMAIS = 201;

        // Homol
        //public const string ANEXO_KEY = "AKIAIUL7U7TWDHPUR53A";
        //public const string ANEXO_SECRET = "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";
        //public const string ANEXO_BUCKET = "meds3desenvhomol​​";

        #endregion

        public enum Cursos
        {
            MEDCURSO = 16,
            MED = 17,
            MEDELETRO = 56,
            ADAPTAMED = 73,
            MEDELETRO_IMED = 88
        }

        public enum Produtos
        {
            MEDCURSO = 1,
            MED = 5,
            ADAPTAMED = 72
        }
        public const string URLCOMENTARIOIMAGEMSIMULADOMSCROSS = "https://api.medgrupo.com.br/MsCross.svc/json/Simulado/Questao/Comentario/Imagem/IDCOMENTARIOIMAGEM";
        //public const string URLCOMENTARIOIMAGEMSIMULADOMSCROSS = "http://desenv.ordomederi.com/API_MsCross/MsCross.svc/json/Simulado/Questao/Comentario/Imagem/IDCOMENTARIOIMAGEM";

        public const int StoreIdAssinaturaEAD = 97;

        public enum GrandesAreas
        {
            ClinicaMedica = 11,
            Cirurgia = 12,
            Pediatria = 13,
            GinecologiaObstetricia = 14,
            Preventiva = 15,
        }

        public const string PosFixoRedis = "-Redis";
        public const string R1 = "R1";
        public const string R3 = "R3";
        public const string R4 = "R4";
        public const string R4Tego = "TEGO";
        public const string MEDREADER_SUFIXO = "MEDREADER";

        public const int ConnectionLimitS3Apostila = 2000;

        public const int ExpiracaoToken = 30;
        public const string IdAplicacao = "1";
        public const string CpfClienteTeste = "95411747872";
        public const string CpfClientTesteSemAcessoGolden = "21724944967";
        public const string NomeClienteTeste = "ACADEMICO";
        public const string TokenVencido = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBcGlNZWRHcnVwbyIsImV4cCI6MTU0NDE5NTI4NC41MDQ5MzEsInN1YiI6OTY0MDksInJlZyI6Ijk1NDExNzQ3ODcyIn0.cPbAvwJ4XdRNY5c2DsOSp-Kcf5wkNys03hKQ5nbpwZk";
        public const string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBcGlNZWRHcnVwbyIsIm1hdCI6OTY0MDksImV4cCI6MTU1MDE2NTczMS44NTk1MTc4fQ.0wF6940ieLbzAsVsuqPNGDrg9HqAJ0N73528sQAN5fk";
        public const string senhaAcademico = "medyklin12345";
        public const string senhaErrada = "abacate9999";
        public const string cadastroInvalido = "abacate9999";

        public const int doisDiasEmMinutos = 2880;

        public const int QTDMAX_NOTIFICACOES = 2000;

        public const int menuCadastroQuestaoConcurso = 428;
        public const int menuAssociarPerfis = 438;
        public const int menuDashboard = 439;
        public const int menuUploadProvas = 440;
        public const int menuImportarQuestoes = 441;
        public const int menuDashboardRMais = 442;

        public const int perfilMGEWeb_Desenvolvedor = 1;
        public const int perfilMGEWeb_Diretoria = 2;
        public const int perfilMGEWeb_SupervisorRecursos = 3;
        public const int perfilMGEWeb_Controller = 4;
        public const int perfilMGEWeb_AnalistaRecursos = 5;
        public const int perfilMGEWeb_AssistenteRecursos = 6;
        public const int perfilMGEWeb_Cadastrador = 7;


        #region Checkout Editoras

        public const int ProdutoEcommerce = 18742;
        public const int FilialIdEcommerce = 248;
        public const int TemplateIdEcommerce = 2724;
        public const int ordemvendaId = 631712;
        public const string CEP = "20260140";
        public const string registro = "98740458172";

        public const string Codigos = "40010"; //"40010, 41106";
        public const string Origem = "21061-030";
        public const decimal Comprimento = 30;
        public const decimal Altura = 2;
        public const decimal Largura = 25;
        public const decimal Diametro = 0;
        public const string Ar = "S";
        #endregion


        public const int FilialiMed_RA = 260;

        public const string ITUNES_STORE_URL = "http://itunes.apple.com/";

        public static DateTime SqlDefaultDateTimeValue = new DateTime(1900, 01, 01);

        public const int LIMITE_HORAS_NOTIFICACAO = 24;

        public enum TipoSimulado
        {
            Extensivo = 1,
            CPMED = 2,
            Intensivo = 3,
            R3_Pediatria = 4,
            R3_Clinica = 5,
            R3_Cirurgia = 6,
            R4_GO = 7,
            CPMedExtensivo = 8
        }

        public enum LessonTypes
        {
            SimuladoOnline = 23
        }

        public enum ExercicioTipo
        {
            Simulado = 1
            , QuestaoConcurso = 2
        }

        public enum TipoSimuladoIdEspecialidade
        {
            R3_Pediatria = 239,
            R3_Clinica = 236,
            R3_Cirurgia = 18,
            R4_GO = 429
        }

        public enum Stores
        {
            R3_MEDERI = 240,
            R3_MEDWRITERS = 241
        }

        public class Messages
        {
            public enum Acesso
            {
                [Description("Usuário não cadastrado")]
                AlunoInexistente,

                [Description("Senha incorreta, digite a senha novamente")]
                SenhaIncorreta
            }

            public enum StatusQuestaoNotificacao
            {
                [Description("Não Cabe Recurso")]
                NaoCabeRecurso = 3,

                [Description("Cabe Recurso")]
                CabeRecurso = 4,

                [Description("Em Análise")]
                EmAnalise = 5,

                [Description("Alterado pela Banca")]
                AlteradaPelaBanca = 11
            }
        }

        public enum Uf_OrdemPremium
        {
            SP = 1,
            RJ = 2,
            Outros = 3
        }

        public class Notificacoes
        {
            public enum Recursos
            {
                StatusProvaFavoritos = 210,
                StatusQuestaoFavoritos = 211,
                RankingAcertosLiberado = 227,
                ComunicadoLiberado = 238,
                AvisaProfessorComentarioPre = 246,
                ConclusaoAnaliseAcademica = 247,
                ConclusaoAnaliseBancaQuestoes = 248
            }
        }

    }
}