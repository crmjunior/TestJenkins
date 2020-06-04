using System;
using System.Data.Common;
using MedCore_DataAccess.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StackExchange.Profiling;

namespace MedCore_API.Academico
{
    public partial class AcademicoContext : DbContext
    {
        public AcademicoContext()
        {
        }

        public AcademicoContext(DbContextOptions<AcademicoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bor2VimeoControl> Bor2VimeoControl { get; set; }
        public virtual DbSet<tblCartaoResposta_Discursiva> tblCartaoResposta_Discursiva { get; set; }
        public virtual DbSet<tblCartaoResposta_objetiva> tblCartaoResposta_objetiva { get; set; }
        public virtual DbSet<tblCartaoResposta_objetiva_Simulado_Online> tblCartaoResposta_objetiva_Simulado_Online { get; set; }
        public virtual DbSet<tblEspecialidades> tblEspecialidades { get; set; }
        public virtual DbSet<tblEspecialidadesSimulado> tblEspecialidadesSimulado { get; set; }
        public virtual DbSet<tblExercicio_Historico> tblExercicio_Historico { get; set; }
        public virtual DbSet<tblLogLeituraCartaoRespostaSimulados> tblLogLeituraCartaoRespostaSimulados { get; set; }
        public virtual DbSet<tblLogSimuladoAlunoTurma> tblLogSimuladoAlunoTurma { get; set; }
        public virtual DbSet<tblQuestaoAlternativas> tblQuestaoAlternativas { get; set; }
        public virtual DbSet<tblQuestao_CometariosLog> tblQuestao_CometariosLog { get; set; }
        public virtual DbSet<tblQuestao_Imagem> tblQuestao_Imagem { get; set; }
        public virtual DbSet<tblQuestao_Marcacao> tblQuestao_Marcacao { get; set; }
        public virtual DbSet<tblQuestao_Simulado> tblQuestao_Simulado { get; set; }
        public virtual DbSet<tblQuestoes> tblQuestoes { get; set; }
        public virtual DbSet<tblQuestoesSimuladoImagem_Comentario> tblQuestoesSimuladoImagem_Comentario { get; set; }
        public virtual DbSet<tblSimulado> tblSimulado { get; set; }
        public virtual DbSet<tblSimuladoConcurso_Excecao> tblSimuladoConcurso_Excecao { get; set; }
        public virtual DbSet<tblSimuladoCorrecaoCasoClinico> tblSimuladoCorrecaoCasoClinico { get; set; }
        public virtual DbSet<tblSimuladoImportacaoCartaoResposta> tblSimuladoImportacaoCartaoResposta { get; set; }
        public virtual DbSet<tblSimuladoOnline_Consolidado> tblSimuladoOnline_Consolidado { get; set; }
        public virtual DbSet<tblSimuladoOnline_Excecao> tblSimuladoOnline_Excecao { get; set; }
        public virtual DbSet<tblSimuladoOrdenacao> tblSimuladoOrdenacao { get; set; }
        public virtual DbSet<tblSimuladoRanking_Fase01> tblSimuladoRanking_Fase01 { get; set; }
        public virtual DbSet<tblSimuladoRanking_Fase02> tblSimuladoRanking_Fase02 { get; set; }
        public virtual DbSet<tblSimuladoRespostas> tblSimuladoRespostas { get; set; }
        public virtual DbSet<tblSimuladoResultados> tblSimuladoResultados { get; set; }
        public virtual DbSet<tblSimuladoResultadosDiscursivas> tblSimuladoResultadosDiscursivas { get; set; }
        public virtual DbSet<tblSimuladoVersao> tblSimuladoVersao { get; set; }
        public virtual DbSet<tblVideo> tblVideo { get; set; }
        public virtual DbSet<tblVideo_Questao_Simulado> tblVideo_Questao_Simulado { get; set; }
        public virtual DbSet<tblVideo_dsv> tblVideo_dsv { get; set; }

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
            var dbConnection = new System.Data.SqlClient.SqlConnection(ConfigurationProvider.Get("ConnectionStrings:AcademicoRDS"));
            return new StackExchange.Profiling.Data.ProfiledDbConnection(dbConnection, MiniProfiler.Current);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bor2VimeoControl>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.FinishedAt).HasColumnType("datetime");

