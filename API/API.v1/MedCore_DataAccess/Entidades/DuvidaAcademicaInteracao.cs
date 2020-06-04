using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "DuvidaAcademicaInteracao", Namespace = "a")]
    public class DuvidaAcademicaInteracao
    {
        public DuvidaAcademicaInteracao()
        {
            Respostas = new List<DuvidaAcademicaInteracao>();
        }

        [DataMember(Name = "ClientId")]
        public int ClientId { get; set; }

        [DataMember(Name = "DuvidaId")]
        private string _DuvidaId;
        public dynamic DuvidaId
        {
            get => _DuvidaId;
            set => this._DuvidaId = value.ToString();
        }

        [DataMember(Name = "ProdutoId")]
        public int ProdutoId { get; set; }

        [DataMember(Name = "RespostaId")]
        public int? RespostaId { get; set; }

        [DataMember(Name = "RespostaParentId")]
        public int? RespostaParentId { get; set; }

        [DataMember(Name = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "DataCriacao")]
        public DateTime DataCriacao { get; set; }

        [DataMember(Name = "Data")]
        public string Data { get; set; }

        [DataMember(Name = "NomeAluno")]
        public string NomeAluno { get; set; }

        [DataMember(Name = "EspecialidadeAluno")]
        public string EspecialidadeAluno { get; set; }

        [DataMember(Name = "EstadoAluno")]
        public string EstadoAluno { get; set; }

        [DataMember(Name = "QuestaoId")]
        private string _QuestaoId;
        public dynamic QuestaoId
        {
            get => _QuestaoId;
            set => this._QuestaoId = value.ToString();
        }

        [DataMember(Name = "NomeFake")]
        public string NomeFake { get; set; }

        [DataMember(Name = "EstadoFake")]
        public string EstadoFake { get; set; }

        [DataMember(Name = "EspecialidadeId")]
        public int? EspecialidadeId { get; set; }

        [DataMember(Name = "Respostas")]
        public List<DuvidaAcademicaInteracao> Respostas { get; set; }

        [DataMember(Name = "Replicas")]
        public List<DuvidaAcademicaInteracao> Replicas { get; set; }

        [DataMember(Name = "UpVotes")]
        public int UpVotes { get; set; }

        [DataMember(Name = "DownVotes")]
        public int DownVotes { get; set; }

        [DataMember(Name = "TipoAvaliacao")]
        public int TipoAvaliacao { get; set; }

        [DataMember(Name = "ConcursoQuestaoId")]
        public int? ConcursoQuestaoId { get; set; }

        [DataMember(Name = "BitAtiva")]
        public bool? BitAtiva { get; set; }

        [DataMember(Name = "AprovacaoMedGrupo")]
        public bool AprovacaoMedGrupo { get; set; }

        [DataMember(Name = "RespostaMedGrupo")]
        public bool RespostaMedGrupo { get; set; }

        [DataMember(Name = "TipoInteracao")]
        public int? TipoInteracao { get; set; } //ENUM tem que criar

        [DataMember(Name = "TipoCategoria")]
        public EnumTipoCategoria TipoCategoria { get; set; }

        [DataMember(Name = "Denuncia", EmitDefaultValue = true)]
        public bool Denuncia { get; set; }

        [DataMember(Name = "BitAtivaDesenv")]
        public bool BitAtivaDesenv { get; set; }

        [DataMember(Name = "TipoDenuncia")]
        public int TipoDenuncia { get; set; }

        [DataMember(Name = "Dono")]
        public bool Dono { get; set; }

        [DataMember(Name = "VotadoUpvote")]
        public bool VotadoUpvote { get; set; }

        [DataMember(Name = "VotadoDownvote")]
        public bool VotadoDownvote { get; set; }

        [DataMember(Name = "CaminhoImagem")]
        public string CaminhoImagem { get; set; }

        [DataMember(Name = "TipoQuestaoId")]
        public int TipoQuestaoId { get; set; }

        [DataMember(Name = "ExercicioId")]
        public int? ExercicioId { get; set; }

        [DataMember(Name = "CodigoMarcacao")]
        public string CodigoMarcacao { get; set; }

        [DataMember(Name = "TipoExercicioId")]
        public int TipoExercicioId { get; set; }

        [DataMember(Name = "Lida")]
        public bool Lida { get; set; }

        [DataMember(Name = "Questao")]
        public Questao Questao { get; set; }

        [DataMember(Name = "Favorita")]
        public bool Favorita { get; set; }

        [DataMember(Name = "Editada")]
        public bool Editada { get; set; }

        [DataMember(Name = "Congelada")]
        public bool Congelada { get; set; }

        [DataMember(Name = "NRespostas")]
        public int NRespostas { get; set; }

        [DataMember(Name = "CursoAluno")]
        public string CursoAluno { get; set; }

        [DataMember(Name = "NomeAlunoCompleto")]
        public string NomeAlunoCompleto { get; set; }

        [DataMember(Name = "MinhasRespostas")]
        public bool MinhasRespostas { get; set; }

        [DataMember(Name = "Origem")]
        public string Origem { get; set; }

        [DataMember(Name = "OrigemSubnivel")]
        public string OrigemSubnivel { get; set; }

        [DataMember(Name = "NumeroQuestao")]
        public int? NumeroQuestao { get; set; }

        [DataMember(Name = "NumeroCapitulo")]
        private string _NumeroCapitulo;
        public dynamic NumeroCapitulo
        {
            get => _NumeroCapitulo;
            set => this._NumeroCapitulo = value.ToString();
        }

        [DataMember(Name = "NotificacaoId")]
        public int? NotificacaoId { get; set; }

        [DataMember(Name = "BitEditada")]
        public bool? BitEditada { get; set; }

        [DataMember(Name = "QuantidadeDuvidas")]
        public int? QuantidadeDuvidas { get; set; }

        [DataMember(Name = "QuantidadeReplicas")]
        public int? QuantidadeReplicas { get; set; }

        [DataMember(Name = "ApostilaId")]
        public int? ApostilaId { get; set; }

        [DataMember(Name = "TrechoApostila")]
        public string TrechoApostila { get; set; }

        [DataMember(Name = "IsProfessor")]
        public bool IsProfessor { get; set; }

        [DataMember(Name = "BitResponderMaisTarde")]
        public bool BitResponderMaisTarde { get; set; }

        [DataMember(Name = "ProfessoresSelecionados")]
        public List<int> ProfessoresSelecionados { get; set; }

        [DataMember(Name = "ObservacaoMedgrupo")]
        public string ObservacaoMedgrupo { get; set; }

        [DataMember(Name = "MedGrupoId")]
        public int? MedGrupoId { get; set; }

        [DataMember(Name = "OrigemProduto")]
        public string OrigemProduto { get; set; }

        [DataMember(Name = "OrigemQuestaoConcurso")]
        public string OrigemQuestaoConcurso { get; set; }
    }

    public enum EnumTipoCategoria
    {
        Indefinido = 0,
        Capitulo = 1,
        Hypotesis = 2,
        Conteudo = 3
    }

    public enum TipoInteracao
    {
        DuvidaQuestao = 1,
        Resposta = 2,
        DuvidaApostila = 3
    };

    public enum TipoInteracaoDuvida
    {
        Upvote = 1,
        Downvote = 2,
        Favorita = 3,
        Denuncia = 4
    };

    public enum TipoVoto
    {
        Upvote = 1,
        Downvote = -1
    }

    public enum TipoDuvida
    {
        Questao = 0,
        Apostila = 1
    };

    public enum TipoAprovacaoMedGrupo
    {
        Indefinido = 0,
        Aprovado = 1
    }

    public enum TipoStatusAtivoDesativado
    {
        Desativado = 0,
        Ativo = 1
    }

    public enum TipoCategoriaDuvidaApostila
    {
        Indefinido = 0,
        Capitulo = 2
    }
}
