using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Filiais", Namespace = "")]
    public class Filiais: List<Filial>
    {
        
    }
}