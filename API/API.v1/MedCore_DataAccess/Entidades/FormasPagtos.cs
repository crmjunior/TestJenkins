using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "FormasPagtos", Namespace = "a")]
    public class FormasPagtos : List<FormaPagto>
    {
    }
}