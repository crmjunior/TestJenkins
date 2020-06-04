using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class ExercicioDTO : IEqualityComparer<ExercicioDTO>
    {

        public int Ano { get; set; }

        public string Descricao { get; set; }

        public int ID { get; set; }

        public string ExercicioName { get; set; }

        public double DataInicio { get; set; }

        public double DataFim { get; set; }

        public int IdTipoRealizacao { get; set; }

        public int Online { get; set; }

        public bool Ativo { get; set; }

        public int Duracao { get; set; }

        public DateTime DtLiberacaoRanking { get; set; }

        public int TipoId { get; set; }

        public bool bitAndamento { get; set; }
        public bool Equals(ExercicioDTO one, ExercicioDTO other)
        {
            if (   one.Ano == other.Ano
                && one.Descricao == other.Descricao
                && one.ID == other.ID
                && one.ExercicioName == other.ExercicioName
                && one.DataInicio == other.DataInicio
                && one.DataFim == other.DataFim
                && one.IdTipoRealizacao == other.IdTipoRealizacao
                && one.Online == other.Online
                && one.Ativo == other.Ativo
                && one.DtLiberacaoRanking.Equals(other.DtLiberacaoRanking))
                return true;
            else
                return false;
        }

        public int GetHashCode(ExercicioDTO one)
        {
            int AnoHash = one.Ano == 0 ? 0 : one.Ano.GetHashCode();
            int DescricaoHash = one.Descricao == null ? 0 : one.Descricao.GetHashCode();
            int IDHash = one.ID == 0 ? 0 : one.ID.GetHashCode();
            int ExercicioNameHash = string.IsNullOrEmpty(one.ExercicioName) ? 0 : one.ExercicioName.GetHashCode();
            int DataInicioHash = one.DataInicio == 0 ? 0 : one.DataInicio.GetHashCode();
            int DataFimHash = one.DataFim == 0 ? 0 : one.DataFim.GetHashCode();
            int IdTipoRealizacaoHash = one.IdTipoRealizacao == 0 ? 0 : one.IdTipoRealizacao.GetHashCode();
            int OnlineHash = one.Online == 0 ? 0 : one.Online.GetHashCode();
            int AtivoHash = one.Ativo? 0 : one.Ativo.GetHashCode();
            int DtLiberacaoRankingHash = one.DtLiberacaoRanking == null ? 0 : one.DtLiberacaoRanking.GetHashCode();

            return AnoHash ^ DescricaoHash ^ IDHash ^ ExercicioNameHash ^ DataInicioHash ^ DataFimHash ^ IdTipoRealizacaoHash ^ OnlineHash ^ AtivoHash ^ DtLiberacaoRankingHash;
        }
  
    }
}