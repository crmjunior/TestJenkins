using System;
using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO.Base;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Business
{
    public class RevalidaBusiness
    {
        private IMednetData _mednetRepo;

        public RevalidaBusiness(IMednetData mednetRepo)
        {
            this._mednetRepo = mednetRepo;
        }
        
        public ResponseDTO<List<int>> ObterTemasRevalidaPermitidos(int matriculaId)
        {
            var response = new ResponseDTO<List<int>>();

            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyObterTemasRevalidaPermitidos))
                return RedisCacheManager.GetItemObject<ResponseDTO<List<int>>>(RedisCacheConstants.DadosFakes.KeyObterTemasRevalidaPermitidos);

            try
            {
                var temasRevalida = _mednetRepo.GetVideosRevalida(matriculaId);
                var temasPermitidos = temasRevalida.Select(x => x.GrupoId.Value).Distinct().ToList();

                response.Sucesso = true;
                response.Retorno = temasPermitidos;
            }
            catch (Exception e)
            {
                response.Sucesso = false;
                response.Retorno = null;
                response.Mensagem = e.Message;
            }

            return response;
        }
    }
}