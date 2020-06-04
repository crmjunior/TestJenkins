using System;

namespace MedCore_DataAccess.Util
{
    public static class RedisCacheConstants
    {
        public static class Questao
        {
            public static TimeSpan Timeout = TimeSpan.FromDays(5);

            public const string KeyGetImagensQuestaoConcurso = "ImagensQuestaoConcurso";
            public const string KeyGetQuestoesComVideos = "QuestoesComVideos";
            public const string KeyGetQuestoesSomenteImpressasComOuSemVideo = "QuestoesSomenteImpressasComOuSemVideo";
            public const string KeyGetQuestoesComComentarioApostila = "QuestoesComComentarioApostila";
            public const string KeyGetYear = "GetYear";
            public const string keyCountQuestoes = "CountQuestoes";
            public const string keyCountQuestoesDiscursivas = "CountQuestoesDiscursivas";
            public const string KeyNomeApostila = "NomeApostila";
            public const string KeyGetQuestaoMontaProva = "QuestoesMontaProva";
        }

        public static class Simulado
        {
            public const string KeyRankingSimulado = "RankingSimulado";
            public const string QuestaoSimulado = "QuestaoSimulado";
        }

        public static class Exercicio
        {
            public const string KeyGetDadosAlunoTurma = "GetDadosAlunoTurma";
            public const int ValidadeGetDadosAlunoTurma = 45;
        }

        public static class DadosFakes
        {
            public const string KeyGetAlunoComMensagensMsCross = "DadosFake-GetAlunoComMensagensMsCross";
            public const string KeyGetPermitidosOffline = "DadosFake-GetPermitidosOffline";
            public const string KeyGetPermitidos = "DadosFake-GetPermitidos";
            public const string KeyGetNotificacoesPorPerfil = "DadosFake-GetNotificacoesPorPerfil";
            public const string KeyGetAvaliar = "DadosFake-GetAvaliar";
            public const string KeyGetCacheConfig = "DadosFake-GetCacheConfig";
            public const string KeyGetOfflineConfig = "DadosFake-GetOfflineConfig";
            public const string KeyGetCronogramaAluno = "DadosFake-GetCronogramaAluno";
            public const string KeyGetMensagensAplicacao = "DadosFake-GetMensagensAplicacao";
            public const string KeyVersaoValida = "DadosFake-VersaoValida";
            public const string KeyGetComboAposLancamentoMsPro = "DadosFake-GetComboAposLancamentoMsPro";
            public const string KeyGetPermissao = "DadosFake-GetPermissao";
            public const string KeyGetProgressos = "DadosFake-GetProgressos";
            public const string KeyGetPercentSemanas = "DadosFake-GetPercentSemanas";
            public const string KeyObterTemasRevalidaPermitidos = "DadosFake-ObterTemasRevalidaPermitidos";
            public const string KeySetDeviceToken = "DadosFake-SetDeviceToken";
            public const string KeyGetPermissoes = "DadosFake-GetPermissoes";
            public const string KeyGetNotificacoesClassificadas = "DadosFake-GetNotificacoesClassificadas";
            public const string KeyGetSimuladosByFilters = "DadosFake-GetSimuladosByFilters";
            public const string KeyGetAnosExerciciosPermitidos = "DadosFake-GetAnosExerciciosPermitidos";
            public const string KeyGetServerDate = "DadosFake-GetServerDate";
            public const string KeyGetTipoPerfilUsuario = "DadosFake-GetTipoPerfilUsuario";
        }

        public static class Permissao
        {
            public const int ValidadeListarTemasDeSimuladoPermitidosAnoAtual = 30;
            public const string ListarTemasDeSimuladoPermitidosAnoAtual = "ListarTemasDeSimuladoPermitidosAnoAtual";
            public const int ValidadeGetProdutosContratadosPorAno = 30;
            public const string GetProdutosContratadosPorAno = "GetProdutosContratadosPorAno";
        }

        public static class Combo
        {
            public static TimeSpan Timeout = TimeSpan.FromMinutes(15);
            public const string KeyGetComboAposLancamentoMsPro = "ComboProdutosMsPro";
        }

        public static class Video
        {
            public const string KeyDuracaoVideoRevisaoAula = "DuracaoVideo";
            public static TimeSpan Timeout = TimeSpan.FromHours(12);
        }

        public static class Utilidades
        {
            public const string KeyGetYear = "Utilidades-GetYear";
            public const string KeyIsAntesDatalimiteCache = "IsAntesDatalimiteCache";
        }

        public static class Config
        {
            public const string KeyTempoInadimplenciaTimeout = "TempoInadimplenciaTimeout";
            public const string KeyDeveBloquearVersaoNula = "DeveBloquearVersaoBula";
            public const string KeyVersaoBloqueadaCache = "VersaoBloqueadaCache";
        }

        public static class Produtos
        {
            public const string KeyProdutosContratadosPorAno = "ProdutosContratadosPorAno";
            public const string KeyIsExAlunoTodosProdutos = "IsExAlunoTodosProdutos";
        }

        public static class Login
        {
            public const string KeyLoginClienteGetByFilters = "GetByFilters";                                 
        }

        public static class Pessoa
        {
            public const string KeyGetPersonType = "GetPersonType";
        }

        public static class AvaliacaoAlunoEntity
        {
            public const string KeyAvaliacaoAlunoGetAvaliar = "AvaliacaoAlunoGetAvaliar";
        }
        
        public static class Aluno
        {
            public const string KeyLoginGetMensagemLoginCache = "GetMensagemLoginCache";
            public const string KeyIsAlunoPendentePagamento = "IsAlunoPendentePagamento";
        }

        public static class Access
        {
            public const string KeyGetCondicoesAlunoChamados = "GetCondicoesAlunoChamados";
            public const string KeyGetRegraCondicoes = "GetRegraCondicoes";
            public const string KeyGetCondicoesAlunoOVs = "GetCondicoesAlunoOVs";
        }
    }
}