using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Notificacao", Namespace = "")]
    public class NotificacaoDuvidaAcademica
    {
        [DataMember(Name = "NotificacaoDuvidaId")]
        public int NotificacaoDuvidaId { get; set; }

        [DataMember(Name = "NotificacaoId")]
        public int NotificacaoId { get; set; }

        [DataMember(Name = "DuvidaId")]
        public int DuvidaId { get; set; }

        [DataMember(Name = "ClientId")]
        public int ClientId { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "DataCadastro")]
        public DateTime DataCadastro { get; set; }

        [DataMember(Name = "Status")]
        public EnumStatusNotificacao Status { get; set; }

        [DataMember(Name = "TipoCategoria")]
        public EnumTipoMensagemNotificacaoDuvidasAcademicas TipoCategoria { get; set; }
    }

    public enum EnumTipoNotificacaoDuvidasAcademicas
    {
        Indefinido = 0,
        RespostaMedGrupo = 1,
        Homologada = 2,
        Replica = 3,
        Resposta = 4
    }

    public enum EnumTipoMensagemNotificacaoDuvidasAcademicas
    {
        Indefinido = 0,
        DuvidaRespondida = 1,
        DuvidaRespondidaAlunos = 2,
        DuvidaRespostaHomologada = 3,
        DuvidaRespostaMedgrupo = 4,
        DuvidaFavoritadaRespondida = 5,
        DuvidaFavoritadaRespostaHomologada = 6,
        DuvidaFavoritadaRespostaMedgrupo = 7,
        RespostaHomologadaMedGrupo = 8,
        NovaReplica = 9,
        InteracaoDuvidaHomologada = 10,
        InteracaoDuvidaRespostaMedGrupo = 11,
        ReplicaDuvida = 12,
        ReplicaResposta = 13

    }
}