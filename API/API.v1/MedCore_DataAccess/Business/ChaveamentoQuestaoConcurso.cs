using System;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Business
{
    public class ChaveamentoQuestaoConcurso : IChaveamentoVimeo
    {
        public bool GetChaveamento()
        {
            string chaveUsarVimeoGeral = ConfigurationProvider.Get("Settings:AtivarVimeoGeral");
            if (!string.IsNullOrEmpty(chaveUsarVimeoGeral) && Convert.ToBoolean(chaveUsarVimeoGeral))
            {
                
                string chaveUsarVimeoMediMiolo = ConfigurationProvider.Get("Settings:AtivarVimeoQuestaoConcurso");
                if (!string.IsNullOrEmpty(chaveUsarVimeoMediMiolo) && Convert.ToBoolean(chaveUsarVimeoMediMiolo))
                {
                    return true;
                }
            }

            return false;
        }   
    }
}