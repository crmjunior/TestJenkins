using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;

namespace MedCore_DataAccess.Entidades
{
    public class LogMsPro
    {
        public int Matricula { get; set; }
        public Aplicacoes IdApp { get; set; }
        public Util.Log.MsProLog_Tela Tela { get; set; }
        public Util.Log.MsProLog_Acao Acao { get; set; }
        public string Obs { get; set; }
    }
}