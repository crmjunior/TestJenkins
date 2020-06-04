using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "PermissoesApostilas", Namespace = "")]
    public class PermissoesApostilas : List<PermissaoApostila>
    {
    }
}