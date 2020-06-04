using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "DuvidaEncaminhamento", Namespace = "a")]
    public class DuvidaEncaminhamento
    {
        [DataMember(Name = "ID")]
        public long ID { get; set; }

        [DataMember(Name = "IDProfessor")]
        public long IDProfessor { get; set; }

        [DataMember(Name = "Interacoes")]
        public IEnumerable<DuvidaInteracao> Interacoes { get; set; }

        [DataMember(Name = "IDIntAprovada")]
        public long? IDIntAprovada { get; set; }
    }
}
