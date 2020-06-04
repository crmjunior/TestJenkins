using System;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Business
{
    public class ChaveamentoVimeoMediMiolo : IChaveamentoVimeo
    {
        public bool GetChaveamento()
        {
            string chaveUsarVimeoGeral = ConfigurationProvider.Get("Settings:AtivarVimeoGeral");
            if (!string.IsNullOrEmpty(chaveUsarVimeoGeral) && Convert.ToBoolean(chaveUsarVimeoGeral))
            {
                
                string chaveUsarVimeoMediMiolo = ConfigurationProvider.Get("Settings:AtivarVimeoMediMiolo");
                if (!string.IsNullOrEmpty(chaveUsarVimeoMediMiolo) && Convert.ToBoolean(chaveUsarVimeoMediMiolo))
                {
                    return true;
                }
            }

            return false;
        }              
    }
}