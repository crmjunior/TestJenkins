using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "MaterialApostilaInteracoes", Namespace = "")]
    public class MaterialApostilaInteracoes : List<MaterialApostilaInteracao>
    {
    }
}