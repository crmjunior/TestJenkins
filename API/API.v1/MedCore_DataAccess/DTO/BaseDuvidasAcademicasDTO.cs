using System;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.DTO
{
    public class BaseDuvidasAcademicasDTO 
    {
        public int ClientId { get; set; }
        public int DuvidaId { get; set; }
        public int? RespostaId { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public string NomeAluno { get; set; }
        public string EspecialidadeAluno { get; set; }
        public bool MinhasApostilas { get; set; }
        public bool MinhasQuestoesApostila { get; set; }
        public int RespostaParentId { get; set; }
        public string NomeFake { get; set; }
        public string EstadoFake { get; set; }
        public string EstadoAluno { get; set; }
        public int? QuestaoId { get; set; }
        public int? ProductId { get; set; }
        public int UpVotes { get; set; }
        public bool? BitAtivaAcademico { get; set; }
        public int DownVotes { get; set; }
        public bool? BitVisualizada { get; set; }
        public int TipoAvaliacao { get; set; }
        public int? TipoCategoriaApostila { get; set; }
        public int? NumeroCategoriaApostila { get; set; }
        public bool? AprovacaoMedGrupo { get; set; }
        public bool DenunciaAluno { get; set; }
        public bool BitAtiva { get; set; }
        public string CodigoMarcacao { get; set; }
        public bool? RespostaMedGrupo { get; set; }
        public string TrechoSelecionado { get; set; }
        public int TipoInteracao { get; set; }
        public bool Denuncia { get; set; }
        public bool Arquivada { get; set; }
        public int? InteracaoId { get; set; }
        public int TipoDenuncia { get; set; }
        public bool Dono { get; set; }
        public bool VotadoUpvote { get; set; }
        public string Data { get; set; }
        public bool VotadoDownvote { get; set; }
        public string CaminhoImagem { get; set; }
        public int? TipoQuestaoId { get; set; }
        public int? ExercicioId { get; set; }
        public int? TipoExercicioId { get; set; }
        public bool Lida { get; set; }
        public Questao Questao { get; set; }
        public bool Favorita { get; set; }
        public bool Editada { get; set; }
        public bool Congelada { get; set; }
        public int NRespostas { get; set; }
        public string CursoAluno { get; set; }
        public string NomeAlunoCompleto { get; set; }
        public bool MinhasRespostas { get; set; }
        public string Origem { get; set; }
        public string OrigemSubnivel { get; set; }
        public int? NumeroQuestao { get; set; }
        public bool MaisDe7Dias { get; set; }
        public bool TemRascunho { get; set; }
        public int? NumeroCapitulo { get; set; }
        public int? NotificacaoId { get; set; }
        public bool? BitEditada { get; set; }
        public bool BitEnviada { get; set; }
        public bool BitEncaminhada { get; set; }
        public bool BitResponderMaisTarde { get; set; }
        public int? QuantidadeDuvidas { get; set; }
        public int? ApostilaId { get; set; }
        public int Genero { get; set; }
        public int? TipoCategoria { get; set; }
        public List<DuvidaAcademicaContract> Replicas { get; set; }
        public IEnumerable<tblDuvidasAcademicas_DuvidasEncaminhadas> ProfessoresEncaminhados { get; set; }
        public int QuantidadeReplicas { get; set; }
        public string MedGrupoId { get; set; }
        public string ObservacaoMedGrupo { get; set; }
        public string NomeGestor { get; set; }
    }
}