using CAContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAContext.Infra.Data.EntityConfig
{
    public class ContribuicaoArquivoConfig : IEntityTypeConfiguration<ContribuicaoArquivo>
    {
        public void Configure(EntityTypeBuilder<ContribuicaoArquivo> builder)
        {
            builder.ToTable("tblContribuicaoArquivo");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("intContribuicaoArquivoID");

            builder.Property(c => c.ContribuicaoId)
                .HasColumnName("intContribuicaoID");

            builder.Property(c => c.DataCriacao)
                .HasColumnName("dteDataCriacao");

            builder.Property(c => c.Nome)
                .HasColumnName("txtContribuicaoArquivo");

            builder.Property(c => c.Ativo)
                .HasColumnName("bitAtivo");

            builder.Property(c => c.Descricao)
                .HasColumnName("txtDescricao");
            
            builder.Property(c => c.TipoArquivo)
                .HasColumnName("intTipoArquivo");

            builder.Property(c => c.TempoDuracao)
                .HasColumnName("txtDuracao");
        }
    }
}