using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MedCore_DataAccess.Util;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MedCore_DataAccess.Util
{
    public static class RedisCacheManager 
    {
        private static IDatabase Db { get; set; }
        private static IServer Server { get; set; }
        private static TimeSpan DefaultTimeout { get; set; }

        private static double DefaultTimeoutDaysValue = Convert.ToInt64(ConfigurationProvider.Get("RedisDefaultDaysTimeout"));

        static RedisCacheManager()
        {
            Connect();
        }

        public static bool IsOffline
        {
            get { return Db == null && Server == null; }
        }

        private static void ResetValues()
        {
            Db = null;
            Server = null;
        }

        public static void Connect()
        {
            try
            {
                var timeoutConnection = Convert.ToInt32(ConfigurationProvider.Get("Settings:RedisCacheConnectionTimeout"));

                var configurationOptions = new ConfigurationOptions()
                {
                    ConnectTimeout = timeoutConnection,
                    SyncTimeout = timeoutConnection,
                    EndPoints =
                {
                    { ConfigurationProvider.Get("Settings:RedisCacheConnection"), Convert.ToInt32(ConfigurationProvider.Get("Settings:RedisCacheConnectionPort")) }
                }
                };

                var Connection = ConnectionMultiplexer.Connect(configurationOptions);
                Db = Connection.GetDatabase();
                Server = Connection.GetServer(Connection.GetEndPoints().FirstOrDefault());
                DefaultTimeout = TimeSpan.FromDays(DefaultTimeoutDaysValue);
            }
            catch
            {
                ResetValues();
            }
        }

        public static bool SetItemString(string key, string value)
        {
            try
            {
                return SetItemString(key, value, GetTimeout(key));
            }
            catch
            {
                ResetValues();
                return true;
            }
        }

        public static bool SetItemString(string key, string value, TimeSpan limit)
        {
            try
            {
                return Db.StringSet(key, value, limit);
            }
            catch
            {
                ResetValues();
                return true;
            }
        }

        public static bool SetItemObject(string key, object value)
        {
            try
            {
                return SetItemObject(key, value, GetTimeout(key));
            }
            catch
            {
                ResetValues();
                return true;
            }
        }

        public static bool SetItemObject(string key, object value, TimeSpan limit)
        {
            try
            {
                var serializedObject = JsonConvert.SerializeObject(value);
                return SetItemString(key, serializedObject, limit);
            }
            catch
            {
                ResetValues();
                return true;
            }
        }

        public static bool SetPrimeiroLista(string key, string value)
        {
            try
            {
                return (Db.ListLeftPush(key, value, flags: CommandFlags.FireAndForget) > 0);
            }
            catch
            {
                ResetValues();
                return true;
            }
        }

        public static bool SetUltimoLista(string key, string value)
        {
            try
            {
                return Db.ListRightPush(key, value) > 0;
            }
            catch
            {
                ResetValues();
                return true;
            }
        }
        
        public static bool DeleteGroup(string groupKey)
        {
            try
            {
                var keys = GetKeysList(Server, groupKey + ":*");
                foreach (var key in keys)
                    Db.KeyDelete(key);
                
                return true;
            }
            catch 
            {
                ResetValues();
                return false;
            }
            
        }

        public static bool DeleteAllKeys()
        {
            throw new Exception("Função causava colaterais, foi removida.");
        }

        public static string GetItemString(string key)
        {
            try
            {
                return Db.StringGet(key);
            }
            catch
            {
                ResetValues();
                return null;
            }
        }

        public static T GetItemObject<T>(string key)
        {
            var s = GetItemString(key);
            if (string.IsNullOrEmpty(s))
                return default(T);            

            return JsonConvert.DeserializeObject<T>(s);
        }

        public static T GetPrimeiroItemLista<T>(string key)
        {
            try
            {
                var primeiroString = Db.ListLeftPop(key);
                if (!primeiroString.HasValue)
                    return default(T);

                return JsonConvert.DeserializeObject<T>(primeiroString);
            }
            catch
            {
                ResetValues();
                return default(T);
            }
        }

        public static T GetUltimoItemLista<T>(string key)
        {
            try
            {
                var ultimoString = Db.ListRightPop(key);
                if (!ultimoString.HasValue)
                    return default(T);

                return JsonConvert.DeserializeObject<T>(ultimoString);
            }
            catch
            {
                ResetValues();
                return default(T);
            }
        }

        public static T GetItemIndexLista<T>(string key, long index)
        {
            try
            {
                var itemString = Db.ListGetByIndex(key, index);
                if (!itemString.HasValue)
                    return default(T);

                return JsonConvert.DeserializeObject<T>(itemString);
            }
            catch
            {
                ResetValues();
                return default(T);

            }
        }

        public static long GetListaTamanho(string key)
        {
            try
            {
                return Db.ListLength(key);
            }
            catch
            {
                ResetValues();
                return 0;
            }
        }

        public static bool HasAny(string key)
        {
            try
            {
                return Db.KeyExists(key);
            }
            catch
            {
                ResetValues();
                return false;
            }
        }

        public static List<string> GetAllGroupKeys()
        {
            var keys = new List<string>();
            var classes = typeof(RedisCacheConstants).GetNestedTypes().ToList();
            foreach (var type in classes)
                keys.AddRange(type.GetFields().Where(f => f.Name.Contains("Key")).Select(f => f.Name).ToList());

            return keys;
        }

        public static TimeSpan GetTimeout(string key)
        {
            var classes = typeof(RedisCacheConstants).GetNestedTypes().ToList();
            foreach (var type in classes)
            {
                var listFields = type.GetFields();

                if (listFields.Any(f => f.GetValue(null).Equals(key)))
                {
                    var time = listFields.Where(f => f.Name.Equals("Timeout")).FirstOrDefault();

                    if (time != null)
                        return (TimeSpan)time.GetValue(null);
                    else
                        break;
                }
            }

            return DefaultTimeout;
        }

        private static List<RedisValue> GetObjects(IDatabase db, List<RedisKey> keys)
        {
            try
            {
                var lista = new List<RedisValue>();
                foreach (var key in keys)
                    lista.Add(db.StringGet(key));

                return lista;
            }
            catch
            {
                ResetValues();
                return null;
            }
        }

        private static List<RedisKey> GetKeysList(IServer server, string value)
        {
            try
            {
                return server.Keys(pattern: value).ToList();
            }
            catch
            {
                ResetValues();
                return null;
            }
        }

        public static List<RedisKey> GetAllKeys()
        {
            try
            {
                return GetKeysList(Server, "*");
            }
            catch
            {
                ResetValues();
                return null;
            }
        }

        public static bool CannotCache(string key)
        {
            var settingsValue = ConfigurationProvider.Get("Settings:" + key + Constants.PosFixoRedis);

            if (!String.IsNullOrEmpty(settingsValue))
            {
                bool result;
                if (bool.TryParse(settingsValue, out result))
                    return !result;
            }

            return true;
        }

        public static string GenerateKey(params object[] parametros)
        {
            StringBuilder sb = new StringBuilder();
            String separador = "";
            foreach (var o in parametros)
            {
                sb.Append(separador);
                sb.Append(o);
                separador = ":";
            }

            return sb.ToString();

        }

    }
}