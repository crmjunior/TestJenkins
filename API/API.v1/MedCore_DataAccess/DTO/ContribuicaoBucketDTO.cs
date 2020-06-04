namespace MedCore_DataAccess.DTO
{
    public class ContribuicaoBucketDTO
    {
        public string AccessKeyId { get; set; }

        public string SecretAccessKey { get; set; }

        public string Region { get; set; }

        public string Bucket { get; set; }

        public string ACL { get; set; }

        public string GetSignedUrl { get; set; }
    }
}