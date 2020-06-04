using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Util
{
    public static class ContribuicaoBucketManager
    {
        public static ContribuicaoBucketDTO GetConfig()
        {
            var config = new ContribuicaoBucketDTO();
            config.AccessKeyId = ConfigurationProvider.Get("Settings:ContribuicaoAccessKey");
            config.SecretAccessKey = ConfigurationProvider.Get("Settings:ContribuicaoSecretAccessKey");
            config.Region = ConfigurationProvider.Get("Settings:ContribuicaoRegion");
            config.Bucket = ConfigurationProvider.Get("Settings:ContribuicaoBucket");
            config.ACL = ConfigurationProvider.Get("Settings:ContribuicaoACL");
            config.GetSignedUrl = ConfigurationProvider.Get("Settings:ContribuicaoGetSignedUrl");
            return config;
        }
    }
}