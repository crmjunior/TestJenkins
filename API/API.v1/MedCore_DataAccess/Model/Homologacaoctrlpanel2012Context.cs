using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MedCore_DataAccess.Model
{
    public partial class Homologacaoctrlpanel2012Context : DbContext
    {
        public Homologacaoctrlpanel2012Context()
        {
        }

        public Homologacaoctrlpanel2012Context(DbContextOptions<Homologacaoctrlpanel2012Context> options)
            : base(options)
        {
        }

        public virtual DbSet<mview_ProdutosPorFilial> mview_ProdutosPorFilial { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=mbd.ordomederi.com;Initial Catalog=Homologacaoctrlpanel2012;Persist Security Info=True;User ID=MateriaisDireito;Password=200t@bl7sL1m1t1;MultipleActiveResultSets=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<mview_ProdutosPorFilial>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("mview_ProdutosPorFilial");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
