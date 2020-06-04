using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "RankingSimuladoAluno", Namespace = "a")]
    public class RankingSimuladoAluno
    {
        [DataMember(Name = "Posicao")]
        public string Posicao { get; set; }

        [DataMember(Name = "Nota")]
        public string Nota { get; set; }

        [DataMember(Name = "NickName")]
        public string NickName { get; set; }

        [DataMember(Name = "Especialidade")]
        public string Especialidade { get; set; }

        [DataMember(Name = "UrlAvatar")]
        public string UrlAvatar { get; set; }

        [DataMember(Name = "Simulado")]
        public Simulado Simulado { get; set; }

        [DataMember(Name = "RankingSimulado")]
        public List<RankingSimulado> RankingSimulado { get; set; }

        [DataMember(Name = "QuantidadeParticipantes")]
        public int QuantidadeParticipantes { get; set; }

        [DataMember(Name = "DataRealizacao")]
        public DateTime? DataRealizacao { get; set; }


        [DataMember(Name = "EstatisticasAlunoRankingOnline")]
        public AlunoConcursoEstatistica EstatisticasAlunoRankingOnline { get; set; }

        [DataMember(Name = "EstatisticasAlunoRankingEstudo")]
        public AlunoConcursoEstatistica EstatisticasAlunoRankingEstudo { get; set; }

        [DataMember(Name = "EstatisticasAlunoRankingModoProva")]
        public AlunoConcursoEstatistica EstatisticasAlunoRankingModoProva { get; set; }
    }
}