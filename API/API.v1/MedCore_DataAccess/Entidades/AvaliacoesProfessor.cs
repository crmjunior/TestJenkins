using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "AvaliacoesProfessor", Namespace = "")]
    public class AvaliacoesProfessor : List<AvaliacaoProfessor>
    {
    }
}