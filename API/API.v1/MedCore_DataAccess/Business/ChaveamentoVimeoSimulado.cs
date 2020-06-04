using System;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Business
{
    public class ChaveamentoVimeoSimulado : IChaveamentoVimeo
    {
        public bool GetChaveamento()
        {
            string chaveUsarVimeoGeral = ConfigurationProvider.Get("Settings:AtivarVimeoGeral");
            if (!string.IsNullOrEmpty(chaveUsarVimeoGeral) && Convert.ToBoolean(chaveUsarVimeoGeral))
            {
                
                string chaveUsarVimeoSimulado = ConfigurationProvider.Get("Settings:AtivarVimeoQuestaoSimulado");
                if (!string.IsNullOrEmpty(chaveUsarVimeoSimulado) && Convert.ToBoolean(chaveUsarVimeoSimulado))
                {
                    return true;
                }
            }

            return false;
        }
    }
}