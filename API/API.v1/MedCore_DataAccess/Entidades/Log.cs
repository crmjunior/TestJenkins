using System;
using System.Runtime.Serialization;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Log", Namespace = "a")]
    public class Log
    {
        [DataMember(Name = "SessaoID")]
        public long SessaoID { get; set; }

        [DataMember(Name = "ClientID")]
        public int ClientID { get; set; }

        [DataMember(Name = "PaginaLog")]
        public int PaginaLog { get; set; }

        [DataMember(Name = "Perfil")]
        public Utilidades.TipoAlunoInscricoes Perfil { get; set; }

        [DataMember(Name = "ProdutID")]
        public int ProdutID { get; set; }

        [DataMember(Name = "FilialID")]
        public int FilialID { get; set; }

        [DataMember(Name = "IP")]
        public String IP { get; set; }
    }
}