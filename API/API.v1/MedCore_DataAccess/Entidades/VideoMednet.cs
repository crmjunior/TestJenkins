using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "VideoMednet", Namespace = "a")]
    public class VideoMednet : Video
    {
        [DataMember(Name = "IdPai", EmitDefaultValue = false)]
        public int IdPai { get; set; }

        [DataMember(Name = "IdProfessor", EmitDefaultValue = false)]
        public int IdProfessor { get; set; }

        [DataMember(Name = "NomeProfessor", EmitDefaultValue = false)]
        public string NomeProfessor { get; set; }

        [DataMember(Name = "Ordem", EmitDefaultValue = false)]
        public int Ordem { get; set; }

        [DataMember(Name = "DataLiberacao", EmitDefaultValue = false)]
        public double DtLiberacao { get; set; }

        [DataMember(Name = "Cue", EmitDefaultValue = false)]
        public int Cue { get; set; }

        [DataMember(Name = "Assistido")]
        public bool Assistido { get; set; }

        [DataMember(Name = "Videos", EmitDefaultValue = false)]
        public VideosMednet Videos { get; set; }

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "DuracaoFormatada", EmitDefaultValue = false)]
        public string DuracaoFormatada { get; set; }

        [DataMember(Name = "Aprovacao", EmitDefaultValue = false)]
        public RevisaoAprovacao Aprovacao { get; set; }

        [DataMember(Name = "IdRevisaoAula", EmitDefaultValue = false)]
        public int IdRevisaoAula { get; set; }

        [DataMember(Name = "IdResumoAula", EmitDefaultValue = false)]
        public int IdResumoAula { get; set; }

        [DataMember(Name = "Aprovacoes", EmitDefaultValue = false)]
        public List<AulaRevisaoVideoAprovacao> Aprovacoes { get; set; }

        [DataMember(Name = "NovoVideo", EmitDefaultValue = false)]
        public bool NovoVideo { get; set; }

        [DataMember(Name = "Visualizacoes", EmitDefaultValue = false)]
        public int Visualizacoes { get; set; }

        [DataMember(Name = "Progresso")]
        public int Progresso { get; set; }

        [DataMember(Name = "TipoVideo", EmitDefaultValue = false)]
        public ETipoVideo TipoVideo { get; set; }

        [DataMember(Name = "Semana")]
        public int Semana { get; set; }

        [DataMember(Name = "IdAula", EmitDefaultValue = false)]
        public int IdAula { get; set; }

        [DataMember(Name = "Apostila", EmitDefaultValue = false)]
        public Exercicio Apostila { get; set; }

        [DataMember(Name = "IdAdaptaMedAula", EmitDefaultValue = false)]
        public int IdAdaptaMedAula { get; set; }

        [DataMember(Name = "UltimaPosicaoAluno")]
        public int UltimaPosicaoAluno { get; set; }

        [DataMember(Name = "IdProvaVideo", EmitDefaultValue = false)]
        public int IdProvaVideo { get; set; }

        [DataMember(Name = "NotaVideo", EmitDefaultValue = false)]
        public int? NotaVideo { get; set; }

        [DataMember(Name = "DataRelease", EmitDefaultValue = false)]
        public DateTime DataRelease { get; set; }
    }

    public enum ETipoVideo
    {
        Questao = 1,
        Apostila = 2,
        Introducao = 3,
        Revisao = 4,
        Revalida = 5,
        Resumo = 6,
        Bonus = 7,
        AdaptaMed = 8,
        MedMe = 9,
        ProvaVideo = 10,
        Especial = 11
    }
}