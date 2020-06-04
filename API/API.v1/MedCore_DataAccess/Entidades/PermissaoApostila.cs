using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "PermissaoApostila", Namespace = "a")]
	[NotMapped]
    public class PermissaoApostila
    {
        [DataMember(Name = "IdApostila")]
        public int IdApostila { get; set; }
        [DataMember(Name = "PermitidoLer")]
        public bool PermitidoLer { get; set; }
        [DataMember(Name = "Bloqueada")]
        public bool Bloqueada { get; set; }
    }
}