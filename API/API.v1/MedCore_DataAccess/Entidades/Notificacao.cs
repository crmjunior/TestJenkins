using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Notificacao", Namespace = "")]
    public class Notificacao
    {
        [DataMember(Name = "IdNotificacao")]
        public int IdNotificacao { get; set; }

		[DataMember(Name = "Texto")]
        public string Texto { get; set; }

        [DataMember(Name = "Titulo")]
        public string Titulo { get; set; }

        [DataMember(Name = "TipoNotificacao")]
        public TipoNotificacao TipoNotificacao { get; set; }

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "Lida")]
        public bool Lida { get; set; }

        [DataMember(Name = "AplicacaoId")]
        public int AplicacaoId { get; set; }

        [DataMember(Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [DataMember(Name = "Data")]
        public string Data { get; set; }

        [DataMember(Name = "Quantidade")]
        public int Quantidade { get; set; }

        [DataMember(Name = "txtInfoAdicional")]
        public string InfoAdicional { get; set; }
        
        public DateTime DataOriginal { get; set; }

        public long DataUnix { get; set; }

        public int RegraPermissaoNotificacaoId { get; set; }

        [DataMember(Name = "StatusEnvio")]
        public EStatusEnvioNotificacao StatusEnvio { get; set; }

        [DataMember(Name = "TipoEnvio", EmitDefaultValue = false)]
        public ETipoEnvioNotificacao TipoEnvio { get; set; }

        [DataMember(Name = "RegrasVisualizacao", EmitDefaultValue = false)]
        public List<Regra> RegrasVisualizacao { get; set; }

        [DataMember(Name = "DuvidaId", EmitDefaultValue = false)]
        public int DuvidaId { get; set; }

        [DataMember(Name = "TipoRespostaId", EmitDefaultValue = false)]
        public int TipoRespostaId { get; set; }

        [DataMember(Name = "Destaque")]
        public bool Destaque { get; set; }

        [DataMember(Name = "Dia")]
        public string Dia { get; set; }

        [DataMember(Name = "TipoNotificacaoId")]
        public int TipoNotificacaoId { get; set; }



    }

    [DataContract(Name = "TipoNotificacao", Namespace = "")]
    public class TipoNotificacao
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "Ordem", EmitDefaultValue = false)]
        public int Ordem { get; set; }

        [DataMember(Name = "Alias", EmitDefaultValue = false)]
        public string Alias { get; set; }
    }

    public enum ETipoEnvioNotificacao
    {
        Nenhum = 0,
        Interna = 1,
        PushExterna = 2,
        Todos = 3
    }

    public enum EStatusEnvioNotificacao
    {
        NaoEnviado = 0,
        Pendente = 1,
        Processando = 2,
        Enviado = 3,
        Erro = 4
    }

    public enum ETipoNotificacao
    {
        Nenhum = 0,
        Informativo = 1,
        InformativoLinkInterno = 2,
        InformativoLinkExterno = 3,
        SimuladoOnline = 4,
        DuvidasAcademicas = 5,
        AvaliacaoAula = 6,
        PrimeiraAula = 7,
        DisparadaPosEvento = 8

    }

    public enum EnumStatusNotificacao
    {
        NaoEnviado = 0,
        Enviado = 1,
        Lida = 2
    }

    public enum ELeituraNotificacaoEvento
    {
        NaoLido = 0,
        Lida = 1
    }
}