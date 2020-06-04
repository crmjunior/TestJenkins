using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Pessoas", Namespace = "")]
    public class Pessoas: List<Pessoa>
    {
    }
}