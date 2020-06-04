namespace MedCore_DataAccess.Contracts.Data
{
     public interface IAuthData
    {
        bool Login(string register, string senha, int idAplicacao);
    }
}