using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Questao", Namespace = "a")]
    public class Questao
    {
        [DataMember(Name = "Guid", EmitDefaultValue = false)]
        public String Guid { get; set; }

        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public Int32 Id { get; set; }

        [MaxLength]
        [DataMember(Name = "Enunciado", EmitDefaultValue = false)]
        public String Enunciado { get; set; }

        [DataMember(Name = "EnunciadoImagemId", EmitDefaultValue = false)]
        public int? EnunciadoImagemId { get; set; }

        [DataMember(Name = "Ano", EmitDefaultValue = false)]
        public int Ano { get; set; }

        [DataMember(Name = "Comentario", EmitDefaultValue = false)]
        public String Comentario { get; set; }

        [DataMember(Name = "ImagensComentario", EmitDefaultValue = false)]
        public Imagens ImagensComentario { get; set; }

        [DataMember(Name = "Anulada", EmitDefaultValue = true)]
        public Boolean Anulada { get; set; }

        [DataMember(Name = "CodigoCorrecao", EmitDefaultValue = false)]
        public String CodigoCorrecao { get; set; }

        [DataMember(Name = "Especialidades", EmitDefaultValue = false)]
        public IEnumerable<Especialidade> Especialidades { get; set; }

        [DataMember(Name = "Apostilas", EmitDefaultValue = false)]
        public IEnumerable<Apostila> Apostilas { get; set; }

        [DataMember(Name = "Duracao", EmitDefaultValue = false)]
        public Int32 Duracao { get; set; }

        [DataMember(Name = "Tipo", EmitDefaultValue = false)]
        public Int32 Tipo { get; set; }

        [DataMember(Name = "Ordem", EmitDefaultValue = false)]
        public Int32 Ordem { get; set; }

        [DataMember(Name = "Auto", EmitDefaultValue = false)]
        public Boolean Auto { get; set; }

        [DataMember(Name = "Impressa", EmitDefaultValue = false)]
        public Boolean Impressa { get; set; }

        [DataMember(Name = "OrdemImpressa", EmitDefaultValue = false)]
        public Int32 OrderImpressa { get; set; }

        [DataMember(Name = "GrupoId", EmitDefaultValue = false)]
        public Int32 GrupoId { get; set; }

        [DataMember(Name = "ExercicioDaQuestao", EmitDefaultValue = false)]
        public Exercicio ExercicioDaQuestao { get; set; }

        [DataMember(Name = "Alternativas", EmitDefaultValue = false)]
        public IEnumerable<Alternativa> Alternativas { get; set; }

        [DataMember(Name = "ExercicioTipoID", EmitDefaultValue = false)]
        public Int32 ExercicioTipoID { get; set; }

        [DataMember(Name = "ExercicioTipo", EmitDefaultValue = false)]
        public String ExercicioTipo { get; set; }

        [DataMember(Name = "Discursivas", EmitDefaultValue = false)]
        public List<QuestaoDiscursiva> Discursivas { get; set; }

        [DataMember(Name = "Anotacoes", EmitDefaultValue = false)]
        public List<QuestaoAnotacao> Anotacoes { get; set; }

        [DataMember(Name = "Respondida", EmitDefaultValue = false)]
        public Boolean Respondida { get; set; }

        [DataMember(Name = "Imagens", EmitDefaultValue = false)]
        public IEnumerable<QuestaoImagem> Imagens { get; set; }

        [DataMember(Name = "MediaComentario", EmitDefaultValue = false)]
        public Media MediaComentario { get; set; }

        [DataMember(Name = "RespostaAluno", EmitDefaultValue = false)]
        public String RespostaAluno { get; set; }

        /// <summary>
        /// Define se o aluno acertou a questão ou não 
        /// </summary>
        [DataMember(Name = "Correta", EmitDefaultValue = false)]
        public Boolean Correta { get; set; }

        /// <summary>
        /// Dependendo do Tipo de exercício o Título é montado de maneira diferente
        /// </summary>
        [DataMember(Name = "Titulo", EmitDefaultValue = false)]
        public String Titulo { get; set; }

        [DataMember(Name = "IsGabaritoPosLiberado", EmitDefaultValue = false)]
        public Boolean IsGabaritoPosLiberado { get; set; }

        [DataMember(Name = "SemGabaritoOficial", EmitDefaultValue = false)]
        public Boolean SemGabaritoOficial { get; set; }

        [DataMember(Name = "Observacao", EmitDefaultValue = false)]
        public string Observacao { get; set; }

        [DataMember(Name = "TextoRecurso", EmitDefaultValue = false)]
        public string TextoRecurso { get; set; }

        [DataMember(Name = "AnuladaPosRecursos", EmitDefaultValue = false)]
        public bool AnuladaPosRecursos { get; set; }

        [DataMember(Name = "ComentarioBanca", EmitDefaultValue = false)]
        public string ComentarioBanca { get; set; }

        [DataMember(Name = "DecisaoMedgrupo", EmitDefaultValue = false)]
        public StatusRecurso DecisaoMedgrupo { get; set; }

        [DataMember(Name = "DecisaoBanca", EmitDefaultValue = false)]
        public StatusRecurso DecisaoBanca { get; set; }

        [DataMember(Name = "Video", EmitDefaultValue = false)]
        public bool Video { get; set; }

        [DataMember(Name = "VideoQuestao", EmitDefaultValue = false)]
        public Video VideoQuestao { get; set; }

        [DataMember(Name = "Prova", EmitDefaultValue = false)]
        public Prova Prova { get; set; }

        [DataMember(Name = "EmployeeComentarioID", EmitDefaultValue = false)]
        public Int32 EmployeeComentarioID { get; set; }

        [DataMember(Name = "ProfessorComentario")]
        public string ProfessorComentario { get; set; }

        [DataMember(Name = "PrimeiroComentario", EmitDefaultValue = false)]
        public Professor PrimeiroComentario { get; set; }

        [DataMember(Name = "UltimoComentario", EmitDefaultValue = false)]
        public Professor UltimoComentario { get; set; }

        [DataMember(Name = "PPImagens", EmitDefaultValue = false)]
        public IEnumerable<PPQuestaoImagem> PPImagens { get; set; }

        public enum tipoQuestao
        {
            OBJETIVA = 1,
            DISCURSIVA = 2
        }

        public Exercicio.tipoExercicio tipoExercicio { get; set; }

        public Questao()
        {
            Especialidades = new Especialidades();
            MediaComentario = new Media();
            MediaComentario.Imagens = new List<String>();
            Apostilas = new Apostilas();
        }

        [DataMember(Name = "Concurso", EmitDefaultValue = false)]
        public Concurso Concurso { get; set; }

        [DataMember(Name = "DataQuestao", EmitDefaultValue = false)]
        public double DataQuestao { get; set; }

        [DataMember(Name = "ProtocoladaPara", EmitDefaultValue = false)]
        public Professor ProtocoladaPara { get; set; }

        [DataMember(Name = "Cabecalho", EmitDefaultValue = false)]
        public string Cabecalho { get; set; }

        [DataMember(Name = "NomeAlunoSimulado", EmitDefaultValue = false)]
        public string NomeAlunoSimulado { get; internal set; }

        [DataMember(Name = "GabaritoDiscursiva", EmitDefaultValue = false)]
        public string GabaritoDiscursiva { get; set; }

        [DataMember(Name = "SemGabaritoDiscursiva", EmitDefaultValue = false)]
        public Boolean SemGabaritoDiscursiva { get; set; }

        [DataMember(Name = "IdExercicio", EmitDefaultValue = false)]
        public Int32 IdExercicio { get; set; }

        [DataMember(Name = "Premium", EmitDefaultValue = false)]
        public Boolean Premium { get; set; }

        [DataMember(Name = "PossuiComentario", EmitDefaultValue = false)]
        public Boolean PossuiComentario { get; set; }

        [DataMember(Name = "OrdemPremium", EmitDefaultValue = false)]
        public int OrdemPremium { get; set; }
    }

    public enum EOrigemQuestao
    {
        Concurso = 1,
        Original = 2

    }
}