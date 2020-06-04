using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "RegrasAcesso", Namespace = "")]
    public class RegrasAcesso : List<RegraAcesso>
    {
    }
}