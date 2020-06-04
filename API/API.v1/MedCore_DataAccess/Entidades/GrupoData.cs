using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
   [DataContract(Name = "GrupoData", Namespace = "a")]
    public class GrupoData
    {
        [DataMember(Name = "GrupoDataId", EmitDefaultValue = false)]
        public Int32 GrupoDataId { get; set; }

        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public Int32 Id { get; set; }

        [DataMember(Name = "DataHoraInicio", EmitDefaultValue = false)]
        public string DataHoraInicio { get; set; }

        public GrupoData() { 
        
        }
    }
}