using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Model;
using MedCore_DataAccess.Entidades;
using System.Linq;
using MedCore_DataAccess.Util;
using System;

namespace MedCore_DataAccess.Repository
{
    public class BannerEntity : IBannerData
    {
        public List<Banner> GetBanners(Aplicacoes aplicacao)
        {
            using (var ctx = new DesenvContext())
            {
                var query = from b in ctx.tblBanners
                            join o in ctx.tblAccess_Object on b.intObjectId equals o.intObjectId
                            join ov in ctx.tblAcess_Object_Validity on o.intObjectId equals ov.intObjectId
                            where o.intObjectTypeId == (int)Utilidades.TipoObjetos.BANNER
                                  && o.intApplicationId == (int)aplicacao
                                  && o.bitAtivo == true
                                  && ov.bitAtivo == true
                                  && ov.dteInicio <= DateTime.Now
                                  && ov.dteFim >= DateTime.Now
                            orderby o.intOrdem ascending
                            select new Banner() {
                                ID = b.intBannerId,
                                ObjectId = b.intObjectId,
                                Descricao = b.txtDescricao,
                                Imagem = b.txtImagem,
                                Link = b.txtLink,
                                IsLinkExterno = b.bitLinkExterno,
                                ClickAqui = b.txtClickAqui,
                                IdSimulado = b.intSimuladoID ?? 0                              
                            };

                return query.ToList();
            }
        }
    }
}