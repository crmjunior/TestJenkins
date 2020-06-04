using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Apostilas", Namespace = "")]
    public class Apostilas : List<Apostila>
    {
    }
}