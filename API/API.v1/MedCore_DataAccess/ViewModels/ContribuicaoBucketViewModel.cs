namespace MedCore_DataAccess.ViewModels
{
    public class ContribuicaoBucketViewModel
    {
        public string AccessKeyId { get; set; }

        public string SecretAccessKey { get; set; }

        public string Region { get; set; }

        public string Bucket { get; set; }

        public string ACL { get; set; }

        public string GetSignedUrl { get; set; }
    }
}