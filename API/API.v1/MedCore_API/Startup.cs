using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using MedCoreAPI.Util.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using StackExchange.Profiling;
using Microsoft.OpenApi.Models;
using AutoMapper;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCoreAPI.ViewModel;
using MedCoreAPI.ViewModel.Base;
using System.Collections.Generic;
using MedCore_DataAccess.ViewModels;
using MedCore_API.ViewModel.Base;
using System;
using MedCore_API.Util;

namespace MedCore_DataAccess
{
    public class Startup
    {
        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container. teste
        // Se alterar o prefixo da rota provavelmente será necessário alterar o arquivo RoutePrefixConvention.cs
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt => {
                opt.AddDefaultPolicy(bldr => {
                    bldr.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddMiniProfiler(opt => { 
                opt.RouteBasePath = "/profiler";
                opt.ExcludeMethod("Utilidades/GetVersao");
            }).AddEntityFramework();
            services.AddMemoryCache();
            services.AddAuthorization();


            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Contribuicao, ContribuicaoDTO>();
                cfg.CreateMap<ContribuicaoDTO, ContribuicaoViewModel>();
                cfg.CreateMap<ContribuicaoArquivo, ContribuicaoArquivoViewModel>();
                cfg.CreateMap<DuvidasAcademicasProfessorViewModel, DuvidasAcademicasProfessorDTO>();
                cfg.CreateMap<DuvidasAcademicasProfessorDTO, DuvidasAcademicasProfessorViewModel>();
                cfg.CreateMap<VideoDTO, VideoViewModel>();
                cfg.CreateMap<VideoQualidadeDTO, VideoQualidadeViewModel>();
                cfg.CreateMap<ContribuicaoBucketDTO, ContribuicaoBucketViewModel>();
                cfg.CreateMap<MaterialApostilaDTO, MaterialApostilaConteudoViewModel>();
                cfg.CreateMap<PreLoginViewModel, AlunoMedsoft>();
                cfg.CreateMap<AlunoMedsoft, PreLoginViewModel>();
                cfg.CreateMap<LoginViewModel, AlunoMedsoft>();
                cfg.CreateMap<AlunoMedsoft, LoginViewModel>();
                cfg.CreateMap<CartaoRespostaSimuladoAgendadoDTO, CartaoRespostaSimuladoAgendadoViewModel>();
                cfg.CreateMap<QuestaoSimuladoAgendadoDTO,QuestaoSimuladoAgendadoCartaoRespostaViewModel>();
                cfg.CreateMap<Media,MediaViewModel>();
                cfg.CreateMap<NotificacoesClassificadasDTO, NotificacoesClassificacaoViewModel>();
                cfg.CreateMap<NotificacaoClassificacaoDTO, NotificacaoClassificacaoViewModel>(); 
                cfg.CreateMap<NotificacaoDTO, NotificacaoViewModel>();
                cfg.CreateMap<NotificacaoInfoAdicional ,NotificacaoInfoAdicionalViewModel>();
                cfg.CreateMap<AulaAvaliacaoDTO, AulaAvaliacaoViewModel>();
                cfg.CreateMap<AulaAvaliacaoTemaDTO, AulaAvaliacaoTemaViewModel>();
                cfg.CreateMap<AulaAvaliacaoSlideDTO, AulaAvaliacaoSlideViewModel>();
                cfg.CreateMap<AulaAvaliacaoConteudoDTO, AulaAvaliacaoConteudoViewModel>();
                cfg.CreateMap<QuestaoSimuladoAgendadoDTO, QuestaoSimuladoAgendadoViewModel>();
                cfg.CreateMap<Media, MediaViewModel>();
                cfg.CreateMap<EspecialidadeDTO, EspecialidadeViewModel>();
                cfg.CreateMap<AlternativaSimualdoAgendadoDTO, AlternativaSimualdoAgendadoViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton(Configuration);

            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<AppSettings>(Configuration.GetSection("Settings"));

            services.AddDbContext<DesenvContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DesenvConnection")));
            services.AddScoped<DbContext, DesenvContext>();

            services.AddApiVersioning(o => 
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddControllers(o => 
            {
                o.UseGeneralRoutePrefix("MsCross.svc/json");
            }).AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MED CORE API", Version = "v 1.0"});
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var enableProfile = Convert.ToBoolean(Util.ConfigurationProvider.Get("Settings:EnableProfile"));
            if (env.IsDevelopment() || enableProfile)
            {
                app.UseDeveloperExceptionPage();
                app.UseMiniProfiler();
            }
            
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(
                options => options.SetIsOriginAllowed(x => _ = true)
                                    .AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());

            app.UseAuthorization();
            app.UseRequestCounter();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
