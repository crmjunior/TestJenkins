using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StackExchange.Profiling;

namespace MedCore_API.MedMail
{
    public partial class MedmailContext : DbContext
    {
        public MedmailContext()
        {
        }

        public MedmailContext(DbContextOptions<MedmailContext> options)
            : base(options)
        {
        }

        public virtual DbSet<mail_queue_items> mail_queue_items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connection = GetProfiledConnection();
                optionsBuilder.UseSqlServer(connection.ConnectionString);
                //optionsBuilder.UseSqlServer(@"Data Source=mbd.ordomederi.com;Initial Catalog=homologacaoCtrlPanel2012;Application Name=API;Persist Security Info=True;User ID=MateriaisDireito;Password=200t@bl7sL1m1t1;MultipleActiveResultSets=True");
            }
        }

        private DbConnection GetProfiledConnection()
        {
            var dbConnection = new System.Data.SqlClient.SqlConnection(@"data source=mbd.medgrupo.com.br;initial catalog=Medmail;persist security info=True;user id=MateriaisDireito;password=200t@bl7sL1m1t1;multipleactiveresultsets=True;application name=API&quot;");
            return new StackExchange.Profiling.Data.ProfiledDbConnection(dbConnection, MiniProfiler.Current);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<mail_queue_items>(entity =>
            {
                entity.HasKey(e => e.mailitem_id)
                    .IsClustered(false);

                entity.HasIndex(e => e.date_queued)
                    .HasName("_net_IX_mail_queue_items_date_queued");

                entity.HasIndex(e => e.mailitem_id)
                    .HasName("_IDX_Medmail_Queue_Items_MAILITEM_ID")
                    .IsClustered();

                entity.HasIndex(e => new { e.status, e.RetryCount })
                    .HasName("_net_IX_mail_queue_items_status");

                entity.Property(e => e.blind_copy_recipients).HasMaxLength(2048);

                entity.Property(e => e.body)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.body_format)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.copy_recipients).HasMaxLength(2048);

                entity.Property(e => e.date_queued).HasColumnType("datetime");

                entity.Property(e => e.profile_id)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.recipients)
                    .IsRequired()
                    .HasMaxLength(2048);

                entity.Property(e => e.subject)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
