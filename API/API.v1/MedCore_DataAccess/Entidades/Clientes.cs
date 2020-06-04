using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Clientes", Namespace = "a")]
    public class Clientes : List<Cliente>
    {
       
    }
}