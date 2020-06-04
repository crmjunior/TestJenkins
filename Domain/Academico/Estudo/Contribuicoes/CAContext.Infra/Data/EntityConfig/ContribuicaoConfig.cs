using CAContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAContext.Infra.Data.EntityConfig
{
    public class ContribuicaoConfig : IEntityTypeConfiguration<Contribuicao>
    {
        public void Configure(EntityTypeBuilder<Contribuicao> builder)
        {
            builder.ToTable("tblContribuicao");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("intContribuicaoID")
                .HasColumnType("int");

             builder.Property(c => c.Matricula)
                .HasColumnName("intClientID");

            builder.Property(c => c.ApostilaId)
                .HasColumnName("intApostilaID");

            builder.Property(c => c.DataCriacao)
                .HasColumnName("dteDataCriacao");
            
            builder.Property(c => c.Descricao)
                .HasColumnName("txtDescricao");

            builder.Property(c => c.Ativa)
                .HasColumnName("bitAtiva");
            
            builder.Property(c => c.Origem)
                .HasColumnName("txtOrigem");

            builder.Property(c => c.Editado)
                .HasColumnName("bitEditado");

            builder.Property(c => c.NumeroCapitulo)
                .HasColumnName("intNumCapitulo");

            builder.Property(c => c.TrechoSelecionado)
                .HasColumnName("txtTrechoSelecionado");

            builder.Property(c => c.CodigoMarcacao)
                .HasColumnName("txtCodigoMarcacao");

            builder.Property(c => c.AprovadoMedgrupo)
                .HasColumnName("bitAprovacaoMedgrupo");

            builder.Property(c => c.OrigemSubnivel)
                .HasColumnName("txtOrigemSubnivel");

            builder.Property(c => c.AcademicoId)
                .HasColumnName("intMedGrupoID");

            builder.Property(c => c.TipoCategoria)
                .HasColumnName("intTipoCategoria");

            builder.Property(c => c.TipoContribuicao)
                .HasColumnName("intTipoContribuicao");

            builder.Property(c => c.Estado)
                .HasColumnName("txtEstado");

            builder.Property(c => c.OpcaoPrivacidade)
                .HasColumnName("intOpcaoPrivacidade");
        }
    }
}