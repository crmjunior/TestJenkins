using System;
using System.Linq;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using Microsoft.EntityFrameworkCore;


namespace MedCore_DataAccess.Repository
{
     public class AvaliacaoAlunoEntity
    {
        public int SetAvaliacao(AvaliacaoAluno avaliacao)
        {
            int n = 0;
            bool r = false;

            if (!avaliacao.AvaliarMaisTarde)
                r = Int32.TryParse(avaliacao.Nota, out n);

            var novaEntidade = new tblAvaliacaoAluno()
            {
                dteDataAtualizacao = DateTime.Now,
                intClientID = avaliacao.Matricula,
                intNota = r ? n : 0,
                txtComentario = avaliacao.Comentario,
                bitAvaliarMaisTarde = avaliacao.AvaliarMaisTarde,
                txtPlataforma = avaliacao.Plataforma,
                txtVersaoApp = avaliacao.VersaoApp,
                txtVersaoPlataforma = avaliacao.VersaoPlataforma,
                bitAtivo = true
            };

            using (var ctx = new DesenvContext())
            {
                var rodadaAtual = ctx.tblRodadaAvaliacao.OrderByDescending(x => x.dteDataCriacao).FirstOrDefault().intID;

                var entidade = ctx.tblAvaliacaoAluno.FirstOrDefault(x => x.intClientID == avaliacao.Matricula
                                                                      && x.intRodadaID == rodadaAtual);

                if (entidade == null)
                {
                    novaEntidade.intRodadaID = rodadaAtual;
                    ctx.tblAvaliacaoAluno.Add(novaEntidade);
                }
                else
                {
                    entidade.intRodadaID = rodadaAtual;
                    entidade.dteDataAtualizacao = DateTime.Now;
                    entidade.intNota = r ? n : 0;
                    entidade.txtComentario = avaliacao.Comentario;
                    entidade.bitAvaliarMaisTarde = avaliacao.AvaliarMaisTarde;
                }

                return ctx.SaveChanges();
            }
        }

        public int GetAvaliar(int matricula)
        {

            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetAvaliar))
                return RedisCacheManager.GetItemObject<int>(RedisCacheConstants.DadosFakes.KeyGetAvaliar);

            var isCacheEnable = !RedisCacheManager.CannotCache(RedisCacheConstants.AvaliacaoAlunoEntity.KeyAvaliacaoAlunoGetAvaliar);
            var key = isCacheEnable ? RedisCacheManager.GenerateKey(RedisCacheConstants.AvaliacaoAlunoEntity.KeyAvaliacaoAlunoGetAvaliar, matricula) : null;
            string bitAvaliarStr = isCacheEnable ? RedisCacheManager.GetItemString(key) : null;


            if (!String.IsNullOrEmpty(bitAvaliarStr))
                return Convert.ToInt32(bitAvaliarStr);

            var bitAvaliar = GetAvaliarNonCache(matricula);

            if (isCacheEnable)
            {
                var timeoutHour = 1;
                RedisCacheManager.SetItemString(key, bitAvaliar.ToString(), TimeSpan.FromHours(timeoutHour));
            }

            return bitAvaliar;

        }


        private int GetAvaliarNonCache(int matricula)
        {

            using (var ctx = new DesenvContext())
            {
                var retQuery = (from rav in ctx.tblRodadaAvaliacao
                                join ral in ctx.tblRodadaAluno on rav.intID equals ral.intRodadaId
                                join aval in ctx.tblAvaliacaoAluno
                                on new { a = rav.intID, b = ral.intClientID } equals new { a = aval.intRodadaID, b = aval.intClientID }
                                into avalLeft
                                from subAval in avalLeft.DefaultIfEmpty()
                                where ral.intClientID == matricula
                                orderby rav.dteDataCriacao descending
                                select new
                                {
                                    rav.intID,
                                    avaliacao = subAval
                                });

                var objQuery = retQuery.FirstOrDefault();

                if (objQuery == null)
                    return 0;

                if (objQuery.avaliacao == null)
                    return 1;

                return CanAvaliar(objQuery.avaliacao) ? 1 : 0;
            }

        }

        public bool CanAvaliar(tblAvaliacaoAluno entidade)
        {
            if (!(bool)entidade.bitAvaliarMaisTarde)
                return false;

            return ValidarIntervalo((DateTime)entidade.dteDataAtualizacao);
        }

        public bool ValidarIntervalo(DateTime dataAtualizacao)
        {
            var intervalo =ConfigurationProvider.Get("Settings:horasIntervaloAvaliacaoAluno");

            var intervaloAvaliacao = DateTime.Now - dataAtualizacao;

            return intervaloAvaliacao.TotalHours > Convert.ToInt32(intervalo);
        }
    }
}