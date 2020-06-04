using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Responsabilidades")]
    public class Responsabilidades : List<Responsabilidade>
    {
    }
}