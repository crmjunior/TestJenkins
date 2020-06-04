using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AccessObject", Namespace = "a")]
    

    public class AccessObject
    {
        
        
        public int Id { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "PermiteOffline")]
        public int PermiteOffline { get; set; }

        public string VersaoMinimaOffline { get; set; }

        [DataMember(Name = "DataInicioDisponibilidade")]
        public DateTime DataInicioDisponibilidade { get; set; }

        [DataMember(Name = "DataFimDisponibilidade")]
        public DateTime DataFimDisponibilidade { get; set; }

        [DataMember(Name = "IntOrdem")]
        public int IntOrdem { get; set; }

    }
}