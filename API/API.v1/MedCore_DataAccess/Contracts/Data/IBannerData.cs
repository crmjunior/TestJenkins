using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IBannerData
    {
        List<Banner> GetBanners(Aplicacoes aplicacao);
    }
}