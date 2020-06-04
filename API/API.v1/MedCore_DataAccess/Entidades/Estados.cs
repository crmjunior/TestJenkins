using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Estados", Namespace = "")]
    public class Estados : List<Estado>
    {
        
    }
}