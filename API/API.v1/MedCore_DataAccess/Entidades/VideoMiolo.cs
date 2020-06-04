using System.IO;
using System.Runtime.Serialization;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Namespace = "a")]
    public class VideoMiolo
    {
        [DataMember(Name = "ID")]
        public string ID { get; set; }

        [DataMember(Name = "CodigoVideo")]
        public string CodigoVideo { get; set; }

        [DataMember(Name = "Apostila")]
        public int IDApostila { get; set; }

        [DataMember(Name = "VideoID")]
        public int? VideoID { get; set; }

        [DataMember(Name = "URL")]
        public string URL { get; set; }

        [DataMember(Name = "URLThumb")]
        public string URLThumb { get; set; }

        [DataMember(Name = "HTTPURL")]
        public string HTTPURL { get; set; }

        [DataMember(Name = "BorKey")]
        public string BorKey { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "Assistido")]
        public int Assistido { get; set; }

        internal string qualidade;
        [DataMember(Name = "Qualidade")]
        public string Qualidade
        {
            get
            {
                return string.IsNullOrEmpty(qualidade) ? "720" : qualidade;
            }
            set
            {
                qualidade = value;
            }
        }

        [DataMember(Name = "XML")]
        public string Xml
        {
            get
            {
                if (string.IsNullOrEmpty(BorKey))
                    return string.Empty;

                string baixa = "-A52nNcvp";
                string xml = string.Format("{0}{1}.xml", BorKey, (Qualidade == "100") ? baixa : string.Empty);

                return Path.Combine("http://content.bitsontherun.com/jwp/", xml);
            }
            set { }
        }

        public VideoQualidadeDTO[] Links { get; set; }

        public int? Ano { get; set; }

    }
}