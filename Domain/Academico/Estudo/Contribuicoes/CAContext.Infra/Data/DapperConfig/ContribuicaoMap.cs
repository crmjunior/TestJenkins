using CAContext.Domain.Entities;
using Dapper.FluentMap.Mapping;

namespace CAContext.Infra.Data.DapperConfig
{
    public class ContribuicaoMap : EntityMap<Contribuicao>
    {
        public ContribuicaoMap()
        {
            Map(c => c.Id)
                .ToColumn("intContribuicaoID");

             Map(c => c.Matricula)
                .ToColumn("intClientID");

            Map(c => c.ApostilaId)
                .ToColumn("intApostilaID");

            Map(c => c.DataCriacao)
                .ToColumn("dteDataCriacao");
            
            Map(c => c.Descricao)
                .ToColumn("txtDescricao");

            Map(c => c.Ativa)
                .ToColumn("bitAtiva");
            
            Map(c => c.Origem)
                .ToColumn("txtOrigem");

            Map(c => c.Editado)
                .ToColumn("bitEditado");

            Map(c => c.NumeroCapitulo)
                .ToColumn("intNumCapitulo");

            Map(c => c.TrechoSelecionado)
                .ToColumn("txtTrechoSelecionado");

            Map(c => c.CodigoMarcacao)
                .ToColumn("txtCodigoMarcacao");

            Map(c => c.AprovadoMedgrupo)
                .ToColumn("bitAprovacaoMedgrupo");

            Map(c => c.OrigemSubnivel)
                .ToColumn("txtOrigemSubnivel");

            Map(c => c.AcademicoId)
                .ToColumn("intMedGrupoID");

            Map(c => c.TipoCategoria)
                .ToColumn("intTipoCategoria");

            Map(c => c.TipoContribuicao)
                .ToColumn("intTipoContribuicao");

            Map(c => c.Estado)
                .ToColumn("txtEstado");

            Map(c => c.OpcaoPrivacidade)
                .ToColumn("intOpcaoPrivacidade");            
        }
    }
}