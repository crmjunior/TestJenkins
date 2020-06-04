using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Especialidades", Namespace = "a")]
    public class Especialidades : List<Especialidade>
    {
    }
}