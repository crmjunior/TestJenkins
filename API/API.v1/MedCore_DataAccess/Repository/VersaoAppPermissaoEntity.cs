using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Model;
using System.Collections.Generic;
using System.Linq;

namespace MedCore_DataAccess.Repository
{
    public class VersaoAppPermissaoEntity : IVersaoAppPermissaoEntityData
    {
        public string GetUltimaVersaoBloqueada(int AplicacaoId = 0)
        {
            using (var ctx = new DesenvContext())
            {
                int? VersaoMsProMobile = (int?)Aplicacoes.MsProMobile;

               var ultimaVersao = (from vers in ctx.tblMedSoft_VersaoAppPermissao
                 where vers.bitBloqueio == true
                   && (AplicacaoId != 0 ? vers.intProdutoId == AplicacaoId : vers.intProdutoId == VersaoMsProMobile)
                 select vers).ToList().OrderByDescending(x => x.intId).FirstOrDefault();

                return ultimaVersao == null ? "" : ultimaVersao.txtVersaoApp;

            }
        }

        public string GetUltimaVersao(Aplicacoes app)
        {
            using (var ctx = new DesenvContext())
            {
                var versao = ctx.tblAccess_Object_Application
                    .Where(x => x.intApplicationId == (int)app)
                    .Select(x => x.txtMinVersion)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();
                
                return versao;
            }
        }

        public List<VersaoAppPermissaoDTO> GetVersoesProduto(int produtoId) {
            using (var ctx = new DesenvContext())
            {
                return ctx.tblMedSoft_VersaoAppPermissao
                    .Where(x => x.intProdutoId == produtoId)
                    .Select(s => new VersaoAppPermissaoDTO { 
                        txtVersaoApp = s.txtVersaoApp,
                        intProdutoId = s.intProdutoId,
                        bitBloqueio = s.bitBloqueio
                    })
                    .ToList();
            }
        }

        
    }
}