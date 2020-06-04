using System;
using System.Linq;
using MedCore_DataAccess.Business.Enums;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Util;
using System.Configuration;

namespace MedCore_DataAccess.Business
{
    public class VersaoAppPermissaoBusiness
    {
        private IVersaoAppPermissaoEntityData _versaoAppPermissaoRepository;

        public VersaoAppPermissaoBusiness(IVersaoAppPermissaoEntityData repository)
        {
            _versaoAppPermissaoRepository = repository;
        }

        public bool VersaoMenorOuIgual(string referencia, string comparacao)
        {
            var arr1 = referencia.Split('.');
            var arr2 = comparacao.Split('.');

            if (Convert.ToInt32(arr1[0]) != Convert.ToInt32(arr2[0])) // change break
                return Convert.ToInt32(arr1[0]) < Convert.ToInt32(arr2[0]);

            if (Convert.ToInt32(arr1[1]) != Convert.ToInt32(arr2[1])) // feature
                return Convert.ToInt32(arr1[1]) < Convert.ToInt32(arr2[1]);

            return Convert.ToInt32(arr1[2]) <= Convert.ToInt32(arr2[2]); // hotfix
        }

        public bool IsVersaoBloqueada(string appVersion, int produtoId)
        {
            if (produtoId == 0)
                produtoId = (int)Aplicacoes.MsProMobile; //Produto default é 17 (MSPro Mobile)

            if (RedisCacheManager.CannotCache(RedisCacheConstants.Config.KeyVersaoBloqueadaCache))
            {
                return VersaoBloqueada(appVersion, produtoId);
            }
            else
            {
                return VersaoBloqueadaCache(appVersion, produtoId);
            }
        }

        private bool VersaoBloqueadaCache(string appVersion, int produtoId)
        {
            try
            {
                var key = String.Format("{0}:{1}", RedisCacheConstants.Config.KeyVersaoBloqueadaCache, appVersion, produtoId);

                if (!RedisCacheManager.HasAny(key))
                {
                    var versao = _versaoAppPermissaoRepository.GetUltimaVersaoBloqueada(produtoId);

                    if (String.IsNullOrEmpty(versao)) // id de produto não existe
                        return false;

                    var bloqueada = VersaoMenorOuIgual(appVersion, versao);
                    RedisCacheManager.SetItemObject(key, bloqueada, TimeSpan.FromHours(1));
                    return bloqueada;
                }
                else
                {
                    return RedisCacheManager.GetItemObject<bool>(key);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }	        

        private bool VersaoBloqueada(string appVersion, int produtoId)
        {
            try
            {
                var versao = _versaoAppPermissaoRepository.GetUltimaVersaoBloqueada(produtoId);

                if (String.IsNullOrEmpty(versao)) // id de produto não existe
                    return false;

                return VersaoMenorOuIgual(appVersion, versao);  
            }
            catch
            {
                throw;
            }
        }

         public int IsVersaoValida(string appVersion, int produtoId)
        {
            if (produtoId == 0)
                produtoId = (int)Aplicacoes.MsProMobile;

            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyVersaoValida))
                return RedisCacheManager.GetItemObject<bool>(RedisCacheConstants.DadosFakes.KeyVersaoValida) ? (int)TipoVersaoValida.VERSAO_VALIDA : (int)TipoVersaoValida.VERSAO_INVALIDA;

            if (produtoId == (int)Aplicacoes.MsProMobile && appVersion == Utilidades.UltimaVersaoIonic1) return (int)TipoVersaoValida.VERSAO_VALIDA;

            var versoesProduto = _versaoAppPermissaoRepository.GetVersoesProduto(produtoId);

            var versao = versoesProduto
                        .OrderByDescending(ma => ma.versao.Major)
                        .ThenByDescending(mi => mi.versao.Minor)
                        .ThenByDescending(bu => bu.versao.Build)
                        .ToList().FirstOrDefault();

            if (String.IsNullOrEmpty(versao.txtVersaoApp)) return (int)TipoVersaoValida.VERSAO_INVALIDA;

            if (versao.bitBloqueio.HasValue && versao.bitBloqueio.Value)
            {
                return VersaoMaior(appVersion, versao.txtVersaoApp) ? (int)TipoVersaoValida.VERSAO_VALIDA : (int)TipoVersaoValida.VERSAO_INVALIDA;
            }
            else
            {
                return VersaoMaiorOuIgual(appVersion, versao.txtVersaoApp) ? (int)TipoVersaoValida.VERSAO_VALIDA : (int)TipoVersaoValida.VERSAO_INVALIDA;
            }
        }

        private bool VersaoMaiorOuIgual(string referencia, string comparacao)
        {
            var arr1 = referencia.Split('.');
            var arr2 = comparacao.Split('.');

            if (Convert.ToInt32(arr1[0]) != Convert.ToInt32(arr2[0])) // change break
                return Convert.ToInt32(arr1[0]) > Convert.ToInt32(arr2[0]);

            if (Convert.ToInt32(arr1[1]) != Convert.ToInt32(arr2[1])) // feature
                return Convert.ToInt32(arr1[1]) > Convert.ToInt32(arr2[1]);

            return Convert.ToInt32(arr1[2]) >= Convert.ToInt32(arr2[2]); // hotfix
        }
        private bool VersaoMaior(string referencia, string comparacao)
        {
            var arr1 = referencia.Split('.');
            var arr2 = comparacao.Split('.');

            if (Convert.ToInt32(arr1[0]) != Convert.ToInt32(arr2[0])) // change break
                return Convert.ToInt32(arr1[0]) > Convert.ToInt32(arr2[0]);

            if (Convert.ToInt32(arr1[1]) != Convert.ToInt32(arr2[1])) // feature
                return Convert.ToInt32(arr1[1]) > Convert.ToInt32(arr2[1]);

            return Convert.ToInt32(arr1[2]) > Convert.ToInt32(arr2[2]); // hotfix
        }

    }
}