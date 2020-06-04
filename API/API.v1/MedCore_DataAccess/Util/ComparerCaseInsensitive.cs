using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MedCore_DataAccess.Util
{
    public class ComparerCaseInsensitive<T> : EqualityComparer<T>
    {
        public override bool Equals(T obj1, T obj2)
        {
            var json1 = JsonConvert.SerializeObject(obj1);
            var json2 = JsonConvert.SerializeObject(obj2);
            return json1.Equals(json2, StringComparison.OrdinalIgnoreCase);
            //return p1.Descricao.Equals(p2.Descricao, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return json.ToLower().GetHashCode();
            //int hash = 13;
            //hash = (hash * 7) + p.Id.GetHashCode();
            //hash = (hash * 7) + p.Descricao.ToLower().GetHashCode();
            //return hash;
        }
    }
}