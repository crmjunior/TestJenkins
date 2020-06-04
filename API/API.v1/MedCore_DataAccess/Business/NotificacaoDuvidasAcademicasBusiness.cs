using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Business
{
    public class NotificacaoDuvidasAcademicasBusiness : INotificacaoDuvidasAcademicasBusiness
    {
        private readonly INotificacaoDuvidasAcademicasData _rep;

        public NotificacaoDuvidasAcademicasBusiness(INotificacaoDuvidasAcademicasData rep)
        {
            _rep = rep;
        }

        public List<NotificacaoDuvidaAcademica> GetNotificacoesDuvidaPorAluno(int duvidaId, int clientId, int categoria = 0)
        {
            return _rep.GetNotificacoesDuvidaPorAluno(duvidaId, clientId, categoria);
        }

        public int SetNotificacaoLida(int DuvidaId, int ClientId, int Categoria)
        {
            var notificacoes = _rep.GetNotificacoesDuvidaPorAluno(DuvidaId, ClientId, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.Indefinido);
            foreach(var item in notificacoes)
            {
                _rep.SetNotificacaoDuvidaAcademicaLida(item);
            }

            return 1;
        }

        public int SetNotificacaoAtiva(int DuvidaId, int ClientId, int Categoria)
        {
            var notificacoes = _rep.GetNotificacoesDuvidaPorAluno(DuvidaId, ClientId, Categoria);
            foreach (var item in notificacoes)
            {
                _rep.SetNotificacaoDuvidaAcademicaAtiva(item);
            }

            return 1;
        }

        public void SetNotificacao(DuvidaAcademicaContract duvida, List<int> alunosFavoritaram, List<int> alunosInteragiram, EnumTipoNotificacaoDuvidasAcademicas tipo, int? clientResposta)
        {
            switch(tipo)
            {
                case EnumTipoNotificacaoDuvidasAcademicas.RespostaMedGrupo:
                    InserirNotificacaoRespondida(duvida, EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaMedgrupo);
                    InserirNotificacaoFavoritas(duvida, alunosFavoritaram, EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaMedgrupo);
                    InserirNotificacaoInteracoes(duvida, alunosInteragiram, EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaRespostaMedGrupo);
                    break;
                case EnumTipoNotificacaoDuvidasAcademicas.Homologada:
                    InserirNotificacaoRespondida(duvida, EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaHomologada);
                    InserirNotificacaoFavoritas(duvida, alunosFavoritaram, EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaHomologada);
                    InserirNotificacaoInteracoes(duvida, alunosInteragiram, EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaHomologada);
                    break;
                case EnumTipoNotificacaoDuvidasAcademicas.Replica:
                    InserirNotificacaoRespondida(duvida, EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaDuvida);
                    InserirNotificacaoRespondida(duvida, EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaResposta, clientResposta);
                    break;
                case EnumTipoNotificacaoDuvidasAcademicas.Resposta:
                    InserirNotificacaoRespondida(duvida, EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespondida);
                    InserirNotificacaoFavoritas(duvida, alunosFavoritaram, EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespondida);
                    break;
            }
        }

        //Notificação para o dono da duvida
        private void InserirNotificacaoRespondida(DuvidaAcademicaContract duvida, EnumTipoMensagemNotificacaoDuvidasAcademicas tipo, int? respostaClientId = null)
        {
            var AlunoReplicouPropriaResposta = respostaClientId == 0;

            SetNotificacaoAtiva(duvida.DuvidaId.Value, duvida.ClientId, (int)tipo);
            if(!AlunoReplicouPropriaResposta)
                SetNotificacaoDuvidaAcademica(duvida, duvida.ClientId, tipo, respostaClientId);
        }

        //Notificação para quem favoritou
        private void InserirNotificacaoFavoritas(DuvidaAcademicaContract duvida, List<int> clientIds, EnumTipoMensagemNotificacaoDuvidasAcademicas tipo)
        {
            foreach (var id in clientIds)
            {  
                var PossuiDuvidaRespostaHomologada = _rep.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, id, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaHomologada).Any();
                var PossuiDuvidaRespostaRespondidaMedGrupo = _rep.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, id, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaMedgrupo).Any();
                if(!PossuiDuvidaRespostaHomologada && !PossuiDuvidaRespostaRespondidaMedGrupo)
                {
                    SetNotificacaoAtiva(duvida.DuvidaId.Value, id, (int)tipo);
                    SetNotificacaoDuvidaAcademica(duvida, id, tipo);
                }
            }
        }

        //Notificação para quem interagiu (Resposta/Replica)
        private void InserirNotificacaoInteracoes(DuvidaAcademicaContract duvida, List<int> clientIds, EnumTipoMensagemNotificacaoDuvidasAcademicas tipo)
        {
            foreach (var id in clientIds)
            {
                var PossuiNotificacaoDeDuvidaHomologada = _rep.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, id, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaHomologada).Any();
                var PossuiNotificacaoDeDevidaRespondidaMedGrupo = _rep.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, id, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaMedgrupo).Any();
                var PosuiDuvidasFavoritdasHomologada = _rep.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, id, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaHomologada).Any();
                var PossuiDuvidasFavoritadasRespondidaMedGrupo = _rep.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, id, (int)EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaMedgrupo).Any();
                if (!PossuiNotificacaoDeDuvidaHomologada && !PossuiNotificacaoDeDevidaRespondidaMedGrupo && !PosuiDuvidasFavoritdasHomologada && !PossuiDuvidasFavoritadasRespondidaMedGrupo)
                {
                    SetNotificacaoAtiva(duvida.DuvidaId.Value, id, (int)tipo);
                    SetNotificacaoDuvidaAcademica(duvida, id, tipo);
                }
            }
        }

        private int SetNotificacaoDuvidaAcademica(DuvidaAcademicaContract duvida, int clientId, EnumTipoMensagemNotificacaoDuvidasAcademicas tipo, int? respostaClientId = null)
        {
            var descricao = GetDuvidasAcademicasMensagens(duvida, tipo);
            var notificacoesExistentes = _rep.GetNotificacoesDuvidaPorAluno(duvida.DuvidaId.Value, duvida.ClientId, (int)tipo);

            var hasNotificacaoEnviada = notificacoesExistentes.Any(x => x.Status == EnumStatusNotificacao.Enviado);
            var id = tipo == EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaResposta ? respostaClientId.Value : duvida.DuvidaId.Value;

            var notificacaoDuvida = new NotificacaoDuvidaAcademica()
            {
                NotificacaoId = Utilidades.NovasInteracoesDuvidasAcademicas,
                DuvidaId = duvida.DuvidaId.Value,
                Status = hasNotificacaoEnviada ? EnumStatusNotificacao.Enviado : EnumStatusNotificacao.NaoEnviado,
                ClientId = respostaClientId != null ? respostaClientId.Value : clientId,
                Descricao = descricao,
                TipoCategoria = tipo
            };

            var result = _rep.SetNotificacaoDuvidaAcademica(notificacaoDuvida);
            return result;
        }

        private string GetDuvidasAcademicasMensagens(DuvidaAcademicaContract duvida, EnumTipoMensagemNotificacaoDuvidasAcademicas tipo)
        {
            var descricao = string.Empty;
            if(duvida.Origem != null && duvida.OrigemSubnivel != null)
            {
                descricao = "em \"" + duvida.Origem + " - " + duvida.OrigemSubnivel + "\"";
            }            

            switch (tipo)
            {
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespondida:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.DuvidaResposta, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaHomologada:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.DuvidaRespostaHomologada, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaRespostaMedgrupo:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.DuvidaRespostaMedgrupo, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespondida:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.DuvidaFavoritadaRespondida, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaHomologada:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.DuvidaFavoritadaRespostaHomologada, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespostaMedgrupo:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.DuvidaFavoritadaRespostaMedgrupo, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.RespostaHomologadaMedGrupo:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.RespostaHomologadaMedGrupo, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.NovaReplica:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.ReplicaResposta, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaRespostaMedGrupo:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.InteracaoDuvidaRespostaMedGrupo, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.InteracaoDuvidaHomologada:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.InteracaoDuvidaHomologada, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaDuvida:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.ReplicaDuvida, descricao);
                    break;
                case EnumTipoMensagemNotificacaoDuvidasAcademicas.ReplicaResposta:
                    descricao = string.Format(Mensagens.NotificacaoMensagens.ReplicaResposta, descricao);
                    break;
            }

            return descricao;
        }

        public int DeleteNotificacoesAluno(int matricula)
        {
            return _rep.DeleteNotificacoesAluno(matricula);
        }
    }
}