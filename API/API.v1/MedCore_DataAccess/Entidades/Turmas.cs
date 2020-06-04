using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Turmas", Namespace = "")]
    public class Turmas: List<Turma>
    {
    }
}