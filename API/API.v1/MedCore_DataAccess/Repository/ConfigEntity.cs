using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;

namespace MedCore_DataAccess.Repository
{
    public class ConfigEntity : IConfigData
    {
        public List<CacheConfig> GetCacheConfig ()
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetCacheConfig))
                return RedisCacheManager.GetItemObject<List<CacheConfig>>(RedisCacheConstants.DadosFakes.KeyGetCacheConfig);

            using (var ctx = new DesenvContext())
            {
                return ctx.tblMedsoft_CacheConfig.Select(x => new CacheConfig()
                {
                    Horas = x.intMinutos.Value / 60,
                    Nome = x.txtNome,
                    Segundos = x.intMinutos.Value * 60
                }).ToList();
            }
        }

        public float GetTempoInadimplenciaTimeoutParametro()
        {
            var key = RedisCacheConstants.Config.KeyTempoInadimplenciaTimeout;
            if (!RedisCacheManager.CannotCache(key))
            {
                return GetTempoInadimplenciaTimeoutParametroCache();
            }
            else
            {
                string tempoChecaInadimplencia;

                using (var ctx = new DesenvContext())
                {
                    var configs = from t in ctx.tblParametrosGenericos
                                  where t.txtName.Equals("intTimeoutChecaInadimplencia")
                                  select t.txtValue;

                    tempoChecaInadimplencia = configs.FirstOrDefault();
                }

                var timeout = float.Parse(tempoChecaInadimplencia, CultureInfo.InvariantCulture);
                return timeout;
            }                
        }

        public float GetTempoInadimplenciaTimeoutParametroCache()
        {
            string tempoChecaInadimplencia;
            var key = RedisCacheConstants.Config.KeyTempoInadimplenciaTimeout;
            if (!RedisCacheManager.HasAny(key))
            {
                using (var ctx = new DesenvContext())
                {
                    var configs = from t in ctx.tblParametrosGenericos
                    where t.txtName.Equals("intTimeoutChecaInadimplencia")
                    select t.txtValue;
                    tempoChecaInadimplencia = configs.FirstOrDefault();
                }
            }
            else
            {
                return RedisCacheManager.GetItemObject<float>(key);
            }

            var timeout = float.Parse(tempoChecaInadimplencia, CultureInfo.InvariantCulture);
            RedisCacheManager.SetItemObject(key, timeout, TimeSpan.FromHours(1));
            return timeout;
        }

        public bool GetDeveBloquearAppVersaoNulaCache()
        {
            var key = RedisCacheConstants.Config.KeyDeveBloquearVersaoNula;
            if (RedisCacheManager.CannotCache(RedisCacheConstants.Config.KeyDeveBloquearVersaoNula))
            {
                return GetDeveBloquearAppVersaoNula();
            }
            else
            {
                if (RedisCacheManager.HasAny(key))
                {
                    return RedisCacheManager.GetItemObject<bool>(key);
                }
                else
                {
                    var versaoNulaBloquear = GetDeveBloquearAppVersaoNula();
                    RedisCacheManager.SetItemObject(key, versaoNulaBloquear, TimeSpan.FromHours(1));
                    return versaoNulaBloquear;
                }
            }      
        }

        public bool GetDeveBloquearAppVersaoNula()
        {
            string bitBloqueiaAppVersaoNulaString;

            using (var ctx = new DesenvContext())
            {
                var configs = from t in ctx.tblParametrosGenericos
                              where t.txtName.Equals("bitBloqueiaAppVersaoNula")
                              select t.txtValue;

                bitBloqueiaAppVersaoNulaString = configs.FirstOrDefault();
            }

            return Convert.ToBoolean(Convert.ToInt32(bitBloqueiaAppVersaoNulaString));
        }

        public List<OfflineConfig> GetOfflineConfig()
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetOfflineConfig))
                return RedisCacheManager.GetItemObject<List<OfflineConfig>>(RedisCacheConstants.DadosFakes.KeyGetOfflineConfig);

            using(MiniProfiler.Current.Step("Obtendo configuração offline"))
            {
                using (var ctx = new DesenvContext())
                {
                    return ctx.tblOfflineConfig.Select(x => new OfflineConfig()
                    {
                        Id = x.intID,
                        Descricao = x.txtDescricao,
                        Minutos = x.intMinutos
                    }).ToList();
                }
            }
        }
    }
}