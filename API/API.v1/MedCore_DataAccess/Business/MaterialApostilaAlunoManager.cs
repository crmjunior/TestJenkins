using Amazon;
using Amazon.S3;
using MedCore_DataAccess.Util;
using System.Text;

namespace MedCore_DataAccess.Business
{
    public class MaterialApostilaAlunoManager
    {
        private AmazonManager amazonManager;

        public MaterialApostilaAlunoManager()
        {
            //key e secret 
            amazonManager = new AmazonManager(ConfigurationProvider.Get("Settings:MaterialApostilaAlunoKey"), ConfigurationProvider.Get("Settings:MaterialApostilaAlunoSecret"));
        }

        public bool SalvarArquivo(string chave, string conteudo)
        {
            var contBytes = Encoding.UTF8.GetBytes(conteudo);
            var ret = amazonManager.UploadFile(ConfigurationProvider.Get("Settings:MaterialApostilaAlunoBucket"), ConfigurationProvider.Get("Settings:MaterialApostilaAlunoSubDirectory"), contBytes, chave, S3CannedACL.AuthenticatedRead);

            if (string.IsNullOrEmpty(ret))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string ObterArquivo(string chave)
        {
            return amazonManager.GetFileText(ConfigurationProvider.Get("Settings:MaterialApostilaAlunoBucket"), ConfigurationProvider.Get("Settings:MaterialApostilaAlunoSubDirectory"), chave);
        }
    }
}