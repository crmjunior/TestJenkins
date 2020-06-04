using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestaoAvaliacaoComentario", Namespace = "a")]
    public class QuestaoAvaliacaoComentario
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "QuestaoId")]
        public int QuestaoId { get; set; }

        [DataMember(Name = "ExercicioTipoId")]
        public int ExercicioTipoId { get; set; }

        [DataMember(Name = "AlunoMatricula")]
        public int AlunoMatricula { get; set; }

        [DataMember(Name = "AlunoAvatar")]
        public string AlunoAvatar { get; set; }

        [DataMember(Name = "Nota")]
        public int Nota { get; set; }
        
        [DataMember(Name = "TipoComentario")]
        public int TipoComentario { get; set; }

        [DataMember(Name = "DataCadastro", EmitDefaultValue = false)]
        public double DataCadastro { get; set; }

        [DataMember(Name = "ComentarioAvaliacao", EmitDefaultValue = false)]
        public string ComentarioAvaliacao { get; set; }

        [DataMember(Name = "Ativo")]
        public bool Ativo { get; set; }

        [DataMember(Name = "OpcaoComentarioNegativo")]
        public int? OpcaoComentarioNegativo { get; set; }


    }
}