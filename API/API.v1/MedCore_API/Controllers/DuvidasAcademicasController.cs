using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MedCoreAPI.ViewModel;
using MedCoreAPI.ViewModel.Base;
using System;
using MedCore_DataAccess.DTO.DuvidaAcademica;
using Microsoft.AspNetCore.Cors;

namespace MedCoreAPI.Controllers
{
    [ApiController]
    [ApiVersion("2")]
    [ApiVersionNeutral]
    [EnableCors]
    public class DuvidasAcademicasController : BaseService
    {
        public DuvidasAcademicasController(IMapper mapper) 
            : base(mapper) {

        }

        [HttpPost]
        [Route("DuvidasAcademicas/Duvida/GetDuvidas")]
        public IList<DuvidaAcademicaContract> GetDuvidas(DuvidaAcademicaFiltro filtro)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).GetDuvidas(filtro);
        }


        [HttpGet]
        [MapToApiVersion("2")]
        [Route("DuvidasAcademicas/Duvida/GetDuvidasProfessor/{idProfessor}")]
        public ResultViewModel<IList<DuvidasAcademicasProfessorViewModel>> GetDuvidasProfessor(string idProfessor)
        {
            var result = Execute(() =>
            {//
                var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity()));
                var contribuicoes = business.GetDuvidasProfessor(Convert.ToInt32(idProfessor));
                return contribuicoes;
            }, true);
            return GetResultViewModel<IList<DuvidasAcademicasProfessorViewModel>, IList<DuvidasAcademicasProfessorDTO>>(result);
        }

        [HttpGet]
        [Route("ConcursosProva/Matricula/{matricula}/IdAplicacao/{idaplicacao}")]
        public List<ConcursoDTO> GetConcursoProva(string matricula, string idaplicacao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).GetConcursoProva(Convert.ToInt32(matricula), Convert.ToInt32(idaplicacao));
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Respostas/GetRespostas/")]
        public IList<DuvidaAcademicaContract> GetRespostasPorDuvida(DuvidaAcademicaFiltro filtro)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).GetRespostasPorDuvida(filtro);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Replicas/GetReplicasPorResposta/")]
        public DuvidaAcademicaReplicaResponse GetReplicasPorResposta(DuvidaAcademicaFiltro filtro)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).GetReplicasResposta(filtro);
        }

        [HttpGet]
        [Route("DuvidasAcademicas/Duvida/EnviarEmail/")]
        public void EnviarEmailDuvidas()
        {
            new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).EnviarEmails();
        }

        [HttpGet]
        [Route("DuvidasAcademicas/Duvida/GetTrechoApostila/")]
        public string GetTrechoApostilaSelecionado(int duvida)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).GetTrechoApostilaSelecionado(duvida);
        }

        [HttpGet]
        [Route("CronogramaSimplificado/Produto/{produto}/Matricula/{matricula}/IsQuestao/{isQuestao}")]
        public List<CronogramaSimplificadoDTO> GetCronogramaSimplificado(string produto, string matricula, string isQuestao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).GetCronogramaSimplificado(Convert.ToInt32(produto), Convert.ToInt32(matricula), Convert.ToBoolean(isQuestao));
        }

        [HttpGet]
        [Route("ProfessoresPorRepresentante/{idEmployee}")]
        public List<PessoaGrupoDTO> GetProfessoresPorRepresentante(string idEmployee)
        {
            return new GrupoPessoaBusiness(new GrupoPessoaEntity()).GetPessoasGrupoPorRepresentante(Convert.ToInt32(idEmployee));
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Duvida/Insert")]
        public int InsertDuvida(DuvidaAcademicaInteracao interacao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).InsertDuvida(interacao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Resposta/Insert")]
        public DuvidaAcademicaContract InsertResposta(DuvidaAcademicaInteracao interacao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).InsertResposta(interacao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/ObservacaoMedGrupo/Insert")]
        public DuvidaAcademicaContract InsertObservacaoMedGrupo(DuvidaAcademicaInteracao interacao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).InsertObservacaoMedGrupo(interacao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Replica/Insert")]
        public DuvidaAcademicaContract InsertReplica(DuvidaAcademicaInteracao interacao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).InsertReplica(interacao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Interacao/Insert")]
        public int InsertInteracao(DuvidaAcademicaInteracao interacao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).InsertInteracao(interacao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Encaminhar/Insert")]
        public int InsertDuvidasEncaminhadas(DuvidaAcademicaInteracao duvidaInteracao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).InsertDuvidasEncaminhadas(duvidaInteracao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Resposta/Homologar")]
        public bool SetRespostaHomologada(DuvidaAcademicaInteracao duvidaInteracao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).SetRespostaHomologada(duvidaInteracao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Duvida/Arquivar")]
        public int SetDuvidaArquivada(DuvidaAcademicaInteracao duvidaInteracao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).SetDuvidaArquivada(duvidaInteracao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Duvida/Delete")]
        public int DeleteDuvida(DuvidaAcademicaInteracao duvidaAcademicaInteracao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).DeleteDuvida(duvidaAcademicaInteracao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/Duvida/DeletePorMarcacao")]
        public int DeleteDuvidaApostilaPorMarcacao(DuvidaAcademicaInteracao interacao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).DeleteDuvidaApostilaPorMarcacao(interacao);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/InsertDuvidaLida")]
        public int InsertDuvidaLida(DuvidaAcademicaInteracao duvidaLida)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).InsertDuvidaLida(duvidaLida);
        }

        [HttpPost]
        [Route("DuvidasAcademicas/RespostaReplica/Delete")]
        public int DeleteRespostaReplica(DuvidaAcademicaInteracao duvidaAcademicaInteracao)
        {
            return new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity())).DeleteRespostaReplica(duvidaAcademicaInteracao);
        }

        [HttpPost]
        [MapToApiVersion("2")]
        [Route("DuvidasAcademicas/SetDenuncia")]
        public ResultViewModel<bool> SetDenuncia(DenunciaDuvidasAcademicasDTO obj)
        {
            var result = Execute(() =>
            {
                var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity()));
                var retorno = business.SetDenuncia(obj);
                return retorno;
            });
            return GetResultViewModel(result);
        }

        [HttpPost]
        [MapToApiVersion("2")]
        [Route("DuvidasAcademicas/SetRespostaReplicaPrivada")]
        public ResultViewModel<bool> SetRespostaReplicaPrivada(DuvidasRespostaPrivadaDTO obj)
        {
            var result = Execute(() =>
            {
                var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity()));
                var resposta = business.SetRespostaReplicaPrivada(obj);
                return resposta;
            });
            return GetResultViewModel(result);
        }

        [HttpPost]
        [MapToApiVersion("2")]
        [Route("DuvidasAcademicas/SetDuvidaAcademicaPrivada")]
        public ResultViewModel<bool> SetDuvidaAcademicaPrivada(DuvidasRespostaPrivadaDTO obj)
        {
            var result = Execute(() =>
            {
                var business = new DuvidasAcademicasBusiness(new DuvidasAcademicasEntity(), new FuncionarioEntity(), new MaterialApostilaEntity(), new ConcursoEntity(), new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity()));
                var resposta = business.SetDuvidaAcademicaPrivada(obj);
                return resposta;
            });
            return GetResultViewModel(result);
        }

        [HttpPost]
        [Route("NotificacaoDuvida/Lida")]
        public int SetNotificacaoDuvidasAcademicasLida(int duvidaId, int clientId, int categoria)
        {
            return new NotificacaoDuvidasAcademicasBusiness(new NotificacaoDuvidasAcademicasEntity()).SetNotificacaoLida(duvidaId, clientId, categoria);
        }
    }
}