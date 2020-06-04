using System.Runtime.Serialization;

namespace MedCore_DataAccess.DTO
{
    [DataContract(Name = "VideoApostilaFiltroPost", Namespace = "")]
    public class VideoApostilaFiltroDTO
    {
        [DataMember(Name = "IdsVideos", EmitDefaultValue = false)]
        public string[] IdsVideos { get; set; }

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "IdAplicacao", EmitDefaultValue = false)]
        public int IdAplicacao { get; set; }

        [DataMember(Name = "AppVersion", EmitDefaultValue = false)]
        public string AppVersion { get; set; }


    }
}