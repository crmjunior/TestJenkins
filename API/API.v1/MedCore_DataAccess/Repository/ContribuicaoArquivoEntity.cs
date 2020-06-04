using MedCore_DataAccess.Model;
using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedCore_DataAccess.Repository
{
    public class ContribuicaoArquivoEntity : IContribuicaoArquivoData
    {

        public int DeletarContribuicaoArquivo(IList<int> lstArquivoIDs)
        {
            using (var ctx = new DesenvContext())
            {
                var lstContr = ctx.tblContribuicaoArquivo.Where(x => lstArquivoIDs.Contains(x.intContribuicaoArquivoID));

                foreach (var arq in lstContr)
                    arq.bitAtivo = false;

                return ctx.SaveChanges();
            }
        }

        public int InserirContribuicaoArquivo(ContribuicaoArquivo contribuicaoArquivo)
        { 
            using (var ctx = new DesenvContext())
            {
                var entity = new tblContribuicaoArquivo()
                {
                    intContribuicaoID = contribuicaoArquivo.ContribuicaoID,
                    intTipoArquivo = (int)contribuicaoArquivo.Tipo,
                    txtDescricao = contribuicaoArquivo.Descricao,
                    bitAtivo = contribuicaoArquivo.BitAtivo,
                    txtContribuicaoArquivo = contribuicaoArquivo.Nome,
                    dteDataCriacao = DateTime.Now,
                    txtDuracao = contribuicaoArquivo.Time
                };

                ctx.tblContribuicaoArquivo.Add(entity);
                return ctx.SaveChanges();
            }
        }

        public int UpdateContribuicaoArquivo(ContribuicaoArquivo arquivo)
        {
            using (var ctx = new DesenvContext())
            {
                var entity = ctx.tblContribuicaoArquivo.FirstOrDefault(x => x.intContribuicaoArquivoID == arquivo.Id);

                entity.txtDescricao = arquivo.Descricao;
                entity.bitAtivo = arquivo.BitAtivo;
                return ctx.SaveChanges();
            }
        }

        public IList<ContribuicaoArquivo> ListarArquivosContribuicao(int idContribuicao)
        {
            using (var ctx = new DesenvContext())
            {
                var arquivos = ctx.tblContribuicaoArquivo.Where(x => x.intContribuicaoID == idContribuicao);
                var list = arquivos
                    .Where(w => w.bitAtivo == true)
                    .Select(x => new ContribuicaoArquivo()
                    {
                        Id = x.intContribuicaoArquivoID,
                        ContribuicaoID = x.intContribuicaoID.Value,
                        DataCriacao = x.dteDataCriacao.Value,
                        Nome = x.txtContribuicaoArquivo,
                        Descricao = x.txtDescricao,
                        Tipo = (EnumTipoArquivoContribuicao)x.intTipoArquivo,
                        BitAtivo = x.bitAtivo.Value,
                        Time = x.txtDuracao
                    });

                return list.ToList();
            }
        }
    }
}
