using System.ComponentModel;

namespace MedCore_DataAccess.Contracts.Enums
{
    public enum TipoOperacoesConcursoEnum
    {
        [Description("Enunciado Alterado")]
        EnunciadoAlterado = 1,
        [Description("Alterou número de questões")]
        AlterouNumeroQuestoesProva = 2,
        [Description("Alterou Alternativa {0}")]
        DescricaoAlternativaAlterada = 3,
        [Description("")]
        livre = 4,
        [Description("Marcou gabarito final para a alternativa {0}")]
        MarcouGabaritoFinal = 5,
        [Description("Marcou gabarito preliminar para a alternativa {0}")]
        MarcouGabaritoPreliminar = 6,
        [Description("Desmarcou gabarito final para a alternativa {0}")]
        DesmarcouGabaritoFinal = 7,
        [Description("Desmarcou gabarito preliminar para a alternativa {0}")]
        DesmarcouGabaritoPreliminar = 8,
        [Description("Alterou para discursiva")]
        AlterouDiscursiva = 9,
        [Description("Alterou para objetiva")]
        AlterouObjetiva = 10,
        [Description("Incluiu imagem no enunciado")]
        CadastrouImagemEnunciado = 11,
        [Description("Excluiu imagem no enunciado")]
        DeletouImagemEnunciado = 12,
        [Description("Incluiu a alternativa {0}")]
        AlternativaCadastrada = 13,
        [Description("Excluiu a alternativa {0}")]
        RemoveuAlternativa = 14,
        [Description("Marcou novo gabarito liberado pós recurso")]
        MarcouNovoGabaritoLiberadoPosRecurso = 15,
        [Description("Desmarcou novo gabarito liberado pós recurso")]
        DesMarcouNovoGabaritoLiberadoPosRecurso = 16,
        [Description("Marcou questão anulada pré recurso")]
        MarcouQuestaoAnuladaPreRecurso = 17,
        [Description("Desmarcou questão anulada pré recurso")]
        DesMarcouQuestaoAnuladaPreRecurso = 18,
        [Description("Marcou questao anulada pós recurso")]
        MarcouQuestaoAnuladaPosRecurso = 19,
        [Description("Desmarcou questão anulada pós recurso")]
        DesMarcouQuestaoAnuladaPosRecurso = 20,
        [Description("Incluiu imagem alternativa {0}")]
        IncluiuImagemAlternativa = 21,
        [Description("Removeu imagem da alternativa {0}")]
        RemoveuImagemDaAlternativa = 22,
        [Description("Desmarcou gabarito oficial")]
        DesmarcouGabaritoOficial = 23,
        [Description("Marcou gabarito oficial")]
        MarcouGabaritoOficial = 24,
        [Description("Alterou imagem da alternativa {0}")]
        AlterouImagemAlternativa = 25,
        [Description("Marcou Prova sem gabarito")]
        MarcouProvaSemGabarito = 26,
        [Description("Desmarcou Prova sem gabarito")]
        DesmarcouProvaSemGabarito = 27,
        [Description("Cadastrou caso clínico")]
        CadastrouCasoClinico = 28,
        [Description("Alterou caso clínico")]
        AlterouCasoClinico = 29,
        [Description("Removeu caso clínico")]
        RemoveuCasoClinico = 30,
        [Description("Associou {0} ao perfil {1}")]
        AssociouUsuarioPerfil = 31,
        [Description("Desassociou {0} do perfil {1}")]
        RemoveuUsuarioPerfil = 32,
        [Description("Removeu a foto de perfil da matricula {0}")]
        RemoveuFotoPerfil = 33,
        [Description("Removeu ranking de acerto da matricula {0}, prova {1}")]
        RemoveuRankingAcerto = 34
    }
}