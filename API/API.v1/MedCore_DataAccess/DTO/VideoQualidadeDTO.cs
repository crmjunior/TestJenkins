using Newtonsoft.Json;

namespace MedCore_DataAccess.DTO
{
    public class VideoQualidadeDTO
    {
        [JsonProperty("quality")]
        public string Qualidade { get; set; }

        [JsonProperty("height")]
        public int Altura { get; set; }

        [JsonProperty("width")]
        public int Largura { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}