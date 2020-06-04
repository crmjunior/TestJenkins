using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MedCore_DataAccess.Entidades
{
   [DataContract(Name = "Combo", Namespace = "a")]

       public class Combo : AccessObject
    {
        public int ComboId { get; set; }

        [DataMember(Name = "Anos")]
        public List<int> Anos { get; set; }

        [DataMember(Name = "tipoLayoutMain")]
        public int tipoLayoutMain { get; set; }

        public string txtMinVersion { get; set; }
    }
}