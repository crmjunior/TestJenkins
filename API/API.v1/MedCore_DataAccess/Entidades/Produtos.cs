using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Produtos", Namespace = "")]
    public class Produtos : List<Produto>
    {
    }
}