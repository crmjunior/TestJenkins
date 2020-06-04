using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "PositionVideo", Namespace = "")]
    public class ProgressoVideo
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Matricula")]
        public int Matricula { get; set; }

        [DataMember(Name = "ProgressoSegundo")]
        public int ProgressoSegundo { get; set; }

        [DataMember(Name = "ProgressoPercentual")]
        public int ProgressoPercentual { get; set; }

        [DataMember(Name = "IdRevisaoAula")]
        public int IdRevisaoAula { get; set; }

        [DataMember(Name = "UltimaAtualizacao")]
        public double UltimaAtualizacao { get; set; }

        [DataMember(Name = "IdResumoAula")]
        public int IdResumoAula { get; set; }

        [DataMember(Name = "IdRevalidaAula")]
        public int IdRevalidaAula { get; set; }

        [DataMember(Name = "IdAdaptaMedAula")]
        public int IdAdaptaMedAula { get; set; }

        [DataMember(Name = "IdMedme")]
        public int IdMedme { get; set; }

        [DataMember(Name = "DuracaoVideo")]
        public double DuracaoVideo { get; set; }
    }
}