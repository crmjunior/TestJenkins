using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class JsonFileVimeoDTO
    {
        public string quality { get; set; }
        public string link { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }

    public class JsonVimeoVideoDTO
    {
        public int total { get; set; }
        public int page { get; set; }
        public int per_page { get; set; }
        public Paging paging { get; set; }
        public List<Datum> data { get; set; }
    }


    public class Paging
    {
        public object next { get; set; }
        public object previous { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Size
    {
        public int width { get; set; }
        public int height { get; set; }
        public string link { get; set; }
        public string link_with_play_button { get; set; }
    }

    public class Datum
    {
        public string uri { get; set; }
        public bool active { get; set; }
        public string type { get; set; }
        public List<Size> sizes { get; set; }
        public string resource_key { get; set; }
    }

    public class jsonVimeo
    {
        public string duration { get; set; }
        public Datum pictures { get; set; }
        public string status { get; set; }
        public string transcodeStatus { get; set; }
        public string uploadStatus { get; set; }
        public List<JsonFileVimeoDTO> files { get; set; }
    }

    public class JsonVimeoOEmbed
    { 
        public string type { get; set; }
        public string version { get; set; }
        public string provider_name { get; set; }
        public string provider_url { get; set; }
        public string title { get; set; }
        public string author_name { get; set; }
        public string author_url { get; set; }
        public string is_plus { get; set; }
        public string account_type { get; set; }
        public string html { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string duration { get; set; }
        public string description { get; set; }
        public string thumbnail_url { get; set; }
        public string thumbnail_width { get; set; }
        public string thumbnail_height { get; set; }
        public string thumbnail_url_with_play_button { get; set; }
        public string upload_date { get; set; }
        public string video_id { get; set; }
        public string uri { get; set; }
    }
}