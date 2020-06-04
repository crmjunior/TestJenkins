using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MedCore_DataAccess.DTO
{
    public class VersaoDTO
    {
        public bool VersaoValida { get; set; }
        public bool VersaoAtualizada { get; set; }
        public string NumeroUltimaVersao { get; set; }
    }

    public class MetadadosLojaDTO
    {
        [JsonProperty(PropertyName = "results")]
        public List<ReleaseLojaDTO> Releases { get; set; }
    }

    public class ReleaseLojaDTO
    {
        [JsonProperty(PropertyName = "currentVersionReleaseDate")]
        public DateTime CurrentVersionReleaseDate { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
    }
}