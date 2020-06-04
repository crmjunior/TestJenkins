using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "VideosMednet", Namespace = "")]
    public class VideosMednet : List<VideoMednet>
    {
    }
}