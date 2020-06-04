using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "PPQuestao", Namespace = "a")]
    public class PPQuestao : Questao
    {
        public enum EtapaPortal
        {
            Todas = -1,
            IgnorarFiltros = 0,
            SemGrandeArea = 1,
            SemApostila = 2,
            SemComentario = 3,
            Protocolo = 4,
            MinhasPendencias = 5,
            SemAreaClinica = 6,
            SemVideo = 7,
            AutorizacaoImpressao = 8,
            SomenteFavoritas = 9,
            PendenteClassificacao = 10,
            SomenteComentario = 11
        }

        /// <summary>
        /// Tipos de classificação para a questão (tblConcursoQuestao_Classificacao)
        /// </summary>
        public enum TipoClassificacao
        {
            /// <summary>
            /// Não utilizado mais
            /// </summary>
            Antigas = 1,
            /// <summary>
            /// Classificação de grande área
            /// </summary>
            GrandeArea = 2,
            /// <summary>
            /// Detalhes de classificação para clínica médica
            /// </summary>
            EspecialidadeClinica = 3,
            /// <summary>
            /// Apostila do Medcurso
            /// </summary>
            ApostilaMedCurso = 4,
            /// <summary>
            /// Apostila do MED
            /// </summary>
            ApostilaMed = 5,
            /// <summary>
            /// Usado para questões com problemaas, antigamente. Não utilizado mais
            /// </summary>
            Pendente = 6,
            /// <summary>
            /// Não sei essa definição. A descrição do banco diz:
            /// Tipo Especial - Utilizado na API
            /// </summary>
            Especiais = 7,
            /// <summary>
            /// Apostila de MedEletro
            /// </summary>
            ApostilaECG = 8,
            /// <summary>
            /// Questão de MedEletro
            /// </summary>
            EspecialidadeECG = 9,

            R3_CLINICA = 13,
            R3_CIRURGIA = 14,
            R3_PEDIATRIA = 15,
            R4_GO = 16









        }

        /// <summary>
        /// Tipo de questão
        /// </summary>
        public enum R1R3
        {
            Todos = 0,
            R1 = 1,
            R3 = 2
        }

        [Flags]
        public enum TipoFiltroPalavraChaveLogico
        {
            Enunciado = 1,
            Alternativa = 2,
            Comentario = 4,
            AlternativaCorreta = 8
        }

        /// <summary>
        /// Itens usados para filtros
        /// </summary>
        #region "Parâmetros usados somente para filtros"
        public EtapaPortal FiltroEtapa { get; set; }

        public R1R3 FiltroR1R3 { get; set; }

        public Produto.Produtos FiltroProduto { get; set; }

        public int FiltroIntEmployeeID { get; set; }

        public GrandeArea.EspeciaisFlags FiltroArea { get; set; }

        public int FiltroIDQuestao { get; set; }

        public int FiltroProva { get; set; }

        public int FiltroApostila { get; set; }

        public int FiltroApostilaEntidade { get; set; }

        public int FiltroBitConcursoPremium { get; set; }

        public string FiltroConcurso { get; set; }

        public int FiltroAnoImpressao { get; set; }

        public List<int> FiltroLabels { get; set; }

        public TipoFiltroPalavraChaveLogico TipoFiltroPalavraChave { get; set; }

        public string FiltroPalavraChave { get; set; }

        public bool FiltroVisualizada { get; set; }

        public bool FiltroConcursoBloqueado { get; set; }

        public int FiltroProtocolo { get; set; }

        public bool FiltroExcluirProtocolada { get; set; }

        public bool FiltroExcluirFavorita { get; set; }
        public int FiltroAnoApostila { get; set; }
        public bool FiltroGabaritoPos { get; set; }

        #endregion

        [DataMember(Name = "EspecialidadePrincipal", EmitDefaultValue = false)]
        public Especialidade EspecialidadePrincipal { get; set; }

        [DataMember(Name = "RegistradaPor", EmitDefaultValue = false)]
        public Professor RegistradaPor { get; set; }

        [DataMember(Name = "ClassificadaPor", EmitDefaultValue = false)]
        public IEnumerable<Professor> ClassificadaPor { get; set; }

        [DataMember(Name = "EmClassificacaoPor", EmitDefaultValue = false)]
        public Professor EmClassificacaoPor { get; set; }

        [DataMember(Name = "PPImagensComentario", EmitDefaultValue = false)]
        public IEnumerable<PPQuestaoImagem> PPImagensComentario { get; set; }

        [DataMember(Name = "AutorizadaImpressao", EmitDefaultValue = false)]
        public bool AutorizadaImpressao { get; set; }

        [DataMember(Name = "PendenteAutorizacaoImpressao", EmitDefaultValue = false)]
        public bool PendenteAutorizacaoImpressao { get; set; }

        [DataMember(Name = "ApostilasAutorizacao", EmitDefaultValue = false)]
        public IEnumerable<Apostila> ApostilasAutorizacao { get; set; }

        [DataMember(Name = "ComentarioCoordenador", EmitDefaultValue = false)]
        public string ComentarioCoordenador { get; set; }

        [DataMember(Name = "IdFuncionario", EmitDefaultValue = false)]
        public int IdFuncionario { get; set; }

        [DataMember(Name = "CodBarras", EmitDefaultValue = false)]
        public string CodBarras { get; set; }

        [DataMember(Name = "CodBarrasCrypt", EmitDefaultValue = false)]
        public string CodBarrasCrypt { get; set; }

        [DataMember(Name = "Rascunho", EmitDefaultValue = false)]
        public Rascunho Rascunho { get; set; }

        [DataMember(Name = "Protocolada", EmitDefaultValue = false)]
        public bool Protocolada { get; set; }

        [DataMember(Name = "Gravada", EmitDefaultValue = false)]
        public bool Gravada { get; set; }

        [DataMember(Name = "FavoritadaPor", EmitDefaultValue = false)]
        public string FavoritadaPor { get; set; }
        public int FiltroBitConcursoNaoPremium { get; set; }
    }
}