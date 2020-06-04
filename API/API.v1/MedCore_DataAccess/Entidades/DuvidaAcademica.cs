using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "DuvidaAcademica", Namespace = "a")]
    public class DuvidaAcademica
    {
        public DuvidaAcademica()
        {
        }

        [DataMember(Name = "intClientID")]
        public int intClientID { get; set; }

        [DataMember(Name = "ID")]
        public long ID { get; set; }

        [DataMember(Name = "TextoDuvida")]
        public string TextoDuvida { get; set; }

        [DataMember(Name = "dteDuvidaEnviada")]
        public string dteDuvidaEnviada { get; set; }

        [DataMember(Name = "IDEncAprovado")]
        public long? IDEncAprovado { get; set; }

        [DataMember(Name = "StatusDuvidaAluno")]
        public long? StatusDuvidaAluno { get; set; }

        [DataMember(Name = "StatusDuvidaSistema")]
        public long? StatusDuvidaSistema { get; set; }

        [DataMember(Name = "Encaminhamentos")]
        public IEnumerable<DuvidaEncaminhamento> Encaminhamentos { get; set; }

        public int intProductID { get; set; }

        public int intApostilaID { get; set; }

        [DataMember(Name = "UID")]
        public string UID { get; set; }

        [DataMember(Name = "IDProfessor")]
        public long IDProfessor { get; set; }

        [DataMember(Name = "NomeProfessor")]
        public string NomeProfessor { get; set; }

        [DataMember(Name = "intPagina")]
        public int? intPagina { get; set; }

        [DataMember(Name = "UrlImagem")]
        public string UrlImagem { get; set; }
    }
}
