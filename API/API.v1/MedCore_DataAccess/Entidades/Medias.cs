using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Medias", Namespace = "")]
    public class Medias : List<Media>
    {


    }
}