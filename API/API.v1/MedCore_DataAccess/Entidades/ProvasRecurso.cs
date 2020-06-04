using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "RecursoProvas", Namespace = "a")]
    public class ProvasRecurso : List<ProvaRecurso>
    {

    }
}