using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AulaAvaliacao", Namespace = "")]
    public class AulaAvaliacao : Apostila
    {

        [DataMember(Name = "IdApostila", EmitDefaultValue = false)]
        public int IdApostila { get { return base.ID; } set { value = base.ID; } }

    }
}