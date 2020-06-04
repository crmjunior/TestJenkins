using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Videos", Namespace = "")]
    public class Videos : List<Video>
    {
    }
}