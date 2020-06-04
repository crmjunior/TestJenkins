using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MedCoreAPI.Util.Extensions
{
    public static class ConvertToJsonExtensions{
        public static string ToJsonString(this Dictionary<string, string> dictionary)
        {
            var tuples = dictionary.Select(d =>
                string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", tuples) + "}";
        }

        public static Dictionary<string, string> ToDictionary(this String jsonString)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
        }
    }
}