                entity.Property(e => e.txtFileName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.txtPath)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblCartaoResposta_Discursiva>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.dblNota, e.txtResposta, e.intDicursivaId })
                    .HasName("_net_IX_tblCartaoResposta_Discursiva_intDicursivaId");

                entity.HasIndex(e => new { e.intID, e.intQuestaoDiscursivaID, e.intHistoricoExercicioID, e.intExercicioTipoId })
                    .HasName("IX_tblCartaoResposta_Discursiva_intExercicioTipoId_02F37");

                entity.HasIndex(e => new { e.intQuestaoDiscursivaID, e.txtResposta, e.intExercicioTipoId, e.dteCadastro, e.intID, e.intHistoricoExercicioID })
                    .HasName("_net_IX_tblCartaoResposta_Discursiva_intHistoricoExercicioID");

                entity.HasIndex(e => new { e.intID, e.intHistoricoExercicioID, e.txtResposta, e.intDicursivaId, e.dteCadastro, e.dblNota, e.intQuestaoDiscursivaID, e.intExercicioTipoId })
                    .HasName("_net_IX_tblCartaoResposta_Discursiva_intQuestaoDiscursivaID");

                entity.HasIndex(e => new { e.intID, e.intQuestaoDiscursivaID, e.intHistoricoExercicioID, e.txtResposta, e.intDicursivaId, e.dteCadastro, e.dblNota, e.intExercicioTipoId })
                    .HasName("IX_tblCartaoResposta_Discursiva_intExercicioTipoId_BCD1E");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.intExercicioTipoId).HasDefaultValueSql("((1))");

                entity.Property(e => e.txtResposta)
                    .HasMaxLength(310)
                    .IsUnicode(false);

                entity.HasOne(d => d.intHistoricoExercicio)
                    .WithMany(p => p.tblCartaoResposta_Discursiva)
                    .HasForeignKey(d => d.intHistoricoExercicioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCartaoResposta_Discursiva_tblExercicio_Historico");
            });

            modelBuilder.Entity<tblCartaoResposta_objetiva>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK_tblCartaoResposta_2");

                entity.HasIndex(e => new { e.intID, e.intQuestaoID, e.intClientID })
                    .HasName("IX_tblCartaoResposta_objetiva_intQuestaoID_intClientID_649E3");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intQuestaoID, e.txtLetraAlternativa, e.intExercicioTipoId })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_intQuestaoID_txtLetraAlternativa_intExercicioTipoID_2");

                entity.HasIndex(e => new { e.intID, e.intClientID, e.intQuestaoID, e.intExercicioTipoId })
                    .HasName("IX_tblCartaoResposta_objetiva_intClientID_intQuestaoID_intExercicioTipoId_C02D3");

                entity.HasIndex(e => new { e.intID, e.intQuestaoID, e.intClientID, e.intExercicioTipoId })
                    .HasName("IX_tblCartaoResposta_objetiva_intQuestaoID_intClientID_intExercicioTipoId_E9F3A");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.txtLetraAlternativa, e.dteCadastro, e.intQuestaoID, e.intExercicioTipoId })
                    .HasName("IX_tblCartaoResposta_objetiva_intQuestaoID_intExercicioTipoId_6E844_2");

                entity.HasIndex(e => new { e.intID, e.intHistoricoExercicioID, e.intExercicioTipoId, e.intClientID, e.intQuestaoID })
                    .HasName("IX_tblCartaoResposta_objetiva_intExercicioTipoId_intClientID_intQuestaoID_7D8D6");

                entity.HasIndex(e => new { e.intID, e.intHistoricoExercicioID, e.txtLetraAlternativa, e.intQuestaoID, e.intExercicioTipoId })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_intQuestaoID_intExercicioTipoId_2");

                entity.HasIndex(e => new { e.intQuestaoID, e.txtLetraAlternativa, e.dteCadastro, e.intClientID, e.intExercicioTipoId })
                    .HasName("IX_tblCartaoResposta_objetiva_intClientID_intExercicioTipoId_43819");

                entity.HasIndex(e => new { e.intID, e.intQuestaoID, e.intHistoricoExercicioID, e.txtLetraAlternativa, e.intExercicioTipoId, e.intClientID })
                    .HasName("idxClientID");

                entity.HasIndex(e => new { e.intQuestaoID, e.txtLetraAlternativa, e.intExercicioTipoId, e.dteCadastro, e.intID, e.intHistoricoExercicioID })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_intHistoricoExercicioID_2");

                entity.HasIndex(e => new { e.intID, e.intQuestaoID, e.intHistoricoExercicioID, e.txtLetraAlternativa, e.guidQuestao, e.dteCadastro, e.intExercicioTipoId })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_intExercicioTipoId_2");

                entity.HasIndex(e => new { e.txtLetraAlternativa, e.intID, e.guidQuestao, e.intExercicioTipoId, e.dteCadastro, e.intQuestaoID, e.intHistoricoExercicioID })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_intQuestaoID_intHistoricoExercicioID_2");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.intExercicioTipoId).HasDefaultValueSql("((1))");

                entity.Property(e => e.txtLetraAlternativa)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.intHistoricoExercicio)
                    .WithMany(p => p.tblCartaoResposta_objetiva)
                    .HasForeignKey(d => d.intHistoricoExercicioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCartaoResposta_objetiva_tblExercicio_Historico_2");
            });

            modelBuilder.Entity<tblCartaoResposta_objetiva_Simulado_Online>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.HasIndex(e => new { e.txtLetraAlternativa, e.intQuestaoID })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_Simulado_Online_intQuestaoID");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intQuestaoID, e.txtLetraAlternativa, e.intExercicioTipoId })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_Simulado_Online_intQuestaoID_txtLetraAlternativa_intExercicioTipoID");

                entity.HasIndex(e => new { e.intID, e.intHistoricoExercicioID, e.txtLetraAlternativa, e.intQuestaoID, e.intExercicioTipoId })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_Simulado_Online_intQuestaoID_intExercicioTipoId");

                entity.HasIndex(e => new { e.intID, e.intQuestaoID, e.intHistoricoExercicioID, e.txtLetraAlternativa, e.guidQuestao, e.dteCadastro, e.intExercicioTipoId })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_Simulado_Online_intExercicioTipoId");

                entity.HasIndex(e => new { e.intID, e.intQuestaoID, e.txtLetraAlternativa, e.guidQuestao, e.intExercicioTipoId, e.dteCadastro, e.intHistoricoExercicioID })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_Simulado_Online_intHistoricoExercicioID");

                entity.HasIndex(e => new { e.txtLetraAlternativa, e.intID, e.guidQuestao, e.intExercicioTipoId, e.dteCadastro, e.intQuestaoID, e.intHistoricoExercicioID })
                    .HasName("_net_IX_tblCartaoResposta_objetiva_Simulado_Online_intQuestaoID_intHistoricoExercicioID");

                entity.Property(e => e.dteCadastro).HasColumnType("datetime");

                entity.Property(e => e.txtLetraAlternativa)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.intHistoricoExercicio)
                    .WithMany(p => p.tblCartaoResposta_objetiva_Simulado_Online)
                    .HasForeignKey(d => d.intHistoricoExercicioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblCartaoResposta_objetiva_Simulado_Online_tblExercicio_Historico");
            });

            modelBuilder.Entity<tblEspecialidades>(entity =>
            {
                entity.HasKey(e => e.intEspecialidadeID)
                    .HasName("PK_tblEspecialidades_1");

                entity.HasIndex(e => e.CD_ESPECIALIDADE)
                    .HasName("IX_tblEspecialidades_unique")
                    .IsUnique();

                entity.Property(e => e.CD_AREA)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CD_ESPECIALIDADE)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CONCURSO)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DE_ESPECIALIDADE)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.INSCRICAO)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.bitAtivo)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<tblEspecialidadesSimulado>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.intEspecialidadeID, e.intOrdem, e.intSimuladoID })
                    .HasName("IX_tblEspecialidadesSimulado")
                    .IsUnique();

                // entity.HasOne(d => d.intEspecialidade)
                //     .WithMany(p => p.tblEspecialidadesSimulado)
                //     .HasForeignKey(d => d.intEspecialidadeID)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("FK_tblEspecialidadesSimulado_tblEspecialidades");

                // entity.HasOne(d => d.intSimulado)
                //     .WithMany(p => p.tblEspecialidadesSimulado)
                //     .HasForeignKey(d => d.intSimuladoID)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("FK_tblEspecialidadesSimulado_tblSimulado");
            });

            modelBuilder.Entity<tblExercicio_Historico>(entity =>
            {
                entity.HasKey(e => e.intHistoricoExercicioID)
                    .HasName("PK_tblExercicio_Historico_2");

                entity.HasIndex(e => e.dteDateFim)
                    .HasName("_net_IX_tblExercicio_Historico_dteDateFim_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intClientID })
                    .HasName("_net_IX_tblExercicio_Historico_intClientID_intExercicioTipo_26082019_2");

                entity.HasIndex(e => new { e.intClientID, e.intExercicioID, e.intApplicationID })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioID_intApplicationID_2");

                entity.HasIndex(e => new { e.intExercicioID, e.intExercicioTipo, e.dteDateFim })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioID_intExercicioTipo_dtefim_2");

                entity.HasIndex(e => new { e.intClientID, e.intExercicioID, e.intExercicioTipo, e.dteDateInicio })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioID_intExercicioTipo_dteDateInicio_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.dteDateFim, e.intClientID, e.intTipoProva })
                    .HasName("IX_tblExercicio_Historico_dteDateFim_intClientID_intTipoProva_2DA4D_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intClientID, e.intExercicioTipo, e.dteDateFim })
                    .HasName("_net_IX_tblExercicio_Historico_intClientID_intExercicioTipo_dteDateFim_2");

                entity.HasIndex(e => new { e.intExercicioID, e.dteDateInicio, e.dteDateFim, e.intExercicioTipo, e.intClientID })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioTipo_intClientID_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.bitRealizadoOnline, e.bitPresencial, e.intExercicioID, e.intExercicioTipo, e.intClientID, e.intVersaoID })
                    .HasName("IX_tblExercicio_Historico_intExercicioID_intExercicioTipo_intClientID_intVersaoID_F5A05_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.bitRealizadoOnline, e.bitPresencial, e.intVersaoID, e.intExercicioID, e.intExercicioTipo, e.intClientID })
                    .HasName("IX_tblExercicio_Historico_intExercicioID_intExercicioTipo_intClientID_85347_2");

                entity.HasIndex(e => new { e.dteDateFim, e.intTempoExcedido, e.intClientID, e.intApplicationID, e.intExercicioTipo, e.intHistoricoExercicioID, e.bitRanqueado, e.intExercicioID, e.dteDateInicio })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioID_dteDateInicio_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intClientID, e.intApplicationID, e.intExercicioTipo, e.bitRanqueado, e.intTempoExcedido, e.intExercicioID, e.dteDateFim, e.dteDateInicio })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioID_dteDateFim_dteDateInicio_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intExercicioTipo, e.dteDateInicio, e.dteDateFim, e.bitRanqueado, e.intTempoExcedido, e.intApplicationID, e.bitRealizadoOnline, e.bitPresencial, e.intExercicioID, e.intClientID })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioID_intClientID_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.dteDateInicio, e.dteDateFim, e.bitRanqueado, e.intTempoExcedido, e.intClientID, e.intApplicationID, e.bitPresencial, e.intVersaoID, e.intExercicioID, e.intExercicioTipo, e.bitRealizadoOnline })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioID_intExercicioTipo_bitRealizadoOnline_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.dteDateInicio, e.dteDateFim, e.bitRanqueado, e.intTempoExcedido, e.intClientID, e.intApplicationID, e.bitRealizadoOnline, e.intExercicioID, e.intExercicioTipo, e.bitPresencial, e.intVersaoID })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioID_intExercicioTipo_bitPresencial_intVersaoID_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intExercicioID, e.intExercicioTipo, e.dteDateInicio, e.dteDateFim, e.bitRanqueado, e.intTempoExcedido, e.bitRealizadoOnline, e.bitPresencial, e.intVersaoID, e.intClientID, e.intApplicationID })
                    .HasName("_net_IX_tblExercicio_Historico_intClientID_intApplicationID_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intExercicioID, e.intExercicioTipo, e.dteDateInicio, e.dteDateFim, e.bitRanqueado, e.intTempoExcedido, e.intClientID, e.bitRealizadoOnline, e.bitPresencial, e.intVersaoID, e.intApplicationID })
                    .HasName("_net_IX_tblExercicio_Historico_intApplicationID_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intExercicioTipo, e.dteDateInicio, e.dteDateFim, e.bitRanqueado, e.intTempoExcedido, e.intClientID, e.intApplicationID, e.bitPresencial, e.intVersaoID, e.intExercicioID, e.bitRealizadoOnline })
                    .HasName("_net_IX_tblExercicio_Historico_intExercicioID_bitRealizadoOnline_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intExercicioID, e.intExercicioTipo, e.dteDateInicio, e.bitRanqueado, e.intTempoExcedido, e.bitRealizadoOnline, e.bitPresencial, e.intVersaoID, e.dteDateFim, e.intClientID, e.intApplicationID, e.intTipoProva })
                    .HasName("IX_tblExercicio_Historico_dteDateFim_intClientID_intApplicationID_intTipoProva_19D50_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intExercicioID, e.intExercicioTipo, e.dteDateInicio, e.bitRanqueado, e.intTempoExcedido, e.intApplicationID, e.bitPresencial, e.intVersaoID, e.dteDateFim, e.intClientID, e.bitRealizadoOnline, e.intTipoProva })
                    .HasName("IX_tblExercicio_Historico_dteDateFim_intClientID_bitRealizadoOnline_intTipoProva_1FCF4_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intExercicioTipo, e.dteDateInicio, e.bitRanqueado, e.intTempoExcedido, e.intApplicationID, e.bitRealizadoOnline, e.bitPresencial, e.intVersaoID, e.intExercicioID, e.dteDateFim, e.intClientID, e.intTipoProva })
                    .HasName("IX_tblExercicio_Historico_intExercicioID_dteDateFim_intClientID_intTipoProva_DFE24_2");

                entity.HasIndex(e => new { e.intHistoricoExercicioID, e.intExercicioTipo, e.dteDateInicio, e.dteDateFim, e.bitRanqueado, e.intTempoExcedido, e.bitRealizadoOnline, e.bitPresencial, e.intVersaoID, e.intExercicioID, e.intClientID, e.intApplicationID, e.intTipoProva })
                    .HasName("IX_tblExercicio_Historico_intExercicioID_intClientID_intApplicationID_intTipoProva_EAD62_2");

                entity.Property(e => e.dteDateFim).HasColumnType("datetime");

                entity.Property(e => e.dteDateInicio).HasColumnType("datetime");

                entity.Property(e => e.intVersaoID).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<tblLogLeituraCartaoRespostaSimulados>(entity =>
            {
                entity.HasKey(e => e.intLogID);

                entity.HasIndex(e => new { e.intClassRoomID, e.txtSimuladoID })
                    .HasName("_net_IX_tblLogLeituraCartaoRespostaSimulados_intClassRoomID_txtSimuladoID");

                entity.HasIndex(e => new { e.intLogID, e.intEmployeeID, e.txtSimuladoID, e.txtNomeArquivo, e.bitInicioImp, e.bitFimImp, e.intQtdLidos, e.intClassRoomID, e.dteFimImportacao, e.dteDate })
                    .HasName("_net_IX_tblLogLeituraCartaoRespostaSimulados_dteDate_intLogID_intEmployeeID_txtSimuladoID");

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.dteFimImportacao).HasColumnType("datetime");

                entity.Property(e => e.txtNomeArquivo).HasMaxLength(100);

                entity.Property(e => e.txtSimuladoID)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<tblLogSimuladoAlunoTurma>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.intSimuladoID, e.intClientID })
                    .HasName("tblLogSimuladoAlunoTurma_intSimuladoID_intClientID");

                entity.Property(e => e.txtEspecialidade).HasMaxLength(200);

                entity.Property(e => e.txtTurma).HasMaxLength(300);

                entity.Property(e => e.txtUnidade).HasMaxLength(300);
            });

            modelBuilder.Entity<tblQuestaoAlternativas>(entity =>
            {
                entity.HasKey(e => new { e.intQuestaoID, e.txtLetraAlternativa });

                entity.HasIndex(e => e.intAlternativaID)
                    .HasName("_net_IX_tblQuestaoAlternativas_intAlternativaID");

                entity.HasIndex(e => new { e.txtLetraAlternativa, e.intAlternativaID })
                    .HasName("_net_IX_tblQuestaoAlternativas_txtLetraAlternativa_intAlternativaID");

                entity.HasIndex(e => new { e.intQuestaoID, e.txtLetraAlternativa, e.bitCorreta })
                    .HasName("_net_IX_tblQuestaoAlternativas_bitCorreta");

                entity.Property(e => e.txtLetraAlternativa)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.bitCorreta).HasDefaultValueSql("((0))");

                entity.Property(e => e.intAlternativaID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtAlternativa)
                    .HasMaxLength(6000)
                    .IsUnicode(false);

                entity.Property(e => e.txtResposta)
                    .HasMaxLength(6000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblQuestao_CometariosLog>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.dteDate).HasColumnType("datetime");

                entity.Property(e => e.txtAlternativa).HasMaxLength(4000);

                entity.Property(e => e.txtComentario).HasColumnType("text");

                entity.Property(e => e.txtEnunciado).HasMaxLength(4000);

                entity.HasOne(d => d.intQuestao)
                    .WithMany(p => p.tblQuestao_CometariosLog)
                    .HasForeignKey(d => d.intQuestaoID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_tblQuestao_CometariosLog_tblQuestoes");
            });

            modelBuilder.Entity<tblQuestao_Imagem>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.imgImagem).HasColumnType("image");

                entity.Property(e => e.imgImagemOtimizada).HasColumnType("image");

                entity.Property(e => e.txtLabel).HasMaxLength(200);

                entity.Property(e => e.txtName).HasMaxLength(200);
            });

            modelBuilder.Entity<tblQuestao_Marcacao>(entity =>
            {
                entity.HasKey(e => e.intID);

                entity.Property(e => e.dtAnotacao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.txtAnotacao).HasColumnType("text");
            });

            modelBuilder.Entity<tblQuestao_Simulado>(entity =>
            {
                entity.HasKey(e => new { e.intQuestaoID, e.intSimuladoID });

                entity.HasIndex(e => new { e.intQuestaoID, e.txtCodigoCorrecao, e.bitAnulada, e.intSimuladoID })
                    .HasName("_net_IX_tblQuestao_Simulado_intSimuladoI");

                entity.Property(e => e.txtCodigoCorrecao).HasMaxLength(50);

                entity.HasOne(d => d.intQuestao)
                    .WithMany(p => p.tblQuestao_Simulado)
                    .HasForeignKey(d => d.intQuestaoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblQuestao_Simulado_tblQuestoes");

                entity.HasOne(d => d.intSimulado)
                    .WithMany(p => p.tblQuestao_Simulado)
                    .HasForeignKey(d => d.intSimuladoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblQuestao_Simulado_tblSimulado1");
            });

            modelBuilder.Entity<tblQuestoes>(entity =>
            {
                entity.HasKey(e => e.intQuestaoID)
                    .HasName("PK_tblQuestions");

                entity.HasIndex(e => new { e.intQuestaoID, e.intEspecialidadeID })
                    .HasName("_net_IX_tblQuestoes_intEspecialidadeID");

                entity.HasIndex(e => new { e.intQuestaoID, e.bitAnulada, e.bitCasoClinico })
                    .HasName("_net_IX_tblQuestoes_bitCasoClinico");

                entity.HasIndex(e => new { e.intQuestaoID, e.bitCasoClinico, e.bitAnulada })
                    .HasName("_net_IX_tblQuestoes_bitAnulada");

                entity.Property(e => e.ID_CONCURSO).HasDefaultValueSql("((-1))");

                entity.Property(e => e.bitCasoClinico)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.bitConceitual)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.dteFilmagem).HasColumnType("datetime");

                entity.Property(e => e.dteLimite).HasColumnType("datetime");

                entity.Property(e => e.dteQuestao).HasColumnType("datetime");

                entity.Property(e => e.guidQuestaoID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.txtEnunciado)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.txtFonteTipo)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtObservacao).HasMaxLength(4000);

                entity.Property(e => e.txtOrigem).HasMaxLength(50);

                entity.Property(e => e.txtRecurso).HasColumnType("text");
            });

            modelBuilder.Entity<tblQuestoesSimuladoImagem_Comentario>(entity =>
            {
                entity.HasKey(e => e.intImagemComentarioID)
                    .HasName("PK__tblQuest__EAFCDFFDF5BC1C44");

                entity.Property(e => e.imgImagem)
                    .IsRequired()
                    .HasColumnType("image");

                entity.Property(e => e.txtLabel).HasMaxLength(50);
            });

            modelBuilder.Entity<tblSimulado>(entity =>
            {
                entity.HasKey(e => e.intSimuladoID);

                entity.Property(e => e.CD_ESPECIALIDADE)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.dteDataHoraInicioWEB).HasColumnType("datetime");

                entity.Property(e => e.dteDataHoraTerminoWEB).HasColumnType("datetime");

                entity.Property(e => e.dteDateFim).HasColumnType("datetime");

                entity.Property(e => e.dteDateInicio).HasColumnType("datetime");

                entity.Property(e => e.dteDateTimeLastUpdate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.dteInicioConsultaRanking).HasColumnType("datetime");

                entity.Property(e => e.dteLimiteParaRanking).HasColumnType("datetime");

                entity.Property(e => e.dteReleaseComentario).HasColumnType("datetime");

                entity.Property(e => e.dteReleaseGabarito).HasColumnType("datetime");

                entity.Property(e => e.dteReleaseSimuladoWeb).HasColumnType("datetime");

                entity.Property(e => e.guidSimuladoID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.txtCodQuestoes)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.txtOrigem)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("('M')");

                entity.Property(e => e.txtPathGabarito)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtSimuladoDescription).HasMaxLength(200);

                entity.Property(e => e.txtSimuladoName).HasMaxLength(50);
            });

            modelBuilder.Entity<tblSimuladoConcurso_Excecao>(entity =>
            {
                entity.HasKey(e => e.intSimuladoConcursoID);
            });

            modelBuilder.Entity<tblSimuladoCorrecaoCasoClinico>(entity =>
            {
                entity.HasKey(e => e.intId);

                entity.Property(e => e.dteDistribuicao).HasColumnType("datetime");

                entity.Property(e => e.dtePrazo).HasColumnType("datetime");

                entity.Property(e => e.txtCoordCorrecao)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.txtObsRevisao)
                    .HasMaxLength(400)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblSimuladoImportacaoCartaoResposta>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.txtClientID, e.txtSimuladoID, e.txtVersaoID, e.txtResposta, e.intLeituraID })
                    .HasName("_net_IX_tblSimuladoImportacaoCartaoResposta_02");

                entity.HasIndex(e => new { e.intLeituraID, e.txtClientID, e.txtColVazia, e.txtVersaoID, e.txtResposta, e.txtSimuladoID })
                    .HasName("_net_IX_tblSimuladoImportacaoCartaoResposta_txtSimuladoID_intLeituraID_txtClientID_txtColVazia_txtVersaoID_txtResposta");

                entity.HasIndex(e => new { e.intLeituraID, e.txtColVazia, e.txtSimuladoID, e.txtVersaoID, e.txtClientID, e.txtResposta })
                    .HasName("_net_ix_01");

                entity.HasIndex(e => new { e.intLeituraID, e.txtClientID, e.txtSimuladoID, e.txtColVazia, e.txtVersaoID, e.txtResposta, e.intArquivoID })
                    .HasName("_net_IX_tblSimuladoImportacaoCartaoResposta_01");

                entity.Property(e => e.intLeituraID).ValueGeneratedOnAdd();

                entity.Property(e => e.txtClientID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtColVazia)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtResposta).HasMaxLength(100);

                entity.Property(e => e.txtSimuladoID)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtTurma).HasMaxLength(200);

                entity.Property(e => e.txtVersaoID)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                // entity.HasOne(d => d.intArquivo)
                //     .WithMany(p => p.tblSimuladoImportacaoCartaoResposta)
                //     .HasForeignKey(d => d.intArquivoID)
                //     .HasConstraintName("FK_tblSimuladoImportacaoCartaoResposta_tblLogLeituraCartaoRespostaSimulados");
            });

            modelBuilder.Entity<tblSimuladoOnline_Consolidado>(entity =>
            {
                entity.HasKey(e => e.intID)
                    .HasName("PK__tblSimul__11B67932C29F2105");

                entity.Property(e => e.dteDataCriacao).HasColumnType("datetime");

                entity.HasOne(d => d.intSimulado)
                    .WithMany(p => p.tblSimuladoOnline_Consolidado)
                    .HasForeignKey(d => d.intSimuladoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblSimula__intSi__1F2E9E6D");
            });

            modelBuilder.Entity<tblSimuladoOnline_Excecao>(entity =>
            {
                entity.HasKey(e => e.intSimuladoExcecaoID)
                    .HasName("PK_tblSimuladoOnlineExcecao");

                entity.Property(e => e.dteDataHoraInicioWEB).HasColumnType("datetime");

                entity.Property(e => e.dteDataHoraTerminoWEB).HasColumnType("datetime");

                entity.Property(e => e.dteInicioConsultaRanking).HasColumnType("datetime");

                entity.Property(e => e.intClientID).HasDefaultValueSql("((-1))");

                entity.Property(e => e.intCourseID).HasDefaultValueSql("((-1))");
            });

            modelBuilder.Entity<tblSimuladoOrdenacao>(entity =>
            {
                entity.HasKey(e => new { e.intSimuladoID, e.intQuestaoID });

                entity.HasOne(d => d._int)
                    .WithMany(p => p.tblSimuladoOrdenacao)
                    .HasForeignKey(d => new { d.intQuestaoID, d.intSimuladoID })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSimuladoOrdenacao_tblQuestao_Simulado");
            });

            modelBuilder.Entity<tblSimuladoRanking_Fase01>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.txtPosicao, e.intAcertos, e.dblNotaFinal, e.intSimuladoID, e.intStateID })
                    .HasName("IX_tblSimuladoRanking_Fase01_intSimuladoID_intStateID_6B9D5");

                entity.HasIndex(e => new { e.txtPosicao, e.intAcertos, e.dblNotaFinal, e.txtEspecialidade, e.intSimuladoID, e.intStateID })
                    .HasName("IX_tblSimuladoRanking_Fase01_intSimuladoID_intStateID_81436");

                entity.HasIndex(e => new { e.txtPosicao, e.intAcertos, e.dblNotaProvaDiscursiva, e.dblNotaObjetiva, e.dblNotaDiscursiva, e.dblNotaFinal, e.intClientID, e.txtUnidade, e.txtLocal, e.txtName, e.txtEspecialidade, e.intStateID, e.intSimuladoID })
                    .HasName("IX_tblSimuladoRanking_Fase01_intSimuladoID_C7067");

                entity.Property(e => e.txtEspecialidade)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtLocal).HasMaxLength(150);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtPosicao).HasMaxLength(6);

                entity.Property(e => e.txtUnidade)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                // entity.HasOne(d => d.intArquivo)
                //     .WithMany(p => p.tblSimuladoRanking_Fase01)
                //     .HasForeignKey(d => d.intArquivoID)
                //     .HasConstraintName("FK_tblSimuladoRanking_Fase01_tblLogLeituraCartaoRespostaSimulados");
            });

            modelBuilder.Entity<tblSimuladoRanking_Fase02>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.txtPosicao, e.intAcertos, e.dblNotaProvaDiscursiva, e.dblNotaObjetiva, e.dblNotaDiscursiva, e.dblNotaFinal, e.intClientID, e.txtUnidade, e.txtLocal, e.txtName, e.txtEspecialidade, e.intStateID, e.intSimuladoID })
                    .HasName("IX_tblSimuladoRanking_Fase02_intSimuladoID_E71D9");

                entity.Property(e => e.txtEspecialidade)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.txtLocal).HasMaxLength(150);

                entity.Property(e => e.txtName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.txtPosicao).HasMaxLength(6);

                entity.Property(e => e.txtUnidade)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tblSimuladoRespostas>(entity =>
            {
                entity.HasKey(e => new { e.intClientID, e.intSimuladoID, e.intQuestao });

                entity.HasIndex(e => e.intArquivoID)
                    .HasName("_net_IX_tblSimuladoRespostas_intArquivoID");

                entity.HasIndex(e => new { e.intClientID, e.intArquivoID })
                    .HasName("_net_IX_tblSimuladoRespostas_intClientID");

                entity.HasIndex(e => new { e.intSimuladoID, e.intVersaoID, e.intQuestao, e.txtLetraResposta })
                    .HasName("_net_IX_tblSimuladoRespostas_txtLetraResposta");

                entity.HasIndex(e => new { e.intClientID, e.intSimuladoID, e.intVersaoID, e.intQuestao, e.txtLetraResposta })
                    .HasName("IX_tblSimuladoRespostas");

                entity.HasIndex(e => new { e.intClientID, e.intArquivoID, e.intSimuladoID, e.intVersaoID, e.intQuestao, e.txtLetraResposta })
                    .HasName("_net_IX_tblSimuladoRespostas_intSimuladoID_intVersaoID");

                entity.Property(e => e.txtLetraResposta)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.intArquivo)
                    .WithMany(p => p.tblSimuladoRespostas)
                    .HasForeignKey(d => d.intArquivoID)
                    .HasConstraintName("FK_tblSimuladoRespostas_tblLogLeituraCartaoRespostaSimulados");

                entity.HasOne(d => d.intSimulado)
                    .WithMany(p => p.tblSimuladoRespostas)
                    .HasForeignKey(d => d.intSimuladoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSimuladoRespostas_tblSimulado");
            });

            modelBuilder.Entity<tblSimuladoResultados>(entity =>
            {
                entity.HasKey(e => new { e.intClientID, e.intSimuladoID, e.intVersaoID });

                entity.HasIndex(e => e.intClientID)
                    .HasName("IX_tblSimuladoResultadosCliente");

                entity.HasIndex(e => new { e.intClientID, e.intAcertos, e.intArquivoID, e.intSimuladoID })
                    .HasName("_net_IX_tblSimuladoResultados_intSimuladoID");

                entity.HasOne(d => d.intArquivo)
                    .WithMany(p => p.tblSimuladoResultados)
                    .HasForeignKey(d => d.intArquivoID)
                    .HasConstraintName("FK_tblSimuladoResultados_tblLogLeituraCartaoRespostaSimulados");

                entity.HasOne(d => d.intSimulado)
                    .WithMany(p => p.tblSimuladoResultados)
                    .HasForeignKey(d => d.intSimuladoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSimuladoResultados_tblSimulado");
            });

            modelBuilder.Entity<tblSimuladoResultadosDiscursivas>(entity =>
            {
                entity.HasKey(e => new { e.intSimuladoID, e.intClientID });

                entity.Property(e => e.dteDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.intSimulado)
                    .WithMany(p => p.tblSimuladoResultadosDiscursivas)
                    .HasForeignKey(d => d.intSimuladoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSimuladoResultadosDiscursivas_tblSimulado");
            });

            modelBuilder.Entity<tblSimuladoVersao>(entity =>
            {
                entity.HasKey(e => new { e.intSimuladoID, e.intQuestaoID, e.intVersaoID });

                entity.HasIndex(e => new { e.intQuestao, e.intQuestaoID, e.intVersaoID })
                    .HasName("_net_IX_tblSimuladoVersao_intQuestaoID_intVersaoID");

                entity.HasIndex(e => new { e.intSimuladoID, e.intQuestaoID, e.intVersaoID, e.intQuestao })
                    .HasName("_net_IX_tblSimuladoVersao_intVersaoID_intQuestao");

                entity.HasOne(d => d._int)
                    .WithMany(p => p.tblSimuladoVersao)
                    .HasForeignKey(d => new { d.intQuestaoID, d.intSimuladoID })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSimuladoVersao_tblQuestao_Simulado");
            });

            modelBuilder.Entity<tblVideo>(entity =>
            {
                entity.HasKey(e => e.intVideoID);

                entity.HasIndex(e => e.guidVideoID)
                    .HasName("IX_tblVideo_guidVideoID_7A0BD");

                entity.HasIndex(e => e.intVimeoID)
                    .HasName("IX_tblVideo_intVimeoID_60177");

                entity.Property(e => e.bitActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.dteCreationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.dteLastModifyDate).HasColumnType("smalldatetime");

                entity.Property(e => e.guidVideoID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.intDuracao)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(300)
                    .IsFixedLength();

                entity.Property(e => e.txtFileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtName).IsRequired();

                entity.Property(e => e.txtPath).IsRequired();

                entity.Property(e => e.txtSubject)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.txtUploadSource)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.txtUrlStreamVimeo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrlThumbVimeo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrlVimeo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtVideoInfo).IsUnicode(false);
            });

            modelBuilder.Entity<tblVideo_Questao_Simulado>(entity =>
            {
                entity.HasKey(e => e.intQuestaoID);

                entity.Property(e => e.intQuestaoID).ValueGeneratedNever();

                entity.Property(e => e.dteCreationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.dteLastModifyDate).HasColumnType("smalldatetime");

                entity.HasOne(d => d.intQuestao)
                    .WithOne(p => p.tblVideo_Questao_Simulado)
                    .HasForeignKey<tblVideo_Questao_Simulado>(d => d.intQuestaoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblVideo_Questao_Simulado_tblVideo_Questao_Simulado");

                entity.HasOne(d => d.intVideo)
                    .WithMany(p => p.tblVideo_Questao_Simulado)
                    .HasForeignKey(d => d.intVideoID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblVideo_Questao_Simulado_tblVideo");
            });

            modelBuilder.Entity<tblVideo_dsv>(entity =>
            {
                entity.HasKey(e => e.intVideoID);

                entity.Property(e => e.intVideoID).ValueGeneratedNever();

                entity.Property(e => e.bitActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.dteCreationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.dteLastModifyDate).HasColumnType("smalldatetime");

                entity.Property(e => e.guidVideoID).HasDefaultValueSql("(newid())");

                entity.Property(e => e.intDuracao)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.txtDescription)
                    .HasMaxLength(300)
                    .IsFixedLength();

                entity.Property(e => e.txtFileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtName).IsRequired();

                entity.Property(e => e.txtPath).IsRequired();

                entity.Property(e => e.txtSubject)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.txtUploadSource)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.txtUrlStreamVimeo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrlThumbVimeo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.txtUrlVimeo)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
