using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "VideoMiolo", Namespace = "")]
    public class VideosMiolo : List<VideoMiolo>
    {
    }
}